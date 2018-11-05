Namespace Library2.Web

    Public Class Common

        Public Shared Sub TextBoxOnEnterKeypressed(ByRef pTextBox As Object, ByRef pTargetControl2Click As Object)
            pTextBox.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + Replace(pTargetControl2Click.UniqueID, "$", "_") + "').click();return false;}} else {return true}; ")
        End Sub

        Public Shared Sub TextBoxOnEnterKeypressed(ByRef pPage As System.Web.UI.Page, ByRef pTextBox As Object, ByRef pTargetControl2Click As Object)
            pTextBox.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {" & pPage.ClientScript.GetPostBackEventReference(pTargetControl2Click, "") & ";return false;}} else {return true}; ")
        End Sub

        Public Shared Function ToXml(ByVal pDataTable As DataTable) As String
            Dim mSw As New System.IO.StringWriter
            pDataTable.WriteXml(mSw, XmlWriteMode.WriteSchema)
            Return mSw.ToString
        End Function

        Public Shared Function ToDataTable(ByVal pTableDefenition As String) As DataTable
            Dim mD As New DataTable
            Dim mSr As New System.IO.StringReader(pTableDefenition)
            mD.ReadXml(mSr)
            Return mD
        End Function


        Public Shared Function UseNullIfEmptyDate(ByVal pDate As Object) As Object
            If MyCDate(pDate) = Date.MinValue Then
                Return "NULL"
            Else
                Return "'" & MyCDate(pDate) & "'"
            End If

        End Function

        Public Shared Function UseNullIfZero(ByVal pNumber As Object) As Object
            If MyCDbl(pNumber) = 0 Then
                Return "NULL"
            Else
                Return pNumber
            End If

        End Function

        Public Shared Function MyCDbl(ByVal pVal As Object) As Object
            If IsNumeric(pVal) Then
                Return CDbl(pVal)
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function MyCInt(ByVal pVal As Object) As Object
            If IsNumeric(pVal) Then
                Return CInt(pVal)
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function MyCLng(ByVal pVal As Object) As Object
            If IsNumeric(pVal) Then
                Return CLng(pVal)
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function MyCDate(ByVal pDate As Object) As Object
            If IsDate(pDate) Then
                Return Convert.ToDateTime(pDate).ToString("dd/MMM/yyyy HH:mm:ss")
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function ListOfString2Csv(pSource As List(Of String)) As String
            Dim mRet As String = ""
            Dim mComma As String = ""
            For Each mS As String In pSource
                If Not String.IsNullOrWhiteSpace(mS) Then
                    mRet += mComma & mS
                    mComma = ","
                End If
            Next
            Return mRet
        End Function


        Public Shared Function ConcatArray(ByVal pFirst As System.Array, ByVal pSecond As System.Array) As System.Array
            Dim mType As Type = pFirst.GetType().GetElementType()
            Dim mResult As System.Array = System.Array.CreateInstance(mType, pFirst.Length + pSecond.Length)
            pFirst.CopyTo(mResult, 0)
            pSecond.CopyTo(mResult, pFirst.Length)
            Return mResult
        End Function

        Public Shared Function GetApplicationRootUrl() As String
            Dim mF As String = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority)
            Dim mS As String = System.Web.HttpContext.Current.Request.ApplicationPath
            If Right(mF, 2) = "//" Then
                mF = Left(mF, mF.Length - 1)
            End If
            If Right(mF, 1) <> "/" Then
                mF += "/"
            End If
            If Left(mS, 1) = "/" Then
                mS = Mid(mS, 2)
            End If
            If Right(mS, 1) <> "/" Then
                mS += "/"
            End If
            If mS = "/" Then
                mS = ""
            End If
            Return mF.Trim & mS.Trim
        End Function

        Public Shared Function FromUnixTimestamp(pTimestamp As Long) As Date
            Dim mDt As New Date(1970, 1, 1, 0, 0, 0, 0)
            Return mDt.AddSeconds(pTimestamp)
        End Function

        Public Shared Function ToUnixTimestamp(pDate As Date) As Long
            Dim mDt As New Date(1970, 1, 1, 0, 0, 0, 0)
            Dim mDifference As TimeSpan = pDate - mDt
            Return Math.Floor(mDifference.TotalSeconds)
        End Function

        Public Shared Function FromUnixTimestamp(pTimestamp As Long, UseMilliSeconds As Boolean) As Date
            Dim mDt As New Date(1970, 1, 1, 0, 0, 0, 0)
            If UseMilliSeconds Then
                Return mDt.AddMilliseconds(pTimestamp)
            Else
                Return mDt.AddSeconds(pTimestamp)
            End If
        End Function

        Public Shared Function ToUnixTimestamp(pDate As Date, UseMilliSeconds As Boolean) As Long
            Dim mDt As New Date(1970, 1, 1, 0, 0, 0, 0)
            Dim mDifference As TimeSpan = pDate - mDt
            If UseMilliSeconds Then
                Return Math.Floor(mDifference.TotalMilliseconds)
            Else
                Return Math.Floor(mDifference.TotalSeconds)
            End If
        End Function

        Public Shared Sub CollectInnerExceptionMessages(pEx As Exception, ByRef pCollect As String)
            If pEx IsNot Nothing Then
                pCollect += "<br>" & pEx.Message
                CollectInnerExceptionMessages(pEx.InnerException, pCollect)
            End If
        End Sub

    End Class

End Namespace