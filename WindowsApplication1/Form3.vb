Public Module Setting
    Public Structure RegKey
        Dim KeyStr As String
        Dim KeyInfo As String
    End Structure
    Public RegKeys(19) As RegKey '= Nothing
End Module
Public Class Form3
    Private HookAdd As New MyDelegate(AddressOf MyKBHook) 'VB.Net委托变量

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = 6 And m.WParam = 0 Then
            SetHook(True)
        ElseIf m.Msg = 6 And m.WParam.ToInt32 > 0 Then
            If Me.ActiveControl Is TextBox1 Then
                SetHook()
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub Form3_Load() Handles MyBase.Load
        Call ShowRegKey()
        Me.TopMost = True
        LoadDesktopList()
    End Sub
    Private Sub LinkLabel1_LinkClicked() Handles LinkLabel1.LinkClicked
        LoadDesktopList()
    End Sub
    Private Sub TextBox1_GotFocus() Handles TextBox1.GotFocus
        SetHook()
    End Sub

    Private Sub TextBox1_LotFocus() Handles TextBox1.LostFocus, MyBase.FormClosed
        SetHook(True)
    End Sub

    Sub ShowRegKey()
        With ListView1.Items
            .Clear()
            If RegKeys Is Nothing Then ReDim RegKeys(-1)
            For i = 0 To RegKeys.GetUpperBound(0)
                .Add(CStr(i + 1))
                .Item(.Count - 1).SubItems.Add(RegKeys(i).KeyStr)
                .Item(.Count - 1).SubItems.Add(RegKeys(i).KeyInfo)
            Next
        End With
    End Sub

    Private Sub LoadDesktopList()
        Dim Desktops As String()
        Desktops = EnumDesktops
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("")
        For Each Desktop In Desktops
            ComboBox1.Items.Add(Desktop)
        Next
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Function MyKBHook(ByVal ncode As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
        Dim MyMsg As EVENTMSG, IsDown As Boolean
        Try
            If ncode = 0 Then
                RtlMoveMemory(MyMsg, lParam, Len(MyMsg))
                If wParam = &H104 Or wParam = &H100 Then
                    IsDown = True
                ElseIf wParam = &H105 Or wParam = &H101 Then
                    IsDown = False
                End If
                Select Case MyMsg.VKey
                    Case Is = 160 : LShift = IsDown
                    Case Is = 161 : RShift = IsDown
                    Case Is = 164 : LAlt = IsDown
                    Case Is = 165 : RAlt = IsDown
                    Case Is = 162 : LCtrl = IsDown
                    Case Is = 163 : RCtrl = IsDown
                    Case Is = 91 : LWin = IsDown
                    Case Is = 92 : RWin = IsDown
                    Case Else
                        If IsDown Then
                            Dim MyKey As Keys = MyMsg.VKey
                            If KeyEx <> "" Then
                                Me.TextBox1.Text = KeyEx & MyKey.ToString
                                Me.TextBox1.SelectionStart = Len(KeyEx & MyKey.ToString)
                            End If
                        End If
                End Select
                KeyEx = (IIf(LCtrl Or RCtrl, "Ctrl + ", "") & IIf(LShift Or RShift, "Shift + ", "") &
                            IIf(LAlt Or RAlt, "Alt + ", "") & IIf(LWin Or RWin, "Win + ", ""))
            End If
        Catch ex As Exception

        End Try
        '吞掉按键消息
        Return 1
    End Function

    Private Sub SetHook(Optional ByVal UnHook As Boolean = False)
        Static HookID As Int32 '钩子的句柄
        If UnHook Then
            UnhookWindowsHookEx(HookID)
            HookID = 0
        Else
            If HookID <> 0 Then Exit Sub
            HookID = SetWindowsHookExA(WH_KEYBOARD_LL, HookAdd, GetModuleHandleA(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        End If
    End Sub



    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            TextBox1.Text = RegKeys(ListView1.SelectedItems(0).Index).KeyStr
            ComboBox1.Text = RegKeys(ListView1.SelectedItems(0).Index).KeyInfo
            TextBox1.Enabled = True
            ComboBox1.Enabled = True

        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        For I = 0 To RegKeys.GetUpperBound(0)
            If RegKeys(I).KeyStr = TextBox1.Text And TextBox1.Text <> "" Then
                If I <> ListView1.SelectedItems(0).Index Then
                    MessageBox.Show("已存在该热键，请从新设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.ActiveControl = TextBox1
                    Exit Sub
                End If
            End If
        Next
        Try
            RegKeys(ListView1.SelectedItems(0).Index).KeyStr = TextBox1.Text
            RegKeys(ListView1.SelectedItems(0).Index).KeyInfo = ComboBox1.Text
            TextBox1.Text = ""
            ComboBox1.SelectedIndex = 0
            TextBox1.Enabled = False
            ComboBox1.Enabled = False
        Catch ex As Exception
            MessageBox.Show("请先选择一个热键", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

        Call ShowRegKey()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        TextBox1.Text = ""
    End Sub
End Class