Namespace Library2.Sys

    Public Class AppInfo

        Private Shared mVer As AppInfo = Nothing
        Public Shared Function [Get]() As AppInfo
            If mVer Is Nothing Then
                mVer = New AppInfo
                mVer._Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
                mVer._date = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location) '.ToShortDateString()
            End If
            Return mVer
        End Function

        Private _Version As String
        Public ReadOnly Property Version() As String
            Get
                Return _Version
            End Get
        End Property

        Private _date As Date
        Public ReadOnly Property [Date]() As Date
            Get
                Return _date
            End Get
        End Property

    End Class

End Namespace