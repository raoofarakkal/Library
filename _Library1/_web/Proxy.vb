Imports System.Net
Imports System.IO
Namespace _Library.proxy

    Public Class ProxyInfo

        Private mIP As String
        Public Property IP() As String
            Get
                Return mIP
            End Get
            Set(ByVal value As String)
                mIP = value
            End Set
        End Property

        Private mPort As Integer = 80
        Public Property Port() As Integer
            Get
                Return mPort
            End Get
            Set(ByVal value As Integer)
                mPort = value
            End Set
        End Property

        Private mUID As String
        Public Property UserID() As String
            Get
                Return mUID
            End Get
            Set(ByVal value As String)
                mUID = value
            End Set
        End Property

        Private mPWD As String
        Public Property Password() As String
            Get
                Return mPWD
            End Get
            Set(ByVal value As String)
                mPWD = value
            End Set
        End Property

        Private mDc As String
        Public Property Domain() As String
            Get
                Return mDc
            End Get
            Set(ByVal value As String)
                mDc = value
            End Set
        End Property

        Public ReadOnly Property Proxy() As WebProxy
            Get
                Dim mRet As WebProxy = Nothing
                If mIP.Length > 0 Then
                    mRet = New WebProxy(mIP, mPort)
                    If Credential IsNot Nothing Then
                        mRet.Credentials = Credential
                    End If
                End If
                Return mRet
            End Get
        End Property

        Public ReadOnly Property Credential() As NetworkCredential
            Get
                If mUID.Length > 0 And mPWD.Length > 0 And mDc.Length > 0 Then
                    Return New NetworkCredential(mUID, mPWD, mDc)
                ElseIf mUID.Length > 0 And mPWD.Length > 0 Then
                    Return New NetworkCredential(mUID, mPWD)
                Else
                    Return Nothing
                End If
            End Get
        End Property
    End Class

End Namespace