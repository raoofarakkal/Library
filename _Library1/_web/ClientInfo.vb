Imports Microsoft.VisualBasic

Namespace _Library._Web

    Public Class ClientInfo
        Public Sub New(ByVal pWebPage As Web.UI.Page)
            mRemoteUser = pWebPage.Request.ServerVariables("REMOTE_USER").ToString
            mRemoteAddress = pWebPage.Request.ServerVariables("REMOTE_ADDR").ToString
            mRemoteHost = pWebPage.Request.ServerVariables("REMOTE_HOST").ToString
            mAllHttp = pWebPage.Request.ServerVariables("ALL_HTTP")
            mHttps = pWebPage.Request.ServerVariables("HTTPS")
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private mRemoteUser As String
        Public ReadOnly Property RemoteUser() As String
            Get
                Return mRemoteUser
            End Get
        End Property

        Private mRemoteAddress As String
        Public ReadOnly Property RemoteAddress() As String
            Get
                Return mRemoteAddress
            End Get
        End Property

        Private mRemoteHost As String
        Public ReadOnly Property RemoteHost() As String
            Get
                Return mRemoteHost
            End Get
        End Property

        Private mAllHttp As Object
        Public ReadOnly Property AllHttp() As Object
            Get
                Return mAllHttp
            End Get
        End Property

        Private mHttps As Object
        Public ReadOnly Property isRequestSecured() As Boolean
            Get
                Dim mRet As Boolean = False
                If mHttps IsNot Nothing Then
                    If mHttps <> "" Then
                        If mHttps.ToString.ToLower = "on" Then
                            mRet = True
                        End If
                    End If
                End If
                Return mRet
            End Get
        End Property

    End Class

End Namespace


'Server Variables
'ALL_HTTP Returns all HTTP headers sent by the client. Always prefixed with HTTP_ and capitalized 
'ALL_RAW Returns all headers in raw form 
'APPL_MD_PATH Returns the meta base path for the application for the ISAPI DLL 
'APPL_PHYSICAL_PATH Returns the physical path corresponding to the meta base path 
'AUTH_PASSWORD Returns the value entered in the client's authentication dialog 
'AUTH_TYPE The authentication method that the server uses to validate users 
'AUTH_USER Returns the raw authenticated user name 
'CERT_COOKIE Returns the unique ID for client certificate as a string 
'CERT_FLAGS bit0 is set to 1 if the client certificate is present and bit1 is set to 1 if the cCertification authority of the client certificate is not valid 
'CERT_ISSUER Returns the issuer field of the client certificate 
'CERT_KEYSIZE Returns the number of bits in Secure Sockets Layer connection key size 
'CERT_SECRETKEYSIZE Returns the number of bits in server certificate private key 
'CERT_SERIALNUMBER Returns the serial number field of the client certificate 
'CERT_SERVER_ISSUER Returns the issuer field of the server certificate 
'CERT_SERVER_SUBJECT Returns the subject field of the server certificate 
'CERT_SUBJECT Returns the subject field of the client certificate 
'CONTENT_LENGTH Returns the length of the content as sent by the client 
'CONTENT_TYPE Returns the data type of the content 
'GATEWAY_INTERFACE Returns the revision of the CGI specification used by the server 
'HTTP_<HeaderName> Returns the value stored in the header HeaderName 
'HTTP_ACCEPT Returns the value of the Accept header 
'HTTP_ACCEPT_LANGUAGE Returns a string describing the language to use for displaying content 
'HTTP_COOKIE Returns the cookie string included with the request 
'HTTP_REFERER Returns a string containing the URL of the page that referred the request to the current page using an <a> tag. If the page is redirected, HTTP_REFERER is empty 
'HTTP_USER_AGENT Returns a string describing the browser that sent the request 
'HTTPS Returns ON if the request came in through secure channel or OFF if the request came in through a non-secure channel 
'HTTPS_KEYSIZE Returns the number of bits in Secure Sockets Layer connection key size 
'HTTPS_SECRETKEYSIZE Returns the number of bits in server certificate private key 
'HTTPS_SERVER_ISSUER Returns the issuer field of the server certificate 
'HTTPS_SERVER_SUBJECT Returns the subject field of the server certificate 
'INSTANCE_ID The ID for the IIS instance in text format 
'INSTANCE_META_PATH The meta base path for the instance of IIS that responds to the request 
'LOCAL_ADDR Returns the server address on which the request came in 
'LOGON_USER Returns the Windows account that the user is logged into 
'PATH_INFO Returns extra path information as given by the client 
'PATH_TRANSLATED A translated version of PATH_INFO that takes the path and performs any necessary virtual-to-physical mapping 
'QUERY_STRING Returns the query information stored in the string following the question mark (?) in the HTTP request 
'REMOTE_ADDR Returns the IP address of the remote host making the request 
'REMOTE_HOST Returns the name of the host making the request 
'REMOTE_USER Returns an unmapped user-name string sent in by the user 
'REQUEST_METHOD Returns the method used to make the request 
'SCRIPT_NAME Returns a virtual path to the script being executed 
'SERVER_NAME Returns the server's host name, DNS alias, or IP address as it would appear in self-referencing URLs 
'SERVER_PORT Returns the port number to which the request was sent 
'SERVER_PORT_SECURE Returns a string that contains 0 or 1. If the request is being handled on the secure port, it will be 1. Otherwise, it will be 0 
'SERVER_PROTOCOL Returns the name and revision of the request information protocol 
'SERVER_SOFTWARE Returns the name and version of the server software that answers the request and runs the gateway 
'URL Returns the base portion of the URL 
