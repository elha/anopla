Imports System.IO
Public Class TargetItem
	Public TargetImage As Image
	Public SmallTargetImage As Image

	Public ClickRect As Rectangle
	Public Property Name As String

	Public Sub SetImage(img As Image)
		TargetImage = img
		Dim filter = New AForge.Imaging.Filters.ResizeBilinear(img.Width \ 2, img.Height \ 2)
		SmallTargetImage = Filter.Apply(TargetImage)
	End Sub

	Public Property ClickRectSerialize As String
		Get
			Return ClickRect.X.ToString & "," & ClickRect.Y.ToString & "," & ClickRect.Width.ToString & "," & ClickRect.Height.ToString
		End Get
		Set(value As String)
			If String.IsNullOrEmpty(value) Then Return
			Dim arr = value.Split(","c)
			ClickRect = New Rectangle(Integer.Parse(arr(0)), Integer.Parse(arr(1)), Integer.Parse(arr(2)), Integer.Parse(arr(3)))
		End Set
	End Property

	Public Property TargetImageSerialize() As String
		Get
			Dim ms As New MemoryStream()
			TargetImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
			Return Convert.ToBase64String(ms.ToArray)
		End Get
		Set(ByVal Value As String)
			If String.IsNullOrEmpty(Value) Then Return
			Dim array As Byte() = Convert.FromBase64String(Value)
			Dim o As Bitmap = Image.FromStream(New MemoryStream(array))
			SetImage(o.Clone(New Rectangle(0, 0, o.Width, o.Height), Imaging.PixelFormat.Format24bppRgb))
		End Set
	End Property

End Class
