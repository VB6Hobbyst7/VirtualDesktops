Public Class Form2

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        Me.TopMost = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim NewDesktop As String
        NewDesktop = Trim(TextBox1.Text)
        If NewDesktop = "" Then
            MessageBox.Show("新桌面名称不得为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.ActiveControl = TextBox1
        Else
            Dim Desktops As String()
            Desktops = EnumDesktops
            For Each Desktop In Desktops
                If UCase(Desktop) = UCase(NewDesktop) Then
                    MessageBox.Show("该桌面名称已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.ActiveControl = TextBox1
                    Exit Sub
                End If
            Next
            MyDesktops(Tag).NewDesktop = NewDesktop
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class