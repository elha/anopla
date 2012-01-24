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
End Class