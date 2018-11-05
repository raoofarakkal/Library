Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports System.Security.Permissions
Imports System.Collections
Imports System.Drawing.Design

Namespace _Library._Web

    Namespace MultiControl



        'vbc /res:MultiControl.bmp,AJCMS.UDWC.Controls.MultiControl.bmp /t:library /out:AJCMS.UDWC.Controls.MultiControl.dll /r:System.dll /r:System.Web.dll *.vb

        <DefaultEvent("TextChanged") _
        , ToolboxData("<{0}:MultiControl " _
        & " runat=server " _
        & " height=19px " _
        & " width=125px " _
        & " EditModeControl=TextBox " _
        & " Mode=View " _
        & " ViewModeControl=Generic " _
        & " ></{0}:MultiControl>") _
        > _
        Public NotInheritable Class MultiControl
            Inherits WebControl
            Implements IPostBackDataHandler
            Private mGeneric As New _MyGeneric
            Private mTextArea As New _MyTextArea
            Private mTextBox As New _MyTextBox
            Private mLabel As New _MyLabel
            Private mImage As New _MyImage
            Private mHyperLink As New _MyHyperLink
            Private mDropDownList As New _MyDropDownList
            Private mOnContentChange As String
            Public Event TextChanged(ByVal sender As Object, ByVal e As System.EventArgs, ByVal OldVal As String, ByVal NewVal As String)

#Region " Hidding Base Properties "

            <Browsable(False)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    'MyBase.AccessKey = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    'MyBase.TabIndex = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    'MyBase.BackColor = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property BorderColor() As System.Drawing.Color
                Get
                    Return MyBase.BorderColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    'MyBase.BorderColor = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
                    'MyBase.BorderStyle = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.BorderWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    'MyBase.BorderWidth = value
                End Set
            End Property

            '<Browsable(False)> _
            'Public Overrides Property CssClass() As String
            '    Get
            '        Return MyBase.CssClass
            '    End Get
            '    Set(ByVal value As String)
            '        'MyBase.CssClass = value
            '    End Set
            'End Property

            <Browsable(False)> _
            Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
                Get
                    Return Nothing ' MyBase.Font
                End Get
            End Property

            <Browsable(False)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    'MyBase.ForeColor = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    'MyBase.EnableTheming = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    'MyBase.SkinID = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overrides Property ToolTip() As String
                Get
                    Return MyBase.ToolTip
                End Get
                Set(ByVal value As String)
                    'MyBase.ToolTip = value
                End Set
            End Property

            <Browsable(True)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <Browsable(True)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property

#End Region

#Region " ENUM "

            Public Enum AJCMS_EnableDisable
                Disabled = 0
                Enabled = 1
            End Enum

            Public Enum AJCMS_ControlMode
                Edit = 23051
                View = 23052
            End Enum

            Public Enum AJCMS_ViewModeControl
                Generic = 230519710
                Label = 230519711
                Image = 230519712
                HyperLink = 230519713
                TextBox = 230519714
                TextArea = 230519715
                DropDownList = 230519716
            End Enum

            Public Enum AJCMS_EditModeControl
                TextBox = 230519714
                TextArea = 230519715
                DropDownList = 230519716
            End Enum

            Public Enum AJCMS_ControlType
                Generic = 230519710
                Label = 230519711
                Image = 230519712
                HyperLink = 230519713
                TextBox = 230519714
                TextArea = 230519715
                DropDownList = 230519716
            End Enum

#End Region

            Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
                MyBase.Page.RegisterRequiresPostBack(Me)
                MyBase.OnLoad(e)
            End Sub

            Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
                If ControlCanPost(PreviousControl) Then
                    If ViewState("PreviousText") <> postCollection(postDataKey) Then
                        RaiseEvent TextChanged(Me, New EventArgs, ViewState("PreviousText"), postCollection(postDataKey))
                    End If
                    CurrentControl.text = postCollection(postDataKey)
                    _Filter()
                    'POSTBACK Handler
                    Dim mTar As String = String.Empty
                    Dim mArg As String = String.Empty
                    Try
                        mTar = MyBase.Page.Request.Form("___EVENTTARGET")
                        mArg = MyBase.Page.Request.Form("___EVENTARGUMENT")
                    Catch ex As Exception
                    End Try
                    'If Not (String.IsNullOrEmpty(mTar)) And Not (String.IsNullOrEmpty(mArg)) Then
                    '    If Replace(mTar.ToLower, "$", "_") = Me.ClientID.ToLower Then
                    '        Select Case mArg.ToLower
                    '            Case "RemoveHtmlTags()".ToLower
                    '                'CurrentControl.text = _Library._Web._Common.RemoveHtmlTags(CurrentControl.text)
                    '        End Select

                    '    End If
                    'End If

                    ViewState("PreviousText") = CurrentControl.text
                End If
            End Function

            Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
            End Sub

            <Browsable(False)> _
            ReadOnly Property ControlType() As AJCMS_ControlType
                Get
                    Dim mRet As AJCMS_ControlType
                    Select Case Mode
                        Case AJCMS_ControlMode.Edit
                            Select Case EditModeControl
                                Case AJCMS_EditModeControl.DropDownList
                                    mRet = AJCMS_ControlType.DropDownList
                                Case AJCMS_EditModeControl.TextArea
                                    mRet = AJCMS_ControlType.TextArea
                                Case AJCMS_EditModeControl.TextBox
                                    mRet = AJCMS_ControlType.TextBox
                            End Select
                        Case Else
                            Select Case ViewModeControl
                                Case AJCMS_ViewModeControl.Generic
                                    mRet = AJCMS_ControlType.Generic
                                Case AJCMS_ViewModeControl.Label
                                    mRet = AJCMS_ControlType.Label
                                Case AJCMS_ViewModeControl.TextBox
                                    mRet = AJCMS_ControlType.TextBox
                                Case AJCMS_ViewModeControl.TextArea
                                    mRet = AJCMS_ControlType.TextArea
                                Case AJCMS_ViewModeControl.DropDownList
                                    mRet = AJCMS_ControlType.DropDownList
                                Case AJCMS_ViewModeControl.Image
                                    mRet = AJCMS_ControlType.Image
                                Case AJCMS_ViewModeControl.HyperLink
                                    mRet = AJCMS_ControlType.HyperLink
                            End Select
                    End Select
                    Return mRet
                End Get
            End Property

            Private Property PreviousControlType() As AJCMS_ControlType
                Get
                    Dim mText As String = CStr(ViewState("PreviousControlType"))
                    If mText Is Nothing Then
                        Return 0
                    Else
                        Return mText
                    End If
                End Get
                Set(ByVal value As AJCMS_ControlType)
                    ViewState("PreviousControlType") = value
                End Set
            End Property

            <Bindable(True), Category("Control-Mode"), Localizable(True)> _
            Property Mode() As AJCMS_ControlMode
                Get
                    Dim mText As String = CStr(ViewState("Mode"))
                    If mText Is Nothing Then
                        Return 0
                    Else
                        Return IIf(mText.ToLower = "edit", AJCMS_ControlMode.Edit, AJCMS_ControlMode.View)
                    End If
                End Get
                Set(ByVal value As AJCMS_ControlMode)
                    If value <> Mode Then
                        PreviousControlType = ControlType
                    End If
                    ViewState("Mode") = value.ToString
                    _Filter()
                End Set
            End Property

            <Bindable(True), Category("Layout"), Localizable(True)> _
            Property MaxChars() As Integer
                Get
                    Dim mText As String = CStr(ViewState("MaxChars"))
                    If mText Is Nothing Then
                        Return 50
                    Else
                        Return CInt(mText)
                    End If
                End Get
                Set(ByVal value As Integer)
                    ViewState("MaxChars") = value.ToString
                End Set
            End Property

            <Bindable(True), Category("Layout"), Localizable(True)> _
            Property StatusBar() As AJCMS_EnableDisable
                Get
                    Dim mText As String = CStr(ViewState("StatusBar"))
                    If mText Is Nothing Then
                        If EditModeControl <> AJCMS_EditModeControl.DropDownList Then
                            Return AJCMS_EnableDisable.Enabled
                        Else
                            Return AJCMS_EnableDisable.Disabled
                        End If
                    Else
                        If EditModeControl <> AJCMS_EditModeControl.DropDownList Then
                            Return IIf(mText = 1, AJCMS_EnableDisable.Enabled, AJCMS_EnableDisable.Disabled)
                        Else
                            Return AJCMS_EnableDisable.Disabled
                        End If
                    End If
                End Get
                Set(ByVal value As AJCMS_EnableDisable)
                    If EditModeControl <> AJCMS_EditModeControl.DropDownList Then
                        ViewState("StatusBar") = IIf(value = AJCMS_EnableDisable.Enabled, 1, 0)
                    End If
                End Set
            End Property

            <Bindable(True), Category("Control-Mode"), Localizable(True)> _
            Property EditModeControl() As AJCMS_EditModeControl
                Get
                    Dim mText As String = CStr(ViewState("EditModeControl"))
                    If mText Is Nothing Then
                        Return 0
                    Else
                        Return mText
                    End If
                End Get
                Set(ByVal value As AJCMS_EditModeControl)
                    ViewState("EditModeControl") = value
                End Set
            End Property


            <Bindable(True), Category("Control-Mode"), Localizable(True)> _
            Property ViewModeControl() As AJCMS_ViewModeControl
                Get
                    Dim mText As String = CStr(ViewState("ViewModeControl"))
                    If mText Is Nothing Then
                        Return 0
                    Else
                        Return mText
                    End If
                End Get

                Set(ByVal value As AJCMS_ViewModeControl)
                    ViewState("ViewModeControl") = value
                End Set

            End Property

            <Browsable(False)> _
            Public ReadOnly Property CurrentControl() As Object
                Get
                    Select Case ControlType
                        Case AJCMS_ControlType.DropDownList
                            Return mDropDownList
                        Case AJCMS_ControlType.HyperLink
                            Return mHyperLink
                        Case AJCMS_ControlType.Image
                            Return mImage
                        Case AJCMS_ControlType.Label
                            Return mLabel
                        Case AJCMS_ControlType.TextArea
                            Return mTextArea
                        Case AJCMS_ControlType.TextBox
                            Return mTextBox
                        Case AJCMS_ControlType.Generic
                            Return mGeneric
                        Case Else
                            Return New Object
                    End Select
                End Get
            End Property

            <Browsable(False)> _
            Public ReadOnly Property PreviousControl() As Object
                Get
                    Select Case PreviousControlType
                        Case AJCMS_ControlType.DropDownList
                            Return mDropDownList
                        Case AJCMS_ControlType.HyperLink
                            Return mHyperLink
                        Case AJCMS_ControlType.Image
                            Return mImage
                        Case AJCMS_ControlType.Label
                            Return mLabel
                        Case AJCMS_ControlType.TextArea
                            Return mTextArea
                        Case AJCMS_ControlType.TextBox
                            Return mTextBox
                        Case AJCMS_ControlType.Generic
                            Return mGeneric
                        Case Else
                            Return New Object
                    End Select
                End Get
            End Property

            <Browsable(False)> _
            Property Text() As String
                Get
                    _Filter()
                    Return CurrentControl.text
                End Get
                Set(ByVal Value As String)
                    CurrentControl.text = Value
                End Set
            End Property

            Private Function _Filter() As Boolean
                If Not String.IsNullOrEmpty(FilterBy) Then
                    Dim mRet As String = CurrentControl.text
                    If String.IsNullOrEmpty(FilterSeparator) Then
                        mRet = Replace(mRet, FilterBy, ReplaceBy)
                    Else
                        If FilterBy.Split(FilterSeparator).Length = ReplaceBy.Split(FilterSeparator).Length Then
                            Dim mIndex As Integer = 0
                            For Each mS As String In FilterBy.Split(FilterSeparator)
                                mRet = Replace(mRet, mS, ReplaceBy.Split(FilterSeparator)(mIndex))
                                mIndex += 1
                            Next
                        Else
                            Throw New Exception("Multi-control Error: " & Me.ID & ". FilterBy and ReplaceBy must have same length")
                        End If
                    End If
                    CurrentControl.text = mRet
                End If
            End Function

            <Bindable(True), Category("Behavior"), Localizable(True)> _
            Property FilterBy() As String
                Get
                    Dim mF As String = CStr(ViewState("FiltB"))
                    If mF Is Nothing Then
                        Return ""
                    Else
                        Return mF
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FiltB") = value.ToString
                End Set
            End Property

            <Bindable(True), Category("Behavior"), Localizable(True)> _
            Property ReplaceBy() As String
                Get
                    Dim mF As String = CStr(ViewState("ReplB"))
                    If mF Is Nothing Then
                        Return ""
                    Else
                        Return mF
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("ReplB") = value.ToString
                End Set
            End Property

            <Bindable(True), Category("Behavior"), Localizable(True)> _
            Property FilterSeparator() As String
                Get
                    Dim mFs As String = CStr(ViewState("FiltSep"))
                    If mFs Is Nothing Then
                        Return ""
                    Else
                        Return mFs
                    End If
                End Get
                Set(ByVal value As String)
                    ViewState("FiltSep") = value.ToString
                End Set
            End Property

            <Browsable(True), Category("Html Area Events"), DescriptionAttribute("Java Script Ver. 1.2 or above scripting required"), DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property OnContentChange() As String
                Get
                    Return mOnContentChange
                End Get
                Set(ByVal value As String)
                    mOnContentChange = value
                End Set
            End Property

 
            Protected Overrides Function SaveViewState() As Object
                EnsureMyChilds()
                Dim NewState As Object()
                ReDim NewState(8)
                NewState(0) = MyBase.SaveViewState()
                NewState(1) = mTextArea.GetProperties
                NewState(2) = mLabel.GetProperties
                NewState(3) = mTextBox.GetProperties
                NewState(4) = mImage.GetProperties
                NewState(5) = mHyperLink.GetProperties
                NewState(6) = mDropDownList.GetProperties
                NewState(7) = mGeneric.GetProperties
                NewState(8) = ControlType
                Return NewState
            End Function

            Protected Overrides Sub LoadViewState(ByVal savedState As Object)
                MyBase.LoadViewState(savedState(0))
                mTextArea.SetProperties(savedState(1))
                mLabel.SetProperties(savedState(2))
                mTextBox.SetProperties(savedState(3))
                mImage.SetProperties(savedState(4))
                mHyperLink.SetProperties(savedState(5))
                mDropDownList.SetProperties(savedState(6))
                mGeneric.SetProperties(savedState(7))
                PreviousControlType = savedState(8)
            End Sub

            Private Function EnsureMyChilds() As Boolean
                Select Case ControlType
                    Case AJCMS_ControlType.TextArea
                        If InitTextArea() Then
                            If Not MyBase.DesignMode Then
                                mTextArea.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText(True, "<BR>," & vbCrLf)
                        End If
                    Case AJCMS_ControlType.Label
                        If InitLabel() Then
                            If Not MyBase.DesignMode Then
                                mLabel.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText(True, vbCrLf & ",<BR>")
                            'If PreviousControlType <> AJCMS_ControlType.HtmlArea Then
                            '    SetViewEditModeText(True, vbCrLf & ",<BR>")
                            'Else
                            '    SetViewEditModeText()
                            'End If
                        End If
                    Case AJCMS_ControlType.Generic
                        InitGeneric()
                        SetViewEditModeText(True, vbCrLf & ",<BR>")
                        'If PreviousControlType <> AJCMS_ControlType.HtmlArea Then
                        '    SetViewEditModeText(True, vbCrLf & ",<BR>")
                        'Else
                        '    SetViewEditModeText()
                        'End If
                    Case AJCMS_ControlType.TextBox
                        If InitTextBox() Then
                            If Not MyBase.DesignMode Then
                                mTextBox.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText()
                        End If
                        'Case AJCMS_ControlType.HtmlArea
                        '    If InitHtmlArea() Then
                        '        If Not MyBase.DesignMode Then
                        '            mHtmlArea.ID = MyBase.UniqueID
                        '        End If
                        '        SetViewEditModeText()
                        '    End If
                    Case AJCMS_ControlType.Image
                        If InitImage() Then
                            If Not MyBase.DesignMode Then
                                mImage.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText()
                        End If
                    Case AJCMS_ControlType.HyperLink
                        If InitHyperLink() Then
                            If Not MyBase.DesignMode Then
                                mHyperLink.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText()
                        End If
                    Case AJCMS_ControlType.DropDownList
                        If InitDropDownList() Then
                            If Not MyBase.DesignMode Then
                                mDropDownList.ID = MyBase.UniqueID
                            End If
                            SetViewEditModeText()
                        End If
                End Select
            End Function

            Private Function SetViewEditModeText(Optional ByVal DoReplace As Boolean = False, Optional ByVal ReplaceThisWithThisCommaSeperated As String = "") As Boolean
                If Not MyBase.DesignMode Then
                    If PreviousControl.GetType.Name.ToLower <> "object" And CurrentControl.GetType.Name.ToLower <> "object" Then
                        If PreviousControl.GetType.Name.ToLower <> CurrentControl.GetType.Name.ToLower Then
                            If PreviousControlType = AJCMS_ControlType.HyperLink Then
                                CurrentControl.text = ReplaceIfEmpty(PreviousControl.text, "Enter Text here") & "," & ReplaceIfEmpty(PreviousControl.NavigateURL, "Enter URL here")
                                If DoReplace Then
                                    CurrentControl.text = Replace(CurrentControl.text, ReplaceThisWithThisCommaSeperated.Split(",")(0), ReplaceThisWithThisCommaSeperated.Split(",")(1))
                                End If
                            ElseIf ControlType = AJCMS_ControlType.HyperLink Then
                                If PreviousControl.text.split(",").length > 1 Then
                                    CurrentControl.NavigateURL = ReplaceIfEmpty(PreviousControl.text.split(",")(1), "Enter URL here")
                                    CurrentControl.text = ReplaceIfEmpty(PreviousControl.text.split(",")(0), "Enter Text here")
                                    If DoReplace Then
                                        CurrentControl.text = Replace(CurrentControl.text, ReplaceThisWithThisCommaSeperated.Split(",")(0), ReplaceThisWithThisCommaSeperated.Split(",")(1))
                                    End If
                                Else
                                    CurrentControl.NavigateURL = ""
                                    CurrentControl.text = PreviousControl.text
                                    If DoReplace Then
                                        CurrentControl.text = Replace(CurrentControl.text, ReplaceThisWithThisCommaSeperated.Split(",")(0), ReplaceThisWithThisCommaSeperated.Split(",")(1))
                                    End If
                                End If
                            Else
                                CurrentControl.text = PreviousControl.text
                                If DoReplace Then
                                    CurrentControl.text = Replace(CurrentControl.text, ReplaceThisWithThisCommaSeperated.Split(",")(0), ReplaceThisWithThisCommaSeperated.Split(",")(1))
                                End If
                            End If
                        End If
                    End If
                End If
            End Function

            Private Function ReplaceIfEmpty(ByVal pString As String, ByVal NewString As String) As String
                Return IIf(pString.Trim.Length > 0, pString, NewString)
            End Function

            Private Function WordCount(ByVal pSourceCtrlId As String, ByVal pTargetCtrlId As String, ByVal pMaxChar As Integer) As HtmlControls.HtmlGenericControl
                Dim mRet As New HtmlControls.HtmlGenericControl("script")
                mRet.Attributes.Add("language", "javascript")
                mRet.Attributes.Add("type", "text/javascript")
                mRet.InnerHtml = ""
                mRet.InnerHtml += "$('#" & pSourceCtrlId & "').bind('textchange', function (event, previousText) {"
                mRet.InnerHtml += "    try {"
                mRet.InnerHtml += "        Wcc($(this).val(), '#" & pTargetCtrlId & "', " & pMaxChar & ");"
                mRet.InnerHtml += "    } catch (e) { }"
                mRet.InnerHtml += "});"
                Return mRet
            End Function

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                If MyBase.DesignMode Then
                    EnsureMyChilds()
                End If
                Select Case ControlType
                    Case AJCMS_ControlType.TextArea         'TEXTAREA
                        mTextArea.CssClass = Me.CssClass
                        If Not (MyBase.DesignMode) And Me.StatusBar = AJCMS_EnableDisable.Enabled Then
                            With mTextArea
                                .Attributes.Add("onselect", "Wcc($(this).getSelection().text, '#" & Replace(Me.UniqueID, "$", "_") & "_ST');") ' onSelectWcc(this,'" & Replace(Me.UniqueID, "$", "_") & "_ST'," & MaxChars & ")")
                                .Attributes.Add("onclick", "Wcc($(this).val(),'#" & Replace(Me.UniqueID, "$", "_") & "_ST'," & MaxChars & ")")
                            End With
                        Else
                            With mTextArea
                                .Attributes.Remove("onselect")
                                .Attributes.Remove("onclick")
                            End With
                        End If
                        mTextArea.RenderControl(writer)
                        ShowStatus(writer)
                        If StatusBar = AJCMS_EnableDisable.Enabled Then
                            WordCount(Replace(Me.UniqueID, "$", "_"), Replace(Me.UniqueID, "$", "_") & "_ST", MaxChars).RenderControl(writer)
                        End If
                    Case AJCMS_ControlType.Label            'LABEL
                        mLabel.CssClass = Me.CssClass
                        mLabel.RenderControl(writer)
                    Case AJCMS_ControlType.Generic          'GENERIC
                        Dim mWrite As String
                        If mGeneric.NoExtraTags Then
                            mWrite = IIf(mGeneric.Text.Length > 0, mGeneric.Text.Trim, IIf(MyBase.DesignMode, Me.ID, ""))
                            If Not String.IsNullOrWhiteSpace(mGeneric.PrefixWhenNoExtraTags) Then
                                mWrite = mGeneric.PrefixWhenNoExtraTags & mWrite
                            End If
                            If Not String.IsNullOrWhiteSpace(mGeneric.SufixWhenNoExtraTags) Then
                                mWrite = mWrite & mGeneric.SufixWhenNoExtraTags
                            End If
                        Else
                            mWrite = "<p class='" & Me.CssClass & "' style='width:" & mGeneric.Width.ToString & ";" & IIf(mGeneric.EnableScrollbar, "height:" & mGeneric.Height.ToString & "; overflow:auto", "") & "'>" & IIf(mGeneric.Text.Length > 0, mGeneric.Text.Trim, " ") & "</p>"
                        End If
                        If MyBase.DesignMode Then
                            writer.RenderBeginTag("Generic")
                            'writer.Write("<table  width='" & mGeneric.Width.ToString & "'><tr><td>" & IIf(mGeneric.Text.Length > 0, mGeneric.Text, "Generic") & "</td></tr></table>")
                            writer.Write(mWrite)
                            writer.RenderEndTag()
                        Else
                            If MyBase.Visible Then
                                writer.Write(mWrite)
                                'writer.Write("<table width='" & mGeneric.Width.ToString & "'><tr><td>" & mGeneric.Text & "</td></tr></table>")
                            End If
                        End If
                    Case AJCMS_ControlType.TextBox          'TEXTBOX
                        mTextBox.CssClass = Me.CssClass
                        If Not (MyBase.DesignMode) And Me.StatusBar = AJCMS_EnableDisable.Enabled Then
                            With mTextBox
                                .Attributes.Add("onselect", "Wcc($(this).getSelection().text, '#" & Replace(Me.UniqueID, "$", "_") & "_ST');") ' onSelectWcc(this,'" & Replace(Me.UniqueID, "$", "_") & "_ST'," & MaxChars & ")")
                                .Attributes.Add("onclick", "Wcc($(this).val(),'#" & Replace(Me.UniqueID, "$", "_") & "_ST'," & MaxChars & ")")
                            End With
                        Else
                            With mTextBox
                                .Attributes.Remove("onselect")
                                .Attributes.Remove("onclick")
                            End With
                        End If
                        mTextBox.RenderControl(writer)
                        ShowStatus(writer)
                        If StatusBar = AJCMS_EnableDisable.Enabled Then
                            WordCount(Replace(Me.UniqueID, "$", "_"), Replace(Me.UniqueID, "$", "_") & "_ST", MaxChars).RenderControl(writer)
                        End If
                        'Case AJCMS_ControlType.HtmlArea         'HTMLAREA
                        '    If DesignMode Then
                        '        mHtmlArea.CssClass = Me.CssClass
                        '        mHtmlArea.RenderControl(writer)
                        '        ShowStatus(writer)
                        '    ElseIf MyBase.Visible Then
                        '        mHtmlArea.CssClass = Me.CssClass
                        '        'If Not (MyBase.DesignMode) And Me.StatusBar = AJCMS_EnableDisable.Enabled Then
                        '        '    With mTextBox
                        '        '        .Attributes.Add("onchange", "Wcc(this,'" & Me.UniqueID & "_ST'," & MaxChars & ")")
                        '        '    End With
                        '        'Else
                        '        '    With mTextBox
                        '        '        .Attributes.Remove("onchange")
                        '        '    End With
                        '        'End If
                        '        mHtmlArea.RenderControl(writer)
                        '        ShowStatus(writer)
                        '        Dim mSm As New _Library._Web.MultiControl.McScriptManager.McScriptManager
                        '        mSm = _Common.FindMcScriptManager(Me.Page)
                        '        If mSm IsNot Nothing Then
                        '            If mSm.HTML_Area_ScriptsAllowed Then
                        '                Dim mHtmlTA As New htmlArea(Me.Page, "")
                        '                mHtmlTA.ToolbarVisible = mHtmlArea.ToolBarVisible
                        '                If StatusBar = AJCMS_EnableDisable.Enabled Then
                        '                    mHtmlTA.AssociateTextAreaControlScript(Replace(Me.UniqueID, "$", "_"), Me.OnContentChange, Replace(Me.UniqueID, "$", "_") & "_ST", MaxChars).RenderControl(writer)
                        '                Else
                        '                    mHtmlTA.AssociateTextAreaControlScript(Replace(Me.UniqueID, "$", "_"), Me.OnContentChange).RenderControl(writer)
                        '                End If
                        '                mHtmlTA = Nothing
                        '            End If
                        '        End If
                        '    End If
                    Case AJCMS_ControlType.Image            'IMAGE
                        mImage.CssClass = Me.CssClass
                        mImage.RenderControl(writer)
                    Case AJCMS_ControlType.HyperLink        'HYPERLINK
                        mHyperLink.CssClass = Me.CssClass
                        mHyperLink.RenderControl(writer)
                    Case AJCMS_ControlType.DropDownList     'DROPDOWNLIST
                        mDropDownList.CssClass = Me.CssClass
                        'If Not (MyBase.DesignMode) And mDropDownList.AutoPostBack Then
                        '    mDropDownList.Attributes.Add("onchange", "javscript:submit();") ' "javascript:setTimeout('__doMultiContPostBack(\'" & mDropDownList.ID & "\',\'\')', 0)")
                        'Else
                        '    mDropDownList.Attributes.Remove("onchange")
                        'End If
                        mDropDownList.RenderControl(writer)
                    Case Else
                        writer.Write("Not available")
                        MyBase.Render(writer)
                End Select
            End Sub

            Private Sub ShowStatus(ByVal writer As System.Web.UI.HtmlTextWriter)
                If StatusBar = AJCMS_EnableDisable.Enabled Then
                    CtrlStatus(Me.UniqueID & "_ST", Me.Width).RenderControl(writer)
                End If
            End Sub

            Private Function CtrlStatus(ByVal pID As String, ByVal pWidth As Unit) As Panel
                Dim mCtrl As New Panel
                mCtrl.ID = pID
                mCtrl.CssClass = "StatusBar"
                'mCtrl.Style.Add("color", "Black")
                'mCtrl.Style.Add("background-color", "threedface")
                'mCtrl.Style.Add("border-color", "threedface")
                'mCtrl.Style.Add("border-style", "solid")
                'mCtrl.Style.Add("border-width", "1px")

                mCtrl.Height = Unit.Parse(23)
                If Me.DesignMode Then
                    Dim ml As New HtmlControls.HtmlGenericControl
                    ml.InnerHtml = "W:&nbsp;0&nbsp;&nbsp;||&nbsp;&nbsp;C:&nbsp;0&nbsp;&nbsp;||&nbsp;&nbsp;R:&nbsp;<span style=""color:red"">0</span>"
                    'If ControlType = AJCMS_ControlType.HtmlArea Then
                    '    ml.InnerHtml = "&nbsp;&nbsp;C:&nbsp;0&nbsp;&nbsp;||&nbsp;&nbsp;R:&nbsp;<span style=""color:red"">0</span>"
                    'Else
                    '    ml.InnerHtml = "W:&nbsp;0&nbsp;&nbsp;||&nbsp;&nbsp;C:&nbsp;0&nbsp;&nbsp;||&nbsp;&nbsp;R:&nbsp;<span style=""color:red"">0</span>"
                    'End If
                    mCtrl.Controls.Add(ml)
                    ml = Nothing
                End If
                mCtrl.Width = pWidth
                Return mCtrl
            End Function

            Private Function ControlCanPost(ByVal pControl As Object) As Boolean
                Select Case pControl.GetType.Name.ToLower
                    Case "_mytextarea"
                        Return True
                    Case "_mytextbox"
                        Return True
                        'Case "_myhtmlarea"
                        '    Return True
                    Case "_mydropdownlist"
                        Return True
                    Case Else
                        Return False
                End Select
            End Function

            Private Function InitTextArea() As Boolean
                Try
                    mTextArea = TextArea
                    mTextArea.EnableViewState = MyBase.EnableViewState
                    mTextArea.Enabled = MyBase.Enabled
                    mTextArea.Visible = MyBase.Visible
                    'mTextArea.TextMode = TextBoxMode.MultiLine
                    mTextArea.Width = MyBase.Width
                    mTextArea.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Function InitTextBox() As Boolean
                Try
                    mTextBox = TextBox
                    mTextBox.EnableViewState = MyBase.EnableViewState
                    mTextBox.Enabled = MyBase.Enabled
                    mTextBox.Visible = MyBase.Visible
                    'mTextArea.TextMode = TextBoxMode.MultiLine
                    mTextBox.Width = MyBase.Width
                    'mTextBox.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            'Private Function InitHtmlArea() As Boolean
            '    Try
            '        mHtmlArea = HtmlArea
            '        mHtmlArea.EnableViewState = MyBase.EnableViewState
            '        mHtmlArea.Enabled = MyBase.Enabled
            '        mHtmlArea.Visible = MyBase.Visible
            '        'mTextArea.TextMode = TextBoxMode.MultiLine
            '        If MyBase.DesignMode Then
            '            mHtmlArea.Width = MyBase.Width
            '            mHtmlArea.Height = MyBase.Height
            '        Else
            '            MyBase.Width = mHtmlArea.Width
            '            MyBase.Height = mHtmlArea.Height
            '        End If
            '        Return True
            '    Catch ex As Exception
            '        Return False
            '    End Try
            'End Function

            Private Function InitLabel() As Boolean
                Try
                    mLabel = Label
                    mLabel.EnableViewState = MyBase.EnableViewState
                    mLabel.Enabled = MyBase.Enabled
                    mLabel.Visible = MyBase.Visible

                    mLabel.Height = MyBase.Height
                    mLabel.Width = MyBase.Width

                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Function InitGeneric() As Boolean
                Try
                    mGeneric = Generic
                    mGeneric.Width = MyBase.Width
                    mGeneric.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Function InitImage() As Boolean
                Try
                    mImage = Image
                    mImage.EnableViewState = MyBase.EnableViewState
                    mImage.Enabled = MyBase.Enabled
                    mImage.Visible = MyBase.Visible
                    mImage.Width = MyBase.Width
                    mImage.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Function InitHyperLink() As Boolean
                Try
                    mHyperLink = HyperLink
                    mHyperLink.EnableViewState = MyBase.EnableViewState
                    mHyperLink.Enabled = MyBase.Enabled
                    mHyperLink.Visible = MyBase.Visible
                    mHyperLink.Width = MyBase.Width
                    'mHyperLink.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Function InitDropDownList() As Boolean
                Try
                    mDropDownList = DropDownList
                    mDropDownList.EnableViewState = MyBase.EnableViewState
                    mDropDownList.Enabled = MyBase.Enabled
                    mDropDownList.Visible = MyBase.Visible
                    mDropDownList.Width = MyBase.Width
                    'mDropDownList.Height = MyBase.Height
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property TextArea() As _MyTextArea
                Get
                    Return mTextArea
                End Get
                Set(ByVal value As _MyTextArea)
                    mTextArea = value
                End Set
            End Property

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property TextBox() As _MyTextBox
                Get
                    Return mTextBox
                End Get
                Set(ByVal value As _MyTextBox)
                    mTextBox = value
                End Set
            End Property

            '<DefaultValue(""), Category("Controls"), _
            'PersistenceMode(PersistenceMode.InnerProperty), _
            'DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            'Public Property HtmlArea() As _MyHtmlArea
            '    Get
            '        Return mHtmlArea
            '    End Get
            '    Set(ByVal value As _MyHtmlArea)
            '        mHtmlArea = value
            '    End Set
            'End Property

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property Label() As _MyLabel
                Get
                    Return mLabel
                End Get
                Set(ByVal value As _MyLabel)
                    mLabel = value
                End Set
            End Property

            <DefaultValue(""), Category("Controls"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property Generic() As _MyGeneric
                Get
                    Return mGeneric
                End Get
                Set(ByVal value As _MyGeneric)
                    mGeneric = value
                End Set
            End Property

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property Image() As _MyImage
                Get
                    Return mImage
                End Get
                Set(ByVal value As _MyImage)
                    mImage = value
                End Set
            End Property

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property HyperLink() As _MyHyperLink
                Get
                    Return mHyperLink
                End Get
                Set(ByVal value As _MyHyperLink)
                    mHyperLink = value
                End Set
            End Property

            <DefaultValue(""), Category("Controls (Advanced)"), _
            PersistenceMode(PersistenceMode.InnerProperty), _
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property DropDownList() As _MyDropDownList
                Get
                    Return mDropDownList
                End Get
                Set(ByVal value As _MyDropDownList)
                    mDropDownList = value
                End Set
            End Property

            Protected Overrides Sub Finalize()
                mGeneric = Nothing
                mTextArea = Nothing
                mLabel = Nothing
                mTextBox = Nothing
                mImage = Nothing
                mHyperLink = Nothing
                mDropDownList = Nothing
                MyBase.Dispose()
                MyBase.Finalize()
            End Sub

        End Class


#Region " Label "

        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyLabel
            Inherits Label

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                AccessKey = mObj(1)
                AssociatedControlID = mObj(2)
                TabIndex = mObj(3)
                BackColor = mObj(4)
                BorderColor = mObj(5)
                BorderStyle = mObj(6)
                BorderWidth = mObj(7)
                CssClass = mObj(8)
                Font.Bold = Split(mObj(9), ",")(0)
                Font.Italic = Split(mObj(9), ",")(1)
                Font.Name = Split(mObj(9), ",")(2)
                Font.Overline = Split(mObj(9), ",")(3)
                If IsNumeric(Split(mObj(9), ",")(4)) Then
                    Font.Size = Split(mObj(9), ",")(4)
                End If
                Font.Strikeout = Split(mObj(9), ",")(5)
                Font.Underline = Split(mObj(9), ",")(6)
                ForeColor = mObj(10)
                Text = mObj(11)
                EnableTheming = mObj(12)
                SkinID = mObj(13)
                ToolTip = mObj(14)
                Height = mObj(15)
                Width = mObj(16)
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(16)
                mObj(1) = AccessKey
                mObj(2) = AssociatedControlID
                mObj(3) = TabIndex
                mObj(4) = BackColor
                mObj(5) = BorderColor
                mObj(6) = BorderStyle
                mObj(7) = BorderWidth
                mObj(8) = CssClass
                mObj(9) = Font.Bold & "," & Font.Italic & "," & Font.Name & "," _
                    & Font.Overline & "," & IIf(Font.Size.Unit.Value > 0, Font.Size.Unit.Value, "") & "," _
                    & Font.Strikeout & "," & Font.Underline
                mObj(10) = ForeColor
                mObj(11) = Text
                mObj(12) = EnableTheming
                mObj(13) = SkinID
                mObj(14) = ToolTip
                mObj(15) = Height
                mObj(16) = Width
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    MyBase.AccessKey = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AssociatedControlID() As String
                Get
                    Return MyBase.AssociatedControlID
                End Get
                Set(ByVal value As String)
                    MyBase.AssociatedControlID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    MyBase.TabIndex = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderColor() As System.Drawing.Color
                Get
                    Return MyBase.BorderColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BorderColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
                    MyBase.BorderStyle = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.BorderWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.BorderWidth = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CssClass() As String
                Get
                    Return MyBase.CssClass
                End Get
                Set(ByVal value As String)
                    MyBase.CssClass = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
                Get
                    Return MyBase.Font
                End Get
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.ForeColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Text() As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    MyBase.EnableTheming = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    MyBase.SkinID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ToolTip() As String
                Get
                    Return MyBase.ToolTip
                End Get
                Set(ByVal value As String)
                    MyBase.ToolTip = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property


            Public Sub New()

            End Sub
        End Class

#End Region

#Region " Generic "
        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC2)) _
        , Serializable()> _
        Public Class _MyGeneric
            Private mOverFlow As Boolean = False
            Private mNoExtraTags As Boolean = False
            Private mText As String = ""
            Private mWidth As System.Web.UI.WebControls.Unit = Unit.Percentage(100)
            Private mHeight As System.Web.UI.WebControls.Unit = Unit.Pixel(21)
            Private mPrefixWhenNoExtraTags As String = ""
            Private mSufixWhenNoExtraTags As String = ""

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                Text = mObj(1)
                Width = mObj(2)
                Height = mObj(3)
                EnableScrollbar = mObj(4)
                NoExtraTags = mObj(5)
                mPrefixWhenNoExtraTags = mObj(6)
                mSufixWhenNoExtraTags = mObj(7)
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(7)
                mObj(1) = Text
                mObj(2) = Width
                mObj(3) = Height
                mObj(4) = EnableScrollbar
                mObj(5) = NoExtraTags
                mObj(6) = mPrefixWhenNoExtraTags
                mObj(7) = mSufixWhenNoExtraTags
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            <Browsable(True)> _
            Public Property Text() As String
                Get
                    Return mText
                End Get
                Set(ByVal value As String)
                    If value Is Nothing Then
                        mText = ""
                    Else
                        mText = value
                    End If
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return mWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    mWidth = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return mHeight
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    mHeight = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property EnableScrollbar() As Boolean
                Get
                    Return mOverFlow
                End Get
                Set(ByVal value As Boolean)
                    mOverFlow = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property NoExtraTags() As Boolean
                Get
                    Return mNoExtraTags
                End Get
                Set(ByVal value As Boolean)
                    mNoExtraTags = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            <Browsable(True)> _
            Public Property PrefixWhenNoExtraTags() As String
                Get
                    Return mPrefixWhenNoExtraTags
                End Get
                Set(ByVal value As String)
                    If value Is Nothing Then
                        mPrefixWhenNoExtraTags = ""
                    Else
                        mPrefixWhenNoExtraTags = value
                    End If
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            <Browsable(True)> _
            Public Property SufixWhenNoExtraTags() As String
                Get
                    Return mSufixWhenNoExtraTags
                End Get
                Set(ByVal value As String)
                    If value Is Nothing Then
                        mSufixWhenNoExtraTags = ""
                    Else
                        mSufixWhenNoExtraTags = value
                    End If
                End Set
            End Property

        End Class

#End Region

#Region " TextBox "

        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyTextBox
            Inherits TextBox

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                AccessKey = mObj(1)
                TabIndex = mObj(2)
                BackColor = mObj(3)
                BorderColor = mObj(4)
                BorderStyle = mObj(5)
                BorderWidth = mObj(6)
                Columns = mObj(7)
                CssClass = mObj(8)
                'Font.CopyFrom(mObj(9))
                Font.Bold = Split(mObj(9), ",")(0)
                Font.Italic = Split(mObj(9), ",")(1)
                Font.Name = Split(mObj(9), ",")(2)
                Font.Overline = Split(mObj(9), ",")(3)
                If IsNumeric(Split(mObj(9), ",")(4)) Then
                    Font.Size = Split(mObj(9), ",")(4)
                End If
                Font.Strikeout = Split(mObj(9), ",")(5)
                Font.Underline = Split(mObj(9), ",")(6)
                ForeColor = mObj(10)
                Text = mObj(11)
                AutoCompleteType = mObj(12)
                AutoPostBack = mObj(13)
                CausesValidation = mObj(14)
                EnableTheming = mObj(15)
                MaxLength = mObj(16)
                [ReadOnly] = mObj(17)
                Rows = mObj(18)
                SkinID = mObj(19)
                TextMode = mObj(20)
                ToolTip = mObj(21)
                ValidationGroup = mObj(22)
                Height = mObj(23)
                Width = mObj(24)
                Wrap = mObj(25)
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(25)
                mObj(1) = AccessKey
                mObj(2) = TabIndex
                mObj(3) = BackColor
                mObj(4) = BorderColor
                mObj(5) = BorderStyle
                mObj(6) = BorderWidth
                mObj(7) = Columns
                mObj(8) = CssClass
                'mObj(9) = Font
                mObj(9) = Font.Bold & "," & Font.Italic & "," & Font.Name & "," _
                    & Font.Overline & "," & IIf(Font.Size.Unit.Value > 0, Font.Size.Unit.Value, "") & "," _
                    & Font.Strikeout & "," & Font.Underline
                mObj(10) = ForeColor
                mObj(11) = Text
                mObj(12) = AutoCompleteType
                mObj(13) = AutoPostBack
                mObj(14) = CausesValidation
                mObj(15) = EnableTheming
                mObj(16) = MaxLength
                mObj(17) = [ReadOnly]
                mObj(18) = Rows
                mObj(19) = SkinID
                mObj(20) = TextMode
                mObj(21) = ToolTip
                mObj(22) = ValidationGroup
                mObj(23) = Height
                mObj(24) = Width
                mObj(25) = Wrap
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    MyBase.AccessKey = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    MyBase.TabIndex = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderColor() As System.Drawing.Color
                Get
                    Return MyBase.BorderColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BorderColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
                    MyBase.BorderStyle = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.BorderWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.BorderWidth = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Columns() As Integer
                Get
                    Return MyBase.Columns
                End Get
                Set(ByVal value As Integer)
                    MyBase.Columns = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CssClass() As String
                Get
                    Return MyBase.CssClass
                End Get
                Set(ByVal value As String)
                    MyBase.CssClass = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
                Get
                    Return MyBase.Font
                End Get
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.ForeColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Text() As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AutoCompleteType() As System.Web.UI.WebControls.AutoCompleteType
                Get
                    Return MyBase.AutoCompleteType
                End Get
                Set(ByVal value As System.Web.UI.WebControls.AutoCompleteType)
                    MyBase.AutoCompleteType = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AutoPostBack() As Boolean
                Get
                    Return MyBase.AutoPostBack
                End Get
                Set(ByVal value As Boolean)
                    MyBase.AutoPostBack = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CausesValidation() As Boolean
                Get
                    Return MyBase.CausesValidation
                End Get
                Set(ByVal value As Boolean)
                    MyBase.CausesValidation = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    MyBase.EnableTheming = value
                End Set
            End Property

            'maxlength (will not support if its text area)
            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property MaxLength() As Integer
                Get
                    Return MyBase.MaxLength
                End Get
                Set(ByVal value As Integer)
                    MyBase.MaxLength = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property [ReadOnly]() As Boolean
                Get
                    Return MyBase.[ReadOnly]
                End Get
                Set(ByVal value As Boolean)
                    MyBase.[ReadOnly] = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Rows() As Integer
                Get
                    Return MyBase.Rows
                End Get
                Set(ByVal value As Integer)
                    MyBase.Rows = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    MyBase.SkinID = value
                End Set
            End Property

            <DefaultValue("MultiLine"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TextMode() As System.Web.UI.WebControls.TextBoxMode
                Get
                    Return MyBase.TextMode
                End Get
                Set(ByVal value As System.Web.UI.WebControls.TextBoxMode)
                    MyBase.TextMode = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ToolTip() As String
                Get
                    Return MyBase.ToolTip
                End Get
                Set(ByVal value As String)
                    MyBase.ToolTip = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ValidationGroup() As String
                Get
                    Return MyBase.ValidationGroup
                End Get
                Set(ByVal value As String)
                    MyBase.ValidationGroup = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Wrap() As Boolean
                Get
                    Return MyBase.Wrap
                End Get
                Set(ByVal value As Boolean)
                    MyBase.Wrap = value
                End Set
            End Property

        End Class

#End Region

#Region " TextArea "
        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyTextArea
            Inherits _MyTextBox

            Sub New()
                MyBase.TextMode = TextBoxMode.MultiLine
            End Sub

            Public Overrides Property TextMode() As System.Web.UI.WebControls.TextBoxMode
                Get
                    Return MyBase.TextMode
                End Get
                Set(ByVal value As System.Web.UI.WebControls.TextBoxMode)
                    MyBase.TextMode = TextBoxMode.MultiLine
                End Set
            End Property
        End Class
#End Region

        '#Region " HtmlArea "
        '        <ToolboxItem(False) _
        '        , TypeConverter(GetType(mEOC)) _
        '        > _
        '        Public Class _MyHtmlArea
        '            Inherits _MyTextArea
        '            Private mTbV As Boolean = True

        '            <DefaultValue("True"), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
        '            <Browsable(True)> _
        '            Public Property ToolBarVisible() As Boolean
        '                Get
        '                    Return mTbV
        '                End Get
        '                Set(ByVal value As Boolean)
        '                    mTbV = value
        '                End Set
        '            End Property

        '            Public Overloads Function SetProperties(ByVal mObj As Object()) As Boolean
        '                ToolBarVisible = mObj(mObj.Length - 1)
        '                MyBase.SetProperties(mObj)
        '            End Function

        '            Public Overloads Function GetProperties() As Object()
        '                Dim mObj As Object() = MyBase.GetProperties()
        '                Dim mInd As Integer = mObj.Length
        '                ReDim Preserve mObj(mInd)
        '                mObj(mInd) = ToolBarVisible
        '                Return mObj
        '            End Function

        '        End Class
        '#End Region

#Region " Image "
        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyImage
            Inherits Image

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                AccessKey = mObj(1)
                DescriptionUrl = mObj(2)
                GenerateEmptyAlternateText = mObj(3)
                TabIndex = mObj(4)
                AlternateText = mObj(5)
                BackColor = mObj(6)
                BorderColor = mObj(7)
                BorderStyle = mObj(8)
                BorderWidth = mObj(9)
                CssClass = mObj(10)
                ForeColor = mObj(11)
                ImageUrl = mObj(12)
                EnableTheming = mObj(13)
                SkinID = mObj(14)
                Height = mObj(15)
                ImageAlign = mObj(16)
                Width = mObj(17)
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(18)
                mObj(1) = AccessKey
                mObj(2) = DescriptionUrl
                mObj(3) = GenerateEmptyAlternateText
                mObj(4) = TabIndex
                mObj(5) = AlternateText
                mObj(6) = BackColor
                mObj(7) = BorderColor
                mObj(8) = BorderStyle
                mObj(9) = BorderWidth
                mObj(10) = CssClass
                mObj(11) = ForeColor
                mObj(12) = ImageUrl
                mObj(13) = EnableTheming
                mObj(14) = SkinID
                mObj(15) = Height
                mObj(16) = ImageAlign
                mObj(17) = Width
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    MyBase.AccessKey = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DescriptionUrl() As String
                Get
                    Return MyBase.DescriptionUrl
                End Get
                Set(ByVal value As String)
                    MyBase.DescriptionUrl = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property GenerateEmptyAlternateText() As Boolean
                Get
                    Return MyBase.GenerateEmptyAlternateText
                End Get
                Set(ByVal value As Boolean)
                    MyBase.GenerateEmptyAlternateText = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    MyBase.TabIndex = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AlternateText() As String
                Get
                    Return MyBase.AlternateText
                End Get
                Set(ByVal value As String)
                    MyBase.AlternateText = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderColor() As System.Drawing.Color
                Get
                    Return MyBase.BorderColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BorderColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
                    MyBase.BorderStyle = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.BorderWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.BorderWidth = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CssClass() As String
                Get
                    Return MyBase.CssClass
                End Get
                Set(ByVal value As String)
                    MyBase.CssClass = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.ForeColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ImageUrl() As String
                Get
                    Return MyBase.ImageUrl
                End Get
                Set(ByVal value As String)
                    MyBase.ImageUrl = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Property Text() As String
                Get
                    Return ImageUrl
                End Get
                Set(ByVal value As String)
                    ImageUrl = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    MyBase.EnableTheming = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    MyBase.SkinID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ImageAlign() As System.Web.UI.WebControls.ImageAlign
                Get
                    Return MyBase.ImageAlign
                End Get
                Set(ByVal value As System.Web.UI.WebControls.ImageAlign)
                    MyBase.ImageAlign = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property

        End Class

#End Region

#Region " HyperLink "
        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyHyperLink
            Inherits HyperLink

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                AccessKey = mObj(1)
                TabIndex = mObj(2)
                BackColor = mObj(3)
                BorderColor = mObj(4)
                BorderStyle = mObj(5)
                BorderWidth = mObj(6)
                CssClass = mObj(7)
                ForeColor() = mObj(8)
                Font.Bold = Split(mObj(9), ",")(0)
                Font.Italic = Split(mObj(9), ",")(1)
                Font.Name = Split(mObj(9), ",")(2)
                Font.Overline = Split(mObj(9), ",")(3)
                If IsNumeric(Split(mObj(9), ",")(4)) Then
                    Font.Size = Split(mObj(9), ",")(4)
                End If
                Font.Strikeout = Split(mObj(9), ",")(5)
                Font.Underline = Split(mObj(9), ",")(6)
                ImageUrl = mObj(10)
                Text = mObj(11)
                EnableTheming = mObj(12)
                SkinID = mObj(13)
                ToolTip = mObj(14)
                Height = mObj(15)
                Width = mObj(16)
                MyBase.NavigateUrl = mObj(17)
                MyBase.Target = mObj(18)
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(19)
                mObj(1) = AccessKey
                mObj(2) = TabIndex
                mObj(3) = BackColor
                mObj(4) = BorderColor
                mObj(5) = BorderStyle
                mObj(6) = BorderWidth
                mObj(7) = CssClass
                mObj(8) = ForeColor()
                mObj(9) = Font.Bold & "," & Font.Italic & "," & Font.Name & "," _
                    & Font.Overline & "," & IIf(Font.Size.Unit.Value > 0, Font.Size.Unit.Value, "") & "," _
                    & Font.Strikeout & "," & Font.Underline
                mObj(10) = ImageUrl
                mObj(11) = Text
                mObj(12) = EnableTheming
                mObj(13) = SkinID
                mObj(14) = ToolTip
                mObj(15) = Height
                mObj(16) = Width
                mObj(17) = MyBase.NavigateUrl
                mObj(18) = MyBase.Target
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    MyBase.AccessKey = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    MyBase.TabIndex = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderColor() As System.Drawing.Color
                Get
                    Return MyBase.BorderColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BorderColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderStyle() As System.Web.UI.WebControls.BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(ByVal value As System.Web.UI.WebControls.BorderStyle)
                    MyBase.BorderStyle = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BorderWidth() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.BorderWidth
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.BorderWidth = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CssClass() As String
                Get
                    Return MyBase.CssClass
                End Get
                Set(ByVal value As String)
                    MyBase.CssClass = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.ForeColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
                Get
                    Return MyBase.Font
                End Get
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ImageUrl() As String
                Get
                    Return MyBase.ImageUrl
                End Get
                Set(ByVal value As String)
                    MyBase.ImageUrl = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Text() As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    MyBase.EnableTheming = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    MyBase.SkinID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ToolTip() As String
                Get
                    Return MyBase.ToolTip
                End Get
                Set(ByVal value As String)
                    MyBase.ToolTip = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property

        End Class
#End Region

#Region " DropDownList "
        <ToolboxItem(False) _
        , TypeConverter(GetType(mEOC)) _
        > _
        Public Class _MyDropDownList
            Inherits DropDownList

            Public Function SetProperties(ByVal mObj As Object()) As Boolean
                AccessKey = mObj(1)
                TabIndex = mObj(2)
                BackColor = mObj(3)
                CssClass = mObj(4)
                Font.Bold = Split(mObj(5), ",")(0)
                Font.Italic = Split(mObj(5), ",")(1)
                Font.Name = Split(mObj(5), ",")(2)
                Font.Overline = Split(mObj(5), ",")(3)
                If IsNumeric(Split(mObj(5), ",")(4)) Then
                    Font.Size = Split(mObj(5), ",")(4)
                End If
                Font.Strikeout = Split(mObj(5), ",")(5)
                Font.Underline = Split(mObj(5), ",")(6)
                ForeColor = mObj(6)
                AppendDataBoundItems = mObj(7)
                AutoPostBack = mObj(8)
                CausesValidation = mObj(9)
                EnableTheming = mObj(10)
                SkinID = mObj(11)
                ToolTip = mObj(12)
                ValidationGroup = mObj(13)
                DataMember = mObj(14)
                DataSourceID = mObj(15)
                DataTextField = mObj(16)
                DataTextFormatString = mObj(17)
                DataValueField = mObj(18)
                Height = mObj(19)
                Width = mObj(20)
                Dim mItem As ListItem
                Items.Clear()
                For Each mI As String In mObj(21)
                    If mI IsNot Nothing Then
                        mItem = New ListItem
                        mItem.Text = mI.Split(",")(0)
                        mItem.Value = mI.Split(",")(1)
                        mItem.Selected = IIf(mI.Split(",")(2) = 1, True, False)
                        Items.Add(mItem)
                    End If
                Next
            End Function

            Public Function GetProperties() As Object()
                Dim mObj As Object()
                ReDim mObj(22)
                mObj(1) = AccessKey
                mObj(2) = TabIndex
                mObj(3) = BackColor
                mObj(4) = CssClass
                mObj(5) = Font.Bold & "," & Font.Italic & "," & Font.Name & "," _
                    & Font.Overline & "," & IIf(Font.Size.Unit.Value > 0, Font.Size.Unit.Value, "") & "," _
                    & Font.Strikeout & "," & Font.Underline
                mObj(6) = ForeColor
                mObj(7) = AppendDataBoundItems
                mObj(8) = AutoPostBack
                mObj(9) = CausesValidation
                mObj(10) = EnableTheming
                mObj(11) = SkinID
                mObj(12) = ToolTip
                mObj(13) = ValidationGroup
                mObj(14) = DataMember
                mObj(15) = DataSourceID
                mObj(16) = DataTextField
                mObj(17) = DataTextFormatString
                mObj(18) = DataValueField
                mObj(19) = Height
                mObj(20) = Width
                ReDim mObj(21)(Items.Count)
                Dim cnt As Integer = 0
                For Each mI As ListItem In Items
                    mObj(21)(cnt) = mI.Text & "," & mI.Value & "," & IIf(mI.Selected, "1", "0")
                    cnt = cnt + 1
                Next
                Return mObj
            End Function

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AccessKey() As String
                Get
                    Return MyBase.AccessKey
                End Get
                Set(ByVal value As String)
                    MyBase.AccessKey = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property TabIndex() As Short
                Get
                    Return MyBase.TabIndex
                End Get
                Set(ByVal value As Short)
                    MyBase.TabIndex = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property BackColor() As System.Drawing.Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.BackColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CssClass() As String
                Get
                    Return MyBase.CssClass
                End Get
                Set(ByVal value As String)
                    MyBase.CssClass = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides ReadOnly Property Font() As System.Web.UI.WebControls.FontInfo
                Get
                    Return MyBase.Font
                End Get
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ForeColor() As System.Drawing.Color
                Get
                    Return MyBase.ForeColor
                End Get
                Set(ByVal value As System.Drawing.Color)
                    MyBase.ForeColor = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AppendDataBoundItems() As Boolean
                Get
                    Return MyBase.AppendDataBoundItems
                End Get
                Set(ByVal value As Boolean)
                    MyBase.AppendDataBoundItems = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property AutoPostBack() As Boolean
                Get
                    Return MyBase.AutoPostBack
                End Get
                Set(ByVal value As Boolean)
                    MyBase.AutoPostBack = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property CausesValidation() As Boolean
                Get
                    Return MyBase.CausesValidation
                End Get
                Set(ByVal value As Boolean)
                    MyBase.CausesValidation = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property EnableTheming() As Boolean
                Get
                    Return MyBase.EnableTheming
                End Get
                Set(ByVal value As Boolean)
                    MyBase.EnableTheming = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property SkinID() As String
                Get
                    Return MyBase.SkinID
                End Get
                Set(ByVal value As String)
                    MyBase.SkinID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ToolTip() As String
                Get
                    Return MyBase.ToolTip
                End Get
                Set(ByVal value As String)
                    MyBase.ToolTip = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property ValidationGroup() As String
                Get
                    Return MyBase.ValidationGroup
                End Get
                Set(ByVal value As String)
                    MyBase.ValidationGroup = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DataMember() As String
                Get
                    Return MyBase.DataMember
                End Get
                Set(ByVal value As String)
                    MyBase.DataMember = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DataSourceID() As String
                Get
                    Return MyBase.DataSourceID
                End Get
                Set(ByVal value As String)
                    MyBase.DataSourceID = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DataTextField() As String
                Get
                    Return MyBase.DataTextField
                End Get
                Set(ByVal value As String)
                    MyBase.DataTextField = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DataTextFormatString() As String
                Get
                    Return MyBase.DataTextFormatString
                End Get
                Set(ByVal value As String)
                    MyBase.DataTextFormatString = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property DataValueField() As String
                Get
                    Return MyBase.DataValueField
                End Get
                Set(ByVal value As String)
                    MyBase.DataValueField = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Height() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Height
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Height = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
            Public Overrides Property Width() As System.Web.UI.WebControls.Unit
                Get
                    Return MyBase.Width
                End Get
                Set(ByVal value As System.Web.UI.WebControls.Unit)
                    MyBase.Width = value
                End Set
            End Property

            <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint), _
            DesignerSerializationVisibility( _
            DesignerSerializationVisibility.Content), _
            PersistenceMode(PersistenceMode.InnerProperty) _
            > _
            Public Overrides ReadOnly Property Items() As System.Web.UI.WebControls.ListItemCollection
                Get
                    Return MyBase.Items
                End Get
            End Property

        End Class
#End Region

        Public Class mEOC
            Inherits ExpandableObjectConverter
            Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                Return "H:" & MyBase.ConvertTo(context, culture, value.height, destinationType) & ",W:" & MyBase.ConvertTo(context, culture, value.width, destinationType)
            End Function

        End Class

        Public Class mEOC2
            Inherits ExpandableObjectConverter
            Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                Return "Generic Control "
            End Function

        End Class


        Namespace Opener
            Public Class InsertHTML
                Public Shared Function Register(ByVal pBasePage As Object) As Boolean
                    Dim mCtrl As New HtmlControls.HtmlGenericControl
                    mCtrl.InnerHtml = _InsertHTMLscript()
                    CType(pBasePage, Web.UI.Page).Header.Controls.Add(mCtrl)
                    mCtrl = Nothing
                End Function

                Public Shared Function Insert(ByVal pBasePage As Object, ByVal MultiControleName As String, ByVal Html As String) As Boolean
                    CType(pBasePage, Web.UI.Page).ClientScript.RegisterStartupScript(CType(pBasePage, Web.UI.Page).GetType, "_MC_INSERT_HTML_" & MultiControleName, "<script>_MC_InsertHtml(""" & MultiControleName & """,""" & Html & """);</script>")
                End Function

                Private Shared Function _InsertHTMLscript() As String
                    Dim mStr As New Text.StringBuilder
                    Dim cnt As Integer = 0
                    mStr.AppendLine("<script language=""javascript"" type=""text/javascript"">")
                    mStr.AppendLine("function _MC_InsertHtml(pControl,pPara) {")
                    mStr.AppendLine("	var objname     = pControl")
                    mStr.AppendLine("	var config      = opener.document.all[objname].config;")
                    mStr.AppendLine("	var editor_obj  = opener.document.all[""_"" +objname+  ""_editor""];")
                    mStr.AppendLine("	var editdoc     = editor_obj.contentWindow.document;")
                    mStr.AppendLine("	var curRange = editdoc.selection.createRange();")
                    mStr.AppendLine("	if (editdoc.selection.type == ""Control"" || curRange.htmlText) {")
                    mStr.AppendLine("   		if (!confirm(""Overwrite selected content?"")) { return; }")
                    mStr.AppendLine("		curRange.execCommand('Delete');")
                    mStr.AppendLine("		curRange = editdoc.selection.createRange();")
                    mStr.AppendLine("	}")
                    mStr.AppendLine("	opener.editor_insertHTML(objname, pPara);")
                    mStr.AppendLine("}")
                    mStr.AppendLine("</script>")
                    Return mStr.ToString
                End Function

            End Class

        End Namespace

    End Namespace

End Namespace

