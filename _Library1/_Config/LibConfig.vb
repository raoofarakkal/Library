'Imports System.IO
'Imports _Library._Web
'Namespace _Library._Config

'    Public Class LibConfig
'        Private Shared mCustCode As String = ""
'        Private Shared mLicenseKey As String = ""
'        Private mConfFile As ConfigFile
'        Private mQSB As QueryStringBuilder

'        Public Sub New()
'            _New()
'        End Sub

'        Public ReadOnly Property QueryStringBuilder() As QueryStringBuilder
'            Get
'                Return mQSB
'            End Get
'        End Property

'        Public Function CustomerCode() As String
'            Return mCustCode
'        End Function

'        Public Function LicenseKey() As String
'            Return mLicenseKey
'        End Function


'        Private Function _New() As Boolean
'            '*HardCoded*
'            Dim mLoc As String
'            Dim mFile As String
'            'Dim mLoc As String = "C:\CONFIG"
'            'Dim mFile As String = "LIBRARY.CFG"
'            Try
'                mLoc = System.Configuration.ConfigurationManager.AppSettings.Item("LIBRARY_CONFIG_FOLDER") '"C:\_AJCMS"
'                mFile = System.Configuration.ConfigurationManager.AppSettings.Item("LIBRARY_CONFIG_FILE") '"AJCMS2.CFG"
'            Catch ex As Exception
'                mLoc = "C:\CONFIG"
'                mFile = "LIBRARY.CFG"
'            End Try

'            mQSB = New QueryStringBuilder
'            mConfFile = New ConfigFile(mLoc, mFile)
'            mConfFile.CreateOrOpen()
'        End Function


'        Private Class ConfigFile
'            Private mConfigFileLocation As String
'            Private mConfigFile As String '= "library.cfg"
'            Private mPro As New _Library._System._Security.Protection.Protection

'            Public Sub New(ByVal ConfigFileLocation As String, ByVal ConfigFileName As String)
'                _new(ConfigFileLocation, ConfigFileName)
'            End Sub

'            Private Function _new(ByVal ConfigFileLocation As String, ByVal ConfigFileName As String) As Boolean
'                If Right(ConfigFileLocation, 1) = "\" Then
'                    mConfigFileLocation = ConfigFileLocation
'                Else
'                    mConfigFileLocation = ConfigFileLocation & "\"
'                End If
'                mConfigFile = ConfigFileName
'            End Function

'            Public Function CreateOrOpen() As Boolean
'                If String.IsNullOrEmpty(mConfigFileLocation) Or String.IsNullOrEmpty(mConfigFile) Then
'                    Throw New Exception("LIBRARY Config File or Location Not found. please check your web config and make sure you have the following configuration on it" & vbCrLf _
'                    & vbCrLf & "<appSettings>" _
'                    & vbCrLf & "	<add key=""LIBRARY_CONFIG_FOLDER"" value=""C:\CONFIG""/> " _
'                    & vbCrLf & "	<add key=""LIBRARY_CONFIG_FILE"" value=""LIBRARY.CFG""/> " _
'                    & vbCrLf & "</appSettings>")

'                End If
'                If Not Directory.Exists(mConfigFileLocation) Then
'                    Directory.CreateDirectory(mConfigFileLocation)
'                End If
'                If Not File.Exists(mConfigFileLocation & "\" & mConfigFile) Then
'                    CreateFile(mConfigFileLocation & "\" & mConfigFile)
'                End If
'                Dim mDs As New Data.DataSet
'                mDs.ReadXml(mConfigFileLocation & "\" & mConfigFile)
'                mCustCode = mDs.Tables(0).Rows(0).Item(0).ToString()
'                mLicenseKey = mDs.Tables(0).Rows(0).Item(1).ToString()
'            End Function

'            Private Function CreateFile(ByVal FilePath As String, Optional ByVal pFree As Boolean = False) As Boolean
'                Dim mDs As New Data.DataSet
'                Dim mDt As New Data.DataTable
'                Dim mDr As Data.DataRow
'                mDt.TableName = "Configuration"
'                mDt.Columns.Add("CUSTOMER_CODE")
'                mDt.Columns.Add("LICENSE_KEY")
'                mDr = mDt.NewRow
'                mDr.Item(0) = mPro.CustomerCode
'                If pFree Then
'                    Dim e As Char = Chr(51) '3
'                    Dim o As Char = Chr(48) '0
'                    Dim i As Char = Chr(49) '1
'                    mDr.Item(1) = mPro.GenerateKey(mPro.CustomerCode, "G" & e & "tM" & e & "Th" & e & "R" & i & "ghtC" & o & "d" & e)
'                Else
'                    mDr.Item(1) = "to obtain this license key please provide above customer code to the library provider"
'                End If
'                mDt.Rows.Add(mDr)
'                mDs.Tables.Add(mDt)
'                mDs.DataSetName = "_LIBRARY"
'                mDs.WriteXml(FilePath)
'                mDr = Nothing
'                mDt.Dispose()
'                mDt = Nothing
'                mDs.Dispose()
'                mDs = Nothing
'            End Function

'            Protected Overrides Sub Finalize()
'                mPro = Nothing
'                MyBase.Finalize()
'            End Sub
'        End Class


'        Protected Overrides Sub Finalize()
'            mQSB = Nothing
'            mConfFile = Nothing
'            MyBase.Finalize()
'        End Sub

'    End Class

'End Namespace
