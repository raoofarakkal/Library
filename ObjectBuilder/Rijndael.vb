Imports Library2.Sys.Security.Rijndael
Public Class Rijndael

    Private Sub btEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btEncrypt.Click
        Try
            Me.tbEncryptedString.Text = Encrypt(Me.tbString.Text, Me.tbKey.Text)
            CalcLen()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btDecrypt.Click
        Try
            Me.tbString.Text = Decrypt(Me.tbEncryptedString.Text, Me.tbKey.Text)
            CalcLen()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btEncode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btEncode.Click
        Try
            Me.tbEncodedString.Text = Encode(Me.tbString.Text, Me.tbKey.Text, -1)
            CalcLen()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btDecode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btDecode.Click
        Try
            Me.tbString.Text = DeCode(Me.tbEncodedString.Text, Me.tbKey.Text)
            CalcLen()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CalcLen()
        Me.lblString.Text = Me.lblString.Text.Split(":")(0).Trim & " : " & Me.tbString.Text.Length
        Me.lblEncryptedString.Text = Me.lblEncryptedString.Text.Split(":")(0).Trim & " : " & Me.tbEncryptedString.Text.Length
        Me.lblEncodedString.Text = Me.lblEncodedString.Text.Split(":")(0).Trim & " : " & Me.tbEncodedString.Text.Length
    End Sub
End Class