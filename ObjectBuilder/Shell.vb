Public Class Shell

    Private Sub btExec_Click(sender As System.Object, e As System.EventArgs) Handles btExec.Click
        Dim password As New Security.SecureString
        For Each c As Char In Me.tbPWD.Text.ToCharArray
            password.AppendChar(c)
        Next
        System.Diagnostics.Process.Start(Me.tbShellCmd.Text, Me.tbPara.Text, Me.tbUID.Text, password, Me.tbDomain.Text)

    End Sub



End Class


