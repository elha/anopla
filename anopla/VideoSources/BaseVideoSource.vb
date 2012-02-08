Public MustInherit Class BaseVideoSource
	Public Event NewFrame(ByVal bm As Bitmap)
	Public Sub RaiseNewFrame(bm As Bitmap)
		RaiseEvent NewFrame(bm)
	End Sub

	Public Shared Function Factory(strInit As String)
		If strInit.ToLower.StartsWith("vnc:") Then
			Return New VNCSource(strInit.Substring(4))
		ElseIf strInit.ToLower.StartsWith("wnd:") Then
			Return New ScreenVideo(strInit.Substring(4))
		End If
		Throw New Exception("VideoSource unknown """ & strInit & """, try vnc:pass@server:port or wnd:firefox")
	End Function

	Public MustOverride Function CaptureFrame() As Bitmap
	Public MustOverride Property CurrentFrame As Bitmap
	Public MustOverride ReadOnly Property VideoSize As Size
	Public MustOverride Sub Click(ByVal p As Point)
End Class
