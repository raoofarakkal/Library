
Namespace _Library._System._Security

    Public Class clsEncrypt
        Inherits _Base.LibraryBase
        Public Function DeCode(ByVal mData As String) As String
            Dim Switch As Boolean
            Dim Cnt, Max As Integer
            Dim Data2 As String
            Data2 = ""
            Switch = True
            Max = Len(mData)
            For Cnt = 1 To Max Step 3
                If Switch = True Then
                    If Mid(mData, Cnt, 3) <= 255 Then
                        Data2 = Data2 & Chr(Mid(mData, Cnt, 3) - 1)
                    End If
                    Switch = False
                Else
                    Switch = True
                End If
            Next
            Return Data2
        End Function

        Public Function CodeIt(ByVal mData As String) As String
            Dim Cnt, Max As Integer
            Dim Data As String = ""
            Max = Len(mData)
            For Cnt = 1 To Max
                Data = Data + lZeroFil(Asc(Mid(mData, Cnt, 1)) + 1, 3)
                Data = Data + lZeroFil(Int(255 * Rnd()), 3)
            Next
            Return Data
        End Function

        Private Function lZeroFil(ByVal Txt As String, ByVal Width As Integer)
            Try
                Dim Cnt As Object
                Dim Zeros As String
                Zeros = ""
                If Width > Len(Txt) Then
                    For Cnt = Len(Txt) + 1 To Width
                        Zeros = Zeros + "0"
                    Next
                End If
                Return Zeros & Txt

                Exit Function
            Catch ex As Exception
                MsgBox(Err.Description, vbOKOnly + vbCritical)
                Return ""
            End Try
        End Function
    End Class

End Namespace

