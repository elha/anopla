Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics


Public Class Screenshot

    Public Shared Function FindWindowLike(ByVal processname As String) As Long
        For Each proc In Process.GetProcesses
            If proc.ProcessName.ToLower = processname.ToLower Then Return proc.MainWindowHandle
        Next
        Return 0
    End Function

    Public Shared Function GetScreenshotPrimaryScreen() As Bitmap

        Dim g As Graphics
        Dim hdcDest As IntPtr = IntPtr.Zero
        Dim desktopHandleDC As IntPtr = IntPtr.Zero
        Dim desktopHandle As IntPtr = API.GetDesktopWindow()
        Dim bmp As Bitmap = New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

        bmp = New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        g = Graphics.FromImage(bmp)
        desktopHandleDC = API.GetWindowDC(desktopHandle)
        hdcDest = g.GetHdc
        API.BitBlt(hdcDest, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, desktopHandleDC, 0, 0, API.SRCCOPY)

        g.ReleaseHdc(hdcDest)
        API.ReleaseDC(desktopHandle, desktopHandleDC)

        g.Dispose() : g = Nothing

        Return bmp

    End Function


    ''' <summary>
    ''' Takes a screenshot associated with the given handle (be it a window or control) and return it
    ''' as a System.Drawing.Bitmap.
    ''' 
    ''' This function uses the BitBlt Windows API to take the screenshot.
    ''' 
    ''' </summary>
    ''' <param name="handle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetScreenshotByHandle(ByVal handle As IntPtr) As Bitmap

        Dim g As Graphics
        Dim hdcDest As IntPtr = IntPtr.Zero
        Dim windowHandleDC As IntPtr = IntPtr.Zero

        Dim windowRect As API.RECT
        Dim bmp As Bitmap

        If API.GetWindowRect(handle, windowRect) Then
            bmp = New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, PixelFormat.Format24bppRgb)
        Else
            Return Nothing
        End If

        g = Graphics.FromImage(bmp)
        windowHandleDC = API.GetWindowDC(handle)
        hdcDest = g.GetHdc
        API.BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, API.SRCCOPY)

        g.ReleaseHdc(hdcDest)
        API.ReleaseDC(handle, windowHandleDC)

        g.Dispose() : g = Nothing

        Return bmp

    End Function

    ''' <summary>
    ''' Takes a screenshot of the specified location and returns it as a System.Drawing.Bitmap.
    ''' 
    ''' This function uses the BitBlt Windows API to take the screenshot.
    ''' 
    ''' </summary>
    ''' <param name="rectLoc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetScreenshotByLocation(ByVal rectLoc As Rectangle) As Bitmap

        Dim g As Graphics
        Dim hdcDest As IntPtr = IntPtr.Zero
        Dim windowHandleDC As IntPtr = IntPtr.Zero

        Dim windowRect As API.RECT

        windowRect.Left = rectLoc.Left
        windowRect.Right = rectLoc.Right
        windowRect.Top = rectLoc.Top
        windowRect.Bottom = rectLoc.Bottom

        Dim bmp As New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top)

        g = Graphics.FromImage(bmp)
        windowHandleDC = API.GetDesktopWindow
        hdcDest = g.GetHdc
        API.BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, windowRect.Left, windowRect.Top, API.SRCCOPY)

        g.ReleaseHdc(hdcDest)
        API.ReleaseDC(API.GetDesktopWindow, windowHandleDC)

        g.Dispose() : g = Nothing

        Return bmp

    End Function


    Public Shared Sub MouseClickWindow(ByVal hWnd As Long, ByVal x As Integer, ByVal y As Integer)

        Dim oldpos = Cursor.Position

        Dim wndStart As API.RECT
        API.GetWindowRect(hWnd, wndStart)
        Cursor.Position = New Point(x + wndStart.Left, y + wndStart.Top)

        API.SetForegroundWindow(hWnd)
        Threading.Thread.Sleep(200)
        API.mouse_event(API.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, New System.IntPtr())
        Threading.Thread.Sleep(100)
        API.mouse_event(API.MOUSEEVENTF_LEFTUP, 0, 0, 0, New System.IntPtr())

        'Cursor.Position = oldpos
    End Sub
End Class
