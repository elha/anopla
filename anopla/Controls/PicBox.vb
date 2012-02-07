Public Class PicBox
	Inherits UserControl

	Public Property Image As Bitmap
		Get
			Return BackgroundImage
		End Get
		Set(value As Bitmap)
			BackgroundImage = value
			Me.PicBox_Resize(Me, Nothing)
		End Set
	End Property

	Private oldSize As Size

	Private Sub PicBox_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
		If Image Is Nothing OrElse Me.Width = 0 OrElse Me.Height = 0 Then Return

		Dim dx = Image.Width / Me.Width
		Dim dy = Image.Height / Me.Height

		If dx < dy Then
			Dim nWidth = Image.Width / dy
			If Me.Width <> nWidth Then Me.Width = nWidth
		Else
			Dim nHeight = Image.Height / dy
			If Me.Height <> nHeight Then Me.Height = nHeight
		End If

		If oldSize <> Size Then
			dx = Size.Width / oldSize.Width
			For Each o As Control In Controls
				o.SetBounds(o.Left * dx, o.Top * dx, o.Width * dx, o.Height * dx)
			Next
		End If

		oldSize = Size
	End Sub

	Public ReadOnly Property Zoomlevel As Double
		Get
			If Image Is Nothing OrElse Me.Width = 0 OrElse Me.Height = 0 Then Return -1

			Dim dx = Me.Width / Image.Width
			Dim dy = Me.Height / Image.Height
			Return Math.Max(dx, dy)
		End Get
	End Property

	Public Sub New()
		BackgroundImageLayout = ImageLayout.Zoom
		'SetStyle(ControlStyles.UserPaint, True)
		'SetStyle(ControlStyles.AllPaintingInWmPaint, True)
		'SetStyle(ControlStyles.DoubleBuffer, True)
	End Sub

	'Protected Overrides Sub OnPaint(e As PaintEventArgs)
	'	'e.Graphics.DrawImage(BackgroundImage, 0, 0, Me.Width, Me.Height)
	'	'MyBase.OnPaint(e)
	'End Sub
End Class
