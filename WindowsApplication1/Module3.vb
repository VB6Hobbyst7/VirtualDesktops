Module Module3
    Friend Sub SwitchDesktop(WinForm As Form, DesktopName As String, DesktopIndex As Integer, Optional Desktophandle As Integer = 0, Optional RegKey As Boolean = False)
        Dim IsThread As Boolean, IsCreate As Boolean
        '快捷键切换任务未完成
        If Not RegKey And Not TempThrea Is Nothing Then
            Exit Sub
        End If
        'WinForm表示新实例启动时，没有找到当前桌面的窗体，所以要创建
        If Not WinForm Is Nothing Then
            If DesktopName = GetDesktopName() Then
                MessageBox.Show(WinForm, "当前已在桌面""" & DesktopName & """", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If
        If Desktophandle = 0 Then
            '查找工作在此桌面的线程,并加测该线程是否还在线运行（防止线程过多节约资源）
            For I = 0 To MyDesktops.GetUpperBound(0)
                With MyDesktops(I)
                    If .Desktop = DesktopName Then
                        IsThread = True '找到线程
                        Desktophandle = .Handle
                        If .Thread.ThreadState = Threading.ThreadState.Running Then
                            '线程还在运行 
                        Else
                            '线程已关闭，从新设置线程并启动
                            .Thread = New Threading.Thread(AddressOf ShowMian)
                            .Thread.Start(I)
                        End If

                    End If
                End With
            Next

            If Desktophandle = 0 Then
                Desktophandle = OpenDesktopProc(DesktopName, 0, True, EnumDESKTOP.GENERIC_ALL)
            End If
            If Desktophandle = 0 Then
                MessageBox.Show(WinForm, "无法切换到桌面""" & DesktopName & """", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Else
            IsCreate = True
        End If

        If IsThread = False Then
            Call CreateThread(Desktophandle, DesktopName, IsCreate)
        End If
        '不是新实例启动导致的创建窗体，则切换桌面前保存缩略图
        If Not WinForm Is Nothing Then MyDesktops(DesktopIndex).Picture = CopyScreen()
        SwitchDesktopProc(Desktophandle)
    End Sub

    Friend Sub CreateThread(Desktophandle As Integer, DesktopName As String, Optional IsCreate As Boolean = False)
        Dim N As Integer
        With MyDesktops
            N = .GetUpperBound(0) + 1
            ReDim Preserve MyDesktops(N)
            MyDesktops(N).Thread = New Threading.Thread(AddressOf ShowMian)
            MyDesktops(N).Handle = Desktophandle
            MyDesktops(N).Desktop = DesktopName
            MyDesktops(N).IsCretae = IsCreate
            MyDesktops(N).Thread.Start(N)
        End With
    End Sub

    Friend Function CopyScreen() As Bitmap
        Try
            Dim Point1 As New Point(0, 0)
            Dim Point2 As New Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
            Dim Source As New Bitmap(Point2.X, Point2.Y)
            Dim BitSize As Size = GetScreenSize()
            Dim Destin As New Bitmap(BitSize.Width, BitSize.Height)
            Using SGrap As Graphics = Graphics.FromImage(Source)
                SGrap.CopyFromScreen(Point1, Point1, Point2)
                Using DGrap As Graphics = Graphics.FromImage(Destin)
                    DGrap.DrawImage(Source, Point1.X, Point1.Y, BitSize.Width, BitSize.Height)
                    Return Destin
                End Using
            End Using

        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Friend Function GetBitmapSize(Bitmap As Bitmap) As Size
        Dim ScreenW As Integer, ScreenH As Integer
        ScreenW = Bitmap.Size.Width
        ScreenH = Bitmap.Size.Height
        If ScreenW / ScreenH < 0.5 Then         '16:9竖屏
            Return New Size(86, 150)
        ElseIf ScreenW / ScreenH < 1 Then       '4:3竖屏
            Return New Size(112, 150)
        ElseIf ScreenW / ScreenH < 1.5 Then     '4:3横屏
            Return New Size(200, 150)
        ElseIf ScreenW / ScreenH < 2 Then       '16:9横屏
            Return New Size(200, 117)
        End If
    End Function

    Friend Function GetScreenSize() As Size
        Dim ScreenW As Integer, ScreenH As Integer
        ScreenW = Screen.PrimaryScreen.Bounds.Width
        ScreenH = Screen.PrimaryScreen.Bounds.Height
        If ScreenW / ScreenH < 0.5 Then         '16:9竖屏
            Return New Size(86, 150)
        ElseIf ScreenW / ScreenH < 1 Then       '4:3竖屏
            Return New Size(112, 150)
        ElseIf ScreenW / ScreenH < 1.5 Then     '4:3横屏
            Return New Size(200, 150)
        ElseIf ScreenW / ScreenH < 2 Then       '16:9横屏
            Return New Size(200, 117)
        End If
    End Function



End Module
