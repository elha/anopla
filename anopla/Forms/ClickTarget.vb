Public Class ClickTarget
    Public Shared Function GetClickTarget(ByVal orig As Bitmap, ByVal click As Point) As TargetItem
        Dim nSize = 100
        Dim nZoom = 4

        'image max 200x200
        Dim img = orig.Clone(Rectangle.Intersect(New Rectangle(click.X - nSize, click.Y - nSize, 2 * nSize, 2 * nSize), New Rectangle(New Point(0, 0), orig.Size)), orig.PixelFormat)

        If click.X > nSize Then click.X = nSize
        If click.Y > nSize Then click.Y = nSize

        Dim o As New ClickTarget
        o.pic.Image = img
        o.pic.SizeMode = PictureBoxSizeMode.Zoom
        o.pic.ClientSize = New Size(img.Width * nZoom, img.Height * nZoom)
        o.ClientSize = o.pic.Size

        Dim clickframe As New ClickPoint
        o.pic.Controls.Add(clickframe)
        clickframe.Size = New Size(10 * nZoom, 10 * nZoom)
        clickframe.CenterLocation = New Point(click.X * nZoom, click.Y * nZoom)
        AddHandler clickframe.DoubleClick2, Sub() o.Close()

        Dim cutframe As New SizeableFrame
        o.pic.Controls.Add(cutframe)
        cutframe.Size = New Size(20 * nZoom, 20 * nZoom)
        cutframe.CenterLocation = clickframe.CenterLocation
        o.ShowDialog()

        ' Copy the source image into the destination bitmap.
        Dim out As New TargetItem
        out.TargetImage = orig.Clone(New Rectangle(cutframe.Location.X \ nZoom, cutframe.Location.Y \ nZoom, cutframe.Size.Width \ nZoom, cutframe.Size.Height \ nZoom), orig.PixelFormat)
        out.ClickRect = New Rectangle((clickframe.Location.X - cutframe.Location.X) \ nZoom, (clickframe.Location.Y - cutframe.Location.Y) \ nZoom, clickframe.Size.Width \ nZoom, clickframe.Size.Height \ nZoom)
        out.Name = InputBox("Name")

        Return out
    End Function

End Class