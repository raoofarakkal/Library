Public Class b67

    Private Sub btFromB67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFromB67.Click
        Dim obj As New _Library._System._Security.Protection.Protection
        Try
            If Me.tbKey.Text = "{raoofabdul}" Then
                Me.tbString.Text = _FromBase67(Me.tbB67String.Text)
            Else
                Me.tbString.Text = obj.FromBase67(Me.tbB67String.Text, Me.tbKey.Text)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            obj = Nothing
        End Try
    End Sub

#Region "hidden"
    Private Const BASE As Integer = 67
    Private Const SEPERATOR As Char = "-"
    Private Const ZERO As Char = "."
    Private _b67() As Char = "0123456789(!=_)abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray
    'Private _Chrs() As Char = "!#$%&'()*+,-.0123456789;<=> @ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~"
    Private Function _FromBase67(ByVal b67String As String) As String
        Dim mRet As String = ""
        Dim mRetDummy As String = ""
        For Each mS As String In b67String.Split(SEPERATOR)
            mRetDummy = ToNumber(mS)
            For i As Integer = 1 To mRetDummy.Length Step 3
                mRet += Chr(Mid(mRetDummy, i, 3) - 100)
            Next
        Next
        Return mRet
    End Function
    Private Function ToNumber(ByVal pHex67String As String) As Long
        Dim mB67 As Long
        Dim mRet As Long = 0

        Dim mMax As Integer = pHex67String.Length - 1
        For Each ms As Char In pHex67String
            If ms <> ZERO Then
                mB67 = B67idx(ms)
                mRet += mB67 * (BASE ^ mMax)
            End If
            mMax -= 1
        Next
        '4*(16^2)+6*(16^1)+8*(16^0)
        Return mRet
    End Function
    Private Function B67idx(ByVal pCh As Char) As Integer
        Dim mRet As Integer = -1
        mRet = Array.IndexOf(_b67, pCh)
        Return mRet
    End Function
#End Region

    Private Sub btToB67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btToB67.Click
        Dim obj As New _Library._System._Security.Protection.Protection
        Try
            Me.tbB67String.Text = obj.ToBase67(Me.tbString.Text, Me.tbKey.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            obj = Nothing
        End Try

    End Sub

    Private Sub btAjnDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAjnDecrypt.Click
        Me.tbString.Text = Aljazeera.Encryption.CryptographyManager.Decrypt(Me.tbB67String.Text, Me.tbKey.Text)
    End Sub

    Private Sub btAjnEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAjnEncrypt.Click
        Me.tbB67String.Text = Aljazeera.Encryption.CryptographyManager.Encrypt(Me.tbString.Text, Me.tbKey.Text)
    End Sub

    Private Sub btB67Key_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btB67Key.Click
        Me.tbKey.Text = "jazeera"
    End Sub

    Private Sub btAjnEncKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btAjnEncKey.Click
        Me.tbKey.Text = "Aljazeera"
    End Sub
End Class