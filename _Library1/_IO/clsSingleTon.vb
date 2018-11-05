Namespace _Library._IO
    ''' <summary>
    ''' SingleTon object
    ''' in each call the object will not re-initialize untill application restart
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsSingleTon
        Private Shared mObj As DataSet

        Protected Sub New()
        End Sub

        Public Shared Function ReadXML(ByVal path As String) As DataSet
            If mObj Is Nothing Then
                mObj = New DataSet
                mObj.ReadXml(path)
            End If
            Return mObj
        End Function

    End Class
End Namespace
