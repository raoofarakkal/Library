Imports System.Text
Imports System.Web
Imports System.Collections.Generic

Namespace _Library._Web
    Public Class RssFeed

        Public Class RssFeedWriter

            Public Enum _Direction
                ltr = 1
                rtl = 2
            End Enum

            Private mDir As _Direction = _Direction.ltr
            Public Property Direction() As _Direction
                Get
                    Return mDir
                End Get
                Set(ByVal value As _Direction)
                    mDir = value
                End Set
            End Property

            Private mTitle As String
            Public Property Title() As String
                Get
                    Return mTitle
                End Get
                Set(ByVal value As String)
                    mTitle = value
                End Set
            End Property

            Private mDesc As String
            Public Property Description() As String
                Get
                    Return mDesc
                End Get
                Set(ByVal value As String)
                    mDesc = value
                End Set
            End Property

            Private mLink As String
            Public Property Link() As String
                Get
                    Return mLink
                End Get
                Set(ByVal value As String)
                    mLink = value
                End Set
            End Property

            Private mImg As New RssImage
            Public ReadOnly Property RssImg() As RssImage
                Get
                    Return mImg
                End Get
            End Property

            Private mCopyright As String
            Public Property Copyright() As String
                Get
                    Return mCopyright
                End Get
                Set(ByVal value As String)
                    mCopyright = value
                End Set
            End Property

            Private mDocs As String
            Public Property Docs() As String
                Get
                    Return mDocs
                End Get
                Set(ByVal value As String)
                    mDocs = value
                End Set
            End Property

            'Private mManagingEditor As String
            'Public Property ManagingEditor() As String
            '    Get
            '        Return mManagingEditor
            '    End Get
            '    Set(ByVal value As String)
            '        mManagingEditor = value
            '    End Set
            'End Property

            'Private mWebMaster As String
            'Public Property WebMaster() As String
            '    Get
            '        Return mWebMaster
            '    End Get
            '    Set(ByVal value As String)
            '        mWebMaster = value
            '    End Set
            'End Property

            'Private mGenerator As String
            'Public Property Generator() As String
            '    Get
            '        Return mGenerator
            '    End Get
            '    Set(ByVal value As String)
            '        mGenerator = value
            '    End Set
            'End Property

            Dim mItems As New RssItems
            Public ReadOnly Property Items() As RssItems
                Get
                    Return mItems
                End Get
            End Property

            Private mEncoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Public Property Encoding() As System.Text.Encoding
                Get
                    Return mEncoding
                End Get
                Set(ByVal value As System.Text.Encoding)
                    mEncoding = value
                End Set
            End Property

            Private mXmlnsMediaUrl As String = ""
            Public Property XmlnsMediaUrl() As String
                Get
                    Return mXmlnsMediaUrl
                End Get
                Set(ByVal value As String)
                    mXmlnsMediaUrl = value
                End Set
            End Property

            Public Function ToXml() As String
                Dim mRet As New StringBuilder
                Dim mEnc As String = ""
                Dim mXmlnsMediaString As String = ""
                If Encoding IsNot Nothing Then
                    mEnc = String.Format(" encoding=""{0}"" ", Encoding.WebName)
                End If
                If XmlnsMediaUrl IsNot Nothing Then
                    mXmlnsMediaString = String.Format(" xmlns:media=""{0}"" ", XmlnsMediaUrl)
                End If
                mRet.AppendLine(String.Format("<?xml version=""1.0"" {0} ?>", mEnc))
                mRet.AppendLine(String.Format("<rss version=""2.0"" {0}>", mXmlnsMediaString))
                mRet.AppendLine("<channel>")
                mRet.AppendLine(String.Format("<title>{0}</title>", Title))
                mRet.AppendLine(String.Format("<description>{0}</description>", Description))
                If Direction = _Direction.rtl Then
                    mRet.AppendLine(String.Format("<language>ar</language>"))
                End If
                mRet.AppendLine(String.Format("<link>{0}</link>", Link))
                mRet.AppendLine(mImg.ToXml)
                mRet.AppendLine(String.Format("<copyright>{0}</copyright>", Copyright))
                mRet.AppendLine(String.Format("<docs>{0}</docs>", Docs))
                'mRet.AppendLine(String.Format("<managingEditor>{0}</managingEditor>", ManagingEditor))
                'mRet.AppendLine(String.Format("<webMaster>{0}</webMaster>", WebMaster))
                'mRet.AppendLine(String.Format("<generator>{0}</generator>", Generator))
                For Each mI As RssItem In mItems
                    mRet.AppendLine(mI.ToXml)
                Next
                mRet.AppendLine("</channel>")
                mRet.AppendLine("</rss>")
                Return mRet.ToString
            End Function

            Public Function Render(ByVal pContext As HttpContext) As Boolean
                pContext.Response.ContentType = "text/xml"
                pContext.Response.ContentEncoding = Encoding
                pContext.Response.Write(ToXml)
            End Function

            Public Class RssImage

                Private mTitle As String
                Public Property Title() As String
                    Get
                        Return mTitle
                    End Get
                    Set(ByVal value As String)
                        mTitle = value
                    End Set
                End Property

                Private mUrl As String
                Public Property Url() As String
                    Get
                        Return mUrl
                    End Get
                    Set(ByVal value As String)
                        mUrl = value
                    End Set
                End Property

                Private mLink As String
                Public Property Link() As String
                    Get
                        Return mLink
                    End Get
                    Set(ByVal value As String)
                        mLink = value
                    End Set
                End Property

                Public Function ToXml() As String
                    Dim mRet As New StringBuilder
                    mRet.AppendLine("<image>")
                    mRet.AppendLine(String.Format("<title>{0}</title>", Title))
                    mRet.AppendLine(String.Format("<url>{0}</url>", Url))
                    mRet.AppendLine(String.Format("<link>{0}</link>", Link))
                    mRet.AppendLine("</image>")
                    Return mRet.ToString
                End Function

            End Class

            <Runtime.InteropServices.ClassInterface(Runtime.InteropServices.ClassInterfaceType.None)> _
            Public Class RssItems
                Inherits List(Of RssItem)
            End Class

            Public Class RssItem

                Private mTitle As String
                Public Property Title() As String
                    Get
                        Return mTitle
                    End Get
                    Set(ByVal value As String)
                        mTitle = value
                    End Set
                End Property

                Private mDesc As String
                Public Property Description() As String
                    Get
                        Return mDesc
                    End Get
                    Set(ByVal value As String)
                        mDesc = value
                    End Set
                End Property

                Private mLink As String
                Public Property Link() As String
                    Get
                        Return mLink
                    End Get
                    Set(ByVal value As String)
                        mLink = value
                    End Set
                End Property

                Private mGUID As String
                Public Property GUID() As String
                    Get
                        Return mGUID
                    End Get
                    Set(ByVal value As String)
                        mGUID = value
                    End Set
                End Property

                Private mPubDate As Date
                Public Property PubDate() As Date
                    Get
                        Return mPubDate
                    End Get
                    Set(ByVal value As Date)
                        mPubDate = value
                    End Set
                End Property

                Private mCategory As String
                Public Property Category() As String
                    Get
                        Return mCategory
                    End Get
                    Set(ByVal value As String)
                        mCategory = value
                    End Set
                End Property

                Private mCustomTags As RssItemCustomTags
                Public ReadOnly Property CustomTags() As RssItemCustomTags
                    Get
                        If mCustomTags Is Nothing Then
                            mCustomTags = New RssItemCustomTags
                        End If
                        Return mCustomTags
                    End Get
                End Property

                Public Function ToXml() As String
                    Dim mRet As New StringBuilder
                    mRet.AppendLine("<item>")
                    mRet.AppendLine(String.Format("<title>{0}</title>", Title))
                    mRet.AppendLine(String.Format("<description>{0}</description>", Description))
                    mRet.AppendLine(String.Format("<link>{0}</link>", Link))
                    mRet.AppendLine(String.Format("<guid isPermaLink=""False"">{0}</guid>", GUID))
                    mRet.AppendLine(String.Format("<pubDate>{0}</pubDate>", PubDate.ToString("r"))) '' "Sun, 21 Dec 2008 05:40:16 GMT"
                    mRet.AppendLine(String.Format("<category>{0}</category>", Category))
                    mRet.AppendLine(CustomTags.ToXml)
                    mRet.AppendLine("</item>")
                    Return mRet.ToString
                End Function

                <Runtime.InteropServices.ClassInterface(Runtime.InteropServices.ClassInterfaceType.None)> _
                Public Class RssItemCustomTags
                    Inherits List(Of RssItemCustomTag)

                    Public Function ToXml() As String
                        Dim mRet As New StringBuilder
                        For Each mT As RssItemCustomTag In Me
                            If mT IsNot Nothing Then
                                mRet.Append(mT.ToXml)
                            End If
                        Next
                        Return mRet.ToString
                    End Function
                End Class

                Public Class RssItemCustomTag

                    Public Sub New()

                    End Sub

                    Public Sub New(ByVal pTagName As String, ByVal pText As String)
                        TagName = pTagName
                        Text = pText
                    End Sub

                    Private mTagName As String
                    Public Property TagName() As String
                        Get
                            Return mTagName
                        End Get
                        Set(ByVal value As String)
                            mTagName = value
                        End Set
                    End Property

                    Private mValue As String
                    Public Property Text() As String
                        Get
                            Return mValue
                        End Get
                        Set(ByVal value As String)
                            mValue = value
                        End Set
                    End Property

                    Private mAttribs As RssItemCustomTagAttributes
                    Public ReadOnly Property Attributes() As RssItemCustomTagAttributes
                        Get
                            If mAttribs Is Nothing Then
                                mAttribs = New RssItemCustomTagAttributes
                            End If
                            Return mAttribs
                        End Get
                    End Property

                    Public Function ToXml() As String
                        Dim mRet As New StringBuilder
                        mRet.AppendLine(String.Format("<{0} {1} >{2}</{0}>", TagName, Attributes.ToXml, Text))
                        Return mRet.ToString
                    End Function

                    <Runtime.InteropServices.ClassInterface(Runtime.InteropServices.ClassInterfaceType.None)> _
                    Public Class RssItemCustomTagAttributes
                        Inherits List(Of RssItemCustomTagAttribute)

                        Public Function ToXml() As String
                            Dim mRet As New StringBuilder
                            For Each mA As RssItemCustomTagAttribute In Me
                                If mA IsNot Nothing Then
                                    mRet.Append(mA.ToXml)
                                End If
                            Next
                            Return mRet.ToString
                        End Function

                    End Class

                    Public Class RssItemCustomTagAttribute

                        Public Sub New()

                        End Sub

                        Public Sub New(ByVal pAttributeName As String, ByVal pAttributeValue As String)
                            AttributeName = pAttributeName
                            AttributeValue = pAttributeValue
                        End Sub

                        Private mAttrib As String
                        Public Property AttributeName() As String
                            Get
                                Return mAttrib
                            End Get
                            Set(ByVal value As String)
                                mAttrib = value
                            End Set
                        End Property

                        Private mValue As String
                        Public Property AttributeValue() As String
                            Get
                                Return mValue
                            End Get
                            Set(ByVal value As String)
                                mValue = value
                            End Set
                        End Property

                        Public Function ToXml() As String
                            Dim mRet As New StringBuilder
                            mRet.Append(String.Format(" {0}=""{1}"" ", AttributeName, AttributeValue))
                            Return mRet.ToString
                        End Function

                    End Class

                End Class


            End Class

        End Class

    End Class

End Namespace