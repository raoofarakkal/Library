#Region " README "
'Avoide following code in aspx file to float the console on top left 
'of the screen while scrolling the page 
'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
#End Region

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Security.Permissions
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Design 'add Referrence to system.design
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports AJCMS.Library
Imports _Library._Web
Imports _Library._Web.MultiControl
Imports _Library._System._Security
Imports _Library._Web._Common

Namespace AJCMS

#Region " ENUM "
    Public Enum AJCMSConsoleMode
        'None = 0 '230519710
        Live = 1 '230519712
        Authoring = 2 '230519711
        Edit = 3 '230519713
        Preview = 4 '230519714
    End Enum

    Public Enum AJCMSConsoleItemVisibility
        Enabled = 0
        Disabled = 1
        Hidden = 2
    End Enum

    Public Enum AJCMconsoleSaveActions
        _Save = 1
        _SaveAndExit = 2
    End Enum

    Public Enum AJCMSconsoleEventTarget
        _S2Auth = 1
        _S2Live = 2
        _S2Edit = 3
        _S2Preview = 4

        _ShwPostPro = 5
        _DelPost = 6
        _CreatePost = 7
        _ShwSiteMap = 8
        _ShwAdmPnl = 9
        _SavePostNoExit = 10
        _SavePostExit = 11
        _ExitWoSave = 12
        _B2Edit = 13

        _CopyPost = 14
        _MovePost = 15

    End Enum
#End Region

#Region " Console - Floating style "

    'NOTE:
    '       If you remove the following line from ASPX file, 
    '       Console will not stay on top while scrolling the page down or up
    '       mouse over color also not work
    '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <Designer(GetType(FloatingConsoleDesigner)) _
    , DefaultProperty("Mode") _
    , Category("Standard") _
    , DefaultEvent("onModeChange") _
    , ToolboxData("<{0}:FloatingConsole runat=server></{0}:FloatingConsole>") _
    > _
    Public Class FloatingConsole
        Inherits CompositeControl

#Region " Constants "
        Const cnslEVENTTARGET As String = "__cnslEVENTTARGET"
        Const cnslEVENTARGUMENT As String = "__cnslEVENTARGUMENT"
        Const cnslXY As String = "__cnslXY"
#End Region

#Region " Public Events declaration "
        Public Event onModeChange(ByVal sender As Object, ByVal e As System.EventArgs, ByVal PreviousMode As AJCMSConsoleMode, ByVal CurrentMode As AJCMSConsoleMode)
        Public Event onAuthoringMode(ByVal PreviousMode As AJCMSConsoleMode)
        Public Event onLiveMode(ByVal PreviousMode As AJCMSConsoleMode)
        Public Event onEditMode(ByVal PreviousMode As AJCMSConsoleMode)
        Public Event onPreviewMode(ByVal PreviousMode As AJCMSConsoleMode)
        Public Event onShowPostingProp()

        Public Event onBeforeDeletePosting(ByRef Cancel As Boolean)
        Public Event onBeforeMovePosting(ByRef Cancel As Boolean)
        Public Event onBeforeCopyPosting(ByRef Cancel As Boolean)
        Public Event onAfterCopyPosting(ByRef ShowProperties As Boolean)
        Public Event onBeforeSavePosting(ByRef LastModifiedDateOptions As LMD_Options, ByRef Cancel As Boolean, ByVal SaveAction As AJCMconsoleSaveActions)
        Public Event onAfterSavePosting(ByRef ShowProperties As Boolean, ByVal SaveAction As AJCMconsoleSaveActions)

        Public Event onCreatePosting()
        Public Event onShowSiteMap(ByRef SelectedChannelGUID As String)
        Public Event onShowAdminPan()
        Public Event onExitWithoutSave(ByRef Cancel As Boolean, ByRef ConsoleMode As AJCMSConsoleMode)

        Public Event OnLastError(ByVal ex As Exception)

        Public Event ChildControlEvents(ByVal source As Object, ByVal args As System.EventArgs)
#End Region

#Region " private Variables/Dynamic Controls Declaration "
        Private mBasePage As AJCMS.BasePage

        Private mHtml As HtmlTags
        Private _Authoring As ITemplate
        Private _Live As ITemplate
        Private _Edit As ITemplate
        Private _Preview As ITemplate
        Private _owner As TemplateOwner
        Private mFS As FlotingScript

        Private mlnkS2Auth As MyLinkButton
        Private mlnkS2Live As MyLinkButton
        Private mlnkS2Edit As MyLinkButton
        Private mlnkS2Preview As MyLinkButton

        Private mlnkShwPostPro As MyLinkButton
        Private mlnkDelPost As MyLinkButton
        Private mlnkCopyPost As MyLinkButton
        Private mlnkMovePost As MyLinkButton

        Private mlnkCreatePost As MyLinkButton
        Private mlnkShwSiteMap As MyLinkButton
        Private mlnkShwAdmPnl As MyLinkButton
        Private mlnkSavePostExit As MyLinkButton
        Private mlnkSavePostNoExit As MyLinkButton
        Private mlnkExitWoSave As MyLinkButton
        Private mlnkB2Edit As MyLinkButton
#End Region

        Private Sub _OnLastError(ByVal ex As Exception)
            RaiseEvent OnLastError(ex)
            If System.Web.HttpContext.Current.IsDebuggingEnabled Then
                MyBase.Page.Response.Write("<BR>FloatingConsole.OnLastError(). <b>" & ex.Message & "</b><BR>" & Replace(ex.InnerException.Message, vbCrLf, "<BR>") & "<BR><HR>")
            End If
        End Sub

        Public Sub New()
            mBasePage = New AJCMS.BasePage
            mHtml = New HtmlTags
            mFS = New FlotingScript(cnslXY)

            mlnkS2Auth = New MyLinkButton
            mlnkS2Auth.CssClass = Me.CssItems

            mlnkS2Live = New MyLinkButton
            mlnkS2Live.CssClass = Me.CssItems

            mlnkS2Edit = New MyLinkButton
            mlnkS2Edit.CssClass = Me.CssItems

            mlnkS2Preview = New MyLinkButton
            mlnkS2Preview.CssClass = Me.CssItems

            mlnkShwPostPro = New MyLinkButton
            mlnkShwPostPro.CssClass = Me.CssItems

            mlnkDelPost = New MyLinkButton
            mlnkDelPost.CssClass = Me.CssItems

            mlnkCopyPost = New MyLinkButton
            mlnkCopyPost.CssClass = Me.CssItems

            mlnkMovePost = New MyLinkButton
            mlnkMovePost.CssClass = Me.CssItems

            mlnkCreatePost = New MyLinkButton
            mlnkCreatePost.CssClass = Me.CssItems

            mlnkShwSiteMap = New MyLinkButton
            mlnkShwSiteMap.CssClass = Me.CssItems

            mlnkShwAdmPnl = New MyLinkButton
            mlnkShwAdmPnl.CssClass = Me.CssItems

            mlnkSavePostExit = New MyLinkButton
            mlnkSavePostExit.CssClass = Me.CssItems

            mlnkSavePostNoExit = New MyLinkButton
            mlnkSavePostNoExit.CssClass = Me.CssItems

            mlnkExitWoSave = New MyLinkButton
            mlnkExitWoSave.CssClass = Me.CssItems

            mlnkB2Edit = New MyLinkButton
            mlnkB2Edit.CssClass = Me.CssItems

        End Sub

        Public Overrides Sub Dispose()
            CleanUp()
            MyBase.Dispose()
        End Sub

        Private Sub CleanUp()

            _Authoring = Nothing
            _Live = Nothing
            _Edit = Nothing
            _Preview = Nothing
            _owner = Nothing
            mFS = Nothing
            mHtml = Nothing
            mBasePage = Nothing

            mlnkS2Auth = Nothing
            mlnkS2Live = Nothing
            mlnkS2Edit = Nothing
            mlnkS2Preview = Nothing

            mlnkShwPostPro = Nothing
            mlnkDelPost = Nothing
            mlnkCopyPost = Nothing
            mlnkMovePost = Nothing
            mlnkCreatePost = Nothing
            mlnkShwSiteMap = Nothing
            mlnkShwAdmPnl = Nothing
            mlnkSavePostExit = Nothing
            mlnkSavePostNoExit = Nothing
            mlnkExitWoSave = Nothing
            mlnkB2Edit = Nothing

            GC.Collect()
            MyBase.Finalize()
        End Sub

#Region " Console Event related Actions "

        Private Sub SwitchToAuth(ByVal sender As Object, ByVal e As String)
            Dim mPrvMode As AJCMSConsoleMode = Mode
            Mode = AJCMSConsoleMode.Authoring
            RaiseEvent onAuthoringMode(mPrvMode)
        End Sub

        Private Sub SwitchToLive(ByVal sender As Object, ByVal e As String)
            Dim mPrvMode As AJCMSConsoleMode = Mode
            Mode = AJCMSConsoleMode.Live
            RaiseEvent onLiveMode(mPrvMode)
        End Sub

        Private Sub SwitchToEdit(ByVal sender As Object, ByVal e As String)
            If mBasePage.Current.PostingGUID <> "" Then
                Dim mPrvMode As AJCMSConsoleMode = Mode
                Mode = AJCMSConsoleMode.Edit
                RaiseEvent onEditMode(mPrvMode)
            Else
                msgbox1("Posting GUID not found", MyBase.Page)
            End If
        End Sub

        '''TODO Still under development: to avoid display again and again when user press the back button {
        Private Function msgbox1(ByVal pMessage As String, ByVal pPage As Web.UI.Page) As Boolean
            Dim mScript As New StringBuilder
            Dim mMsg As String = pMessage
            mMsg = Replace(mMsg, "'", "\'")
            mMsg = Replace(mMsg, """", "\""")
            mScript.AppendLine("<script language='javascript' type='text/javascript'><!--")
            'mScript.AppendLine("alert(document.getElementById('__TEST').value);")
            mScript.AppendLine("if(document.getElementById('__TEST').value==1) {")
            mScript.AppendLine("    alert('" & mMsg & "');")
            mScript.AppendLine("    document.getElementById('__TEST').value=0;")
            mScript.AppendLine("}")
            mScript.AppendLine("--></script>")
            pPage.ClientScript.RegisterStartupScript(pPage.GetType, "MESSAGE", mScript.ToString)
            mBasePage.ClientScript.RegisterHiddenField("__TEST", "1")
        End Function
        '}

        Private Sub Back2Edit(ByVal sender As Object, ByVal e As String)
            Dim mPrvMode As AJCMSConsoleMode = Mode
            Mode = AJCMSConsoleMode.Edit
            RaiseEvent onEditMode(mPrvMode)
        End Sub

        Private Sub SwitchToPreview(ByVal sender As Object, ByVal e As String)
            Dim mPrvMode As AJCMSConsoleMode = Mode
            Mode = AJCMSConsoleMode.Preview
            RaiseEvent onPreviewMode(mPrvMode)
        End Sub

        Private Sub CreatePosting(ByVal sender As Object, ByVal e As String)
            RaiseEvent onCreatePosting()
        End Sub

        Private Sub ShowAdminPan(ByVal sender As Object, ByVal e As String)
            RaiseEvent onShowAdminPan()
        End Sub

        Private Sub ShowSiteMap(ByVal sender As Object, ByVal e As String)
            Dim mChGuid As String = mBasePage.Current.ChannelGUID
            RaiseEvent onShowSiteMap(mChGuid)
            Dim mC As New Config 'Posting
            Dim mSiteVD As String = mC.SiteVirtualDir(mBasePage.Current.SiteID)
            Dim mQsb As New Config.QueryStringBuilder
            mQsb.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowNoPostingPage
            If mChGuid <> "" Then
                mQsb.ChannelGUID = mChGuid
            End If
            mQsb.SiteID = mBasePage.Current.SiteID
            Dim mQrStr As String = Config.QueryStringBuilder.Create(mQsb)
            mQsb = Nothing
            'MyBase.Page.Response.Redirect(mP.Config.WebServerURL() & "/" & mSiteVD & "/" & mP.Config.NoPostingURL & mQrStr)
            clsLib.RedirectURL(MyBase.Page, mC.WebServerURL() & "/" & mSiteVD & "/" & mC.NoPostingURL & mQrStr)
        End Sub

        Private Sub ExitWithoutSave(ByVal sender As Object, ByVal e As String)
            Dim mCancel As Boolean = False
            Dim mCnslmode As AJCMSConsoleMode = AJCMSConsoleMode.Authoring
            RaiseEvent onExitWithoutSave(mCancel, mCnslmode)
            Me.Mode = mCnslmode
            'mBasePage.Session("CNSLMD") = mCnslmode
            If Not mCancel Then
                If mBasePage.Current.PreviousURL IsNot Nothing Then
                    'MyBase.Page.Response.Redirect(mBasePage.Current.PreviousURL)
                    clsLib.RedirectURL(MyBase.Page, mBasePage.Current.PreviousURL)
                ElseIf mBasePage.Current.PostingGUID <> "" Then
                    Dim mP As New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    Dim mURL As String = ""
                    If mP.Load(mBasePage.Current.PostingGUID) Then
                        mURL = mP.URL
                    Else
                        mURL = Config.SiteAddress
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    'MyBase.Page.Response.Redirect(mURL)
                    clsLib.RedirectURL(MyBase.Page, mURL)
                Else
                    'MyBase.Page.Response.Redirect(Config.SiteAddress) '  mConf.NoPostingURL & mPara)
                    clsLib.RedirectURL(MyBase.Page, Config.SiteAddress) '  mConf.NoPostingURL & mPara)
                End If
            End If
        End Sub

        Private Sub ShowPostingProp(ByVal sender As Object, ByVal e As String)
            RaiseEvent onShowPostingProp()
        End Sub

        Private Sub _EditPost(ByRef mPosting As Posting, ByVal pSaveAction As AJCMconsoleSaveActions)
            Dim mRow As Data.DataRow
            Dim mCtrl As New Object
            Dim mCtrlText As String = ""
            If mPosting.Template.TemplateDefinition.Count > 0 Then
                mPosting.PostingData.Clear()
                mRow = mPosting.PostingData.NewRow
                For cnt As Integer = 0 To mPosting.PostingData.Columns.Count - 1
                    If mPosting.PostingData.Columns(cnt).ColumnName.ToUpper.Trim = "POSTING_GUID" Then
                        mRow.Item(mPosting.PostingData.Columns(cnt).ColumnName) = mPosting.GUID
                    Else
                        Try
                            mCtrlText = ""
                            mCtrl = MyBase.Page.FindControl(mPosting.PostingData.Columns(cnt).ColumnName)
                            If mCtrl IsNot Nothing Then
                                If mCtrl.GetType.Name = "MultiControl" Then
                                    mCtrlText = CType(mCtrl, MultiControl).Text
                                ElseIf mCtrl.GetType.Name = "TinyMCE" Then
                                    mCtrlText = CType(mCtrl, TinyMCE).Text
                                End If
                            Else
                                Dim mMCs As New List(Of MultiControl)
                                mMCs = _Library._Web._Common.FindMultiControls(MyBase.Page)
                                If mMCs IsNot Nothing Then
                                    For Each Mc As MultiControl In mMCs
                                        If Mc.ID.ToLower = mPosting.PostingData.Columns(cnt).ColumnName.ToLower Then
                                            mCtrlText = Mc.Text
                                        End If
                                    Next
                                End If
                                mMCs = Nothing

                                Dim mTMCEs As New List(Of TinyMCE)
                                mTMCEs = TinyMCE.FindTinyMCEControls(MyBase.Page)
                                If mTMCEs IsNot Nothing Then
                                    For Each TMCE As TinyMCE In mTMCEs
                                        If TMCE.ID.ToLower = mPosting.PostingData.Columns(cnt).ColumnName.ToLower Then
                                            mCtrlText = TMCE.Text
                                        End If
                                    Next
                                End If
                                mTMCEs = Nothing
                            End If
                            mCtrl = Nothing
                            mRow.Item(mPosting.PostingData.Columns(cnt).ColumnName) = mCtrlText
                        Catch
                        End Try

                    End If
                Next
                mPosting.PostingData.Rows.Add(mRow)
            End If
            mPosting.State = State.Saved
            Dim mCancel As Boolean = False
            RaiseEvent onBeforeSavePosting(mPosting.LastModifiedDateOptions, mCancel, pSaveAction)
            If Not mCancel Then
                If mPosting.Save() Then
                    Dim mShowProperties As Boolean = True
                    RaiseEvent onAfterSavePosting(mShowProperties, pSaveAction)
                    If mShowProperties Then
                        'SHOW Posting Properties
                        Dim mQrStr As String
                        Dim mConf As New AJCMS.Library.Config
                        Dim mQsb As New Config.QueryStringBuilder
                        mQsb.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowPostingProp
                        mQsb.PostingGUID = mPosting.GUID
                        mQsb.SiteID = mBasePage.Current.SiteID
                        mQsb.ChannelGUID = mBasePage.Current.ChannelGUID.Trim
                        mQrStr = Config.QueryStringBuilder.Create(mQsb)
                        ShowModalWindow(Config.SiteAddress & mConf.PostingPropURL & mQrStr, "PostingProp") ' "/Console/PostingProp.aspx?pEDIT,p;" & Me.PostingGUID.Trim)
                        mQsb = Nothing
                        mConf = Nothing
                    Else
                        If pSaveAction = AJCMconsoleSaveActions._SaveAndExit Then
                            Mode = AJCMSConsoleMode.Authoring
                        End If
                    End If
                    'mBasePage.Session("CNSLMD") = AJCMSConsoleMode.Authoring
                Else
                    Message(mPosting.LastError, MyBase.Page)
                End If
            End If
        End Sub

        Private Sub _AddPost(ByVal pSaveAction As AJCMconsoleSaveActions)
            Dim mConf As New AJCMS.Library.ChannelItems
            If mBasePage.Current.ChannelGUID <> "" Then
                Dim mTempID As String = ""
                Dim mCtrlText As String = ""
                Dim mCtrl As New Object
                mTempID = mBasePage.Current.TemplateGUID
                If mTempID <> "" Then
                    Dim mPosting As New Posting(mTempID, mBasePage.Current.ChannelGUID, mBasePage.Current.PostingGUID)
                    AddHandler mPosting.OnLastError, AddressOf _OnLastError
                    mPosting.DisplayName = mPosting.GUID ' Date.Today.ToShortDateString & ":" & Int(Now.TimeOfDay.TotalMilliseconds) 'mPosting.Parent.DisplayName & Date.Today.ToString & Now.TimeOfDay.ToString
                    mPosting.Name = mPosting.DisplayName ' mPosting.DisplayName 'mPosting.Parent.Name & Date.Today.ToString & Now.TimeOfDay.ToString
                    If mPosting.Template.TemplateDefinition.Count > 0 Then
                        Dim mRow As Data.DataRow
                        mRow = mPosting.PostingData.NewRow
                        For cnt As Integer = 0 To mPosting.PostingData.Columns.Count - 1
                            If mPosting.PostingData.Columns(cnt).ColumnName.ToUpper.Trim = "POSTING_GUID" Then
                                mRow.Item(mPosting.PostingData.Columns(cnt).ColumnName) = mPosting.GUID
                            Else
                                Try
                                    mCtrlText = ""
                                    mCtrl = MyBase.Page.FindControl(mPosting.PostingData.Columns(cnt).ColumnName)
                                    If mCtrl IsNot Nothing Then
                                        If mCtrl.GetType.Name = "MultiControl" Then
                                            mCtrlText = CType(mCtrl, MultiControl).Text
                                        ElseIf mCtrl.GetType.Name = "TinyMCE" Then
                                            mCtrlText = CType(mCtrl, TinyMCE).Text
                                        End If
                                    Else
                                        Dim mMCs As New List(Of MultiControl)
                                        mMCs = _Library._Web._Common.FindMultiControls(MyBase.Page)
                                        If mMCs IsNot Nothing Then
                                            For Each Mc As MultiControl In mMCs
                                                If Mc.ID.ToLower = mPosting.PostingData.Columns(cnt).ColumnName.ToLower Then
                                                    mCtrlText = Mc.Text
                                                End If
                                            Next
                                        End If
                                        mMCs = Nothing

                                        Dim mTMCEs As New List(Of TinyMCE)
                                        mTMCEs = TinyMCE.FindTinyMCEControls(MyBase.Page)
                                        If mTMCEs IsNot Nothing Then
                                            For Each TMCE As TinyMCE In mTMCEs
                                                If TMCE.ID.ToLower = mPosting.PostingData.Columns(cnt).ColumnName.ToLower Then
                                                    mCtrlText = TMCE.Text
                                                End If
                                            Next
                                        End If
                                        mTMCEs = Nothing

                                    End If
                                    mCtrl = Nothing
                                    mRow.Item(mPosting.PostingData.Columns(cnt).ColumnName) = mCtrlText
                                Catch
                                End Try
                            End If
                        Next
                        mPosting.PostingData.Rows.Add(mRow)
                    End If
                    mPosting.State = State.Saved
                    Dim mCancel As Boolean = False
                    RaiseEvent onBeforeSavePosting(mPosting.LastModifiedDateOptions, mCancel, pSaveAction)
                    If Not mCancel Then
                        If mPosting.Save() Then
                            Dim mShowProperties As Boolean = True
                            RaiseEvent onAfterSavePosting(mShowProperties, pSaveAction)
                            If mShowProperties Then
                                'SHOW posting properties
                                Dim mQrStr2 As String
                                Dim mConf2 As New AJCMS.Library.Config
                                Dim mQsb2 As New Config.QueryStringBuilder
                                mQsb2.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowPostingProp
                                mQsb2.PostingGUID = mPosting.GUID
                                mQsb2.SiteID = mBasePage.Current.SiteID
                                mQsb2.ChannelGUID = mBasePage.Current.ChannelGUID.Trim
                                mQrStr2 = Config.QueryStringBuilder.Create(mQsb2)
                                ShowModalWindow(Config.SiteAddress & mConf2.PostingPropURL & mQrStr2, "PostingProp") ' "/Console/PostingProp.aspx?pEDIT,p;" & Me.PostingGUID.Trim)
                                mQsb2 = Nothing
                                mConf2 = Nothing
                            Else
                                If pSaveAction = AJCMconsoleSaveActions._SaveAndExit Then
                                    Mode = AJCMSConsoleMode.Authoring
                                End If
                            End If
                            'mBasePage.Session("CNSLMD") = AJCMSConsoleMode.Authoring
                        Else
                            Message(mPosting.LastError, MyBase.Page)
                        End If
                    End If
                    RemoveHandler mPosting.OnLastError, AddressOf _OnLastError
                    mPosting = Nothing
                Else
                    Message("Template GUID not found", mBasePage)
                End If
            Else
                Message("Channel GUID not found", mBasePage)
            End If
        End Sub

        Private Sub SavePostingExit(ByVal sender As Object, ByVal e As String)
            If mBasePage.Current.PostingGUID <> "" Then
                Dim mPosting As New Posting
                AddHandler mPosting.OnLastError, AddressOf _OnLastError
                If mPosting.Load(mBasePage.Current.PostingGUID) Then 'Edit
                    _EditPost(mPosting, AJCMconsoleSaveActions._SaveAndExit)
                Else 'ADD
                    If mBasePage.Current.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.CreatePostingByConsoleFromSelectTemplate Then
                        _AddPost(AJCMconsoleSaveActions._SaveAndExit)
                    Else
                        Message("PostingGUID invalid or not found", mBasePage)
                    End If
                End If
                RemoveHandler mPosting.OnLastError, AddressOf _OnLastError
                mPosting = Nothing
            Else
                Message("PostingGUID not found", mBasePage)
            End If
        End Sub

        Private Sub SavePostingNoExit(ByVal sender As Object, ByVal e As String)
            If mBasePage.Current.PostingGUID <> "" Then
                Dim mPosting As New Posting
                AddHandler mPosting.OnLastError, AddressOf _OnLastError
                If mPosting.Load(mBasePage.Current.PostingGUID) Then 'Edit
                    _EditPost(mPosting, AJCMconsoleSaveActions._Save)
                Else 'ADD
                    If mBasePage.Current.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.CreatePostingByConsoleFromSelectTemplate Then
                        _AddPost(AJCMconsoleSaveActions._Save)
                    Else
                        Message("PostingGUID invalid or not found", mBasePage)
                    End If
                End If
                RemoveHandler mPosting.OnLastError, AddressOf _OnLastError
                mPosting = Nothing
            Else
                Message("PostingGUID not found", mBasePage)
            End If
        End Sub

        Private Sub CopyPosting(ByVal sender As Object, ByVal e As String)
            Dim mCancel As Boolean = False
            RaiseEvent onBeforeCopyPosting(mCancel)
            If Not mCancel Then
                If mBasePage.Current.PostingGUID <> "" Then
                    Dim mOldPost As New Posting
                    AddHandler mOldPost.OnLastError, AddressOf _OnLastError
                    Dim mQrStr As String = ""
                    Dim mNewPost As Posting
                    If mOldPost.Load(mBasePage.Current.PostingGUID) Then
                        mNewPost = mOldPost.Copy
                        If mNewPost IsNot Nothing Then
                            Dim mShowProperties As Boolean = True
                            RaiseEvent onAfterCopyPosting(mShowProperties)
                            If mShowProperties Then
                                'SHOW posting properties
                                Dim mQrStr2 As String
                                Dim mConf2 As New AJCMS.Library.Config
                                Dim mQsb2 As New Config.QueryStringBuilder
                                mQsb2.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowPostingProp
                                mQsb2.PostingGUID = mNewPost.GUID
                                mQsb2.SiteID = mBasePage.Current.SiteID
                                mQsb2.ChannelGUID = mBasePage.Current.ChannelGUID.Trim
                                mQrStr2 = Config.QueryStringBuilder.Create(mQsb2)
                                ShowModalWindow(Config.SiteAddress & mConf2.PostingPropURL & mQrStr2, "PostingProp") ' "/Console/PostingProp.aspx?pEDIT,p;" & Me.PostingGUID.Trim)
                                mQsb2 = Nothing
                                mConf2 = Nothing
                            Else
                                'MyBase.Page.Response.Redirect(mNewPost.URL)
                                clsLib.RedirectURL(MyBase.Page, mNewPost.URL)
                            End If
                        Else
                            Message("Copy process failed!. " & mOldPost.LastError, MyBase.Page)
                        End If
                    Else
                        Message("Could not load current posting!. " & mOldPost.LastError, MyBase.Page)
                    End If
                    RemoveHandler mOldPost.OnLastError, AddressOf _OnLastError
                    mOldPost = Nothing
                Else
                    Message("PostingGUID invalid or not found", MyBase.Page)
                End If
            End If
        End Sub

        Private Sub MovePosting(ByVal sender As Object, ByVal e As String)
            Dim mCancel As Boolean = False
            RaiseEvent onBeforeMovePosting(mCancel)
            If Not mCancel Then
                Dim mQrStr2 As String
                Dim mConf2 As New AJCMS.Library.Config
                Dim mQsb2 As New Config.QueryStringBuilder
                mQsb2.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.MovePosting
                mQsb2.PostingGUID = mBasePage.Current.PostingGUID
                mQsb2.SiteID = mBasePage.Current.SiteID
                mQsb2.ChannelGUID = mBasePage.Current.ChannelGUID
                mQrStr2 = Config.QueryStringBuilder.Create(mQsb2)
                ShowModalWindow(Config.SiteAddress & mConf2.MovePostingURL & mQrStr2, "MovePost") ' "/Console/PostingProp.aspx?pEDIT,p;" & Me.PostingGUID.Trim)
                mQsb2 = Nothing
                mConf2 = Nothing
            End If
        End Sub

        Private Sub DeletePosting(ByVal sender As Object, ByVal e As String)
            Dim mCancel As Boolean = False
            RaiseEvent onBeforeDeletePosting(mCancel)
            If Not mCancel Then
                If mBasePage.Current.PostingGUID <> "" Then 'Mark for Deletion
                    Dim mPosting As New Posting
                    AddHandler mPosting.OnLastError, AddressOf _OnLastError
                    Dim mDefaultPostingGuid As String = ""
                    Dim mLastPostingGuid As String = ""
                    Dim mQrStr As String = ""
                    Dim mP As Posting
                    If mPosting.Load(mBasePage.Current.PostingGUID) Then
                        mDefaultPostingGuid = mPosting.Parent.DefaultPosting
                        mLastPostingGuid = mPosting.Parent.LastPosting
                        If mPosting.Delete Then
                            If mDefaultPostingGuid.Length > 0 Then
                                mP = New Posting
                                AddHandler mP.OnLastError, AddressOf _OnLastError
                                If mP.Load(mDefaultPostingGuid) Then
                                    mQrStr = mP.URL
                                End If
                                RemoveHandler mP.OnLastError, AddressOf _OnLastError
                                mP = Nothing
                            ElseIf mLastPostingGuid.Length > 0 Then
                                mP = New Posting
                                AddHandler mP.OnLastError, AddressOf _OnLastError
                                If mP.Load(mLastPostingGuid) Then
                                    mQrStr = mP.URL
                                End If
                                RemoveHandler mP.OnLastError, AddressOf _OnLastError
                                mP = Nothing
                            End If
                            RemoveHandler mPosting.OnLastError, AddressOf _OnLastError
                            If mQrStr <> "" Then
                                clsLib.RedirectURL(MyBase.Page, mQrStr)
                            Else
                                ShowSiteMap(Nothing, Nothing)
                            End If
                        Else
                            Message("Delete process failed!. " & mPosting.LastError, MyBase.Page)
                        End If
                    Else
                        MyBase.Page.ClientScript.RegisterStartupScript(Me.GetType, "ALERT", "<script language='javascript'>alert('PostingGUID invalid or not found');</script>")
                    End If
                    RemoveHandler mPosting.OnLastError, AddressOf _OnLastError
                    mPosting = Nothing
                End If
            End If
        End Sub


#End Region

        Private Sub ShowModalWindow(ByVal URL As String, ByVal pWinName As String)
            Dim mScript As New StringBuilder
            mScript.AppendLine("")
            mScript.AppendLine("<script type='text/javascript'>")
            mScript.AppendLine("var ReturnedValue = window.open('" & URL & "','" & pWinName & "','toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,resizable=yes,height=400,width=700,left=100, top=100')")
            mScript.AppendLine("ReturnedValue.focus()")
            mScript.AppendLine("</script>")
            MyBase.Page.ClientScript.RegisterStartupScript(Me.GetType, "ModalWindow", mScript.ToString)
        End Sub

        Private Sub ChangeMultiControlsMode(ByVal pMode As AJCMSConsoleMode)
            If mBasePage.Current.Command <> Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowNoPostingPage Then
                Dim mTemplate As New Template
                AddHandler mTemplate.OnLastError, AddressOf _OnLastError
                mTemplate.Load(mBasePage.Current.TemplateGUID)
                Dim mCtrl As New Object
                Dim mCtrlsNotFound As String = ""
                Dim mComma As String = ""
                For cnt As Integer = 1 To mTemplate.TemplateDefinition.NoOfControls
                    mTemplate.TemplateDefinition.SelectRow(cnt - 1)
                    Try
                        'mCtrl = CType(MyBase.Page.FindControl(mTemplate.TemplateDefinition.ControlName), MultiControl)
                        mCtrl = MyBase.Page.FindControl(mTemplate.TemplateDefinition.ControlName)
                        If mCtrl IsNot Nothing Then
                            If mCtrl.GetType.Name = "MultiControl" Then
                                Select Case pMode
                                    Case AJCMSConsoleMode.Authoring
                                        CType(mCtrl, MultiControl).Mode = MultiControl.AJCMS_ControlMode.View
                                    Case AJCMSConsoleMode.Edit
                                        CType(mCtrl, MultiControl).Mode = MultiControl.AJCMS_ControlMode.Edit
                                    Case AJCMSConsoleMode.Preview
                                        CType(mCtrl, MultiControl).Mode = MultiControl.AJCMS_ControlMode.View
                                    Case Else
                                        CType(mCtrl, MultiControl).Mode = MultiControl.AJCMS_ControlMode.View
                                End Select
                            ElseIf mCtrl.GetType.Name = "TinyMCE" Then
                                Select Case pMode
                                    Case AJCMSConsoleMode.Authoring
                                        CType(mCtrl, TinyMCE).Mode = TinyMCE.TinyMCE_Mode.View
                                    Case AJCMSConsoleMode.Edit
                                        CType(mCtrl, TinyMCE).Mode = TinyMCE.TinyMCE_Mode.Edit
                                    Case AJCMSConsoleMode.Preview
                                        CType(mCtrl, TinyMCE).Mode = TinyMCE.TinyMCE_Mode.View
                                    Case Else
                                        CType(mCtrl, TinyMCE).Mode = TinyMCE.TinyMCE_Mode.View
                                End Select

                            End If
                        Else
                            mCtrlsNotFound += mComma & mTemplate.TemplateDefinition.ControlName
                            mComma = ","
                        End If
                        mCtrl = Nothing
                    Catch
                        'No Error to user
                    End Try
                Next
                Try
                    If mCtrlsNotFound <> "" Then
                        Dim mMCs As New List(Of MultiControl)
                        mMCs = _Library._Web._Common.FindMultiControls(MyBase.Page)
                        If mMCs IsNot Nothing Then
                            For Each mCtrName As String In mCtrlsNotFound.Split(",")
                                For Each Mc As MultiControl In mMCs
                                    If Mc.ID.ToLower = mCtrName.ToLower Then
                                        Select Case pMode
                                            Case AJCMSConsoleMode.Authoring
                                                Mc.Mode = MultiControl.AJCMS_ControlMode.View
                                            Case AJCMSConsoleMode.Edit
                                                Mc.Mode = MultiControl.AJCMS_ControlMode.Edit
                                            Case AJCMSConsoleMode.Preview
                                                Mc.Mode = MultiControl.AJCMS_ControlMode.View
                                            Case Else
                                                Mc.Mode = MultiControl.AJCMS_ControlMode.View
                                        End Select
                                    End If
                                Next
                            Next
                        End If
                        mMCs = Nothing

                        Dim mTMCEs As New List(Of TinyMCE)
                        mTMCEs = TinyMCE.FindTinyMCEControls(MyBase.Page)
                        If mTMCEs IsNot Nothing Then
                            For Each mCtrName As String In mCtrlsNotFound.Split(",")
                                For Each mTMCE As TinyMCE In mTMCEs
                                    If mTMCE.ID.ToLower = mCtrName.ToLower Then
                                        Select Case pMode
                                            Case AJCMSConsoleMode.Authoring
                                                mTMCE.Mode = TinyMCE.TinyMCE_Mode.View
                                            Case AJCMSConsoleMode.Edit
                                                mTMCE.Mode = TinyMCE.TinyMCE_Mode.Edit
                                            Case AJCMSConsoleMode.Preview
                                                mTMCE.Mode = TinyMCE.TinyMCE_Mode.View
                                            Case Else
                                                mTMCE.Mode = TinyMCE.TinyMCE_Mode.View
                                        End Select
                                    End If
                                Next
                            Next
                        End If
                        mMCs = Nothing

                    End If
                Catch ex As Exception
                    'No Error to user
                End Try
                RemoveHandler mTemplate.OnLastError, AddressOf _OnLastError
                mTemplate = Nothing
            End If
        End Sub

        Private Function ModeToStr(ByVal pMode As AJCMSConsoleMode) As String
            Select Case pMode
                Case AJCMSConsoleMode.Authoring
                    Return "Authoring"
                Case AJCMSConsoleMode.Edit
                    Return "Edit"
                Case AJCMSConsoleMode.Preview
                    Return "Preview"
                Case Else
                    Return "live"
            End Select
        End Function

        Private Function AddConsoleEvent(ByVal pEvent As AJCMSconsoleEventTarget, Optional ByVal pArguments As String = "") As String
            Return String.Format("__doConsolePostBack('{0}','{1}');", CInt(pEvent), pArguments)
        End Function

        Private Function ProcessConsoleEvents(ByVal pTarget As String, ByVal pArguments As String) As Boolean
            Select Case pTarget

                Case AJCMSconsoleEventTarget._B2Edit
                    Back2Edit(mlnkB2Edit, pArguments)

                Case AJCMSconsoleEventTarget._CreatePost
                    CreatePosting(mlnkCreatePost, pArguments)

                Case AJCMSconsoleEventTarget._CopyPost
                    CopyPosting(mlnkCopyPost, pArguments)

                Case AJCMSconsoleEventTarget._MovePost
                    MovePosting(mlnkMovePost, pArguments)


                Case AJCMSconsoleEventTarget._DelPost
                    DeletePosting(mlnkDelPost, pArguments)

                Case AJCMSconsoleEventTarget._ExitWoSave
                    ExitWithoutSave(mlnkExitWoSave, pArguments)

                Case AJCMSconsoleEventTarget._S2Auth
                    SwitchToAuth(mlnkS2Auth, pArguments)

                Case AJCMSconsoleEventTarget._S2Edit
                    SwitchToEdit(mlnkS2Edit, pArguments)

                Case AJCMSconsoleEventTarget._S2Live
                    SwitchToLive(mlnkS2Live, pArguments)

                Case AJCMSconsoleEventTarget._S2Preview
                    SwitchToPreview(mlnkS2Preview, pArguments)

                Case AJCMSconsoleEventTarget._SavePostExit
                    SavePostingExit(mlnkSavePostExit, pArguments)

                Case AJCMSconsoleEventTarget._SavePostNoExit
                    SavePostingNoExit(mlnkSavePostNoExit, pArguments)

                Case AJCMSconsoleEventTarget._ShwAdmPnl
                    ShowAdminPan(mlnkShwAdmPnl, pArguments)

                Case AJCMSconsoleEventTarget._ShwPostPro
                    ShowPostingProp(mlnkShwPostPro, pArguments)

                Case AJCMSconsoleEventTarget._ShwSiteMap
                    ShowSiteMap(mlnkShwSiteMap, pArguments)


            End Select
        End Function

        Private Function DefaultControls(ByVal pMode As AJCMSConsoleMode) As List(Of Control)
            Select Case pMode
                Case AJCMSConsoleMode.Authoring
                    Return AuthModeControls()
                Case AJCMSConsoleMode.Edit
                    Return EditModeControls()
                Case AJCMSConsoleMode.Preview
                    Return PreviewModeControls()
                Case Else 'live
                    Return LiveModeControls()
            End Select
        End Function

        Private Function LiveModeControls() As List(Of Control)
            Dim mCtrls As New List(Of Control)
            mlnkS2Auth.Text = "Switch to Authoring mode"
            mlnkS2Auth.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._S2Auth)
            mCtrls.Add(mlnkS2Auth)
            Return mCtrls
        End Function

        Private Function AuthModeControls() As List(Of Control)
            Dim mCtrls As New List(Of Control)
            Dim mQrStr As String
            Dim mConf As AJCMS.Library.Config
            Dim mQsb As Config.QueryStringBuilder
            Dim mP As Posting


            mlnkS2Live.Text = "Switch to Live mode"
            mlnkS2Live.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._S2Live)
            mCtrls.Add(mlnkS2Live)

            If mBasePage.Current.Command <> Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowNoPostingPage Then
                If Me.Posting_Properties <> AJCMSConsoleItemVisibility.Hidden Then
                    mP = New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    mlnkShwPostPro.Text = "Posting properties"
                    If Me.Posting_Properties = AJCMSConsoleItemVisibility.Enabled And mP.Load(mBasePage.Current.PostingGUID) Then
                        mlnkShwPostPro.Enabled = True
                        mQrStr = ""
                        mConf = New AJCMS.Library.Config
                        mQsb = New Config.QueryStringBuilder
                        mQsb.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowPostingProp
                        mQsb.PostingGUID = mBasePage.Current.PostingGUID.Trim
                        mQsb.SiteID = mBasePage.Current.SiteID
                        mQsb.ChannelGUID = mBasePage.Current.ChannelGUID.Trim
                        mQrStr = Config.QueryStringBuilder.Create(mQsb)
                        mlnkShwPostPro.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._ShwPostPro)
                        mlnkShwPostPro.OnClientClick += "; " & clsLib.ShowDialog(Config.SiteAddress & mConf.PostingPropURL & mQrStr, "PostingProp")
                        mQsb = Nothing
                        mConf = Nothing
                    Else
                        mlnkShwPostPro.Enabled = False
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    mCtrls.Add(mlnkShwPostPro)
                End If

                If Me.Edit_Posting <> AJCMSConsoleItemVisibility.Hidden Then
                    mP = New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    mlnkS2Edit.Text = "Edit-Posting"
                    If Me.Edit_Posting = AJCMSConsoleItemVisibility.Enabled And mP.Load(mBasePage.Current.PostingGUID) Then
                        mlnkS2Edit.Enabled = True
                        mlnkS2Edit.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._S2Edit)
                    Else
                        mlnkS2Edit.Enabled = False
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    mCtrls.Add(mlnkS2Edit)
                End If

                If Me.Copy_Posting <> AJCMSConsoleItemVisibility.Hidden Then
                    mP = New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    mlnkCopyPost.Text = "Copy-Posting"
                    If Me.Copy_Posting = AJCMSConsoleItemVisibility.Enabled And mP.Load(mBasePage.Current.PostingGUID) Then
                        mlnkCopyPost.Enabled = True
                        mlnkCopyPost.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._CopyPost)
                    Else
                        mlnkCopyPost.Enabled = False
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    mCtrls.Add(mlnkCopyPost)
                End If

                If Me.Move_Posting <> AJCMSConsoleItemVisibility.Hidden Then
                    mP = New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    mlnkMovePost.Text = "Move-Posting"
                    If Me.Move_Posting = AJCMSConsoleItemVisibility.Enabled And mP.Load(mBasePage.Current.PostingGUID) Then
                        mlnkMovePost.Enabled = True
                        mlnkMovePost.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._MovePost)
                    Else
                        mlnkMovePost.Enabled = False
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    mCtrls.Add(mlnkMovePost)
                End If

                If Me.Delete_Posting <> AJCMSConsoleItemVisibility.Hidden Then
                    mP = New Posting
                    AddHandler mP.OnLastError, AddressOf _OnLastError
                    mlnkDelPost.Text = "Delete Posting"
                    If Me.Delete_Posting = AJCMSConsoleItemVisibility.Enabled And mP.Load(mBasePage.Current.PostingGUID) Then
                        mlnkDelPost.Enabled = True
                        mlnkDelPost.OnClientClick = "if (confirm('Are you sure to delete this posting ?')) { " & AddConsoleEvent(AJCMSconsoleEventTarget._DelPost) & "}"
                    Else
                        mlnkDelPost.Enabled = False
                    End If
                    RemoveHandler mP.OnLastError, AddressOf _OnLastError
                    mP = Nothing
                    mCtrls.Add(mlnkDelPost)
                End If
            End If

            If Me.Create_New_Posting <> AJCMSConsoleItemVisibility.Hidden Then
                mlnkCreatePost.Text = "Create new Posting"
                If Me.Create_New_Posting = AJCMSConsoleItemVisibility.Enabled Then
                    mQrStr = ""
                    mConf = New AJCMS.Library.Config
                    mQsb = New Config.QueryStringBuilder
                    mQsb.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.SelectTemplate
                    mQsb.ChannelGUID = mBasePage.Current.ChannelGUID.Trim
                    mQsb.SiteID = mBasePage.Current.SiteID
                    mQrStr = Config.QueryStringBuilder.Create(mQsb)
                    If mQsb.ChannelGUID <> "" And mQsb.SiteID <> "" Then
                        mlnkCreatePost.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._CreatePost)
                        mlnkCreatePost.OnClientClick += "; " & clsLib.ShowDialog(Config.SiteAddress & mConf.SelectTemplateURL & mQrStr, "CreateNewPosting")
                        'mlnkCreatePost.OnClientClick = "alert(""" & ShowDialog(mConf.WebServerURL & mConf.SelectTemplateURL & mQrStr, "CreateNewPosting") & """)"
                    Else
                        mlnkCreatePost.OnClientClick = "alert('Channel GUID /Site ID not found');"
                    End If
                Else
                    mlnkCreatePost.Enabled = False
                End If
                mCtrls.Add(mlnkCreatePost)
                mConf = Nothing
                mQsb = Nothing
            End If

            If mBasePage.Current.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.ShowNoPostingPage Then
                mlnkShwSiteMap.Text = "Back to Site"
                If Me.Show_Site_Map = AJCMSConsoleItemVisibility.Enabled Then
                    mlnkShwSiteMap.OnClientClick = "window.location ='" & Config.SiteAddress & "'" ' AddConsoleEvent(AJCMSconsoleEventTarget._ShwSiteMap)
                Else
                    mlnkShwSiteMap.Enabled = False
                End If
                mCtrls.Add(mlnkShwSiteMap)
            Else
                If Me.Show_Site_Map <> AJCMSConsoleItemVisibility.Hidden Then
                    mlnkShwSiteMap.Text = "Show site-map"
                    If Me.Show_Site_Map = AJCMSConsoleItemVisibility.Enabled Then
                        mlnkShwSiteMap.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._ShwSiteMap)
                    Else
                        mlnkShwSiteMap.Enabled = False
                    End If
                    mCtrls.Add(mlnkShwSiteMap)
                End If
            End If


            If Me.Show_Admin_Panel <> AJCMSConsoleItemVisibility.Hidden Then
                mlnkShwAdmPnl.Text = "Show Administrative panel"
                If Me.Show_Admin_Panel = AJCMSConsoleItemVisibility.Enabled Then
                    mlnkShwAdmPnl.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._ShwAdmPnl)
                    mConf = New AJCMS.Library.Config
                    mlnkShwAdmPnl.OnClientClick += "; " & clsLib.ShowDialog(mConf.AjcmsApURL & "/default.aspx", "AdminPan")
                Else
                    mlnkShwAdmPnl.Enabled = False
                End If
                mCtrls.Add(mlnkShwAdmPnl)
            End If

            mQsb = Nothing
            mConf = Nothing

            Return mCtrls
        End Function

        Private Function EditModeControls() As List(Of Control)
            Dim mCtrls As New List(Of Control)

            mlnkS2Preview.Text = "Preview"
            mlnkS2Preview.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._S2Preview)
            mCtrls.Add(mlnkS2Preview)

            mlnkSavePostNoExit.Text = "Save"
            mlnkSavePostNoExit.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._SavePostNoExit)
            mCtrls.Add(mlnkSavePostNoExit)

            mlnkSavePostExit.Text = "Save and Exit"
            mlnkSavePostExit.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._SavePostExit)
            mCtrls.Add(mlnkSavePostExit)

            mlnkExitWoSave.Text = "Exit without save"
            mlnkExitWoSave.OnClientClick = "javascript:if(confirm('Changes will be lost. Are you sure to exit without save?')) " & AddConsoleEvent(AJCMSconsoleEventTarget._ExitWoSave)
            mCtrls.Add(mlnkExitWoSave)

            Return mCtrls
        End Function

        Private Function PreviewModeControls() As List(Of Control)
            Dim mCtrls As New List(Of Control)

            mlnkB2Edit.Text = "Back to Edit-Posting"
            mlnkB2Edit.OnClientClick = AddConsoleEvent(AJCMSconsoleEventTarget._B2Edit)
            mCtrls.Add(mlnkB2Edit)


            Return mCtrls
        End Function

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            mBasePage = CType(MyBase.Page, AJCMS.BasePage)
            If Not DesignMode And Me.Visible Then
                If Not mBasePage.Current.Anonymous Then
                    Dim mValid As Boolean = False
                    Dim mKey As String = ""
                    Dim mConf As New AJCMS.Library.Config
                    Dim mPro As New Protection.Protection
                    mKey = mConf.LicenseKey
                    mConf = Nothing
                    mValid = mPro.Valid(mKey)
                    mKey = Nothing
                    mPro = Nothing
                    If mValid Then

                        'Maintain console position -- start
                        If mBasePage.IsPostBack Then
                            If InStr(MyBase.Page.Request.Form(cnslXY), ",") <> 0 Then
                                Dim xMove
                                Dim yMove
                                xMove = Replace(MyBase.Page.Request.Form(cnslXY).Split(",")(0), "px", "")
                                yMove = Replace(MyBase.Page.Request.Form(cnslXY).Split(",")(1), "px", "")
                                Me.X = MyCint(Me.X) + MyCint(xMove)
                                Me.Y = MyCint(Me.Y) + MyCint(yMove)
                                If Me.X < 0 Then
                                    Me.X = 0
                                End If
                                If Me.Y < 0 Then
                                    Me.Y = 0
                                End If
                            End If
                        End If
                        'Maintain console position -- end

                        mFS = New FlotingScript(cnslXY, Me.X, Me.Y)
                        mFS.CssClassForConsoleItems = Me.CssItems
                        mFS.CssTableClassForConsoleItems = Me.CssItemsTable
                        mFS.CssTableClassForTitle = Me.CssTitle
                        mFS.Register(MyBase.Page)

                        'Console Postback {
                        Dim mCtrl As New HtmlGenericControl
                        mCtrl.InnerHtml = ConsoleDoPostbackScript()
                        mBasePage.Header.Controls.Add(mCtrl)
                        mCtrl = Nothing
                        mBasePage.ClientScript.RegisterHiddenField(cnslEVENTTARGET, "")
                        mBasePage.ClientScript.RegisterHiddenField(cnslEVENTARGUMENT, "")
                        '}

                        'console X Y position storage field -- start
                        mBasePage.ClientScript.RegisterHiddenField(cnslXY, "")
                        'console X Y position storage field -- end

                    End If
                End If
            End If

            If Not mBasePage.IsPostBack Then
                If mBasePage.Current.Command = Config.QueryStringBuilder.RequestQueryStringCommandtype.CreatePostingByConsoleFromSelectTemplate Then
                    Mode = AJCMSConsoleMode.Edit
                Else
                    Mode = AJCMSConsoleMode.Authoring
                End If
            Else
                'Console Postback {
                ProcessConsoleEvents(MyBase.Page.Request.Form(cnslEVENTTARGET), MyBase.Page.Request.Form(cnslEVENTTARGET))
                '}
            End If
        End Sub

        Private Function ConsoleDoPostbackScript() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine("<script language=""javascript"" type=""text/javascript"">")
            mStr.AppendLine("function __doConsolePostBack(eventTarget, eventArgument) {")
            mStr.AppendLine("    if (!document.forms(0).onsubmit || (document.forms(0).onsubmit() != false)) {")
            mStr.AppendLine("        document.forms(0)." & cnslEVENTTARGET & ".value = eventTarget;")
            mStr.AppendLine("        document.forms(0)." & cnslEVENTARGUMENT & ".value = eventArgument;")
            mStr.AppendLine("        document.forms(0).submit();")
            mStr.AppendLine("    }")
            mStr.AppendLine("}")
            mStr.AppendLine("</script>")
            Return mStr.ToString
        End Function


        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Posting_Properties() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_PP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_PP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Edit_Posting() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_EP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_EP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Copy_Posting() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_CP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_CP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Move_Posting() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_MP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_MP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Delete_Posting() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_DP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_DP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Create_New_Posting() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_CNP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_CNP") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Show_Site_Map() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_SSM")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_SSM") = value
            End Set
        End Property

        < _
         Bindable(True), _
         Category("Console-Items-Behavior"), _
         DefaultValue("Enabled"), _
         Localizable(True) _
         > _
         Public Property Show_Admin_Panel() As AJCMS.AJCMSConsoleItemVisibility
            Get
                Return Civ("CIV_SAP")
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                Civ("CIV_SAP") = value
            End Set
        End Property

        Private Property Civ(ByVal Item As String) As AJCMS.AJCMSConsoleItemVisibility
            Get
                Dim mCM As String = CStr(ViewState(Item))
                If mCM Is Nothing Then
                    Dim mCMses As String
                    If Not Me.DesignMode Then
                        mCMses = CStr(mBasePage.Session(Item))
                    Else
                        mCMses = Nothing
                    End If
                    If mCMses IsNot Nothing Then
                        mCM = mCMses
                    Else
                        mCM = AJCMS.AJCMSConsoleItemVisibility.Enabled
                    End If
                End If
                Return mCM
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleItemVisibility)
                ViewState(Item) = value
                If Not Me.DesignMode Then
                    mBasePage.Session(Item) = value
                End If
            End Set
        End Property

        < _
         Bindable(True), _
         Category("AJCMS-Console"), _
         DefaultValue("Live"), _
         Description("Mode"), _
         Localizable(True) _
         > _
         Public Property Mode() As AJCMS.AJCMSConsoleMode
            Get
                'Dim mCM As String = CStr(mBasePage.Session("CNSLMD"))
                Dim mCM As String = CStr(ViewState("CNSLMD"))
                If mCM Is Nothing Then
                    Dim mCMses As String
                    If Not Me.DesignMode Then
                        mCMses = CStr(mBasePage.Session("CNSLMD"))
                    Else
                        mCMses = Nothing
                    End If
                    If mCMses IsNot Nothing Then
                        mCM = mCMses
                    Else
                        mCM = AJCMS.AJCMSConsoleMode.Live
                    End If
                End If
                Return mCM
            End Get
            Set(ByVal value As AJCMS.AJCMSConsoleMode)
                Dim mPrvMode As AJCMSConsoleMode = Mode
                ViewState("CNSLMD") = value
                If Not Me.DesignMode Then
                    mBasePage.Session("CNSLMD") = value
                    '' '' ''CreateChildControls()
                    Me.ChangeMultiControlsMode(Mode)
                    RaiseEvent onModeChange(Me, New EventArgs, mPrvMode, Mode)
                End If
            End Set
        End Property

        '< _
        ' Bindable(True), _
        ' Category("AJCMS-Console"), _
        ' DefaultValue("10"), _
        ' Description("X position"), _
        ' Localizable(True) _
        ' > _
        <Browsable(False)> _
        Public Property X() As String
            Get
                'Dim mCM As String = CStr(ViewState("X_POS"))
                Dim mCM As String = CStr(mBasePage.Session("X_POS"))
                If mCM Is Nothing Then
                    mCM = 0
                End If
                Return mCM
            End Get
            Set(ByVal value As String)
                'ViewState("X_POS") = value
                mBasePage.Session("X_POS") = value
            End Set
        End Property

        '< _
        ' Bindable(True), _
        ' Category("AJCMS-Console"), _
        ' DefaultValue("10"), _
        ' Description("Y position"), _
        ' Localizable(True) _
        ' > _
        <Browsable(False)> _
        Public Property Y() As String
            Get
                'Dim mCM As String = CStr(ViewState("Y_POS"))
                Dim mCM As String = CStr(mBasePage.Session("Y_POS"))
                If mCM Is Nothing Then
                    mCM = 0
                End If
                Return mCM
            End Get
            Set(ByVal value As String)
                'ViewState("Y_POS") = value
                mBasePage.Session("Y_POS") = value
            End Set
        End Property

        < _
        Bindable(True), _
        Category("AJCMS-Console"), _
        DefaultValue("Console"), _
        Description("Console Title"), _
        Localizable(True) _
        > _
        Public Property Title() As String
            Get
                Dim mT As String = CStr(ViewState("ConsoleTitle"))
                If mT Is Nothing Then
                    mT = "Console"
                End If
                Return mT
                'Return mFS.Title
            End Get
            Set(ByVal value As String)
                ViewState("ConsoleTitle") = value
                mFS.Title = value
            End Set
        End Property

        < _
        Bindable(True), _
        Category("CssClass"), _
        DefaultValue(""), _
        Description("CssClass for Console Title. Table class required"), _
        Localizable(True) _
        > _
        Public Property CssTitle() As String
            Get
                Return mFS.CssTableClassForTitle
            End Get
            Set(ByVal value As String)
                mFS.CssTableClassForTitle = value
            End Set
        End Property

        < _
        Bindable(True), _
        Category("CssClass"), _
        DefaultValue(""), _
        Description("CssClass for Console Items. Table class required"), _
        Localizable(True) _
        > _
        Public Property CssItems() As String
            Get
                Return mFS.CssClassForConsoleItems
            End Get
            Set(ByVal value As String)
                mFS.CssClassForConsoleItems = value
            End Set
        End Property

        < _
        Bindable(True), _
        Category("CssClass"), _
        DefaultValue(""), _
        Description("CssClass for Console Items. Table class required"), _
        Localizable(True) _
        > _
        Public Property CssItemsTable() As String
            Get
                Return mFS.CssTableClassForConsoleItems
            End Get
            Set(ByVal value As String)
                mFS.CssTableClassForConsoleItems = value
            End Set
        End Property

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Not mBasePage.Current.Anonymous Then
                'EnsureChildControls()
                CreateChildControls()
                Dim mOptionalText As String = ""
                writer.RenderBeginTag("DIV")
                If mBasePage.Current.PostingGUID <> "" Then
                    Dim mP As New Posting
                    mP.Load(mBasePage.Current.PostingGUID)
                    mOptionalText = mP.DisplayName 'mP.Parent.DisplayName & "/" & mP.DisplayName
                    mP = Nothing
                End If

                writer.Write(mFS.BodyScriptBegin(ModeToStr(Mode), mOptionalText))

                Dim mValid As Boolean = False
                Dim mKey As String = ""
                Dim mConf As New AJCMS.Library.Config
                Dim mPro As New Protection.Protection
                mKey = mConf.LicenseKey
                mConf = Nothing
                mValid = mPro.Valid(mKey)
                mKey = Nothing
                mPro = Nothing
                If mValid And Not MyBase.DesignMode Then
                    For Each mCtr As Control In DefaultControls(Mode)
                        mHtml.Space.RenderControl(writer)
                        mCtr.RenderControl(writer)
                        mHtml.Space.RenderControl(writer)
                        mHtml.LineBreak.RenderControl(writer)
                    Next
                Else
                    Dim mL As New LiteralControl("License required")
                    mL.RenderControl(writer)
                    mL = Nothing
                End If
                mHtml.HorLine.RenderControl(writer)
                MyBase.Render(writer)
                writer.Write(mFS.BodyScriptEnd)
                writer.RenderEndTag()
            End If
        End Sub

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()
            _owner = New TemplateOwner()

            Dim temp As ITemplate
            temp = GetTemplate(Mode)
            If temp Is Nothing Then
                temp = New DefaultTemplate
            End If
            _owner.Controls.Clear()

            _owner.EnableViewState = True

            temp.InstantiateIn(_owner)


            Me.Controls.Add(_owner)

        End Sub

        Protected Overrides Function OnBubbleEvent(ByVal source As Object, ByVal args As System.EventArgs) As Boolean
            RaiseEvent ChildControlEvents(source, args)
            Return MyBase.OnBubbleEvent(source, args)
        End Function

#Region " CONSOLE DESIGNER TEMPLATE RELATED "

        Private ReadOnly Property GetTemplate(ByVal Mode As AJCMS.AJCMSConsoleMode) As ITemplate
            Get
                Select Case Mode
                    Case AJCMS.AJCMSConsoleMode.Authoring
                        Return Authoring
                    Case AJCMS.AJCMSConsoleMode.Edit
                        Return Edit
                    Case AJCMS.AJCMSConsoleMode.Preview
                        Return Preview
                    Case Else
                        Return Live
                End Select
            End Get
        End Property

        < _
        Browsable(False), _
        DesignerSerializationVisibility( _
        DesignerSerializationVisibility.Hidden) _
        > _
        Private ReadOnly Property Owner() As TemplateOwner
            Get
                Return _owner
            End Get
        End Property


        < _
        Browsable(False), _
        PersistenceMode(PersistenceMode.InnerProperty), _
        DefaultValue(GetType(ITemplate), ""), _
        Description("Live"), _
        TemplateContainer(GetType(FloatingConsole)) _
        > _
        Public Overridable Property Live() As ITemplate
            Get
                Return _Live
            End Get
            Set(ByVal value As ITemplate)
                _Live = value
            End Set
        End Property

        < _
        Browsable(False), _
        PersistenceMode(PersistenceMode.InnerProperty), _
        DefaultValue(GetType(ITemplate), ""), _
        Description("Authoring"), _
        TemplateContainer(GetType(FloatingConsole)) _
        > _
        Public Overridable Property Authoring() As ITemplate
            Get
                Return _Authoring
            End Get
            Set(ByVal value As ITemplate)
                _Authoring = value
            End Set
        End Property

        < _
        Browsable(False), _
        PersistenceMode(PersistenceMode.InnerProperty), _
        DefaultValue(GetType(ITemplate), ""), _
        Description("Edit"), _
        TemplateContainer(GetType(FloatingConsole)) _
        > _
        Public Overridable Property Edit() As ITemplate
            Get
                Return _Edit
            End Get
            Set(ByVal value As ITemplate)
                _Edit = value
            End Set
        End Property

        < _
        Browsable(False), _
        PersistenceMode(PersistenceMode.InnerProperty), _
        DefaultValue(GetType(ITemplate), ""), _
        Description("Preview"), _
        TemplateContainer(GetType(FloatingConsole)) _
        > _
        Public Overridable Property Preview() As ITemplate
            Get
                Return _Preview
            End Get
            Set(ByVal value As ITemplate)
                _Preview = value
            End Set
        End Property

#End Region

        Private Class HtmlTags
            Public ReadOnly Property LineBreak() As LiteralControl
                Get
                    Return New LiteralControl("<br>")
                End Get
            End Property

            Public ReadOnly Property Space(Optional ByVal NumberOf As Integer = 1) As LiteralControl
                Get
                    Dim mStr As String = ""
                    For cnt As Integer = 1 To NumberOf
                        mStr += "&nbsp;"
                    Next
                    Return New LiteralControl(mStr)
                End Get
            End Property

            Public ReadOnly Property HorLine() As LiteralControl
                Get
                    Return New LiteralControl("<hr>")
                End Get
            End Property
        End Class

        Private Class MyLinkButton
            Inherits LinkButton

            Public Sub New()
                'MyBase.ForeColor = Color.White
                MyBase.Font.Underline = False
                Enabled = True
                'MyBase.Style.Add("cursor", "hand")
                'MyBase.Font.Name = "Verdana"
                'MyBase.Font.Size = 7
                'MyBase.Font.Bold = True
            End Sub

            Public Overrides Property Enabled() As Boolean
                Get
                    Return MyBase.Enabled
                End Get
                Set(ByVal value As Boolean)
                    MyBase.Enabled = value
                    'If MyBase.Enabled Then
                    '    MyBase.Style.Add("cursor", "hand")
                    'Else
                    '    MyBase.Style.Remove("cursor")
                    'End If
                End Set
            End Property
        End Class

    End Class

#Region "    Floating Console dependencies "

    <ToolboxItem(False)> _
    Public Class TemplateOwner
        Inherits WebControl
    End Class

    NotInheritable Class DefaultTemplate
        Implements ITemplate

        Sub InstantiateIn(ByVal owner As Control) _
            Implements ITemplate.InstantiateIn
            'Dim mLineBreak As New LiteralControl("&lt;&lt;your controls goes here&gt;&gt;</br>")
            'owner.Controls.Add(mLineBreak)
        End Sub

    End Class

    Public Class FloatingConsoleDesigner
        Inherits ControlDesigner

        Public Overrides Sub Initialize(ByVal Component As IComponent)
            MyBase.Initialize(Component)
            SetViewFlags(ViewFlags.TemplateEditing, True)
        End Sub

        Public Overloads Overrides Function GetDesignTimeHtml() As String
            Return "<span>Floating Console</span>"
        End Function

        Public Overrides ReadOnly Property TemplateGroups() As TemplateGroupCollection
            Get
                Dim collection As New TemplateGroupCollection
                Dim group As TemplateGroup
                Dim template As TemplateDefinition
                Dim control As FloatingConsole

                control = CType(Component, FloatingConsole)
                group = New TemplateGroup("Live")
                template = New TemplateDefinition(Me, "Live", control, "Live", True)
                group.AddTemplateDefinition(template)
                collection.Add(group)

                group = New TemplateGroup("Authoring")
                template = New TemplateDefinition(Me, "Authoring", control, "Authoring", True)
                group.AddTemplateDefinition(template)
                collection.Add(group)

                group = New TemplateGroup("Edit")
                template = New TemplateDefinition(Me, "Edit", control, "Edit", True)
                group.AddTemplateDefinition(template)
                collection.Add(group)

                group = New TemplateGroup("Preview")
                template = New TemplateDefinition(Me, "Preview", control, "Preview", True)
                group.AddTemplateDefinition(template)
                collection.Add(group)

                Return collection
            End Get
        End Property
    End Class

#End Region

#End Region

#Region " Floating script "
    Public Class FlotingScript
        Private _ConsoleX As String = "10"
        Private _ConsoleY As String = "10"
        Private _CnslXY As String = ""
        Private mClsConsoleItems As String = "clsConsoleItems"
        Private mClsConsoleItemsTable As String = "clsConsoleItemsTable"
        Private mClsTitle As String = "clsTitle"
        Private mTitle As String = "Console"

        Public Sub New(ByVal pCnslXY As String, Optional ByVal x_pos As String = "10", Optional ByVal y_pos As String = "10")
            _ConsoleX = x_pos
            _ConsoleY = y_pos
            _CnslXY = pCnslXY
        End Sub

        Public Property CssTableClassForTitle() As String
            Get
                Return mClsTitle
            End Get
            Set(ByVal value As String)
                mClsTitle = value
            End Set
        End Property

        Public Property CssTableClassForConsoleItems() As String
            Get
                Return mClsConsoleItemsTable
            End Get
            Set(ByVal value As String)
                mClsConsoleItemsTable = value
            End Set
        End Property

        Public Property CssClassForConsoleItems() As String
            Get
                Return mClsConsoleItems
            End Get
            Set(ByVal value As String)
                mClsConsoleItems = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return mTitle
            End Get
            Set(ByVal value As String)
                mTitle = value
            End Set
        End Property

        Public Function Register(ByVal pPage As System.Web.UI.Page) As Boolean
            Dim ScriptName As String = "ClsFlotingScript"
            Dim mCtrl As New HtmlGenericControl
            mCtrl.InnerHtml = "<script language='javascript'>" & vbCrLf & HeadScript(_ConsoleX, _ConsoleY, _CnslXY) & vbCrLf & "</script>"
            pPage.Header.Controls.Add(mCtrl)
            mCtrl = Nothing
        End Function

        Private ReadOnly Property HeadScript(ByVal X As String, ByVal Y As String, ByVal pCnslXY As String) As String
            Get
                Dim mScr As New StringBuilder
                mScr.AppendLine("")
                mScr.AppendLine("<!--")
                mScr.AppendLine("function initDrag() ")
                mScr.AppendLine("{")
                mScr.AppendLine("	if (document.layers) ")
                mScr.AppendLine("	   document.captureEvents(Event.MOUSEMOVE|Event.MOUSEDOWN|Event.MOUSEUP);")
                mScr.AppendLine("	document.onmousemove=dragf;")
                mScr.AppendLine("	document.onmousedown=dragf;")
                mScr.AppendLine("	document.onmouseup=dragf;")
                mScr.AppendLine("	dragDiv=null;")
                mScr.AppendLine("	dragInit=1;")
                mScr.AppendLine("	if (document.getElementsByTagName) ")
                mScr.AppendLine("	   zMax=document.getElementsByTagName(""DIV"").length;")
                mScr.AppendLine("	else if (document.all) zMax=document.body.all.tags(""DIV"").length;")
                mScr.AppendLine("	else if (document.layers) zMax=document.layers.length;")
                mScr.AppendLine("}")

                mScr.AppendLine("function dragf(arg) ")
                mScr.AppendLine("{")
                mScr.AppendLine("	ev=arg?arg:event;")
                mScr.AppendLine("	if (dragDiv && ev.type==""mousedown"") ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		dragOn=1;")
                mScr.AppendLine("		dragX=(ev.pageX?ev.pageX:ev.clientX)-parseInt(dragDiv.style.left);")
                mScr.AppendLine("		dragY=(ev.pageY?ev.pageY:ev.clientY)-parseInt(dragDiv.style.top);")
                mScr.AppendLine("		dragDiv.style.zIndex=zMax++; // remove this line to preserve z-indexes")
                mScr.AppendLine("		return false;")
                mScr.AppendLine("	}")
                mScr.AppendLine("	if (ev.type==""mouseup"") ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		dragOn=0;")
                mScr.AppendLine("	}")
                mScr.AppendLine("	if (dragDiv && ev.type==""mousemove"" && dragOn)")
                mScr.AppendLine("	{")
                mScr.AppendLine("		dragDiv.style.left=(ev.pageX?ev.pageX:ev.clientX)-dragX;")
                mScr.AppendLine("		dragDiv.style.top=(ev.pageY?ev.pageY:ev.clientY)-dragY;")
                mScr.AppendLine("		document.getElementById('" & pCnslXY & "').value = dragDiv.style.left+ ',' + dragDiv.style.top;")
                'mScr.AppendLine("		document.title = document.getElementById('" & pCnslXY & "').value;")
                mScr.AppendLine("		return false;")
                mScr.AppendLine("	}")
                mScr.AppendLine("	if (ev.type==""mouseout"") ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		if (!dragOn) dragDiv=null;")
                mScr.AppendLine("	}")
                mScr.AppendLine("}")

                mScr.AppendLine("function drag(div) ")
                mScr.AppendLine("{")
                mScr.AppendLine("	if (!dragInit) initDrag();")
                mScr.AppendLine("	if (!dragOn) ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		dragDiv=document.getElementById?document.getElementById(div): ")
                mScr.AppendLine("		document.all?document.all[div]:document.layers?document.layers[div]:null;")
                mScr.AppendLine("		if (document.layers) dragDiv.style=dragDiv;")
                mScr.AppendLine("		dragDiv.onmouseout=dragf;")
                mScr.AppendLine("	}")
                mScr.AppendLine("}")

                mScr.AppendLine("function OpenCloseDiv(divName)")
                mScr.AppendLine("{")
                mScr.AppendLine("	if (divName.style.display == ""none"") ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		divName.style.display=""block"";")
                mScr.AppendLine("	}")
                mScr.AppendLine("	else ")
                mScr.AppendLine("	{")
                mScr.AppendLine("		divName.style.display=""none"";")
                mScr.AppendLine("	}")
                mScr.AppendLine("}")

                mScr.AppendLine("function AJCMS_FloatTopDiv()")
                mScr.AppendLine("{")
                mScr.AppendLine(String.Format("	var startX = {0},", X))
                mScr.AppendLine(String.Format("	startY = {0};", Y))
                mScr.AppendLine("if (startX > (screen.width-50)) {")
                mScr.AppendLine("	startX = screen.width - 50;")
                mScr.AppendLine("}")
                mScr.AppendLine("if (startY > (screen.height-50)) {")
                mScr.AppendLine("	startY = screen.height - 50;")
                mScr.AppendLine("}")

                mScr.AppendLine("	var ns = (navigator.appName.indexOf(""Netscape"") != -1);")
                mScr.AppendLine("	var d = document;")

                mScr.AppendLine("	function ml(id)")
                mScr.AppendLine("	{")
                mScr.AppendLine("		var el =d.getElementById?d.getElementById(id):d.all?d.all[id]:d.layers[id];")
                mScr.AppendLine("		if(d.layers)el.style=el;")
                mScr.AppendLine("		el.sP=function(x,y){this.style.left=x;this.style.top=y;};")
                mScr.AppendLine("		el.x = startX;")
                mScr.AppendLine("		if (verticalpos==""fromtop"")")
                mScr.AppendLine("		el.y = startY;")
                mScr.AppendLine("		else{")
                mScr.AppendLine("		el.y = ns ? pageYOffset + innerHeight : document.documentElement.scrollTop + document.body.clientHeight;")
                mScr.AppendLine("		el.y -= startY;")
                mScr.AppendLine("		}")
                mScr.AppendLine("		return el;")
                mScr.AppendLine("	}")
                mScr.AppendLine("	window.stayTopLeft=function()")
                mScr.AppendLine("	{")
                mScr.AppendLine("		if (verticalpos==""fromtop"")")
                mScr.AppendLine("		{")
                mScr.AppendLine("			var pY = ns ? pageYOffset : document.documentElement.scrollTop;")
                mScr.AppendLine("			ftlObj.y += (pY + startY - ftlObj.y)/8;")
                mScr.AppendLine("		}")
                mScr.AppendLine("		else")
                mScr.AppendLine("		{")
                mScr.AppendLine("			var pY = ns ? pageYOffset + innerHeight : document.documentElement.scrollTop + document.body.clientHeight;")
                mScr.AppendLine("			ftlObj.y += (pY - startY - ftlObj.y)/8;")
                mScr.AppendLine("		}")
                mScr.AppendLine("		ftlObj.sP(ftlObj.x, ftlObj.y);")
                mScr.AppendLine("		setTimeout(""stayTopLeft()"", 20);")
                mScr.AppendLine("	}")
                mScr.AppendLine("	ftlObj = ml(""divStayTopLeft"");")
                mScr.AppendLine("	stayTopLeft();")
                mScr.AppendLine("}")
                mScr.AppendLine("-->")

                mScr.AppendLine("")
                Return mScr.ToString
            End Get
            'document.body.scrollTop 
            'will not work with the following
            '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
            'use the following instead
            'document.documentElement.scrollTop
        End Property

        Public ReadOnly Property BodyScriptBegin(ByVal pMode As String, Optional ByVal OptionalText As String = "") As String
            Get
                Dim mScr As New StringBuilder
                mScr.AppendLine("<style>")
                mScr.AppendLine(".clsTitle{background-color:Gray;color:white;border-collapse:collapse;border-width:1px;font-weight:bold;font-family:Verdana;height:25px;border-color:Gray;}")
                mScr.AppendLine(".clsConsoleItemsTable{background-color:silver;color:white;border-collapse:collapse;border-width:1px;font-family:Verdana;font-size:10pt;border-color:Gray;}")
                mScr.AppendLine("      .clsConsoleItems{background-color:silver;color:white;cursor:hand;}")
                mScr.AppendLine(":hover.clsConsoleItems{background-color:black; color:yellow;  cursor:hand;}")
                mScr.AppendLine("</style>")
                mScr.AppendLine("<DIV id=""divStayTopLeft"" style=""Z-INDEX: 100; POSITION: absolute"">")
                mScr.AppendLine("   <DIV id=""editConsole"" ondblclick=""OpenCloseDiv(editConsoleOptions)"" style=""PADDING-RIGHT: 0px; PADDING-LEFT: 0px; Z-INDEX: 100; LEFT: 0px; PADDING-BOTTOM: 0px; WIDTH: 300px; DIRECTION: ltr; PADDING-TOP: 0px; POSITION: absolute; TOP: 5px; TEXT-ALIGN: left"">")
                mScr.AppendLine("       <TABLE class=""" & CssTableClassForTitle & """ onmouseover=""drag('editConsole')"" onfocus=""this.blur()"" cellSpacing=""0"" cellPadding=""1"" border=""0"">")
                mScr.AppendLine("           <TR>")
                mScr.AppendLine("               <TD title=""double click to toggle menu"" style=""cursor:move"" unselectable=""on"">&nbsp;" & mTitle & "&nbsp;</TD>")
                mScr.AppendLine("           </TR>")
                mScr.AppendLine("           <TR>")
                mScr.AppendLine("               <TD style=""cursor:move"" unselectable=""on"">&nbsp;<span style='font-size:xx-small'>" & pMode & " mode " & IIf(OptionalText <> "", " of " & OptionalText, "") & "</span>&nbsp;</TD>")
                mScr.AppendLine("           </TR>")
                mScr.AppendLine("           <TR>")
                mScr.AppendLine("               <TD>")
                mScr.AppendLine("                   <DIV id=""editConsoleOptions"">")
                mScr.AppendLine("                       <TABLE width='100%' class=""" & CssTableClassForConsoleItems & """ cellSpacing=""0"" cellPadding=""1"" border=""1"">")
                mScr.AppendLine("                           <TR>")
                mScr.AppendLine("                               <TD>")
                Return mScr.ToString
            End Get
        End Property

        Public ReadOnly Property BodyScriptEnd() As String
            Get
                Dim mScr As New StringBuilder
                mScr.AppendLine("                               </TD>")
                mScr.AppendLine("                           </TR>")
                mScr.AppendLine("                       </TABLE>")
                mScr.AppendLine("                   </DIV>")
                mScr.AppendLine("               </TD>")
                mScr.AppendLine("           </TR>")
                mScr.AppendLine("       </TABLE>")
                mScr.AppendLine("   </DIV>")
                mScr.AppendLine("</DIV>")
                mScr.AppendLine("<SCRIPT language=""JavaScript"">")
                mScr.AppendLine("<!--")
                mScr.AppendLine("	var dragOn=0")
                mScr.AppendLine("	var dragDiv=null;")
                mScr.AppendLine("	var dragX=0,dragY=0;")
                mScr.AppendLine("	var zMax=0;")
                mScr.AppendLine("	var dragInit=0;")
                mScr.AppendLine("	var verticalpos = ""fromtop""")
                mScr.AppendLine("	if (!document.layers)")
                mScr.AppendLine("		document.write('</div>')")
                mScr.AppendLine("	AJCMS_FloatTopDiv();")
                mScr.AppendLine("-->")
                mScr.AppendLine("</SCRIPT>")
                mScr.AppendLine("")
                Return mScr.ToString
            End Get
        End Property

    End Class
#End Region


End Namespace
