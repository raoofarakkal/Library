Imports System.Net
Imports System.IO

Namespace _Library._Web

    Public Class YouTubeDwn

        Private mProxyInfo As _Library.proxy.ProxyInfo

        Public Sub New()
            mProxyInfo = Nothing
        End Sub

        Public Sub New(ByVal pProxyInfo As _Library.proxy.ProxyInfo)
            mProxyInfo = pProxyInfo
        End Sub

        Protected Overrides Sub Finalize()
            mProxyInfo = Nothing
            MyBase.Finalize()
        End Sub

        Public Shared Function GetHTTPcontents(ByVal requestURL As String) As String
            Try
                Dim request As WebRequest = WebRequest.Create(requestURL)
                'request.Credentials = CredentialCache.DefaultCredentials
                Dim wResponse As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                'Console.WriteLine(response.StatusDescription)
                Dim dataStream As Stream = wResponse.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim PostingData As String = reader.ReadToEnd()
                reader.Close()
                dataStream.Close()
                Return PostingData
            Catch ex As Exception
                Return ""
            End Try

        End Function

        Public Function DownloadVideo(ByVal VideoID As String, ByVal FolderLocation As String) As Boolean
            Dim mRet As Boolean = False
            Dim mUrl As String = Nothing
            Dim mDw As New Net.WebClient
            Try
                mUrl = GetDownloadVideoURL(String.Format("http://youtube.com/watch?v={0}", VideoID))
                If mProxyInfo IsNot Nothing Then
                    mDw.Proxy = mProxyInfo.Proxy ' New WebProxy(mdlConfig.ProxyIP, mdlConfig.ProxyPort)
                    'mDw.Proxy.Credentials = New NetworkCredential(mdlConfig.UID, mdlConfig.PWD, mdlConfig.DC)
                End If
                Dim mSize As Long = _GetDownloadVideoSize(mUrl)
                mDw.DownloadFile(mUrl, String.Format("{0}\{1}.flv", FolderLocation, VideoID))
                If My.Computer.FileSystem.GetFileInfo(String.Format("{0}\{1}.flv", FolderLocation, VideoID)).Length = mSize Then
                    mRet = True
                Else
                    Throw New Exception(String.Format("Failed to download {0}\{1}.flv", FolderLocation, VideoID))
                End If
            Catch ex As Exception
                Throw ex
            Finally
                mDw.Dispose()
                mDw = Nothing
            End Try
            Return mRet
        End Function

        Public Function GetDownloadVideoSize(ByVal VideoID As String) As Long
            Return _GetDownloadVideoSize(GetDownloadVideoURL(String.Format("http://youtube.com/watch?v={0}", VideoID)))
        End Function

        Private Function GetDownloadVideoURL(ByVal url As String) As String
            Dim mRet As String = ""
            Dim mTag As String = """fmt_url_map"": """
            Dim mStr As String
            Dim mPos As Integer
            Try
                mStr = GetHTTPcontents(url) 'getContent(url)
                mPos = InStr(mStr, mTag)
                If mPos > 0 Then
                    mStr = Mid(mStr, mPos).Replace(mTag, "")
                    mStr = mStr.Split("""")(0)

                    mStr = System.Web.HttpUtility.UrlDecode(mStr)
                    mRet = ""
                    For Each mU As String In mStr.Split("|")
                        If Left(mU.Trim, 7).ToLower = "http://" Then
                            If InStr(mU, ",") = 0 Then
                                mRet = mU
                                Exit For
                            End If
                        End If
                    Next
                    If mRet = "" Then
                        Throw New Exception(String.Format("_Library._Web.YoutubeDwn.GetDownloadVideoURL(): Couldn't find a URL not contain comma, seems like Youtube change their fmt url. {0} ", mStr))
                    End If
                    'mStr = mStr.Split("|")(2)
                    If InStr(mRet, "http") = 0 Then
                        Throw New Exception(String.Format("_Library._Web.YoutubeDwn.GetDownloadVideoURL(): URL not contain HTTP. {0} ", mRet))
                    End If
                    Return mRet 'mStr '.Split(",")(0)
                Else
                    Throw New Exception(mStr)
                End If
            Catch ex As Exception
                Throw New Exception(String.Format("_Library._Web.YoutubeDwn.GetDownloadVideoURL(): {0} ", ex.Message))
            End Try
        End Function

        Private Function _GetDownloadVideoSize(ByVal pURL As String) As Long
            Dim mRet As Long = 0
            Dim mRequest As WebRequest
            Try
                mRequest = GetRequest(pURL)
                Using mResponse As HttpWebResponse = DirectCast(mRequest.GetResponse(), HttpWebResponse)
                    mRet = mResponse.ContentLength
                End Using
            Catch ex As Exception
                Throw ex
            Finally
                mRequest = Nothing
            End Try
            Return mRet
        End Function

        Private Function GetRequest(ByVal pURL As String) As WebRequest
            Dim mRet As WebRequest = WebRequest.Create(pURL)
            mRet.Timeout = 30000
            If mProxyInfo IsNot Nothing Then
                mRet.Proxy = mProxyInfo.Proxy 'New WebProxy(mdlConfig.ProxyIP, mdlConfig.ProxyPort)
                'mRet.Proxy.Credentials = New NetworkCredential(mdlConfig.UID, mdlConfig.PWD, mdlConfig.DC)
            End If
            Return mRet
        End Function

        'Private Function getContent(ByVal url As String) As String
        '    Dim mRet As String
        '    Dim mRequest As WebRequest
        '    Dim mWRequest As HttpWebRequest
        '    Dim mOpBuffer As String
        '    Dim mSwOut As StreamWriter
        '    Dim mWResponse As HttpWebResponse
        '    Dim mSRead As StreamReader
        '    Try

        '        mRequest = WebRequest.Create(url)
        '        mRequest.Timeout = 120 * 1000
        '        If mProxyInfo IsNot Nothing Then
        '            mRequest.Proxy = mProxyInfo.Proxy ' New WebProxy(mdlConfig.ProxyIP, mdlConfig.ProxyPort)
        '            'mRequest.Proxy.Credentials = New NetworkCredential(mdlConfig.UID, mdlConfig.PWD, mdlConfig.DC)
        '        End If

        '        mOpBuffer = "where=46038"
        '        'Dim req As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        '        mWRequest = DirectCast(mRequest, HttpWebRequest)
        '        mWRequest.Method = "POST"
        '        mWRequest.ContentLength = mOpBuffer.Length
        '        mWRequest.ContentType = "application/x-www-form-urlencoded"
        '        mSwOut = New StreamWriter(mWRequest.GetRequestStream())
        '        Try
        '            mSwOut.Write(mOpBuffer)
        '            mSwOut.Close()
        '        Catch ex As Exception
        '            Throw ex
        '        Finally
        '            mSwOut.Dispose()
        '        End Try
        '        mWResponse = DirectCast(mWRequest.GetResponse(), HttpWebResponse)
        '        mSRead = New StreamReader(mWResponse.GetResponseStream())
        '        Try
        '            mRet = mSRead.ReadToEnd()
        '            mSRead.Close()
        '        Catch ex As Exception
        '            Throw ex
        '        Finally
        '            mSRead.Dispose()
        '        End Try
        '    Catch exp As Exception
        '        mRet = "Error: " & exp.Message.ToString()
        '    Finally
        '        mRequest = Nothing
        '        mWRequest = Nothing
        '        mOpBuffer = Nothing
        '        mSwOut = Nothing
        '        mWResponse = Nothing
        '        mSRead = Nothing
        '    End Try
        '    Return mRet
        'End Function

    End Class
End Namespace