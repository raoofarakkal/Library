Namespace Library2.Sys

    Public Class SystemInformation

        Public Sub New()
        End Sub

        Public Sub New(ByVal pMachineIP As String, ByVal pUsername As String, ByVal pPassword As String)
            MachineIP = pMachineIP
            UserName = pUsername
            Password = pPassword
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private mMachineIP As String = "."

        Public Property MachineIP() As String
            Get
                Return mMachineIP
            End Get
            Set(ByVal value As String)
                mMachineIP = value
            End Set
        End Property

        Private mUID As String = ""
        Public Property UserName() As String
            Get
                Return mUID
            End Get
            Set(ByVal value As String)
                mUID = value
            End Set
        End Property

        Private mPWD As String = ""
        Public Property Password() As String
            Get
                Return mPWD
            End Get
            Set(ByVal value As String)
                mPWD = value
            End Set
        End Property

        Public Function GetSerialNumber() As String
            Return ExecuteWmicCommand("bios get serialnumber")
        End Function

        Public Function GetName() As String
            Return ExecuteWmicCommand("csproduct get name")
        End Function

        Public Function GetVendor() As String
            Return ExecuteWmicCommand("csproduct get vendor")
        End Function

        Public Function GetIdentifyingNumber() As String
            Return ExecuteWmicCommand("csproduct get identifyingnumber")
        End Function

        Public Function GetLogicalDiskInfo() As String
            Return ExecuteWmicCommand("logicaldisk WHERE drivetype=3 GET name,freespace,SystemName,FileSystem,Size,VolumeSerialNumber")
        End Function

        Public Function GetWmicCommandReferenceUrl() As Boolean
            Return "http://support.microsoft.com/servicedesks/webcasts/wc072402/listofsampleusage.asp"
        End Function

        Public Function ExecuteWmicCommand(ByVal WMIC_Command As String) As String
            Dim mRet As String = ""
            'WMIC /node:"10.10.71.201" /user:"ajfas" /password:"12345" csproduct get vendor,name,identifyingnumber
            Dim mPro As New Process
            Dim mCmdLine As [String] = "WMIC"
            mPro.StartInfo = New ProcessStartInfo(mCmdLine)
            mPro.StartInfo.UseShellExecute = False 'false
            If MachineIP = "." Then
                mPro.StartInfo.Arguments = String.Format(" {3}", MachineIP, UserName, Password, WMIC_Command)
            Else
                mPro.StartInfo.Arguments = String.Format(" /node:""{0}"" /user:""{1}"" /password:""{2}"" {3}", MachineIP, UserName, Password, WMIC_Command)
            End If
            mPro.StartInfo.CreateNoWindow = True
            mPro.StartInfo.RedirectStandardOutput = True
            mPro.Start()
            mPro.WaitForExit()
            mRet = mPro.StandardOutput.ReadToEnd()
            Return mRet
        End Function
    End Class

End Namespace