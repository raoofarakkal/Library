'''Use Microsoft Office Document Imaging 2003 Object Model
'''http://msdn.microsoft.com/en-us/library/aa167607(office.11).aspx 

'Namespace Library2.Console.MsModi

'    Public Class OCR

'        Public Function Process(ByVal pFilePath As String) As OcrText
'            Dim mRet As OcrText = Nothing
'            Dim mDoc As MODI.Document
'            Dim mImage As MODI.Image
'            Try
'                mDoc = New MODI.Document()
'                Try
'                    mDoc.Create(pFilePath)
'                Catch ex As Exception
'                    Throw New Exception(String.Format("File Access Error. Check the file {0}", pFilePath))
'                End Try
'                mDoc.OCR(MODI.MiLANGUAGES.miLANG_SYSDEFAULT, True, True)
'                mImage = DirectCast(mDoc.Images(0), MODI.Image)
'                mRet = New OcrText(mImage.Layout)
'            Catch ex As Exception
'                Throw New Exception(ex.Message & ". " & System.Environment.StackTrace.ToString, ex.InnerException)
'            Finally
'                mDoc = Nothing
'                mImage = Nothing
'            End Try
'            Return mRet
'        End Function

'    End Class

'    Public Class OcrText

'#Region ""
'        Private Function IgnoredWords() As List(Of String)
'            Dim _IgnoredWords As New List(Of String)
'            _IgnoredWords.Add("about")
'            _IgnoredWords.Add("1")
'            _IgnoredWords.Add("after")
'            _IgnoredWords.Add("2")
'            _IgnoredWords.Add("all")
'            _IgnoredWords.Add("also")
'            _IgnoredWords.Add("3")
'            _IgnoredWords.Add("an")
'            _IgnoredWords.Add("4")
'            _IgnoredWords.Add("and")
'            _IgnoredWords.Add("5")
'            _IgnoredWords.Add("another")
'            _IgnoredWords.Add("6")
'            _IgnoredWords.Add("any")
'            _IgnoredWords.Add("7")
'            _IgnoredWords.Add("are")
'            _IgnoredWords.Add("8")
'            _IgnoredWords.Add("as")
'            _IgnoredWords.Add("9")
'            _IgnoredWords.Add("at")
'            _IgnoredWords.Add("0")
'            _IgnoredWords.Add("be")
'            _IgnoredWords.Add("$")
'            _IgnoredWords.Add("because")
'            _IgnoredWords.Add("been")
'            _IgnoredWords.Add("before")
'            _IgnoredWords.Add("being")
'            _IgnoredWords.Add("between")
'            _IgnoredWords.Add("both")
'            _IgnoredWords.Add("but")
'            _IgnoredWords.Add("by")
'            _IgnoredWords.Add("came")
'            _IgnoredWords.Add("can")
'            _IgnoredWords.Add("come")
'            _IgnoredWords.Add("could")
'            _IgnoredWords.Add("did")
'            _IgnoredWords.Add("do")
'            _IgnoredWords.Add("does")
'            _IgnoredWords.Add("each")
'            _IgnoredWords.Add("else")
'            _IgnoredWords.Add("for")
'            _IgnoredWords.Add("from")
'            _IgnoredWords.Add("get")
'            _IgnoredWords.Add("got")
'            _IgnoredWords.Add("has")
'            _IgnoredWords.Add("had")
'            _IgnoredWords.Add("he")
'            _IgnoredWords.Add("have")
'            _IgnoredWords.Add("her")
'            _IgnoredWords.Add("here")
'            _IgnoredWords.Add("him")
'            _IgnoredWords.Add("himself")
'            _IgnoredWords.Add("his")
'            _IgnoredWords.Add("how")
'            _IgnoredWords.Add("if")
'            _IgnoredWords.Add("in")
'            _IgnoredWords.Add("into")
'            _IgnoredWords.Add("is")
'            _IgnoredWords.Add("it")
'            _IgnoredWords.Add("its")
'            _IgnoredWords.Add("just")
'            _IgnoredWords.Add("like")
'            _IgnoredWords.Add("make")
'            _IgnoredWords.Add("many")
'            _IgnoredWords.Add("me")
'            _IgnoredWords.Add("might")
'            _IgnoredWords.Add("more")
'            _IgnoredWords.Add("most")
'            _IgnoredWords.Add("much")
'            _IgnoredWords.Add("must")
'            _IgnoredWords.Add("my")
'            _IgnoredWords.Add("never")
'            _IgnoredWords.Add("now")
'            _IgnoredWords.Add("of")
'            _IgnoredWords.Add("on")
'            _IgnoredWords.Add("only")
'            _IgnoredWords.Add("or")
'            _IgnoredWords.Add("other")
'            _IgnoredWords.Add("our")
'            _IgnoredWords.Add("out")
'            _IgnoredWords.Add("over")
'            _IgnoredWords.Add("re")
'            _IgnoredWords.Add("said")
'            _IgnoredWords.Add("same")
'            _IgnoredWords.Add("see")
'            _IgnoredWords.Add("should")
'            _IgnoredWords.Add("since")
'            _IgnoredWords.Add("so")
'            _IgnoredWords.Add("some")
'            _IgnoredWords.Add("still")
'            _IgnoredWords.Add("such")
'            _IgnoredWords.Add("take")
'            _IgnoredWords.Add("than")
'            _IgnoredWords.Add("that")
'            _IgnoredWords.Add("the")
'            _IgnoredWords.Add("their")
'            _IgnoredWords.Add("them")
'            _IgnoredWords.Add("then")
'            _IgnoredWords.Add("there")
'            _IgnoredWords.Add("these")
'            _IgnoredWords.Add("they")
'            _IgnoredWords.Add("this")
'            _IgnoredWords.Add("those")
'            _IgnoredWords.Add("through")
'            _IgnoredWords.Add("to")
'            _IgnoredWords.Add("too")
'            _IgnoredWords.Add("under")
'            _IgnoredWords.Add("up")
'            _IgnoredWords.Add("use")
'            _IgnoredWords.Add("very")
'            _IgnoredWords.Add("want")
'            _IgnoredWords.Add("was")
'            _IgnoredWords.Add("way")
'            _IgnoredWords.Add("we")
'            _IgnoredWords.Add("well")
'            _IgnoredWords.Add("were")
'            _IgnoredWords.Add("what")
'            _IgnoredWords.Add("when")
'            _IgnoredWords.Add("where")
'            _IgnoredWords.Add("which")
'            _IgnoredWords.Add("while")
'            _IgnoredWords.Add("who")
'            _IgnoredWords.Add("will")
'            _IgnoredWords.Add("with")
'            _IgnoredWords.Add("would")
'            _IgnoredWords.Add("you")
'            _IgnoredWords.Add("your")
'            For i As Integer = Asc("a") To Asc("z")
'                _IgnoredWords.Add(Chr(i))
'            Next
'            Return _IgnoredWords
'        End Function
'#End Region

'        Public Sub New(ByVal pValue As MODI.Layout)
'            mLang = pValue.Language
'            mText = pValue.Text
'            Try
'                mWords = pValue.Text.Replace(vbCrLf, " ").Split(" ")
'                'mSearchKeywords = mWords
'                'For i As Integer = mWords.Count - 1 To 0 Step -1
'                '    If mWords(i) Then
'                'Next
'            Catch ex As Exception
'            End Try
'        End Sub

'        Private mText As String
'        Public ReadOnly Property Text() As String
'            Get
'                Return mText
'            End Get
'        End Property

'        Private mLang As Integer
'        Public ReadOnly Property Language() As Integer
'            Get
'                Return mLang
'            End Get
'        End Property

'        Private mWords As String()
'        Public ReadOnly Property Words() As String()
'            Get
'                Return mWords
'            End Get
'        End Property

'        'Private mSearchKeywords As String()
'        'Public ReadOnly Property SearchKeyWords() As String()
'        '    Get
'        '        Return mSearchKeywords
'        '    End Get

'        'End Property

'    End Class
'End Namespace

