Public Class TargetList
	Inherits System.ComponentModel.BindingList(Of TargetItem)
	Dim cstrFile As String

	Public Sub New(strFile As String)
		cstrFile = strFile
		If System.IO.File.Exists(cstrFile) Then Me.Deserialize(System.IO.File.ReadAllText(cstrFile))
		AddHandler Me.ListChanged, Sub() Me.Serialize()
	End Sub

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
					fld.SetValue(t, oDB(0).InnerText, Nothing)
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
				oDB.InnerText = fld.GetValue(target, Nothing)
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

End Class
