Imports System.Collections

Public Class Main
	Dim Targets As TargetList
	Dim WithEvents ScreenVid As ScreenVideo
	Dim cstrFile = "targetlist.xml"

	Private Sub Main_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		ScreenVid.Stop()
		System.IO.File.WriteAllText(cstrFile, Targets.Serialize)
	End Sub

	Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		detector = New AForge.Vision.Motion.MotionDetector(motionDetector, motionProcessing)

		ScreenVid = New ScreenVideo
		ScreenVid.SetWindow("opera")

		If System.IO.File.Exists(cstrFile) Then
			Targets = TargetList.Deserialize(System.IO.File.ReadAllText(cstrFile))
		Else
			Targets = New TargetList
		End If

		VideoWindow.VideoSource = ScreenVid
		motionProcessing.HighlightMotionRegions = True
		While ScreenVid.FramesReceived = 0
			System.Threading.Thread.Sleep(100)
		End While
		Me.VideoWindow.Size = New Size(ScreenVid.VideoSize.Width \ 2, ScreenVid.VideoSize.Height \ 2)
	End Sub

	Dim motionDetector = New AForge.Vision.Motion.SimpleBackgroundModelingDetector
	Dim motionProcessing As New AForge.Vision.Motion.BlobCountingObjectsProcessing
	Dim detector As AForge.Vision.Motion.MotionDetector

	Private Sub ScreenVid_NewFrame(sender As Object, eventArgs As AForge.Video.NewFrameEventArgs) Handles ScreenVid.NewFrame
		'If detector.ProcessFrame(eventArgs.Frame) > 0.02 Then
		'	If motionProcessing.ObjectsCount > 1 Then
		'		For Each r In motionProcessing.ObjectRectangles

		'		Next
		'	End If
		'End If
		Try
			Dim filter = New AForge.Imaging.Filters.ResizeBilinear(eventArgs.Frame.Width \ 2, eventArgs.Frame.Height \ 2)
			Dim img = filter.Apply(eventArgs.Frame)
			Dim templateMatching = New AForge.Imaging.ExhaustiveTemplateMatching(0.95)

			For Each zone In GetZones()
				For Each t In Targets
					'If templateMatching.ProcessImage(img, t.SmallTargetImage, New Rectangle(zone.X \ 2, zone.Y \ 2, zone.Width \ 2, zone.Height \ 2)).Length > 0 Then
					Dim m = templateMatching.ProcessImage(eventArgs.Frame, t.TargetImage, zone)
					If m.Length > 0 Then
						ScreenVid.Click(New Point(m(0).Rectangle.X + t.ClickRect.X + (t.ClickRect.Width * Rnd(1)), m(0).Rectangle.Y + t.ClickRect.Y + (t.ClickRect.Width * Rnd(1))))
						System.Threading.Thread.Sleep(500)
						ScreenVid.Start()
						Return
					End If
					'End If
				Next
			Next
		Catch ex As Exception

		End Try

	End Sub

	Private Function GetZones() As Rectangle()
		Dim out As New generic.list(Of Rectangle)
		For Each o As Control In VideoWindow.Controls
			If TypeOf o Is SizeableFrame Then
				out.Add(UIToVid(New Rectangle(o.Location, o.Size)))
			End If
		Next
		Return out.ToArray
	End Function

	Private Sub FrameResize(sender As Object, e As Object)
		detector.MotionZones = GetZones()
		If detector.MotionZones.Length = 0 Then detector.MotionZones = {New Rectangle(New Point(0, 0), ScreenVid.VideoSize)}
	End Sub

    Private Sub VideoWindow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles VideoWindow.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                Dim o As New SizeableFrame
                o.Size = New Size(100, 100)
                o.Location = New Point(100, 100)
                Me.VideoWindow.Controls.Add(o)
                AddHandler o.LocationChanged, AddressOf FrameResize
                AddHandler o.SizeChanged, AddressOf FrameResize
                AddHandler o.MouseDown, AddressOf VideoWindow_MouseDown
            Case Windows.Forms.MouseButtons.Left
                Dim p = e.Location
                If TypeOf sender Is SizeableFrame Then
                    p = Point.Add(e.Location, CType(sender, SizeableFrame).Location)
                End If
                p = UIToVid(p)

				Targets.Add(ClickTarget.GetClickTarget(ScreenVid.LastFrame, p))

                ScreenVid.Click(p)
            Case Windows.Forms.MouseButtons.Middle
                detector.Reset()
        End Select
    End Sub

	Private Function UIToVid(p As Point) As Point
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoWindow.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoWindow.Size.Height
		Return New Point(CInt(p.X * dx), CInt(p.Y * dy))
	End Function

	Private Function UIToVid(p As Rectangle) As Rectangle
		Dim dx As Double = ScreenVid.VideoSize.Width / VideoWindow.Size.Width
		Dim dy As Double = ScreenVid.VideoSize.Height / VideoWindow.Size.Height
		Return New Rectangle(CInt(p.X * dx), CInt(p.Y * dy), CInt(p.Width * dx), CInt(p.Height * dy))
	End Function
End Class