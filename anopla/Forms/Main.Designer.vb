<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.VideoWindow = New AForge.Controls.VideoSourcePlayer()
		Me.SuspendLayout()
		'
		'VideoWindow
		'
		Me.VideoWindow.Dock = System.Windows.Forms.DockStyle.Fill
		Me.VideoWindow.Location = New System.Drawing.Point(0, 0)
		Me.VideoWindow.Name = "VideoWindow"
		Me.VideoWindow.Size = New System.Drawing.Size(284, 262)
		Me.VideoWindow.TabIndex = 0
		Me.VideoWindow.Text = "VideoSourcePlayer1"
		Me.VideoWindow.VideoSource = Nothing
		'
		'Main
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 262)
		Me.Controls.Add(Me.VideoWindow)
		Me.Name = "Main"
		Me.Text = "Main"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents VideoWindow As AForge.Controls.VideoSourcePlayer
End Class
