Imports System.Collections

Public Class Main
	Implements ITraceListener

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Me.TargetGrid.DataSource = Engine.Targets

        AddHandler Engine.ScreenVid.NewFrame, AddressOf ShowFrames
	End Sub

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Engine.stop()
        RemoveHandler Engine.ScreenVid.NewFrame, AddressOf ShowFrames
    End Sub

    Private Sub ShowFrames(ByVal bm As Bitmap)
        VideoBox.Invoke(Sub() VideoBox.Image = bm.Clone)
    End Sub

    Private Sub SetZones()
        Dim out As New Generic.List(Of Rectangle)
        For Each o As Control In VideoBox.Controls
            If TypeOf o Is SizeableFrame Then
                out.Add(Multiply(New Rectangle(o.Location, o.Size), 1 / VideoBox.Zoomlevel))
            End If
        Next
        If out.Count = 0 Then out.Add(New Rectangle(New Point(0, 0), VideoBox.Image.Size))
        Engine.Zones = out.ToArray
    End Sub

    Private Sub VideoWindow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles VideoBox.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                Dim o As New SizeableFrame
                o.Size = New Size(100, 100)
                o.Location = New Point(100, 100)
                Me.VideoBox.Controls.Add(o)
                AddHandler o.Resize, AddressOf SetZones

            Case Windows.Forms.MouseButtons.Left, Windows.Forms.MouseButtons.Middle
                Dim p = e.Location
                If TypeOf sender Is SizeableFrame Then
                    p = Point.Add(e.Location, CType(sender, SizeableFrame).Location)
                End If
                p = Multiply(p, 1 / VideoBox.Zoomlevel)

                If e.Button = Windows.Forms.MouseButtons.Left Then
                    Dim t = ClickTarget.GetClickTarget(VideoBox.Image, p)
                    If t Is Nothing OrElse String.IsNullOrEmpty(t.Name) Then Return
                    Engine.Targets.Add(t)

                    Tracer.Trace("VideoWindow_MouseDown", "SaveTarget " & t.ClickRect.X.ToString & ", " & t.ClickRect.Y.ToString & ", " & t.ClickRect.Width & ", " & t.ClickRect.Height)
                Else
                    Tracer.Trace("VideoWindow_MouseDown", "Click " & p.X.ToString & ", " & p.Y.ToString)
                End If

                Engine.ScreenVid.Click(p)
        End Select
    End Sub

    Public Sub AddTrace(ByVal time As String, ByVal strFunction As String, ByVal msg As String) Implements Tracer.ITraceListener.AddTrace
        If Me.TraceBox.InvokeRequired Then
            Me.TraceBox.Invoke(Sub() AddTrace(time, strFunction, msg))
        Else
            Dim item As New ListViewItem
            item.Text = time
            item.SubItems.Add(strFunction)
            item.SubItems.Add(msg)

            TraceBox.Items.Add(item)
        End If
    End Sub

    Private Sub TargetGrid_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles TargetGrid.CellDoubleClick
        If e.RowIndex < 0 OrElse TargetGrid.Rows.Count - 1 < e.RowIndex OrElse TargetGrid.Rows(e.RowIndex) Is Nothing Then Return
        Dim target = CType(TargetGrid.Rows(e.RowIndex).DataBoundItem, TargetItem)
        Dim patch = ClickTarget.GetClickTarget(target)

        If patch Is Nothing OrElse String.IsNullOrEmpty(patch.Name) Then Return
        Engine.Targets.Insert(Engine.Targets.IndexOf(target), patch)
        Engine.Targets.Remove(target)
    End Sub
End Class