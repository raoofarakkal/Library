Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports _Library._Web._Common

Namespace _Library._Web

    Public Class clsftp2
        Private mFtp As FtpWebRequest
        Private mHost As String
        Private mPort As String
        Private mTimeOut As Integer = 20
        Private mTCP1Client, mTCP2Client As New TcpClient()
        Private mNW1Stream, mNW2Stream As NetworkStream
        Private mDirListItem As DirListItem
        Private mDirList As DirList
        Private mProxyInfo As _Library.proxy.ProxyInfo


        Public Sub New()
            mProxyInfo = Nothing
        End Sub

        Public Sub New(ByVal pProxyInfo As _Library.proxy.ProxyInfo)
            mProxyInfo = pProxyInfo
        End Sub

        Private Function FtpCommandHelp() As Boolean

            'www.nsftools.com site 

            'List of raw FTP commands
            '(Warning: this is a technical document, not necessary for most FTP use.) 

            'Note that commands marked with a * are not implemented in a number of FTP servers. 

            '        Common(commands)
            'ABOR - abort a file transfer 
            'CWD - change working directory 
            'DELE - delete a remote file 
            'LIST - list remote files 
            'MDTM - return the modification time of a file 

            'not sure these 3 commands will support or not
            'MFMT - Modify File Modification Time
            'MFCT - Modify File Creation(Time)
            'MFF  - Modify File Facts

            'MKD - make a remote directory 
            'NLST - name list of remote directory 
            'PASS - send password 
            'PASV - enter passive mode 
            'PORT - open a data port 
            'PWD - print working directory 
            'QUIT - terminate the connection 
            'RETR - retrieve a remote file 
            'RMD - remove a remote directory 
            'RNFR - rename from 
            'RNTO - rename to 
            'SITE - site-specific commands 
            'SIZE - return the size of a file 
            'STOR - store a file on the remote host 
            'TYPE - set transfer type 
            'USER - send username 
            'Less common commands
            'ACCT* - send account information 
            'APPE - append to a remote file 
            'CDUP - CWD to the parent of the current directory 
            'HELP - return help on using the server 
            'MODE - set transfer mode 
            'NOOP - do nothing 
            'REIN* - reinitialize the connection 
            'STAT - return server status 
            'STOU - store a file uniquely 
            'STRU - set file transfer structure 
            'SYST - return system type 
            '        ABOR()
            'Syntax: ABOR()

            'Aborts a file transfer currently in progress. 

            'ACCT*
            'Syntax: ACCT(account - info)

            'This command is used to send account information on systems that require it. Typically sent after a PASS command. 

            '        ALLO()
            'Syntax: ALLO size [R max-record-size] 

            'Allocates sufficient storage space to receive a file. If the maximum size of a record also needs to be known, that is sent as a second numeric parameter following a space, the capital letter "R", and another space. 

            '        APPE()
            'Syntax: APPE(remote - filename)

            'Append data to the end of a file on the remote host. If the file does not already exist, it is created. This command must be preceded by a PORT or PASV command so that the server knows where to receive data from. 

            '        CDUP()
            'Syntax: CDUP()

            'Makes the parent of the current directory be the current directory. 

            '        CWD()
            'Syntax: CWD(remote - Directory)

            'Makes the given directory be the current directory on the remote host. 

            '        DELE()
            'Syntax: DELE(remote - filename)

            'Deletes the given file on the remote host. 

            '        HELP()
            'Syntax: HELP([Command])

            'If a command is given, returns help on that command; otherwise, returns general help for the FTP server (usually a list of supported commands). 

            '            List()
            'Syntax: LIST [remote-filespec] 

            'If remote-filespec refers to a file, sends information about that file. If remote-filespec refers to a directory, sends information about each file in that directory. remote-filespec defaults to the current directory. This command must be preceded by a PORT or PASV command. 

            '                MDTM()
            'Syntax:         MDTM(remote - filename)

            'Returns the last-modified time of the given file on the remote host in the format "YYYYMMDDhhmmss": YYYY is the four-digit year, MM is the month from 01 to 12, DD is the day of the month from 01 to 31, hh is the hour from 00 to 23, mm is the minute from 00 to 59, and ss is the second from 00 to 59. 

            '                MKD()
            'Syntax:         MKD(remote - Directory)

            'Creates the named directory on the remote host. 

            '                MODE()
            'Syntax:         MODE(mode - character)

            'Sets the transfer mode to one of: 
            '                S(-Stream)
            '                B(-Block)
            '                C(-Compressed)
            'The default mode is Stream. 

            '                NLST()
            'Syntax: NLST [remote-directory] 

            'Returns a list of filenames in the given directory (defaulting to the current directory), with no other information. Must be preceded by a PORT or PASV command. 

            '                NOOP()
            'Syntax:         NOOP()

            'Does nothing except return a response. 

            '                PASS()
            'Syntax:         PASS(password)

            'After sending the USER command, send this command to complete the login process. (Note, however, that an ACCT command may have to be used on some systems.) 

            '                PASV()
            'Syntax:         PASV()

            'Tells the server to enter "passive mode". In passive mode, the server will wait for the client to establish a connection with it rather than attempting to connect to a client-specified port. The server will respond with the address of the port it is listening on, with a message like:
            '227 Entering Passive Mode (a1,a2,a3,a4,p1,p2)
            'where a1.a2.a3.a4 is the IP address and p1*256+p2 is the port number. 

            '                PORT()
            'Syntax:         PORT(a1, a2, a3, a4, p1, p2)

            'Specifies the host and port to which the server should connect for the next file transfer. This is interpreted as IP address a1.a2.a3.a4, port p1*256+p2. 

            '                PWD()
            'Syntax:         PWD()

            'Returns the name of the current directory on the remote host. 

            '                QUIT()
            'Syntax:         QUIT()

            'Terminates the command connection. 

            'REIN*
            'Syntax:         REIN()

            'Reinitializes the command connection - cancels the current user/password/account information. Should be followed by a USER command for another login. 

            '                REST()
            'Syntax:         REST(position)

            'Sets the point at which a file transfer should start; useful for resuming interrupted transfers. For nonstructured files, this is simply a decimal number. This command must immediately precede a data transfer command (RETR or STOR only); i.e. it must come after any PORT or PASV command. 

            '                RETR()
            'Syntax:         RETR(remote - filename)

            'Begins transmission of a file from the remote host. Must be preceded by either a PORT command or a PASV command to indicate where the server should send data. 

            '                RMD()
            'Syntax:         RMD(remote - Directory)

            'Deletes the named directory on the remote host. 

            '                RNFR()
            'Syntax:         RNFR(from - filename)

            'Used when renaming a file. Use this command to specify the file to be renamed; follow it with an RNTO command to specify the new name for the file. 

            '                RNTO()
            'Syntax: RNTO to-filename 

            'Used when renaming a file. After sending an RNFR command to specify the file to rename, send this command to specify the new name for the file. 

            'SITE*
            'Syntax:         SITE(site - specific - Command())

            'Executes a site-specific command. 

            '                SIZE()
            'Syntax:         SIZE(remote - filename)

            'Returns the size of the remote file as a decimal number. 

            '                STAT()
            'Syntax: STAT [remote-filespec] 

            'If invoked without parameters, returns general status information about the FTP server process. If a parameter is given, acts like the LIST command, except that data is sent over the control connection (no PORT or PASV command is required). 

            '                    STOR()
            'Syntax:             STOR(remote - filename)

            'Begins transmission of a file to the remote site. Must be preceded by either a PORT command or a PASV command so the server knows where to accept data from. 

            '                    STOU()
            'Syntax:             STOU()

            'Begins transmission of a file to the remote site; the remote filename will be unique in the current directory. The response from the server will include the filename. 

            '                    STRU()
            'Syntax: STRU structure-character 

            'Sets the file structure for transfer to one of: 
            'F - File (no structure) 
            'R - Record structure 
            'P - Page structure 
            'The default structure is File. 

            '                    SYST()
            'Syntax:             SYST()

            'Returns a word identifying the system, the word "Type:", and the default transfer type (as would be set by the TYPE command). For example: UNIX Type: L8 

            '                    Type()
            'Syntax: TYPE type-character [second-type-character] 

            'Sets the type of file to be transferred. type-character can be any of: 
            'A - ASCII text 
            'E - EBCDIC text 
            'I - image (binary data) 
            'L - local format 
            'For A and E, the second-type-character specifies how the text should be interpreted. It can be: 
            'N - Non-print (not destined for printing). This is the default if second-type-character is omitted. 
            'T - Telnet format control (<CR>, <FF>, etc.) 
            'C - ASA Carriage Control 
            'For L, the second-type-character specifies the number of bits per byte on the local system, and may not be omitted. 

            '                            USER()
            'Syntax:                     USER(username)

            'Send this command to begin the login process. username should be a valid username on the system, or "anonymous" to initiate an anonymous login.
        End Function

        <Runtime.InteropServices.ClassInterface(Runtime.InteropServices.ClassInterfaceType.None)> _
        Public Class DirList
            Inherits List(Of DirListItem)
        End Class

        Public Class DirListItem
            Private mDateTime As Date
            Private mType As EnumFileType
            Private mSize As Integer
            Private mFileName As String
            Public Property DateTime() As Date
                Get
                    Return mDateTime
                End Get
                Set(ByVal value As Date)
                    mDateTime = value
                End Set
            End Property
            Public Property Type() As EnumFileType
                Get
                    Return mType
                End Get
                Set(ByVal value As EnumFileType)
                    mType = value
                End Set
            End Property
            Public Property Size() As Integer
                Get
                    Return mSize
                End Get
                Set(ByVal value As Integer)
                    mSize = value
                End Set
            End Property
            Public Property FileName() As String
                Get
                    Return mFileName
                End Get
                Set(ByVal value As String)
                    mFileName = value
                End Set
            End Property
            Protected Overrides Sub Finalize()
                mDateTime = Nothing
                mType = Nothing
                mSize = Nothing
                mFileName = Nothing
                MyBase.Finalize()
            End Sub
        End Class

        Public Enum EnumFileType
            _Directory = 1
            _File = 2
        End Enum

        Public ReadOnly Property Connected() As Boolean
            Get
                Return mTCP1Client.Connected
            End Get
        End Property

        Public Property TimeOut() As Integer
            Get
                Return mTimeOut
            End Get
            Set(ByVal value As Integer)
                mTimeOut = value
            End Set
        End Property

        Public ReadOnly Property Host() As String
            Get
                Return mHost
            End Get
        End Property

        Public Function Connect(ByVal Host As String, ByVal Port As Integer, ByVal UID As String, ByVal PWD As String) As Boolean
            Dim mStr As String = Nothing
            Try
                mHost = Host
                mPort = Port
                mTCP1Client.Close()
                mTCP1Client = New TcpClient()
                mTCP1Client.Connect(Host, Port)
                mNW1Stream = mTCP1Client.GetStream()
                mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                If MySubStr(mStr, 0, 3) = "220" Then
                    Try
                        SendRequest(mNW1Stream, "USER " & UID)
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        If MySubStr(mStr, 0, 3) = "331" Then
                            SendRequest(mNW1Stream, "PASS " & PWD)
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                            If MySubStr(mStr, 0, 3) <> "230" Then
                                mTCP1Client.Close()
                                RaiseException("Incorrect password!. " & mStr, "FTP.Connect()")
                            End If
                        Else
                            mTCP1Client.Close()
                            RaiseException("Server rejected user " & UID & "!. " & mStr, "FTP.Connect()")
                        End If
                    Catch ex As Exception
                        mTCP1Client.Close()
                        Throw ex
                    End Try
                Else
                    mTCP1Client.Close()
                    RaiseException("Could not connect to " & Host & " at port " & Port & ": " & mStr, "FTP.Connect()")
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mTCP1Client.Connected
        End Function

        Public Function Close() As Boolean
            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "QUIT")
                    mTCP1Client.Close()
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            Return Not mTCP1Client.Connected
        End Function

        Public Function Dir(Optional ByVal pDirectory As String = "..", Optional ByVal ExcludeDirs As Boolean = False) As DirList
            Dim mMS As New MemoryStream()
            Dim mStr, mStr2 As String
            Dim mPort2 As Integer
            Dim mRet As String = Nothing
            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then


                        mTCP2Client = New TcpClient(mHost, mPort2)
                        If pDirectory = ".." Then
                            mStr2 = "LIST -aL" & vbCrLf
                        Else
                            mStr2 = "LIST " & pDirectory & vbCrLf
                        End If
                        mStr = ""
                        SendRequest(mNW1Stream, mStr2)
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)


                        If MySubStr(mStr, 0, 4) <> "125 " Then
                            RaiseException("Data Transfer error!", "FTP.Dir()")
                        Else
                            Try
                                mNW2Stream = mTCP2Client.GetStream()
                                mRet = GetResponse2Port(mTCP2Client, mNW2Stream, TimeOut)


                                mTCP2Client.Close()
                            Catch Ex As Exception
                                Throw Ex
                            End Try
                            If InStr(mStr, "226 Transfer complete") = 0 Then
                                mStr = ""
                                mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)

                                If MySubStr(mStr, 0, 4) <> "226 " Then
                                    RaiseException("Data transfer not completed!", "FTP.Dir() " & mStr)
                                End If
                            End If
                        End If
                    End If
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            mDirList = New DirList
            If mRet.Length > 0 Then
                Dim mAr() As String
                mAr = Split(mRet, vbCrLf)
                For cnt As Integer = 1 To UBound(mAr)
                    Dim mM() As String
                    mM = FAT(mAr(cnt - 1).Split(" "))
                    If mM(1) = "DIR" Then
                        If Not ExcludeDirs Then
                            mDirListItem = New DirListItem
                            mDirListItem.DateTime = mM(0)
                            mDirListItem.Type = EnumFileType._Directory
                            mDirListItem.Size = MyCint(mM(2))
                            mDirListItem.FileName = mM(3)
                            mDirList.Add(mDirListItem)
                            mDirListItem = Nothing
                        End If
                    Else
                        mDirListItem = New DirListItem
                        mDirListItem.DateTime = mM(0)
                        mDirListItem.Type = EnumFileType._File
                        mDirListItem.Size = MyCint(mM(2))
                        mDirListItem.FileName = mM(3)
                        mDirList.Add(mDirListItem)
                        mDirListItem = Nothing
                    End If
                    mM = Nothing
                Next
            End If
            Return mDirList
        End Function

        Public Function DownloadMsFtp(ByVal FtpSitePathAndFileName As String, ByVal LocalPathAndFileName As String, ByVal HostIP As String, ByVal UID As String, ByVal PWD As String) As Boolean
            Dim mRet As Boolean = False
            Dim mReqFTP As FtpWebRequest
            Try
                Dim outputStream As New FileStream(LocalPathAndFileName, FileMode.Create)

                Dim mUri As String = "ftp://" + HostIP + IIf(Left(FtpSitePathAndFileName, 1) = "/", FtpSitePathAndFileName, "/" + FtpSitePathAndFileName)
                mReqFTP = DirectCast(FtpWebRequest.Create(New Uri(mUri)), FtpWebRequest)
                mReqFTP.Method = WebRequestMethods.Ftp.DownloadFile
                mReqFTP.UseBinary = True
                mReqFTP.Credentials = New NetworkCredential(UID, PWD)
                mReqFTP.Timeout = TimeOut * 1000

                If mProxyInfo IsNot Nothing Then
                    mReqFTP.Proxy = mProxyInfo.Proxy
                Else
                    mReqFTP.Proxy = WebRequest.DefaultWebProxy
                End If

                'If mdlConfig.Proxy("UseProxyForDownload").ToLower.Trim = "true" Then
                '    mReqFTP.Proxy = New WebProxy(mdlConfig.Proxy("ProxyIP"), MyCint(mdlConfig.Proxy("ProxyPort")))
                '    mReqFTP.Proxy.Credentials = New NetworkCredential(mdlConfig.Proxy("UID"), mdlConfig.Proxy("PWD"), mdlConfig.Proxy("DC"))
                'Else
                '    mReqFTP.Proxy = WebRequest.DefaultWebProxy
                'End If

                Dim mResponse As FtpWebResponse = DirectCast(mReqFTP.GetResponse(), FtpWebResponse)
                Dim mFtpStream As Stream = mResponse.GetResponseStream()
                'Dim mCl As Long = mResponse.ContentLength
                Dim mBufferSize As Integer = 2048
                Dim mReadCount As Integer
                Dim mBuffer As Byte() = New Byte(mBufferSize - 1) {}

                mReadCount = mFtpStream.Read(mBuffer, 0, mBufferSize)
                While mReadCount > 0
                    outputStream.Write(mBuffer, 0, mReadCount)
                    mReadCount = mFtpStream.Read(mBuffer, 0, mBufferSize)
                End While

                mFtpStream.Close()
                outputStream.Close()
                mResponse.Close()
                mRet = True
            Catch ex As Exception
                Throw ex
            Finally
                mReqFTP = Nothing
            End Try
            Return mRet
        End Function


        Public Function UploadMsFtp(ByVal FtpSitePathAndFileName As String, ByVal LocalPathAndFileName As String, ByVal HostIP As String, ByVal UID As String, ByVal PWD As String) As Boolean
            Dim mRet As Boolean = False
            Dim mFileInf As New FileInfo(LocalPathAndFileName)
            Dim mUri As String = "ftp://" + HostIP + IIf(Left(FtpSitePathAndFileName, 1) = "/", FtpSitePathAndFileName, "/" + FtpSitePathAndFileName) 'fileInf.Name
            Dim mReqFTP As FtpWebRequest

            ' Create FtpWebRequest object from the Uri provided 
            'mReqFTP = DirectCast(FtpWebRequest.Create(New Uri(("ftp://" & ftpServerIP & "/") + fileInf.Name)), FtpWebRequest)
            mReqFTP = DirectCast(FtpWebRequest.Create(New Uri(mUri)), FtpWebRequest)

            ' Provide the WebPermission Credintials 
            mReqFTP.Credentials = New NetworkCredential(UID, PWD)

            ' By default KeepAlive is true, where the control connection is not closed 
            ' after a command is executed. 
            mReqFTP.KeepAlive = False

            ' Specify the command to be executed. 
            mReqFTP.Method = WebRequestMethods.Ftp.UploadFile

            ' Specify the data transfer type. 
            mReqFTP.UseBinary = True

            ' Notify the server about the size of the uploaded file 
            mReqFTP.ContentLength = mFileInf.Length
            'mReqFTP.Timeout = TimeOut * 1000
            mReqFTP.Proxy = Nothing

            ' The buffer size is set to 2kb 
            Dim mBuffLength As Integer = 2048 'mFileInf.Length '2048
            Dim mBuffer As Byte() = New Byte(mBuffLength - 1) {}
            Dim mContentLen As Integer

            ' Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
            Dim mFs As FileStream = mFileInf.OpenRead()

            Try
                ' Stream to which the file to be upload is written 
                Dim mStream As Stream = mReqFTP.GetRequestStream()

                ' Read from the file stream 2kb at a time 
                mContentLen = mFs.Read(mBuffer, 0, mBuffLength)

                ' Till Stream content ends 
                While mContentLen <> 0
                    ' Write Content from the file stream to the FTP Upload Stream 
                    mStream.Write(mBuffer, 0, mContentLen)
                    mContentLen = mFs.Read(mBuffer, 0, mBuffLength)
                End While

                'mBuffLength = mFileInf.Length
                'mContentLen = mFs.Read(mBuffer, 0, mBuffLength)
                'mStream.Write(mBuffer, 0, mContentLen)


                ' Close the file stream and the Request Stream 
                mStream.Close()
                mFs.Close()
                mRet = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRet
        End Function


        Public Function Download(ByVal FtpSiteFileName As String, ByVal LocalPathAndFileName As String) As Boolean
            Dim mRet As Boolean = False
            Dim mBin As New MemoryStream()
            Dim mStr As String
            Dim mPort2 As Integer
            Dim mIO As FileStream
            Dim mBytes() As Byte

            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "TYPE I")
                    mStr = ""
                    mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                    If MySubStr(mStr, 0, 4) <> "200 " Then
                        RaiseException("Cannot switch to Binary type!", "FTP.Download() " & mStr)
                    End If
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then
                        mTCP2Client = New TcpClient(mHost, mPort2)
                        SendRequest(mNW1Stream, "RETR " & FtpSiteFileName)
                        mStr = ""
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        Try
                            mNW2Stream = mTCP2Client.GetStream()
                            mBin = GetResponse2PortBin(mTCP2Client, mNW2Stream, TimeOut)
                            mTCP2Client.Close()
                        Catch Ex As Exception
                            Throw Ex
                        End Try
                        mBytes = mBin.ToArray()
                        mIO = File.OpenWrite(LocalPathAndFileName)
                        mIO.Write(mBytes, 0, mBytes.Length)
                        mIO.Close()
                        If InStr(mStr, "226 ") = 0 Then
                            mStr = ""
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                            If InStr(mStr, "226 ") = 0 Then
                                RaiseException("Transfer failure!", "FTP.Download() " & mStr)
                            End If
                        End If
                        mRet = True
                    End If
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            Return mRet
        End Function

        Public Function Upload(ByVal LocalPathAndFileName As String) As Boolean
            Dim mRet As Boolean = False
            Dim mBin As New MemoryStream()
            Dim mStr As String
            Dim mPort2 As Integer
            Dim mIO As FileStream
            Dim mBytes() As Byte
            Dim mBr As Integer
            Dim mFileAr() As String
            Dim mSpeed As Long = 2048 ' * 64
            Dim mBc(mSpeed) As Byte
            Dim mSec As Long
            Try
                If mTCP1Client.Connected Then
                    SendRequest(mNW1Stream, "TYPE I")
                    mStr = ""
                    mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                    If MySubStr(mStr, 0, 4) <> "200 " Then
                        RaiseException("Cannot switch to Binary type!", "FTP.Upload() " & mStr)
                    End If
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then
                        mTCP2Client = New TcpClient(mHost, mPort2)
                        mFileAr = Split(LocalPathAndFileName, "\")

                        SendRequest(mNW1Stream, "STOR " & mFileAr(UBound(mFileAr)))
                        mStr = ""
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        '125 Data connection already open; Transfer starting. 
                        If InStr(mStr, "Transfer starting") > 0 Then
                            Try
                                ReDim mBytes(FileLen(LocalPathAndFileName))
                                mIO = File.OpenRead(LocalPathAndFileName)
                                mBr = mIO.Read(mBytes, 0, FileLen(LocalPathAndFileName))
                                mNW2Stream = mTCP2Client.GetStream()
                                mNW2Stream.WriteTimeout = TimeOut * 1000

                                mNW2Stream.Write(mBytes, 0, mBytes.Length - 1)

                                'Dim ii As Integer = 0
                                'For i As Integer = 0 To mBytes.Length - 1
                                '    ii += 1
                                '    mBc(ii - 1) = mBytes(i)
                                '    If ii >= (mSpeed) Or i >= mBytes.Length - 1 Then
                                '        ii = 0
                                '        mNW2Stream.Write(mBc, 0, mBc.Length - 1)
                                '    End If
                                'Next

                                'For Each mB As Byte In mBytes
                                '    mNW2Stream.WriteByte(mB)
                                'Next

                                mNW2Stream.Close()
                                mTCP2Client.Close()
                                mIO.Close()

                                'ReDim mBytes(FileLen(LocalPathAndFileName))
                                'mIO = File.OpenRead(LocalPathAndFileName)
                                'mBr = mIO.Read(mBytes, 0, FileLen(LocalPathAndFileName))

                                'mBin.Write(mBytes, 0, mBytes.Length - 1)
                                'mNW2Stream = mTCP2Client.GetStream()
                                'mBin.WriteTo(mNW2Stream)

                                'mNW2Stream.Close()
                                'mTCP2Client.Close()
                                'mIO.Close()
                            Catch Ex As Exception
                                Throw Ex
                            End Try
                            mSec = (Date.Now.Ticks / 10000000)
                            Do While Not mNW1Stream.DataAvailable
                                If Not mTCP1Client.Connected Then
                                    Exit Do
                                End If
                                If (Date.Now.Ticks / 10000000) > mSec + TimeOut Then
                                    Exit Do
                                End If
                            Loop
                            mStr = ""
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                            'Windows.Forms.Application.DoEvents()
                            'MsgBox(mStr)
                            If MySubStr(mStr, 0, 4) <> "226 " Then
                                RaiseException("Transfer failure!", "FTP.Upload() " & mStr)
                            Else
                                mRet = True
                            End If
                        Else
                            RaiseException("Transfer failure!", "FTP.Upload() " & mStr)
                        End If
                        'mStr = ""
                        'mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        'If MySubStr(mStr, 0, 4) <> "226 " Then
                        '    RaiseException("Transfer failure!", "FTP.Upload() " & mStr)
                        'End If
                        'mRet = True
                    End If
                End If
            Catch Ex As Exception
                Throw Ex
            Finally
                mBin = Nothing
                mStr = Nothing
                mIO = Nothing
                mBytes = Nothing
                mFileAr = Nothing
                mBc(mSpeed) = Nothing
            End Try
            Return mRet
        End Function

        Public Function ExecCommand(ByVal pCommand As String) As String
            SendRequest(mNW1Stream, pCommand)
            Return GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
        End Function

        Public Function CD(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "CWD " & cName)
            Dim mRet As String = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
            Return (MySubStr(mRet, 0, 4) = "250 ")
        End Function

        Public Function CD() As String
            Dim mRet As String = ""
            SendRequest(mNW1Stream, "PWD ")
            mRet = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
            If MySubStr(mRet, 0, 4) = "257 " Then
                mRet = Replace(Split(mRet, " ")(1), """", "")
            Else
                RaiseException("CD command failed!", "FTP.CD() " & mRet)
            End If
            Return mRet
        End Function

        Public Function CreateDir(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "MKD " & cName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "257 ")
        End Function

        Public Function RemoveDir(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "RMD " & cName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
        End Function

        Public Function RenameFile(ByVal cOldName As String, ByVal cNewName As String) As Boolean
            Dim mRet As Boolean
            SendRequest(mNW1Stream, "RNFR " & cOldName)
            mRet = (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "350 ")
            SendRequest(mNW1Stream, "RNTO " & cNewName)
            mRet = mRet And (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
            Return mRet
        End Function

        Public Function DeleteFile(ByVal cFileName As String) As Boolean
            SendRequest(mNW1Stream, "DELE " & cFileName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
        End Function

        Private Function FAT(ByVal pArray() As String) As String()
            Dim mArr(3) As String
            Dim mCount As Integer = 0
            Dim mDate As String = ""

            For cnt As Integer = 0 To UBound(pArray)
                If pArray(cnt).Length <> 0 Then
                    mCount = mCount + 1
                    Select Case mCount
                        Case 1 'date
                            mDate = Split(pArray(cnt), "-")(1) & "/" & MyCMonth(Split(pArray(cnt), "-")(0)) & "/" & Split(pArray(cnt), "-")(2)
                        Case 2 'time
                            mArr(0) = mDate & " " & pArray(cnt)
                        Case 3 'type/size
                            If IsNumeric(pArray(cnt)) Then
                                mArr(1) = "FILE"
                                mArr(2) = pArray(cnt)
                            Else
                                mArr(1) = "DIR"
                                mArr(2) = ""
                            End If
                        Case 4 'filename 
                            For cnt1 As Integer = cnt To UBound(pArray)
                                mArr(3) = mArr(3) & pArray(cnt1) & " "
                            Next
                            mArr(3) = LTrim(RTrim(mArr(3)))
                    End Select
                End If
            Next
            Return mArr
        End Function


        Private Function ExtractPasvPortNo(ByVal pString As String) As Integer
            Dim mAr() As String
            Dim mRet As Integer = 0
            Dim mP1 As String = ""
            Dim mP2 As String = ""
            'MsgBox(pString)
            If pString.Length > 0 Then
                Try
                    mAr = Split(pString, ",")
                    If mAr.Length >= 2 Then
                        mP1 = mAr(mAr.Length - 2)
                        mP2 = Replace(mAr(mAr.Length - 1), ")", "")
                        mP2 = Replace(mP2, ".", "")
                        If IsNumeric(mP1) And IsNumeric(mP2) Then
                            mRet = (mP1 * 256) + mP2
                        End If
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRet
            End If
        End Function

        Private Function GetResponse(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, ByVal pTimeOut As Integer) As String
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As StringBuilder = New StringBuilder()
            Dim mBytesRead As Integer = 0
            Dim mSec As Long
            Try
                mSec = (Date.Now.Ticks / 10000000)
                If pNetWorkStream.CanRead Then
                    Do While mInData.ToString = ""
                        Do While pNetWorkStream.DataAvailable
                            mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                            mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead))
                        Loop
                        If (Date.Now.Ticks / 10000000) > mSec + pTimeOut Then
                            Exit Do
                        End If
                    Loop
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mInData.ToString
        End Function

        Private Function GetResponse2Port(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, ByVal pTimeOut As Integer) As String
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As StringBuilder = New StringBuilder()
            Dim mBytesRead As Integer = 0
            Try
                If pNetWorkStream.CanRead Then
                    pNetWorkStream.ReadTimeout = IIf(pTimeOut = 0, 1, pTimeOut * 1000)
                    Do
                        mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                        mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead))
                    Loop While pNetWorkStream.DataAvailable Or mBytesRead > 0
                End If
            Catch ex As Exception
                Err.Clear()
            End Try
            Return mInData.ToString
        End Function

        Private Function GetResponse2PortBin(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, ByVal pTimeOut As Integer) As MemoryStream
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As New MemoryStream()
            Dim mBytesRead As Integer = 0
            Try
                If pNetWorkStream.CanRead Then
                    pNetWorkStream.ReadTimeout = IIf(pTimeOut = 0, 1, pTimeOut * 1000)
                    Do
                        mBytesRead = 0
                        Try
                            mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                        Catch ex As Exception
                            Err.Clear()
                        Finally
                            If mBytesRead > 0 Then
                                mInData.Write(mReadBuffer, 0, mBytesRead)
                            End If
                        End Try
                    Loop While pNetWorkStream.DataAvailable Or mBytesRead > 0
                End If
            Catch ex As Exception
                Err.Clear()
            End Try
            Return mInData
        End Function

        Private Function SendRequest(ByVal pNetWorkStream As NetworkStream, ByVal pRequest As String) As Boolean
            Dim mRet As Boolean = False
            Dim mStr As String = Nothing
            Dim mBytes() As Byte
            Try
                mStr = pRequest & vbCrLf
                mBytes = Encoding.ASCII.GetBytes(mStr)
                pNetWorkStream.Write(mBytes, 0, mBytes.Length)
                mRet = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRet
        End Function

        Private Sub RaiseException(ByVal Message As String, ByVal Source As String, Optional ByVal ErrorNumber As Integer = 1, Optional ByVal HelpFile As String = "HelpFile.hlp")
            Err.Raise(vbObjectError + 512 + ErrorNumber, Source, Message, HelpFile) ', WidthHelp)
        End Sub

        Private Function MySubStr(ByVal value As String, ByVal StartIndex As Integer, ByVal Length As Integer) As String
            If value.Length > 0 Then
                Return value.Substring(StartIndex, Length)
            Else
                Return ""
            End If
        End Function

    End Class

    Public Class clsFtpInews
        Private mHost As String
        Private mPort As String
        Private mTimeOut As Integer = 20
        Private mTCP1Client, mTCP2Client As New TcpClient()
        Private mNW1Stream, mNW2Stream As NetworkStream
        Private mDirListItem As DirListItem
        Private mDirList As DirList

        Private Function FtpCommandHelp() As Boolean
            'www.nsftools.com site 

            'List of raw FTP commands
            '(Warning: this is a technical document, not necessary for most FTP use.) 

            'Note that commands marked with a * are not implemented in a number of FTP servers. 

            '        Common(commands)
            'ABOR - abort a file transfer 
            'CWD - change working directory 
            'DELE - delete a remote file 
            'LIST - list remote files 
            'MDTM - return the modification time of a file 
            'MKD - make a remote directory 
            'NLST - name list of remote directory 
            'PASS - send password 
            'PASV - enter passive mode 
            'PORT - open a data port 
            'PWD - print working directory 
            'QUIT - terminate the connection 
            'RETR - retrieve a remote file 
            'RMD - remove a remote directory 
            'RNFR - rename from 
            'RNTO - rename to 
            'SITE - site-specific commands 
            'SIZE - return the size of a file 
            'STOR - store a file on the remote host 
            'TYPE - set transfer type 
            'USER - send username 
            'Less common commands
            'ACCT* - send account information 
            'APPE - append to a remote file 
            'CDUP - CWD to the parent of the current directory 
            'HELP - return help on using the server 
            'MODE - set transfer mode 
            'NOOP - do nothing 
            'REIN* - reinitialize the connection 
            'STAT - return server status 
            'STOU - store a file uniquely 
            'STRU - set file transfer structure 
            'SYST - return system type 
            '        ABOR()
            'Syntax: ABOR()

            'Aborts a file transfer currently in progress. 

            'ACCT*
            'Syntax: ACCT(account - info)

            'This command is used to send account information on systems that require it. Typically sent after a PASS command. 

            '        ALLO()
            'Syntax: ALLO size [R max-record-size] 

            'Allocates sufficient storage space to receive a file. If the maximum size of a record also needs to be known, that is sent as a second numeric parameter following a space, the capital letter "R", and another space. 

            '        APPE()
            'Syntax: APPE(remote - filename)

            'Append data to the end of a file on the remote host. If the file does not already exist, it is created. This command must be preceded by a PORT or PASV command so that the server knows where to receive data from. 

            '        CDUP()
            'Syntax: CDUP()

            'Makes the parent of the current directory be the current directory. 

            '        CWD()
            'Syntax: CWD(remote - Directory)

            'Makes the given directory be the current directory on the remote host. 

            '        DELE()
            'Syntax: DELE(remote - filename)

            'Deletes the given file on the remote host. 

            '        HELP()
            'Syntax: HELP([Command])

            'If a command is given, returns help on that command; otherwise, returns general help for the FTP server (usually a list of supported commands). 

            '            List()
            'Syntax: LIST [remote-filespec] 

            'If remote-filespec refers to a file, sends information about that file. If remote-filespec refers to a directory, sends information about each file in that directory. remote-filespec defaults to the current directory. This command must be preceded by a PORT or PASV command. 

            '                MDTM()
            'Syntax:         MDTM(remote - filename)

            'Returns the last-modified time of the given file on the remote host in the format "YYYYMMDDhhmmss": YYYY is the four-digit year, MM is the month from 01 to 12, DD is the day of the month from 01 to 31, hh is the hour from 00 to 23, mm is the minute from 00 to 59, and ss is the second from 00 to 59. 

            '                MKD()
            'Syntax:         MKD(remote - Directory)

            'Creates the named directory on the remote host. 

            '                MODE()
            'Syntax:         MODE(mode - character)

            'Sets the transfer mode to one of: 
            '                S(-Stream)
            '                B(-Block)
            '                C(-Compressed)
            'The default mode is Stream. 

            '                NLST()
            'Syntax: NLST [remote-directory] 

            'Returns a list of filenames in the given directory (defaulting to the current directory), with no other information. Must be preceded by a PORT or PASV command. 

            '                NOOP()
            'Syntax:         NOOP()

            'Does nothing except return a response. 

            '                PASS()
            'Syntax:         PASS(password)

            'After sending the USER command, send this command to complete the login process. (Note, however, that an ACCT command may have to be used on some systems.) 

            '                PASV()
            'Syntax:         PASV()

            'Tells the server to enter "passive mode". In passive mode, the server will wait for the client to establish a connection with it rather than attempting to connect to a client-specified port. The server will respond with the address of the port it is listening on, with a message like:
            '227 Entering Passive Mode (a1,a2,a3,a4,p1,p2)
            'where a1.a2.a3.a4 is the IP address and p1*256+p2 is the port number. 

            '                PORT()
            'Syntax:         PORT(a1, a2, a3, a4, p1, p2)

            'Specifies the host and port to which the server should connect for the next file transfer. This is interpreted as IP address a1.a2.a3.a4, port p1*256+p2. 

            '                PWD()
            'Syntax:         PWD()

            'Returns the name of the current directory on the remote host. 

            '                QUIT()
            'Syntax:         QUIT()

            'Terminates the command connection. 

            'REIN*
            'Syntax:         REIN()

            'Reinitializes the command connection - cancels the current user/password/account information. Should be followed by a USER command for another login. 

            '                REST()
            'Syntax:         REST(position)

            'Sets the point at which a file transfer should start; useful for resuming interrupted transfers. For nonstructured files, this is simply a decimal number. This command must immediately precede a data transfer command (RETR or STOR only); i.e. it must come after any PORT or PASV command. 

            '                RETR()
            'Syntax:         RETR(remote - filename)

            'Begins transmission of a file from the remote host. Must be preceded by either a PORT command or a PASV command to indicate where the server should send data. 

            '                RMD()
            'Syntax:         RMD(remote - Directory)

            'Deletes the named directory on the remote host. 

            '                RNFR()
            'Syntax:         RNFR(from - filename)

            'Used when renaming a file. Use this command to specify the file to be renamed; follow it with an RNTO command to specify the new name for the file. 

            '                RNTO()
            'Syntax: RNTO to-filename 

            'Used when renaming a file. After sending an RNFR command to specify the file to rename, send this command to specify the new name for the file. 

            'SITE*
            'Syntax:         SITE(site - specific - Command())

            'Executes a site-specific command. 

            '                SIZE()
            'Syntax:         SIZE(remote - filename)

            'Returns the size of the remote file as a decimal number. 

            '                STAT()
            'Syntax: STAT [remote-filespec] 

            'If invoked without parameters, returns general status information about the FTP server process. If a parameter is given, acts like the LIST command, except that data is sent over the control connection (no PORT or PASV command is required). 

            '                    STOR()
            'Syntax:             STOR(remote - filename)

            'Begins transmission of a file to the remote site. Must be preceded by either a PORT command or a PASV command so the server knows where to accept data from. 

            '                    STOU()
            'Syntax:             STOU()

            'Begins transmission of a file to the remote site; the remote filename will be unique in the current directory. The response from the server will include the filename. 

            '                    STRU()
            'Syntax: STRU structure-character 

            'Sets the file structure for transfer to one of: 
            'F - File (no structure) 
            'R - Record structure 
            'P - Page structure 
            'The default structure is File. 

            '                    SYST()
            'Syntax:             SYST()

            'Returns a word identifying the system, the word "Type:", and the default transfer type (as would be set by the TYPE command). For example: UNIX Type: L8 

            '                    Type()
            'Syntax: TYPE type-character [second-type-character] 

            'Sets the type of file to be transferred. type-character can be any of: 
            'A - ASCII text 
            'E - EBCDIC text 
            'I - image (binary data) 
            'L - local format 
            'For A and E, the second-type-character specifies how the text should be interpreted. It can be: 
            'N - Non-print (not destined for printing). This is the default if second-type-character is omitted. 
            'T - Telnet format control (<CR>, <FF>, etc.) 
            'C - ASA Carriage Control 
            'For L, the second-type-character specifies the number of bits per byte on the local system, and may not be omitted. 

            '                            USER()
            'Syntax:                     USER(username)

            'Send this command to begin the login process. username should be a valid username on the system, or "anonymous" to initiate an anonymous login.
        End Function

        Public Class DirList
            Private mD() As DirListItem

            Public Function Add(ByVal pDirlistItem As DirListItem) As Boolean
                If mD Is Nothing Then
                    ReDim mD(0)
                Else
                    ReDim Preserve mD(UBound(mD) + 1)
                End If
                mD(UBound(mD)) = pDirlistItem
            End Function

            Public Function Clear() As Boolean
                ReDim mD(0)
            End Function

            Public ReadOnly Property Rows(ByVal index As Integer) As DirListItem
                Get
                    Return mD(index)
                End Get
            End Property

            Public ReadOnly Property Count() As Integer
                Get
                    If mD Is Nothing Then
                        Return 0
                    Else
                        Return mD.Length '  UBound(mD)
                    End If
                End Get
            End Property

            Protected Overrides Sub Finalize()
                mD = Nothing
                MyBase.Finalize()
            End Sub

        End Class

        Public Class DirListItem
            Private mDateTime As Date
            Private mType As EnumFileType
            Private mSize As Integer
            Private mFileName As String
            Public Property DateTime() As Date
                Get
                    Return mDateTime
                End Get
                Set(ByVal value As Date)
                    mDateTime = value
                End Set
            End Property
            Public Property Type() As EnumFileType
                Get
                    Return mType
                End Get
                Set(ByVal value As EnumFileType)
                    mType = value
                End Set
            End Property
            Public Property Size() As Integer
                Get
                    Return mSize
                End Get
                Set(ByVal value As Integer)
                    mSize = value
                End Set
            End Property
            Public Property FileName() As String
                Get
                    Return mFileName
                End Get
                Set(ByVal value As String)
                    mFileName = value
                End Set
            End Property
            Protected Overrides Sub Finalize()
                mDateTime = Nothing
                mType = Nothing
                mSize = Nothing
                mFileName = Nothing
                MyBase.Finalize()
            End Sub
        End Class

        Public Enum EnumFileType
            _Directory = 1
            _File = 2
        End Enum

        Public ReadOnly Property Connected() As Boolean
            Get
                Return mTCP1Client.Connected
            End Get
        End Property

        Public Property TimeOut() As Integer
            Get
                Return mTimeOut
            End Get
            Set(ByVal value As Integer)
                mTimeOut = value
            End Set
        End Property

        Public ReadOnly Property Host() As String
            Get
                Return mHost
            End Get
        End Property

        Public Function Connect(ByVal Host As String, ByVal Port As Integer, ByVal UID As String, ByVal PWD As String, Optional ByVal WaitForActualResponse As Boolean = False) As Boolean
            Dim mStr As String = Nothing
            Try
                mHost = Host
                mPort = Port
                mTCP1Client.Close()
                mTCP1Client = New TcpClient()
                mTCP1Client.Connect(Host, Port)
                mNW1Stream = mTCP1Client.GetStream()
                mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut, IIf(WaitForActualResponse, "220", ""))
                If InStr(mStr, "220") > 0 Then
                    Try
                        SendRequest(mNW1Stream, "USER " & UID)
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut, IIf(WaitForActualResponse, "331", ""))
                        If InStr(mStr, "331") > 0 Then
                            SendRequest(mNW1Stream, "PASS " & PWD)
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut, IIf(WaitForActualResponse, "230", ""))
                            If InStr(mStr, "230") = 0 Then
                                mTCP1Client.Close()
                                RaiseException("Incorrect password!. " & mStr, "FTP.Connect()")
                            End If
                        Else
                            mTCP1Client.Close()
                            RaiseException("Server rejected user " & UID & "!. " & mStr, "FTP.Connect()")
                        End If
                    Catch ex As Exception
                        mTCP1Client.Close()
                        Throw ex
                    End Try
                Else
                    mTCP1Client.Close()
                    RaiseException("Could not connect to " & Host & " at port " & Port & ": " & mStr, "FTP.Connect()")
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mTCP1Client.Connected
        End Function

        Public Function Close() As Boolean
            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "QUIT")
                    mTCP1Client.Close()
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            Return Not mTCP1Client.Connected
        End Function

        Public Function Dir(Optional ByVal pDirectory As String = "..", Optional ByVal ExcludeDirs As Boolean = False) As DirList
            Dim mMS As New MemoryStream()
            Dim mStr, mStr2 As String
            Dim mPort2 As Integer
            Dim mRet As String = Nothing
            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then
                        mTCP2Client = New TcpClient(mHost, mPort2)
                        If pDirectory = ".." Then
                            mStr2 = "LIST -aL" & vbCrLf
                        Else
                            mStr2 = "LIST " & pDirectory & vbCrLf
                        End If
                        mStr = ""
                        SendRequest(mNW1Stream, mStr2)
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        If InStr(mStr, "125") = 0 Then
                            RaiseException("Data Transfer error!", "FTP.Dir()")
                        Else
                            Try
                                mNW2Stream = mTCP2Client.GetStream()
                                mRet = GetResponse2Port(mTCP2Client, mNW2Stream)
                                mTCP2Client.Close()
                            Catch Ex As Exception
                                Throw Ex
                            End Try
                            mStr = ""
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                            If InStr(mStr, "226") = 0 Then
                                RaiseException("Data transfer not completed!", "FTP.Dir() " & mStr)
                            End If
                        End If
                    End If
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            mDirList = New DirList
            If mRet.Length > 0 Then
                Dim mAr() As String
                mAr = Split(mRet, vbCrLf)
                For cnt As Integer = 1 To UBound(mAr)
                    Dim mM() As String
                    mM = FAT(mAr(cnt - 1).Split(" "))
                    If mM(1) = "DIR" Then
                        If Not ExcludeDirs Then
                            mDirListItem = New DirListItem
                            mDirListItem.DateTime = mM(0)
                            mDirListItem.Type = EnumFileType._Directory
                            mDirListItem.Size = MyCint(mM(2))
                            mDirListItem.FileName = mM(3)
                            mDirList.Add(mDirListItem)
                            mDirListItem = Nothing
                        End If
                    Else
                        mDirListItem = New DirListItem
                        mDirListItem.DateTime = mM(0)
                        mDirListItem.Type = EnumFileType._File
                        mDirListItem.Size = MyCint(mM(2))
                        mDirListItem.FileName = mM(3)
                        mDirList.Add(mDirListItem)
                        mDirListItem = Nothing
                    End If
                    mM = Nothing
                Next
            End If
            Return mDirList
        End Function

        Private Function MyCint(ByVal value As String) As Integer
            If IsNumeric(value) Then
                Return value
            Else
                Return 0
            End If
        End Function

        Public Function Download(ByVal FtpSiteFileName As String, ByVal LocalPathAndFileName As String) As Boolean
            Dim mRet As Boolean = False
            Dim mBin As New MemoryStream()
            Dim mStr As String
            Dim mPort2 As Integer
            Dim mIO As FileStream
            Dim mBytes() As Byte

            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "TYPE I")
                    mStr = ""
                    mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                    If InStr(mStr, "200") = 0 Then
                        RaiseException("Cannot switch to Binary type!", "FTP.Download() " & mStr)
                    End If
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then
                        mTCP2Client = New TcpClient(mHost, mPort2)
                        SendRequest(mNW1Stream, "RETR " & FtpSiteFileName)
                        mStr = ""
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        Try
                            mNW2Stream = mTCP2Client.GetStream()
                            mBin = GetResponse2PortBin(mTCP2Client, mNW2Stream)
                            mTCP2Client.Close()
                        Catch Ex As Exception
                            Throw Ex
                        End Try
                        mBytes = mBin.ToArray()
                        mIO = File.OpenWrite(LocalPathAndFileName)
                        mIO.Write(mBytes, 0, mBytes.Length)
                        mIO.Close()
                        If InStr(mStr, "226") = 0 Then
                            mStr = ""
                            mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                            If InStr(mStr, "226 ") = 0 Then
                                RaiseException("Transfer failure!", "FTP.Download() " & mStr)
                            End If
                        End If
                        mRet = True
                    End If
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            Return mRet
        End Function

        Public Function Upload(ByVal LocalPathAndFileName As String) As Boolean
            Dim mRet As Boolean = False
            Dim mBin As New MemoryStream()
            Dim mStr As String
            Dim mPort2 As Integer
            Dim mIO As FileStream
            Dim mBytes() As Byte
            Dim mBr As Integer
            Dim mFileAr() As String
            If mTCP1Client.Connected Then
                Try
                    SendRequest(mNW1Stream, "TYPE I")
                    mStr = ""
                    mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                    If InStr(mStr, "200") = 0 Then
                        RaiseException("Cannot switch to Binary type!", "FTP.Download() " & mStr)
                    End If
                    SendRequest(mNW1Stream, "PASV")
                    mPort2 = ExtractPasvPortNo(GetResponse(mTCP1Client, mNW1Stream, mTimeOut))
                    If mPort2 > 0 Then
                        mTCP2Client = New TcpClient(mHost, mPort2)
                        mFileAr = Split(LocalPathAndFileName, "\")

                        SendRequest(mNW1Stream, "STOR " & mFileAr(UBound(mFileAr)))
                        mStr = ""
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        Try
                            ReDim mBytes(FileLen(LocalPathAndFileName))
                            mIO = File.OpenRead(LocalPathAndFileName)
                            mBr = mIO.Read(mBytes, 0, FileLen(LocalPathAndFileName))
                            mBin.Write(mBytes, 0, mBytes.Length - 1)
                            mNW2Stream = mTCP2Client.GetStream()
                            mBin.WriteTo(mNW2Stream)
                            mNW2Stream.Close()
                            mTCP2Client.Close()
                            mIO.Close()
                        Catch Ex As Exception
                            Throw Ex
                        End Try
                        mStr = ""
                        mStr = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
                        If InStr(mStr, "226") = 0 Then
                            RaiseException("Transfer failure!", "FTP.Download() " & mStr)
                        End If
                        mRet = True
                    End If
                Catch Ex As Exception
                    Throw Ex
                End Try
            End If
            Return mRet
        End Function

        Public Function CD(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "CWD " & cName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
        End Function

        Public Function CD() As String
            Dim mRet As String = ""
            SendRequest(mNW1Stream, "PWD ")
            mRet = GetResponse(mTCP1Client, mNW1Stream, mTimeOut)
            If InStr(mRet, "257") > 0 Then
                mRet = Replace(Split(mRet, " ")(1), """", "")
            Else
                RaiseException("CD command failed!", "FTP.CD() " & mRet)
            End If
            Return mRet
        End Function

        Public Function CreateDir(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "MKD " & cName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "257 ")
        End Function

        Public Function RemoveDir(ByVal cName As String) As Boolean
            SendRequest(mNW1Stream, "RMD " & cName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
        End Function

        Public Function RenameFile(ByVal cOldName As String, ByVal cNewName As String) As Boolean
            Dim mRet As Boolean
            SendRequest(mNW1Stream, "RNFR " & cOldName)
            mRet = (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "350 ")
            SendRequest(mNW1Stream, "RNTO " & cNewName)
            mRet = mRet And (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
            Return mRet
        End Function

        Public Function DeleteFile(ByVal cFileName As String) As Boolean
            SendRequest(mNW1Stream, "DELE " & cFileName)
            Return (MySubStr(GetResponse(mTCP1Client, mNW1Stream, mTimeOut), 0, 4) = "250 ")
        End Function

        Private Function FAT(ByVal pArray() As String) As String()
            Dim mArr(3) As String
            Dim mCount As Integer = 0
            Dim mDate As String = ""

            For cnt As Integer = 0 To UBound(pArray)
                If pArray(cnt).Length <> 0 Then
                    mCount = mCount + 1
                    Select Case mCount
                        Case 1 'date
                            mDate = Split(pArray(cnt), "-")(1) & "/" & cMonth(Split(pArray(cnt), "-")(0)) & "/" & Split(pArray(cnt), "-")(2)
                        Case 2 'time
                            mArr(0) = mDate & " " & pArray(cnt)
                        Case 3 'type/size
                            If IsNumeric(pArray(cnt)) Then
                                mArr(1) = "FILE"
                                mArr(2) = pArray(cnt)
                            Else
                                mArr(1) = "DIR"
                                mArr(2) = ""
                            End If
                        Case 4 'filename 
                            For cnt1 As Integer = cnt To UBound(pArray)
                                mArr(3) = mArr(3) & pArray(cnt1) & " "
                            Next
                            mArr(3) = LTrim(RTrim(mArr(3)))
                    End Select
                End If
            Next
            Return mArr
        End Function

        Private Function cMonth(ByVal value As String) As String
            Dim mRet As String = value
            If IsNumeric(value) Then
                Select Case CInt(value)
                    Case 1
                        mRet = "JAN"
                    Case 2
                        mRet = "FEB"
                    Case 3
                        mRet = "MAR"
                    Case 4
                        mRet = "APR"
                    Case 5
                        mRet = "MAY"
                    Case 6
                        mRet = "JUN"
                    Case 7
                        mRet = "JUL"
                    Case 8
                        mRet = "AUG"
                    Case 9
                        mRet = "SEP"
                    Case 10
                        mRet = "OCT"
                    Case 11
                        mRet = "NOV"
                    Case 12
                        mRet = "DEC"
                End Select
            End If
            Return mRet
        End Function

        Private Function ExtractPasvPortNo(ByVal pString As String) As Integer
            Dim mAr() As String
            Dim mRet As Integer = 0
            Dim mP1 As String = ""
            Dim mP2 As String = ""
            'MsgBox(pString)
            If pString.Length > 0 Then
                Try
                    mAr = Split(pString, ",")
                    If mAr.Length >= 2 Then
                        mP1 = mAr(mAr.Length - 2)
                        mP2 = Replace(mAr(mAr.Length - 1), ")", "")
                        mP2 = Replace(mP2, ".", "")
                        If IsNumeric(mP1) And IsNumeric(mP2) Then
                            mRet = (mP1 * 256) + mP2
                        End If
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                Return mRet
            End If
        End Function

        Private Function GetResponse(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, Optional ByVal pTimeOut As Integer = 0, Optional ByVal WaitForResponseContain As String = "") As String
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As StringBuilder = New StringBuilder()
            Dim mBytesRead As Integer = 0
            Dim mSec As Long
            Try
                mSec = (Date.Now.Ticks / 10000000)
                If pNetWorkStream.CanRead Then
                    If WaitForResponseContain <> "" Then
                        Do While InStr(mInData.ToString, WaitForResponseContain) = 0
                            Do While pNetWorkStream.DataAvailable
                                mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                                mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead))
                            Loop
                            If (Date.Now.Ticks / 10000000) > mSec + pTimeOut Then
                                Exit Do
                            End If
                        Loop
                    Else
                        Do While mInData.ToString = ""
                            Do While pNetWorkStream.DataAvailable
                                mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                                mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead))
                            Loop
                            If (Date.Now.Ticks / 10000000) > mSec + pTimeOut Then
                                Exit Do
                            End If
                        Loop
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return mInData.ToString
        End Function

        Private Function GetResponse2Port(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, Optional ByVal pTimeOut As Integer = 0) As String
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As StringBuilder = New StringBuilder()
            Dim mBytesRead As Integer = 0
            Try
                If pNetWorkStream.CanRead Then
                    pNetWorkStream.ReadTimeout = IIf(pTimeOut = 0, 1, pTimeOut)
                    Do
                        mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                        mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead))
                    Loop While pNetWorkStream.DataAvailable Or mBytesRead > 0
                End If
            Catch ex As Exception
                Err.Clear()
            End Try
            Return mInData.ToString
        End Function

        Private Function GetResponse2PortBin(ByVal pTcpClient As TcpClient, ByVal pNetWorkStream As NetworkStream, Optional ByVal pTimeOut As Integer = 0) As MemoryStream
            Dim mReadBuffer(pTcpClient.ReceiveBufferSize) As Byte
            Dim mInData As New MemoryStream()
            Dim mBytesRead As Integer = 0
            Try
                If pNetWorkStream.CanRead Then
                    pNetWorkStream.ReadTimeout = IIf(pTimeOut = 0, 1, pTimeOut)
                    Do
                        mBytesRead = pNetWorkStream.Read(mReadBuffer, 0, mReadBuffer.Length)
                        mInData.Write(mReadBuffer, 0, mBytesRead)
                    Loop While pNetWorkStream.DataAvailable Or mBytesRead > 0
                End If
            Catch ex As Exception
                Err.Clear()
            End Try
            Return mInData
        End Function

        Private Function SendRequest(ByVal pNetWorkStream As NetworkStream, ByVal pRequest As String) As Boolean
            Dim mRet As Boolean = False
            Dim mStr As String = Nothing
            Dim mBytes() As Byte
            Try
                mStr = pRequest & vbCrLf
                mBytes = Encoding.ASCII.GetBytes(mStr)
                pNetWorkStream.Write(mBytes, 0, mBytes.Length)
                mRet = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRet
        End Function

        Private Sub RaiseException(ByVal Message As String, ByVal Source As String, Optional ByVal ErrorNumber As Integer = 1, Optional ByVal HelpFile As String = "HelpFile.hlp")
            Err.Raise(vbObjectError + 512 + ErrorNumber, Source, Message, HelpFile) ', WidthHelp)
        End Sub

        Private Function MySubStr(ByVal value As String, ByVal StartIndex As Integer, ByVal Length As Integer) As String
            If value.Length > 0 Then
                Return value.Substring(StartIndex, Length)
            Else
                Return ""
            End If
        End Function

    End Class

End Namespace
