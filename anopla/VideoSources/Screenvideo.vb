Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Drawing.Imaging

Public Class ScreenVideo
	Inherits BaseVideoSource

	Public Sub New(ByVal strtitle As String)
		CapturedWindow = FindWindowLike(strtitle)
		If CapturedWindow = 0 Then Throw New Exception("Window """ & strtitle & """ not found!")
	End Sub

	Private Property CapturedWindow As Long

	Public Overrides Function CaptureFrame() As Bitmap
		GetScreenshotByHandle(CapturedWindow, CurrentFrame)
		RaiseNewFrame(CurrentFrame)
		Return CurrentFrame
	End Function

	Public Overrides Property CurrentFrame As Bitmap

	Public Overrides ReadOnly Property VideoSize As Size
		Get
			If CurrentFrame IsNot Nothing Then Return CurrentFrame.Size
			Return Nothing
		End Get
	End Property

	Public Overrides Sub Click(ByVal p As Point)
		Dim oldpos = Cursor.Position

		Dim wndStart As API.RECT
		API.GetWindowRect(CapturedWindow, wndStart)
		Cursor.Position = New Point(p.X + wndStart.Left, p.Y + wndStart.Top)

		API.SetForegroundWindow(CapturedWindow)
		Threading.Thread.Sleep(200)
		API.mouse_event(API.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, New System.IntPtr())
		Threading.Thread.Sleep(100)
		API.mouse_event(API.MOUSEEVENTF_LEFTUP, 0, 0, 0, New System.IntPtr())

	End Sub

	'Helperfunctions
	Public Shared Function FindWindowLike(ByVal processname As String) As Long
		For Each proc In Process.GetProcesses
			If proc.ProcessName.ToLower = processname.ToLower Then Return proc.MainWindowHandle
		Next
		Return 0
	End Function

	Public Shared Sub GetScreenshotByHandle(ByVal handle As IntPtr, ByRef bmp As Bitmap)
		Dim windowRect As API.RECT
		If API.GetWindowRect(handle, windowRect) Then
			Dim size = New Size(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top)
			If bmp Is Nothing OrElse bmp.Size <> size Then bmp = New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, PixelFormat.Format24bppRgb)
		Else
			bmp = Nothing
		End If

		Dim g = Graphics.FromImage(bmp)
		Dim windowHandleDC = API.GetWindowDC(handle)
		Dim hdcDest = g.GetHdc
		API.BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, API.SRCCOPY)

		g.ReleaseHdc(hdcDest)
		API.ReleaseDC(handle, windowHandleDC)

		g.Dispose() : g = Nothing
	End Sub

End Class