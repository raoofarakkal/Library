Namespace Library2.Web

    Public Class Proxy

        Private mEnabled As Boolean
        Public Property Enabled() As Boolean
            Get
                Return mEnabled
            End Get
            Set(ByVal value As Boolean)
                mEnabled = value
            End Set
        End Property

        Private mUseItForNetworkCredential As Boolean = False
        Public Property UseItForNetworkCredential() As Boolean
            Get
                Return mUseItForNetworkCredential
            End Get
            Set(ByVal value As Boolean)
                mUseItForNetworkCredential = value
            End Set
        End Property

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
        Public Property UID() As String
            Get
                Return mUID
            End Get
            Set(ByVal value As String)
                mUID = value
            End Set
        End Property

        Private mPWD As String
        Public Property PWD() As String
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

        Private ReadOnly Property Credential() As Net.NetworkCredential
            Get
                If Me.UID.Length > 0 And Me.PWD.Length > 0 And Me.Domain.Length > 0 Then
                    Return New Net.NetworkCredential(Me.UID, Me.PWD, Me.Domain)
                ElseIf Me.UID.Length > 0 And Me.PWD.Length > 0 Then
                    Return New Net.NetworkCredential(Me.UID, Me.PWD)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(pConfigSectionName As String, pPwdKey As String)
            Dim mSections As Specialized.NameValueCollection = System.Configuration.ConfigurationManager.GetSection(pConfigSectionName)
            For Each mTag As String In mSections
                Select Case mTag.ToLower.Trim
                    Case "Enabled".ToLower.Trim
                        Me.Enabled = IIf(mSections.Get(mTag).ToLower.Trim = "true", True, False)
                    Case "UseItForNetworkCredential".ToLower.Trim
                        Me.UseItForNetworkCredential = IIf(mSections.Get(mTag).ToLower.Trim = "true", True, False)
                    Case "UID".ToLower.Trim
                        Me.UID = mSections.Get(mTag)
                    Case "PWD".ToLower.Trim
                        Me.PWD = mSections.Get(mTag)
                        Try
                            Me.PWD = Library2.Sys.Security.Rijndael.Decrypt(Me.PWD, pPwdKey)
                        Catch ex As Exception
                            Dim mEnc As New _Library._System._Security.Protection.Protection
                            Me.PWD = mEnc.FromBase67(Me.PWD, pPwdKey)
                            mEnc = Nothing
                        End Try
                    Case "DOMAIN".ToLower.Trim
                        Me.Domain = mSections.Get(mTag)
                    Case "IP".ToLower.Trim
                        Me.IP = mSections.Get(mTag)
                    Case "PORT".ToLower.Trim
                        Me.Port = mSections.Get(mTag)
                End Select
            Next
            mSections = Nothing
        End Sub

        Public Function toWebProxy() As Net.WebProxy
            Dim mProxy As System.Net.WebProxy = Nothing
            If Me.Enabled Then
                If Me.IP.Length > 0 Then
                    mProxy = New Net.WebProxy(mIP, mPort)
                    If Credential IsNot Nothing Then
                        mProxy.Credentials = Credential
                    End If
                End If
            End If
            Return mProxy
        End Function

        Public Function toNetworkCredential() As Net.NetworkCredential
            If Me.UseItForNetworkCredential Then
                If Me.UID.Length > 0 And Me.PWD.Length > 0 And Me.Domain.Length > 0 Then
                    Return New Net.NetworkCredential(Me.UID, Me.PWD, Me.Domain)
                ElseIf Me.UID.Length > 0 And Me.PWD.Length > 0 Then
                    Return New Net.NetworkCredential(Me.UID, Me.PWD)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End Function

    End Class

End Namespace
