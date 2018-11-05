Namespace _Library._Base

    Public Class LibraryBase
        Friend Shared _LicenseValid As Boolean = False
        Friend Sub New()
            _LicenseValid = True 'license check disabled 
            If Not _LicenseValid Then
                'Dim mValid As Boolean = False
                'Dim mKey As String = ""
                'Dim mConf As New _Library._Config.LibConfig '  AJCMS.Library.Config
                'Dim mPro As New _Library._System._Security.Protection.Protection ' AJCMS.Protection.AjCmsProtection
                'mKey = mConf.LicenseKey
                'mConf = Nothing
                'mValid = mPro.Valid(mKey)
                'mKey = Nothing
                'mPro = Nothing
                'If Not mValid Then
                '    Throw New Exception("Library license required")
                'Else
                '    _LicenseValid = True
                'End If
            End If
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace