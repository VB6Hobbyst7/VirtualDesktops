<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DeskList = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MShowForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.MCreateDesktop = New System.Windows.Forms.ToolStripMenuItem()
        Me.MSwitchDesktop = New System.Windows.Forms.ToolStripMenuItem()
        Me.MExitApp = New System.Windows.Forms.ToolStripMenuItem()
        Me.LinRefresh = New System.Windows.Forms.LinkLabel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 180)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(155, 28)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "创建桌面(&C)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DeskList
        '
        Me.DeskList.FormattingEnabled = True
        Me.DeskList.IntegralHeight = False
        Me.DeskList.ItemHeight = 12
        Me.DeskList.Location = New System.Drawing.Point(12, 24)
        Me.DeskList.Name = "DeskList"
        Me.DeskList.Size = New System.Drawing.Size(155, 150)
        Me.DeskList.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 12)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "系统桌面列表："
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(173, 180)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(155, 28)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "切换桌面(&S)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MShowForm, Me.MCreateDesktop, Me.MSwitchDesktop, Me.MExitApp})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowCheckMargin = True
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 114)
        '
        'MShowForm
        '
        Me.MShowForm.Name = "MShowForm"
        Me.MShowForm.Size = New System.Drawing.Size(152, 22)
        Me.MShowForm.Text = "显示主窗体(&S)"
        '
        'MCreateDesktop
        '
        Me.MCreateDesktop.Name = "MCreateDesktop"
        Me.MCreateDesktop.Size = New System.Drawing.Size(152, 22)
        Me.MCreateDesktop.Text = "创建新桌面(&C)"
        '
        'MSwitchDesktop
        '
        Me.MSwitchDesktop.Name = "MSwitchDesktop"
        Me.MSwitchDesktop.Size = New System.Drawing.Size(152, 22)
        Me.MSwitchDesktop.Text = "切换桌面(&S)"
        '
        'MExitApp
        '
        Me.MExitApp.Name = "MExitApp"
        Me.MExitApp.Size = New System.Drawing.Size(152, 22)
        Me.MExitApp.Text = "退出程序(&E)"
        '
        'LinRefresh
        '
        Me.LinRefresh.AutoSize = True
        Me.LinRefresh.Location = New System.Drawing.Point(138, 9)
        Me.LinRefresh.Name = "LinRefresh"
        Me.LinRefresh.Size = New System.Drawing.Size(29, 12)
        Me.LinRefresh.TabIndex = 1
        Me.LinRefresh.TabStop = True
        Me.LinRefresh.Text = "刷新"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(175, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(200, 150)
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(173, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "桌面预览："
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(298, 9)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(77, 12)
        Me.LinkLabel1.TabIndex = 5
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "启动Explorer"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(334, 180)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(41, 28)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "热键"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(387, 219)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.LinRefresh)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DeskList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DeskList As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MShowForm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MCreateDesktop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MSwitchDesktop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MExitApp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinRefresh As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Button3 As System.Windows.Forms.Button

End Class
