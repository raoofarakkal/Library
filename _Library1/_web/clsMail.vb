Imports Microsoft.VisualBasic
Imports System.Net.Mail
Namespace _Library._Web

    Public Class _clsMail
        Inherits _Base.LibraryBase
        Dim mSMTPserver As String = ""
        Dim mFrom As String = ""
        Dim mTo As String = ""
        Dim mCc As String = ""
        Dim mBCc As String = ""
        Dim mSubj As String = ""
        Dim mBody As String = ""
        Dim mBodyHtml As Boolean = False
        Dim mFileAttachments As String = "" 'MUST BE AVAILABLE IN WEB SERVER

        Public Property _MailServer() As String
            Get
                Return mSMTPserver
            End Get
            Set(ByVal value As String)
                mSMTPserver = value
            End Set
        End Property

        Public Property _From() As String
            Get
                Return mFrom
            End Get
            Set(ByVal value As String)
                mFrom = value
            End Set
        End Property

        Public Property _To() As String
            Get
                Return mTo
            End Get
            Set(ByVal value As String)
                mTo = value
            End Set
        End Property

        Public Property _Cc() As String
            Get
                Return mCc
            End Get
            Set(ByVal value As String)
                mCc = value
            End Set
        End Property

        Public Property _BCc() As String
            Get
                Return mBCc
            End Get
            Set(ByVal value As String)
                mBCc = value
            End Set
        End Property

        Public Property _Subject() As String
            Get
                Return mSubj
            End Get
            Set(ByVal value As String)
                mSubj = value
            End Set
        End Property

        Public Property _Body() As String
            Get
                Return mBody
            End Get
            Set(ByVal value As String)
                mBody = value
            End Set
        End Property

        Public Property _BodyIsHTML() As Boolean
            Get
                Return mBodyHtml
            End Get
            Set(ByVal value As Boolean)
                mBodyHtml = value
            End Set
        End Property

        Public Property _FileAttachments() As String
            Get
                Return mFileAttachments
            End Get
            Set(ByVal value As String)
                mFileAttachments = value
            End Set
        End Property

        Public Function _Send() As Boolean
            Dim mRet As Boolean = False
            Try
                Dim mmTo() As String = Split(mTo, ";")
                Dim mmCc() As String = Split(mCc, ";")
                Dim mmBCc() As String = Split(mBCc, ";")
                Dim mAttachMents() As String = Split(mFileAttachments, ";")

                Dim mMail As New MailMessage()

                'FROM
                Dim mAdd As New MailAddress(mFrom)
                mMail.From = mAdd
                mAdd = Nothing

                'TO
                For cnt As Integer = 1 To mmTo.Length
                    If mmTo(cnt - 1).Trim <> "" Then
                        mAdd = New MailAddress(mmTo(cnt - 1))
                        mMail.To.Add(mAdd)
                        mAdd = Nothing
                    End If
                Next

                'CC
                For cnt As Integer = 1 To mmCc.Length
                    If mmCc(cnt - 1).Trim <> "" Then
                        mAdd = New MailAddress(mmCc(cnt - 1))
                        mMail.CC.Add(mAdd)
                        mAdd = Nothing
                    End If
                Next

                'BCC
                For cnt As Integer = 1 To mmBCc.Length
                    If mmBCc(cnt - 1).Trim <> "" Then
                        mAdd = New MailAddress(mmBCc(cnt - 1))
                        mMail.Bcc.Add(mAdd)
                        mAdd = Nothing
                    End If
                Next

                Dim mAttachment As Attachment
                For cnt As Integer = 1 To mAttachMents.Length
                    If mAttachMents(cnt - 1).Trim <> "" Then
                        mAttachment = New Attachment(mAttachMents(cnt - 1))
                        mMail.Attachments.Add(mAttachment)
                        mAttachment = Nothing
                    End If
                Next

                mMail.Subject = mSubj
                mMail.Body = mBody
                mMail.IsBodyHtml = mBodyHtml

                Dim mClient As New SmtpClient
                mClient.Host = mSMTPserver
                mClient.Port = 25
                mClient.UseDefaultCredentials = True
                mClient.Send(mMail)
                mMail.Dispose()
                mMail = Nothing
                mClient = Nothing


                mRet = True
            Catch ex As FormatException
                mRet = False
                Throw ex
            Catch ex As SmtpException
                mRet = False
                Throw ex
            End Try
            Return mRet
        End Function
    End Class

End Namespace
