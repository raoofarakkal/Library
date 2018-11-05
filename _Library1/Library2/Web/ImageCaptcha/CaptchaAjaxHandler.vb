
'Imports System
'Imports System.Web
'Imports System.Web.SessionState
'Imports System.Drawing

'Namespace Library2.Web.ImageCaptcha

'    Public Class CaptchaAjaxHandler : Implements IHttpHandler, IReadOnlySessionState

'        Private mContext As HttpContext
'        Protected Event OnMatch(ByRef context As HttpContext, Parameters As System.Collections.Specialized.NameValueCollection)
'        Protected Event OnUnMatch(ByRef context As HttpContext, Parameters As System.Collections.Specialized.NameValueCollection)
'        Protected Event onCommandNotFound(ByRef context As HttpContext, Parameters As System.Collections.Specialized.NameValueCollection)

'        Protected Event onCommand_GetCaptchaImage(ByRef context As HttpContext, ByRef Cancel As Boolean, Parameters As System.Collections.Specialized.NameValueCollection)
'        Protected Event onCommand_GetCaptchaImageUrlpara(ByRef context As HttpContext, ByRef Cancel As Boolean, Parameters As System.Collections.Specialized.NameValueCollection)
'        Protected Event onCommand_VerifyAndSubmitData(ByRef context As HttpContext, ByRef Cancel As Boolean, Parameters As System.Collections.Specialized.NameValueCollection)

'        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
'            Get
'                Return False
'            End Get
'        End Property

'        Protected Sub Write(ByVal pStr As String)
'            mContext.Response.Write(pStr)
'        End Sub

'        Private Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
'            mContext = context
'            mContext.Response.ContentType = "text/plain"
'            Dim mParameters As System.Collections.Specialized.NameValueCollection
'            Dim mCommand As String

'            If mContext.Request.Form.Item("cmd") <> "" Then
'                mParameters = mContext.Request.Form
'            ElseIf mContext.Request.QueryString.Item("cmd") <> "" Then
'                mParameters = mContext.Request.QueryString
'            Else
'                mParameters = Nothing
'            End If

'            If mParameters IsNot Nothing Then
'                mCommand = mParameters.Item("cmd")
'                Select Case mCommand
'                    Case "GetCaptchaImage"
'                        Dim mCancel As Boolean = False
'                        RaiseEvent onCommand_GetCaptchaImage(mContext, mCancel, mParameters)
'                        If Not mCancel Then
'                            Dim mImagePara As String = mParameters.Item("eImgaeText")
'                            SendImage(mImagePara)
'                        End If
'                    Case "GetCaptchaImageUrlpara"
'                        Dim mCancel As Boolean = False
'                        RaiseEvent onCommand_GetCaptchaImageUrlpara(mContext, mCancel, mParameters)
'                        If Not mCancel Then
'                            Write(GetCaptchaImageUrlParameter())
'                        End If

'                    Case "VerifyAndSubmitData"
'                        Dim mCancel As Boolean = False
'                        RaiseEvent onCommand_VerifyAndSubmitData(mContext, mCancel, mParameters)
'                        If Not mCancel Then
'                            If isTextMatchingCaptchaImage(mParameters.Item("CaptchaUrlPara"), mParameters.Item("CaptchaUserInput")) Then
'                                RaiseEvent OnMatch(mContext, mParameters)
'                            Else
'                                RaiseEvent OnUnMatch(mContext, mParameters)
'                            End If
'                        End If
'                    Case Else
'                        RaiseEvent onCommandNotFound(mContext, mParameters)
'                End Select
'            End If

'        End Sub

'        Private Function GetCaptchaImageUrlParameter() As String
'            Dim mRet As String = ""
'            Dim mCaptcha As New ImageCaptcha.Captcha
'            mRet = mCaptcha.GetCaptchaImageUrlParameter()
'            mCaptcha = Nothing
'            Return mRet
'        End Function


'        Private Function isTextMatchingCaptchaImage(pCaptchaUrlPara As String, pCaptchaUserInput As String) As Boolean
'            Dim mRet As Boolean = False
'            Dim mCaptcha As New ImageCaptcha.Captcha
'            mRet = mCaptcha.isTextMatchingCaptchaImage(pCaptchaUrlPara, pCaptchaUserInput)
'            mCaptcha = Nothing
'            Return mRet
'        End Function

'        Private Sub SendImage(pPara)
'            Dim mC As New ImageCaptcha.Captcha
'            Dim mBitmap As Bitmap = mC.getCaptchaImage(pPara)
'            mC.SendImage2Client(mContext, mBitmap)
'            mBitmap.Dispose()
'        End Sub



'    End Class

'End Namespace