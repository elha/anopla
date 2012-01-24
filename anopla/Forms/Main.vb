Imports System.Collections

Public Class Main

	'Protected Overrides ReadOnly Property CreateParams() As CreateParams
	'	Get
	'		CreateParams = MyBase.CreateParams
	'		CreateParams.ExStyle = CreateParams.ExStyle And API.WS_EX_NOACTIVATE
	'		Return CreateParams
	'	End Get
	'End Property

	'Protected Overrides Sub WndProc(ByRef m As Message)
	'	If (m.Msg = API.WM_MOUSEACTIVATE) Then
	'		m.Result = API.MA_NOACTIVATE
	'	Else
	'		MyBase.WndProc(m)
	'	End If
	'End Sub

	Dim WithEvents ScreenVid As ScreenVideo
	Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		ScreenVid = New ScreenVideo
		ScreenVid.SetWindow("opera")
		VideoWindow.VideoSource = ScreenVid
		detector = New AForge.Vision.Motion.MotionDetector(motionDetector, motionProcessing)
		motionProcessing.HighlightMotionRegions = True
		While ScreenVid.FramesReceived = 0
			System.Threading.Thread.Sleep(100)
		End While
		Me.VideoWindow.Size = New Size(ScreenVid.VideoSize.Width \ 2, ScreenVid.VideoSize.Height \ 2)
	End Sub

	Dim motionDetector = New AForge.Vision.Motion.SimpleBackgroundModelingDetector
	Dim motionProcessing As New AForge.Vision.Motion.BlobCountingObjectsProcessing
	Dim detector As AForge.Vision.Motion.MotionDetector

	Private Sub ScreenVid_NewFrame(sender As Object, eventArgs As AForge.Video.NewFrameEventArgs) Handles ScreenVid.NewFrame
		If detector.ProcessFrame(eventArgs.Frame) > 0.02 Then
			If motionProcessing.ObjectsCount > 1 Then
				For Each r In motionProcessing.ObjectRectangles

				Next
			End If
		End If
	End Sub

	Private Function GetZones() As Rectangle()
		Dim out As New generic.list(Of Rectangle)
		For Each o As Control In VideoWindow.Controls
			If TypeOf o Is SizeableFrame Then
				out.Add(UIToVid(New Rectangle(o.Location, o.Size)))
			End If
		Next
		Return out.ToArray
	End Function

	Private Sub FrameResize(sender As Object, e As Object)
		detector.MotionZones = GetZones()
		If detector.MotionZones.Length = 0 Then detector.MotionZones = {New Rectangle(New Point(0, 0), ScreenVid.VideoSize)}
	End Sub

	Private Sub VideoWindow_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles VideoWindow.MouseDown
		Select Case e.Button
			Case Windows.Forms.MouseButtons.Right
				Dim o As New SizeableFrame
				o.Size = New Size(100, 100)
				o.Location = New Point(100, 100)
				Me.VideoWindow.Controls.Add(o)
				AddHandler o.LocationChanged, AddressOf FrameResize
				AddHandler o.SizeChanged, AddressOf FrameResize
			Case Windows.Forms.MouseButtons.Left
				ScreenVid.Click(UIToVid(e.Location))
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
End Class