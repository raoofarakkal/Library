Imports Microsoft.VisualBasic
Imports System.Collections.Specialized

Namespace Library2.Sys
    Public Class oDomain

        Public Sub New()

        End Sub

        Public Sub New(ByVal pWebConfigSectionName As String, Optional ByVal pRijndaelKeyForPWD As String = "")
            Dim mSections As NameValueCollection = System.Configuration.ConfigurationManager.GetSection("DOMAIN")
            For Each mTag As String In mSections
                Select Case mTag.ToLower
                    Case "ip"
                        IP = mSections.Get(mTag)
                    Case "dc1"
                        Dc1 = mSections.Get(mTag)
                    Case "dc2"
                        Dc2 = mSections.Get(mTag)
                    Case "uid"
                        UID = mSections.Get(mTag)
                    Case "pwd"
                        If pRijndaelKeyForPWD <> "" Then

                            'Dim mEnc As New _Library._System._Security.Protection.Protection
                            'PWD = mEnc.FromBase67(mSections.Get(mTag), pRijndaelKeyForPWD)
                            'mEnc = Nothing

                            PWD = Library2.Sys.Security.Rijndael.Decrypt(mSections.Get(mTag), pRijndaelKeyForPWD)

                        Else
                            PWD = mSections.Get(mTag)
                        End If
                End Select
            Next
            mSections = Nothing
        End Sub

        Public Sub New(ByVal pIP As String, ByVal pDC1 As String, ByVal pDC2 As String)
            IP = pIP
            Dc1 = pDC1
            Dc2 = pDC2
        End Sub

        Private mIP As String
        Public Property IP() As String
            Get
                Return mIP
            End Get
            Set(ByVal value As String)
                mIP = value
            End Set
        End Property

        Private mDc1 As String
        Public Property Dc1() As String
            Get
                Return mDc1.ToLower
            End Get
            Set(ByVal value As String)
                mDc1 = value
            End Set
        End Property

        Private mDc2 As String
        Public Property Dc2() As String
            Get
                Return mDc2.ToLower
            End Get
            Set(ByVal value As String)
                mDc2 = value
            End Set
        End Property

        Private mUID As String
        Public Property UID() As String
            Get
                Return mUID.ToLower
            End Get
            Set(ByVal value As String)
                mUID = value
            End Set
        End Property

        Private mPWD As String
        Friend Property PWD() As String
            Get
                Return mPWD
            End Get
            Set(ByVal value As String)
                mPWD = value
            End Set
        End Property


    End Class
End Namespace