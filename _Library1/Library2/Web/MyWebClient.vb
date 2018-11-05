Namespace Library2.Web

    Public Class MyWebClient
        Inherits System.Net.WebClient

        Private _TimeoutMS As Integer = 0

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal TimeoutMS As Integer, UserAgent As String)
            MyBase.New()
            _TimeoutMS = TimeoutMS
            _UserAgent = UserAgent
        End Sub

        ''' <summary>
        ''' Set the web call timeout in Milliseconds
        ''' </summary>
        ''' <value></value>
        Public WriteOnly Property setTimeout() As Integer
            Set(ByVal value As Integer)
                _TimeoutMS = value
            End Set
        End Property

        Private _UserAgent As String
        Public Property setUserAgent() As String
            Get
                Return _UserAgent
            End Get
            Set(ByVal value As String)
                _UserAgent = value
            End Set
        End Property

        Protected Overrides Function GetWebRequest(ByVal address As System.Uri) As System.Net.WebRequest
            Dim w As System.Net.HttpWebRequest = MyBase.GetWebRequest(address)
            If _TimeoutMS <> 0 Then
                w.Timeout = _TimeoutMS
                w.UserAgent = _UserAgent ' "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)"
                w.MaximumAutomaticRedirections = 300
            End If
            Return MyBase.GetWebRequest(address)
        End Function

    End Class

End Namespace