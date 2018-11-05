Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace _Library._Web

    Public Class _Common

        Public Shared Function Message(ByVal pMessage As String, ByVal pPage As Web.UI.Page) As Boolean
            Dim mScript As New Text.StringBuilder
            Dim mMsg As String = pMessage
            mMsg = Replace(mMsg, "'", "\'")
            mMsg = Replace(mMsg, """", "\""")
            mScript.AppendLine("<script language='javascript' type='text/javascript'>")
            mScript.AppendLine("alert('" & mMsg & "');")
            mScript.AppendLine("</script>")
            pPage.ClientScript.RegisterStartupScript(pPage.GetType, "MESSAGE", mScript.ToString)
        End Function

        Public Shared Function CloseWindow(ByVal pPage As Web.UI.Page, Optional ByVal AlertMessage As String = "") As Boolean
            Dim mSCriptName As String
            Dim mScriptType As Type
            Dim mScript As New Text.StringBuilder
            mScript.Append("<script type='text/javascript' language='javascript'>" & vbCrLf)
            If Not String.IsNullOrEmpty(AlertMessage) Then
                mScript.Append("alert('" & AlertMessage & "');" & vbCrLf)
            End If
            mScript.Append(" var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;" & vbCrLf)
            mScript.Append("   if (ie7) " & vbCrLf)
            mScript.Append("     {     " & vbCrLf)
            mScript.Append("      //This method is required to close a window without any prompt for IE7" & vbCrLf)
            mScript.Append("          window.open('','_parent','');" & vbCrLf)
            mScript.Append("          window.close();" & vbCrLf)
            mScript.Append("          }" & vbCrLf)
            mScript.Append("    else " & vbCrLf)
            mScript.Append("        {" & vbCrLf)
            mScript.Append("           //This method is required to close a window without any prompt for IE6" & vbCrLf)
            mScript.Append("           this.focus();" & vbCrLf)
            mScript.Append("           self.opener = this;" & vbCrLf)
            mScript.Append("           self.close();" & vbCrLf)
            mScript.Append("           }" & vbCrLf)
            mScript.Append("" & vbCrLf)


            'mScript.Append("window.opener=self;" & vbCrLf)
            'mScript.Append("window.close();" & vbCrLf)
            mScript.Append("</script>" & vbCrLf)
            mSCriptName = "SelfClose"
            mScriptType = pPage.GetType

            If (Not pPage.ClientScript.IsClientScriptBlockRegistered(mScriptType, mSCriptName)) Then
                pPage.ClientScript.RegisterStartupScript(mScriptType, mSCriptName, mScript.ToString(), False)
            End If
        End Function

        Public Shared Function PutNullIfNotDate(ByVal value As String) As String
            If Not IsDate(value) Then
                Return "NULL"
            ElseIf value = "00:00:00" Then
                Return "NULL"
            ElseIf value = "01/01/0001 00:00:00" Then
                Return "NULL"
            Else
                Return "'" & value & "'"
            End If
        End Function

        Public Shared Function RemoveArrayElement(ByVal pArray() As Object, ByVal IndexToBeRemoved As Integer) As Object()
            If IndexToBeRemoved >= 0 And IndexToBeRemoved < UBound(pArray) Then
                Dim mAr(UBound(pArray) - 1) As Object
                For cnt As Integer = 0 To UBound(pArray)
                    If cnt >= IndexToBeRemoved Then
                        If cnt + 1 < UBound(pArray) Then
                            mAr(cnt) = pArray(cnt + 1)
                        End If
                    Else
                        mAr(cnt) = pArray(cnt)
                    End If
                Next
                pArray = mAr
            End If
            Return pArray
        End Function

#Region " HTML TAG REMOVER "

        Public Shared Function RemoveHtmlTags(ByVal html As String) As String
            If html Is Nothing Or html = String.Empty Then
                Return ""
            Else
                'html = Replace(html, """", """""")
                'html = Replace(html, "'", "`")
                html = Replace(html, "<BR>", "@ARAKKAL@")
                html = _ScriptsStyleCleaner(html)
                html = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", String.Empty)
                html = _SpaceCleaner(html)
                html = Replace(html, "@ARAKKAL@", vbCrLf)
                html = Replace(html, "^", "")
                Return html
            End If
        End Function

        Private Shared Function _SpaceCleaner(ByVal pStr As String) As String
            Dim mRet As String = ""
            Dim mSpaceFound As Boolean = False
            pStr = Replace(pStr, vbCrLf, " ")
            pStr = Replace(pStr, vbTab, " ")
            pStr = Replace(pStr, "&nbsp;", " ")
            For Each mC As Char In pStr
                If mC <> " " Then
                    mSpaceFound = False
                End If
                If Not mSpaceFound Then
                    mRet += mC
                End If
                If mC = " " Then
                    mSpaceFound = True
                End If
            Next
            Return mRet
        End Function

        Private Shared Function _ScriptsStyleCleaner(ByVal pStr As String) As String
            Dim mStSkip As Boolean = False
            Dim mScSkip As Boolean = False
            Dim mRet As String = ""
            Dim mScSt As String = "<script"
            Dim mScEnd As String = "</script>"
            Dim mStSt As String = "<style"
            Dim mStEnd As String = "</style>"
            For cnt As Integer = 1 To pStr.Length
                If Mid(pStr, cnt, Len(mScSt)).ToLower = mScSt Then
                    mScSkip = True
                ElseIf Mid(pStr, cnt, Len(mStSt)).ToLower = mStSt Then
                    mStSkip = True
                End If

                If (Not mScSkip) And (Not mStSkip) Then
                    mRet += Mid(pStr, cnt, 1)
                End If

                If Mid(pStr, cnt, Len(mScEnd)).ToLower = mScEnd Then
                    cnt = cnt + Len(mScEnd) - 1
                    mScSkip = False
                ElseIf Mid(pStr, cnt, Len(mStEnd)).ToLower = mStEnd Then
                    cnt = cnt + Len(mStEnd) - 1
                    mStSkip = False
                End If
            Next
            Return mRet
        End Function

#End Region

        Public Shared Function MyTextMarker(ByVal pSource As String, ByVal pStrToMark As String, ByVal pColor As String) As String
            Dim mRet As New Text.StringBuilder
            pStrToMark = pStrToMark.Trim
            Do While True
                Dim i As Integer
                i = InStr(pSource.ToLower, pStrToMark.ToLower)
                If i > 0 Then
                    mRet.Append(Left(pSource, (i - 1)))
                    mRet.Append("<span style=""background-color: " & pColor & ";"">" & Mid(pSource, i, pStrToMark.Length) & "</span>")
                    pSource = Mid(pSource, i + pStrToMark.Length)
                Else
                    Exit Do
                End If
            Loop
            mRet.Append(pSource)
            Return mRet.ToString
        End Function

        Public Shared Function MyCint(ByVal value As String) As Integer
            If IsNumeric(value) Then
                Return CInt(value)
            Else
                Return 0
            End If
        End Function

        Public Shared Function MyCdbl(ByVal value As String) As Double
            If IsNumeric(value) Then
                Return CDbl(value)
            Else
                Return 0
            End If
        End Function

        Public Shared Function MyCDate(ByVal pDate As Date) As String
            Dim mRet As String = ""
            Try
                mRet = pDate.Day & "-" & MyCMonth(pDate.Month) & "-" & pDate.Year
            Catch ex As Exception
                mRet = ""
            End Try
            Return mRet
        End Function

        Public Shared Function MyCMonth(ByVal pMonth As Integer) As String
            Select Case pMonth
                Case 1 : Return "Jan"
                Case 2 : Return "Feb"
                Case 3 : Return "Mar"
                Case 4 : Return "Apr"
                Case 5 : Return "May"
                Case 6 : Return "Jun"
                Case 7 : Return "Jul"
                Case 8 : Return "Aug"
                Case 9 : Return "Sep"
                Case 10 : Return "Oct"
                Case 11 : Return "Nov"
                Case Else : Return "Dec"
            End Select
        End Function

        Public Shared Function FindDdlItemIndex(ByVal DropDownListControl As DropDownList, ByVal Text As String, Optional ByVal FindByValue As Boolean = True) As Integer
            Dim mRet As Integer = -1
            For cnt As Integer = 1 To DropDownListControl.Items.Count
                If FindByValue Then
                    If DropDownListControl.Items(cnt - 1).Value = Text Then
                        mRet = cnt - 1
                        Exit For
                    End If
                Else
                    If DropDownListControl.Items(cnt - 1).Text = Text Then
                        mRet = cnt - 1
                        Exit For
                    End If
                End If
            Next
            Return mRet
        End Function

        Public Shared Function FindListBoxItemIndex(ByVal ListBoxControl As ListBox, ByVal Text As String, Optional ByVal FindByValue As Boolean = True) As Integer
            Dim mRet As Integer = -1
            For cnt As Integer = 1 To ListBoxControl.Items.Count
                If FindByValue Then
                    If ListBoxControl.Items(cnt - 1).Value = Text Then
                        mRet = cnt - 1
                        Exit For
                    End If
                Else
                    If ListBoxControl.Items(cnt - 1).Text = Text Then
                        mRet = cnt - 1
                        Exit For
                    End If
                End If
            Next
            Return mRet
        End Function

        Public Shared Function BooleanToInteger(ByVal pVal As Boolean) As Integer
            If pVal Then
                Return 1
            Else
                Return 0
            End If
        End Function

        Public Shared Function MyToDateTime(ByVal Value As String) As String
            If IsDate(Value) Then
                Return Convert.ToDateTime(Value)
            ElseIf Value = "00:00:00" Then
                Return ""
            Else
                Return ""
            End If
        End Function

        Public Shared Function MyIsDate(ByVal Value As String) As Date
            If IsDate(Value) Then
                Return Value
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function RandomNumbers(ByVal upperbound As Integer, ByVal lowerbound As Integer) As String
            Return CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))
        End Function

        Public Shared Function EnsureVisible(ByRef pTreeView As TreeView) As Boolean
            If pTreeView IsNot Nothing Then
                pTreeView.CollapseAll()
                _EnsureVisible(pTreeView.SelectedNode)
            End If
        End Function

        Private Shared Function _EnsureVisible(ByRef pNode As TreeNode) As Boolean
            If pNode IsNot Nothing Then
                pNode.Expand()
                If pNode.Parent IsNot Nothing Then
                    _EnsureVisible(pNode.Parent)
                End If
            End If
        End Function

        Public Shared Function SetSrcAttributes(ByVal Control As Object, ByVal Key As String, ByVal Src As String) As Boolean
            Control.Attributes.Item(Key) = "document.getElementById('" & Control.ClientID & "').src='" & Src & "';"
        End Function


        Public Shared Sub MaintainScrollPos(pCtrl As HtmlControls.HtmlGenericControl)
            Dim mHf As New HiddenField
            mHf.ID = "hf_" & pCtrl.ID & "_scrollpos"
            If pCtrl.FindControl(mHf.ID) Is Nothing Then
                mHf.Value = Nothing
                mHf.EnableViewState = True
                pCtrl.Controls.Add(mHf)
            End If
            pCtrl.Attributes.Item("onscroll") = "javascript:document.getElementById('" & mHf.ClientID & "').value = this.scrollTop +','+this.scrollLeft"
            Dim mScript As New HtmlControls.HtmlGenericControl("script")
            mScript.ID = "js_" & pCtrl.ID & "_scrollpos"
            If pCtrl.FindControl(mScript.ID) Is Nothing Then
                mScript.Attributes.Item("language") = "javascript"
                mScript.Attributes.Item("type") = "text/javascript"
                mScript.InnerHtml = "try{" & vbCrLf
                mScript.InnerHtml += "   document.getElementById('" & pCtrl.ClientID & "').scrollTop = document.getElementById('" & mHf.ClientID & "').value.split(',')[0];" & vbCrLf
                mScript.InnerHtml += "   document.getElementById('" & pCtrl.ClientID & "').scrollLeft = document.getElementById('" & mHf.ClientID & "').value.split(',')[1]" & vbCrLf
                mScript.InnerHtml += "} catch(e){ } " & vbCrLf
                pCtrl.Controls.Add(mScript)
            End If
        End Sub


        'Public Shared Function FindWebControls(ByVal pPage As System.Web.UI.Page) As List(Of Web.UI.WebControls.WebControl)
        '    Dim mC As New _Common
        '    Dim mRet As New List(Of Web.UI.WebControls.WebControl)
        '    Dim mParent As New Object
        '    If pPage.Parent IsNot Nothing Then
        '        mParent = mC.FindTopParent(pPage.Parent)
        '    Else
        '        mParent = mC.FindTopParent(pPage.Controls(0))
        '    End If
        '    mC.FindWebControl(mParent, mRet)
        '    mC = Nothing
        '    Return mRet
        'End Function

        'Private Sub FindWebControl(ByVal pCtrlToLook As Web.UI.Control, ByRef pCollectHere As List(Of Web.UI.WebControls.WebControl))
        '    If pCtrlToLook IsNot Nothing Then
        '        If pCtrlToLook.GetType.Namespace = "System.Web.UI.WebControls" Then
        '            Try
        '                pCollectHere.Add(pCtrlToLook)
        '            Catch ex As Exception
        '            End Try
        '        Else
        '            If pCtrlToLook.Controls.Count > 0 Then
        '                For Each mCtrl As Web.UI.Control In pCtrlToLook.Controls
        '                    FindWebControl(mCtrl, pCollectHere)
        '                Next
        '            End If
        '        End If
        '    End If
        'End Sub

        'Public Shared Function FindMultiControls(ByVal pPage As System.Web.UI.Page) As List(Of _Library._Web.MultiControl.MultiControl)
        '    Dim mC As New _Common
        '    Dim mRet As New List(Of _Library._Web.MultiControl.MultiControl)
        '    Dim mParent As New Object
        '    If pPage.Parent IsNot Nothing Then
        '        mParent = mC.FindTopParent(pPage.Parent)
        '    Else
        '        mParent = mC.FindTopParent(pPage.Controls(0))
        '    End If
        '    mC.FindMultiControl(mParent, mRet)
        '    mC = Nothing
        '    If mRet.Count > 0 Then
        '        Return mRet
        '    Else
        '        Return Nothing
        '    End If
        'End Function



        'Private Function FindMultiControl(ByVal pCtrlToLook As Web.UI.Control, ByRef pCollectHere As List(Of _Library._Web.MultiControl.MultiControl)) As Boolean
        '    If pCtrlToLook IsNot Nothing Then
        '        If pCtrlToLook.GetType.Name = "MultiControl" Then
        '            pCollectHere.Add(pCtrlToLook)
        '        Else
        '            If pCtrlToLook.Controls.Count > 0 Then
        '                For Each mCtrl As Web.UI.Control In pCtrlToLook.Controls
        '                    FindMultiControl(mCtrl, pCollectHere)
        '                Next
        '            End If
        '        End If
        '    End If
        'End Function


 

    End Class

End Namespace
