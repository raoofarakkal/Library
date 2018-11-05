Imports System.IO

Namespace Library2

    Public Class LibConfig

        Private Shared mRijndaelSALT As Byte()
        Private Shared mRjKey2DecryptPwdKey As String
        Private Shared mb67Key2DecryptPwdKey As String
        Private Shared mApplicationMode As String
        Private Shared mLibConfFile As LibConfigFile

        Private mConfigCfgLoc As String = System.Configuration.ConfigurationManager.AppSettings.Item("AJCMS_CONFIG_FOLDER") '"C:\_AJCMS"
        Private mConfigCfgFileName As String = System.Configuration.ConfigurationManager.AppSettings.Item("AJCMS_CONFIG_FILE") '"AJCMS2.CFG"

        Public Sub New()
            If mLibConfFile Is Nothing Then
                'mRijndaelSALT = Text.Encoding.ASCII.GetBytes(i("6135723861316b366b3261386c371"))
                'mRjKey2DecryptPwdKey = i("41bf6a40a97a33d733c6b8aea7b72")
                'mb67Key2DecryptPwdKey = i("41316a417a333336613")
                mApplicationMode = "web"
                mLibConfFile = New LibConfigFile(mConfigCfgLoc, mConfigCfgFileName, Me)
                Try
                    mLibConfFile.FetchSettings()
                Catch ex As Exception
                    mLibConfFile = Nothing
                    Throw
                End Try
            End If
        End Sub

        Public Function RijndaelSALT() As Byte()
            Return mRijndaelSALT
        End Function

        Public Function RjKey2DecryptPwdKey() As String
            Return mRjKey2DecryptPwdKey
        End Function

        Public Function b67Key2DecryptPwdKey() As String
            Return mb67Key2DecryptPwdKey
        End Function


        Public Enum _AppMode
            Web = 1
            Console = 2
        End Enum

        Public Function ApplicationMode() As _AppMode
            Return IIf(mApplicationMode.Trim.ToLower = "console", _AppMode.Console, _AppMode.Web)
        End Function

        Private Shared Function i(ByVal p As String) As String
            Dim text As New System.Text.StringBuilder(p.Length \ 2)
            For ii As Integer = 0 To p.Length - 2 Step 2
                text.Append(Chr(Convert.ToByte(p.Substring(ii, 2), 16)))
            Next
            Return text.ToString()
        End Function

        Private Class LibConfigFile
            Private mConfigFileLocation As String
            Private mConfigFileName As String
            Private mConf As LibConfig

            Public Sub New(ByVal ConfigFileLocation As String, ByVal ConfigFileName As String, ByRef pConfig As LibConfig)
                mConfigFileLocation = ConfigFileLocation
                mConfigFileName = ConfigFileName
                mConf = pConfig
            End Sub

            Private Function getConfigItem(pDs As DataSet, pTableName As String, pItemName As String) As String
                Try
                    If pDs IsNot Nothing Then
                        If pDs.Tables(pTableName).Rows.Count > 0 Then
                            If pDs.Tables(pTableName).Rows(0).Item(pItemName) IsNot Nothing Then
                                Return pDs.Tables(pTableName).Rows(0).Item(pItemName).ToString()
                            Else
                                Return ""
                            End If
                        Else
                            Throw New Exception(String.Format("Failed to Read from AJCMS Config Section:{0} not found", pTableName))
                        End If
                    Else
                        Throw New Exception("AJCMS Config file not loaded in memory")
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format("Failed to Read from AJCMS Config Section:{0} Item:{1}", pTableName, pItemName))
                End Try
            End Function

            Public Function FetchSettings() As Boolean

                If String.IsNullOrEmpty(mConfigFileLocation) Or String.IsNullOrEmpty(mConfigFileName) Then
                    Throw New Exception("AJCMS Config File or Location Not found. please check your web config and make sure you have the following configuration on it" & vbCrLf _
                    & vbCrLf & "<appSettings>" _
                    & vbCrLf & "	<add key=""AJCMS_CONFIG_FOLDER"" value=""C:\CONFIG""/> " _
                    & vbCrLf & "	<add key=""AJCMS_CONFIG_FILE"" value=""AJCMS2.CFG""/> " _
                    & vbCrLf & "</appSettings>")
                End If

                Dim mDs As New Data.DataSet
                Try
                    SyncLock GetType(LibConfigFile)
                        mDs = _Library._IO.clsSingleTon.ReadXML(mConfigFileLocation & "\" & mConfigFileName)
                    End SyncLock
                Catch ex As Exception
                    Throw New Exception(String.Format("File {0}\{1} Read Error: {2}", mConfigFileLocation, mConfigFileName, ex.Message))
                End Try

                If mDs Is Nothing Then
                    Throw New Exception(String.Format("Unable to read File {0}\{1}", mConfigFileLocation, mConfigFileName))
                Else

                    Try

                        'Library
                        Try
                            Dim mTemp As String = getConfigItem(mDs, "Library", "RijndaelSALT")
                            If String.IsNullOrWhiteSpace(mTemp) Then
                                Throw New Exception("RijndaelSALT vaue cannot be null")
                            End If
                            mRijndaelSALT = Text.Encoding.ASCII.GetBytes(i(mTemp))
                        Catch ex As Exception
                            Throw New Exception("Error in  Library section. " & ex.Message)
                        End Try

                        Try
                            mRjKey2DecryptPwdKey = i(getConfigItem(mDs, "Library", "RjKey2DecryptPwdKey"))
                            If String.IsNullOrWhiteSpace(mRjKey2DecryptPwdKey) Then
                                Throw New Exception("RjKey2DecryptPwdKey vaue cannot be null")
                            End If
                        Catch ex As Exception
                            Throw New Exception("Error in  Library section. " & ex.Message)
                        End Try

                        Try
                            mb67Key2DecryptPwdKey = i(getConfigItem(mDs, "Library", "b67Key2DecryptPwdKey"))
                            If String.IsNullOrWhiteSpace(mb67Key2DecryptPwdKey) Then
                                Throw New Exception("b67Key2DecryptPwdKey vaue cannot be null")
                            End If
                        Catch ex As Exception
                            Throw New Exception("Error in  Library section. " & ex.Message)
                        End Try

                    Catch ex As Exception
                        Throw New Exception("Invalid AJCMS config file. " & ex.Message)
                    End Try
                End If
            End Function

        End Class

    End Class

End Namespace
