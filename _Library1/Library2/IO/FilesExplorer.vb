Imports System.IO

Namespace Library2.IO

    Public Class FileExplorer
        'Public Event onFileFound(ByVal ObjectFound As System.IO.FileInfo, ByRef CancelProcess As Boolean)
        'Public Event onDirectoryFound(ByVal ObjectFound As System.IO.DirectoryInfo, ByRef CancelProcess As Boolean)
        'Public Event onStillExecuting(ByVal sender As Object, ByRef CancelProcess As Boolean)
        Private mDomn As _Library._System._Security.clsImpersonatePara = Nothing

        Public Enum _SortOrder
            None = 0
            LastWriteTimeAsc = 1
            LastWriteTimeDesc = 2
        End Enum

        Public Sub New()

        End Sub

        Public Sub New(ByVal pCredential As _Library._System._Security.clsImpersonatePara)
            mDomn = pCredential
        End Sub

        Protected Overrides Sub Finalize()
            mDomn = Nothing
            MyBase.Finalize()
        End Sub

        Private mSeacrhLocation As String = ""
        Public Property SearchLocation() As String
            Get
                Return mSeacrhLocation.Trim
            End Get
            Set(ByVal value As String)
                mSeacrhLocation = value
            End Set
        End Property

        Private mIncSubDirs As Boolean = True
        ''' <summary>
        ''' default is true
        ''' </summary>
        ''' <remarks></remarks>
        Public Property IncludeSubDirectories() As Boolean
            Get
                Return mIncSubDirs
            End Get
            Set(ByVal value As Boolean)
                mIncSubDirs = value
            End Set
        End Property

        Private mPattern As String = "*.*"
        ''' <summary>
        ''' default is *.*
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pattern2Search() As String
            Get
                Return mPattern
            End Get
            Set(ByVal value As String)
                mPattern = value
            End Set
        End Property

        Private mSortOrder As _SortOrder = _SortOrder.None
        Public Property SortOrder() As _SortOrder
            Get
                Return mSortOrder
            End Get
            Set(ByVal value As _SortOrder)
                mSortOrder = value
            End Set
        End Property

        Public Function GetFiles() As FileInfo()
            Dim mDirInfo As DirectoryInfo
            Dim mFiles() As FileInfo
            mDirInfo = New DirectoryInfo(SearchLocation)
            If mDomn IsNot Nothing Then
                Dim mImpr As New _Library._System._Security.clsImpersonate(mDomn)
                Try
                    mImpr.BeginImpersonation()
                    mFiles = mDirInfo.GetFiles(Pattern2Search, IIf(IncludeSubDirectories, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly))
                    mFiles = mDirInfo.GetFiles(Pattern2Search, IIf(IncludeSubDirectories, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly))
                    mImpr.EndImpersonation()
                Catch ex As Exception
                    Throw New Exception(" mDirInfo.GetFiles(). " & ex.Message)
                Finally
                    mImpr = Nothing
                End Try
            Else
                mFiles = mDirInfo.GetFiles(Pattern2Search, IIf(IncludeSubDirectories, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly))
            End If
            If mFiles IsNot Nothing Then
                If mFiles.Length > 1 Then
                    Select Case SortOrder
                        Case _SortOrder.LastWriteTimeAsc
                            Try
                                Array.Sort(mFiles, New __SortFileInfoAsc)
                            Catch ex As Exception
                                Throw New Exception("list.sort(asc) failed. Try with user/admin account " & ex.Message)
                            End Try
                        Case _SortOrder.LastWriteTimeDesc
                            Try
                                Array.Sort(mFiles, New __SortFileInfoDesc)
                            Catch ex As Exception
                                Throw New Exception("list.sort(desc)  failed. Try with user/admin account " & ex.Message)
                            End Try

                    End Select
                End If
            End If
            mDirInfo = Nothing
            Return mFiles
        End Function

        Public Class __SortFileInfoAsc
            Implements IComparer  ' System.Collections.Generic.IComparer(Of FileInfo)

            'Public Function Compare1(ByVal x As System.IO.FileInfo, ByVal y As System.IO.FileInfo) As Integer Implements System.Collections.Generic.IComparer(Of System.IO.FileInfo).Compare
            '    Dim mRet As Integer = 0
            '    Dim mFile1 As FileInfo
            '    Dim mFile2 As FileInfo
            '    Try
            '        mFile1 = DirectCast(x, FileInfo)
            '        mFile2 = DirectCast(y, FileInfo)

            '        mRet = DateTime.Compare(mFile1.LastWriteTime, mFile2.LastWriteTime)
            '    Catch ex As Exception
            '        Throw ex
            '    Finally
            '        mFile1 = Nothing
            '        mFile2 = Nothing
            '    End Try
            '    Return mRet
            'End Function

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim mRet As Integer = 0
                Dim mFile1 As FileInfo
                Dim mFile2 As FileInfo
                Try
                    mFile1 = DirectCast(x, FileInfo)
                    mFile2 = DirectCast(y, FileInfo)

                    mRet = DateTime.Compare(mFile1.LastWriteTime, mFile2.LastWriteTime)
                Catch ex As Exception
                    Throw ex
                Finally
                    mFile1 = Nothing
                    mFile2 = Nothing
                End Try
                Return mRet
            End Function
        End Class

        Public Class __SortFileInfoDesc
            Implements IComparer 'System.Collections.Generic.IComparer(Of FileInfo)

            'Public Function Compare1(ByVal x As System.IO.FileInfo, ByVal y As System.IO.FileInfo) As Integer Implements System.Collections.Generic.IComparer(Of System.IO.FileInfo).Compare
            '    Dim mRet As Integer = 0
            '    Dim mFile1 As FileInfo
            '    Dim mFile2 As FileInfo
            '    Try
            '        mFile1 = DirectCast(x, FileInfo)
            '        mFile2 = DirectCast(y, FileInfo)

            '        mRet = DateTime.Compare(mFile2.LastWriteTime, mFile1.LastWriteTime)
            '    Catch ex As Exception
            '        Throw ex
            '    Finally
            '        mFile1 = Nothing
            '        mFile2 = Nothing
            '    End Try
            '    Return mRet
            'End Function

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim mRet As Integer = 0
                Dim mFile1 As FileInfo
                Dim mFile2 As FileInfo
                Try
                    mFile1 = DirectCast(x, FileInfo)
                    mFile2 = DirectCast(y, FileInfo)

                    mRet = DateTime.Compare(mFile2.LastWriteTime, mFile1.LastWriteTime)
                Catch ex As Exception
                    Throw ex
                Finally
                    mFile1 = Nothing
                    mFile2 = Nothing
                End Try
                Return mRet
            End Function

        End Class
    End Class

End Namespace