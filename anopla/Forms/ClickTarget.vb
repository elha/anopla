Public Class ClickTarget
    Public Shared Function GetClickTarget(ByVal img As Bitmap, ByVal click As Point)
        Dim o As New ClickTarget
        o.BackgroundImage = img
        o.ClientSize = img.Size
        o.sfClickframe.CenterLocation = click
        o.sfTarget.CenterLocation = click
        o.ShowDialog()

        Dim out As New TargetItem

        ' Copy the source image into the destination bitmap.
        out.TargetImage = img.Clone(New Rectangle(o.sfTarget.Location, o.sfTarget.Size), img.PixelFormat)
        out.ClickRect = New Rectangle(Point.Subtract(o.sfClickframe.Location, o.sfTarget.Location), o.sfClickframe.Size)

        Return out
    End Function

    Private Sub sfClickframe_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles sfClickframe.DoubleClick
        Me.Close()
    End Sub

End Class