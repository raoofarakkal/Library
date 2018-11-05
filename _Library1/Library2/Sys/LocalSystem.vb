Imports System.DirectoryServices

Namespace Library2.Sys

    Public Class LocalSystem
        Private mMachineName As String

        Public Sub New(pMachineName As String)
            mMachineName = pMachineName
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Public Function GetGroups(pUserName As String) As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDE As New DirectoryEntry
            Dim mDE2 As New DirectoryEntry
            Dim mGroups As Object
            Try
                mDE = New DirectoryEntry(String.Format("WinNT://{0}/{1}", mMachineName, pUserName))
                mGroups = mDE.Invoke("Groups", Nothing)
                For Each mGroup As Object In CType(mGroups, IEnumerable)
                    mDE2 = New DirectoryEntry(mGroup)
                    mRet.Add(mDE2.Name)
                Next

            Catch ex As Exception
                Throw New Exception(String.Format("Unable to find Groups for specified user {0}", pUserName), ex)
            Finally
                mDE.Dispose()
                mDE2.Dispose()
                mDE = Nothing
                mDE2 = Nothing
                mGroups = Nothing
            End Try

            Return mRet
        End Function

        Public Function GetUsers(pGroupName As String) As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDE As New DirectoryEntry
            Dim mDE2 As New DirectoryEntry
            Dim mGroups As Object
            Try
                mDE = New DirectoryEntry(String.Format("WinNT://{0}/{1}", mMachineName, pGroupName))
                mGroups = mDE.Invoke("Members", Nothing)
                For Each mGroup As Object In CType(mGroups, IEnumerable)
                    mDE2 = New DirectoryEntry(mGroup)
                    mRet.Add(mDE2.Name)
                Next

            Catch ex As Exception
                Throw New Exception(String.Format("Unable to find Members for specified group {0}", pGroupName), ex)
            Finally
                mDE.Dispose()
                mDE2.Dispose()
                mDE = Nothing
                mDE2 = Nothing
                mGroups = Nothing
            End Try

            Return mRet
        End Function

    End Class

End Namespace
