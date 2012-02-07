Imports System.IO

Public Class TargetList
	Inherits System.ComponentModel.BindingList(Of TargetItem)
	Dim cstrFile As String

	Public Sub New(strFile As String)
		cstrFile = strFile
		If System.IO.File.Exists(cstrFile) Then Me.Deserialize(System.IO.File.ReadAllText(cstrFile))
		AddHandler Me.ListChanged, Sub() Me.Serialize()
	End Sub

    Public Function List() As Collections.Generic.List(Of TargetItem)
        Dim out As New Collections.Generic.List(Of TargetItem)
        out.AddRange(Me)
        Return out
    End Function

	Friend Sub Deserialize(ByVal strXML As String)
		Dim oXML As New Xml.XmlDocument
		Try
			oXML.LoadXml(strXML)
		Catch ex As Exception

		End Try

		For Each node In oXML.GetElementsByTagName("target")
			Dim t As New TargetItem
			For Each fld In t.GetType().GetProperties()
				If fld.CanWrite Then
					Dim oDB = node.GetElementsByTagName(fld.Name)
					If oDB.Count = 0 Then Continue For


					fld.SetValue(t, DeSerializeValue(oDB(0).InnerText, fld.PropertyType), Nothing)
				End If
			Next
			Me.Add(t)
		Next
	End Sub

	Friend Sub Serialize()
		Dim oXML As New Xml.XmlDocument
		oXML.LoadXml("<AnoplaTargetList/>")

		For Each target In Me
			Dim oNode = oXML.CreateElement("target")
			For Each fld In target.GetType().GetProperties()
				Dim oDB = oXML.CreateElement(fld.Name)
				oDB.InnerText = SerializeValue(fld.GetValue(target, Nothing))
				oNode.AppendChild(oDB)
			Next
			oXML.DocumentElement.AppendChild(oNode)
		Next
		Dim oSb As New System.Text.StringBuilder
		Dim settings = New System.Xml.XmlWriterSettings
		settings.Indent = True
		settings.IndentChars = "  "
		settings.NewLineChars = vbCrLf

		Dim writer = Xml.XmlTextWriter.Create(oSb, settings)
		oXML.Save(writer)
		writer.Close()

		System.IO.File.WriteAllText(cstrFile, oSb.ToString)
	End Sub

	Private Shared Function SerializeValue(o As Object) As String
		If o Is Nothing Then Return String.Empty
		If TypeOf o Is Rectangle Then
			Dim clickrect = CType(o, Rectangle)
			Return clickrect.X.ToString & "," & clickrect.Y.ToString & "," & clickrect.Width.ToString & "," & clickrect.Height.ToString
		End If
		If TypeOf o Is Bitmap Then
			Dim ms As New MemoryStream()
			CType(o, Bitmap).Save(ms, System.Drawing.Imaging.ImageFormat.Png)
			Return Convert.ToBase64String(ms.ToArray)
		End If

		Return o.ToString
	End Function

	Private Shared Function DeSerializeValue(s As String, ByVal t As Type) As Object
		If t Is GetType(Rectangle) Then
			If String.IsNullOrEmpty(s) Then Return Nothing
			Dim arr = s.Split(","c)
			Return New Rectangle(Integer.Parse(arr(0)), Integer.Parse(arr(1)), Integer.Parse(arr(2)), Integer.Parse(arr(3)))
		End If
		If t Is GetType(Bitmap) Then
			If String.IsNullOrEmpty(s) Then Return Nothing
			Dim array As Byte() = Convert.FromBase64String(s)
			Dim img As Bitmap = Image.FromStream(New MemoryStream(array))
			Return img.Clone(New Rectangle(0, 0, img.Width, img.Height), Imaging.PixelFormat.Format24bppRgb)
		End If
		Return s
	End Function
End Class
