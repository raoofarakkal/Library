'Imports System
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.ComponentModel
'Imports System.Drawing.Design
'Imports System.Text
'Imports System.Web
'Imports System.Web.UI
'Imports System.Web.UI.Page
'Imports System.Security.Permissions
'Imports System.Web.UI.WebControls

'Namespace _Library._Web.FlexGrid
'    '1) Pressing tab at the end of a row should automatically add a new row without using mouse

'    '2) While filling records in flexgrid from collection, after inserting data in flexgrid, the corresponding data should be loaded in combobox also,but its not working

'    'For eg if we r loading a record "1  Anjana   Nationality="Indian" and here if nationality is shown as a dropdown box , then loading Anjana's record should show Nationality as indian in dropdown box

'    '3) Adding Formula in a particular cell loads the grid with "Values" infinity or zero as default, nothing should be thr instead of this values

'    '4) After Changing name of the flexgrid and inserting columns inside its collection, its autopostback will not work

'    '5) InFlexGrid->need the  changeEvent of a single cell, its cellValue required ,now getting only CellCaption.


'    Public Class FlexGridSource
'        Dim mRows As New FlexGridRows

'        Public ReadOnly Property Rows() As FlexGridRows
'            Get
'                Return mRows
'            End Get
'        End Property

'    End Class

'    Public Class FlexGridRows
'        Inherits List(Of FlexGridRow)

'        Overloads ReadOnly Property Item(ByVal index) As FlexGridRow
'            Get
'                Return MyBase.Item(index)
'            End Get
'        End Property


'    End Class

'    Public Class FlexGridUpdRows
'        Inherits List(Of FlexGridUpdRow)

'        Overloads ReadOnly Property Item(ByVal index) As FlexGridUpdRow
'            Get
'                Return MyBase.Item(index)
'            End Get
'        End Property

'    End Class

'    Public Class FlexGridUpdRow
'        Inherits FlexGridRow
'        Dim mPkCold As String

'        Public Sub New(ByRef Grid As FlexGrid)
'            MyBase.New(Grid)
'        End Sub

'        Public Property OldPrimarykeyCol() As String
'            Get
'                Return mPkCold
'            End Get
'            Friend Set(ByVal value As String)
'                mPkCold = value
'            End Set
'        End Property

'    End Class

'    Public Class FlexGridRow
'        Dim mCol As FlexGridColumns

'        Public Sub New(ByRef Grid As FlexGrid)
'            mCol = New FlexGridColumns(Grid)
'        End Sub

'        Default ReadOnly Property Item(ByVal index As Integer) As FlexGridColumn
'            Get
'                Return mCol(index)
'            End Get
'        End Property

'        Default ReadOnly Property Item(ByVal Title As String) As FlexGridColumn
'            Get
'                Return mCol(Title)
'            End Get
'        End Property

'        Public ReadOnly Property Columns() As FlexGridColumns
'            Get
'                Return mCol
'            End Get
'        End Property

'        Protected Overrides Sub Finalize()
'            mCol = Nothing
'            MyBase.Finalize()
'        End Sub

'    End Class

'    Public Class FlexGridColumns
'        Dim mFgCols As List(Of FlexGridColumn)

'        Public Sub New(ByRef Grid As FlexGrid)
'            mFgCols = New List(Of FlexGridColumn)
'            For Each mCol As FgColumn In Grid.Columns
'                Dim mFgCol As New FlexGridColumn
'                mFgCol.Cell = New FlexGridCell
'                mFgCol.Title = mCol.Caption
'                mFgCol.Type = mCol.ColumnType
'                mFgCols.Add(mFgCol)
'            Next
'        End Sub

'        Public ReadOnly Property Count() As Integer
'            Get
'                Return mFgCols.Count
'            End Get
'        End Property

'        Default Public ReadOnly Property Item(ByVal pTitle As String) As FlexGridColumn
'            Get
'                Dim mRetIndex As Integer = -1
'                For i As Integer = 0 To mFgCols.Count - 1
'                    If mFgCols.Item(i).Title.ToLower = pTitle.ToLower Then
'                        mRetIndex = i
'                    End If
'                Next
'                If mRetIndex <> -1 Then
'                    Return mFgCols.Item(mRetIndex)
'                Else
'                    Throw New Exception("Title not found")
'                End If
'            End Get
'        End Property

'        Default Public ReadOnly Property Item(ByVal index As Integer) As FlexGridColumn
'            Get
'                If index >= 0 And index < mFgCols.Count Then
'                    Return mFgCols.Item(index)
'                Else
'                    Throw New Exception("index out of range")
'                End If
'            End Get
'        End Property

'        Protected Overrides Sub Finalize()
'            mFgCols = Nothing
'            MyBase.Finalize()
'        End Sub

'    End Class

'    Public Class FlexGridCell
'        Dim mText As String = ""
'        Dim mValue As String = ""

'        Public Property Text() As String
'            Get
'                Return mText
'            End Get
'            Set(ByVal value As String)
'                mText = value
'            End Set
'        End Property

'        Public Property Value() As String
'            Get
'                Return mValue
'            End Get
'            Friend Set(ByVal value As String)
'                mValue = value
'            End Set
'        End Property

'    End Class

'    Public Class FlexGridColumn
'        Dim mCell As New FlexGridCell
'        Dim mCellVal As String = ""
'        Dim mTitle As String = ""
'        Dim mType As New ColumnTypes

'        Public Property Cell() As FlexGridCell
'            Get
'                Return mCell
'            End Get
'            Set(ByVal value As FlexGridCell)
'                mCell = value
'            End Set
'        End Property

'        Public Property Title() As String
'            Get
'                Return mTitle
'            End Get
'            Friend Set(ByVal value As String)
'                mTitle = value
'            End Set
'        End Property

'        Public Property Type() As ColumnTypes
'            Get
'                Return mType
'            End Get
'            Friend Set(ByVal value As ColumnTypes)
'                mType = value
'            End Set
'        End Property

'    End Class

'    <Serializable()> _
'    Public Class FlexGridChangeSets
'        Inherits List(Of FlexGridChangeSet)
'    End Class

'    <Serializable()> _
'    Public Class FlexGridChangeSet
'        Dim mCell As String
'        Dim mCellVal As String
'        Dim mRow As Integer
'        Dim mCol As Integer

'        Public Property Text() As String
'            Get
'                Return mCell
'            End Get
'            Friend Set(ByVal value As String)
'                mCell = value
'            End Set
'        End Property

'        Public Property Value() As String
'            Get
'                Return mCellVal
'            End Get
'            Friend Set(ByVal value As String)
'                mCellVal = value
'            End Set
'        End Property

'        Public Property Row() As Integer
'            Get
'                Return mRow
'            End Get
'            Friend Set(ByVal value As Integer)
'                mRow = value
'            End Set
'        End Property

'        Public Property Col() As Integer
'            Get
'                Return mCol
'            End Get
'            Friend Set(ByVal value As Integer)
'                mCol = value
'            End Set
'        End Property

'    End Class

'    <DefaultEvent("AutoPostback_OnChange"), ToolboxData("<{0}:FlexGrid runat=server></{0}:FlexGrid>")> _
'    Public Class FlexGrid
'        Inherits WebControl
'        'Private mRws As Integer = 5 'Design mode onlny
'        Private mDataSource As FlexGridSource
'        Private mDataSourceOrg As FlexGridSource
'        Private mColumns As List(Of FgColumn)
'        Private mSetFocus As HtmlControls.HtmlGenericControl
'        Private NewRowAdded As Boolean = False
'        Public Event AutoPostback_OnChange(ByVal Changes As FlexGridChangeSet)
'        Public Event onFormulaError(ByVal pFormula As String, ByVal Col As Integer, ByVal Row As Integer, ByVal ex As Exception)

'#Region " Hidding Base Properties "

'        <Browsable(False)> _
'        Public Overrides Property AccessKey() As String
'            Get
'                Return MyBase.AccessKey
'            End Get
'            Set(ByVal value As String)
'                'MyBase.AccessKey = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property TabIndex() As Short
'            Get
'                Return MyBase.TabIndex
'            End Get
'            Set(ByVal value As Short)
'                'MyBase.TabIndex = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BackColor() As System.Drawing.Color
'            Get
'                Return MyBase.BackColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                'MyBase.BackColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
'            Get
'                Return MyBase.BorderStyle
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
'                'MyBase.BorderStyle = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
'            Get
'                Return Nothing ' MyBase.Font
'            End Get
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ForeColor() As System.Drawing.Color
'            Get
'                Return MyBase.ForeColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                'MyBase.ForeColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property EnableTheming() As Boolean
'            Get
'                Return MyBase.EnableTheming
'            End Get
'            Set(ByVal value As Boolean)
'                'MyBase.EnableTheming = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property SkinID() As String
'            Get
'                Return MyBase.SkinID
'            End Get
'            Set(ByVal value As String)
'                'MyBase.SkinID = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ToolTip() As String
'            Get
'                Return MyBase.ToolTip
'            End Get
'            Set(ByVal value As String)
'                'MyBase.ToolTip = value
'            End Set
'        End Property

'        '<Browsable(True)> _
'        'Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
'        '    Get
'        '        Return MyBase.BorderWidth
'        '    End Get
'        '    Set(ByVal value As System.Web.UI.WebControls.Unit)
'        '        'MyBase.BorderWidth = value
'        '    End Set
'        'End Property

'        '<Browsable(False)> _
'        'Public Overrides Property Height() As System.Web.UI.WebControls.Unit
'        '    Get
'        '        Return MyBase.Height
'        '    End Get
'        '    Set(ByVal value As System.Web.UI.WebControls.Unit)
'        '        'MyBase.Height = value
'        '    End Set
'        'End Property

'        '<Browsable(True)> _
'        'Public Overrides Property BorderColor() As System.Drawing.Color
'        '    Get
'        '        Return MyBase.BorderColor
'        '    End Get
'        '    Set(ByVal value As System.Drawing.Color)
'        '        MyBase.BorderColor = value
'        '    End Set
'        'End Property

'#End Region

'        Public Sub New()
'            mDataSource = New FlexGridSource
'            mColumns = New List(Of FgColumn)
'        End Sub

'        Protected Overrides Sub Finalize()
'            mColumns = Nothing
'            mDataSource = Nothing
'            MyBase.Finalize()
'        End Sub

'        Private Sub FlexGrid_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
'            If Not Me.DesignMode Then
'                InitGridData()
'            End If
'        End Sub

'        Private Sub InitGridData()
'            DataSource.Rows.Clear()
'            For i As Integer = 0 To Rows
'                DataSource.Rows.Add(New FlexGridRow(Me))
'            Next
'            CopyDataSource2Orginal()
'            'Me.DataSourceOrg = Me.DataSource
'        End Sub

'        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
'            If Not Me.DesignMode Then
'                'To generate function __doPostBack(eventTarget, eventArgument)
'                MyBase.Page.ClientScript.GetPostBackEventReference(Me, Me.UniqueID)

'                'MyBase.Page.MaintainScrollPositionOnPostBack = True

'                Dim mScript As New HtmlControls.HtmlGenericControl("script")
'                mScript.ID = "FlexGridScript"
'                mScript.Attributes.Add("language", "javascript")
'                mScript.Attributes.Add("type", "text/javascript")
'                mScript.InnerHtml = vbCrLf & "<!-- " & vbCrLf
'                mScript.InnerHtml += GridFocusScript()
'                mScript.InnerHtml += FormulaScript()
'                mScript.InnerHtml += toNumber()
'                mScript.InnerHtml += KeyDown()
'                mScript.InnerHtml += vbCrLf & " -->" & vbCrLf
'                If MyBase.Page.Header.FindControl(mScript.ID) Is Nothing Then
'                    MyBase.Page.Header.Controls.Add(mScript)
'                End If
'                mScript = Nothing
'            End If
'            MyBase.OnLoad(e)
'        End Sub

'        Private Sub FlexGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'            If Not Me.DesignMode Then
'                If Not MyBase.Page.IsPostBack Then
'                    KeepOriginalSource()
'                End If
'                SetFocus = Nothing
'                PostbackDataHandler() 'always call before PostbackEventHandler() 
'                PostbackEventHandler() 'POSTBACK Handler
'            End If
'        End Sub

'        Private Property SetFocus() As HtmlControls.HtmlGenericControl
'            Get
'                Return mSetFocus
'            End Get
'            Set(ByVal value As HtmlControls.HtmlGenericControl)
'                mSetFocus = value
'            End Set
'        End Property

'        Public Overrides Property ID() As String
'            Get
'                Return MyBase.ID
'            End Get
'            Set(ByVal value As String)
'                If InStr(value, "_") > 0 Then
'                    Throw New Exception("Underscore cannot be used in ID")
'                Else
'                    MyBase.ID = value
'                End If
'            End Set
'        End Property

'        Public Function Inserted() As FlexGridRows
'            Dim mRet As New FlexGridRows
'            Dim mSrc, mOrg As String
'            Dim mFound As Boolean = False
'            For iSrc As Integer = 0 To Me.DataSource.Rows.Count - 1
'                mSrc = Me.DataSource.Rows(iSrc).Item(Me.PrimaryKeyColumn).Cell.Text
'                mFound = False
'                For iOrg As Integer = 0 To Me.DataSourceOrg.Rows.Count - 1
'                    mOrg = Me.DataSourceOrg.Rows(iOrg)(Me.PrimaryKeyColumn).Cell.Text
'                    If mSrc.Trim.ToLower = mOrg.Trim.ToLower Then
'                        mFound = True
'                        Exit For
'                    End If
'                Next
'                If Not mFound Then
'                    mRet.Add(Me.DataSource.Rows(iSrc))
'                End If
'            Next
'            Return mRet
'        End Function

'        Public Function Deleted() As FlexGridRows
'            Dim mRet As New FlexGridRows
'            Dim mSrc, mOrg As String
'            Dim mFound As Boolean = False
'            For iOrg As Integer = 0 To Me.DataSourceOrg.Rows.Count - 1
'                mOrg = Me.DataSourceOrg.Rows(iOrg)(Me.PrimaryKeyColumn).Cell.Text
'                mFound = False
'                For iSrc As Integer = 0 To Me.DataSource.Rows.Count - 1
'                    mSrc = Me.DataSource.Rows(iSrc).Item(Me.PrimaryKeyColumn).Cell.Text
'                    If mSrc.Trim.ToLower = mOrg.Trim.ToLower Then
'                        mFound = True
'                        Exit For
'                    End If
'                Next
'                If Not mFound Then
'                    mRet.Add(Me.DataSourceOrg.Rows(iOrg))
'                End If
'            Next
'            Return mRet
'        End Function

'        Public Function Updated() As FlexGridUpdRows
'            Dim mRet As New FlexGridUpdRows
'            Dim mUpd As Boolean = False
'            Dim mSr, mOr As Integer
'            mSr = 0
'            mOr = 0
'            For Each mSrcRow As FlexGridRow In Me.DataSource.Rows
'                mUpd = False
'                For Each mOrgRow As FlexGridRow In Me.DataSourceOrg.Rows
'                    Dim mS, mO As String
'                    mS = mSrcRow.Item(Me.PrimaryKeyColumn).Cell.Text
'                    mO = mOrgRow.Item(Me.PrimaryKeyColumn).Cell.Text
'                    If Not String.IsNullOrEmpty(mS) And Not String.IsNullOrEmpty(mS) And mS.Trim.ToLower = mO.Trim.ToLower Then
'                        For i As Integer = 0 To mSrcRow.Columns.Count - 1
'                            If mSrcRow.Item(i).Cell.Text.Trim.ToLower <> mOrgRow.Item(i).Cell.Text.Trim.ToLower Then
'                                Dim mR As New FlexGridUpdRow(Me)
'                                mR.OldPrimarykeyCol = mO
'                                For ii As Integer = 0 To mSrcRow.Columns.Count - 1
'                                    mR.Columns.Item(ii).Cell = mSrcRow.Columns(ii).Cell
'                                Next
'                                mRet.Add(mR)
'                                mUpd = True
'                                Exit For
'                            End If
'                        Next
'                        If mUpd Then
'                            Exit For
'                        End If
'                    End If
'                    mOr += 1
'                Next
'                mSr += 1
'            Next
'            Return mRet
'        End Function

'        Public Function FlushChanges() As Boolean
'            CopyDataSource2Orginal()
'            'Me.DataSourceOrg = Me.DataSource
'        End Function

'        Private Sub PostbackEventHandler()
'            'Must be called the following line on OnLoad() in-order to use the postback event handler
'            'MyBase.Page.ClientScript.GetPostBackEventReference(Me, me.UniqueID)

'            Dim mTar As String = String.Empty
'            Dim mArg As String = String.Empty
'            Try
'                mTar = MyBase.Page.Request.Form("__EVENTTARGET")
'                If Me.UniqueID = mTar Then
'                    mArg = MyBase.Page.Request.Form("__EVENTARGUMENT")
'                    If Left(mArg, (Me.UniqueID & "_R").Length) = (Me.UniqueID & "_R") Then
'                        Dim mR1 As Integer = Mid(mArg.Split("_")(1), 2)
'                        Dim mC1 As Integer = Mid(mArg.Split("_")(2), 2)
'                        Dim mEvRet As New FlexGridChangeSet
'                        mEvRet.Row = mR1
'                        mEvRet.Col = mC1
'                        mEvRet.Text = FindText(mR1, mC1)
'                        mEvRet.Value = FindValue(mR1, mC1)
'                        RaiseEvent AutoPostback_OnChange(mEvRet)
'                        mEvRet = Nothing
'                    Else
'                        Select Case Left(mArg, 3)
'                            Case "DTR" 'delete this row
'                                DeleteRow(Mid(mArg, 4))
'                            Case "ARB" 'add row below
'                                AddRowBelow(Mid(mArg, 4))
'                        End Select
'                    End If

'                    'SET FOCUS
'                    Dim mFoc As String = MyBase.Page.Request.Form.Item(String.Format("{0}_hf", Me.UniqueID))
'                    If Not String.IsNullOrEmpty(mFoc) Then
'                        Dim mSetFocusScript As New HtmlControls.HtmlGenericControl("script")
'                        mSetFocusScript.ID = "GridSetFocusScript"
'                        mSetFocusScript.Attributes.Add("language", "javascript")
'                        mSetFocusScript.Attributes.Add("type", "text/javascript")
'                        mSetFocusScript.InnerHtml = String.Format("GridFocus('{0}')", mFoc)

'                        'Leave it for rendering
'                        SetFocus = mSetFocusScript
'                        mSetFocusScript = Nothing
'                    End If
'                End If
'            Catch ex As Exception
'            End Try
'        End Sub

'        Private Function FindText(ByVal pRow As Integer, ByVal pCol As Integer) As String
'            Dim mRet As String = ""
'            If Me.Columns.Item(pCol).ColumnType = ColumnTypes.DropDownList Then
'                For i As Integer = 1 To Me.Columns.Item(pCol).DropDownListItems.Count
'                    If Me.Columns.Item(pCol).DropDownListItems(i - 1).Text = Me.DataSource.Rows(pRow)(pCol).Cell.Text Then
'                        mRet = Me.Columns.Item(pCol).DropDownListItems(i - 1).Text
'                        Exit For
'                    End If
'                Next
'            Else
'                mRet = Me.DataSource.Rows(pRow)(pCol).Cell.Text
'            End If
'            Return mRet
'        End Function

'        Private Function FindValue(ByVal pRow As Integer, ByVal pCol As Integer) As String
'            Dim mRet As String = ""
'            If Me.Columns.Item(pCol).ColumnType = ColumnTypes.DropDownList Then
'                For i As Integer = 1 To Me.Columns.Item(pCol).DropDownListItems.Count
'                    If Me.Columns.Item(pCol).DropDownListItems(i - 1).Text = Me.DataSource.Rows(pRow)(pCol).Cell.Text Then
'                        mRet = Me.Columns.Item(pCol).DropDownListItems(i - 1).Value
'                        Exit For
'                    End If
'                Next
'            Else
'                mRet = Me.DataSource.Rows(pRow)(pCol).Cell.Text
'            End If
'            Return mRet
'        End Function

'        Private Function DeleteRow(ByVal RowNo As Integer) As Boolean
'            If RowNo >= 0 And RowNo < Me.DataSource.Rows.Count Then
'                Me.DataSource.Rows.RemoveAt(RowNo)
'            End If
'        End Function

'        Private Function AddRowBelow(ByVal Rows As Integer) As Boolean
'            For i As Integer = 1 To Rows
'                Me.DataSource.Rows.Add(New FlexGridRow(Me))
'                NewRowAdded = True
'            Next
'        End Function

'        Private Sub PostbackDataHandler()
'            If MyBase.Page.IsPostBack Then
'                Dim mField As String = ""
'                For R As Integer = 0 To Rows - 1
'                    Dim C As Integer = 0
'                    For Each mCol As FgColumn In Columns
'                        If mCol.ColumnType <> ColumnTypes.Generic Then
'                            mField = String.Format("{0}_R{1}_C{2}", Me.UniqueID, R, C)
'                            DataSource.Rows(R)(C).Cell.Value = MyBase.Page.Request.Form.Item(mField)
'                            DataSource.Rows(R)(C).Cell.Text = FindText(R, C)
'                        End If
'                        C += 1
'                    Next
'                Next
'                'FormulaServerSide()
'            End If
'        End Sub

'        Private Function DelButton(ByVal RowNo As Integer) As HtmlControls.HtmlTableCell
'            Dim mScript As New StringBuilder
'            Dim mSubScr As String
'            mScript.AppendLine("<table cellpadding='0'>")
'            mScript.AppendLine("<tr>")
'            mSubScr = String.Format("javascript:if(confirm('Delete this row?')) setTimeout('__doPostBack(\'{0}\',\'DTR{1}\')', 0);", Me.UniqueID, RowNo)
'            mScript.AppendLine(String.Format("<td><img alt='Delete this row' style='border-color:gray;border-style:solid ;border-width:1px;cursor:hand' src='{0}' onclick=""{1}"" /></td>", MyBase.Page.ClientScript.GetWebResourceUrl(Me.GetType(), "DeleteRow.jpg"), mSubScr))
'            'mSubScr = String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'ARA{1}\')', 0)", Me.UniqueID, RowNo)
'            'mScript.AppendLine(String.Format("<td><img alt='Add row above' style='border-color:gray;border-style:solid ;border-width:1px;cursor:hand' src='{0}' onclick=""{1}"" /></td>", MyBase.Page.ClientScript.GetWebResourceUrl(Me.GetType(), "AddRowAbove.jpg"), mSubScr))
'            mScript.AppendLine("</tr>")
'            mScript.AppendLine("</table>")
'            mSubScr = Nothing

'            Dim mTbCt As HtmlControls.HtmlTableCell = TD(mScript.ToString, 20, HorizontalAlign.Left)
'            mTbCt.Style.Add("background-color", System.Drawing.ColorTranslator.ToHtml(Me.CaptionBgColor).ToString)
'            mTbCt.Style.Add("text-align", "left")
'            Return mTbCt
'        End Function

'        Private Function ArbButton() As HtmlControls.HtmlTableCell
'            Dim mScript As New StringBuilder
'            Dim mSubScr As String
'            mScript.AppendLine("<table cellpadding='0'>")
'            mScript.AppendLine("<tr>")
'            mSubScr = String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'ARB{1}\')', 0)", Me.UniqueID, 1)
'            mScript.AppendLine(String.Format("<td><img alt='Add new Row [Ctrl+{0}]' style='border-color:gray;border-style:solid ;border-width:1px;cursor:hand' src='{1}' onclick=""{2}"" /></td>", Me.NewRowShortCutKey.ToString, MyBase.Page.ClientScript.GetWebResourceUrl(Me.GetType(), "AddRowBelow.jpg"), mSubScr))
'            mScript.AppendLine("</tr>")
'            mScript.AppendLine("</table>")
'            mSubScr = Nothing


'            Dim mTbCt As HtmlControls.HtmlTableCell = TD(mScript.ToString, 20, HorizontalAlign.Left)
'            mTbCt.Style.Add("background-color", System.Drawing.ColorTranslator.ToHtml(Me.CaptionBgColor).ToString)
'            mTbCt.Style.Add("text-align", "left")
'            Return mTbCt
'        End Function

'        Private Function FormulaServerSide() As Boolean
'            For r As Integer = 0 To Rows - 1
'                For c As Integer = 0 To Columns.Count - 1
'                    If Left(Me.Columns.Item(c).Formula.Trim, 1) = "=" Then
'                        Dim mFormula As String = Me.Columns(c).Formula
'                        '_Library.Arithmetic.Math
'                        mFormula = Replace(mFormula, " ", "")
'                        mFormula = ReplaceColVal(mFormula, Me.DataSource.Rows(r))
'                        mFormula = Mid(mFormula, 2)
'                        Try
'                            Me.DataSource.Rows(r)(c).Cell.Text = Arithmetic.Math.ExpressionParser.Evaluate(mFormula)
'                        Catch ex As Exception
'                            RaiseEvent onFormulaError(Me.Columns(c).Formula, c, r, ex)
'                        End Try
'                    End If
'                Next
'            Next

'        End Function

'        Private Function ReplaceColVal(ByVal pFormula As String, ByVal pDataSourceRow As FlexGridRow) As String
'            Dim mStPos As Integer = InStr(pFormula, "[")
'            Dim mEnPos As Integer = InStr(pFormula, "]")
'            Dim mCol As Integer
'            If mStPos > 0 And mEnPos > 0 Then
'                mCol = Mid(pFormula, mStPos + 1, ((mEnPos - 1) - mStPos))
'                Dim mMid As String = pDataSourceRow(mCol).Cell.Text
'                If Not IsNumeric(mMid) Then
'                    mMid = "0"
'                End If
'                pFormula = Left(pFormula, mStPos - 1) & mMid & Mid(pFormula, mEnPos + 1)
'                Return ReplaceColVal(pFormula, pDataSourceRow)
'            Else
'                Return pFormula
'            End If
'        End Function

'        Private Function FormulaScript() As String
'            Dim mRet As New StringBuilder
'            Dim mScript As New StringBuilder
'            mRet.AppendLine("function RunFormula(pRows) {")
'            For c As Integer = 0 To Me.Columns.Count - 1
'                If Left(Me.Columns.Item(c).Formula.Trim, 1) = "=" Then
'                    Dim mFormula As String = Me.Columns(c).Formula
'                    mFormula = Replace(mFormula, " ", "")
'                    mFormula = Replace(mFormula, "]", "').value) ")
'                    mFormula = Replace(mFormula, "[", String.Format(" toNumber(document.getElementById('{0}_R'+r+'_C", Me.UniqueID))
'                    Select Case Me.Columns.Item(c).ColumnType
'                        Case ColumnTypes.Generic
'                            mScript.AppendLine(String.Format("      document.getElementById('{0}_R'+r+'_C{1}').innerText {2};", Me.UniqueID, c, mFormula))
'                            'Math.round(1235.2*100)/100
'                            If Split(Me.Columns(c).NumericFormat, ".").Length = 2 Then
'                                Dim mDiv As String = "1" & StrDup(Split(Replace(Me.Columns(c).NumericFormat, "]", ""), ".")(1).Length, "0")
'                                mScript.AppendLine(String.Format("      document.getElementById('{0}_R'+r+'_C{1}').innerText = Math.round(document.getElementById('{0}_R'+r+'_C{1}').innerText*{2})/{2};", Me.UniqueID, c, mDiv))
'                            End If
'                        Case Else
'                            mScript.AppendLine(String.Format("      document.getElementById('{0}_R'+r+'_C{1}').value {2};", Me.UniqueID, c, mFormula))
'                            If Split(Me.Columns(c).NumericFormat, ".").Length = 2 Then
'                                Dim mDiv As String = "1" & StrDup(Split(Replace(Me.Columns(c).NumericFormat, "]", ""), ".")(1).Length, "0")
'                                mScript.AppendLine(String.Format("      document.getElementById('{0}_R'+r+'_C{1}').value = Math.round(document.getElementById('{0}_R'+r+'_C{1}').value*{2})/{2};", Me.UniqueID, c, mDiv))
'                            End If
'                    End Select

'                End If
'            Next
'            If Not String.IsNullOrEmpty(mScript.ToString) Then
'                'mRet.AppendLine(String.Format("   for(r=0;r<{0};r++)", Me.Rows) & " {")
'                mRet.AppendLine(String.Format("   for(r=0;r<pRows;r++)", Me.Rows) & " {")
'                mRet.AppendLine(mScript.ToString)
'                mRet.AppendLine("   }")
'            End If
'            mRet.AppendLine("}")
'            Return mRet.ToString
'        End Function

'        Private Function toNumber() As String
'            Dim mRet As New StringBuilder
'            mRet.AppendLine("function isNumeric(value) {")
'            mRet.AppendLine("return typeof value != 'boolean' && value !== null && !isNaN(+ value);")
'            mRet.AppendLine("}")
'            mRet.AppendLine(" ")
'            mRet.AppendLine("function toNumber(value) {")
'            mRet.AppendLine("if(isNumeric(value)) {return (1*value);}")
'            'mRet.AppendLine("else {return value;}")
'            mRet.AppendLine("else {return null;}")
'            mRet.AppendLine("}")
'            Return mRet.ToString
'        End Function

'        Private Function KeyDown() As String
'            Dim mRet As New StringBuilder
'            mRet.AppendLine("function GridKeyDown() {")
'            mRet.AppendLine("   if(event.ctrlKey) {")
'            mRet.AppendLine("       if(event.keyCode==" & Asc(NewRowShortCutKey.ToString.ToUpper) - 64 & ") {")
'            mRet.AppendLine(String.Format("         javascript:setTimeout('__doPostBack(\'{0}\',\'ARB{1}\')', 0)", Me.UniqueID, 1))
'            mRet.AppendLine("       }")
'            mRet.AppendLine("       else {")

'            'Test Line
'            'mRet.AppendLine("           alert(event.ctrlKey+'.'+event.keyCode);")

'            mRet.AppendLine("       }")
'            mRet.AppendLine("   }")
'            mRet.AppendLine("   else {")

'            'Test Line
'            'mRet.AppendLine("       alert(event.keyCode);")

'            mRet.AppendLine("   }")
'            mRet.AppendLine("}")
'            Return mRet.ToString
'        End Function

'        Private Function GridFocusScript() As String
'            Dim mRet As New StringBuilder
'            mRet.AppendLine("function GridFocus(CtrlID) {")
'            mRet.AppendLine("   try {")
'            mRet.AppendLine("       pObj = document.getElementById(CtrlID);")
'            mRet.AppendLine("       pObj.focus();")
'            mRet.AppendLine("       pObj.select();")
'            mRet.AppendLine("   }")
'            mRet.AppendLine("   catch(err) {")
'            mRet.AppendLine("   }")
'            mRet.AppendLine("}")
'            Return mRet.ToString
'        End Function

'        Private Function SetFormat(ByVal pStr As String, ByVal pStyle As String) As String
'            If Left(pStyle.Trim, 1) = "=" And IsNumeric(pStr) Then
'                pStyle = Replace(pStyle, "=", "")
'                pStyle = Replace(pStyle, "[", "")
'                pStyle = Replace(pStyle, "]", "")
'                If pStr > 0 Then
'                    Return Format(CDbl(pStr), pStyle)
'                Else
'                    Return pStr
'                End If
'            Else
'                Return pStr
'            End If
'        End Function

'        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
'            If Me.DesignMode Then
'                RndrCont(writer)
'            Else
'                Dim mLC As New _Base.LibraryBase 'check license
'                mLC = Nothing
'                If _Base.LibraryBase._LicenseValid Then
'                    RndrCont(writer)
'                Else
'                    writer.Write("License required")
'                End If
'            End If
'        End Sub

'        Private Sub RndrCont(ByVal writer As System.Web.UI.HtmlTextWriter)
'            If Not Me.DesignMode Then
'                FormulaServerSide()
'            End If
'            Dim mHead As Boolean = True
'            Dim mTbl As New HtmlControls.HtmlTable
'            Dim mC As Integer = 0
'            mTbl.Attributes.Clear()
'            mTbl.Attributes.Add("border", Me.BorderWidth.ToString)
'            mTbl.Attributes.Add("bordercolor", System.Drawing.ColorTranslator.ToHtml(Me.BorderColor))
'            If Not String.IsNullOrEmpty(Me.CssClass) Then
'                mTbl.Attributes.Add("class", Me.CssClass)
'            End If
'            mTbl.Attributes.Add("cellspacing", "0")
'            mTbl.Attributes.Add("cellpadding", "0")
'            mTbl.Style.Add("border-collapse", "collapse")
'            Dim mTr As HtmlControls.HtmlTableRow
'            Dim mColFound As Boolean = False
'            If Columns IsNot Nothing Then
'                If Columns.Count > 0 Then
'                    mColFound = True
'                End If
'            End If
'            If mColFound Then
'                'HEADER
'                mTr = New HtmlControls.HtmlTableRow
'                'mC = 0
'                If Toolbar = FlexGridToolbar.Visible Then
'                    mTr.Cells.Add(ArbButton())
'                End If

'                For Each mCol As FgColumn In Columns
'                    Dim mTbC As HtmlControls.HtmlTableCell = TD(mCol.Caption, mCol.Width, mCol.HorizontalAlign)
'                    mTbC.Style.Add("color", System.Drawing.ColorTranslator.ToHtml(Me.CaptionFgColor).ToString)
'                    mTbC.Style.Add("background-color", System.Drawing.ColorTranslator.ToHtml(Me.CaptionBgColor).ToString)
'                    If Not String.IsNullOrEmpty(mCol.CaptionCssClass) Then
'                        mTr.Attributes.Add("class", mCol.CaptionCssClass)
'                    Else
'                        mTbC.Style.Add("text-align", "center")
'                        mTbC.Style.Add("font-weight", "bold")
'                    End If
'                    mTr.Cells.Add(mTbC)

'                    'mC += 1
'                Next
'                mTbl.Rows.Add(mTr)
'                mTr = Nothing

'                'DETAIL
'                For i As Integer = 0 To Rows - 1
'                    mTr = New HtmlControls.HtmlTableRow
'                    mC = 0

'                    If Toolbar = FlexGridToolbar.Visible Then
'                        mTr.Cells.Add(DelButton(i))
'                    End If

'                    For Each mCol As FgColumn In Columns

'                        If Not String.IsNullOrEmpty(mCol.ItemCssClass) Then
'                            mTr.Attributes.Add("class", mCol.ItemCssClass)
'                        End If
'                        Select Case mCol.ColumnType
'                            Case ColumnTypes.Generic
'                                Dim mGn As New Label
'                                mGn.BorderWidth = 0
'                                mGn.Width = mCol.Width
'                                mGn.CssClass = mCol.ItemCssClass
'                                mGn.ID = String.Format("{0}_R{1}_C{2}", Me.UniqueID, i, mC)
'                                Try
'                                    If Not Me.DesignMode Then
'                                        mGn.Text = SetFormat(DataSource.Rows(i)(mC).Cell.Text, mCol.NumericFormat)
'                                    End If
'                                Catch ex As Exception
'                                End Try
'                                mTr.Cells.Add(TD(mGn, mCol.HorizontalAlign))
'                                mGn = Nothing
'                            Case ColumnTypes.TextBox
'                                Dim mTb As New TextBox
'                                mTb.BorderWidth = 0
'                                mTb.Width = mCol.Width
'                                mTb.CssClass = mCol.ItemCssClass
'                                mTb.Enabled = mCol.Enabled
'                                mTb.Visible = mCol.Visible
'                                mTb.ID = String.Format("{0}_R{1}_C{2}", Me.UniqueID, i, mC)

'                                If mCol.AutoPostback And Not Me.DesignMode Then
'                                    mTb.Attributes.Add("onchange", String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'{1}\')', 0)", Me.UniqueID, mTb.ID))
'                                    mTb.Attributes.Add("onfocus", String.Format("document.getElementById('{0}_hf').value=this.id", Me.UniqueID))
'                                Else
'                                    mTb.Attributes.Add("onchange", String.Format("RunFormula({0})", Me.Rows))
'                                End If
'                                Try
'                                    If Not Me.DesignMode Then
'                                        mTb.Text = SetFormat(DataSource.Rows(i)(mC).Cell.Text, mCol.NumericFormat)
'                                    End If
'                                Catch ex As Exception
'                                End Try
'                                mTb.Style.Add("text-align", mCol.HorizontalAlign.ToString)
'                                mTr.Cells.Add(TD(mTb, mCol.HorizontalAlign))
'                                mTb = Nothing
'                            Case ColumnTypes.TextArea
'                                Dim mTb As New TextBox
'                                mTb.BorderWidth = 0
'                                mTb.Width = mCol.Width
'                                mTb.Enabled = mCol.Enabled
'                                mTb.Visible = mCol.Visible
'                                mTb.TextMode = TextBoxMode.MultiLine
'                                mTb.CssClass = mCol.ItemCssClass
'                                mTb.ID = String.Format("{0}_R{1}_C{2}", Me.UniqueID, i, mC)
'                                If mCol.AutoPostback And Not Me.DesignMode Then
'                                    mTb.Attributes.Add("onchange", String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'{1}\')', 0)", Me.UniqueID, mTb.ID))
'                                    mTb.Attributes.Add("onfocus", String.Format("document.getElementById('{0}_hf').value=this.id", Me.UniqueID))
'                                Else
'                                    mTb.Attributes.Add("onchange", String.Format("RunFormula({0})", Me.Rows))
'                                End If
'                                Try
'                                    If Not Me.DesignMode Then
'                                        mTb.Text = SetFormat(DataSource.Rows(i)(mC).Cell.Text, mCol.NumericFormat)
'                                    End If
'                                Catch ex As Exception
'                                End Try
'                                mTb.Style.Add("text-align", mCol.HorizontalAlign.ToString)
'                                mTr.Cells.Add(TD(mTb, mCol.HorizontalAlign))
'                                mTb = Nothing
'                            Case ColumnTypes.DropDownList
'                                Dim mDdl As New HtmlControls.HtmlGenericControl("select")
'                                mDdl.ID = String.Format("{0}_R{1}_C{2}", Me.UniqueID, i, mC)
'                                mDdl.Attributes.Add("name", mDdl.ID)
'                                If mCol.AutoPostback And Not Me.DesignMode Then
'                                    mDdl.Attributes.Add("onchange", String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'{1}\')', 0)", Me.UniqueID, mDdl.ID))
'                                    mDdl.Attributes.Add("onfocus", String.Format("document.getElementById('{0}_hf').value=this.id", Me.UniqueID))
'                                Else
'                                    mDdl.Attributes.Add("onchange", String.Format("RunFormula({0})", Me.Rows))
'                                End If
'                                mDdl.Style.Add("border-width", "0px")
'                                mDdl.Style.Add("width", "100%")
'                                If Not mCol.Enabled Then
'                                    mDdl.Attributes.Add("onbeforeactivate", "this.disabled=true;")
'                                End If
'                                If Not String.IsNullOrEmpty(mCol.ItemCssClass) Then
'                                    mDdl.Attributes.Add("class", mCol.ItemCssClass)
'                                End If
'                                Dim mSelectedValue As String = ""
'                                'FINDING SELECTED VALUE
'                                For Each mLi As ListItem In mCol.DropDownListItems
'                                    If Not Me.DesignMode Then
'                                        If Not String.IsNullOrEmpty(DataSource.Rows(i).Columns(mC).Cell.Text) Then
'                                            If DataSource.Rows(i).Columns(mC).Cell.Text.ToLower = mLi.Value.ToLower Then
'                                                mSelectedValue = mLi.Value
'                                                Exit For
'                                            End If
'                                        End If
'                                        If mLi.Selected Then
'                                            mSelectedValue = mLi.Value
'                                        End If
'                                    End If
'                                Next

'                                For Each mLi As ListItem In mCol.DropDownListItems
'                                    Dim mDdlopt As New HtmlControls.HtmlGenericControl("option")
'                                    If mSelectedValue = mLi.Value Then
'                                        mDdlopt.Attributes.Add("selected", "selected")
'                                    End If
'                                    mDdlopt.Attributes.Add("value", mLi.Value)
'                                    mDdlopt.InnerText = mLi.Text
'                                    mDdl.Controls.Add(mDdlopt)
'                                    mDdlopt.Dispose()
'                                    mDdlopt = Nothing
'                                Next
'                                mTr.Cells.Add(TD(mDdl, mCol.HorizontalAlign))
'                                mDdl.Dispose()
'                                mDdl = Nothing
'                            Case ColumnTypes.CheckBox
'                                Dim mCb As New HtmlControls.HtmlInputCheckBox
'                                mCb.Style.Add("border-width", "0px")
'                                mCb.Style.Add("width", "100%")
'                                If Not mCol.Enabled Then
'                                    mCb.Attributes.Add("onbeforeactivate", "this.disabled=true;")
'                                End If
'                                If Not String.IsNullOrEmpty(mCol.ItemCssClass) Then
'                                    mCb.Attributes.Add("class", mCol.ItemCssClass)
'                                End If
'                                mCb.ID = String.Format("{0}_R{1}_C{2}", Me.UniqueID, i, mC)
'                                If mCol.AutoPostback And Not Me.DesignMode Then
'                                    mCb.Attributes.Add("onclick", String.Format("javascript:setTimeout('__doPostBack(\'{0}\',\'{1}\')', 0)", Me.UniqueID, mCb.ID))
'                                    mCb.Attributes.Add("onfocus", String.Format("document.getElementById('{0}_hf').value=this.id", Me.UniqueID))
'                                Else
'                                    mCb.Attributes.Add("onclick", String.Format("RunFormula({0})", Me.Rows))
'                                End If
'                                Try
'                                    If Not Me.DesignMode Then
'                                        If DataSource.Rows(i).Columns(mC).Cell.Text.ToString.ToLower = "on" Then
'                                            mCb.Checked = True
'                                        Else
'                                            mCb.Checked = False
'                                        End If
'                                    End If
'                                Catch ex As Exception
'                                End Try
'                                mTr.Cells.Add(TD(mCb, mCol.HorizontalAlign))
'                                mCb = Nothing
'                            Case Else
'                        End Select
'                        mC += 1
'                    Next
'                    mTbl.Rows.Add(mTr)
'                    mTr = Nothing

'                Next
'            Else
'                mTr = New HtmlControls.HtmlTableRow
'                mTr.Cells.Add(TD("FlexGrid", Me.Width, HorizontalAlign.NotSet, 70))
'                mTbl.Rows.Add(mTr)
'                mTr = Nothing
'            End If
'            Dim mBs As New HtmlControls.HtmlGenericControl("div")
'            mBs.Attributes.Add("onkeypress", "GridKeyDown()")
'            mBs.Style.Add("overflow", "auto")
'            'mBs.Style.Add("overflow", "scroll")
'            mBs.Style.Add("height", Me.Height.ToString)
'            mBs.Style.Add("width", Me.Width.ToString)
'            mBs.ID = String.Format("{0}_Div", Me.UniqueID)
'            mBs.Controls.Add(mTbl)

'            Dim mBsT As New HtmlControls.HtmlTable
'            If Not String.IsNullOrEmpty(Me.CssClass) Then
'                mBsT.Attributes.Add("class", Me.CssClass)
'            End If
'            mBsT.Attributes.Add("border", Me.BorderWidth.ToString)
'            mBsT.Attributes.Add("bordercolor", System.Drawing.ColorTranslator.ToHtml(Me.BorderColor))
'            mBsT.Attributes.Add("cellspacing", "0")
'            mBsT.Attributes.Add("cellpadding", "0")
'            mBsT.Style.Add("border-collapse", "collapse")
'            mBsT.ID = Me.UniqueID

'            Dim mBsTr As New HtmlControls.HtmlTableRow
'            mBsTr.Cells.Add(TD(mBs, HorizontalAlign.NotSet))
'            mBsT.Rows.Add(mBsTr)
'            mBsTr = Nothing
'            mBsT.RenderControl(writer)

'            If Not Me.DesignMode Then
'                Dim mH As New HtmlControls.HtmlInputHidden
'                mH.ID = String.Format("{0}_hf", Me.UniqueID)
'                mH.RenderControl(writer)

'                If NewRowAdded Then
'                    Dim _C As Integer = 0
'                    For Each mCol As FgColumn In Columns
'                        If mCol.ColumnType <> ColumnTypes.Generic And mCol.Enabled Then
'                            Exit For
'                        End If
'                        _C += 1
'                    Next
'                    Dim mFoc As String = String.Format("{0}_R{1}_C{2}", Me.UniqueID, Rows - 1, _C)
'                    If Not String.IsNullOrEmpty(mFoc) Then
'                        Dim mSetFocusScript As New HtmlControls.HtmlGenericControl("script")
'                        mSetFocusScript.ID = "NewRowFocusScript"
'                        mSetFocusScript.Attributes.Add("language", "javascript")
'                        mSetFocusScript.Attributes.Add("type", "text/javascript")
'                        mSetFocusScript.InnerHtml = String.Format("GridFocus('{0}')", mFoc)

'                        'Leave it for rendering
'                        SetFocus = mSetFocusScript
'                        mSetFocusScript = Nothing
'                    End If
'                End If

'                If SetFocus IsNot Nothing Then
'                    SetFocus.RenderControl(writer)
'                End If
'            End If
'            mBsT = Nothing
'            mBs = Nothing

'        End Sub

'        Private Sub CopyDataSource2Orginal()
'            Me.DataSourceOrg = New FlexGridSource
'            For r As Integer = 0 To Me.DataSource.Rows.Count - 1
'                Dim mFgr As New FlexGridRow(Me)
'                For c As Integer = 0 To Me.DataSource.Rows(r).Columns.Count - 1
'                    mFgr.Columns(c).Cell.Text = Me.DataSource.Rows(r)(c).Cell.Text
'                    mFgr.Columns(c).Cell.Value = Me.DataSource.Rows(r)(c).Cell.Value
'                Next
'                Me.DataSourceOrg.Rows.Add(mFgr)
'            Next
'        End Sub

'        Private Sub KeepOriginalSource()
'            CopyDataSource2Orginal()
'            'Me.DataSourceOrg = Me.DataSource
'        End Sub

'        Protected Overrides Function SaveViewState() As Object
'            Dim NewState As Object()
'            ReDim NewState(3)
'            Rows = Me.DataSource.Rows.Count - 1
'            NewState(0) = MyBase.SaveViewState()
'            Dim mCol As New FlexGridChangeSets
'            For r As Integer = 0 To Me.DataSource.Rows.Count - 1
'                For c As Integer = 0 To Me.DataSource.Rows(r).Columns.Count - 1
'                    If Me.DataSource.Rows(r).Columns(c).Type = ColumnTypes.Generic Then
'                        Dim mC As New FlexGridChangeSet
'                        mC.Text = Me.DataSource.Rows(r).Columns(c).Cell.Text
'                        mC.Value = Me.DataSource.Rows(r).Columns(c).Cell.Value
'                        mC.Row = r
'                        mC.Col = c
'                        mCol.Add(mC)
'                    End If
'                Next
'            Next
'            NewState(1) = mCol

'            Dim mCol2 As New FlexGridChangeSets
'            For r As Integer = 0 To Me.DataSourceOrg.Rows.Count - 1
'                For c As Integer = 0 To Me.DataSourceOrg.Rows(r).Columns.Count - 1
'                    Dim mC As New FlexGridChangeSet
'                    mC.Text = Me.DataSourceOrg.Rows(r).Columns(c).Cell.Text
'                    mC.Value = Me.DataSourceOrg.Rows(r).Columns(c).Cell.Value
'                    mC.Row = r
'                    mC.Col = c
'                    mCol2.Add(mC)
'                Next
'            Next
'            NewState(2) = mCol2
'            Return NewState
'        End Function

'        Protected Overrides Sub LoadViewState(ByVal savedState As Object)
'            MyBase.LoadViewState(savedState(0))
'            InitGridData()

'            For Each mS As FlexGridChangeSet In savedState(1)
'                If mS IsNot Nothing Then
'                    Me.DataSource.Rows(mS.Row).Columns(mS.Col).Cell.Text = mS.Text
'                    Me.DataSource.Rows(mS.Row).Columns(mS.Col).Cell.Value = mS.Value
'                End If
'            Next
'            For Each mS As FlexGridChangeSet In savedState(2)
'                If mS IsNot Nothing Then
'                    Me.DataSourceOrg.Rows(mS.Row).Columns(mS.Col).Cell.Text = mS.Text
'                    Me.DataSourceOrg.Rows(mS.Row).Columns(mS.Col).Cell.Value = mS.Value
'                End If
'            Next
'        End Sub

'        Private Function TD(ByVal pCtrl As Object, ByVal mAlign As System.Web.UI.WebControls.HorizontalAlign, Optional ByVal ColSpan As Integer = 0) As HtmlControls.HtmlTableCell
'            Dim mTd As New HtmlControls.HtmlTableCell
'            If ColSpan > 0 Then
'                mTd.ColSpan = ColSpan
'            End If
'            If mAlign <> HorizontalAlign.NotSet Then
'                mTd.Align = mAlign.ToString
'            End If
'            mTd.Controls.Add(pCtrl)
'            Return mTd
'        End Function

'        Private Function TD(ByVal pText As String, ByVal pWidth As Unit, ByVal mAlign As System.Web.UI.WebControls.HorizontalAlign, Optional ByVal ColSpan As Integer = 0) As HtmlControls.HtmlTableCell
'            Dim mTd As New HtmlControls.HtmlTableCell
'            If ColSpan > 0 Then
'                mTd.ColSpan = ColSpan
'            End If
'            If pWidth.Type = UnitType.Pixel Then
'                mTd.Width = pWidth.Value & "px"
'            Else
'                mTd.Width = pWidth.Value & "%"
'            End If
'            If mAlign <> HorizontalAlign.NotSet Then
'                mTd.Align = mAlign.ToString
'            End If
'            mTd.InnerHtml = pText
'            Return mTd
'        End Function

'        <Browsable(False)> _
'        Public ReadOnly Property DataSource() As FlexGridSource
'            Get
'                Return mDataSource
'            End Get
'        End Property

'        <Browsable(False)> _
'        Private Property DataSourceOrg() As FlexGridSource
'            Get
'                Return mDataSourceOrg
'            End Get
'            Set(ByVal value As FlexGridSource)
'                mDataSourceOrg = value
'            End Set
'        End Property

'        <Bindable(True), Category("Layout"), Localizable(True)> _
'        Public Property PrimaryKeyColumn() As Integer
'            Get
'                If ViewState(__PR_KEY) Is Nothing Then
'                    Return 0
'                Else
'                    Return ViewState(__PR_KEY)
'                End If
'            End Get
'            Set(ByVal value As Integer)
'                If value < 0 Then
'                    ViewState(__PR_KEY) = 0
'                Else
'                    ViewState(__PR_KEY) = value
'                End If
'            End Set
'        End Property

'        <Bindable(True), Category("Layout"), Localizable(True)> _
'        Public Property Rows() As Integer
'            Get
'                If ViewState(__RWS) Is Nothing Then
'                    Return 10
'                Else
'                    Return ViewState(__RWS)
'                End If
'            End Get
'            Set(ByVal value As Integer)
'                If value < 1 Then
'                    ViewState(__RWS) = 1
'                Else
'                    ViewState(__RWS) = value
'                End If
'            End Set
'        End Property


'        <Bindable(True), Category("ShortCutKeys [Ctrl+]"), Localizable(True)> _
'        Public Property NewRowShortCutKey() As Char
'            Get
'                If ViewState(__ARB) Is Nothing Then
'                    Return "m"
'                Else
'                    Return ViewState(__ARB)
'                End If
'            End Get
'            Set(ByVal value As Char)
'                If (Asc(value) >= Asc("a") And Asc(value) <= Asc("z")) Or (Asc(value) >= Asc("A") And Asc(value) <= Asc("Z")) Then
'                    ViewState(__ARB) = value
'                Else
'                    MsgBox("Invalid Property Value. valid values are A-Z,a-z")
'                End If
'            End Set
'        End Property

'        <Bindable(True), Category("Behavior"), Localizable(True)> _
'        Public Property Toolbar() As FlexGridToolbar
'            Get
'                If ViewState(__TB) Is Nothing Then
'                    Return FlexGridToolbar.Visible
'                Else
'                    Return ViewState(__TB)
'                End If
'            End Get
'            Set(ByVal value As FlexGridToolbar)
'                ViewState(__TB) = value
'            End Set
'        End Property

'        <Bindable(True), Category("Appearance"), Localizable(True)> _
'        Public Property CaptionBgColor() As System.Drawing.Color
'            Get
'                If ViewState(__CBGC) Is Nothing Then
'                    Return System.Drawing.Color.Silver
'                Else
'                    Return ViewState(__CBGC)
'                End If
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                ViewState(__CBGC) = value
'            End Set
'        End Property

'        <Bindable(True), Category("Appearance"), Localizable(True)> _
'        Public Property CaptionFgColor() As System.Drawing.Color
'            Get
'                If ViewState(__CFGC) Is Nothing Then
'                    Return System.Drawing.Color.Black
'                Else
'                    Return ViewState(__CFGC)
'                End If
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                ViewState(__CFGC) = value
'            End Set
'        End Property

'        <DefaultValue(""), Category("Layout"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint), _
'        DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
'        PersistenceMode(PersistenceMode.InnerProperty) _
'        > _
'        Public ReadOnly Property Columns() As List(Of FgColumn)
'            Get
'                Return mColumns
'            End Get
'        End Property


'    End Class

'    Public Enum FlexGridToolbar
'        Hidden = 0
'        Visible = 1
'    End Enum

'    Public Enum ColumnTypes
'        Generic = 0
'        TextBox = 1
'        TextArea = 2
'        DropDownList = 3
'        CheckBox = 4
'    End Enum

'#Region " FgColumn "
'    <ToolboxItem(False) _
'    , TypeConverter(GetType(FgColumnEOC)) _
'    > _
'    Public Class FgColumn
'        Inherits WebControl
'        Private mCap As String = "Caption"
'        Private mFormula As String = "example: =[<ColumnNo>]+[<ColumnNo>]"
'        Private mNumFormat As String = "example: =[##,##0.00]"
'        Private mCapCssClass As String
'        Private mItemCssClass As String
'        Private mAutoPostback As Boolean = False
'        Private mColType As ColumnTypes = ColumnTypes.TextBox
'        Private mHorizontalAlign As System.Web.UI.WebControls.HorizontalAlign = WebControls.HorizontalAlign.NotSet
'        Private mDdlItems As New ListItemCollection

'#Region " Hidding Base Properties "

'        <Browsable(False)> _
'        Public Overrides Property CssClass() As String
'            Get
'                Return MyBase.CssClass
'            End Get
'            Set(ByVal value As String)
'                'MyBase.CssClass = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property AccessKey() As String
'            Get
'                Return MyBase.AccessKey
'            End Get
'            Set(ByVal value As String)
'                'MyBase.AccessKey = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property TabIndex() As Short
'            Get
'                Return MyBase.TabIndex
'            End Get
'            Set(ByVal value As Short)
'                'MyBase.TabIndex = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BackColor() As System.Drawing.Color
'            Get
'                Return MyBase.BackColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                'MyBase.BackColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderColor() As System.Drawing.Color
'            Get
'                Return MyBase.BorderColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                'MyBase.BorderColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
'            Get
'                Return MyBase.BorderStyle
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
'                'MyBase.BorderStyle = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
'            Get
'                Return MyBase.BorderWidth
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.Unit)
'                'MyBase.BorderWidth = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
'            Get
'                Return Nothing ' MyBase.Font
'            End Get
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ForeColor() As System.Drawing.Color
'            Get
'                Return MyBase.ForeColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                'MyBase.ForeColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property EnableTheming() As Boolean
'            Get
'                Return MyBase.EnableTheming
'            End Get
'            Set(ByVal value As Boolean)
'                'MyBase.EnableTheming = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property SkinID() As String
'            Get
'                Return MyBase.SkinID
'            End Get
'            Set(ByVal value As String)
'                'MyBase.SkinID = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ToolTip() As String
'            Get
'                Return MyBase.ToolTip
'            End Get
'            Set(ByVal value As String)
'                'MyBase.ToolTip = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ID() As String
'            Get
'                Return MyBase.ID
'            End Get
'            Set(ByVal value As String)
'                'MyBase.ID = value
'            End Set
'        End Property

'#End Region

'        <Category("Appearance"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property CaptionCssClass() As String
'            Get
'                Return mCapCssClass
'            End Get
'            Set(ByVal value As String)
'                mCapCssClass = value
'            End Set
'        End Property

'        <Category("Appearance"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property ItemCssClass() As String
'            Get
'                Return mItemCssClass
'            End Get
'            Set(ByVal value As String)
'                mItemCssClass = value
'            End Set
'        End Property

'        <Category("Layout"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property Caption() As String
'            Get
'                Return mCap
'            End Get
'            Set(ByVal value As String)
'                mCap = value
'            End Set
'        End Property

'        <Category("Layout"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property Formula() As String
'            Get
'                Return mFormula
'            End Get
'            Set(ByVal value As String)
'                mFormula = value
'            End Set
'        End Property

'        <Category("Layout"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property NumericFormat() As String
'            Get
'                Return mNumFormat
'            End Get
'            Set(ByVal value As String)
'                mNumFormat = value
'            End Set
'        End Property


'        <Category("Behavior"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property ColumnType() As ColumnTypes
'            Get
'                Return mColType
'            End Get
'            Set(ByVal value As ColumnTypes)
'                mColType = value
'            End Set
'        End Property

'        <Category("Behavior"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property AutoPostback() As Boolean
'            Get
'                Return mAutoPostback
'            End Get
'            Set(ByVal value As Boolean)
'                mAutoPostback = value
'            End Set
'        End Property

'        <Category("Behavior"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint) _
'        > _
'        Public Property HorizontalAlign() As System.Web.UI.WebControls.HorizontalAlign
'            Get
'                Return mHorizontalAlign
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.HorizontalAlign)
'                mHorizontalAlign = value
'            End Set
'        End Property

'        <DefaultValue(""), Category("Drop-Down-List"), NotifyParentProperty(True), _
'        RefreshProperties(RefreshProperties.Repaint), _
'        DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
'        PersistenceMode(PersistenceMode.InnerProperty) _
'        > _
'        Public ReadOnly Property DropDownListItems() As System.Web.UI.WebControls.ListItemCollection
'            Get
'                Return mDdlItems
'            End Get
'        End Property

'    End Class
'#End Region

'#Region " TypeConverter "

'    Public Class mEOC
'        Inherits ExpandableObjectConverter
'        Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
'            Return "H:" & MyBase.ConvertTo(context, culture, value.height, destinationType) & ",W:" & MyBase.ConvertTo(context, culture, value.width, destinationType)
'        End Function

'    End Class

'    Public Class mEOC2
'        Inherits ExpandableObjectConverter
'        Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
'            Return "Generic Control "
'        End Function

'    End Class

'    Public Class FgColumnEOC
'        Inherits ExpandableObjectConverter
'        Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
'            Return value.Caption
'        End Function

'    End Class

'#End Region

'End Namespace

''''TODO
''•	Events (Trigger)
''•	Insert Row
''•	Delete Row
''•	Paging 
''•	Lookup type columns
