Imports System.IO
Namespace _Library._Web

    Public Class clsSecureDownload
        Inherits _Base.LibraryBase
        Private mPage As System.Web.UI.Page
        Private mPath As String
        Private mFileName As String

        Public WriteOnly Property _Page() As System.Web.UI.Page
            Set(ByVal value As System.Web.UI.Page)
                mPage = value
            End Set
        End Property

        Public Property FilePath() As String
            Get
                Return mPath
            End Get
            Set(ByVal value As String)
                mPath = value & IIf(Right(value, 1) <> "\", "\", "")
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return mFileName
            End Get
            Set(ByVal value As String)
                mFileName = value
            End Set
        End Property

        Public Enum ContentType
            _Default = 1
            _Excel = 2
            _Word = 3
        End Enum

        Private Function ContentTypeConvert(ByVal pContentType As ContentType) As String
            Dim mRet As String = "application/octet-stream"
            Select Case pContentType
                Case ContentType._Excel
                    mRet = "application/vnd.ms-excel"
                Case ContentType._Word
                    mRet = "application/vnd.ms-word"
                Case ContentType._Default
                    mRet = "application/octet-stream"
            End Select
            Return mRet
        End Function


        Public Function SecureDownLoad(ByVal DeleteSource As Boolean, ByVal pContentType As ContentType) As Boolean
            Dim mret As Boolean = False
            Try
                Dim mFs As FileStream
                mFs = File.Open(mPath & "\" & mFileName, FileMode.Open)
                Dim mBytes(mFs.Length) As Byte
                mFs.Read(mBytes, 0, mFs.Length)
                mFs.Close()
                mPage.Response.AddHeader("Content-disposition", "attachment; filename=" & mFileName)
                mPage.Response.ContentType = ContentTypeConvert(pContentType)
                mPage.Response.BinaryWrite(mBytes)
                mPage.Response.End()
                mret = True
            Catch ex As Exception
                mret = False
                Throw ex
            Finally
                If DeleteSource Then
                    File.Delete(mPath & mFileName)
                End If
            End Try
            Return mret
        End Function

    End Class

End Namespace
