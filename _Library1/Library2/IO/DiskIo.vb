Imports System
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices.ComTypes

Namespace Library2.IO

    Friend Class Win32

        Private Const MAX_PATH As Integer = 256

        Public Structure WIN32_FIND_DATA
            Dim dwFileAttributes As Integer
            Dim ftCreationTime As FILETIME
            Dim ftLastAccessTime As FILETIME
            Dim ftLastWriteTime As FILETIME
            Dim nFileSizeHigh As Integer
            Dim nFileSizeLow As Integer
            Dim dwReserved0 As Integer
            Dim dwReserved1 As Integer
            <VBFixedString(MAX_PATH), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)> Public cFileName As String
            <VBFixedString(14), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=14)> Public cAlternate As String
        End Structure

        Public Declare Function FindFirstFile Lib "kernel32" Alias "FindFirstFileA" (ByVal lpFileName As String, ByRef lpFindFileData As Win32.WIN32_FIND_DATA) As Integer

        Public Declare Function FindNextFile Lib "kernel32" Alias "FindNextFileA" (ByVal hFindFile As Integer, ByRef lpFindFileData As Win32.WIN32_FIND_DATA) As Integer

        Public Function GetFileName(ByRef pFd As WIN32_FIND_DATA) As String
            Dim mFile As String
            mFile = Left(pFd.cFileName, InStr(pFd.cFileName, vbNullChar) - 1)
            Return mFile
        End Function

    End Class



    Public Class DiskIo
        Private mDomn As _Library._System._Security.clsImpersonatePara = Nothing

        Public Event onFound(ByVal ObjectFound As DiskIoFileinfo, ByRef CancelProcess As Boolean)
        Public Event onNotFound(ByVal sender As Object, ByVal e As EventArgs)
        Public Event onStillExecuting(ByVal sender As Object, ByRef CancelProcess As Boolean)

        Public Sub New()

        End Sub

        Public Sub RunAs(ByVal pCredential As _Library._System._Security.clsImpersonatePara)
            mDomn = pCredential
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private mDir As String = ""
        Public Property Directory() As String
            Get
                Return mDir.Trim
            End Get
            Set(ByVal value As String)
                mDir = value
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

        ''' <summary>
        ''' StartExploring() and listen to the events its raise, such as, onFound , onNotFound, ...
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StartExploring()
            If Directory.Length > 0 Then
                Dim mDir As String = Directory
                If Right(mDir, 1) = "\" Then
                    mDir = Mid(mDir, 1, Directory.Length - 1)
                End If
                mStopExploring = False
                _StartExploring(mDir)
            Else
                Throw New Exception("Directory required")
            End If
        End Sub

        Dim mStopExploring As Boolean = False
        Public Sub StopExploring()
            mStopExploring = True
        End Sub

        Private Function FindFirst(ByVal pPath As String, ByRef pFi As Win32.WIN32_FIND_DATA) As Integer
            Dim mPointer As Integer = -1
            If mDomn IsNot Nothing Then
                Dim mImpr As New _Library._System._Security.clsImpersonate(mDomn)
                Try
                    mImpr.BeginImpersonation()
                    mPointer = Win32.FindFirstFile(pPath, pFi)
                    mImpr.EndImpersonation()
                Catch ex As Exception
                    Throw ex
                Finally
                    mImpr = Nothing
                End Try
            Else
                mPointer = Win32.FindFirstFile(pPath, pFi)
            End If
            Return mPointer
        End Function

        Private Function FindNext(ByRef pPointer As Integer, ByRef pFi As Win32.WIN32_FIND_DATA) As Integer
            Dim mRet As Integer = 0
            If mDomn IsNot Nothing Then
                Dim mImpr As New _Library._System._Security.clsImpersonate(mDomn)
                Try
                    mImpr.BeginImpersonation()
                    mRet = Win32.FindNextFile(pPointer, pFi)
                    mImpr.EndImpersonation()
                Catch ex As Exception
                    Throw ex
                Finally
                    mImpr = Nothing
                End Try
            Else
                mRet = Win32.FindNextFile(pPointer, pFi)
            End If
            Return mRet
        End Function

        Private Function _StartExploring(ByVal pDir As String) As Boolean
            Dim mFi As New Win32.WIN32_FIND_DATA
            Dim mPtr As Integer
            Dim mFile As String
            Dim NoFiles As Integer

            'mPtr = Win32.FindFirstFile(String.Format("{0}\{1}", pDir, Pattern2Search), mFi)
            mPtr = FindFirst(String.Format("{0}\{1}", pDir, Pattern2Search), mFi)
            If mPtr = -1 Then
                RaiseEvent onNotFound(Me, Nothing)
            Else
                Dim mCancel As Boolean = False
                Do While True
                    mFile = mFi.cFileName.Trim
                    If (Not String.IsNullOrEmpty(mFile)) Then
                        If mFile <> "." And mFile <> ".." Then
                            RaiseEvent onFound(New DiskIo.DiskIoFileinfo(mFi, pDir), mCancel)
                            If mCancel Then
                                Exit Do
                            End If
                            If System.IO.Directory.Exists(String.Format("{0}\{1}", pDir, mFile)) And IncludeSubDirectories Then
                                _StartExploring(String.Format("{0}\{1}", pDir, mFile))
                            End If
                        End If
                    End If
                    RaiseEvent onStillExecuting(Me, mCancel)
                    If mCancel Or mStopExploring Then
                        Exit Do
                    End If

                    'NoFiles = Win32.FindNextFile(mPtr, mFi)
                    NoFiles = FindNext(mPtr, mFi)

                    If NoFiles = 0 Then
                        Exit Do
                    End If
                Loop
                If mCancel Then
                    Throw New Exception("Cancelled by user")
                End If
            End If
        End Function

        Public Class DiskIoFileinfo
            Dim mFi As Win32.WIN32_FIND_DATA
            Dim mDir As String = ""

            Public Sub New()
                mFi = New Win32.WIN32_FIND_DATA
            End Sub

            Friend Sub New(ByVal pFi As Win32.WIN32_FIND_DATA, ByVal pDir As String)
                mFi = pFi
                mDir = pDir
            End Sub

            Protected Overrides Sub Finalize()
                mFi = Nothing
                MyBase.Finalize()
            End Sub

            Public Function IsDirectoryExist() As Boolean
                Try
                    Return System.IO.Directory.Exists(FullName)
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Public Function isFileExist() As Boolean
                Try
                    Return System.IO.File.Exists(FullName)
                Catch ex As Exception
                    Return False
                End Try
            End Function

            ReadOnly Property Attribute() As Integer
                Get
                    Return mFi.dwFileAttributes
                End Get
            End Property

            ReadOnly Property CreationTime() As FILETIME
                Get
                    Return mFi.ftCreationTime
                End Get
            End Property

            ReadOnly Property LastAccessTime() As FILETIME
                Get
                    Return mFi.ftLastAccessTime
                End Get
            End Property

            ReadOnly Property LastWriteTime() As FILETIME
                Get
                    Return mFi.ftLastWriteTime
                End Get
            End Property

            ReadOnly Property FileSizeHigh() As Integer
                Get
                    Return mFi.nFileSizeHigh
                End Get
            End Property

            ReadOnly Property FileSizeLow() As Integer
                Get
                    Return mFi.nFileSizeLow
                End Get
            End Property

            ReadOnly Property Path() As String
                Get
                    Return mDir
                End Get
            End Property

            ReadOnly Property FileName() As String
                Get
                    Return mFi.cFileName
                End Get
            End Property

            ReadOnly Property FullName() As String
                Get
                    Return String.Format("{0}\{1}", mDir, mFi.cFileName)
                End Get
            End Property

            ReadOnly Property Alternate() As String
                Get
                    Return mFi.cAlternate
                End Get
            End Property

        End Class


    End Class

End Namespace
