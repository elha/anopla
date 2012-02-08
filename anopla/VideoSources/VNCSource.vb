Imports System.Drawing.Imaging
Imports VncSharp

Public Class VNCSource
	Inherits BaseVideoSource

	Public Overrides Property CurrentFrame As Bitmap

	Private mDesktop As Bitmap

	Private WithEvents vnc As VncClient
	Private fullScreenRefresh As Boolean = False
	Private Property state As RuntimeState = RuntimeState.Disconnected

	Private Enum RuntimeState
		Disconnected
		Disconnecting
		Connected
		Connecting
	End Enum

	Public Sub New(connect As String)
		'pwd@host:port
		vnc = New VncClient()
		Dim arrParts = connect.Split({"@"c, ":"c})
		Dim i = IIf(arrParts.Length = 3, 1, 0)
		If vnc.Connect(arrParts(i), 0, arrParts(i + 1), False) Then
			If Not vnc.Authenticate(arrParts(0)) Then Throw New Exception("vnc: not connected, wrong password?")
		End If

		vnc.Initialize()

		state = RuntimeState.Connected

		mDesktop = New Bitmap(vnc.Framebuffer.Width, vnc.Framebuffer.Height, PixelFormat.Format32bppPArgb)

		vnc.StartUpdates()
	End Sub

	Public Overrides Function CaptureFrame() As System.Drawing.Bitmap
		CurrentFrame = mDesktop.Clone
		RaiseNewFrame(CurrentFrame)
		Return CurrentFrame
	End Function

	Public Overrides ReadOnly Property VideoSize As System.Drawing.Size
		Get
			If mDesktop IsNot Nothing Then Return mDesktop.Size
			Return Nothing
		End Get
	End Property

	Public Sub Disconnect()
		vnc.Disconnect()
		state = RuntimeState.Disconnected
	End Sub

	Public Overrides Sub Click(current As Point)
		Dim mask As Byte = 1
		'If Control.MouseButtons = MouseButtons.Left Then mask += 1
		'If Control.MouseButtons = MouseButtons.Middle Then mask += 2
		'If Control.MouseButtons = MouseButtons.Right Then mask += 4
		'scroll forward 				mask += 8
		'scroll backward 			mask += 16

		vnc.WritePointerEvent(0, current)
		System.Threading.Thread.Sleep(200)
		vnc.WritePointerEvent(mask, current)
		System.Threading.Thread.Sleep(200)
		vnc.WritePointerEvent(0, current)
	End Sub

	' This event handler deals with Frambebuffer Updates coming from the host. An
	' EncodedRectangle object is passed via the VncEventArgs (actually an IDesktopUpdater
	' object so that *only* Draw() can be called here--Decode() is done elsewhere).
	' The VncClient object handles thread marshalling onto the UI thread.
	Protected Sub Vnc_Update(sender As Object, e As VncEventArgs) Handles vnc.VncUpdate
		e.DesktopUpdater.Draw(mDesktop)

		If state = RuntimeState.Connected Then
			vnc.RequestScreenUpdate(fullScreenRefresh)

			' Make sure the next screen update is incremental
			fullScreenRefresh = False
		End If
	End Sub


End Class
