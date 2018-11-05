'Imports System
'Imports System.Collections.Generic
'Imports System.ComponentModel
'Imports System.Text
'Imports System.Web
'Imports System.Web.UI
'Imports System.Web.UI.WebControls


'Namespace _Library._Web.MultiControl.McScriptManager

'    <DefaultProperty("Text") _
'    , ToolboxData("<{0}:McScriptManager runat=server></{0}:McScriptManager>")> _
'    Public Class McScriptManager
'        Inherits WebControl
'        Private mToolbarVisible As Boolean = True
'        Private mQuoteBoxItems As New ListItemCollection
'        Private mInsertImageOnClick As String
'        Private mNoHtmlAreaOnAnonymous As Boolean = True

'        Protected Overrides Sub Finalize()
'            MyBase.Finalize()
'        End Sub

'#Region " Hidding Base Properties "

'        <Browsable(False)> _
'        Public Overrides Property EnableViewState() As Boolean
'            Get
'                Return MyBase.EnableViewState
'            End Get
'            Set(ByVal value As Boolean)
'                MyBase.EnableViewState = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property Visible() As Boolean
'            Get
'                Return MyBase.Visible
'            End Get
'            Set(ByVal value As Boolean)
'                MyBase.Visible = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property AccessKey() As String
'            Get
'                Return MyBase.AccessKey
'            End Get
'            Set(ByVal value As String)
'                MyBase.AccessKey = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property TabIndex() As Short
'            Get
'                Return MyBase.TabIndex
'            End Get
'            Set(ByVal value As Short)
'                MyBase.TabIndex = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BackColor() As System.Drawing.Color
'            Get
'                Return MyBase.BackColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)

'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderColor() As System.Drawing.Color
'            Get
'                Return MyBase.BorderColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                MyBase.BorderColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
'            Get
'                Return MyBase.BorderStyle
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
'                MyBase.BorderStyle = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
'            Get
'                Return MyBase.BorderWidth
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.Unit)
'                MyBase.BorderWidth = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property CssClass() As String
'            Get
'                Return MyBase.CssClass
'            End Get
'            Set(ByVal value As String)
'                MyBase.CssClass = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
'            Get
'                Return MyBase.Font
'            End Get
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ForeColor() As System.Drawing.Color
'            Get
'                Return MyBase.ForeColor
'            End Get
'            Set(ByVal value As System.Drawing.Color)
'                MyBase.ForeColor = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property EnableTheming() As Boolean
'            Get
'                Return MyBase.EnableTheming
'            End Get
'            Set(ByVal value As Boolean)
'                MyBase.EnableTheming = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property SkinID() As String
'            Get
'                Return MyBase.SkinID
'            End Get
'            Set(ByVal value As String)
'                MyBase.SkinID = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property ToolTip() As String
'            Get
'                Return MyBase.ToolTip
'            End Get
'            Set(ByVal value As String)
'                MyBase.ToolTip = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property Height() As System.Web.UI.WebControls.Unit
'            Get
'                Return MyBase.Height
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.Unit)
'                MyBase.Height = value
'            End Set
'        End Property

'        <Browsable(False)> _
'        Public Overrides Property Width() As System.Web.UI.WebControls.Unit
'            Get
'                Return MyBase.Width
'            End Get
'            Set(ByVal value As System.Web.UI.WebControls.Unit)
'                MyBase.Width = value
'            End Set
'        End Property


'#End Region

'        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
'            If MyBase.Enabled Then
'                'If Not DesignMode Then
'                '    If HTML_Area_ScriptsAllowed() Then
'                '        mHtmlTA = New htmlArea(MyBase.Page, "") 'HtmlArea scripts
'                '        'mHtmlTA.ToolbarVisible = ToolbarVisible
'                '        mHtmlTA.QuoteBoxItems = QuoteBoxItems
'                '        mHtmlTA.InsertImage_OnClick = InsertImage_OnClick
'                '        mHtmlTA.Register()
'                '    End If
'                'End If
'            Else

'                'Dim mCtrl As New HtmlControls.HtmlGenericControl
'                'mCtrl.InnerHtml = TextAreaDummyScript()
'                'mCtrl.ID = "HTMLAREADUMMYSCRIPT"
'                'If MyBase.Page.Header.FindControl(mCtrl.ID) Is Nothing Then
'                '    MyBase.Page.Header.Controls.Add(mCtrl)
'                'End If
'                'mCtrl = Nothing

'            End If
'            MyBase.OnLoad(e)
'        End Sub

'        'Private Function TextAreaDummyScript() As String
'        '    Dim mScript As New StringBuilder
'        '    mScript.AppendLine("<script language=""JavaScript1.2"" defer> ")
'        '    mScript.AppendLine("<!--")
'        '    mScript.AppendLine("function editor_generate(objname,NoToolBar,userConfig) {")
'        '    mScript.AppendLine("}")
'        '    mScript.AppendLine("-->")
'        '    mScript.AppendLine("</script>")
'        '    Return mScript.ToString
'        'End Function

'        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
'            If DesignMode Then
'                writer.Write(Text) '"MultiControls Script Manager")
'            Else
'                If MyBase.Enabled Then

'                    ''If MyBase.Page.Header.FindControl("WCC") Is Nothing Then
'                    ''    MyBase.Page.Header.Controls.Add(CtrlStatusScript("WCC"))
'                    ''End If
'                    'If Me.Page.FindControl("WCC") Is Nothing Then
'                    '    CtrlStatusScript("WCC").RenderControl(writer)
'                    'End If
'                Else
'                    ''If MyBase.Page.Header.FindControl("WCCD") Is Nothing Then
'                    ''    MyBase.Page.Header.Controls.Add(CtrlStatusDummyScript("WCCD"))
'                    ''End If
'                    'If Me.Page.FindControl("WCCD") Is Nothing Then
'                    '    CtrlStatusDummyScript("WCCD").RenderControl(writer)
'                    'End If
'                End If
'            End If
'        End Sub

'        <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> _
'        Property Text() As String
'            Get
'                Dim s As String = CStr(ViewState("Text"))
'                If s Is Nothing Then
'                    Return "McSM"
'                Else
'                    Return s
'                End If
'            End Get

'            Set(ByVal Value As String)
'                ViewState("Text") = Value
'            End Set
'        End Property

'#Region " Status Bar "

'        'Private Function CtrlStatusDummyScript(ByVal pID As String) As HtmlControls.HtmlGenericControl
'        '    Dim mCtrl As New HtmlControls.HtmlGenericControl
'        '    mCtrl.ID = pID
'        '    mCtrl.InnerHtml = StatusDummyScript()
'        '    Return mCtrl
'        'End Function

'        'Private Function StatusDummyScript() As String
'        '    Dim mScript As New StringBuilder
'        '    mScript.AppendLine("<script language=""javascript"">")
'        '    mScript.AppendLine("<!--")
'        '    mScript.AppendLine("function onSelectWcc(obj,target,pMax) {")
'        '    mScript.AppendLine("}")
'        '    mScript.AppendLine("function Wcc(obj,target,pMax) {")
'        '    mScript.AppendLine("}")
'        '    mScript.AppendLine("-->")
'        '    mScript.AppendLine("</script>")
'        '    Return mScript.ToString
'        'End Function

'        'Private Function CtrlStatusScript(ByVal pID As String) As HtmlControls.HtmlGenericControl
'        '    Dim mCtrl As New HtmlControls.HtmlGenericControl
'        '    mCtrl.ID = pID
'        '    mCtrl.InnerHtml = StatusScript()
'        '    Return mCtrl
'        'End Function

'        'Private Function StatusScript() As String
'        '    Dim mScript As New StringBuilder

'        '    'mScript.AppendLine("<script language=""vbscript"">")
'        '    'mScript.AppendLine("<!--")
'        '    'mScript.AppendLine("Function GetWordCount(ByVal strInput)")
'        '    'mScript.AppendLine("    Dim strTemp")
'        '    'mScript.AppendLine("    strTemp = Replace(strInput, vbTab, "" "")")
'        '    'mScript.AppendLine("    strTemp = Replace(strTemp, vbCr, "" "")")
'        '    'mScript.AppendLine("    strTemp = Replace(strTemp, vbLf, "" "")")
'        '    'mScript.AppendLine("    strTemp = Trim(strTemp)")
'        '    'mScript.AppendLine("    Do While InStr(1, strTemp, ""  "", 1) <> 0")
'        '    'mScript.AppendLine("        strTemp = Replace(strTemp, ""  "", "" "")")
'        '    'mScript.AppendLine("    Loop")
'        '    'mScript.AppendLine("    GetWordCount = UBound(Split(strTemp, "" "", -1, 1)) + 1")
'        '    'mScript.AppendLine("End Function")
'        '    'mScript.AppendLine("-->")
'        '    'mScript.AppendLine("</script>")



'        '    mScript.AppendLine("<script language=""javascript"">")
'        '    mScript.AppendLine("<!--")

'        '    mScript.AppendLine("function GetWordCount(pString) {")
'        '    mScript.AppendLine("    try{")
'        '    mScript.AppendLine("        var mStr = pString.replace(/\s+/g, ' ');")
'        '    mScript.AppendLine("        while (mStr.substring(0, 1) == ' ')")
'        '    mScript.AppendLine("            mStr = mStr.substring(1);")
'        '    mScript.AppendLine("        while (mStr.substring(mStr.length - 2, mStr.length - 1) == ' ')")
'        '    mScript.AppendLine("            mStr = mStr.substring(0, mStr.length - 1);")
'        '    mScript.AppendLine("        var mStr2 = mStr.split(' ');")
'        '    mScript.AppendLine("        return mStr2.length;")
'        '    mScript.AppendLine("    } catch (e) {}")
'        '    mScript.AppendLine("}")

'        '    mScript.AppendLine("function onSelectWcc(obj,target,pMax) {")
'        '    mScript.AppendLine("    try{")
'        '    mScript.AppendLine("        var mC = obj.value.length;")
'        '    mScript.AppendLine("        mC = mC - window.document.selection.createRange().text.length;")
'        '    mScript.AppendLine("        var mR = pMax-mC;")
'        '    mScript.AppendLine("        document.getElementById(target).innerHTML = '&nbsp;&nbsp;C:&nbsp;'+mC+'&nbsp;&nbsp;||&nbsp;&nbsp;R:&nbsp;<span '+((mR<0)?'style=""color:red""':'')+'>'+mR+'</span>'")
'        '    mScript.AppendLine("    } catch (e) {}")
'        '    mScript.AppendLine("}")

'        '    mScript.AppendLine("function Wcc(obj,target,pMax) {")
'        '    mScript.AppendLine("    try{")
'        '    mScript.AppendLine("        var mW = GetWordCount(obj.value);")
'        '    mScript.AppendLine("        var mC = obj.value.length;")
'        '    mScript.AppendLine("        var mR = pMax-mC;")
'        '    mScript.AppendLine("        document.getElementById(target).innerHTML = 'W:&nbsp;'+mW+'&nbsp;&nbsp;||&nbsp;&nbsp;C:&nbsp;'+mC+'&nbsp;&nbsp;||&nbsp;&nbsp;R:&nbsp;<span '+((mR<0)?'style=""color:red""':'')+'>'+mR+'</span>'")
'        '    mScript.AppendLine("    } catch (e) {}")
'        '    mScript.AppendLine("}")
'        '    mScript.AppendLine("-->")
'        '    mScript.AppendLine("</script>")
'        '    Return mScript.ToString
'        'End Function

'#End Region

'        '#Region " HTML Area "

'        '        <Browsable(False)> _
'        '        Public Function HTML_Area_ScriptsAllowed() As Boolean
'        '            If Not DesignMode Then
'        '                Dim mLg() As String
'        '                mLg = Split(MyBase.Page.Request.ServerVariables("REMOTE_USER"), "\")
'        '                If mLg.Length > 1 Then
'        '                    Return True
'        '                Else
'        '                    Return Not NoHtmlAreaOnAnonymous
'        '                End If
'        '            Else
'        '                Return False
'        '            End If
'        '        End Function

'        '        <Browsable(True), Category("HTML Area"), DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
'        '        Property NoHtmlAreaOnAnonymous() As Boolean
'        '            Get
'        '                Return mNoHtmlAreaOnAnonymous
'        '            End Get
'        '            Set(ByVal value As Boolean)
'        '                mNoHtmlAreaOnAnonymous = value
'        '            End Set
'        '        End Property

'        '        <Browsable(False), Category("HTML Area"), DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
'        '        Private Property ToolbarVisible() As Boolean
'        '            Get
'        '                Return mToolbarVisible
'        '            End Get
'        '            Set(ByVal value As Boolean)
'        '                mToolbarVisible = value
'        '            End Set
'        '        End Property

'        '        <Browsable(True), Category("HTML Area"), DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
'        '        Public Property InsertImage_OnClick() As String
'        '            Get
'        '                Return mInsertImageOnClick
'        '            End Get
'        '            Set(ByVal value As String)
'        '                mInsertImageOnClick = value
'        '            End Set
'        '        End Property

'        '        <DefaultValue(""), Category("HTML Area"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint), _
'        '        DesignerSerializationVisibility( _
'        '        DesignerSerializationVisibility.Content), _
'        '        PersistenceMode(PersistenceMode.InnerProperty) _
'        '        > _
'        '        Public ReadOnly Property QuoteBoxItems() As System.Web.UI.WebControls.ListItemCollection
'        '            Get
'        '                Return mQuoteBoxItems
'        '            End Get
'        '        End Property

'        '#End Region

'    End Class
'End Namespace
