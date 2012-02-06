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
		Me.SplitMain = New System.Windows.Forms.SplitContainer()
		Me.SplitBottom = New System.Windows.Forms.SplitContainer()
		Me.TraceBox = New System.Windows.Forms.ListView()
		Me.colTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colFunction = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colArgs = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.TargetGrid = New System.Windows.Forms.DataGridView()
		Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.colClickTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.colMatchTarget = New System.Windows.Forms.DataGridViewImageColumn()
		CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitMain.Panel1.SuspendLayout()
		Me.SplitMain.Panel2.SuspendLayout()
		Me.SplitMain.SuspendLayout()
		CType(Me.SplitBottom, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitBottom.Panel1.SuspendLayout()
		Me.SplitBottom.Panel2.SuspendLayout()
		Me.SplitBottom.SuspendLayout()
		CType(Me.TargetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'VideoWindow
		'
		Me.VideoWindow.Dock = System.Windows.Forms.DockStyle.Fill
		Me.VideoWindow.Location = New System.Drawing.Point(0, 0)
		Me.VideoWindow.Name = "VideoWindow"
		Me.VideoWindow.Size = New System.Drawing.Size(880, 302)
		Me.VideoWindow.TabIndex = 0
		Me.VideoWindow.Text = "VideoSourcePlayer1"
		Me.VideoWindow.VideoSource = Nothing
		'
		'SplitMain
		'
		Me.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitMain.Location = New System.Drawing.Point(0, 0)
		Me.SplitMain.Name = "SplitMain"
		Me.SplitMain.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'SplitMain.Panel1
		'
		Me.SplitMain.Panel1.Controls.Add(Me.VideoWindow)
		'
		'SplitMain.Panel2
		'
		Me.SplitMain.Panel2.Controls.Add(Me.SplitBottom)
		Me.SplitMain.Size = New System.Drawing.Size(880, 690)
		Me.SplitMain.SplitterDistance = 302
		Me.SplitMain.TabIndex = 1
		'
		'SplitBottom
		'
		Me.SplitBottom.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitBottom.Location = New System.Drawing.Point(0, 0)
		Me.SplitBottom.Name = "SplitBottom"
		'
		'SplitBottom.Panel1
		'
		Me.SplitBottom.Panel1.Controls.Add(Me.TraceBox)
		'
		'SplitBottom.Panel2
		'
		Me.SplitBottom.Panel2.Controls.Add(Me.TargetGrid)
		Me.SplitBottom.Size = New System.Drawing.Size(880, 384)
		Me.SplitBottom.SplitterDistance = 515
		Me.SplitBottom.TabIndex = 0
		'
		'TraceBox
		'
		Me.TraceBox.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTime, Me.colFunction, Me.colArgs})
		Me.TraceBox.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TraceBox.Location = New System.Drawing.Point(0, 0)
		Me.TraceBox.Name = "TraceBox"
		Me.TraceBox.Size = New System.Drawing.Size(515, 384)
		Me.TraceBox.TabIndex = 0
		Me.TraceBox.UseCompatibleStateImageBehavior = False
		Me.TraceBox.View = System.Windows.Forms.View.Details
		'
		'colTime
		'
		Me.colTime.Text = "Timestamp"
		Me.colTime.Width = 80
		'
		'colFunction
		'
		Me.colFunction.Text = "Function"
		Me.colFunction.Width = 200
		'
		'colArgs
		'
		Me.colArgs.Text = "Args"
		Me.colArgs.Width = 200
		'
		'TargetGrid
		'
		Me.TargetGrid.AllowUserToAddRows = False
		Me.TargetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.TargetGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colClickTarget, Me.colMatchTarget})
		Me.TargetGrid.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TargetGrid.Location = New System.Drawing.Point(0, 0)
		Me.TargetGrid.Name = "TargetGrid"
		Me.TargetGrid.Size = New System.Drawing.Size(361, 384)
		Me.TargetGrid.TabIndex = 0
		'
		'colName
		'
		Me.colName.DataPropertyName = "Name"
		Me.colName.HeaderText = "Name"
		Me.colName.Name = "colName"
		'
		'colClickTarget
		'
		Me.colClickTarget.DataPropertyName = "ClickRect"
		Me.colClickTarget.HeaderText = "ClickRect"
		Me.colClickTarget.Name = "colClickTarget"
		Me.colClickTarget.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
		'
		'colMatchTarget
		'
		Me.colMatchTarget.DataPropertyName = "TargetImage"
		Me.colMatchTarget.HeaderText = "TargetImage"
		Me.colMatchTarget.Name = "colMatchTarget"
		'
		'Main
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(880, 690)
		Me.Controls.Add(Me.SplitMain)
		Me.MinimumSize = New System.Drawing.Size(300, 200)
		Me.Name = "Main"
		Me.Text = "Main"
		Me.SplitMain.Panel1.ResumeLayout(False)
		Me.SplitMain.Panel2.ResumeLayout(False)
		CType(Me.SplitMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitMain.ResumeLayout(False)
		Me.SplitBottom.Panel1.ResumeLayout(False)
		Me.SplitBottom.Panel2.ResumeLayout(False)
		CType(Me.SplitBottom, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitBottom.ResumeLayout(False)
		CType(Me.TargetGrid, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents VideoWindow As AForge.Controls.VideoSourcePlayer
	Friend WithEvents SplitMain As System.Windows.Forms.SplitContainer
	Friend WithEvents SplitBottom As System.Windows.Forms.SplitContainer
	Friend WithEvents TraceBox As System.Windows.Forms.ListView
	Friend WithEvents colTime As System.Windows.Forms.ColumnHeader
	Friend WithEvents colFunction As System.Windows.Forms.ColumnHeader
	Friend WithEvents colArgs As System.Windows.Forms.ColumnHeader
	Friend WithEvents TargetGrid As System.Windows.Forms.DataGridView
	Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
	Friend WithEvents colClickTarget As System.Windows.Forms.DataGridViewTextBoxColumn
	Friend WithEvents colMatchTarget As System.Windows.Forms.DataGridViewImageColumn
End Class
