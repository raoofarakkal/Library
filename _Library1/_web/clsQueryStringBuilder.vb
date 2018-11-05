Namespace _Library._Web
    Public Class QueryStringBuilder

        Public Function Encrypt(ByVal pStr As String) As String
            Dim mEnc As New _Library._System._Security.Protection.Protection
            Dim mStr As String = ""
            'Return Replace(pStr, ",", "-")
            mStr = mEnc.ToBase64(pStr, "T1")
            mEnc = Nothing
            Return mStr
        End Function

        Public Function Decrypt(ByVal pStr As String) As String
            Dim mEnc As New _Library._System._Security.Protection.Protection
            Dim mStr As String = ""
            'Return Replace(pStr, "-", ",")
            mStr = mEnc.FromBase64(pStr, "T1")
            mEnc = Nothing
            Return mStr
        End Function

    End Class
End Namespace
