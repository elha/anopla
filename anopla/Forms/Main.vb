Imports System.Collections

Public Class Main
	Implements ITraceListener

	Dim Targets As TargetList
	Dim WithEvents ScreenVid As ScreenVideo

	Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		ScreenVid.Stop()
	End Sub

	Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Tracer.Tracer = Me
		Tracer.Trace("Main_Load", "Starting ...")
		detector = New AForge.Vision.Motion.MotionDetector(motionDetector, motionProcessing)

		ScreenVid = New ScreenVideo
		ScreenVid.SetWindow("opera")
		Tracer.Trace("Main_Load", "Video started")

		Targets = New TargetList("..\..\..\targetlist.xml")
		Me.TargetGrid.AutoGenerateColumns = False
		Me.TargetGrid.DataSource = Targets

		VideoWindow.VideoSource = ScreenVid
		motionProcessing.HighlightMotionRegions = True
		While ScreenVid.FramesReceived = 0
			System.Threading.Thread.Sleep(100)
		End While
		Me.VideoWindow.Size = New Size(ScreenVid.VideoSize.Width \ 2, ScreenVid.VideoSize.Height \ 2)

		Tracer.Trace("Main_Load", "Init complete")
	End Sub

	Dim motionDetector = New AForge.Vision.Motion.SimpleBackgroundModelingDetector
	Dim motionProcessing As New AForge.Vision.Motion.BlobCountingObjectsProcessing
	Dim detector As AForge.Vision.Motion.MotionDetector

	Private Sub ScreenVid_NewFrame(sender As Object, eventArgs As AForge.Video.NewFrameEventArgs) Handles ScreenVid.NewFrame
		'If detector.ProcessFrame(eventArgs.Frame) > 0.02 Then
		'	If motionProcessing.ObjectsCount > 1 Then
		'		For Each r In motionProcessing.ObjectRectangles

		'		Next
		'	End If
		'End If
		Try
			Tracer.Trace("ScreenVid_NewFrame", "")

			Dim filter = New AForge.Imaging.Filters.ResizeBilinear(eventArgs.Frame.Width \ 2, eventArgs.Frame.Height \ 2)
			Dim img = filter.Apply(eventArgs.Frame)
			Dim templateMatching = New BoyerMooreTemplateMatching

			For Each zone In GetZones()
				For Each t In Targets
					Dim m = templateMatching.ProcessImage(eventArgs.Frame, t.TargetImage, zone)
					If m.Length > 0 Then
						Tracer.Trace("ScreenVid_NewFrame", "Match on " & t.Name)

						ScreenVid.Click(New Point(m(0).Rectangle.X + t.ClickRect.X + (t.ClickRect.Width * (Rnd(0.6) + 0.2)), m(0).Rectangle.Y + t.ClickRect.Y + (t.ClickRect.Width * (Rnd(0.6) + 0.2))))
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
		For Each o As Control In VideoWindow.Controls
			If TypeOf o Is SizeableFrame Then
				out.Add(UIToVid(New Rectangle(o.Location, o.Size)))
			End If
		Next
		If out.Count = 0 Then out.Add(New Rectangle(New Point(0, 0), ScreenVid.VideoSize))
		Return out.ToArray
	End Function

	Private Sub FrameResize(sender As Object, e As Object)
		detector.MotionZones = GetZones()
		If detector.MotionZones.Length = 0 Then detector.MotionZones = {New Rectangle(New Point(0, 0), ScreenVid.VideoSize)}
	End Sub

	Private Sub VideoWindow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles VideoWindow.MouseDown
		Select Case e.Button
			Case Windows.Forms.MouseButtons.Right
				Dim o As New SizeableFrame
				o.Size = New Size(100, 100)
				o.Location = New Point(100, 100)
				Me.VideoWindow.Controls.Add(o)
				AddHandler o.LocationChanged, AddressOf FrameResize
				AddHandler o.SizeChanged, AddressOf FrameResize
				AddHandler o.MouseDown, AddressOf VideoWindow_MouseDown
			Case Windows.Forms.MouseButtons.Left
				Dim p = e.Location
				If TypeOf sender Is SizeableFrame Then
					p = Point.Add(e.Location, CType(sender, SizeableFrame).Location)
				End If
				p = UIToVid(p)

				Tracer.Trace("VideoWindow_MouseDown", "Leftclick " & p.X.ToString & ", " & p.Y.ToString)

				Dim t = ClickTarget.GetClickTarget(ScreenVid.LastFrame, p)
				If t Is Nothing OrElse String.IsNullOrEmpty(t.Name) Then Return
				Targets.Add(t)

				Tracer.Trace("VideoWindow_MouseDown", "SaveTarget " & t.ClickRect.X.ToString & ", " & t.ClickRect.Y.ToString & ", " & t.ClickRect.Width & ", " & t.ClickRect.Height)

				ScreenVid.Click(p)
			Case Windows.Forms.MouseButtons.Middle
				detector.Reset()
		End Select
	End Sub

	Private Function UIToVid(p As Point) As Point
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoWindow.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoWindow.Size.Height
		Return New Point(CInt(p.X * dx), CInt(p.Y * dy))
	End Function

	Private Function UIToVid(p As Rectangle) As Rectangle
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoWindow.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoWindow.Size.Height
		Return New Rectangle(CInt(p.X * dx), CInt(p.Y * dy), CInt(p.Width * dx), CInt(p.Height * dy))
	End Function

	Public Sub AddTrace(time As String, strFunction As String, msg As String) Implements Tracer.ITraceListener.AddTrace
		If Me.TraceBox.InvokeRequired Then
		Else
			Dim item As New ListViewItem
			item.Text = time
			item.SubItems.Add(strFunction)
			item.SubItems.Add(msg)

			TraceBox.Items.Add(item)
		End If
	End Sub
End Class