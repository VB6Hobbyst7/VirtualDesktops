Public Module Keyhook
    Friend Structure EVENTMSG
        Dim VKey As Integer
        Dim SKey As Integer
        Dim Flag As Integer
        Dim time As Integer
    End Structure
    Friend Structure RegSwit
        Dim Desktop As String
        Dim Index As Integer
        Dim handle As Integer
        Dim Form As Form1
    End Structure
    Friend LCtrl As Boolean, RCtrl As Boolean
    Friend LShift As Boolean, RShift As Boolean
    Friend LAlt As Boolean, RAlt As Boolean
    Friend LWin As Boolean, RWin As Boolean
    Friend KeyEx As String = ""
    Friend Const WH_KEYBOARD_LL = 13 '常数，表示钩子类型为低级键盘钩子
    Friend Const WM_KEYUP = &H101 '常数，表示按键弹起
    Friend Const WM_KEYDOWN = &H100 '常数，表示按键按下
    Friend Const WM_SYSKEYUP = &H105 '常数，表示系统按键弹起（Ait键）
    Friend Const WM_SYSKEYDOWN = &H104 '常数，表示系统按键按下（Ait键）
    Friend Declare Function SetWindowsHookExA Lib "user32" (ByVal idHook As Int32, ByVal lpfn As MyDelegate, ByVal hmod As Int32, ByVal dwThreadId As Int32) As Int32
    Friend Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Int32, ByVal ncode As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    Friend Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Int32) As Int32
    Friend Declare Sub RtlMoveMemory Lib "kernel32" (ByRef Destination As EVENTMSG, ByVal Source As Int32, ByVal Length As Int32)
    Friend Delegate Function MyDelegate(ByVal ncode As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    Friend Declare Function GetModuleHandleA Lib "kernel32" (ByVal lpModuleName As String) As Int32
    Friend HookAdd As New MyDelegate(AddressOf MyKBHook) 'VB.Net委托变量
    Friend TempThrea As Threading.Thread
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
                            Dim MyKey As Keys = MyMsg.VKey, DownKey As String
                            Dim DesktopIndex As Integer, DeskTop As String
                            If KeyEx <> "" Then
                                DownKey = KeyEx & MyKey.ToString
                                DeskTop = GetDesktopName()
                                '查找热键定义
                                For Each RegKey In RegKeys
                                    If RegKey.KeyStr = DownKey Then
                                        '因为是热键线程调用这里，必须遍历得到当前桌面的编号
                                        For i = 0 To MyDesktops.GetUpperBound(0)
                                            If MyDesktops(i).Desktop = DeskTop Then
                                                DesktopIndex = i
                                                Exit For
                                            End If
                                        Next
                                        '因为钩子必须很快返回，否则系统以为钩子死掉了
                                        '切换桌面交给新线程
                                        If TempThrea Is Nothing Then
                                            TempThrea = New Threading.Thread(AddressOf RegSwitchDesk)
                                            Dim RSwit As RegSwit
                                            RSwit.Desktop = RegKey.KeyInfo
                                            RSwit.Index = DesktopIndex
                                            RSwit.handle = MyDesktops(DesktopIndex).Handle
                                            RSwit.Form = MyDesktops(DesktopIndex).WinForm
                                            TempThrea.Start(RSwit)
                                            'SwitchDesktop(RegKey.KeyInfo, DesktopIndex)
                                            '吞掉按键消息
                                        End If
                                        Return 1
                                    End If
                                Next
                            End If
                        End If
                End Select
                KeyEx = (IIf(LCtrl Or RCtrl, "Ctrl + ", "") & IIf(LShift Or RShift, "Shift + ", "") &
                            IIf(LAlt Or RAlt, "Alt + ", "") & IIf(LWin Or RWin, "Win + ", ""))
            End If
        Catch ex As Exception

        End Try
        '传递到下一个钩子
        Return CallNextHookEx(0, ncode, wParam, lParam)
    End Function

    Friend Sub RegkeyHook()
        Dim HookID As Integer
        HookID = SetWindowsHookExA(WH_KEYBOARD_LL, HookAdd, GetModuleHandleA(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        If HookID = 0 Then
            MessageBox.Show("热键检测模块启动失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub RegSwitchDesk(RSwit As RegSwit)
        '把当前线程放到被切换的桌面，并关闭桌面句柄
        SetThreadDesktop(RSwit.handle)
        SwitchDesktop(RSwit.Form, RSwit.Desktop, RSwit.Index, , True)
        '任务完成后线程释放线程变量
        CloseDesktop(RSwit.handle)
        TempThrea = Nothing
    End Sub

End Module