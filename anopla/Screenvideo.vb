Imports System
Imports System.Drawing

Public Class ScreenVideo
	Public Event NewFrame(bm As Bitmap)

	Public Function GetFrame() As Bitmap
		Dim bm As Bitmap = Nothing

		Select Case Me.ScreenshotType
			Case ScreenshotTypes.ActiveWindow
				bm = Screenshot.GetScreenshotCurrentWindow
			Case ScreenshotTypes.PrimaryDesktop
				bm = Screenshot.GetScreenshotPrimaryScreen
			Case ScreenshotTypes.SpecialWindow
				bm = Screenshot.GetScreenshotByHandle(CapturedWindow)
		End Select
		VideoSize = bm.Size
		RaiseEvent NewFrame(bm)
		Return bm
	End Function

	Public Property CapturedWindow As Long

	Public Enum ScreenshotTypes
		ActiveWindow
		PrimaryDesktop
		SpecialWindow
	End Enum

	Public Property ScreenshotType As ScreenshotTypes = ScreenshotTypes.ActiveWindow
	Public Property VideoSize As Size

	Public Sub SetWindow(ByVal strTitle As String)
		CapturedWindow = Screenshot.FindWindowLike(strTitle)

		If CapturedWindow = 0 Then Return

		ScreenshotType = ScreenshotTypes.SpecialWindow
	End Sub

	Public Sub Click(ByVal p As Point)
		Screenshot.MouseClickWindow(CapturedWindow, p.X, p.Y)
	End Sub

End Class