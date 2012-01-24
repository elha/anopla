<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClickTarget
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
        Me.sfClickframe = New anopla.SizeableFrame()
        Me.sfTarget = New anopla.SizeableFrame()
        Me.SuspendLayout()
        '
        'sfClickframe
        '
        Me.sfClickframe.BackColor = System.Drawing.Color.Transparent
        Me.sfClickframe.BorderColor = System.Drawing.Color.Orange
        Me.sfClickframe.Direction = 15
        Me.sfClickframe.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.sfClickframe.Location = New System.Drawing.Point(379, 284)
        Me.sfClickframe.Margin = New System.Windows.Forms.Padding(0)
        Me.sfClickframe.Name = "sfClickframe"
        Me.sfClickframe.Size = New System.Drawing.Size(25, 25)
        Me.sfClickframe.TabIndex = 1
        '
        'sfTarget
        '
        Me.sfTarget.BackColor = System.Drawing.Color.Transparent
        Me.sfTarget.BorderColor = System.Drawing.Color.Black
        Me.sfTarget.Direction = 15
        Me.sfTarget.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.sfTarget.Location = New System.Drawing.Point(227, 177)
        Me.sfTarget.Margin = New System.Windows.Forms.Padding(0)
        Me.sfTarget.Name = "sfTarget"
        Me.sfTarget.Size = New System.Drawing.Size(325, 244)
        Me.sfTarget.TabIndex = 2
        '
        'ClickTarget
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 597)
        Me.Controls.Add(Me.sfClickframe)
        Me.Controls.Add(Me.sfTarget)
        Me.Name = "ClickTarget"
        Me.Text = "ClickTarget"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sfClickframe As anopla.SizeableFrame
    Friend WithEvents sfTarget As anopla.SizeableFrame
End Class
