Option Strict On

Imports System
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ClickPoint
    Inherits System.Windows.Forms.UserControl

	Public Property Direction As Integer

    Const l = AnchorStyles.Left
    Const t = AnchorStyles.Top
    Const r = AnchorStyles.Right
    Const b = AnchorStyles.Bottom

	Dim start As Size = Nothing
	Dim drag As Integer = -1
	Const Border As Integer = 5

	Private Sub Knubbel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
		drag = Direction
		start = New Size(e.Location)
	End Sub

	Private Sub Knubbel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
		If drag = -1 Then
			Direction = 0
			If e.X < Border Then Direction += l
			If e.Y < Border Then Direction += t
			If e.X > (Me.Width - Border) Then Direction += r
			If e.Y > (Me.Height - Border) Then Direction += b
			If Direction = 0 Then Direction = l + t + r + b
			Select Case Direction
				Case l, r
					Cursor = Cursors.SizeWE
				Case l + t, r + b
					Cursor = Cursors.SizeNWSE
				Case t, b
					Cursor = Cursors.SizeNS
				Case t + r, l + b
					Cursor = Cursors.SizeNESW
				Case l + t + r + b
					Cursor = Cursors.SizeAll
			End Select
		Else
			Dim diff = Point.Subtract(e.Location, start)
			Dim nl = Me.Left + IIf(BitSet(drag, l), diff.X, 0)
			Dim nt = Me.Top + IIf(BitSet(drag, t), diff.Y, 0)
			Dim nr = Me.Right + IIf(BitSet(drag, r), diff.X, 0)
			Dim nb = Me.Bottom + IIf(BitSet(drag, b), diff.Y, 0)

			Me.SetBounds(nl, nt, nr - nl, nr - nl)
		End If
	End Sub

	Private Sub Knubbel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
		drag = -1
	End Sub

	Public Sub New()
		MyBase.new()

		Me.SuspendLayout()

		Me.BackColor = Color.FromArgb(50, Color.DarkRed)
		Me.Size = New System.Drawing.Size(10, 10)
		Me.Margin = New Padding(0, 0, 0, 0)
		Me.Padding = New Padding(0, 0, 0, 0)
		Me.DoubleBuffered = True

		Me.ResumeLayout(False)
	End Sub

	Public Property Rect As Rectangle
		Get
			Return New Rectangle(Me.Location, Me.Size)
		End Get
		Set(value As Rectangle)
			Me.Location = value.Location
			Me.Size = value.Size
		End Set
	End Property
End Class
