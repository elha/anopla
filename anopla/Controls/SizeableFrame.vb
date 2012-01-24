Option Strict On
Public Class SizeableFrame
	Inherits System.Windows.Forms.UserControl
	Implements IKnubbel

	Public Property Direction As Integer Implements IKnubbel.Direction

	Const l = AnchorStyles.Left
	Const t = AnchorStyles.Top
	Const r = AnchorStyles.Right
	Const b = AnchorStyles.Bottom

	Dim pressed As IKnubbel = Nothing
	Dim start As Size = Nothing

	Private Sub Knubbel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
		pressed = CType(sender, IKnubbel)
		start = New Size(e.Location)
	End Sub

	Private Sub Knubbel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
		If pressed Is Nothing Then Return

		Dim diff = Point.Subtract(e.Location, start)
		Dim nl = Me.Left + IIf(BS(pressed.Direction, l), diff.X, 0)
		Dim nt = Me.Top + IIf(BS(pressed.Direction, t), diff.Y, 0)
		Dim nr = Me.Right + IIf(BS(pressed.Direction, r), diff.X, 0)
		Dim nb = Me.Bottom + IIf(BS(pressed.Direction, b), diff.Y, 0)

		Me.SetBounds(nl, nt, nr - nl, nb - nt)
	End Sub

	Private Sub Knubbel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
		pressed = Nothing
	End Sub

	Private Sub SizeableFrame_MouseHover(sender As Object, e As System.EventArgs) Handles Me.MouseHover
		BackColor = Color.WhiteSmoke
	End Sub

	Private Sub SizeableFrame_MouseLeave(sender As Object, e As System.EventArgs) Handles Me.MouseLeave
		BackColor = Color.Transparent
	End Sub

	Public Sub New()
		MyBase.new()

		Me.SuspendLayout()

		Me.BackColor = System.Drawing.Color.Transparent
		Me.ForeColor = System.Drawing.SystemColors.ButtonShadow
		Me.Cursor = System.Windows.Forms.Cursors.SizeAll
		Me.Size = New System.Drawing.Size(100, 100)
		Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
		Me.Margin = New Padding(0, 0, 0, 0)
		Me.Padding = New Padding(0, 0, 0, 0)
		Me.Direction = l + t + r + b

		For Each d As Integer In {l, l + t, t, t + r, r, r + b, b, l + b}
			Dim o As New Knubbel
			o.Direction = d
			o.Anchor = CType(d, AnchorStyles)

			Dim nX = IIf(BS(d, l), 0, 0.5) + IIf(BS(d, r), 0.5, 0)
			Dim nY = IIf(BS(d, t), 0, 0.5) + IIf(BS(d, b), 0.5, 0)

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
			End Select

			Me.Controls.Add(o)

			AddHandler o.MouseDown, AddressOf Knubbel_MouseDown
			AddHandler o.MouseMove, AddressOf Knubbel_MouseMove
			AddHandler o.MouseUp, AddressOf Knubbel_MouseUp
		Next

		Me.ResumeLayout(False)
	End Sub

	Private Function BS(value As Integer, bit As Integer) As Boolean
		Return CBool(value And bit)
	End Function

	Private Function IIf(Of T)(cond As Boolean, tv As T, fv As T) As T
		If cond Then Return tv
		Return fv
	End Function

	Public Interface IKnubbel
		Property Direction As Integer
	End Interface

	Public Class Knubbel
		Inherits UserControl
		Implements IKnubbel

		Public Sub New()
			Size = New Size(8, 8)
			Padding = New Padding(0, 0, 0, 0)
			BorderStyle = Windows.Forms.BorderStyle.FixedSingle
			Me.ForeColor = Color.Beige
		End Sub

		Public Property Direction As Integer Implements IKnubbel.Direction
	End Class

	Private Sub InitializeComponent()
		Me.SuspendLayout()
		'
		'SizeableFrame
		'
		Me.BackColor = System.Drawing.Color.Transparent
		Me.Name = "SizeableFrame"
		Me.ResumeLayout(False)

	End Sub
End Class
