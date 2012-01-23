Imports System
Imports System.Drawing
Imports System.Threading
Imports AForge.Video

Public Class ScreenVideo
    Implements AForge.Video.IVideoSource

    Private _threadCapture As New System.Threading.Thread(AddressOf ScreenCapture)

    Private Sub ScreenCapture()
        Do
            SyncLock Me
                _framesReceived += 1

                Dim bm As Bitmap = Nothing

                Select Case Me.ScreenshotType
                    Case ScreenshotTypes.ActiveWindow
                        bm = Screenshot.GetScreenshotCurrentWindow
                    Case ScreenshotTypes.PrimaryDesktop
                        bm = Screenshot.GetScreenshotPrimaryScreen
                    Case ScreenshotTypes.SpecialWindow
                        bm = Screenshot.GetScreenshotByHandle(CapturedWindow)
                End Select

                If bm Is Nothing Then
                    ' Don't know why, the but the Bitmap here ends up null in some cases.  If it is null, just get out for now.
                    Exit Sub
                Else
                    VideoSize = bm.Size
                    ' The Bitmap shouldn't be null, raise the NewFrame event then free the resources.
                    RaiseEvent NewFrame(Nothing, New AForge.Video.NewFrameEventArgs(bm))
                    bm.Dispose()
                    bm = Nothing
                End If

                ' This is to keep the memory usage down.  The CLR should handle and free memory when the OS needs it but
                ' a lot of people will complain when this quickly uses upwards of 500-700MB of RAM depending on how fast
                ' screenshots are pulled in.  This will keep the memory usage down but will create some additional overhead.
                If Me.AutoCallGarbageCollect = True Then
                    GC.Collect()
                End If

            End SyncLock
            Threading.Thread.Sleep(Me.Interval)
        Loop
    End Sub

    Public Property AutoCallGarbageCollect As Boolean = True
    Property Interval As Integer = 1000
    Public Property CapturedWindow As Long

    Public Enum ScreenshotTypes
        ActiveWindow
        PrimaryDesktop
        SpecialWindow
    End Enum

    Public Property ScreenshotType As ScreenshotTypes = ScreenshotTypes.ActiveWindow
    Public Property VideoSize As Size

    Public ReadOnly Property IsRunning As Boolean Implements AForge.Video.IVideoSource.IsRunning
        Get
            Return _threadCapture.IsAlive
        End Get
    End Property

    Public Sub SignalToStop() Implements AForge.Video.IVideoSource.SignalToStop
        Me.Stop()
    End Sub

    Public ReadOnly Property Source As String Implements AForge.Video.IVideoSource.Source
        Get
            Return ""
        End Get
    End Property

    Public Sub Start() Implements AForge.Video.IVideoSource.Start
        _threadCapture = New System.Threading.Thread(AddressOf ScreenCapture)
        _threadCapture.Start()
    End Sub

    Public Sub [Stop]() Implements AForge.Video.IVideoSource.Stop
        _threadCapture.Abort()
    End Sub

    Public Sub SetWindow(ByVal strTitle As String)
        [Stop]()
        CapturedWindow = Screenshot.FindWindowLike(strTitle)

        If CapturedWindow = 0 Then Return

        ScreenshotType = ScreenshotTypes.SpecialWindow
        Start()
    End Sub

    Public Event VideoSourceError(ByVal sender As Object, ByVal eventArgs As AForge.Video.VideoSourceErrorEventArgs) Implements AForge.Video.IVideoSource.VideoSourceError

    Public Sub WaitForStop() Implements AForge.Video.IVideoSource.WaitForStop

    End Sub

    Public ReadOnly Property BytesReceived As Long Implements AForge.Video.IVideoSource.BytesReceived
        Get
            Return 0
        End Get
    End Property

    Public Event NewFrame(ByVal sender As Object, ByVal eventArgs As AForge.Video.NewFrameEventArgs) Implements AForge.Video.IVideoSource.NewFrame

    Public Event PlayingFinished(ByVal sender As Object, ByVal reason As AForge.Video.ReasonToFinishPlaying) Implements AForge.Video.IVideoSource.PlayingFinished

    Private _framesReceived As Integer = 0
    Public ReadOnly Property FramesReceived As Integer Implements AForge.Video.IVideoSource.FramesReceived
        Get
            Return _framesReceived
        End Get
    End Property

    Public Sub Click(ByVal p As Point)
        Screenshot.MouseClickWindow(CapturedWindow, p.X, p.Y)
    End Sub
End Class