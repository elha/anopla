Namespace Anopla
	Public Class Engine
		Public Targets As TargetList
		Public Zones() As Rectangle
		Public ScreenVid As BaseVideoSource
		Private Runner As New Threading.Thread(AddressOf ProcessLoop)

		Public Sub Start()
			Runner.Start()
		End Sub

		Public Sub [Stop]()
			Runner.Abort()
		End Sub

		Private Sub ProcessLoop()
			System.Threading.Thread.Sleep(3000)	'initial wait 3 seconds

			Dim d = Now.Ticks \ 10000
			While True
				Dim t = Now.Ticks \ 10000
				If t - d < 1000 Then System.Threading.Thread.Sleep(1000 - ((t - d) \ 10))
				d = t
				ProcessFrame(ScreenVid.CaptureFrame)
			End While
		End Sub

		Public Sub ProcessFrame(ByVal img As Bitmap)
			If Targets Is Nothing Then Return
			Try
				Dim templateMatching = New BoyerMooreTemplateMatching

				If Zones Is Nothing OrElse Zones.Length = 0 Then Zones = {New Rectangle(New Point(0, 0), img.Size)}

				For Each zone In Zones.Clone
					For Each t In Targets.List
						Dim m = templateMatching.ProcessImage(img, t.TargetImage, zone)
						If m.Length > 0 Then
							Tracer.Trace("Engine", "Match on " & t.Name)

							ScreenVid.Click(New Point(m(0).X + t.ClickRect.X + (t.ClickRect.Width * (Rnd(0.6) + 0.2)), m(0).Y + t.ClickRect.Y + (t.ClickRect.Width * (Rnd(0.6) + 0.2))))
							Return
						End If
					Next
				Next
			Catch ex As Exception

			End Try
		End Sub

		Public Enum Actions
			Click
		End Enum
	End Class
End Namespace
