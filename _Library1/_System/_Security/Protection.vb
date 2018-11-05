Namespace _Library._System._Security

    Namespace Protection

        Public NotInheritable Class Protection
            Private mAlg As String = "AjCmsWeb"

            Private Function ProcessorID() As String
                Dim mRet As String = ""
                Dim objMOS As Management.ManagementObjectSearcher
                Dim objMOC As System.Management.ManagementObjectCollection
                Dim objMO As New System.Management.ManagementObject
                objMOS = New Management.ManagementObjectSearcher("Select ProcessorID From Win32_Processor")
                objMOC = objMOS.Get
                For Each objMO In objMOC
                    mRet = objMO("ProcessorID")
                    Exit For
                Next
                objMOS.Dispose()
                objMOS = Nothing
                objMO.Dispose()
                objMO = Nothing
                Return mRet
            End Function

            Private Function BiosSerial() As String
                Dim mRet As String = ""
                Dim objMOS As Management.ManagementObjectSearcher
                Dim objMOC As System.Management.ManagementObjectCollection
                Dim objMO As New System.Management.ManagementObject
                objMOS = New Management.ManagementObjectSearcher("Select SerialNumber From Win32_BIOS")
                objMOC = objMOS.Get
                For Each objMO In objMOC
                    mRet = objMO("SerialNumber")
                    Exit For
                Next
                objMOS.Dispose()
                objMOS = Nothing
                objMO.Dispose()
                objMO = Nothing
                Return mRet
            End Function

            Private Function Code(ByVal Value As String) As String
                If Value.Length > 0 Then
                    Dim mRTL As String = ""
                    Dim mDash As String = ""
                    'Right To Left
                    For cnt As Integer = Value.Length - 1 To 0 Step -1
                        mRTL = mRTL & Hex(Asc(Value.Substring(cnt, 1)))
                    Next
                    'Adding -
                    For cnt As Integer = 0 To mRTL.Length - 1
                        If cnt <> 0 And (cnt Mod 5) = 0 Then
                            mDash = mDash & "-" & mRTL.Substring(cnt, 1)
                        Else
                            mDash = mDash & mRTL.Substring(cnt, 1)
                        End If
                    Next
                    Return mDash
                Else
                    Return Value
                End If
            End Function

            Private Function DeCode(ByVal Value As String) As String
                If Value.Length > 0 Then
                    Dim mRet As String = ""
                    Dim mRTL As String = ""
                    Value = Replace(Value, "-", "")
                    For cnt As Integer = 0 To Value.Length - 1 Step 2
                        mRTL = mRTL & Chr(Convert.ToInt32(Value.Substring(cnt, 2), 16))
                    Next
                    For cnt As Integer = mRTL.Length - 1 To 0 Step -1
                        mRet = mRet & mRTL.Substring(cnt, 1)
                    Next
                    Return mRet
                Else
                    Return Value
                End If
            End Function

            Private Function ToBase64(ByVal Value As String) As String
                If Value.Length > 0 Then
                    Value = Value & mAlg
                    Dim mByte() As Byte
                    ReDim mByte(Value.Length)
                    For cnt As Integer = 0 To Value.Length - 1
                        mByte(cnt) = CByte(Asc(Value.Substring(cnt, 1)))
                    Next
                    Return Convert.ToBase64String(mByte, Base64FormattingOptions.None, mByte.Length)
                Else
                    Return ""
                End If
            End Function

            Private Function FromBase64(ByVal Base64String As String) As String
                Dim mRet As String = ""
                Dim mByte() As Byte = Convert.FromBase64String(Base64String)
                For cnt As Integer = 0 To UBound(mByte) - 1
                    mRet = mRet & Chr(mByte(cnt))
                Next
                Return Replace(mRet, mAlg, "")
            End Function

            Public Function Valid(ByVal Key As String) As Boolean
                Return True
                'If Code(ToBase67(CustomerCode(), "AjLib")) = Key Then
                '    Return True
                'Else
                '    Return False
                'End If
            End Function

            Public Function CustomerCode() As String
                Return Code(ProcessorID.Trim & BiosSerial.Trim)
            End Function

            Public Function GenerateKey(ByVal CustomerCode As String, ByVal Password As String) As String
                '<!--
                Dim mRet As String
                Try
                    Dim e As Char = Chr(51) '3
                    Dim o As Char = Chr(48) '0
                    Dim i As Char = Chr(49) '1
                    If Password = "G" & e & "tM" & e & "Th" & e & "R" & i & "ghtC" & o & "d" & e Then
                        mRet = Code(ToBase67(CustomerCode, "AjLib"))
                    Else
                        mRet = "Err:2311 Invalid CustomerCode/Password"
                    End If
                Catch ex As Exception
                    mRet = "Err:2731 Invalid CustomerCode/Password"
                End Try
                Return mRet
                '-->
            End Function

            Public Function ToBase64(ByVal Value As String, ByVal Method As String) As String
                Value = Value & Method
                Return ToBase64(Value)
            End Function

            Public Function FromBase64(ByVal Base64String As String, ByVal Method As String) As String
                Return Replace(FromBase64(Base64String), Method, "")
            End Function

            Public Function ToBase16(ByVal pStr As String)
                Dim mHex As String = ""
                For Each mS As Char In pStr
                    mHex += Hex(Asc(mS))
                Next
                Return mHex
            End Function

            Public Function FromBase16(ByVal pStr As String)
                Dim mStr As String = ""
                Dim mSinglehex As String = ""
                For Each mS As Char In pStr
                    mSinglehex += mS
                    If mSinglehex.Length = 2 Then
                        mStr += Chr(Convert.ToInt32(mSinglehex, 16))
                        mSinglehex = ""
                    End If
                Next
                Return mStr
            End Function

            Public Function ToBase67(ByVal pStr As String, ByVal Key As String) As String
                Dim mC As New Base67
                Dim mRet As String = ""
                Try
                    mRet = mC.ToBase67(pStr, Key)
                Catch ex As Exception
                    Throw ex
                End Try
                mC = Nothing
                Return mRet
            End Function

            Public Function FromBase67(ByVal Base67String As String, ByVal Key As String) As String
                Dim mC As New Base67
                Dim mRet As String = ""
                Try
                    mRet = mC.FromBase67(Base67String, Key)
                Catch ex As Exception
                    Throw ex
                End Try
                mC = Nothing
                Return mRet
            End Function

            Public Function ToBase67ltd(ByVal pStr As String, ByVal Key As String) As String
                Dim mRet As String = ""
                Dim mC As New Base67
                Try
                    mC.Encryption = Base67.CompressionType.High
                    mRet = mC.ToBase67(pStr, Key)
                Catch ex As Exception
                    Throw ex
                End Try
                mC = Nothing
                Return mRet
            End Function

            Public Function FromBase67ltd(ByVal Base67String As String, ByVal Key As String) As String
                Dim mRet As String = ""
                Dim mC As New Base67
                Try
                    mC.Encryption = Base67.CompressionType.High
                    mRet = mC.FromBase67(Base67String, Key)
                Catch ex As Exception
                    Throw ex
                End Try
                mC = Nothing
                Return mRet
            End Function

            Private NotInheritable Class Base67
                Private Const BASE As Integer = 67
                Private Const SEPERATOR As Char = "-"
                Private Const ZERO As Char = "."
                Private _b67() As Char = "0123456789(!=_)abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray
                Private _Chrs() As Char = "!#$%&'()*+,-.0123456789;<=> @ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~"
                Private mType As CompressionType = CompressionType.Normal

                Public Sub New()

                End Sub

                Protected Overrides Sub Finalize()
                    _b67 = Nothing
                    MyBase.Finalize()
                End Sub

                Private Function B67idx(ByVal pCh As Char) As Integer
                    Dim mRet As Integer = -1
                    mRet = Array.IndexOf(_b67, pCh)
                    Return mRet
                End Function

                Private Function _Asc(ByVal _Chr As Char) As Integer
                    Dim mRet As Integer = -1
                    mRet = Array.IndexOf(_Chrs, _Chr)
                    Return mRet
                End Function

                Private Function _chr(ByVal _AscCode As Integer) As Char
                    Return _Chrs(_AscCode)
                End Function

                Private Function ToB67(ByVal pNumber As Long) As String
                    Dim mMyHexS As String = ""
                    Dim mResult As Long = pNumber
                    Dim mRemainder As Double
                    While (mResult >= BASE)
                        mRemainder = mResult Mod BASE
                        mResult = Math.Floor(mResult / BASE)
                        mMyHexS = _b67(mRemainder) & mMyHexS
                    End While
                    mMyHexS = _b67(mResult) & mMyHexS
                    Return mMyHexS.PadLeft(2, ZERO)
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

                Private Function _ToBase67(ByVal pString As String) As String
                    Dim mRet As String = ""
                    Dim mRetDummy As String = ""
                    Dim mAscStr As String = ""
                    Dim i As Integer = 5
                    Dim mSep As String = ""
                    For Each mC As Char In pString
                        If i = 0 Then
                            i = 5
                            mAscStr += SEPERATOR
                        End If
                        mAscStr += (Asc(mC) + 100).ToString
                        i -= 1
                    Next
                    For Each mS As String In mAscStr.Split(SEPERATOR)
                        mRetDummy = ToB67(mS)
                        mRet += mSep & mRetDummy
                        mSep = SEPERATOR
                    Next
                    Return mRet
                End Function

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

                Private Function _ToBase67Ltd(ByVal pString As String) As String
                    Dim mRet As String = ""
                    Dim mRetDummy As String = ""
                    Dim mAscStr As String = ""
                    Dim i As Integer = 8
                    Dim mSep As String = ""
                    For Each mC As Char In pString
                        If i = 0 Then
                            i = 8
                            mAscStr += SEPERATOR
                        End If
                        If _Asc(mC) = -1 Then
                            Throw New Exception("Invalid character found in parameter. valid characters are : " & _Chrs & " or try normal encryption")
                        End If
                        mAscStr += (_Asc(mC) + 10).ToString
                        i -= 1
                    Next
                    For Each mS As String In mAscStr.Split(SEPERATOR)
                        mRetDummy = ToB67(mS)
                        mRet += mSep & mRetDummy
                        mSep = SEPERATOR
                    Next
                    Return mRet
                End Function

                Private Function _FromBase67Ltd(ByVal b67String As String) As String
                    Dim mRet As String = ""
                    Dim mRetDummy As String = ""
                    For Each mS As String In b67String.Split(SEPERATOR)
                        mRetDummy = ToNumber(mS)
                        For i As Integer = 1 To mRetDummy.Length Step 2
                            mRet += _chr(Mid(mRetDummy, i, 2) - 10)
                        Next
                    Next
                    Return mRet
                End Function

                Private Function ValidKey(ByVal Key As String) As Boolean
                    Dim mRet As Boolean = True
                    For Each mC As Char In Key
                        If InStr("0123456789abcdefghijklmnopqrstuvwxyz", mC, CompareMethod.Text) = 0 Then
                            mRet = False
                            Exit For
                        End If
                    Next
                    Return mRet
                End Function

                Public Enum CompressionType
                    Normal = 1
                    High = 2
                End Enum

                Public Property Encryption() As CompressionType
                    Get
                        Return mType
                    End Get
                    Set(ByVal value As CompressionType)
                        If value = CompressionType.High Then
                            mType = CompressionType.High
                        Else
                            mType = CompressionType.Normal
                        End If
                    End Set
                End Property

                Public Function ToBase67(ByVal pString As String, ByVal Key As String) As String
                    Dim mRet As String = ""
                    If pString.Length > 2 And Key.Length > 2 Then
                        If ValidKey(Key) Then
                            If Encryption = CompressionType.High Then
                                mRet = _ToBase67Ltd(Key + "'" & pString)
                            Else
                                mRet = _ToBase67(Key + "'" & pString)
                            End If
                        Else
                            Throw New Exception("Key should not contain any Symbols or space")
                        End If
                    Else
                        Throw New Exception("Minimum length 3 required for key or string to encode")
                    End If
                    Return mRet
                End Function

                Public Function FromBase67(ByVal Base67String As String, ByVal Key As String) As String
                    Dim mRet As String = ""
                    If Key.Length > 2 Then
                        If ValidKey(Key) Then
                            Dim mStr As String
                            If Encryption = CompressionType.High Then
                                mStr = _FromBase67Ltd(Base67String)
                            Else
                                mStr = _FromBase67(Base67String)
                            End If
                            If Key = mStr.Split("'")(0) Then
                                mRet = mStr.Split("'")(1)
                            Else
                                mRet = "Invalid Key"
                            End If
                        Else
                            Throw New Exception("Key should not contain any Symbols or space")
                        End If
                    Else
                        Throw New Exception("Minimum length 2 required for key")
                    End If
                    Return mRet
                End Function

            End Class

        End Class

    End Namespace

End Namespace


