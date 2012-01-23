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

    Dim ScreenVid As ScreenVideo
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ScreenVid = New ScreenVideo
        ScreenVid.SetWindow("opera")
        VideoSourcePlayer1.VideoSource = ScreenVid
    End Sub
End Class