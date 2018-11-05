Imports System.IO
Imports System.Web.UI.HtmlControls
Imports System.Text

Namespace _Library._Web

    Public Class clsTableToExl
        Inherits _Base.LibraryBase
        Private mPage As System.Web.UI.Page
        Private FOLDER As String ' = "c:\www.io.exl"
        Const EXTENSION = ".csv"

        Public Sub New(ByVal pTempFolder As String, ByVal _page As System.Web.UI.Page)
            mPage = _page
            FOLDER = pTempFolder
        End Sub

        Public Function SendCsv2Client(ByVal Table As HtmlTable) As Boolean
            Dim mTbl As New HtmlTable
            Dim mIo As New _Library._IO.clsIO
            Dim mSd As New _Library._Web.clsSecureDownload
            Dim mTempFileName As String
            Dim mStr As New StringBuilder
            Dim mComma As String = ""

            mPage.Session("Nothing") = "Nothing" 'Just to keep the session alive
            mTempFileName = mPage.Session.SessionID
            mTbl = Table
            For Each mR As HtmlTableRow In mTbl.Rows
                mComma = ""
                For Each mC As HtmlTableCell In mR.Cells
                    mStr.Append(mComma & mC.InnerText)
                    mComma = ","
                Next
                mStr.AppendLine()
            Next
            If Not Directory.Exists(FOLDER) Then
                Directory.CreateDirectory(FOLDER)
            End If
            mIo.FilePath = FOLDER & "\" & mTempFileName & EXTENSION
            mIo.Stream = mStr.ToString
            mIo.WriteFile()
            mIo = Nothing

            mSd._Page = mPage
            mSd.FilePath = FOLDER
            mSd.FileName = mTempFileName & EXTENSION
            mSd.SecureDownLoad(True, clsSecureDownload.ContentType._Excel)
            Return True
        End Function

    End Class

End Namespace
