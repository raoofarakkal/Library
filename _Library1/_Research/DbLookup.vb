'Imports System
'Imports System.Collections.Generic
'Imports System.ComponentModel
'Imports System.Text
'Imports System.Web
'Imports System.Web.UI
'Imports System.Web.UI.WebControls


'Namespace _Library._Web.DbLookup

'    <ToolboxData("<{0}:DbLookup runat=server></{0}:DbLookup>")> _
'    Public Class DbLookup
'        Inherits WebControl

'        <Bindable(True), Category("Layout"), Localizable(True)> _
'        Property X() As Integer
'            Get
'                If ViewState("DBL_X") Is Nothing Then
'                    Return 10
'                Else
'                    Return ViewState("DBL_X")
'                End If
'            End Get
'            Set(ByVal value As Integer)
'                ViewState("DBL_X") = CStr(value)
'            End Set
'        End Property

'        <Bindable(True), Category("Layout"), Localizable(True)> _
'        Property Y() As Integer
'            Get
'                If ViewState("DBL_Y") Is Nothing Then
'                    Return 10
'                Else
'                    Return ViewState("DBL_Y")
'                End If
'            End Get
'            Set(ByVal value As Integer)
'                ViewState("DBL_Y") = CStr(value)
'            End Set
'        End Property

'        <Browsable(False)> _
'        Property Datasource() As DataTable
'            Get
'                If ViewState("DBL_DG") Is Nothing Then
'                    ViewState("DBL_DG") = New DataTable
'                End If
'                Return ViewState("DBL_DG")
'            End Get
'            Set(ByVal value As DataTable)
'                ViewState("DBL_DG") = value
'            End Set
'        End Property

'        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
'            If Me.DesignMode Then
'                writer.Write("DbLookup")
'            Else
'                LookupDiv.RenderControl(writer)
'            End If
'        End Sub

'        Private Function LookupDiv() As HtmlControls.HtmlGenericControl
'            Dim mDiv As New HtmlControls.HtmlGenericControl("div")
'            mDiv.ID = Me.UniqueID
'            mDiv.Style.Add("position", "absolute")
'            mDiv.Style.Add("left", String.Format("{0}px", X))
'            mDiv.Style.Add("top", String.Format("{0}px", Y))
'            mDiv.Style.Add("width", String.Format("{0}", Width))
'            mDiv.Style.Add("height", String.Format("{0}", Height))
'            mDiv.Style.Add("overflow", "scroll")

'            Dim mTb As New HtmlControls.HtmlTable
'            Dim mTbR As New HtmlControls.HtmlTableRow
'            Dim mTbC As HtmlControls.HtmlTableCell
'            mTb.Style.Add("width", "100%")
'            mTb.Style.Add("background-color", "#C0C0C0")
'            mTb.CellPadding = 0
'            mTb.CellSpacing = 0

'            mTbC = New HtmlControls.HtmlTableCell
'            mTbC.InnerHtml = "&nbsp;"
'            mTbR.Cells.Add(mTbC)

'            mTbC = New HtmlControls.HtmlTableCell
'            mTbC.Style.Add("width", "15px")
'            mTbC.Style.Add("cursor", "hand")
'            mTbC.InnerText = "X"
'            mTbR.Cells.Add(mTbC)

'            mTb.Rows.Add(mTbR)

'            mTbR = New HtmlControls.HtmlTableRow
'            mTbC = New HtmlControls.HtmlTableCell
'            mTbC.ColSpan = 2
'            mTbC.InnerText = "test"
'            mTbC.Controls.Add(DataGrid)
'            mTbR.Cells.Add(mTbC)

'            mTb.Rows.Add(mTbR)

'            mDiv.Controls.Add(mTb)

'            Return mDiv
'        End Function
'    End Class

'End Namespace
