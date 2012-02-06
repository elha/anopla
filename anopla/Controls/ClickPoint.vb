Option Strict On

Imports System
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ClickPoint
    Inherits System.Windows.Forms.UserControl
    Implements IKnubbel
    Public Event DoubleClick2(ByVal sender As Object)

    Public Property Direction As Integer Implements IKnubbel.Direction

    Const l = AnchorStyles.Left
    Const t = AnchorStyles.Top
    Const r = AnchorStyles.Right
    Const b = AnchorStyles.Bottom

    Dim pressed As IKnubbel = Nothing
    Dim start As Size = Nothing

    Private Sub Knubbel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        pressed = CType(sender, IKnubbel)
        start = New Size(e.Location)
    End Sub

    Private Sub Knubbel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If pressed Is Nothing Then Return

        Dim diff = Point.Subtract(e.Location, start)
        Dim nl = Me.Left + IIf(BitSet(pressed.Direction, l), diff.X, 0)
        Dim nt = Me.Top + IIf(BitSet(pressed.Direction, t), diff.Y, 0)
        Dim nr = Me.Right + IIf(BitSet(pressed.Direction, r), diff.X, 0)
        Dim nb = Me.Bottom + IIf(BitSet(pressed.Direction, b), diff.Y, 0)

        Me.SetBounds(nl, nt, nr - nl, nr - nl)
    End Sub

    Private Sub Knubbel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        pressed = Nothing
    End Sub

    Private Sub Knubbel_MouseDoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        RaiseEvent DoubleClick2(Me)
    End Sub

    Public Sub New()
        MyBase.new()

        Me.SuspendLayout()

        Me.BackColor = Color.FromArgb(50, Color.DarkRed)
        Me.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.Size = New System.Drawing.Size(10, 10)
        Me.Margin = New Padding(0, 0, 0, 0)
        Me.Padding = New Padding(0, 0, 0, 0)
        Me.Direction = l + t + r + b
        Me.DoubleBuffered = True
        Me.Cursor = Cursors.SizeAll

        For Each d As Integer In {l}
            Dim o As New Knubbel
            o.Direction = d
            o.Anchor = CType(d, AnchorStyles)
            o.ForeColor = Me.ForeColor
            o.Size = New Size(8, 8)
            o.Padding = Me.Padding

            Dim nX = IIf(BitSet(d, l), 0, 0.5) + IIf(BitSet(d, r), 0.5, 0)
            Dim nY = IIf(BitSet(d, t), 0, 0.5) + IIf(BitSet(d, b), 0.5, 0)

            o.Location = New Point(CInt((Me.Size.Width - o.Size.Width) * nX), CInt((Me.Size.Height - o.Size.Height) * nY))

            Select Case d
                Case l + t, r + b
                    o.Cursor = Cursors.SizeNWSE
                Case l, r
                    o.Cursor = Cursors.SizeWE
                Case l + b, r + t
                    o.Cursor = Cursors.SizeNESW
                Case t, b
                    o.Cursor = Cursors.SizeNS
                Case Else
                    o.Cursor = Cursors.SizeAll
                    o.Anchor = AnchorStyles.None
            End Select

            Me.Controls.Add(o)

            AddHandler o.MouseDown, AddressOf Knubbel_MouseDown
            AddHandler o.MouseMove, AddressOf Knubbel_MouseMove
            AddHandler o.MouseUp, AddressOf Knubbel_MouseUp
            AddHandler o.DoubleClick, AddressOf Knubbel_MouseDoubleClick
        Next

        Me.ResumeLayout(False)
    End Sub

    Public Property BorderColor() As Color = Color.Black

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim lpPen As Pen = New Pen(BorderColor, 1)
        e.Graphics.DrawRectangle(lpPen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)
        lpPen.Dispose()
    End Sub

    Public Property CenterLocation As Point
        Get
            Return New Point(Me.Left + Me.Width \ 2, Me.Top + Me.Height \ 2)
        End Get
        Set(ByVal value As Point)
            Me.Location = New Point(Math.Max(0, value.X - (Me.Size.Width \ 2)), Math.Max(0, value.Y - (Me.Size.Height \ 2)))
        End Set
    End Property

    Public Interface IKnubbel
        Property Direction As Integer
    End Interface

    Public Class Knubbel
        Inherits UserControl
        Implements IKnubbel

        Public Property Direction As Integer Implements IKnubbel.Direction
    End Class


End Class
