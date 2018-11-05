'Imports Microsoft.VisualBasic
'Imports System.Drawing
'Imports System.Drawing.Imaging
'Imports System.Security.Cryptography
'Imports System.Drawing.Text
'Imports System.IO
'Imports System.Web

'Namespace Library2.Web.ImageCaptcha

'    Public Class Captcha

'        Private mWidth As Integer = 100
'        Public Property Width() As Integer
'            Get
'                Return mWidth
'            End Get
'            Set(ByVal value As Integer)
'                mWidth = value
'            End Set
'        End Property

'        Private mHeight As Integer = 32
'        Public Property Height() As Integer
'            Get
'                Return mHeight
'            End Get
'            Set(ByVal value As Integer)
'                mHeight = value
'            End Set
'        End Property

'        Private mLen As Integer = 4
'        Public Property Length() As Integer
'            Get
'                Return mLen
'            End Get
'            Set(ByVal value As Integer)
'                mLen = value
'            End Set
'        End Property

'        Private mEncKey As String = "AJCMSENC0987"
'        Public Property EncryptionKey() As String
'            Get
'                Return mEncKey
'            End Get
'            Set(ByVal value As String)
'                mEncKey = value
'            End Set
'        End Property

'        Public Function isTextMatchingCaptchaImage(pImageURLparameter As String, pText As String) As Boolean
'            Dim mRet As Boolean = False
'            If pText <> "" And pImageURLparameter <> "" Then
'                Try
'                    Dim mPara As String = Replace(pImageURLparameter, "cmd=GetCaptchaImage&eImgaeText=", "")
'                    mRet = IIf(Decrypt(mPara).ToLower = pText.ToLower, True, False)
'                Catch ex As Exception

'                End Try
'            End If
'            Return mRet
'        End Function

'        Public Function GetCaptchaImageUrlParameter() As String
'            Return String.Format("cmd=GetCaptchaImage&eImgaeText={0}", generateRandomText())
'        End Function

'        Private Function generateRandomText() As String
'            Dim mRet As String
'            Dim mRng As New RNGCryptoServiceProvider()
'            Dim mBuf As Byte() = New Byte(Length - 1) {}
'            mRng.GetBytes(mBuf)

'            mRet = Left(Convert.ToBase64String(mBuf).ToUpper, Length)
'            mRet = mRet.Replace("0", "W")
'            mRet = mRet.Replace("1", "Q")
'            mRet = mRet.Replace("2", "R")
'            mRet = mRet.Replace("3", "T")
'            mRet = mRet.Replace("4", "S")
'            mRet = mRet.Replace("5", "U")
'            mRet = mRet.Replace("6", "M")
'            mRet = mRet.Replace("7", "N")
'            mRet = mRet.Replace("8", "L")
'            mRet = mRet.Replace("9", "Z")
'            mRet = mRet.Replace("*", "E")
'            mRet = mRet.Replace("=", "G")
'            mRet = mRet.Replace("+", "P")
'            mRet = mRet.Replace("/", "V")


'            Return Encrypt(mRet)
'        End Function

'        Friend Function getCaptchaImage(pEncImageText As String) As Bitmap
'            Dim mImageText As String = Decrypt(pEncImageText)
'            Dim mRnd As New Random()
'            Dim mBitmap As New Bitmap(Width, Height, PixelFormat.Format24bppRgb)
'            Dim mGrx As Graphics = Graphics.FromImage(mBitmap)
'            mGrx.TextRenderingHint = TextRenderingHint.AntiAlias
'            mGrx.Clear(Color.FromArgb(236, 236, 236, 236))       'Changing Back ground Color
'            mGrx.DrawRectangle(Pens.White, 1, 1, Width - 3, Height - 3)  'Drawing inner rectangle
'            'mGrx.DrawRectangle(Pens.Black, 0, 0, width, height)          'Drawing outer rectangle border

'            Dim mymat As New System.Drawing.Drawing2D.Matrix()
'            For i As Integer = 0 To mImageText.Length - 1

'                mymat.Reset()
'                mymat.RotateAt(mRnd.[Next](-30, 0), New PointF(CSng((Width * (0.12 * i))), CSng((Height * 0.5))))
'                mGrx.Transform = mymat
'                mGrx.DrawString(mImageText.Substring(i, 1), New Font("Comic Sans MS", 15, FontStyle.Bold), Brushes.Black, CSng((Width * (0.2 * i))), CSng((Height * 0.2)))

'                mGrx.ResetTransform()
'            Next

'            Dim mPen As System.Drawing.Pen    'Start Drawing grid on image'
'            mPen = Pens.SlateGray
'            mGrx.DrawLine(mPen, 5, 8, 95, 8)
'            mGrx.DrawLine(mPen, 5, 16, 95, 16)
'            mGrx.DrawLine(mPen, 5, 24, 95, 24)

'            mGrx.DrawLine(mPen, 20, 4, 20, 28)
'            mGrx.DrawLine(mPen, 40, 4, 40, 28)
'            mGrx.DrawLine(mPen, 60, 4, 60, 28)
'            mGrx.DrawLine(mPen, 80, 4, 80, 28)    'End Drawing Grid'


'            mGrx.Dispose()
'            Return mBitmap
'        End Function

'        Friend Sub SendImage2Client(pContext As HttpContext, pBitmap As Bitmap)
'            pContext.Response.ContentType = "image/jpeg"
'            pContext.Response.Buffer = True
'            'pContext.Response.Clear()

'            Dim stream As New MemoryStream()
'            pBitmap.Save(stream, ImageFormat.Bmp)
'            Dim buff() As Byte = stream.ToArray()

'            pContext.Response.BinaryWrite(buff)
'            'pContext.Response.End()

'            'pBitmap.Save(pContext.Response.OutputStream, ImageFormat.Bmp)
'        End Sub


'        Private Function Encrypt(ByVal pString As String) As String
'            Dim mRet As String = ""
'            mRet = Library2.Sys.Security.Rijndael.Encode(pString, EncryptionKey)
'            Return mRet
'        End Function

'        Private Function Decrypt(ByVal pEncryptedString As String) As String
'            Dim mRet As String = ""
'            mRet = Library2.Sys.Security.Rijndael.DeCode(pEncryptedString, EncryptionKey)
'            Return mRet
'        End Function




'    End Class

'End Namespace

