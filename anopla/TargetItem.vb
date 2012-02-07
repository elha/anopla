Public Class TargetItem
	Public Property TargetImage As Bitmap
	Public Property ClickRect As Rectangle
	Public Property Name As String

	Public Function Clone() As TargetItem
		Dim out As New TargetItem
		out.TargetImage = TargetImage.Clone
		out.Name = Name
		out.ClickRect = ClickRect
		Return out
	End Function
End Class
