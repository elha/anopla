Public Class ClickTarget

	Public Shared Function GetClickTarget(ByVal orig As Bitmap, ByVal click As Point) As TargetItem
		Const nSize = 200

		Dim img = orig.Clone(Rectangle.Intersect(New Rectangle(click.X - nSize, click.Y - nSize, 2 * nSize, 2 * nSize), New Rectangle(New Point(0, 0), orig.Size)), orig.PixelFormat)

		If click.X > nSize Then click.X = nSize
		If click.Y > nSize Then click.Y = nSize

		Dim out As New TargetItem
		out.TargetImage = img
        out.ClickRect = New Rectangle(click.X - 10, click.Y - 10, 20, 20)
		out.Name = "Action " & Now.ToString("HH:mm:ss")

		Dim rect As New Rectangle(img.Size.Width * 0.2, img.Size.Height * 0.2, img.Size.Width * 0.6, img.Size.Height * 0.6)

		Return GetClickTarget(out, Rect)
	End Function

	Public Shared Function GetClickTarget(out As TargetItem) As TargetItem
		Return GetClickTarget(out.Clone, New Rectangle(0, 0, out.TargetImage.Width, out.TargetImage.Height))
	End Function

	Private Shared Function GetClickTarget(out As TargetItem, SelectionRect As Rectangle) As TargetItem
        Dim o As New ClickTarget
        o.txtName.Text = out.Name
		o.pic.Image = out.TargetImage

        Dim clickframe As New SizeableFrame
		o.pic.Controls.Add(clickframe)
		clickframe.Size = Multiply(out.ClickRect.Size, o.pic.Zoomlevel)
        clickframe.Location = Multiply(out.ClickRect.Location, o.pic.Zoomlevel)
        clickframe.BackColor = Color.FromArgb(160, Color.DarkGreen)
		AddHandler clickframe.DoubleClick, Sub() o.Close()

		Dim cutframe As New SizeableFrame
		o.pic.Controls.Add(cutframe)
		cutframe.Size = Multiply(SelectionRect.Size, o.pic.Zoomlevel)
		cutframe.Location = Multiply(SelectionRect.Location, o.pic.Zoomlevel)

        Dim oResult = o.ShowDialog()
        If oResult = Windows.Forms.DialogResult.OK Then
            Dim nZoom = 1 / o.pic.Zoomlevel

            out.TargetImage = out.TargetImage.Clone(Rectangle.Intersect(Multiply(cutframe.Rect, nZoom), New Rectangle(New Point(0, 0), out.TargetImage.Size)), out.TargetImage.PixelFormat)
            Dim rect = clickframe.Rect
            rect.Offset(-cutframe.Location.X, -cutframe.Location.Y)

            out.ClickRect = Multiply(rect, nZoom)

            out.Name = o.txtName.Text
        End If
        Return out
    End Function

End Class