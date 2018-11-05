Imports System.Drawing

Namespace Library2.Web

    Public Class Image

        Public Shared Function CreateThumbnail(ByVal pBitMap As Bitmap, ByVal pWidth As Integer, ByVal pHeight As Integer, Optional ByVal pKeepAspectRatio As Boolean = True) As Bitmap
            Dim mRet As System.Drawing.Bitmap = Nothing
            Try
                If pKeepAspectRatio Then
                    Dim mRatio As Decimal
                    'Dim mNewWidth As Integer = 0
                    'Dim mNewHeight As Integer = 0
                    If pBitMap.Width < pWidth AndAlso pBitMap.Height < pHeight Then
                        Return pBitMap
                    End If
                    If pBitMap.Width > pBitMap.Height Then
                        mRatio = CDec(pWidth) / pBitMap.Width
                        Dim mTemp As Decimal = pBitMap.Height * mRatio
                        pHeight = CInt(mTemp)
                    Else
                        mRatio = CDec(pHeight) / pBitMap.Height
                        Dim mTemp As Decimal = pBitMap.Width * mRatio
                        pWidth = CInt(mTemp)
                    End If

                End If
                mRet = New Bitmap(pWidth, pHeight)
                Dim mGrx As Graphics = Graphics.FromImage(mRet)
                mGrx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                mGrx.FillRectangle(Brushes.White, 0, 0, pWidth, pHeight)
                mGrx.DrawImage(pBitMap, 0, 0, pWidth, pHeight)
                pBitMap.Dispose()
            Catch
                Return Nothing
            End Try
            Return mRet
        End Function

        Public Shared Function Watermark(ByVal pBitMap As Bitmap, ByVal pWaterMarkText As String) As Bitmap
            Dim mRet As Bitmap = pBitMap
            If pWaterMarkText <> "" Then
                Dim X As Integer = 50
                Dim Y As Integer = 0
                Dim mCanvas As Graphics = Graphics.FromImage(mRet)
                Dim mStringSizeF As SizeF
                Dim mDesiredWidth As Single
                Dim mFont As Font
                Dim mRequiredFontSize As Single
                Dim mRatio As Single
                mFont = New Font("Verdana", 6, FontStyle.Bold)
                mDesiredWidth = mRet.Width * 0.75
                mStringSizeF = mCanvas.MeasureString(pWaterMarkText, mFont)
                mRatio = mStringSizeF.Width / mFont.SizeInPoints
                mRequiredFontSize = mDesiredWidth / mRatio
                mFont = New Font("Verdana", mRequiredFontSize, FontStyle.Bold)

                mCanvas.RotateTransform(45.0F)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.Beige), X, Y)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.Gray), X + 2, Y + 2)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.Red), X, Y)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.FromArgb(128, 0, 0, 0)), X + 2, Y + 2)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.FromArgb(128, 255, 255, 255)), X, Y)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.FromArgb(128, 0, 0, 0)), X + 2, Y + 2)
                mCanvas.DrawString(pWaterMarkText, mFont, New SolidBrush(Color.FromArgb(128, 255, 255, 255)), X, Y)
                mCanvas.RotateTransform(-45.0F)

                'mRet.SetResolution(96, 96)
                mCanvas.Dispose()

            End If
            Return mRet
        End Function

        Public Shared Function CompositImage(ByVal pBackGroundImg As System.Drawing.Image, ByVal pForeGroundImg As System.Drawing.Image) As Bitmap
            Dim x As Integer = (pBackGroundImg.Width - pForeGroundImg.Width) / 2
            Dim y As Integer = (pBackGroundImg.Height - pForeGroundImg.Height) / 2
            Return Image.CompositImage(pBackGroundImg, pForeGroundImg, x, y)
        End Function


        Public Shared Function CompositImage(ByVal pBackGroundImg As System.Drawing.Image, ByVal pForeGroundImg As System.Drawing.Image, ByVal pForeGroundImgX As Integer, ByVal pForeGroundImgY As Integer) As Bitmap
            Dim mRet As New Bitmap(pBackGroundImg)
            Dim mCanvas As Graphics = Graphics.FromImage(mRet)
            mCanvas.CompositingMode = Drawing2D.CompositingMode.SourceOver
            mCanvas.DrawImage(pForeGroundImg, pForeGroundImgX, pForeGroundImgY)
            Return mRet
        End Function


    End Class

End Namespace
