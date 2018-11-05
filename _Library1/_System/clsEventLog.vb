
Namespace _Library._System
    Public Class clsEventLog
        Inherits _Base.LibraryBase
        Private mLastError As String = ""
        Private mWriteToScreen As Boolean = False
        Private mServiceName As String = ""

        Public Property ServiceName() As String
            Get
                Return mServiceName
            End Get
            Set(ByVal value As String)
                mServiceName = value
            End Set
        End Property

        Public Property _LastError() As String
            Get
                Return mLastError
            End Get
            Set(ByVal value As String)
                mLastError = value
            End Set
        End Property

        Public Function WriteEvLog(ByVal pMessage As String, ByVal pType As EventLogEntryType) As Boolean
            Try
                If mLastError <> pMessage Then
                    Dim mEL As New EventLog

                    'Make sure user have full permission on 
                    'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog 
                    'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog\Security 
                    '(in regedit - right click - permission)

                    If Not Diagnostics.EventLog.SourceExists(mServiceName) Then
                        Diagnostics.EventLog.CreateEventSource(mServiceName, "Application")
                    End If

                    mEL.Source = mServiceName
                    mEL.WriteEntry(pMessage, pType)
                    mEL.Close()
                    mEL.Dispose()
                    mEL = Nothing
                    mLastError = pMessage
                End If
                If mWriteToScreen Then
                    MsgBox(pMessage)
                End If
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
        End Function

        Protected Overrides Sub Finalize()
            mLastError = Nothing
            mWriteToScreen = Nothing
            MyBase.Finalize()
        End Sub

    End Class
End Namespace

