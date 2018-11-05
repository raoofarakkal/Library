Imports System.IO

Namespace Library2.IO

    Public Class Writer
        Private mFilePath As String
        Private mTextEncoding As System.Text.Encoding
        Private mWriter As StreamWriter
        Private mStream As FileStream


        Public Sub New(ByVal FilePathAndName As String, ByVal TextEncoding As System.Text.Encoding)
            mFilePath = FilePathAndName
            mTextEncoding = TextEncoding
            mStream = New FileStream(mFilePath, System.IO.FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
            mStream.SetLength(0)
            mWriter = New StreamWriter(mStream, mTextEncoding)
        End Sub

        Public Function Write(ByVal Text As String) As Boolean
            mWriter.Write(Text)
        End Function

        Public Function WriteLine(ByVal Text As String) As Boolean
            mWriter.WriteLine(Text)
        End Function

        Private Flushed As Boolean = False
        Public Sub Flush()
            If Not Flushed Then
                mWriter.Close()
            End If
            Flushed = True
        End Sub

        Public Sub Dispose()
            Flush()
            mWriter.Dispose()
            mStream.Dispose()
            mWriter = Nothing
            mStream = Nothing
            File.SetLastAccessTime(mFilePath, Date.Now)
            File.SetLastWriteTime(mFilePath, Date.Now)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class

    Public Class Reader
        Private mFilePath As String
        Private mTextEncoding As System.Text.Encoding
        Private mReader As StreamReader
        Private mStream As FileStream

        Public Sub New(ByVal FilePathAndName As String, ByVal TextEncoding As System.Text.Encoding)
            mFilePath = FilePathAndName
            mTextEncoding = TextEncoding
            If File.Exists(mFilePath) Then
                mStream = New FileStream(mFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                mReader = New StreamReader(mStream, mTextEncoding)

            End If
        End Sub

        Public Sub New(ByVal pFi As System.IO.FileInfo, ByVal TextEncoding As System.Text.Encoding)
            mTextEncoding = TextEncoding
            If pFi.Exists() Then
                mStream = pFi.OpenRead()
                mReader = New StreamReader(mStream, mTextEncoding)

            End If
        End Sub

        Public Function Read() As Integer
            Return mReader.Read
        End Function

        Public Function ReadBlock(ByRef buffer() As Char, ByVal index As Integer, ByVal count As Integer) As Integer
            Return mReader.ReadBlock(buffer, index, count)
        End Function

        Public Function ReadLine() As String
            Return mReader.ReadLine
        End Function

        Public Function ReadToEnd() As String
            Return mReader.ReadToEnd
        End Function

        Private Closed As Boolean = False
        Public Sub Close()
            If Not Closed Then
                mReader.Close()
            End If
            Closed = True
        End Sub

        Public Sub Dispose()
            Close()
            mReader.Dispose()
            mStream.Dispose()
            mReader = Nothing
            mStream = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class

End Namespace
