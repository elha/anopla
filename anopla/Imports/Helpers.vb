Module Helpers
    Public Function Multiply(ByVal p As Size, ByVal m As Double) As Size
        Return New Size(p.Width * m, p.Height * m)
    End Function
    Public Function Multiply(ByVal p As Point, ByVal m As Double) As Point
        Return New Point(p.X * m, p.Y * m)
    End Function
    Public Function Multiply(ByVal p As Rectangle, ByVal m As Double) As Rectangle
        Return New Rectangle(p.Left * m, p.Top * m, p.Width * m, p.Height * m)
    End Function
End Module
