Imports System.DirectoryServices
Imports System.Text

Namespace _Library._System._ActiveDirectory

    Public Class MyAdManager
        Inherits _Base.LibraryBase
        Private _Domain As String
        Private _LDAPpath As String
        Private _AdminLoginID As String
        Private _AdminLoginPWD As String

        Public Sub New(ByVal pServerAddress As String, ByVal pDC1 As String)
            _New(pServerAddress, pDC1, Nothing)
        End Sub

        Public Sub New(ByVal pServerAddress As String, ByVal pDC1 As String, ByVal pDC2 As String)
            _New(pServerAddress, pDC1, pDC2)
        End Sub

        Public Sub New(ByVal pServerAddress As String, ByVal pDC1 As String, ByVal AdminLoginID As String, ByVal AdminPassword As String)
            _New(pServerAddress, pDC1, Nothing)
            SetAdmin(AdminLoginID, AdminPassword)
        End Sub

        Public Sub New(ByVal pServerAddress As String, ByVal pDC1 As String, ByVal pDC2 As String, ByVal AdminLoginID As String, ByVal AdminPassword As String)
            _New(pServerAddress, pDC1, pDC2)
            SetAdmin(AdminLoginID, AdminPassword)
        End Sub

        Public Sub New(ByVal pDomainSettings As Library2.Sys.oDomain)
            _New(pDomainSettings.IP, pDomainSettings.Dc1, pDomainSettings.Dc2)
            SetAdmin(pDomainSettings.UID, pDomainSettings.PWD)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub


        Private Sub _New(ByVal pServerIPAddress As String, ByVal pDC1 As String, ByVal pDC2 As String)
            _LDAPpath = String.Format("LDAP://{0}/DC={1}", pServerIPAddress.Trim.ToLower, pDC1.Trim.ToLower)
            If Not pDC2 = Nothing Then
                _LDAPpath += ",DC=" & pDC2
            End If
            _Domain = pDC1
        End Sub

        Private Sub SetAdmin(ByVal AdminLoginID As String, ByVal AdminPassword As String)
            _AdminLoginID = AdminLoginID
            _AdminLoginPWD = AdminPassword
        End Sub

        Public Function isValidUser() As Boolean
            Return isValidUser(_AdminLoginID, _AdminLoginPWD)
        End Function

        Public Function isValidUser(ByVal pUserID As String, ByVal pPassword As String) As Boolean
            Dim mDomainLogin As String = _Domain & "\" & pUserID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, pPassword)
            Try
                Dim obj As Object = mDE.NativeObject
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function ReadAdProperties(ByVal pUserID As String) As SearchResult
            Dim mUid As String = ""
            If pUserID.Split("\").Length > 1 Then
                mUid = pUserID.Split("\")(1)
            Else
                mUid = pUserID
            End If
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)

            Dim mSearcher As DirectorySearcher = New DirectorySearcher(mDE)
            Dim mFilter As StringBuilder = New StringBuilder
            mFilter.AppendFormat("(&(objectClass=user)(sAMAccountName={0}))", mUid)
            mSearcher.Filter = mFilter.ToString
            mSearcher.SearchScope = SearchScope.Subtree
            Dim mSR As SearchResult = mSearcher.FindOne
            If mSR Is Nothing Then
                Throw New NullReferenceException("No such directory entry exists")
            End If
            Return mSR
        End Function

        Private Function ReadAdProperties(ByVal AdFindString As String, ByVal Value As String) As SearchResult
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mSearcher As DirectorySearcher = New DirectorySearcher(mDE)
            mSearcher.Filter = String.Format("(&(objectClass=user)({0}={1}))", AdFindString, Value) '"(&(objectClass=user)(sAMAccountName=" + Value + "))"
            mSearcher.SearchScope = SearchScope.Subtree
            Dim mSR As SearchResult = mSearcher.FindOne()
            Return mSR
        End Function

        Private Function GetADProperty(ByVal pSearchResult As SearchResult, ByRef PropertyName As AdField) As Boolean
            Try
                If pSearchResult.Properties.Contains(PropertyName.AdFieldName) Then
                    PropertyName.Value = pSearchResult.Properties(PropertyName.AdFieldName).Item(0).ToString()
                Else
                    PropertyName.Value = String.Empty
                End If
                Return True
            Catch ex As Exception
                PropertyName.Value = String.Empty
                Return False
            End Try
        End Function

        Friend Shared Sub SetADProperty(ByVal pDE As DirectoryEntry, ByVal pProperty As AdField)
            If Not (pProperty.Value = String.Empty) Then
                If pDE.Properties.Contains(pProperty.AdFieldName) Then
                    pDE.Properties(pProperty.AdFieldName).Item(0) = pProperty.Value
                Else
                    pDE.Properties(pProperty.AdFieldName).Add(pProperty.Value)
                End If
            End If
        End Sub

        Public Function ReadByEmpId(ByVal pEmpID As String) As MyAdUserCard
            Dim mSR As SearchResult = ReadAdProperties("employeeID", pEmpID)
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry '(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            If Not (mSR Is Nothing) Then
                mDE = New DirectoryEntry(mSR.Path, mDomainLogin, _AdminLoginPWD, AuthenticationTypes.Secure)
                Return Read(mDE.Username)
            Else
                Return Nothing
            End If
        End Function

        Public Function ReadByEmail(ByVal pEmail As String) As MyAdUserCard
            Dim mSR As SearchResult = ReadAdProperties("mail", pEmail)
            Dim mUc As New MyAdUserCard
            GetADProperty(mSR, mUc.LoginID)
            If Not (mSR Is Nothing) Then
                Return Read(mUc.LoginID.Value)
            Else
                Return Nothing
            End If
        End Function

        Public Function ReadByProxyAddresses(ByVal pProxyAddress As String) As MyAdUserCard
            Dim mSR As SearchResult = ReadAdProperties("proxyaddresses", String.Format("*{0}*", pProxyAddress))
            Dim mUc As New MyAdUserCard
            GetADProperty(mSR, mUc.LoginID)
            If Not (mSR Is Nothing) Then
                Return Read(mUc.LoginID.Value)
            Else
                Return Nothing
            End If
        End Function

        Public Function Read() As MyAdUserCard
            Return Read(_AdminLoginID)
        End Function

        Public Function Read(ByVal pUserID As String) As MyAdUserCard
            Dim mRet As New MyAdUserCard
            Dim mSR As SearchResult '= ReadAdProperties(pUserID)
            If pUserID.Split("\").Length > 1 Then
                mSR = ReadAdProperties(pUserID.Split("\")(1))
            Else
                mSR = ReadAdProperties(pUserID)
            End If
            mRet.LoginID.Value = pUserID
            GetADProperty(mSR, mRet.DisplayName)
            GetADProperty(mSR, mRet.FirstName)
            GetADProperty(mSR, mRet.LastName)
            GetADProperty(mSR, mRet.MiddleName)
            GetADProperty(mSR, mRet.Title)
            GetADProperty(mSR, mRet.Department)
            GetADProperty(mSR, mRet.Section)
            GetADProperty(mSR, mRet.Company)
            'GetADProperty(mSR, mRet.DirectManager)

            GetADProperty(mSR, mRet.EmpID)
            GetADProperty(mSR, mRet.AddressStreet)
            GetADProperty(mSR, mRet.POBox)
            GetADProperty(mSR, mRet.City)
            GetADProperty(mSR, mRet.StateProvince)
            GetADProperty(mSR, mRet.Country)
            GetADProperty(mSR, mRet.OfficePhone)
            GetADProperty(mSR, mRet.MobilePhone)
            GetADProperty(mSR, mRet.Fax)
            GetADProperty(mSR, mRet.Email)

            GetADProperty(mSR, mRet.Pager)
            GetADProperty(mSR, mRet.IpPhone)
            GetADProperty(mSR, mRet.ZipPostalCode)
            GetADProperty(mSR, mRet.OfficePhone2)

            Return mRet
        End Function

        Public Function Write(ByVal UserCard As MyAdUserCard) As Boolean
            Dim mRet As Boolean = False
            Dim mDE As DirectoryEntry
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mSR As SearchResult = ReadAdProperties(UserCard.LoginID.Value)
            If Not (mSR Is Nothing) Then
                mDE = New DirectoryEntry(mSR.Path, mDomainLogin, _AdminLoginPWD, AuthenticationTypes.Secure)
                If Valid4Write(UserCard.DisplayName.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.DisplayName))
                End If
                If Valid4Write(UserCard.FirstName.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.FirstName))
                End If
                If Valid4Write(UserCard.LastName.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.LastName))
                End If
                If Valid4Write(UserCard.MiddleName.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.MiddleName))
                End If
                If Valid4Write(UserCard.Title.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Title))
                End If
                If Valid4Write(UserCard.Department.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Department))
                End If
                If Valid4Write(UserCard.Section.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Section))
                End If
                If Valid4Write(UserCard.Company.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Company))
                End If
                If Valid4Write(UserCard.EmpID.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.EmpID))
                End If
                If Valid4Write(UserCard.AddressStreet.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.AddressStreet))
                End If
                If Valid4Write(UserCard.POBox.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.POBox))
                End If
                If Valid4Write(UserCard.City.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.City))
                End If
                If Valid4Write(UserCard.StateProvince.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.StateProvince))
                End If
                If Valid4Write(UserCard.Country.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Country))
                End If
                If Valid4Write(UserCard.OfficePhone.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.OfficePhone))
                End If
                If Valid4Write(UserCard.MobilePhone.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.MobilePhone))
                End If
                If Valid4Write(UserCard.Fax.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Fax))
                End If
                'SetADProperty(mDE, FirstCap(UserCard.Email)

                If Valid4Write(UserCard.Pager.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.Pager))
                End If
                If Valid4Write(UserCard.IpPhone.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.IpPhone))
                End If
                If Valid4Write(UserCard.ZipPostalCode.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.ZipPostalCode))
                End If
                If Valid4Write(UserCard.OfficePhone2.Value) Then
                    SetADProperty(mDE, FirstCap(UserCard.OfficePhone2))
                End If

                mDE.CommitChanges()
                'Try
                '    mDE.CommitChanges()
                'Catch ex As Exception
                '    Throw New Exception("Access Denied!.")
                'End Try
                mRet = True
            End If
            Return mRet
        End Function

        Private Function FirstCap(ByVal pString As AdField) As AdField
            Dim mRet As New AdField
            mRet._AdFieldName = pString._AdFieldName
            If pString IsNot Nothing Then
                pString.Value = pString.Value.Trim
                If pString.Value.Length > 0 Then
                    Dim mStr() As String
                    Dim mSpace As String = ""
                    mStr = pString.Value.Split(" ")
                    For Each mSt As String In mStr
                        mSt = Left(mSt, 1).ToUpper & Mid(mSt, 2).ToLower
                        mRet.Value = mRet.Value & mSpace & Transform(mSt)
                        mSpace = " "
                    Next
                End If
            End If
            Return mRet
        End Function

        Private Function Transform(ByVal pString As String)
            Dim mFrom As String = "\IT"
            Dim mTo As String = "IT"
            Dim mRet As String = ""
            Dim mSt As Integer
            mSt = InStr(pString.ToLower, mFrom.ToLower)
            If mSt > 0 Then
                If mSt > 1 Then
                    mRet = Left(pString, mSt - 1)
                End If
                mRet = mRet & mTo & Mid(pString, mSt + mFrom.Length)
            Else
                mRet = pString
            End If
            Return mRet
        End Function

        Private Function Valid4Write(ByVal pString As String) As Boolean
            If pString <> CStr(MyAdUserCard._SKIP.Skip) Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function FindCN(ByVal pString As String) As String
            Dim mSt As Integer = InStr(pString, "cn=", CompareMethod.Text)
            Dim mRet As String = ""
            Dim mChar As Char = ""
            If mSt > 0 Then
                For i As Integer = mSt + 3 To pString.Length
                    mChar = Mid(pString, i, 1)
                    If mChar = "," Then
                        Exit For
                    End If
                    mRet += mChar
                Next
            End If
            Return mRet
        End Function

        'Public Function GetSearchResult(ByVal pUserID As String) As SearchResult
        '    Dim mRet As New MyAdUserCard
        '    Dim mSR As SearchResult
        '    If pUserID.Split("\").Length > 1 Then
        '        mSR = ReadAdProperties(pUserID.Split("\")(1))
        '    Else
        '        mSR = ReadAdProperties(pUserID)
        '    End If
        '    Return mSR
        'End Function

        Public Function GetAllGroupsInAD() As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mDS As New DirectorySearcher()
            mDS.SearchRoot = mDE
            mDS.Filter = "(objectClass=group)" '"(&(objectClass=group)(cn=CS_*))"
            Dim mDSR As SearchResultCollection = mDS.FindAll()
            If mDSR.Count > 0 Then
                For Each Result As SearchResult In mDSR
                    mRet.Add(Result.Properties("cn")(0).ToString)
                Next
                mRet.Sort()
            Else
                mRet = Nothing
            End If
            Return mRet
        End Function

        Public Function GetAllGroupsInAD(ByVal pFilter As String) As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mDS As New DirectorySearcher()
            mDS.SearchRoot = mDE
            mDS.Filter = "(&(objectClass=group)(cn=" & pFilter & "))"
            Dim mDSR As SearchResultCollection = mDS.FindAll()
            If mDSR.Count > 0 Then
                For Each Result As SearchResult In mDSR
                    mRet.Add(Result.Properties("cn")(0).ToString)
                Next
                mRet.Sort()
            Else
                mRet = Nothing
            End If
            Return mRet
        End Function

        Public Function GetGroups(ByVal UserName As String) As List(Of String)
            Dim mRet As New List(Of String)
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mDS As New DirectorySearcher(mDE)
            'mDS.Filter = String.Format("(sAMAccountName={0}))", username)
            mDS.Filter = String.Format("(&(objectClass=user)(sAMAccountName={0}))", UserName)
            mDS.PropertiesToLoad.Add("memberOf")
            Dim propCount As Integer
            Try
                Dim mDSR As SearchResult = mDS.FindOne()
                propCount = mDSR.Properties("memberOf").Count
                Dim mGrp As String
                Dim mEqualsIndex As String
                Dim mCommaIndex As String
                For i As Integer = 0 To propCount - 1
                    mGrp = mDSR.Properties("memberOf")(i)
                    mEqualsIndex = mGrp.IndexOf("=", 1)
                    mCommaIndex = mGrp.IndexOf(",", 1)
                    If mEqualsIndex = -1 Then
                        Return Nothing
                    End If
                    If Not mRet.Contains(mGrp.Substring((mEqualsIndex + 1), (mCommaIndex - mEqualsIndex) - 1)) Then
                        mRet.Add(mGrp.Substring((mEqualsIndex + 1), (mCommaIndex - mEqualsIndex) - 1))
                    End If
                Next
            Catch ex As Exception
                If ex.GetType Is GetType(System.NullReferenceException) Then
                    'Throw New Exception("Does not have a group")
                    mRet = Nothing
                Else
                    Throw ex
                End If
            End Try
            If mRet IsNot Nothing Then
                mRet.Sort()
            End If
            Return mRet
        End Function

        Public Function GetUsers(ByVal GroupName As String) As List(Of MyAdUserCard)
            Dim mRet As New List(Of MyAdUserCard)
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mDS As New DirectorySearcher()
            mDS.SearchRoot = mDE
            mDS.Filter = "(&(objectClass=group)(cn=" & GroupName & "))"
            Dim mDSR As SearchResult
            Dim mDSRC As SearchResultCollection = mDS.FindAll()
            Dim mFilter As New List(Of String)
            Dim mUCard As MyAdUserCard
            If mDSRC.Count > 0 Then
                Try
                    For cnt As Integer = 1 To mDSRC(0).GetDirectoryEntry.Properties("member").Count
                        mFilter.Add(FindCN(mDSRC(0).GetDirectoryEntry.Properties("member").Item(cnt - 1)))
                    Next
                    For Each mF As String In mFilter
                        mDS.Filter = "(&(objectClass=user)(cn=" & mF & "))"
                        mDSR = mDS.FindOne
                        If mDSR IsNot Nothing Then
                            If mDSR.Properties.Contains("sAMAccountName") Then
                                mUCard = New MyAdUserCard
                                mUCard = Read(mDSR.Properties("sAMAccountName").Item(0))
                                If mUCard IsNot Nothing Then
                                    mRet.Add(mUCard)
                                End If
                            End If
                        End If
                    Next
                Catch ex As Exception
                    mRet = Nothing
                End Try

            End If
            Return mRet
        End Function

        Public Function GetUserInfo(ByVal pUserID As String) As MyAdUserCard
            Return Read(pUserID)
        End Function

        Public Function GetUserEmails(ByVal pUserID As String) As String()
            Dim mRet() As String
            Try
                Dim mEml As String
                Dim mDSR As SearchResult = GetMemberProperty(pUserID, "mail")
                ReDim mRet(1)
                mRet(0) = LTrim(RTrim(mDSR.Properties("mail").Item(0))).ToLower
                For cnt As Integer = 1 To mDSR.Properties("proxyAddresses").Count
                    If Left(LTrim(mDSR.Properties("proxyAddresses").Item(cnt - 1)), 4).ToLower = "smtp" Then
                        mEml = LTrim(RTrim(Mid(mDSR.Properties("proxyAddresses").Item(cnt - 1), 6))).ToLower
                        If Not isEmailExist(mRet, mEml) Then
                            ReDim Preserve mRet(UBound(mRet) + 1)
                            mRet(UBound(mRet) - 1) = mEml
                        End If
                    End If
                Next
            Catch ex As Exception
                ReDim mRet(1)
            End Try
            Return mRet
        End Function

        Private Function GetMemberProperty(ByVal UserName As String, ByVal PropertyName As String) As SearchResult
            Dim mDomainLogin As String = _Domain & "\" & _AdminLoginID
            Dim mDE As DirectoryEntry = New DirectoryEntry(_LDAPpath, mDomainLogin, _AdminLoginPWD)
            Dim mDS As DirectorySearcher = New DirectorySearcher(mDE)
            Dim filter As StringBuilder = New StringBuilder
            filter.AppendFormat("(&(objectClass=user)(sAMAccountName={0}))", UserName)
            mDS.Filter = filter.ToString
            Dim mDSR As SearchResult = mDS.FindOne
            If mDSR Is Nothing Then
                Throw New NullReferenceException("No such directory entry exists")
            End If
            Return mDSR
        End Function

        Private Function isEmailExist(ByVal pArray() As String, ByVal Find As String) As Boolean
            Dim mRet As Boolean = False
            For Each mStr As String In pArray
                If mStr = Find Then
                    mRet = True
                End If
            Next
            Return mRet
        End Function


    End Class


    Public Class MyAdUserCard
        Private _loginID As AdField 'sAMAccountName
        Private _DisplayName As AdField  'in AD: displayName
        Private _FirstName As AdField  'givenName
        Private _LastName As AdField  'sn
        Private _MiddleName As AdField  'initials
        Private _Title As AdField 'title
        Private _Department As AdField 'department
        Private _Section As AdField 'physicalDeliveryOfficeName
        Private _Company As AdField 'company
        Private _DirectManager As MyAdUserCard 'Manager

        Private _EmpID As AdField 'employeeID = EMPID
        Private _AddressStreet As AdField 'postalAddress
        Private _POBox As AdField 'postOfficeBox
        Private _City As AdField 'l
        Private _StateProvince As AdField 'st
        Private _Country As AdField 'c
        Private _OfficePhone As AdField 'telephoneNumber
        Private _MobilePhone As AdField 'mobile
        Private _Fax As AdField 'facsimileTelephoneNumber
        Private _Email As AdField 'mail

        Private _Pager As AdField 'pager
        Private _IpPhone As AdField 'ipphone
        Private _ZipPostalCode As AdField 'postalcode
        Private _PhoneAdditional As AdField 'otherTelephone

        Public Enum _SKIP
            Skip = -1
        End Enum

        Private Class _AdField
            Inherits AdField

            Public Sub New(ByVal pAdFieldname As String)
                _AdFieldName = pAdFieldname
            End Sub
        End Class

        Public Sub New()
            _loginID = New _AdField("sAMAccountName")
            _DisplayName = New _AdField("displayName")
            _FirstName = New _AdField("givenName")
            _LastName = New _AdField("sn")
            _MiddleName = New _AdField("initials")
            _Title = New _AdField("title")
            _Department = New _AdField("department")
            _Section = New _AdField("physicalDeliveryOfficeName")
            _Company = New _AdField("company")
            '_DirectManager = New _AdField("Manager")

            _EmpID = New _AdField("employeeID")
            _AddressStreet = New _AdField("streetAddress")
            _POBox = New _AdField("postOfficeBox")
            _City = New _AdField("l")
            _StateProvince = New _AdField("st")
            _Country = New _AdField("c")
            _OfficePhone = New _AdField("telephoneNumber")
            _MobilePhone = New _AdField("mobile")
            _Fax = New _AdField("facsimileTelephoneNumber")
            _Email = New _AdField("mail")

            _Pager = New _AdField("pager") '
            _IpPhone = New _AdField("ipphone") '
            _ZipPostalCode = New _AdField("postalcode") '
            _PhoneAdditional = New _AdField("otherTelephone") '

        End Sub

        Public Property LoginID() As AdField
            Get
                Return _loginID
            End Get
            Set(ByVal value As AdField)
                _loginID.Value = value.Value
            End Set
        End Property

        Public Property DisplayName() As AdField
            Get
                Return _DisplayName
            End Get
            Set(ByVal value As AdField)
                _DisplayName.Value = value.Value
            End Set
        End Property

        Public Property FirstName() As AdField
            Get
                Return _FirstName
            End Get
            Set(ByVal value As AdField)
                _FirstName.Value = value.Value
            End Set
        End Property

        Public Property LastName() As AdField
            Get
                Return _LastName
            End Get
            Set(ByVal value As AdField)
                _LastName.Value = value.Value
            End Set
        End Property

        Public Property MiddleName() As AdField
            Get
                Return _MiddleName
            End Get
            Set(ByVal value As AdField)
                _MiddleName.Value = value.Value
            End Set
        End Property

        Public Property Title() As AdField
            Get
                Return _Title
            End Get
            Set(ByVal value As AdField)
                _Title.Value = value.Value
            End Set
        End Property

        Public Property Department() As AdField
            Get
                Return _Department
            End Get
            Set(ByVal value As AdField)
                _Department.Value = value.Value
            End Set
        End Property

        Public Property Section() As AdField
            Get
                Return _Section
            End Get
            Set(ByVal value As AdField)
                _Section.Value = value.Value
            End Set
        End Property

        Public Property Company() As AdField
            Get
                Return _Company
            End Get
            Set(ByVal value As AdField)
                _Company.Value = value.Value
            End Set
        End Property

        'Public Property DirectManager() As MyAdUserCard
        '    Get
        '        Return _DirectManager
        '    End Get
        '    Set(ByVal value As MyAdUserCard)
        '        _DirectManager = value
        '    End Set
        'End Property

        Public Property EmpID() As AdField
            Get
                Return _EmpID
            End Get
            Set(ByVal value As AdField)
                _EmpID.Value = value.Value
            End Set
        End Property

        Public Property AddressStreet() As AdField
            Get
                Return _AddressStreet
            End Get
            Set(ByVal value As AdField)
                _AddressStreet.Value = value.Value
            End Set
        End Property

        Public Property POBox() As AdField
            Get
                Return _POBox
            End Get
            Set(ByVal value As AdField)
                _POBox.Value = value.Value
            End Set
        End Property

        Public Property City() As AdField
            Get
                Return _City
            End Get
            Set(ByVal value As AdField)
                _City.Value = value.Value
            End Set
        End Property

        Public Property StateProvince() As AdField
            Get
                Return _StateProvince
            End Get
            Set(ByVal value As AdField)
                _StateProvince.Value = value.Value
            End Set
        End Property

        Public Property Country() As AdField
            Get
                Return _Country
            End Get
            Set(ByVal value As AdField)
                _Country.Value = value.Value
            End Set
        End Property

        Public Property OfficePhone() As AdField
            Get
                Return _OfficePhone
            End Get
            Set(ByVal value As AdField)
                _OfficePhone.Value = value.Value
            End Set
        End Property

        Public Property MobilePhone() As AdField
            Get
                Return _MobilePhone
            End Get
            Set(ByVal value As AdField)
                _MobilePhone.Value = value.Value
            End Set
        End Property

        Public Property Fax() As AdField
            Get
                Return _Fax
            End Get
            Set(ByVal value As AdField)
                _Fax.Value = value.Value
            End Set
        End Property

        Public Property Email() As AdField
            Get
                Return _Email
            End Get
            Set(ByVal value As AdField)
                _Email.Value = value.Value
            End Set
        End Property

        Public Property Pager() As AdField
            Get
                Return _Pager
            End Get
            Set(ByVal value As AdField)
                _Pager.Value = value.Value
            End Set
        End Property

        Public Property IpPhone() As AdField
            Get
                Return _IpPhone
            End Get
            Set(ByVal value As AdField)
                _IpPhone.Value = value.Value
            End Set
        End Property

        Public Property ZipPostalCode() As AdField
            Get
                Return _ZipPostalCode
            End Get
            Set(ByVal value As AdField)
                _ZipPostalCode.Value = value.Value
            End Set
        End Property

        Public Property OfficePhone2() As AdField
            Get
                Return _PhoneAdditional
            End Get
            Set(ByVal value As AdField)
                _PhoneAdditional.Value = value.Value
            End Set
        End Property

    End Class

    Public Class AdField
        Private _value As String = ""
        Protected Friend _AdFieldName As String

        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal pValue As String)
                _value = pValue
            End Set
        End Property

        Public ReadOnly Property AdFieldName() As String
            Get
                Return _AdFieldName
            End Get
        End Property

    End Class

End Namespace

'Module Module1
'#Region "..."
'    Private mU As String = "arakkala"
'    Private mP As String = "6546"
'#End Region
'    Private mDomainIP As String '= '"10.10.80.36" '"130.255.99.5"
'    Private mDC1 As String '= '"TCWS" '"ALJAZEERA"
'    Private mDC2 As String '= '"NET" '"TV"

'    Private Sub Jsc()
'        mDomainIP = "130.255.99.5"
'        mDC1 = "ALJAZEERA"
'        mDC2 = "TV"
'    End Sub

'    Private Sub TestDc()
'        mDomainIP = "10.10.80.36"
'        mDC1 = "TCWS"
'        mDC2 = "NET"
'    End Sub

'    Sub Main()
'        'TestDc()
'        Jsc()
'        Dim mAd As New MyAdManager(mDomainIP, mDC1, mDC2, mU, mP)
'        If mAd.isValidUser(mU, mP) Then
'            Console.WriteLine("Ok")
'        End If
'        Dim mC As New MyAdUserCard
'        'mC = mAd.Read("arakkala")
'        'mC = mAd.ReadByEmpId("913")
'        'mC = mAd.ReadByEmail("arakkala@aljazeera.net")
'        Dim mR(8) As String
'        mR(0) = "jad@aljazeera.net"
'        mR(1) = "rizwan@aljazeera.net"
'        mR(2) = "raoofabdul@aljazeera.net"
'        mR(3) = "zubirh@aljazeera.net"
'        mR(4) = "ghanemj@aljazeera.net"
'        mR(5) = "ahmedr@aljazeera.net"
'        mR(6) = "arakkala@aljazeera.net"
'        mR(7) = "attilim@aljazeera.net"
'        For Each mStr As String In mR
'            If mStr IsNot Nothing Then
'                mC = mAd.ReadByProxyAddresses(mStr)
'                If mC IsNot Nothing Then
'                    Console.WriteLine(mC.DisplayName.Value)
'                Else
'                    Console.WriteLine(mStr & " not found")
'                End If
'            End If
'        Next
'        'mC.DisplayName.Value = "Abdulraoof S Arakkal"

'        'TestDc()
'        'mAd = New MyAdManager(mDomainIP, mDC1, mDC2, mU, mP)
'        ''mC.MobilePhone.Value = "5816287"
'        ''mc.DirectManager=mad.Read(
'        ''mC.DirectManager.Value = "Munjed Attitli"
'        'mAd.Write(mC)
'    End Sub

'End Module
