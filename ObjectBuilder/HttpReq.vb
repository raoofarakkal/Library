Public Class HttpReq

    Private Sub btGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGo.Click
        Dim mProxy As New _Library.proxy.ProxyInfo
        mProxy.IP = Me.tbProxy.Text.Split(":")(0)
        mProxy.IP = Me.tbProxy.Text.Split(":")(0)
        mProxy.Port = Me.tbProxy.Text.Split(":")(1)
        mProxy.Domain = Me.tbUID.Text.Split("\")(0)
        mProxy.UserID = Me.tbUID.Text.Split("\")(1)
        mProxy.Password = Me.tbPWD.Text
        Me.tbResult.Text += "Requesting..."
        Me.tbResult.Refresh()
        Application.DoEvents()
        Me.tbResult.Text = Library2.Web.html.HtmlDoc.GetHTTPcontents(Me.tbURL.Text, mProxy, Me.tbReferer.Text)
        Dim mWb As New WebBrowser
        mWb.ScriptErrorsSuppressed = True
        mWb.Width = Me.Panel1.Width
        mWb.Height = Me.Panel1.Height
        mWb.DocumentText = Me.tbResult.Text
        Me.Panel1.Controls.Clear()
        Me.Panel1.Controls.Add(mWb)
        mProxy = Nothing
        Me.tbResult.Text += vbCrLf & vbCrLf & "End of Response."
    End Sub

End Class