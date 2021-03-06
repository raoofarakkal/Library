﻿Imports System.Data.SqlClient

'The only difference beween DB2 and 3 is SYNCLOCK

Namespace Library2.Db3

    Public Class oDbSettings
        Inherits MyDbBase.oDbSettings

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal pServer As String, ByVal pDbName As String, ByVal pUID As String, ByVal pPWD As String)
            MyBase.New(pServer, pDbName, pUID, pPWD)
        End Sub

        Public Sub New(pWebConfigDbSectionName As String)
            MyBase.New(pWebConfigDbSectionName)
        End Sub

    End Class

    Public Class ExecuteNonQueryPara
        Inherits MyDbBase.ExecuteNonQueryPara

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal pSET_DATEFORMAT_dmy As Boolean, ByVal pReturnIdentityIncrementValue As Boolean, ByVal pSupportMultipleStatements As Boolean)
            MyBase.New(pSET_DATEFORMAT_dmy, pReturnIdentityIncrementValue, pSupportMultipleStatements)
        End Sub

    End Class

    Public Class PagingInfo
        Inherits MyDbBase.PagingInfo

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal pRecordsPerPage As Integer)
            MyBase.New(pRecordsPerPage)
        End Sub

        Public Sub New(ByVal pRecordsPerPage As Integer, ByVal pCurrentPage As Integer)
            MyBase.New(pRecordsPerPage, pCurrentPage)
        End Sub

    End Class

    Public Class DbSqlParamters
        Inherits MyDbBase.DbSqlParamters
    End Class

    Public NotInheritable Class MyDB
        Inherits _Library._Base.LibraryBase
        Private mDbSettings As oDbSettings
        Private mTrans As SqlTransaction
        Private mConnection As SqlConnection
        Private mCommand As SqlCommand

        Public Sub New(pDbSettings As oDbSettings)
            SyncLock GetType(MyDB)
                mDbSettings = pDbSettings
                If mDbSettings.Server Is Nothing Then
                    Throw New Exception("Server name required")
                End If
                If mDbSettings.DbName Is Nothing Then
                    Throw New Exception("Database name required")
                End If
                If mDbSettings.UID Is Nothing Then
                    Throw New Exception("User name required")
                End If
            End SyncLock
        End Sub

        Public Sub New(pConnectionString As String)
            SyncLock GetType(MyDB)
                mDbSettings = New oDbSettings
                mDbSettings.ConnectionString = pConnectionString
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            SyncLock GetType(MyDB)
                Dispose()
                MyBase.Finalize()
            End SyncLock
        End Sub

        Public Sub Dispose()
            SyncLock GetType(MyDB)
                Try
                    If mTrans IsNot Nothing Then
                        mTrans.Dispose()
                    End If
                Catch ex As Exception

                End Try
                Try
                    If mConnection IsNot Nothing Then
                        mConnection.Dispose()
                    End If
                Catch ex As Exception

                End Try
                Try
                    If mCommand IsNot Nothing Then
                        mCommand.Dispose()
                    End If
                Catch ex As Exception

                End Try
                mDbSettings = Nothing
                mTrans = Nothing
                mConnection = Nothing
                mCommand = Nothing
            End SyncLock
        End Sub

        Public Shared Function EnsureFullTextSearchEnabled(ByVal pDbSettings As oDbSettings) As Integer
            SyncLock GetType(MyDB)
                Dim mConnection As SqlConnection
                Dim mCommand As New SqlCommand
                Dim mSp As String = "exec sp_fulltext_Service 'verify_signature',0"
                Try
                    mConnection = New SqlConnection(pDbSettings.ConnectionString)
                    mConnection.Open()
                    Try
                        mCommand = New SqlCommand
                        mCommand = mConnection.CreateCommand()
                        mCommand.CommandText = mSp
                        mCommand.CommandType = Data.CommandType.Text
                        mCommand.ExecuteNonQuery()
                    Catch ex As Exception
                        mConnection.Close()
                        Throw ex
                    Finally
                        mCommand.Dispose()
                        mCommand = Nothing
                        mConnection.Dispose()
                        mConnection = Nothing
                    End Try
                Catch ex As Exception
                    Throw New Exception("Failed to execute sp_fulltext_Service 'verify_signature',0.  Permission may denied.", ex)
                End Try
            End SyncLock
        End Function

        Private mTimeout As Integer = 0
        Public Property Timeout() As Integer
            Get
                Return mTimeout
            End Get
            Set(ByVal value As Integer)
                mTimeout = value
            End Set
        End Property

        Public ReadOnly Property DbSettings() As oDbSettings
            Get
                Return mDbSettings
            End Get
        End Property

        Private Function ParsTableName(ByVal pSQL) As String
            SyncLock GetType(MyDB)
                Dim mRet As String = ""
                Dim cnt As Integer
                Dim mStr As String = ""
                cnt = InStr(pSQL, "FROM ")
                If cnt > 0 Then
                    mStr = LTrim(Mid(pSQL, cnt + 5)).Split(" ")(0)
                End If
                If mStr <> "" Then
                    mRet = mStr
                End If
                Return mRet
            End SyncLock
        End Function

        Private Function Open() As Boolean
            SyncLock GetType(MyDB)
                Dim mRetVal As Boolean = False
                Try
                    If mConnection Is Nothing Then
                        mConnection = New SqlConnection(DbSettings.ConnectionString)
                        mConnection.Open()
                        Try
                            mCommand = New SqlCommand
                            mCommand = mConnection.CreateCommand()
                            If Timeout > 0 Then
                                mCommand.CommandTimeout = Timeout
                            End If
                            mRetVal = True
                        Catch ex As Exception
                            mConnection.Close()
                            Throw ex
                        End Try
                    Else
                        If Not (mConnection.State = Data.ConnectionState.Open) Then
                            mConnection = New SqlConnection(DbSettings.ConnectionString)
                            mConnection.Open()
                            Try
                                mCommand = New SqlCommand
                                mCommand = mConnection.CreateCommand()
                                If Timeout > 0 Then
                                    mCommand.CommandTimeout = Timeout
                                End If
                                mRetVal = True
                            Catch ex As Exception
                                mConnection.Close()
                                Throw ex
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    mRetVal = False
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Private Function Close() As Boolean
            SyncLock GetType(MyDB)
                Dim mRetVal As Boolean = False
                Try
                    If mConnection.State = Data.ConnectionState.Open Then
                        mConnection.Close()

                        mCommand.Dispose()
                        mConnection.Dispose()
                        'mTrans.Dispose()

                        mCommand = Nothing
                        mConnection = Nothing
                        'mTrans = Nothing

                        mRetVal = True
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Public Function Begin() As Boolean
            SyncLock GetType(MyDB)
                Dim mRetVal As Boolean = False
                Try
                    If Open() Then
                        mTrans = mConnection.BeginTransaction
                        mCommand.Transaction = mTrans
                        mRetVal = True
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Public Function Commit() As Boolean
            SyncLock GetType(MyDB)
                Dim mRetVal As Boolean = False
                Try
                    mTrans.Commit()
                    mTrans.Dispose()
                    mTrans = Nothing
                    If Close() Then
                        mRetVal = True
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Public Function RollBack() As Boolean
            SyncLock GetType(MyDB)
                Dim mRetVal As Boolean = False
                Try
                    mTrans.Rollback()
                    mTrans.Dispose()
                    mTrans = Nothing
                    If Close() Then
                        mRetVal = True
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Private mPrvntSQLinj As Boolean = True
        Public Property PreventSQLinjection() As Boolean
            Get
                Return mPrvntSQLinj
            End Get
            Set(ByVal value As Boolean)
                mPrvntSQLinj = value
            End Set
        End Property

        Private Sub CheckSQL(ByVal pSQLcommand As String, Optional ByVal SemiColumnAllowed As Boolean = False)
            SyncLock GetType(MyDB)
                If PreventSQLinjection Then
                    Dim mSQLst As String = ""
                    Dim sQ As Boolean = False
                    For Each mC As Char In pSQLcommand
                        If mC = "'" Then
                            sQ = Not sQ
                        End If
                        If Not sQ Then
                            If mC <> "'" Then
                                mSQLst += mC
                            End If
                        End If
                    Next
                    If mSQLst <> "" Then
                        If InStr(mSQLst, "--") > 0 Then
                            Throw New Exception("SQL injection: SQL comment found", New Exception(mSQLst))
                        End If
                        If Not SemiColumnAllowed Then
                            If InStr(mSQLst, ";", ) > 0 Then
                                Throw New Exception("SQL injection: semi column found", New Exception(mSQLst))
                            End If
                        End If
                    End If
                End If
            End SyncLock
        End Sub

        Public Function GetTable(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, 0, 0, pSqlParameters, CommandType.Text, 0)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, 0, 0, pSqlParameters, pCommandType, 0)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal ReturnTableIndex As Integer) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, 0, 0, pSqlParameters, CommandType.Text, ReturnTableIndex)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType, ByVal ReturnTableIndex As Integer) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, 0, 0, pSqlParameters, pCommandType, ReturnTableIndex)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, PageNumber, PageSize, pSqlParameters, CommandType.Text, 0)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, PageNumber, PageSize, pSqlParameters, pCommandType, 0)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal ReturnTableIndex As Integer) As Data.DataTable
            SyncLock GetType(MyDB)
                Return GetTable(SQLcommand, PageNumber, PageSize, pSqlParameters, CommandType.Text, ReturnTableIndex)
            End SyncLock
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType, ByVal ReturnTableIndex As Integer) As Data.DataTable
            SyncLock GetType(MyDB)
                CheckSQL(SQLcommand, False)
                Return _GetTable(SQLcommand, PageNumber, PageSize, pSqlParameters, pCommandType, ReturnTableIndex)
            End SyncLock
        End Function

        'Private Function ApplySQlCommandProps(ByVal SQLcommand As String, ByVal pCommandType As System.Data.CommandType, ByRef pSqlParameters As List(Of System.Data.SqlClient.SqlParameter))
        '    SyncLock GetType(MyDB)
        '        mCommand.CommandText = ParseSqlParameters(pSqlParameters) & " " & SQLcommand
        '        mCommand.CommandType = pCommandType
        '        'If pSqlParameters IsNot Nothing Then
        '        '    SyncLock mCommand.GetType()
        '        '        Try
        '        '            mCommand.Parameters.Clear()

        '        '        Catch ex As Exception
        '        '            Throw ex
        '        '        End Try
        '        '        Try
        '        '            mCommand.Parameters.AddRange(pSqlParameters.ToArray())
        '        '        Catch ex As Exception
        '        '            Throw ex
        '        '        End Try
        '        '    End SyncLock
        '        'End If
        '    End SyncLock
        'End Function

        Private Function ParseSqlParameters(ByRef pSqlParameters As List(Of System.Data.SqlClient.SqlParameter))
            SyncLock GetType(MyDB)
                Dim mRet As String = ""
                If pSqlParameters IsNot Nothing Then
                    For Each mSqlpara As System.Data.SqlClient.SqlParameter In pSqlParameters
                        If mSqlpara IsNot Nothing Then
                            If mSqlpara.SqlDbType.ToString().ToLower().Contains("char") Then
                                mRet += String.Format(" Declare {0} {1}({2}) set {0} = '{3}'; ", mSqlpara.ParameterName, mSqlpara.SqlDbType.ToString(), mSqlpara.Size, mSqlpara.Value)
                            Else
                                mRet += String.Format(" Declare {0} {1} set {0} = {2}; ", mSqlpara.ParameterName, mSqlpara.SqlDbType.ToString(), mSqlpara.Value)
                            End If
                        End If
                    Next
                End If
                Return mRet
            End SyncLock
        End Function

        Private Function _GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer, ByRef pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType, ByVal ReturnTableIndex As Integer) As Data.DataTable
            SyncLock GetType(MyDB)
                Dim mDa As SqlDataAdapter
                Dim mDataSet As New Data.DataSet
                Dim mDataTable As New Data.DataTable
                Dim TableName As String = ""
                Try
                    If mCommand Is Nothing Then
                        Open() 'Begin()
                        Try
                            mDataTable = _GetTable(SQLcommand, PageNumber, PageSize, pSqlParameters, pCommandType, ReturnTableIndex)
                            Close() 'Commit()
                        Catch ex1 As Exception
                            Close() 'RollBack()
                            Throw ex1
                        End Try
                    Else
                        If SQLcommand.Trim.Length > 0 Then
                            'ApplySQlCommandProps(SQLcommand, pCommandType, pSqlParameters)
                            With mCommand
                                .CommandText = ParseSqlParameters(pSqlParameters) & SQLcommand
                                .CommandType = pCommandType
                                'If pSqlParameters IsNot Nothing Then
                                '    SyncLock GetType(SqlCommand)
                                '        .Parameters.Clear()
                                '        .Parameters.AddRange(pSqlParameters.ToArray())
                                '    End SyncLock
                                'End If
                            End With

                            mDa = New SqlDataAdapter
                            mDa.SelectCommand = mCommand

                            TableName = ParsTableName(SQLcommand)
                            If TableName = "" Then
                                TableName = "T" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond
                            End If
                            If PageSize > 0 And PageNumber > 0 Then
                                mDa.Fill(mDataSet, (PageNumber - 1) * PageSize, PageSize, TableName)
                            Else
                                mDa.Fill(mDataSet, TableName)
                            End If
                            mDataTable = mDataSet.Tables(ReturnTableIndex)

                            mDa.Dispose()
                            mDataSet.Dispose()

                        End If
                    End If
                Catch ex2 As Exception
                    Throw ex2
                End Try
                Return mDataTable
            End SyncLock
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As Integer
            SyncLock GetType(MyDB)
                Return ExecuteNonQuery(SQLcommand, New ExecuteNonQueryPara, pSqlParameters, CommandType.Text)
            End SyncLock
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Integer
            SyncLock GetType(MyDB)
                Return ExecuteNonQuery(SQLcommand, New ExecuteNonQueryPara, pSqlParameters, pCommandType)
            End SyncLock
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, ByVal pPara As ExecuteNonQueryPara, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As Integer
            SyncLock GetType(MyDB)
                Return ExecuteNonQuery(SQLcommand, pPara, pSqlParameters, CommandType.Text)
            End SyncLock
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, ByVal pPara As ExecuteNonQueryPara, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Integer
            SyncLock GetType(MyDB)
                CheckSQL(SQLcommand, pPara.SupportMultipleStatements)
                Return _ExecuteNonQuery(SQLcommand, pPara, pSqlParameters, pCommandType)
            End SyncLock
        End Function

        Private Function _ExecuteNonQuery(ByVal SQLcommand As String, ByVal pPara As ExecuteNonQueryPara, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Integer
            SyncLock GetType(MyDB)
                Dim mSQLdmy As String = "SET DATEFORMAT dmy"
                Dim mRetVal As Integer = False
                Try
                    If SQLcommand <> "" Then
                        If mCommand Is Nothing Then
                            Open() 'Begin()
                            Try
                                mRetVal = _ExecuteNonQuery(SQLcommand, pPara, pSqlParameters, pCommandType)
                                Close() 'Commit()
                            Catch ex1 As Exception
                                Close() 'RollBack()
                                Throw ex1
                            End Try
                        Else
                            With mCommand
                                If pPara.SET_DATEFORMAT_dmy Then
                                    'SET DATE FORMAT to DD/MM/YYYY"
                                    .CommandText = mSQLdmy
                                    .CommandType = pCommandType
                                    .ExecuteNonQuery()
                                End If
                                If pPara.ReturnIdentityIncrementValue Then
                                    .CommandText = ParseSqlParameters(pSqlParameters) & SQLcommand & ";SELECT SCOPE_IDENTITY();"
                                    .CommandType = pCommandType

                                    'If pSqlParameters IsNot Nothing Then
                                    '    .Parameters.Clear()
                                    '    .Parameters.AddRange(pSqlParameters.ToArray)
                                    'End If

                                    mRetVal = .ExecuteScalar
                                Else
                                    .CommandText = ParseSqlParameters(pSqlParameters) & SQLcommand
                                    .CommandType = pCommandType
                                    'If pSqlParameters IsNot Nothing Then
                                    '    .Parameters.Clear()
                                    '    .Parameters.AddRange(pSqlParameters.ToArray)
                                    'End If
                                    mRetVal = .ExecuteNonQuery()
                                End If

                            End With
                        End If
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Public Function ExecuteScalar(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As Object
            SyncLock GetType(MyDB)
                Return ExecuteScalar(SQLcommand, False, pSqlParameters, CommandType.Text)
            End SyncLock
        End Function

        Public Function ExecuteScalar(ByVal SQLcommand As String, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Object
            SyncLock GetType(MyDB)
                Return ExecuteScalar(SQLcommand, False, pSqlParameters, pCommandType)
            End SyncLock
        End Function

        Public Function ExecuteScalar(ByVal SQLcommand As String, ByVal isMultipleStatement As Boolean, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Object
            SyncLock GetType(MyDB)
                CheckSQL(SQLcommand, isMultipleStatement)
                Return _ExecuteScalar(SQLcommand, isMultipleStatement, pSqlParameters, pCommandType)
            End SyncLock
        End Function

        Private Function _ExecuteScalar(ByVal SQLcommand As String, ByVal isMultipleStatement As Boolean, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As Object
            SyncLock GetType(MyDB)
                Dim mRetVal As Object = Nothing
                Try

                    If SQLcommand.Trim.Length > 0 Then
                        If mCommand Is Nothing Then
                            Open() 'Begin()
                            Try
                                mRetVal = _ExecuteScalar(SQLcommand, isMultipleStatement, pSqlParameters, pCommandType)
                                Close() 'Commit()
                            Catch ex1 As Exception
                                Close() 'RollBack()
                                Throw ex1
                            End Try
                        Else
                            With mCommand
                                .CommandText = ParseSqlParameters(pSqlParameters) & SQLcommand
                                .CommandType = pCommandType
                                'If pSqlParameters IsNot Nothing Then
                                '    .Parameters.Clear()
                                '    .Parameters.AddRange(pSqlParameters.ToArray)
                                'End If
                                mRetVal = .ExecuteScalar

                            End With
                        End If
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

        Public Function ExecuteReader(ByVal SQLcommand As String, ByVal pBehavior As System.Data.CommandBehavior, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter)) As System.Data.SqlClient.SqlDataReader
            SyncLock GetType(MyDB)
                Return ExecuteReader(SQLcommand, pBehavior, pSqlParameters, CommandType.Text)
            End SyncLock
        End Function

        Public Function ExecuteReader(ByVal SQLcommand As String, ByVal pBehavior As System.Data.CommandBehavior, ByVal pSqlParameters As List(Of System.Data.SqlClient.SqlParameter), ByVal pCommandType As System.Data.CommandType) As System.Data.SqlClient.SqlDataReader
            SyncLock GetType(MyDB)
                Dim mRetVal As System.Data.SqlClient.SqlDataReader = Nothing
                Try
                    If SQLcommand.Trim.Length > 0 Then
                        If mCommand Is Nothing Then
                            Open() 'Begin()
                            Try
                                mRetVal = ExecuteReader(SQLcommand, pBehavior, pSqlParameters, pCommandType)
                                Close() 'Commit()
                            Catch ex1 As Exception
                                Close() 'RollBack()
                                Throw ex1
                            End Try
                        Else
                            With mCommand
                                .CommandText = ParseSqlParameters(pSqlParameters) & SQLcommand
                                .CommandType = pCommandType
                                'If pSqlParameters IsNot Nothing Then
                                '    .Parameters.Clear()
                                '    .Parameters.AddRange(pSqlParameters.ToArray)
                                'End If
                                mRetVal = .ExecuteReader(pBehavior)
                            End With
                        End If
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRetVal
            End SyncLock
        End Function

    End Class



End Namespace