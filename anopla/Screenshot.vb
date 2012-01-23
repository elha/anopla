Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics


Public Class Screenshot

    ''' <summary>
    ''' The method that is used to take the screenshot.
    ''' </summary>
    ''' <remarks></remarks>
    Enum ScreenshotMethod
        ''' <summary>
        ''' The BitBlt method using the BitBlt Windows API in the gdi32 library file.  Using the Windows API may provide
        ''' benefits but also may break with future OS releases.
        ''' </summary>
        ''' <remarks></remarks>
        BitBlt
        ''' <summary>
        ''' The CopyFromScreen method uses .Net's Graphics class to take a screenshot.  This method uses all managed code
        ''' from the .Net Framework and should be sheletered from changes in the OS.
        ''' </summary>
        ''' <remarks>
        ''' At the writing of this code, the Graphics.CopyFromScreen method had some issues with copying pixels in regards
        ''' to Aero's transparency which is why both the BitBlt and this method are provided.
        ''' </remarks>
        CopyFromScreen
    End Enum

    <DllImport("gdi32")> _
    Public Shared Function BitBlt(ByVal hDestDC As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As IntPtr, ByVal SrcX As Integer, ByVal SrcY As Integer, ByVal Rop As Integer) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetWindowDC(ByVal hwnd As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hDc As IntPtr) As IntPtr
    End Function

    <DllImport("user32")> _
    Private Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Integer
    End Function

    <DllImport("user32")> _
    Private Shared Function GetDesktopWindow() As Long
    End Function
    <DllImport("user32")> _
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As IntPtr
    End Function



    Private Const SRCCOPY As Integer = &HCC0020

    Public Shared Function FindWindowLike(ByVal processname As String) As Long
        For Each proc In Process.GetProcesses
            If proc.ProcessName.ToLower = processname.ToLower Then Return proc.MainWindowHandle
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Rectable structure to pass to the Windows API's
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure RECT
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure

    ''' <summary>
    ''' Takes a screenshot of the primary screen and returns it as a Sysem.Drawing.Bitmap.
    ''' 
    ''' This function uses the BitBlt Windows API to take the screenshot.
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetScreenshotPrimaryScreen() As Bitmap

        Dim g As Graphics
        Dim hdcDest As IntPtr = IntPtr.Zero
        Dim desktopHandleDC As IntPtr = IntPtr.Zero
        Dim desktopHandle As IntPtr = Screenshot.GetDesktopWindow()
        Dim bmp As Bitmap = New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

        bmp = New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        g = Graphics.FromImage(bmp)
        desktopHandleDC = Screenshot.GetWindowDC(desktopHandle)
        hdcDest = g.GetHdc
        BitBlt(hdcDest, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, desktopHandleDC, 0, 0, SRCCOPY)

        g.ReleaseHdc(hdcDest)
        ReleaseDC(desktopHandle, desktopHandleDC)

        g.Dispose() : g = Nothing

        Return bmp

    End Function

    ''' <summary>
    ''' Takes a screenshot of all of the screens available and returns them as a generic System.Drawing.Bitmap list.
    ''' 
    ''' This function uses the BitBlt Windows API to take the screenshot.
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetScreenshotAllScreens() As List(Of Bitmap)
        GetScreenshotAllScreens = New List(Of Bitmap)

        For Each sc As Screen In Screen.AllScreens
            Dim g As Graphics
            Dim hdcDest As IntPtr = IntPtr.Zero
            Dim desktopHandleDC As IntPtr = IntPtr.Zero

            Dim desktopHandle As IntPtr = IntPtr.Zero
            Dim bmp As Bitmap = New Bitmap(sc.Bounds.Width, sc.Bounds.Height)

            bmp = New Bitmap(sc.Bounds.Width, sc.Bounds.Height)
            g = Graphics.FromImage(bmp)
            desktopHandleDC = Screenshot.GetWindowDC(desktopHandle)
            hdcDest = g.GetHdc
            BitBlt(hdcDest, 0, 0, sc.Bounds.Width, sc.Bounds.Height, desktopHandleDC, 0, 0, SRCCOPY)

            g.ReleaseHdc(hdcDest)
            ReleaseDC(desktopHandle, desktopHandleDC)

            g.Dispose() : g = Nothing

            GetScreenshotAllScreens.Add(bmp)

        Next

    End Function

    ''' <summary>
    ''' Takes a screenshot of the current window and return it as a System.Drawing.Bitmap.
    ''' 
    ''' This function uses the BitBlt Windows API to take the screenshot.
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetScreenshotCurrentWindow() As Bitmap

        Dim g As Graphics
        Dim hdcDest As IntPtr = IntPtr.Zero
        Dim windowHandleDC As IntPtr = IntPtr.Zero
        Dim windowHandle As IntPtr = Screenshot.GetForegroundWindow

        Dim windowRect As RECT
        Dim bmp As Bitmap

        If GetWindowRect(windowHandle, windowRect) Then
            bmp = New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top)
        Else
            Return Nothing
        End If

        g = Graphics.FromImage(bmp)
        windowHandleDC = Screenshot.GetWindowDC(windowHandle)
        hdcDest = g.GetHdc
        BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, SRCCOPY)

        g.ReleaseHdc(hdcDest)
        ReleaseDC(windowHandle, windowHandleDC)

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

        Dim windowRect As RECT
        Dim bmp As Bitmap

        If GetWindowRect(handle, windowRect) Then
            bmp = New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top)
        Else
            Return Nothing
        End If

        g = Graphics.FromImage(bmp)
        windowHandleDC = Screenshot.GetWindowDC(handle)
        hdcDest = g.GetHdc
        BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, 0, 0, SRCCOPY)

        g.ReleaseHdc(hdcDest)
        ReleaseDC(handle, windowHandleDC)

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

        Dim windowRect As RECT

        windowRect.Left = rectLoc.Left
        windowRect.Right = rectLoc.Right
        windowRect.Top = rectLoc.Top
        windowRect.Bottom = rectLoc.Bottom

        Dim bmp As New Bitmap(windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top)

        g = Graphics.FromImage(bmp)
        windowHandleDC = Screenshot.GetDesktopWindow
        hdcDest = g.GetHdc
        BitBlt(hdcDest, 0, 0, bmp.Width, bmp.Height, windowHandleDC, windowRect.Left, windowRect.Top, SRCCOPY)

        g.ReleaseHdc(hdcDest)
        ReleaseDC(Screenshot.GetDesktopWindow, windowHandleDC)

        g.Dispose() : g = Nothing

        Return bmp

    End Function

    <DllImport("user32.dll")> _
    Private Shared Sub mouse_event(ByVal dwFlags As UInt32, ByVal dx As UInt32, ByVal dy As UInt32, ByVal dwData As UInt32, ByVal dwExtraInfo As IntPtr)
    End Sub

    Public Const MOUSEEVENTF_LEFTDOWN = &H2
    Public Const MOUSEEVENTF_LEFTUP = &H4
    Public Const MOUSEEVENTF_MIDDLEDOWN = &H20
    Public Const MOUSEEVENTF_MIDDLEUP = &H40
    Public Const MOUSEEVENTF_RIGHTDOWN = &H8
    Public Const MOUSEEVENTF_RIGHTUP = &H10

    Public Shared Sub MouseClickWindow(ByVal hWnd As Long, ByVal x As Integer, ByVal y As Integer)

        Dim oldpos = Cursor.Position

        Dim wndStart As RECT
        GetWindowRect(hWnd, wndStart)
        Cursor.Position = New Point(x + wndStart.Left, y + wndStart.Top)

        SetForegroundWindow(hWnd)
        Threading.Thread.Sleep(200)
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, New System.IntPtr())
        Threading.Thread.Sleep(100)
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, New System.IntPtr())

        Cursor.Position = oldpos
    End Sub
End Class
