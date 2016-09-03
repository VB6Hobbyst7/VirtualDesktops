Public Class Form1
    Dim SubMenu() As ToolStripMenuItem
    Public DesktopIndex As Integer
    Dim SetForm As Form
    Dim NewForm As Form

    Friend Sub ShowTisp()
        NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
        NotifyIcon1.BalloonTipText = "Hi~我在这里，请右键单击这里，选择""显示主窗体""。"
        NotifyIcon1.BalloonTipTitle = "程序已经运行"
        NotifyIcon1.ShowBalloonTip(3)
    End Sub

    Private Sub Form1_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '当前窗体被模式窗体锁死后
        '新实例运行调用API仍能激活窗体
        '所以激活时如有模式窗体，则激活模式窗体
        On Error Resume Next
        If Not SetForm Is Nothing Then
            SetForegroundWindow(SetForm.Handle)
        ElseIf Not NewForm Is Nothing Then
            SetForegroundWindow(NewForm.Handle)
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case e.CloseReason
            Case Is = CloseReason.UserClosing, CloseReason.TaskManagerClosing
                e.Cancel = True
                Me.Hide()
                NotifyIcon1.Visible = True
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.BalloonTipText = "如需退出程序请右键单击这里，选择""退出程序""。"
                NotifyIcon1.BalloonTipTitle = "程序还在运行"
                NotifyIcon1.ShowBalloonTip(3)
            Case Else
                CloseDesktop(MyDesktops(DesktopIndex).Handle)
                Application.Exit()
        End Select
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_SYSCOMMAND = &H112
        Const WM_SIZE = &HF020
        If m.Msg = WM_SYSCOMMAND And m.WParam.ToInt32 = WM_SIZE Then
            Me.Hide()
            NotifyIcon1.Visible = True
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipText = "如需退出程序请右键单击这里，选择""退出程序""。"
            NotifyIcon1.BalloonTipTitle = "程序还在运行"
            NotifyIcon1.ShowBalloonTip(3)
            Exit Sub
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '初始化
        NotifyIcon1.Icon = Me.Icon
        NotifyIcon1.Text = MainText
        NotifyIcon1.Visible = True
        Me.TopMost = True
        Me.Text = MainText
        Control.CheckForIllegalCrossThreadCalls = False
        '窗体会被多次加载，不可多次初始化
        Dim Desktop As String
        Desktop = GetDesktopName(GetCurrentThreadId)
        If MyDesktops Is Nothing Then
            ReDim MyDesktops(0)
            '获取当前桌面信息
            RegKeys(0).KeyStr = "Ctrl + D"
            RegKeys(0).KeyInfo = "Default"
            DesktopIndex = 0
            MyDesktops(0).WinForm = Me
            MyDesktops(0).Desktop = Desktop
            MyDesktops(0).Thread = Threading.Thread.CurrentThread
        Else
            For I = 0 To MyDesktops.GetUpperBound(0)
                If MyDesktops(I).Desktop = Desktop Then
                    DesktopIndex = I
                    Exit For
                End If
            Next
        End If
        Call RegkeyHook()
        '枚举当前桌面
        Call RefreshDesktop()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click, MCreateDesktop.Click
        Dim Deskhandle As Integer
        NewForm = New Form2

        If Not Me.Visible Or Me.WindowState = 1 Then
            NewForm.StartPosition = FormStartPosition.CenterScreen
        End If

        NewForm.Tag = DesktopIndex
        If NewForm.ShowDialog(Me) = DialogResult.OK Then
            Deskhandle = CreateNewDesktop(MyDesktops(DesktopIndex).NewDesktop)
            If Deskhandle = 0 Then
                MessageBox.Show("创建桌面失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Call RefreshDesktop()
                'If MessageBox.Show("创建桌面成功，是否立即切换到此桌面", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                SwitchDesktop(Me, MyDesktops(DesktopIndex).NewDesktop, DesktopIndex, Deskhandle)
                'End If
            End If

        End If
        NewForm = Nothing
    End Sub

    Private Sub Button_List(sender As System.Object, e As System.EventArgs) Handles Button2.Click, DeskList.DoubleClick
        SwitchDesktop(Me, DeskList.SelectedItem, DesktopIndex)
    End Sub

    Private Sub LinRefresh_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinRefresh.LinkClicked
        Call RefreshDesktop()
    End Sub

    Private Sub ExitApp_Click(sender As System.Object, e As System.EventArgs) Handles MExitApp.Click
        If MessageBox.Show(Me, "确定要退出程序么？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub ShowForm_Click(sender As System.Object, e As System.EventArgs) Handles MShowForm.Click
        Me.Show()
        BringWindowToTop(Handle.ToInt32)
    End Sub
    Private Sub SubMenu_Click(sender As System.Object, e As System.EventArgs)
        '子菜单切换桌面
        SwitchDesktop(Me, sender.text, DesktopIndex)
    End Sub

    Private Sub RefreshDesktop()
        Dim Desktops As String()
        Dim Menu As ToolStripMenuItem, N As Integer = 0
        Desktops = EnumDesktops
        DeskList.Items.Clear()
        Menu = ContextMenuStrip1.Items(2)
        Menu.DropDownItems.Clear()
        ReDim SubMenu(Desktops.GetUpperBound(0))
        For Each Desktop In Desktops
            DeskList.Items.Add(Desktop)
            SubMenu(N) = New ToolStripMenuItem
            SubMenu(N).Text = Desktop
            AddHandler SubMenu(N).Click, AddressOf SubMenu_Click
            Menu.DropDownItems.Add(SubMenu(N))
            N += 1
        Next
        Dim DesktopName As String = GetDesktopName(GetCurrentThreadId)
        For I = 0 To DeskList.Items.Count - 1
            If DeskList.Items(I).ToString = DesktopName Then
                DeskList.SelectedItem = DeskList.Items(I)
            End If
        Next
    End Sub

    Private Function CreateNewDesktop(DesktopName As String) As Integer
        '创建新桌面 
        Dim Desktophandle As Integer
        Desktophandle = CreateDesktopProc(DesktopName, vbNullString, 0, 0, EnumDESKTOP.GENERIC_ALL, 0)
        Return Desktophandle
    End Function

    Private Sub ShowStatue(ByVal info As String, Font As Font)
        Dim nBrush As New SolidBrush(Color.Black) '创建一个颜色刷子
        Dim bmp As New Bitmap(PictureBox1.Width, PictureBox1.Height) '创建一个位图
        Dim g As Graphics = Graphics.FromImage(bmp) '指定在Bmp绘图
        Dim left As Single, top As Single '用于保存绘图坐标
        Dim sizef As SizeF = g.MeasureString(info, Font) '获取文字高宽
        g.Clear(PictureBox1.BackColor) '清除画布，背景色为图片框的颜色
        top = (PictureBox1.Height - sizef.Height) / 2 '计算文字居中的坐标
        left = (PictureBox1.Width - sizef.Width) / 2 '同上
        g.DrawString(info, Font, nBrush, left, top) '按坐标绘制文字
        PictureBox1.Image = bmp
        g = Nothing '释放内存
        bmp = Nothing '释放内存
    End Sub

    Private Sub DeskList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles DeskList.SelectedIndexChanged
        '查找桌面缩略图
        Dim Desktop As String, Bmp As Bitmap = Nothing
        Desktop = DeskList.SelectedItem
        If GetDesktopName(GetCurrentThreadId) = Desktop Then
            '如果是当前桌面
            Bmp = CopyScreen()
        Else
            For Each MyDesktop In MyDesktops
                If MyDesktop.Desktop = Desktop Then
                    Bmp = MyDesktop.Picture
                End If
            Next
        End If

        If Bmp Is Nothing Then
            PictureBox1.Size = New Size(200, 150)
            PictureBox1.Location = New Point(175, 24)
            ShowStatue("此桌面暂时无法预览", Me.Font)
        Else
            Dim Bitmapsize As Size = GetBitmapSize(Bmp)
            PictureBox1.Size = Bitmapsize
            If Bitmapsize = New Size(86, 150) Then    '16:9竖屏
                PictureBox1.Location = New Point(232, 24)
            ElseIf Bitmapsize = New Size(112, 150) Then     '4:3竖屏
                PictureBox1.Location = New Point(219, 24)
            ElseIf Bitmapsize = New Size(200, 150) Then     '4:3横屏
                PictureBox1.Location = New Point(175, 24)
            ElseIf Bitmapsize = New Size(200, 117) Then     '16:9横屏
                PictureBox1.Location = New Point(175, 40)
            End If
            PictureBox1.Image = Bmp
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        CreateProcess(Environ("WinDir") & "\Explorer.exe", MyDesktops(DesktopIndex).Desktop)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        SetForm = New Form3
        SetForm.ShowDialog()
        SetForm = Nothing
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub
End Class
