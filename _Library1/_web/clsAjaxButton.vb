Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


Namespace _Library._Web
    <DefaultProperty("Text"), ToolboxData("<{0}:AjaxButton runat=server></{0}:AjaxButton>")> _
        Public Class AjaxButton
        Inherits Button

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Dim mScript As New HtmlControls.HtmlGenericControl("script")
            mScript.ID = "AjaxButtonScript"
            mScript.Attributes.Add("language", "javascript")
            mScript.Attributes.Add("type", "text/javascript")
            mScript.InnerHtml = submitForm()
            If MyBase.Page.Header.FindControl(mScript.ID) Is Nothing Then
                MyBase.Page.Header.Controls.Add(mScript)
            End If
            'Me.Attributes.Add("onmouseup", "submitForm(this)")
            mScript = Nothing
            MyBase.OnLoad(e)
        End Sub


        Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
            Static Run As Boolean = True
            If Run Then
                Run = False
                MyBase.OnClick(e)
                Run = True
            End If
        End Sub

        <Browsable(False)> _
        Public Overrides Property OnClientClick() As String
            Get
                Return "submitForm(this);"
            End Get
            Set(ByVal value As String)
                MyBase.OnClientClick = value
            End Set
        End Property

        <Bindable(True), Category("Behavior"), Localizable(True)> _
        Public Property ConfirmationText() As String
            Get
                Return ViewState(__AJAXBUTTON_CT)
            End Get
            Set(ByVal value As String)
                ViewState(__AJAXBUTTON_CT) = value
            End Set
        End Property

        Private Function submitForm() As String
            Dim mStr As New StringBuilder
            mStr.AppendLine("function submitForm(obj) {")
            If Not String.IsNullOrEmpty(ConfirmationText) Then
                mStr.AppendLine(String.Format("if(confirm('')) {", ConfirmationText))
            End If
            mStr.AppendLine("   if(obj.type=='submit') {")
            mStr.AppendLine("	    var mObj = obj;")
            mStr.AppendLine("	    obj.value='Loading...';")
            'mStr.AppendLine("	    obj.disabled=true;")
            mStr.AppendLine("	    while(true)	{")
            mStr.AppendLine("	    	try	{")
            mStr.AppendLine("	    		if (mObj.tagName == 'FORM'){mObj.submit();break;}")
            mStr.AppendLine("	    		if (mObj.tagName == 'BODY')break;")
            mStr.AppendLine("	    		if (mObj.tagName == 'HTML')break;")
            mStr.AppendLine("	    		alert(mObj.tagName);")
            mStr.AppendLine("	    		mObj = mObj.parentElement;")
            mStr.AppendLine("	    	}catch(err){break;}")
            mStr.AppendLine("	    }")
            mStr.AppendLine("    }")
            If Not String.IsNullOrEmpty(ConfirmationText) Then
                mStr.AppendLine("}")
            End If
            mStr.AppendLine("}")
            Return mStr.ToString
        End Function

    End Class
End Namespace