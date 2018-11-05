Imports System.Collections.Specialized
Imports System.Data.SqlClient

Namespace Library2.MyDbBase

    Public Class oDbSettings

        Private _RjKey2DecryptPwdKey As String = (New Library2.LibConfig()).RjKey2DecryptPwdKey()
        Private _b67Key2DecryptPwdKey As String = (New Library2.LibConfig()).b67Key2DecryptPwdKey()

        Public Sub New()

        End Sub

        Public Sub New(ByVal pServer As String, ByVal pDbName As String, ByVal pUID As String, ByVal pPWD As String)
            Server = pServer
            DbName = pDbName
            UID = pUID
            PWD = pPWD
        End Sub

        Public Sub New(pWebConfigDbSectionName As String)
            _New(System.Configuration.ConfigurationManager.GetSection(pWebConfigDbSectionName), pWebConfigDbSectionName)
        End Sub

        Private Sub _New(pSections As NameValueCollection, pSectionName As String)
            Server = pSections.Get("server")
            DbName = pSections.Get("database")
            UID = pSections.Get("uid")
            PwdEncMethod = pSections.Get("pwdencmethod")
            If String.IsNullOrWhiteSpace(PwdEncMethod) Then
                Throw New Exception(String.Format("PwdEncMethod element not found on section {0}. required values are Rijndael or b67. Any value other than b67 considered as Rijndael", pSectionName))
            End If
            PwdEncKey = pSections.Get("pwdenckey")
            If String.IsNullOrWhiteSpace(PwdEncKey) Then
                Throw New Exception(String.Format("PwdEncKey element not found on section {0}. Rijndael encrypted or b67 encoded string expected", pSectionName))
            End If
            If PwdEncMethod.ToLower().Trim() = "b67" Then
                Try
                    PwdEncKey = (New _Library._System._Security.Protection.Protection).FromBase67(PwdEncKey, _b67Key2DecryptPwdKey)
                Catch ex As Exception
                    Throw New Exception("Failed to decode b67 PwdEncKey. " & ex.Message)
                End Try
            Else
                Try
                    PwdEncKey = Library2.Sys.Security.Rijndael.Decrypt(PwdEncKey, _RjKey2DecryptPwdKey)
                Catch ex As Exception
                    Throw New Exception("Failed to decode Rijndael PwdEncKey. " & ex.Message)
                End Try
            End If
            PWD = pSections.Get("pwd")
            If PwdEncMethod.ToLower().Trim() = "b67" Then
                Try
                    PWD = (New _Library._System._Security.Protection.Protection).FromBase67(PWD, PwdEncKey)
                Catch ex As Exception
                    Throw New Exception("Failed to decode b67 PWD. " & ex.Message)
                End Try
            Else
                Try
                    PWD = Library2.Sys.Security.Rijndael.Decrypt(PWD, PwdEncKey)
                Catch ex As Exception
                    Throw New Exception("Failed to decode Rijndael PWD. " & ex.Message)
                End Try
            End If

            'For Each mTag As String In mSections
            '    Select Case mTag.ToLower
            '        Case "server"
            '            Server = mSections.Get(mTag)
            '        Case "database"
            '            DbName = mSections.Get(mTag)
            '        Case "uid"
            '            UID = mSections.Get(mTag)
            '        Case "pwdenckey"
            '            PwdEncKey = mSections.Get(mTag)
            '        Case "pwd"
            '            If Not String.IsNullOrWhiteSpace(pPasswordDecryptKey) Then
            '                Dim mKey As String = ""
            '                If Not String.IsNullOrWhiteSpace(pKey2DecryptFirstKey) Then
            '                    Try
            '                        mKey = Library2.Sys.Security.Rijndael.Decrypt(pPasswordDecryptKey, pKey2DecryptFirstKey)
            '                    Catch ex As Exception
            '                        mKey = (New _Library._System._Security.Protection.Protection).FromBase67(pPasswordDecryptKey, pKey2DecryptFirstKey)
            '                    End Try
            '                Else
            '                    mKey = pPasswordDecryptKey
            '                End If
            '                Try
            '                    PWD = Library2.Sys.Security.Rijndael.Decrypt(mSections.Get(mTag), mKey)
            '                Catch ex As Exception
            '                    PWD = (New _Library._System._Security.Protection.Protection).FromBase67(mSections.Get(mTag), mKey)
            '                End Try
            '            Else
            '                PWD = mSections.Get(mTag)
            '            End If
            '    End Select
            'Next
        End Sub

        Public Property Server() As String

        Public Property DbName() As String

        Public Property UID() As String

        Public Property PWD() As String

        Public Property PwdEncKey() As String

        Public Property PwdEncMethod() As String

        Public Property PersistSecurityInfo() As String

        Public Property Additionalinfo() As String

        Dim mConStr As String = ""
        Public Property ConnectionString() As String
            Get
                If String.IsNullOrEmpty(mConStr) Then
                    mConStr = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Server, DbName, UID, PWD)
                    If PersistSecurityInfo <> "" Then
                        mConStr = String.Format("{0};Persist Security Info={1}", mConStr, PersistSecurityInfo)
                    End If
                    If Additionalinfo <> "" Then
                        mConStr = String.Format("{0};{1}", mConStr, Additionalinfo)
                    End If
                End If
                Return mConStr
            End Get
            Set(value As String)
                mConStr = value
            End Set

        End Property


#Region "Generate Db Section and Verify"

        'before calling generateDbSection() need create object by New(pServer, pDbName, pUID, pPWD) 
        Public Function generateDbSection(pDbSectionName As String, pPwdEncKey As String, pUseB67 As Boolean) As String
            Dim mRet As New Text.StringBuilder
            Dim mPwd As String = ""
            Dim mPwdEncKey As String = ""
            Dim mPwsEncMethod As String = "Rijndael"
            If pUseB67 Then
                mPwsEncMethod = "b67"
                mPwdEncKey = (New _Library._System._Security.Protection.Protection).ToBase67(pPwdEncKey, _b67Key2DecryptPwdKey)
                mPwd = (New _Library._System._Security.Protection.Protection).ToBase67(PWD, pPwdEncKey)
            Else
                mPwsEncMethod = "Rijndael"
                mPwdEncKey = Library2.Sys.Security.Rijndael.Encrypt(pPwdEncKey, _RjKey2DecryptPwdKey)
                mPwd = Library2.Sys.Security.Rijndael.Encrypt(PWD, pPwdEncKey)
            End If
            mRet.AppendLine(String.Format("<{0}>", pDbSectionName))
            mRet.AppendLine(String.Format("    <add key=""Server"" value=""{0}"" />", Server))
            mRet.AppendLine(String.Format("    <add key=""Database"" value=""{0}"" />", DbName))
            mRet.AppendLine(String.Format("    <add key=""Uid"" value=""{0}"" />", UID))
            mRet.AppendLine(String.Format("    <add key=""Pwd"" value=""{0}"" />", mPwd))
            mRet.AppendLine(String.Format("    <add key=""PwdEncKey"" value=""{0}"" />", mPwdEncKey))
            mRet.AppendLine(String.Format("    <!--PwdEncMethod:Rijndael | b67-->"))
            mRet.AppendLine(String.Format("    <add key=""PwdEncMethod"" value=""{0}"" />", mPwsEncMethod))
            mRet.AppendLine(String.Format("</{0}>", pDbSectionName))
            Return mRet.ToString()
        End Function

        Public Shared Function verifyDbSection(pDbSectionString As String, pSectionName As String) As oDbSettings
            Dim mRet As New oDbSettings()
            Dim mSXML As New Xml.XmlDocument()
            mSXML.LoadXml(pDbSectionString)
            mRet._New(GetSection(mSXML, pSectionName), pSectionName)
            Return mRet
        End Function

        Private Shared Function GetSection(pXml As Xml.XmlDocument, pSectionName As String) As NameValueCollection
            Dim mRet As NameValueCollection = Nothing
            Dim mSect As Xml.XmlNodeList
            Dim mName, mValue As String
            Try
                If pXml IsNot Nothing Then
                    mRet = New NameValueCollection
                    mSect = pXml.GetElementsByTagName(pSectionName)
                    If mSect.Count > 0 Then
                        For Each mKeyVal As Xml.XmlNode In mSect(0).ChildNodes
                            If mKeyVal.LocalName = "add" Then
                                mName = ""
                                mValue = ""
                                For i As Integer = 0 To mKeyVal.Attributes.Count - 1
                                    If mKeyVal.Attributes(i).Name.ToLower.Trim = "key" Then
                                        mName = mKeyVal.Attributes(i).Value
                                    ElseIf mKeyVal.Attributes(i).Name.ToLower.Trim = "value" Then
                                        mValue = mKeyVal.Attributes(i).Value
                                    End If
                                Next
                                mRet.Add(mName, mValue)
                            End If
                        Next
                    End If
                    mSect = Nothing
                End If
            Catch ex As Exception
                Throw
            Finally
                mSect = Nothing
            End Try
            Return mRet
        End Function

#End Region


    End Class

    Public Class ExecuteNonQueryPara

        Public Sub New()

        End Sub

        Public Sub New(ByVal pSET_DATEFORMAT_dmy As Boolean, ByVal pReturnIdentityIncrementValue As Boolean, ByVal pSupportMultipleStatements As Boolean)
            SET_DATEFORMAT_dmy = pSET_DATEFORMAT_dmy
            ReturnIdentityIncrementValue = pReturnIdentityIncrementValue
            SupportMultipleStatements = pSupportMultipleStatements
        End Sub

        Public Property SET_DATEFORMAT_dmy() As Boolean = False

        Public Property ReturnIdentityIncrementValue() As Boolean = False

        Public Property SupportMultipleStatements() As Boolean = False

    End Class

    Public Class PagingInfo

        Public Sub New()

        End Sub

        Public Sub New(ByVal pRecordsPerPage As Integer)
            RecordsPerPage = pRecordsPerPage
        End Sub

        Public Sub New(ByVal pRecordsPerPage As Integer, ByVal pCurrentPage As Integer)
            RecordsPerPage = pRecordsPerPage
            CurrentPage = pCurrentPage
        End Sub

        Public Property TotalRecordsAvailable() As Long = 0

        Public Property RecordsPerPage() As Integer = 10

        Public Property TotalPageAvailable() As Integer = 0

        Private mCurPage As Long = 1
        Public Property CurrentPage() As Long
            Get
                If mCurPage <= 0 Then
                    mCurPage = 1
                End If
                Return mCurPage
            End Get
            Set(ByVal value As Long)
                mCurPage = value
            End Set
        End Property


    End Class

    Public Class DbSqlParamters
        Inherits List(Of System.Data.SqlClient.SqlParameter)

        Public Shared Function SqlParameter(pName As String, pType As SqlDbType, pValue As Object) As System.Data.SqlClient.SqlParameter
            Dim mRet As New System.Data.SqlClient.SqlParameter(pName, pType)
            mRet.Value = pValue
            Return mRet
        End Function

        Public Shared Function CloneSqlParameters(pSqlParameters As List(Of System.Data.SqlClient.SqlParameter))
            If pSqlParameters IsNot Nothing Then
                Dim mSqlParas As New List(Of System.Data.SqlClient.SqlParameter)
                For Each mSp As SqlClient.SqlParameter In pSqlParameters
                    mSqlParas.Add(CloneSqlParameter(mSp))
                Next
                Return mSqlParas
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function CloneSqlParameter(pSqlParameter As System.Data.SqlClient.SqlParameter)
            If pSqlParameter IsNot Nothing Then
                Dim mRet As New SqlParameter() 'pSqlParameter.ParameterName, pSqlParameter.DbType, pSqlParameter.Size)
                mRet.DbType = pSqlParameter.DbType
                mRet.Direction = pSqlParameter.Direction
                mRet.IsNullable = pSqlParameter.IsNullable
                mRet.ParameterName = pSqlParameter.ParameterName
                mRet.Precision = pSqlParameter.Precision
                mRet.Size = pSqlParameter.Size
                mRet.SourceColumn = pSqlParameter.SourceColumn
                mRet.SourceColumnNullMapping = pSqlParameter.SourceColumnNullMapping
                mRet.SourceVersion = pSqlParameter.SourceVersion
                mRet.Value = pSqlParameter.Value
                mRet.XmlSchemaCollectionDatabase = pSqlParameter.XmlSchemaCollectionDatabase
                mRet.XmlSchemaCollectionName = pSqlParameter.XmlSchemaCollectionName
                mRet.XmlSchemaCollectionOwningSchema = pSqlParameter.XmlSchemaCollectionOwningSchema
                Return mRet
            Else
                Return Nothing
            End If
        End Function


    End Class

End Namespace
