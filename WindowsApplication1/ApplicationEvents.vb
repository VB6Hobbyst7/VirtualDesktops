Namespace My
    ' 以下事件可用于 MyApplication:
    ' 
    ' Startup: 应用程序启动时在创建启动窗体之前引发。
    ' Shutdown: 在关闭所有应用程序窗体后引发。如果应用程序异常终止，则不会引发此事件。
    ' UnhandledException: 在应用程序遇到未经处理的异常时引发。
    ' StartupNextInstance: 在启动单实例应用程序且应用程序已处于活动状态时引发。
    ' NetworkAvailabilityChanged: 在连接或断开网络连接时引发。
    Partial Friend Class MyApplication

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            e.BringToForeground = False
            Dim MyForm As Form1 = Nothing, Desktop As String
            Desktop = GetDesktopName()
            For Each MyDesktop In MyDesktops
                If MyDesktop.Desktop = Desktop Then
                    MyForm = MyDesktop.WinForm
                    Exit For
                End If
            Next
            If MyForm Is Nothing Then
                Dim Temp As New Threading.Thread(AddressOf InstanceSwitch)
                Temp.Start(Desktop)
                Temp = Nothing
            Else
                Dim Temp As New Threading.Thread(AddressOf ShowTisp)
                Temp.Start(MyForm)
                Temp = Nothing
            End If
        End Sub
        Private Sub ShowTisp(MyForm As Form1)
            Dim InputDesk As String = GetDesktopName()
            Dim Desktophandle As Integer = OpenDesktopProc(InputDesk, 0, True, EnumDESKTOP.GENERIC_ALL)
            SetThreadDesktop(Desktophandle)
            If MyForm.Visible Then
                SetForegroundWindow(MyForm.Handle)
            Else
                MyForm.ShowTisp()
            End If
        End Sub
        Private Sub InstanceSwitch(Desktop As String)
            Dim InputDesk As String = GetDesktopName()
            Dim Desktophandle As Integer = OpenDesktopProc(InputDesk, 0, True, EnumDESKTOP.GENERIC_ALL)
            SetThreadDesktop(Desktophandle)
            Call CreateThread(Desktophandle, InputDesk, False)
        End Sub

    End Class


End Namespace

