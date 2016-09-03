Public Module Module1
    '为防止热键冲突本程序用钩子获取组合键信息
    'Public Declare Function RegisterHotKey Lib "user32" (ByVal hWnd As Integer, ByVal id As Integer, ByVal fsModifiers As Integer, ByVal vk As Integer) As Integer
    Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
    Public Declare Function BringWindowToTop Lib "user32" Alias "BringWindowToTop" (ByVal hwnd As Integer) As Integer
    Public Declare Function GetUserObjectInformation Lib "User32" Alias "GetUserObjectInformationA" (ByVal hObj As Integer, ByVal nIndex As EnumUOI, ByVal pvInfo As String, ByVal nLength As Integer, ByRef lpnLengthNeeded As Integer) As Integer
    Public Declare Function CreateProcess Lib "kernel32" Alias "CreateProcessA" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByVal lpProcessAttributes As Integer, ByVal lpThreadAttributes As Integer, ByVal bInheritHandles As Integer, ByVal dwCreationFlags As Integer, ByVal lpEnvironment As Integer, ByVal lpCurrentDriectory As String, ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Boolean
    Public Declare Function GetProcessWindowStation Lib "user32" Alias "GetProcessWindowStation" () As Integer
    Public Declare Function GetDesktopWindow Lib "user32" Alias "GetDesktopWindow" () As Integer
    Public Declare Function CreateDesktopProc Lib "user32" Alias "CreateDesktopA" (ByVal lpszDesktop As String, ByVal lpszDevice As String, pDevmode As Integer, ByVal dwFlags As Integer, ByVal dwDesiredAccess As Integer, lpsa As Integer) As Integer
    Public Declare Function OpenDesktopProc Lib "user32" Alias "OpenDesktopA" (ByVal lpszDesktop As String, ByVal dwFlags As Integer, ByVal fInherit As Boolean, ByVal dwDesiredAccess As EnumDESKTOP) As Integer
    Public Declare Function OpenInputDesktopProc Lib "user32" Alias "OpenInputDesktop" (ByVal dwFlags As Integer, ByVal fInherit As Boolean, ByVal dwDesiredAccess As Integer) As Integer
    Public Declare Function SwitchDesktopProc Lib "user32" Alias "SwitchDesktop" (ByVal hDesktop As Integer) As Integer
    Public Declare Function CloseDesktop Lib "user32" (ByVal hDesktop As Integer) As Integer
    Public Declare Function GetThreadDesktop Lib "user32" (ByVal dwThread As Integer) As Integer
    Public Declare Function SetThreadDesktop Lib "user32" (ByVal hDesktop As Integer) As Integer
    Private Declare Function EnumDesktops Lib "user32" Alias "EnumDesktopsA" (ByVal hwinsta As Integer, ByVal lpEnumFunc As DelegateEDProc, ByVal lParam As Integer) As Integer
    Public Declare Function EnumDesktopWindows Lib "user32" (ByVal hDesktop As Integer, ByVal lpfn As Integer, ByVal lParam As Integer) As Integer
    Public Declare Function GetCurrentThreadId Lib "kernel32" () As Integer
    Public Delegate Function DelegateEDProc(Handle As String, lParam As Integer) As Boolean
    Public EnumDesktopsProc As New DelegateEDProc(AddressOf FunctionEDProc)
    Public Const MainText As String = "Windows虚拟桌面 "
    Private DesktopsNames() As String
    Public MyDesktops() As MyDesktop
    Public Structure MyDesktop
        Dim Desktop As String  '桌面窗口名字
        Dim Handle As Integer '桌面的句柄
        Dim Thread As Threading.Thread '程序线程
        Dim WinForm As Form1 '线程的窗体
        Dim NewDesktop As String '创建新桌面和New的Form2交换数据
        Dim Picture As Bitmap '保存桌面预览
        Dim IsCretae As Boolean '是新创建的桌面（新创建桌面会启动Explorer）
    End Structure

    '此API有回调函数，这里重载一下，API声明成私有,方便外部调用
    Public Function EnumDesktops() As String()
        ReDim DesktopsNames(-1)
        EnumDesktops(GetProcessWindowStation, EnumDesktopsProc, 0)
        Return DesktopsNames
    End Function

    '这是一个API的回调函数，设置私有，防止外部调用
    Private Function FunctionEDProc(Desktop As String, lParam As Integer) As Boolean
        ReDim Preserve DesktopsNames(DesktopsNames.GetUpperBound(0) + 1)
        DesktopsNames(DesktopsNames.GetUpperBound(0)) = Desktop
        Return True '返回一个非0的值，确认回调成功
    End Function

    Public Sub ShowMian(ByVal DesktopIndex As Long)
        '设置线程的桌面
        With MyDesktops(DesktopIndex)
            SetThreadDesktop(.Handle)
            '为新的桌面启动Explorer
            If .IsCretae Then
                .IsCretae = False
                CreateProcess(Environ("WinDir") & "\Explorer.exe", .Desktop)
            End If
            '为线程新建一个窗口
            .WinForm = New Form1
            '这里如果用Show，线程会结束导致窗体关闭
            'ShowDialog窗体会以模式窗体展现 
            Application.Run(.WinForm)
        End With
    End Sub

    Public Function GetDesktopName(ThreadID As Integer) As String
        Dim DesktopHandle As Integer, DesktopName As New String(Chr(0), 256), length As Integer
        DesktopHandle = GetThreadDesktop(ThreadID)
        GetUserObjectInformation(DesktopHandle, 2, DesktopName, 256, length)
        CloseDesktop(DesktopHandle)
        Return DesktopName.Replace(Chr(0), "")
    End Function

    Public Function GetDesktopName() As String
        Dim DesktopHandle As Integer, DesktopName As New String(Chr(0), 256), length As Integer
        DesktopHandle = OpenInputDesktopProc(0, False, 0)
        GetUserObjectInformation(DesktopHandle, 2, DesktopName, 256, length)
        CloseDesktop(DesktopHandle)
        Return DesktopName.Replace(Chr(0), "")
    End Function
    Public Function CreateProcess(ApplicationName As String, Desktop As String) As Boolean
        Dim sInfo As STARTUPINFO = Nothing
        Dim pInfo As PROCESS_INFORMATION
        sInfo.lpDesktop = Desktop
        Return CreateProcess(vbNullString, ApplicationName, 0, 0, False, &H4000000, 0, vbNullString, sInfo, pInfo)
    End Function

End Module