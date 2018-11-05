Namespace Library2.Sys.Security

    Public Class ShortEncodeDecode
        Implements IDisposable

        Public Alphabet As String

        Public Sub New()
            init(False)
        End Sub

        Public Sub New(CaseSensitive As Boolean)
            init(CaseSensitive)
        End Sub

        Private Sub init(CaseSensitive As Boolean)
            If CaseSensitive Then
                Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Else
                Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789" '"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            End If
        End Sub

        Public Function Encode(Number As Int64) As String
            If Number.ToString().Length > 15 Then
                Throw New Exception("Number should not exceed more 15 digits")
            Else
                Dim result As String = [String].Empty
                For i As Integer = CInt(Math.Floor(Math.Log(Number) / Math.Log(Alphabet.Length))) To 0 Step -1
                    result += Alphabet.Substring(CInt(Math.Floor(Number / BcPow(Alphabet.Length, i)) Mod Alphabet.Length), 1)
                Next
                Return ReverseString(result)
            End If
        End Function

        Public Function Decode(Id As String) As Int64
            Dim str As String = ReverseString(Id)
            Dim result As Int64 = 0
            Dim [end] As Integer = str.Length - 1
            For i As Integer = 0 To [end]
                result = result + CType(Alphabet.IndexOf(str.Substring(i, 1)) * BcPow(Alphabet.Length, [end] - i), Int64)
            Next
            Return result
        End Function

        Private Function BcPow(_a As Double, _b As Double) As Double
            Return Math.Floor(Math.Pow(_a, _b))
        End Function

        Private Function ReverseString(s As String) As String
            Dim arr As Char() = s.ToCharArray()
            Array.Reverse(arr)
            Return New String(arr)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace