Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Namespace _Library._Db.__dbsys

    Public Class clsDatabaseRoot
        Inherits _Base.LibraryBase
        Private mConstring As String = ""

        Public Sub New(ByVal ConnectionString As String)
            mConstring = ConnectionString
        End Sub

        Public ReadOnly Property ConnectionString() As String
            Get
                Return mConstring
            End Get
        End Property

        Protected Function ParsTableName(ByVal pSQL) As String
            Dim mRet As String = "RAOOF"
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
        End Function

        Private mPreventSQLinjection As Boolean = False
        Public Property PreventSQLinjection() As Boolean
            Get
                Return mPreventSQLinjection
            End Get
            Set(ByVal value As Boolean)
                mPreventSQLinjection = value
            End Set
        End Property

        Protected Sub CheckSQLinjection(ByVal pSQLcommand As String, Optional ByVal SemiColumnAllowed As Boolean = False)
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
        End Sub


    End Class

End Namespace

Namespace _Library._Db

    Public Class MyDatabase
        Inherits __dbsys.clsDatabaseRoot
        Private mTransaction As _Trans

        Public Sub New(ByVal ConnectionString As String)
            MyBase.new(ConnectionString)
            mTransaction = New _Trans(ConnectionString)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            mTransaction = Nothing
        End Sub

        Public ReadOnly Property Transaction() As _Trans
            Get
                Return mTransaction
            End Get
        End Property

        Public Function GetTable(ByVal SQLcommand As String) As Data.DataTable
            Return GetTable(SQLcommand, 0, 0)
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer) As Data.DataTable
            Dim mConnection As SqlConnection
            Dim mCommand As SqlCommand
            Dim mDa As SqlDataAdapter
            Dim mDataSet As New Data.DataSet
            Dim mDataTable As New Data.DataTable
            Dim TableName As String = ""
            Try
                If SQLcommand.Trim.Length > 0 Then
                    CheckSQLinjection(SQLcommand)

                    mConnection = New SqlConnection(MyBase.ConnectionString)
                    mConnection.Open()
                    mCommand = mConnection.CreateCommand

                    mCommand.CommandType = Data.CommandType.Text
                    mCommand.CommandText = SQLcommand

                    'using System.Data;
                    'using System.Data.SqlClient;

                    'using (SqlConnection connection = new SqlConnection(connectionString))
                    '{
                    '  DataSet userDataset = new DataSet();
                    '  SqlDataAdapter myDataAdapter = new SqlDataAdapter(
                    '         "SELECT au_lname, au_fname FROM Authors WHERE au_id = @au_id", 
                    '         connection);                
                    '  myCommand.SelectCommand.Parameters.Add("@au_id", SqlDbType.VarChar, 11);
                    '  myCommand.SelectCommand.Parameters["@au_id"].Value = SSN.Text;
                    '  myDataAdapter.Fill(userDataset);
                    '}

                    mDa = New SqlDataAdapter
                    mDa.SelectCommand = mCommand
                    TableName = MyBase.ParsTableName(SQLcommand)
                    If TableName = "RAOOF" Then
                        TableName = "T" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond
                    End If
                    If PageSize > 0 And PageNumber > 0 Then
                        mDa.Fill(mDataSet, (PageNumber - 1) * PageSize, PageSize, TableName)
                    Else
                        mDa.Fill(mDataSet, TableName)
                    End If
                    mDataTable = mDataSet.Tables(0)
                    mConnection.Close()

                    mDa.Dispose()
                    mDataSet.Dispose()
                    mCommand.Dispose()
                    mConnection.Dispose()
                End If
            Catch ex As Exception
                Throw ex
            End Try
            '_Select = mDataTable
            Return mDataTable
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, Optional ByVal Set_SQL_DB_Date_Format_To_DD_MM_YYYY_Before_Executing_Query As Boolean = False) As Integer
            Dim mConnection As SqlConnection
            Dim mCommand As SqlCommand
            Dim mSQL As String = "SET DATEFORMAT dmy"
            Dim mRetVal As Integer = False
            Try
                If SQLcommand <> "" Then
                    CheckSQLinjection(SQLcommand)


                    mConnection = New SqlConnection(MyBase.ConnectionString)
                    mConnection.Open()
                    mCommand = mConnection.CreateCommand

                    If Set_SQL_DB_Date_Format_To_DD_MM_YYYY_Before_Executing_Query Then
                        'SET DATE FORMAT to DD/MM/YYYY"
                        mCommand.CommandText = mSQL
                        mCommand.CommandType = Data.CommandType.Text
                        mCommand.ExecuteNonQuery()
                    End If
                    mCommand.CommandText = SQLcommand
                    mCommand.CommandType = Data.CommandType.Text
                    mRetVal = mCommand.ExecuteNonQuery()

                    mConnection.Close()

                    mCommand.Dispose()
                    mConnection.Dispose()

                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function ExecuteScalar(ByVal SQLcommand As String) As Object
            Dim mConnection As SqlConnection
            Dim mCommand As SqlCommand
            Dim mRetVal As Object = Nothing
            Try
                If SQLcommand.Trim.Length > 0 Then
                    CheckSQLinjection(SQLcommand)


                    mConnection = New SqlConnection(MyBase.ConnectionString)
                    mConnection.Open()
                    mCommand = mConnection.CreateCommand

                    mCommand.CommandText = SQLcommand
                    mCommand.CommandType = Data.CommandType.Text
                    mRetVal = mCommand.ExecuteScalar

                    mConnection.Close()

                    mCommand.Dispose()
                    mConnection.Dispose()
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

    End Class

    Public Class _Trans
        Inherits __dbsys.clsDatabaseRoot
        Dim mTrans As SqlTransaction
        Dim mConnection As SqlConnection
        Dim mCommand As SqlCommand

        Public Sub New(ByVal ConnectionString As String)
            MyBase.new(ConnectionString)
        End Sub

        Private Function Open() As Boolean
            Dim mRetVal As Boolean = False
            Try
                If mConnection Is Nothing Then
                    mConnection = New SqlConnection(MyBase.ConnectionString)
                    mConnection.Open()
                    Try
                        mCommand = mConnection.CreateCommand()
                        mRetVal = True
                    Catch ex As Exception
                        mConnection.Close()
                        Throw ex
                    End Try
                Else
                    If Not (mConnection.State = Data.ConnectionState.Open) Then
                        mConnection = New SqlConnection(MyBase.ConnectionString)
                        mConnection.Open()
                        Try
                            mCommand = mConnection.CreateCommand()
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
        End Function

        Public Function BeginTransaction() As Boolean
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
        End Function

        Public Function GetTable(ByVal SQLcommand As String) As Data.DataTable
            Return GetTable(SQLcommand, 0, 0)
        End Function

        Public Function GetTable(ByVal SQLcommand As String, ByVal PageNumber As Integer, ByVal PageSize As Integer) As Data.DataTable
            Dim mDa As SqlDataAdapter
            Dim mDataSet As New Data.DataSet
            Dim mDataTable As New Data.DataTable
            Dim TableName As String = ""
            Try
                If SQLcommand.Trim.Length > 0 Then
                    CheckSQLinjection(SQLcommand)


                    mCommand.CommandType = Data.CommandType.Text
                    mCommand.CommandText = SQLcommand


                    mDa = New SqlDataAdapter
                    mDa.SelectCommand = mCommand
                    TableName = MyBase.ParsTableName(SQLcommand)
                    If TableName = "RAOOF" Then
                        TableName = "T" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond
                    End If
                    If PageSize > 0 And PageNumber > 0 Then
                        mDa.Fill(mDataSet, (PageNumber - 1) * PageSize, PageSize, TableName)
                    Else
                        mDa.Fill(mDataSet, TableName)
                    End If
                    mDataTable = mDataSet.Tables(0)

                    mDa.Dispose()
                    mDataSet.Dispose()
                End If
            Catch ex As Exception
                Throw ex
            End Try
            '_Select = mDataTable
            Return mDataTable
        End Function

        Public Function ExecuteNonQuery(ByVal SQLcommand As String, Optional ByVal Set_SQL_DB_Date_Format_To_DD_MM_YYYY_Before_Executing_Query As Boolean = False) As Integer
            Dim mSQL As String = "SET DATEFORMAT dmy"
            Dim mRetVal As Integer = False
            Try
                If SQLcommand <> "" Then
                    CheckSQLinjection(SQLcommand)


                    If Set_SQL_DB_Date_Format_To_DD_MM_YYYY_Before_Executing_Query Then
                        'SET DATE FORMAT to DD/MM/YYYY"
                        mCommand.CommandText = mSQL
                        mCommand.CommandType = Data.CommandType.Text
                        mCommand.ExecuteNonQuery()
                    End If
                    mCommand.CommandText = SQLcommand
                    mCommand.CommandType = Data.CommandType.Text
                    mRetVal = mCommand.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function ExecuteScalar(ByVal SQLcommand As String) As Object
            Dim mRetVal As Object = Nothing
            Try
                If SQLcommand.Trim.Length > 0 Then
                    CheckSQLinjection(SQLcommand)


                    mCommand.CommandText = SQLcommand
                    mCommand.CommandType = Data.CommandType.Text
                    mRetVal = mCommand.ExecuteScalar

                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function Commit() As Boolean
            Dim mRetVal As Boolean = False
            Try
                mTrans.Commit()
                If Close() Then
                    mRetVal = True
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function RollBack() As Boolean
            Dim mRetVal As Boolean = False
            Try
                mTrans.Rollback()
                If Close() Then
                    mRetVal = True
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Private Function Close() As Boolean
            Dim mRetVal As Boolean = False
            Try
                If mConnection.State = Data.ConnectionState.Open Then
                    mConnection.Close()

                    mCommand.Dispose()
                    mConnection.Dispose()
                    mTrans.Dispose()

                    mRetVal = True
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

    End Class

    Public Class MySql
        Private mTable As String
        Private mFieldInfo As New List(Of SQlField)

        Public Sub New(ByVal Table As String)
            mTable = Table
        End Sub

        Protected Overrides Sub Finalize()
            mFieldInfo = Nothing
            MyBase.Finalize()
        End Sub

        Public Property Table()
            Get
                Return mTable
            End Get
            Set(ByVal value)
                mTable = value
            End Set
        End Property

        Public ReadOnly Property Fields() As List(Of SQlField)
            Get
                Return mFieldInfo
            End Get
        End Property

        Public Function Ins() As String
            Dim mRet As New Text.StringBuilder
            Dim mComma As String = ""
            Dim mQuot As String = ""
            mRet.Append(String.Format("INSERT INTO {0}", Table))
            mRet.Append("(")
            For Each mF As SQlField In Fields
                mRet.Append(mComma & mF.Name)
                mComma = ","
            Next
            mRet.Append(") VALUES(")
            mComma = ""
            For Each mF As SQlField In Fields
                mQuot = IIf(mF.Quot, "'", "")
                mRet.Append(mComma & mQuot & mF.Value & mQuot)
                mComma = ","
            Next
            mRet.Append(") ")
            Return mRet.ToString
        End Function

        Public Function Upd(ByVal WhereCondition As String) As String
            Dim mRet As New Text.StringBuilder
            Dim mComma As String = ""
            Dim mQuot As String = ""
            mRet.Append(String.Format("UPDATE {0} SET ", Table))
            For Each mF As SQlField In Fields
                mQuot = IIf(mF.Quot, "'", "")
                mRet.Append(String.Format("{0}{1}={2}{3}{4}", mComma, mF.Name, mQuot, mF.Value, mQuot))
                mComma = ","
            Next
            mRet.Append(String.Format(" WHERE {0}", WhereCondition))
            Return mRet.ToString
        End Function


        Public Function Del(ByVal WhereCondition As String) As String
            Dim mRet As New Text.StringBuilder
            Dim mComma As String = ""
            Dim mQuot As String = ""
            mRet.Append(String.Format("DELETE FROM {0} ", Table))
            mRet.Append(String.Format(" WHERE {0}", WhereCondition))
            Return mRet.ToString
        End Function

    End Class

    Public Class SQlField
        Private mName As String
        Private mVal As String
        Private mQuot As Boolean = False

        Public Sub New(ByVal Name As String, ByVal Value As String, ByVal NeedQuot As Boolean)
            mName = Name
            mVal = Value
            mQuot = NeedQuot
        End Sub

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal value As String)
                mName = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return mVal
            End Get
            Set(ByVal _value As String)
                mVal = _value
            End Set
        End Property

        Public Property Quot() As Boolean
            Get
                Return mQuot
            End Get
            Set(ByVal value As Boolean)
                mQuot = value
            End Set
        End Property

    End Class


End Namespace
