Imports Library2.Db
Imports Library2.Web.Common
Imports Library2.Code

Public Class ObjectBuilder2
    Private mDbSetings As oDbSettings
    Private mDb As MyDB
    Private mDt As DataTable
    Private mSQL As String = ""
    Private mObjBldr As ObjBuilder2

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Protected Overrides Sub Finalize()
        mDb.Dispose()
        mDbSetings = Nothing
        mDb = Nothing
        mDt = Nothing
        MyBase.Finalize()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tbServer.Text = System.Configuration.ConfigurationManager.AppSettings("SERVER").ToString
        Me.tbUid.Text = System.Configuration.ConfigurationManager.AppSettings("UID").ToString
        Me.tbPwd.Text = System.Configuration.ConfigurationManager.AppSettings("PWD").ToString

        ''test
        'Me.tbServer.Text = "01-web-09"
        'Me.tbUid.Text = "sa"
        'Me.tbPwd.Text = "Password@2"
        'btConnect_Click(Me, Nothing)
        ''test end

    End Sub

    Private Sub btConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btConnect.Click
        Me.ddlTables.Items.Clear()
        Me.ddlTables.Enabled = False
        Me.ddlDB.Items.Clear()
        Me.ddlDB.Enabled = False
        Try

            mDbSetings = New oDbSettings(Me.tbServer.Text, "master", Me.tbUid.Text, Me.tbPwd.Text)
            mDb = New MyDB(mDbSetings)
            mDt = New DataTable
            mSQL = ""
            mSQL = "SELECT [name] FROM sys.databases WHERE [name] not in('master','model','msdb','tempdb')"
            mDb.Begin()
            Try
                mDt = mDb.GetTable(mSQL)
                mDb.Commit()
            Catch ex As Exception
                mDb.RollBack()
            End Try
            If mDt.Rows.Count > 0 Then
                Me.ddlDB.Items.Clear()
                For Each mR As DataRow In mDt.Rows
                    Me.ddlDB.Items.Add(mR.Item(0).ToString)
                Next
                Me.ddlDB.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If mDb Is Nothing Then
                mDb.Dispose()
            End If
            mDb = Nothing
            mDbSetings = Nothing
        End Try
    End Sub

    Private Sub ddlDB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDB.SelectedIndexChanged
        Me.ddlTables.Items.Clear()
        Me.ddlTables.Enabled = False
        If Me.ddlDB.SelectedItem.ToString <> "" Then
            mDbSetings = New oDbSettings(Me.tbServer.Text, Me.ddlDB.SelectedItem.ToString, Me.tbUid.Text, Me.tbPwd.Text)
            mDb = New MyDB(mDbSetings)
            mDt = New DataTable
            mSQL = ""
            Try
                mSQL = "SELECT name FROM sysobjects  where type='U' and name <> 'sysdiagrams' order by name"
                mDb.Begin()
                Try
                    mDt = mDb.GetTable(mSQL)
                    mDb.Commit()
                Catch ex As Exception
                    mDb.RollBack()
                End Try
                If mDt.Rows.Count > 0 Then
                    Me.ddlTables.Items.Clear()
                    For Each mR As DataRow In mDt.Rows
                        Me.ddlTables.Items.Add(mR.Item(0).ToString)
                    Next
                    Me.ddlTables.Enabled = True
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
            End Try
        End If
    End Sub

    Private Sub ddlTables_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTables.SelectedIndexChanged
        Me.btGenerate.Enabled = False
        Me.DataGridView1.DataSource = Nothing
        Try
            If Me.ddlTables.SelectedItem.ToString <> "" Then
                Me.tbXmlDef.Text = GetTabDef(Me.ddlTables.SelectedItem.ToString)
                Me.btGenerate.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub

    Private Function GetTabDef(ByVal pTabName As String) As String
        Dim mList As New List(Of String)
        Dim mPK As New List(Of String)
        mSQL = String.Format("select Column_Name,is_nullable,data_type,character_maximum_length from information_schema.COLUMNS where table_name ='{0}'", pTabName)
        mDb.Begin()
        Try
            'Looking for Identity Columns
            mDt = mDb.GetTable(String.Format("sp_help '{0}'", pTabName), 2)
            If mDt.Rows.Count > 0 Then
                If mDt.Rows(0).Item(0).ToString <> "No identity column defined." Then
                    For Each mR As DataRow In mDt.Rows
                        mList.Add(mR.Item(0).ToString)
                    Next
                End If
            End If

            'Looking for Primary key Column
            Try
                mDt = mDb.GetTable(String.Format("sp_help '{0}'", pTabName), 6)
                If mDt.Rows.Count > 0 Then
                    If mDt.Rows(0).Item(6).ToString <> "" Then
                        For Each mR As DataRow In mDt.Rows
                            mPK.Add(mR.Item(6).ToString)
                        Next
                    End If
                End If
            Catch ex As Exception

            End Try

            mDt = mDb.GetTable(mSQL)
            mDt.TableName = pTabName
            mDt.Columns.Add("SKIP")
            mDt.Columns.Add("PK")
            For Each mR As DataRow In mDt.Rows
                If mList.Count = 0 Then
                    mR.Item("SKIP") = "No"
                Else
                    For Each mS As String In mList
                        If mR.Item(0).ToString.ToLower = mS.ToLower Then
                            mR.Item("SKIP") = "Yes"
                        Else
                            mR.Item("SKIP") = "No"
                        End If
                    Next
                End If

                If mPK.Count = 0 Then
                    mR.Item("PK") = "No"
                Else
                    For Each mS As String In mPK
                        If mR.Item(0).ToString.ToLower = mS.ToLower Then
                            mR.Item("PK") = "Yes"
                        Else
                            mR.Item("PK") = "No"
                        End If
                    Next
                End If

            Next
            mDb.Commit()
        Catch ex As Exception
            mDb.RollBack()
            Throw ex
        End Try
        Return ToXml(mDt)
    End Function

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            If TabControl1.SelectedIndex = 1 Then
                Me.DataGridView1.DataSource = ToDataTable(Me.tbXmlDef.Text)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGenerate.Click
        Try
            mObjBldr = New ObjBuilder2(ToDataTable(Me.tbXmlDef.Text))
            Me.tbCode.Text = mObjBldr.Generate()
            mObjBldr = Nothing
            Me.btCopy.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btB67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btB67.Click
        b67.Show()
    End Sub

    Private Sub btCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCopy.Click
        'Generate DLL
        'http://www.west-wind.com/presentations/dynamicCode/DynamicCode.htm

        Clipboard.Clear()
        Clipboard.SetText(Me.tbCode.Text)
    End Sub


    Private Sub btjScript2StrBldr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btjScript2StrBldr.Click
        Dim mRet As New System.Text.StringBuilder
        Dim mSource As String = Me.tbCode.Text
        If mSource <> "" Then
            mSource = Replace(mSource, Chr(10), "")
            mRet.AppendLine("Dim mRet As New Text.StringBuilder")
            For Each mLine As String In mSource.Split(Chr(13))
                mRet.AppendLine(String.Format("mRet.AppendLine(""{0}"")", Replace(mLine, """", "'")))
            Next
            Me.tbCode.Text = mRet.ToString
            Me.btCopy.Enabled = True
        End If
    End Sub

    Private Sub bthttpReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bthttpReq.Click
        HttpReq.Show()
    End Sub

    Private Sub btSql2Csv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSql2Csv.Click
        SQL2CSV.tbServer.Text = Me.tbServer.Text
        SQL2CSV.tbDB.Text = Me.ddlDB.SelectedItem
        SQL2CSV.tbUID.Text = Me.tbUid.Text
        SQL2CSV.tbPWD.Text = Me.tbPwd.Text
        SQL2CSV.Show()
    End Sub

    Private Sub btVer1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btVer1.Click
        ObjectBuilder.Show()
    End Sub

    Private Sub btRijndael_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRijndael.Click
        Rijndael.Show()
    End Sub

    Private Sub btShell_Click(sender As System.Object, e As System.EventArgs) Handles btShell.Click
        Shell.Show()
    End Sub

    Private Sub btListOfFindByField_Click(sender As System.Object, e As System.EventArgs) Handles btListOfFindByField.Click

        Dim mRet As New System.Text.StringBuilder
        mRet.AppendLine("")
        mRet.AppendLine("    Public Class MyObjCollection")
        mRet.AppendLine("        Inherits List(Of MyObj)")
        mRet.AppendLine("")
        mRet.AppendLine("        Private _Name2Find As String = ''")
        mRet.AppendLine("")
        mRet.AppendLine("        Public Function FindByName(pName As String) As MyObj")
        mRet.AppendLine("            _Name2Find = pName")
        mRet.AppendLine("            Return Me.Find(AddressOf _FindByName)")
        mRet.AppendLine("        End Function")
        mRet.AppendLine("")
        mRet.AppendLine("        Private Function _FindByName(pT As MyObj) As Boolean")
        mRet.AppendLine("            Return pT.Name.Trim.ToLower() = _Name2Find.Trim.ToLower()")
        mRet.AppendLine("        End Function")
        mRet.AppendLine("")
        mRet.AppendLine("    End Class")
        mRet.AppendLine("")
        mRet.AppendLine("    Public Class MyObj")
        mRet.AppendLine("")
        mRet.AppendLine("        Public Sub New(pName As String, pPhone As String)")
        mRet.AppendLine("            Name = pName")
        mRet.AppendLine("            Phone = Phone")
        mRet.AppendLine("        End Sub")
        mRet.AppendLine("")
        mRet.AppendLine("        Private mName As String")
        mRet.AppendLine("        Public Property Name() As String")
        mRet.AppendLine("            Get")
        mRet.AppendLine("                Return mName")
        mRet.AppendLine("            End Get")
        mRet.AppendLine("            Set(ByVal value As String)")
        mRet.AppendLine("                mName = value")
        mRet.AppendLine("            End Set")
        mRet.AppendLine("        End Property")
        mRet.AppendLine("")
        mRet.AppendLine("        Private mPhone As String")
        mRet.AppendLine("        Public Property Phone() As String")
        mRet.AppendLine("            Get")
        mRet.AppendLine("                Return mPhone")
        mRet.AppendLine("            End Get")
        mRet.AppendLine("            Set(ByVal value As String)")
        mRet.AppendLine("                mPhone = value")
        mRet.AppendLine("            End Set")
        mRet.AppendLine("        End Property")
        mRet.AppendLine("")
        mRet.AppendLine("")
        mRet.AppendLine("    End Class")


        Me.tbCode.Text = mRet.ToString
        Me.btCopy.Enabled = True

    End Sub


    Private Sub btThreadGen_Click_1(sender As System.Object, e As System.EventArgs) Handles btThreadGen.Click
        Dim mOB As New ObjBuilder2(Nothing)

        Me.tbCode.Text = mOB.MultiThreadGenerate()
        Me.btCopy.Enabled = True

        mOB = Nothing
    End Sub

End Class

