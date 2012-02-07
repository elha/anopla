Public Class PicBox
	Inherits UserControl
    Private mImage As Bitmap
    Public Property Image As Bitmap
        Get
            Return mImage
        End Get
        Set(ByVal value As Bitmap)
            mImage = value
            Me.Refresh()
        End Set
    End Property

    Dim ctrl As New Collections.Generic.Dictionary(Of SizeableFrame, Rectangle)
    Private Sub PicBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        For Each o As Control In Controls
            If Not TypeOf o Is SizeableFrame Then Continue For
            Dim s = CType(o, SizeableFrame)
            If Not ctrl.ContainsKey(s) Then
                AddHandler s.Resize, Sub() ctrl(s) = Multiply(s.Rect, 1 / Zoomlevel)
                AddHandler s.LocationChanged, Sub() ctrl(s) = Multiply(s.Rect, 1 / Zoomlevel)
                ctrl.Add(s, Multiply(s.Rect, 1 / Zoomlevel)) 'normated size
            End If
            s.Rect = Multiply(ctrl(s), Zoomlevel)
        Next
        Me.Refresh()
    End Sub

	Public ReadOnly Property Zoomlevel As Double
		Get
			If Image Is Nothing OrElse Me.Width = 0 OrElse Me.Height = 0 Then Return -1

			Dim dx = Me.Width / Image.Width
			Dim dy = Me.Height / Image.Height
            Return Math.Min(dx, dy)
		End Get
	End Property

	Public Sub New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
	End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g = e.Graphics
        MyBase.OnPaintBackground(e)
        If Image IsNot Nothing Then g.DrawImage(Image, 0, 0, CInt(Image.Width * Zoomlevel), CInt(Image.Height * Zoomlevel))
    End Sub
End Class
