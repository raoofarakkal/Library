'USAGE
'
'Copy the following code to Page Load Event
'
'Dim mCRM As New ContextMenu
'mCRM.AddMenuItems("MenuItem1", "MenuIcon/Card.png", "Target link or Empty string (will postback) ", "MenuItem1Para")
'mCRM.AddMenuItems("MenuItem2", "MenuIcon/Card.png", "Target link or Empty string (will postback)  ", "MenuItem2Para")
'mCRM.Register(Me.Page, Me.TreeView1, ContextMenu.TypeOfControl.TreeView)
'mCRM = Nothing

'If LINK is Empty string then Copy the following code to Page Load event
'
'Dim mEvTarget As String = "" 
'Dim mEvArg() As String
'        mEvTarget = Request.Form("__ContMenuEVENTTARGET")
'        If mEvTarget = Me.TreeView1.ID & "_postback" Then
'            mEvArg = Split(Request.Form("__ContMenuEVENTARGUMENT"), ",")
'            Select Case mEvArg(0)
'                Case "MenuItem1Para"
'                    your code goes here
'                Case "MenuItem2Para"
'                    your code goes here
'            End Select
'        End If


Imports Microsoft.VisualBasic
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Text

Namespace _Library._Web

    Public Class ContextMenu
        Private Const DefaultWidth As Integer = 55
        Private Const DefaultHeight As Integer = 29
        Private mRcMenu_title As String = ""
        Private mRcMenu_titlecol1 = "black"
        Private mRcMenu_titlecol2 = "blue"
        Private mRcMenu_titletext = "white"
        Private mRcMenu_bg = "grey"
        Private mRcMenu_bgov = "black"
        Private mRcMenu_cl = "black"
        Private mRcMenu_clov = "white"
        Private mRcMenu_width = DefaultWidth
        Private mRcMenu_height = DefaultHeight
        Private mAutoWidth As Boolean = True
        Private mAutoHeight As Boolean = True
        Private mScriptName As String = ""
        Private mType As TypeOfControl
        Private mPostbackString As String = "javascript:dopostback()"
        Private mAssociatedControl As Object
        Private mShowMenuEvenNoParaAvailable As Boolean = False

        Private mMenuText() As String
        Private mMenuIcon() As String
        Private mMenuLink() As String
        Private mMenuLinkPara() As String
        Private mMenuDispConditionInStr() As String
        Private mMenuDispConditionNotInStr() As String

        Private mMenuType As CM_MenuType = CM_MenuType.ContextMenu
        Private mPage As Web.UI.Page

        Public Enum CM_MenuType
            ContextMenu
            Floating
        End Enum

        Public Enum TypeOfControl
            TreeView
            Other
        End Enum

        Public Sub New(ByVal ContestMenuBasePage As System.Web.UI.Page)
            mPage = ContestMenuBasePage
        End Sub

        Protected Overrides Sub Finalize()
            mPage = Nothing
            MyBase.Finalize()
        End Sub

        'for no icon use 'null'
        'rcMenu_item('Show Details','Images/card.png','http://www.javascriptkit.com')
        Public Property MenuType() As CM_MenuType
            Get
                Return mMenuType
            End Get
            Set(ByVal value As CM_MenuType)
                mMenuType = value
            End Set
        End Property

        Public Property MenuTitle() As String
            Get
                Return mRcMenu_title
            End Get
            Set(ByVal value As String)
                mRcMenu_title = value
            End Set
        End Property

        Public Property TitleBgGradientColorStart() As String
            Get
                Return mRcMenu_titlecol1
            End Get
            Set(ByVal value As String)
                mRcMenu_titlecol1 = value
            End Set
        End Property

        Public Property TitleBgGradientColorEnd() As String
            Get
                Return mRcMenu_titlecol2
            End Get
            Set(ByVal value As String)
                mRcMenu_titlecol2 = value
            End Set
        End Property

        Public Property TitleColor() As String
            Get
                Return mRcMenu_titletext
            End Get
            Set(ByVal value As String)
                mRcMenu_titletext = value
            End Set
        End Property

        Public Property MenuItemBgColor() As String
            Get
                Return mRcMenu_bg
            End Get
            Set(ByVal value As String)
                mRcMenu_bg = value
            End Set
        End Property

        Public Property MenuItemBgColorOnMouseOver() As String
            Get
                Return mRcMenu_bgov
            End Get
            Set(ByVal value As String)
                mRcMenu_bgov = value
            End Set
        End Property

        Public Property MenuColor() As String
            Get
                Return mRcMenu_cl
            End Get
            Set(ByVal value As String)
                mRcMenu_cl = value
            End Set
        End Property

        Public Property MenuColorOnMouseOver() As String
            Get
                Return mRcMenu_clov
            End Get
            Set(ByVal value As String)
                mRcMenu_clov = value
            End Set
        End Property

        Public Property MenuWidth() As Integer
            Get
                Return mRcMenu_width
            End Get
            Set(ByVal value As Integer)
                mRcMenu_width = value
            End Set
        End Property

        Public Property MenuHeight() As Integer
            Get
                Return mRcMenu_height
            End Get
            Set(ByVal value As Integer)
                mRcMenu_height = value
            End Set
        End Property

        Public Property AutoHeight() As Boolean
            Get
                Return mAutoHeight
            End Get
            Set(ByVal value As Boolean)
                mAutoHeight = value
            End Set
        End Property

        Public Property AutoWidth() As Boolean
            Get
                Return mAutoWidth
            End Get
            Set(ByVal value As Boolean)
                mAutoWidth = value
            End Set
        End Property

        Public Property mShowMenuEvenNoParameterAvailable() As Boolean
            Get
                Return mShowMenuEvenNoParaAvailable
            End Get
            Set(ByVal value As Boolean)
                mShowMenuEvenNoParaAvailable = value
            End Set
        End Property

        Public Function AddMenuItems(ByVal MenuItem As String, ByVal IconPath As String, ByVal link As String, ByVal pParameter As String, Optional ByVal VisibleWhenContain As String = "", Optional ByVal VisibleWhenNotContain As String = "") As Boolean
            If mMenuText Is Nothing Then
                ReDim mMenuText(1)
                ReDim mMenuIcon(1)
                ReDim mMenuLink(1)
                ReDim mMenuLinkPara(1)
                ReDim mMenuDispConditionInStr(1)
                ReDim mMenuDispConditionNotInStr(1)
            Else
                ReDim Preserve mMenuText(UBound(mMenuText) + 1)
                ReDim Preserve mMenuIcon(UBound(mMenuIcon) + 1)
                ReDim Preserve mMenuLink(UBound(mMenuLink) + 1)
                ReDim Preserve mMenuLinkPara(UBound(mMenuLinkPara) + 1)
                ReDim Preserve mMenuDispConditionInStr(UBound(mMenuDispConditionInStr) + 1)
                ReDim Preserve mMenuDispConditionNotInStr(UBound(mMenuDispConditionNotInStr) + 1)
            End If
            mMenuText(UBound(mMenuText) - 1) = MenuItem
            mMenuIcon(UBound(mMenuIcon) - 1) = IIf(IconPath = "", "null", IconPath)
            mMenuLink(UBound(mMenuLink) - 1) = IIf(link.Trim = "", mPostbackString, link)
            mMenuLinkPara(UBound(mMenuLinkPara) - 1) = IIf(pParameter = "", "NA", pParameter)
            mMenuDispConditionInStr(UBound(mMenuDispConditionInStr) - 1) = VisibleWhenContain
            mMenuDispConditionNotInStr(UBound(mMenuDispConditionNotInStr) - 1) = VisibleWhenNotContain


            If AutoWidth Then
                If (MenuWidth < DefaultWidth + (7 * MenuItem.Length)) Then
                    MenuWidth = DefaultWidth + (7 * MenuItem.Length)
                End If
            End If

            'AUTO HEIGHT
            If AutoHeight Then
                MenuHeight = DefaultHeight
                If UBound(mMenuText) > 1 Then
                    For cnt As Integer = 2 To UBound(mMenuText)
                        If IsOdd(cnt) Then
                            MenuHeight = MenuHeight + 26
                        Else
                            MenuHeight = MenuHeight + 25
                        End If
                    Next
                End If
            End If

        End Function

        Public Property ScriptName() As String
            Get
                Return mScriptName
            End Get
            Set(ByVal value As String)
                mScriptName = value
            End Set
        End Property

        Private Function IsOdd(ByVal value As Integer) As Boolean
            Dim mMOd As Long
            mMOd = value Mod 2
            If mMOd = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function RegisterStyle(ByVal pPage As System.Web.UI.Page) As Boolean
            'Dim mCtrl As New HtmlGenericControl
            'mCtrl.InnerHtml = HeadScriptStyle()
            'pPage.Header.Controls.Add(mCtrl)
            'mCtrl = Nothing
        End Function


        Public Function Register(ByVal pPage As System.Web.UI.Page, ByVal pAssociatedControl As Object, Optional ByVal pTypeOfControl As TypeOfControl = TypeOfControl.TreeView, Optional ByVal ApplyToWholeDocument As Boolean = False) As Boolean
            mType = pTypeOfControl
            mAssociatedControl = pAssociatedControl
            If ScriptName = "" Then
                ScriptName = mAssociatedControl.ID
            End If
            Dim mCtrl As New HtmlGenericControl
            mCtrl.InnerHtml = HeadScript()
            pPage.Header.Controls.Add(mCtrl)
            mCtrl = Nothing

            pPage.ClientScript.RegisterHiddenField("__ContMenuEVENTTARGET", "")
            pPage.ClientScript.RegisterHiddenField("__ContMenuEVENTARGUMENT", "")

            If ApplyToWholeDocument Then
                Dim mScript As New StringBuilder
                mScript.AppendLine("<script language=""javascript"" type=""text/javascript"" >")
                mScript.AppendLine("window.document.oncontextmenu = function() {return " & ScriptName & "__" & "ShowMenu()};")
                mScript.AppendLine("</script>")
                Dim mCont As New HtmlGenericControl
                mCont.InnerHtml = mScript.ToString
                pPage.Header.Controls.Add(mCont)
            Else
                mAssociatedControl.Attributes.Add("oncontextmenu", "return " & ScriptName & "__" & "ShowMenu()")
            End If
        End Function

        Private Function HeadScript()
            Dim mStr As New StringBuilder
            Dim mRcMenu1 As New StringBuilder
            Dim mRcMenu2 As New StringBuilder
            mStr.AppendLine("<script language=""javascript"" type=""text/javascript"">")

            mStr.AppendLine("function " & ScriptName & "__" & "Hide(){")
            mStr.AppendLine("MyWin=window.createPopup();")
            mStr.AppendLine("MyWin.document.write('');")
            mStr.AppendLine("MyWin.show();")
            mStr.AppendLine("return false;")
            mStr.AppendLine("};")

            mStr.AppendLine("function __doContMenuPostBack(eventTarget, eventArgument) {")
            mStr.AppendLine("    document.form1.__ContMenuEVENTTARGET.value = eventTarget;")
            mStr.AppendLine("    document.form1.__ContMenuEVENTARGUMENT.value = eventArgument;")
            mStr.AppendLine("    document.form1.submit();")
            mStr.AppendLine("}")

            mStr.AppendLine("function " & ScriptName & "__" & "ShowMenu(){")

            mStr.AppendLine("Para = '';")
            mStr.AppendLine("PostBack = '" & mAssociatedControl.ID.ToString & "_postback';")

            If mType = TypeOfControl.TreeView Then
                mStr.AppendLine("Para2 = '';")
                mStr.AppendLine("Para3 = '';")
                mStr.AppendLine("if (document.activeElement.href != null) {")
                mStr.AppendLine("   Para2 = document.activeElement.href.split('(')[1].split(')')[0];")

                mStr.AppendLine("   for (num = 0; num < Para2.length; num++) {")
                mStr.AppendLine("       if (Para2.substring(num, num+1) != '\'') {")
                mStr.AppendLine("           Para3 = Para3 + Para2.substring(num, num+1);")
                mStr.AppendLine("       }")
                mStr.AppendLine("   }")

                mStr.AppendLine("   Para3 = Para3.split(',')[1].substring(1,Para3.split(',')[1].length) ;")
                mStr.AppendLine("}")


                mStr.AppendLine("Para = Para3;")
            Else
                mStr.AppendLine("Para2 = '';")
                mStr.AppendLine("if (document.activeElement.href !=null) {")
                mStr.AppendLine("Para2 = document.activeElement.href;")
                mStr.AppendLine("}")

                mStr.AppendLine("if (Para2 !='') {")
                mStr.AppendLine("   Para = Para2;")
                mStr.AppendLine("}")
                mStr.AppendLine("else {")
                mStr.AppendLine("   Para = '#';")
                mStr.AppendLine("}")

                'mStr.AppendLine("Para = Para2;")
            End If
            'mStr.AppendLine("alert(Para3);")

            mRcMenu1.Append("<style type='text/css'>.raoof{font-style:italic;color:gray}.textul{position:absolute;top:0px;color:" & mRcMenu_titletext & ";writing-mode:	tb-rl;padding-top:10px;filter: flipH() flipV() dropShadow( Color=000000,offX=-2,offY=-2,positive=true);z-Index:10;width:100%;height:100%;font: bold 12px sans-serif}.gradientul{position:relative;top:0px;left:0px;width:100%;background-color:" & mRcMenu_titlecol2 & ";height:100%;z-Index:9;FILTER: alpha( style=1,opacity=0,finishOpacity=100,startX=100,finishX=100,startY=0,finishY=100)}.contra{background-color:" & mRcMenu_titlecol1 & ";border:1px inset " & mRcMenu_bg & ";height:98%;width:18px;z-Index:8;top:0px;left:0px;margin:2px;position:absolute;}.men{position:absolute;top:0px;left:0px;padding-left:18px;background-color:" & mRcMenu_bg & ";border:2px outset " & mRcMenu_bg & ";z-Index:1;}.men a{margin:1px;cursor:default;padding-bottom:4px;padding-left:1px;padding-right:1px;padding-top:3px;text-decoration:none;height:100%;width:100%;color:" & mRcMenu_cl & ";font:normal 12px sans-serif;}.men a:hover{background:" & mRcMenu_bgov & ";color:" & mRcMenu_clov & ";} BODY{overflow:hidden;border:0px;padding:0px;margin:0px;}.ico{border:none;float:left;}</style><div class='men'>")

            mStr.AppendLine("rcmenu='';")
            mStr.AppendLine("viscount=0;")

            If Not (mMenuText Is Nothing) Then
                For cnt As Integer = 0 To UBound(mMenuText)
                    If mMenuText(cnt) <> "" Then
                        If LCase(Left(mMenuLink(cnt), mPostbackString.Length)) = "javascript:dopostback()" Then
                            mStr.AppendLine("Action" & cnt & " = '" & mMenuLinkPara(cnt) & ",';")

                            If mMenuDispConditionNotInStr(cnt) = "" And mMenuDispConditionInStr(cnt) = "" Then
                                mStr.AppendLine("rcmenu+=""<a href='#' onmousedown='self.blur(); parent.window.location.href=\""javascript:__doContMenuPostBack(PostBack, Action" & cnt & "+Para)\""'><img src='" & mMenuIcon(cnt) & "' width='16' height='16' class='ico'>" & mMenuText(cnt) & "</a>""")
                                mStr.AppendLine("viscount++;")
                            Else
                                mStr.AppendLine("Style" & cnt & " = 'Disable';")

                                If mMenuDispConditionInStr(cnt) <> "" Then
                                    mStr.AppendLine("if (Para.indexOf('" & mMenuDispConditionInStr(cnt) & "') > 0) {")
                                    mStr.AppendLine("Style" & cnt & " = 'Enable';")
                                    mStr.AppendLine("}")
                                End If
                                If mMenuDispConditionNotInStr(cnt) <> "" Then
                                    mStr.AppendLine("if (Para.indexOf('" & mMenuDispConditionNotInStr(cnt) & "') == -1) {")
                                    mStr.AppendLine("Style" & cnt & " = 'Enable';")
                                    mStr.AppendLine("}")
                                End If

                                mStr.AppendLine("if (Style" & cnt & " == 'Disable') {")
                                mStr.AppendLine("rcmenu+=""<a href='#' onmousedown='parent.window.location.href=\""#\""'><img src='" & mMenuIcon(cnt) & "' width='16' height='16' class='ico'><div class='raoof'>" & mMenuText(cnt) & "</div></a>""")
                                mStr.AppendLine("}")
                                mStr.AppendLine("else {")
                                mStr.AppendLine("rcmenu+=""<a href='#' onmousedown='parent.window.location.href=\""javascript:__doContMenuPostBack(PostBack, Action" & cnt & "+Para)\""'><img src='" & mMenuIcon(cnt) & "' width='16' height='16' class='ico'>" & mMenuText(cnt) & "</a>""")
                                mStr.AppendLine("viscount++;")
                                mStr.AppendLine("}")
                            End If
                        Else
                            mStr.AppendLine("rcmenu+=""<a href='#' onmousedown='parent.window.location.href=\""" & mMenuLink(cnt) & "?" & mMenuLinkPara(cnt) & ",""+Para+""\""'><img src='" & mMenuIcon(cnt) & "' width='16' height='16' class='ico'>" & mMenuText(cnt) & "</a>""")
                            mStr.AppendLine("viscount++;")
                        End If
                    End If
                Next
            End If

            mRcMenu2.Append("</div><div class='contra'><div class='gradientul'></div><div class='textul' id='titlu'>" & mRcMenu_title & "</div></div>")


            mStr.AppendLine("mx=event.clientX;")
            mStr.AppendLine("my=event.clientY;")
            mStr.AppendLine("menx=window.screenLeft+mx;")
            mStr.AppendLine("meny=window.screenTop+my;")
            mStr.AppendLine("sysmen=window.createPopup();")
            mStr.AppendLine("sysmen.document.write(""" & mRcMenu1.ToString & """+rcmenu+""" & mRcMenu2.ToString & """);")
            mStr.AppendLine("if(viscount > 0 && (Para !='' || " & IIf(mShowMenuEvenNoParameterAvailable, "true", "false") & ") ) {")
            mStr.AppendLine("   sysmen.show(menx,meny," & mRcMenu_width & "," & mRcMenu_height & ");")
            mStr.AppendLine("   sysmen.document.oncontextmenu=" & ScriptName & "__" & "Hide;")
            mStr.AppendLine("}")
            mStr.AppendLine("return false;")
            mStr.AppendLine("};")


            mStr.AppendLine("</script>")

            Return mStr.ToString
        End Function

        Private Function HeadScriptStyle()
            Dim mStr As New StringBuilder
            mStr.AppendLine("<style>")
            mStr.AppendLine("<!--")
            mStr.AppendLine("#men {")
            mStr.AppendLine("BORDER-RIGHT: 2px outset; BORDER-TOP: 2px outset; Z-INDEX: 1; LEFT: 0px; VISIBILITY: hidden; BORDER-LEFT: 2px outset; BORDER-BOTTOM: 2px outset; POSITION: absolute; TOP: 0px")
            mStr.AppendLine("}")
            mStr.AppendLine("#men A {")
            mStr.AppendLine("PADDING-RIGHT: 1px; PADDING-LEFT: 1px; PADDING-BOTTOM: 4px; MARGIN: 1px 1px 1px 16px; FONT: 12px sans-serif; WIDTH: 100%; PADDING-TOP: 3px; HEIGHT: 100%; TEXT-DECORATION: none")
            mStr.AppendLine("}")
            mStr.AppendLine(".ico {")
            mStr.AppendLine("BORDER-RIGHT: medium none; BORDER-TOP: medium none; FLOAT: left; BORDER-LEFT: medium none; BORDER-BOTTOM: medium none")
            mStr.AppendLine("}")
            mStr.AppendLine("//-->")
            mStr.AppendLine("</style>")

            Return mStr.ToString
        End Function


    End Class

End Namespace
