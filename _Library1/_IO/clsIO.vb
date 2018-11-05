Imports Microsoft.VisualBasic
Imports System.IO

Namespace _Library._IO

    Public Class clsIO
        Inherits _Base.LibraryBase
        Private mFilePath As String
        Private mText As String


        Public Shared Function SplitDirFileName(ByVal pPath As String) As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDir As String = ""
            Dim mFile As String = ""
            Dim mLastItem As String = ""
            Dim mSlash As String = ""
            Try
                If InStr(pPath.Split("\")(pPath.Split("\").Length - 1), ".") > 0 Then
                    mLastItem = pPath.Split("\")(pPath.Split("\").Length - 1)
                    For Each mS As String In pPath.Split("\")
                        If mS = mLastItem Then
                            mFile = mS
                        Else
                            mDir += mSlash + mS
                            mSlash = "\"
                        End If
                    Next
                Else
                    mDir = pPath
                End If
            Catch ex As Exception
                mDir = pPath
            End Try
            mRet.Add(mDir)
            mRet.Add(mFile)
            Return mRet
        End Function

        Public Property FilePath() As String
            Get
                Return mFilePath
            End Get
            Set(ByVal value As String)
                mFilePath = value
            End Set
        End Property

        Public Property Stream() As String
            Get
                Return mText
            End Get
            Set(ByVal value As String)
                mText = value
            End Set
        End Property

        Public Function WriteFile(Optional ByVal CreateFolderStructure As Boolean = False) As Boolean
            Dim mWriter As StreamWriter
            Dim mStream As FileStream
            Dim mFldr As String = ""
            Dim mSlash As String = ""
            Try
                'If CreateFolderStructure Then
                '    For Each mFolder As String In mFilePath.Split("\")
                '        mFldr += mSlash & mFolder.Trim
                '        If Not Directory.Exists(mFldr) Then
                '            Directory.CreateDirectory(mFolder)
                '        End If
                '        mSlash = "\"
                '    Next
                'End If
                mStream = New FileStream(mFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
                mStream.SetLength(0)
                mWriter = New StreamWriter(mStream)
                With mWriter
                    .Write(mText)
                    .WriteLine()
                    .Close()
                End With
                mWriter = Nothing
                mStream = Nothing
                File.SetLastAccessTime(mFilePath, Date.Now)
                File.SetLastWriteTime(mFilePath, Date.Now)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ReadFile() As Boolean
            Dim mReader As StreamReader
            Dim mStream As FileStream
            mText = ""
            Try
                If File.Exists(mFilePath) Then
                    mStream = New FileStream(mFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    If mStream.Length > 0 Then
                        mReader = New StreamReader(mStream, System.Text.Encoding.Default)
                        mText = mReader.ReadToEnd
                        mReader.Close()
                        mReader = Nothing
                    Else
                        mText = "File is empty!."
                    End If
                    mStream = Nothing
                Else
                    mText = "File not found!."
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

    End Class

End Namespace

