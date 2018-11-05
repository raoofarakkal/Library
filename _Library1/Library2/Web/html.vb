Namespace Library2.Web

    Public Class html

        Public Class HtmlDoc
            Private mDoc As String

            Public Sub New(ByVal pHtml As String)
                mDoc = pHtml
            End Sub

            Protected Overrides Sub Finalize()
                mDoc = Nothing
                MyBase.Finalize()
            End Sub

            Public Function GetElementsByTagName(ByVal pTagName As String) As List(Of HtmlElmnt)
                Dim mRet As New List(Of HtmlElmnt)
                Dim mSingleItem As String = ""
                Dim mSearch As String = mDoc
                Dim mStPos As Integer = 0
                Dim mEndPos As Integer = 0
                Dim mStTag As String = String.Format("<{0}", pTagName)
                Dim mEndTag As String = String.Format("</{0}>", pTagName)
                Dim mRun As Boolean = True
                Do While mRun
                    mStPos = 0
                    mEndPos = 0
                    mSingleItem = ""
                    mStPos = InStr(mSearch, mStTag, CompareMethod.Text)
                    If mStPos = 0 Then
                        Exit Do
                    Else
                        mSearch = Mid(mSearch, mStPos)
                        If Mid(mSearch, Len(mStTag) + 1, 1) = ">" Or Mid(mSearch, Len(mStTag) + 1, 1) = " " Then
                            mEndPos = InStr(mSearch, mEndTag, CompareMethod.Text)
                            If mEndPos = 0 Then
                                mEndPos = InStr(mSearch, "/>", CompareMethod.Text)
                            End If
                            If mEndPos = 0 Then
                                mEndPos = InStr(mSearch, ">", CompareMethod.Text)
                            End If
                            If mEndPos > 0 Then
                                mSingleItem = Mid(mSearch, 1, mEndPos + Len(mEndTag))
                                mRet.Add(New HtmlElmnt(mSingleItem))
                                mSearch = Mid(mSearch, mEndPos + Len(mEndTag))
                            Else
                                Exit Do
                            End If
                        Else
                            mSearch = Mid(mSearch, Len(mStTag) + 1)
                        End If
                    End If
                Loop
                Return mRet
            End Function

            Public Function GetElementsByID(ByVal pTagName As String, ByVal ID As String, ByVal CaseSensitive As Boolean) As List(Of HtmlElmnt)
                Dim mRet As New List(Of HtmlElmnt)
                For Each mT As HtmlElmnt In GetElementsByTagName(pTagName)
                    If CaseSensitive Then
                        If mT.ID.Trim = ID.Trim Then
                            mRet.Add(mT)
                        End If
                    Else
                        If mT.ID.ToLower.Trim = ID.ToLower.Trim Then
                            mRet.Add(mT)
                        End If
                    End If
                Next
                Return mRet
            End Function

            Public Function GetElementsByID(ByVal pTagName As String, ByVal ID As String) As List(Of HtmlElmnt)
                Return GetElementsByID(pTagName, ID, False)
            End Function

            Public Shared Function GetHTTPcontents(ByVal requestURL As String, ByVal pProxyInfo As _Library.proxy.ProxyInfo, Optional ByVal HttpReferer As String = "", Optional ByVal TimeOut As Long = 30000) As String
                Dim mWReq As Net.WebRequest
                Dim mWResp As Net.HttpWebResponse
                Dim mDStrm As System.IO.Stream
                Dim mReader As System.IO.StreamReader
                Dim PostingData As String
                Try
                    mWReq = Net.WebRequest.Create(requestURL)
                    CType(mWReq, Net.HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)"
                    If HttpReferer <> "" Then
                        CType(mWReq, Net.HttpWebRequest).Referer = HttpReferer
                    End If
                    'mWReq.ContentType = "application/x-www-form-urlencoded" '"text/html; charset=UTF-8"
                    'mWReq.Method = "GET"
                    mWReq.Timeout = TimeOut
                    If pProxyInfo IsNot Nothing Then
                        mWReq.Proxy = pProxyInfo.Proxy
                    End If
                    mWResp = CType(mWReq.GetResponse(), Net.HttpWebResponse)
                    mDStrm = mWResp.GetResponseStream()
                    mReader = New System.IO.StreamReader(mDStrm)
                    PostingData = mReader.ReadToEnd()
                    mReader.Close()
                    mDStrm.Close()
                    mReader.Dispose()
                    mDStrm.Dispose()
                    Return PostingData
                Catch ex As Exception
                    Return ex.Message
                Finally
                    PostingData = Nothing
                    mReader = Nothing
                    mDStrm = Nothing
                    mWResp = Nothing
                    mWReq = Nothing

                End Try

            End Function

        End Class

        Public Class HtmlElmnt
            Inherits System.Web.UI.HtmlControls.HtmlGenericControl
            Dim mElem As String

            Public Sub New(ByVal pElement As String)
                mElem = pElement
                Parse()
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
            End Sub

            Overrides Property Innertext() As String
                Get
                    Return _Library._Web._Common.RemoveHtmlTags(InnerHtml)
                End Get
                Set(ByVal value As String)
                    MyBase.InnerText = value
                End Set
            End Property

            Private Sub Parse()
                ParseAttribs()
                parseInnerHtml()
            End Sub

            Private Sub ParseAttribs()
                Dim mSearch As String = mElem
                Dim mStPos As Integer = 0
                Dim mEndPos As Integer = 0
                Dim mFirstTag As Boolean = True
                Try
                    mStPos = InStr(mSearch, "<")
                    If mStPos > 0 Then
                        mSearch = Mid(mSearch, mStPos + 1)
                        mEndPos = InStr(mSearch, ">")
                        If mEndPos > 0 Then
                            mSearch = Mid(mSearch, 1, mEndPos - 1)
                            For Each mS As String In mSearch.Split(" ")
                                If mFirstTag Then
                                    TagName = mS
                                    mFirstTag = False
                                Else
                                    If InStr(mS, "=") > 0 Then
                                        If mS.Split("=")(0).ToLower = "id" Then
                                            ID = mS.Split("=")(1)
                                        End If
                                        Attributes.Add(mS.Split("=")(0), mS.Split("=")(1))
                                    End If
                                End If
                            Next
                        Else
                            Throw New Exception("> tag missing")
                        End If
                    Else
                        Throw New Exception("< tag missing")
                    End If
                Catch ex As Exception
                    Throw New Exception("not in HTML format. " & ex.Message)
                Finally
                End Try
            End Sub

            Private Sub parseInnerHtml()
                Dim mSearch As String = mElem
                Dim mStPos As Integer = 0
                Dim mEndPos As Integer = 0
                mStPos = InStr(mSearch, ">")
                If mStPos > 0 Then
                    mSearch = Mid(mSearch, mStPos + 1)
                    mEndPos = InStr(mSearch, "</" & TagName & ">")
                    If mEndPos > 0 Then
                        InnerHtml = Mid(mSearch, 1, mEndPos - 1)
                    End If
                End If
            End Sub

        End Class

    End Class
End Namespace
