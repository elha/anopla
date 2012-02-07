Imports System.Collections

Public Class Main
	Implements ITraceListener

	Dim Targets As TargetList
	Dim WithEvents ScreenVid As ScreenVideo

	Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Tracer.Tracer = Me
		Tracer.Trace("Main_Load", "Starting ...")

		ScreenVid = New ScreenVideo
		ScreenVid.SetWindow("opera")
		Tracer.Trace("Main_Load", "Video started")

		Targets = New TargetList("..\..\..\targetlist.xml")
		Me.TargetGrid.AutoGenerateColumns = False
		Me.TargetGrid.DataSource = Targets

		VideoBox.Image = ScreenVid.GetFrame

		Tracer.Trace("Main_Load", "Init complete")
	End Sub

	Private Sub ScreenVid_NewFrame(bm As Bitmap) Handles ScreenVid.NewFrame
		If Targets Is Nothing Then Return
		Try
			Tracer.Trace("ScreenVid_NewFrame", "")

			Dim templateMatching = New BoyerMooreTemplateMatching

			For Each zone In GetZones()
				For Each t In Targets
					Dim m = templateMatching.ProcessImage(bm, t.TargetImage, zone)
					If m.Length > 0 Then
						Tracer.Trace("ScreenVid_NewFrame", "Match on " & t.Name)

						ScreenVid.Click(New Point(m(0).X + t.ClickRect.X + (t.ClickRect.Width * (Rnd(0.6) + 0.2)), m(0).Y + t.ClickRect.Y + (t.ClickRect.Width * (Rnd(0.6) + 0.2))))
						System.Threading.Thread.Sleep(500)
						Return
					End If
				Next
			Next
		Catch ex As Exception

		End Try
	End Sub

	Private Function GetZones() As Rectangle()
		Dim out As New Generic.List(Of Rectangle)
		For Each o As Control In VideoBox.Controls
			If TypeOf o Is SizeableFrame Then
				out.Add(UIToVid(New Rectangle(o.Location, o.Size)))
			End If
		Next
		If out.Count = 0 Then out.Add(New Rectangle(New Point(0, 0), ScreenVid.VideoSize))
		Return out.ToArray
	End Function

	Private Sub VideoWindow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles VideoBox.MouseDown
		Select Case e.Button
			Case Windows.Forms.MouseButtons.Right
				Dim o As New SizeableFrame
				o.Size = New Size(100, 100)
				o.Location = New Point(100, 100)
				Me.VideoBox.Controls.Add(o)
				AddHandler o.MouseDown, AddressOf VideoWindow_MouseDown
			Case Windows.Forms.MouseButtons.Left
				Dim p = e.Location
				If TypeOf sender Is SizeableFrame Then
					p = Point.Add(e.Location, CType(sender, SizeableFrame).Location)
				End If
				p = UIToVid(p)

				Tracer.Trace("VideoWindow_MouseDown", "Leftclick " & p.X.ToString & ", " & p.Y.ToString)

				Dim t = ClickTarget.GetClickTarget(VideoBox.Image, p)
				If t Is Nothing OrElse String.IsNullOrEmpty(t.Name) Then Return
				Targets.Add(t)

				Tracer.Trace("VideoWindow_MouseDown", "SaveTarget " & t.ClickRect.X.ToString & ", " & t.ClickRect.Y.ToString & ", " & t.ClickRect.Width & ", " & t.ClickRect.Height)

				ScreenVid.Click(p)
		End Select
	End Sub

	Private Function UIToVid(p As Point) As Point
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoBox.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoBox.Size.Height
		Return New Point(CInt(p.X * dx), CInt(p.Y * dy))
	End Function

	Private Function UIToVid(p As Rectangle) As Rectangle
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoBox.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoBox.Size.Height
		Return New Rectangle(CInt(p.X * dx), CInt(p.Y * dy), CInt(p.Width * dx), CInt(p.Height * dy))
	End Function

	Public Sub AddTrace(time As String, strFunction As String, msg As String) Implements Tracer.ITraceListener.AddTrace
        If Me.TraceBox.InvokeRequired Then
            Me.TraceBox.Invoke(Sub() AddTrace(time, strFunction, msg))
        Else
            Dim item As New ListViewItem
            item.Text = time
            item.SubItems.Add(strFunction)
            item.SubItems.Add(msg)

            TraceBox.Items.Add(item)
        End If
	End Sub

	Private Sub TargetGrid_CellDoubleClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles TargetGrid.CellDoubleClick
		If e.RowIndex < 0 OrElse TargetGrid.Rows.Count - 1 < e.RowIndex OrElse TargetGrid.Rows(e.RowIndex) Is Nothing Then Return
		Dim target = CType(TargetGrid.Rows(e.RowIndex).DataBoundItem, TargetItem)
		Dim patch = ClickTarget.GetClickTarget(target)

		If patch Is Nothing OrElse String.IsNullOrEmpty(patch.Name) Then Return
		Targets.Insert(Targets.IndexOf(target), patch)
		Targets.Remove(target)
	End Sub
End Class