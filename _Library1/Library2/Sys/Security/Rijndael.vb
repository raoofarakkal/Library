Imports System.Security.Cryptography
Imports System.IO

Namespace Library2.Sys.Security

    Public Class Rijndael

        Private Shared mSalt As Byte() '= Text.Encoding.ASCII.GetBytes("a5r8a1k6k2a8l7")
        Private Shared mBit As Integer = 8

        Public Sub New()
        End Sub

        Private Shared Function SALT() As Byte()
            If mSalt Is Nothing Then
                mSalt = (New Library2.LibConfig()).RijndaelSALT()
            End If
            Return mSalt
        End Function


        Private Shared Function Encode(ByVal Value As Byte(), ByVal CharLimit As Integer) As String
            Dim mRet As String = ""
            If Value.Length > 0 Then
                For Each mC As Integer In Value
                    mRet += Hex(mC).PadLeft(2, "0")
                Next
                If CharLimit > 0 Then
                    If mRet.Length > CharLimit Then
                        Throw New Exception(String.Format("Exceeding the limit of {0} characters", CharLimit))
                    End If
                End If
            End If
            Return mRet
        End Function

        Private Shared Function DeCode(ByVal Value As String) As Byte()
            Dim mRet((Value.Length / 2) - 1) As Byte
            If Value.Length > 0 Then
                Dim cnt As Integer = 0
                For i As Integer = 0 To Value.Length - 1 Step 2
                    mRet(cnt) = Convert.ToInt32(Value.Substring(i, 2), 16)
                    cnt += 1
                Next
            End If
            Return mRet
        End Function

        Public Shared Function Encode(ByVal pString2Encode As String, ByVal pKey As String, Optional ByVal CharLimit As Integer = 1024)
            Dim mRet As String = ""
            If String.IsNullOrEmpty(pString2Encode) Then
                Throw New ArgumentNullException("Error on Encrypt() Parameter. pString2Encode required")
            End If
            If String.IsNullOrEmpty(pKey) Then
                Throw New ArgumentNullException("Error on Encrypt() Parameter. pKey required")
            End If

            Dim mRijMngd As RijndaelManaged = Nothing
            Try
                Dim mKey As New Rfc2898DeriveBytes(pKey, SALT())
                mRijMngd = New RijndaelManaged()
                mRijMngd.Key = mKey.GetBytes(mRijMngd.KeySize / mBit)
                mRijMngd.IV = mKey.GetBytes(mRijMngd.BlockSize / mBit)
                Dim mEncryptor As ICryptoTransform = mRijMngd.CreateEncryptor(mRijMngd.Key, mRijMngd.IV)
                Using msEncrypt As New MemoryStream()
                    Using csEncrypt As New CryptoStream(msEncrypt, mEncryptor, CryptoStreamMode.Write)
                        Using mSwEncrypt As New StreamWriter(csEncrypt)
                            mSwEncrypt.Write(pString2Encode)
                        End Using
                    End Using

                    mRet = Encode(msEncrypt.ToArray(), CharLimit)

                End Using
            Catch ex As Exception
                If InStr(ex.Message, "Exceeding the limit") > 0 Then
                    Throw ex
                Else
                    Throw New Exception("Encryption Failed. Key may be invalid. ", ex)
                End If
            Finally
                If mRijMngd IsNot Nothing Then
                    mRijMngd.Clear()
                End If
            End Try
            Return mRet
        End Function

        Public Shared Function Decode(ByVal pString2Decode As String, ByVal pKey As String)
            If String.IsNullOrEmpty(pString2Decode) Then
                Throw New ArgumentNullException("Error on Decrypt() Parameter. pString2Decode required")
            End If
            If String.IsNullOrEmpty(pKey) Then
                Throw New ArgumentNullException("Error on Decrypt() Parameter.  pKey required")
            End If

            Dim mRijMngd As RijndaelManaged = Nothing
            Dim mRet As String = Nothing
            Try
                Dim mKey As New Rfc2898DeriveBytes(pKey, SALT())
                mRijMngd = New RijndaelManaged()
                mRijMngd.Key = mKey.GetBytes(mRijMngd.KeySize / mBit)
                mRijMngd.IV = mKey.GetBytes(mRijMngd.BlockSize / mBit)
                Dim mDecryptor As ICryptoTransform = mRijMngd.CreateDecryptor(mRijMngd.Key, mRijMngd.IV)

                Dim mEncBytes As Byte() = Decode(pString2Decode)


                Using mMsDecrypt As New MemoryStream(mEncBytes)
                    Using mCsDecrypt As New CryptoStream(mMsDecrypt, mDecryptor, CryptoStreamMode.Read)
                        Using mSrDecrypt As New StreamReader(mCsDecrypt)
                            mRet = mSrDecrypt.ReadToEnd()
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                Throw New Exception("Decryption Failed. Key may be invalid. ", ex)
            Finally
                If mRijMngd IsNot Nothing Then
                    mRijMngd.Clear()
                End If
            End Try
            Return mRet
            'Return Decrypt(mRet, pKey)
        End Function

        Public Shared Function Encrypt(ByVal pString2Encrypt As String, ByVal pKey As String) As String
            If String.IsNullOrEmpty(pString2Encrypt) Then
                Throw New ArgumentNullException("Error on Encrypt() Parameter. pString2Encrypt required")
            End If
            If String.IsNullOrEmpty(pKey) Then
                Throw New ArgumentNullException("Error on Encrypt() Parameter. pKey required")
            End If

            Dim mRet As String = Nothing
            Dim mRijMngd As RijndaelManaged = Nothing
            Try
                Dim mKey As New Rfc2898DeriveBytes(pKey, SALT())
                mRijMngd = New RijndaelManaged()
                mRijMngd.Key = mKey.GetBytes(mRijMngd.KeySize / mBit)
                mRijMngd.IV = mKey.GetBytes(mRijMngd.BlockSize / mBit)
                Dim mEncryptor As ICryptoTransform = mRijMngd.CreateEncryptor(mRijMngd.Key, mRijMngd.IV)
                Using msEncrypt As New MemoryStream()
                    Using csEncrypt As New CryptoStream(msEncrypt, mEncryptor, CryptoStreamMode.Write)
                        Using mSwEncrypt As New StreamWriter(csEncrypt)
                            mSwEncrypt.Write(pString2Encrypt)
                        End Using
                    End Using

                    mRet = Convert.ToBase64String(msEncrypt.ToArray())

                End Using
            Catch ex As Exception
                Throw New Exception("Encryption Failed. Key may be invalid. ", ex)
            Finally
                If mRijMngd IsNot Nothing Then
                    mRijMngd.Clear()
                End If
            End Try
            Return mRet
        End Function

        Public Shared Function Decrypt(ByVal pEncryptedString As String, ByVal pKey As String) As String
            If String.IsNullOrEmpty(pEncryptedString) Then
                Throw New ArgumentNullException("Error on Decrypt() Parameter. pEncryptedString required")
            End If
            If String.IsNullOrEmpty(pKey) Then
                Throw New ArgumentNullException("Error on Decrypt() Parameter.  pKey required")
            End If

            Dim mRijMngd As RijndaelManaged = Nothing
            Dim mRet As String = Nothing
            Try
                Dim mKey As New Rfc2898DeriveBytes(pKey, SALT())
                mRijMngd = New RijndaelManaged()
                mRijMngd.Key = mKey.GetBytes(mRijMngd.KeySize / mBit)
                mRijMngd.IV = mKey.GetBytes(mRijMngd.BlockSize / mBit)
                Dim mDecryptor As ICryptoTransform = mRijMngd.CreateDecryptor(mRijMngd.Key, mRijMngd.IV)

                Dim mEncBytes As Byte() = Convert.FromBase64String(pEncryptedString)

                Try
                    Using mMsDecrypt As New MemoryStream(mEncBytes)
                        Using mCsDecrypt As New CryptoStream(mMsDecrypt, mDecryptor, CryptoStreamMode.Read)
                            Using mSrDecrypt As New StreamReader(mCsDecrypt)
                                mRet = mSrDecrypt.ReadToEnd()
                            End Using
                        End Using
                    End Using
                Catch ex As Exception
                    Throw New Exception("Encrypted String / Key / SALT may be invalid. ", Nothing)
                End Try

            Catch ex2 As Exception
                Throw New Exception("Decryption Failed. " & ex2.Message, Nothing)
            Finally
                If mRijMngd IsNot Nothing Then
                    mRijMngd.Clear()
                End If
            End Try
            Return mRet
        End Function

        Private Shared Function ByteArray2Str(ByVal pArray As Byte()) As String
            Dim mRet As String = ""
            Dim mEnc As New System.Text.ASCIIEncoding ' .UTF8Encoding()
            mRet = mEnc.GetString(pArray)
            mEnc = Nothing
            Return mRet
        End Function

        Private Shared Function Str2ByteArray(ByVal pString As String) As Byte()
            Dim mRet As Byte()
            Dim mEnc As New System.Text.ASCIIEncoding '.UTF8Encoding()
            mRet = mEnc.GetBytes(pString)
            mEnc = Nothing
            Return mRet
        End Function



    End Class



End Namespace