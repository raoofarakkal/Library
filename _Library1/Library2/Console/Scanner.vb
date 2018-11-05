'Imports System.IO
'''' <summary>
'''' Resource collected from http://www.xtremevbtalk.com/showthread.php?t=288758
'''' </summary>
'''' <remarks></remarks>
'Namespace Library2.Console

'    Public Class Scanner
'        Inherits _Library._Base.LibraryBase

'        Private Const mHe As String = "Horizontal Extent"
'        Private Const mVe As String = "Vertical Extent"
'        Private Const mHr As String = "Horizontal Resolution"
'        Private Const mVr As String = "Vertical Resolution"
'        Private Const mFeederPropName As String = "Document Handling Select"

'        Private WithEvents mWiaDevcMgr As WIA.DeviceManager
'        Private mWiaDialog As New WIA.CommonDialog
'        Private mWiaImg As New WIA.ImageFile
'        Private mWiaDevc As WIA.Device
'        Private mConfigFile As String
'        ''' <summary>
'        ''' use Scanner.WiaImageConverter class to convert Wia ImageFile to system.drawing.image 
'        ''' </summary>
'        ''' <param name="pImage"></param>
'        ''' <remarks></remarks>
'        Public Event OnPageComplete(ByVal pImage As WIA.ImageFile)
'        Public Event OnScanningComplete(ByVal Count As Integer)
'        Public Event OnScanningError(ByVal Message As String)

'        Public Sub _New()
'            mWiaDevcMgr = New WIA.DeviceManager
'            mWiaDialog = New WIA.CommonDialog
'            mWiaImg = New WIA.ImageFile
'        End Sub

'        Public Sub New(ByVal pScannerConfigFile As String)
'            mConfigFile = pScannerConfigFile
'            _New()
'        End Sub

'        Public Sub Dispose()
'            Disconnect()
'            mWiaDevcMgr = Nothing
'            mWiaDialog = Nothing
'            mWiaImg = Nothing
'        End Sub

'        Protected Overrides Sub Finalize()
'            MyBase.Finalize()
'        End Sub

'        'Public Class ScannerConfiguration
'        '    Inherits DataSet
'        'End Class

'        Dim mScannerConfig As Data.DataSet = Nothing
'        Public Function ScannerConfig() As Data.DataSet
'            If mScannerConfig Is Nothing Then
'                If File.Exists(mConfigFile) Then
'                    mScannerConfig = New Data.DataSet
'                    Try
'                        mScannerConfig.ReadXml(mConfigFile)
'                    Catch ex As Exception
'                        mScannerConfig = Nothing
'                    End Try
'                End If
'            End If
'            Return mScannerConfig
'        End Function

'        Private mJpegCompression As Long = 50
'        ''' <summary>
'        ''' value between 1-100 %, default value is 50%
'        ''' </summary>
'        ''' <remarks></remarks>
'        Public Property JpegCompression() As Long
'            Get
'                Return mJpegCompression
'            End Get
'            Set(ByVal value As Long)
'                If value > 0 And value <= 100 Then
'                    mJpegCompression = value
'                End If
'            End Set
'        End Property

'        ''' <summary>
'        ''' SelectScanner() Return DeviceID
'        ''' </summary>
'        ''' <returns></returns>
'        ''' <remarks></remarks>
'        Public Function SelectScanner() As String
'            Dim mRet As String
'            Dim mDialog = New WIA.CommonDialog
'            mWiaDevc = mDialog.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, True, False)
'            mRet = mWiaDevc.DeviceID
'            mDialog = Nothing
'            Return mRet
'        End Function

'        Public Function Connected() As Boolean
'            Dim mRet As Boolean = False
'            If mWiaDevc IsNot Nothing Then
'                mRet = Not mWiaDevc.DeviceID = ""
'            End If
'            Return mRet
'        End Function

'        Private Function Disconnect() As Boolean
'            mWiaDevc = Nothing
'        End Function

'        Public Function Connect() As String
'            Return Connect(SelectScanner())
'        End Function

'        Public Function Connect(ByVal DeviceID As String) As Boolean
'            Dim mRet As Boolean = False
'            If mWiaDevc Is Nothing Then
'                If DeviceID = "" Then
'                    Throw New Exception("Connect() failed. Device ID required. use SelectScanner() method for DeviceID")
'                Else
'                    For Each mInfo As WIA.DeviceInfo In mWiaDevcMgr.DeviceInfos
'                        If (mInfo.DeviceID = DeviceID) Then
'                            mWiaDevc = mInfo.Connect
'                            For Each mEvents As WIA.DeviceEvent In mWiaDevc.Events
'                                mWiaDevcMgr.RegisterEvent(mEvents.EventID)
'                            Next
'                            If mWiaDevc Is Nothing Then
'                                mRet = True
'                            End If
'                            Exit For
'                        End If
'                    Next
'                End If
'            Else
'                mRet = True
'            End If
'            Return mRet
'        End Function

'        Public Function GenerateScannerConfigFile(ByVal pConfigFile As String) As Boolean
'            If mWiaDevc Is Nothing Then
'                Throw New Exception("Scanner not connected")
'            End If
'            Dim mWiaPage As WIA.Item
'            Dim mWiaPages As WIA.Items = mWiaDevc.Items

'            Dim mDs As New Data.DataSet
'            Dim mDt As Data.DataTable
'            Dim mDr As Data.DataRow

'            mDt = New Data.DataTable
'            mDt.TableName = "DEVICE"
'            mDt.Columns.Add("PropertyID")
'            mDt.Columns.Add("Type")
'            mDt.Columns.Add("Name")
'            mDt.Columns.Add("Value")

'            For Each mProperty As WIA.Property In mWiaDevc.Properties
'                If Not mProperty.IsReadOnly Then
'                    Dim mSv As String = ""
'                    Dim mSc As String = ""
'                    Try
'                        For Each mS As Object In mProperty.SubTypeValues
'                            mSv += mSc & mS
'                            mSc = ";"
'                        Next
'                    Catch ex As Exception
'                    End Try
'                    mDr = mDt.NewRow
'                    mDr.Item("PropertyID") = mProperty.PropertyID
'                    mDr.Item("Type") = mProperty.Type
'                    mDr.Item("Name") = mProperty.Name
'                    mDr.Item("Value") = mProperty.Value

'                    mDt.Rows.Add(mDr)
'                    mDr = Nothing
'                End If
'            Next
'            mDs.Tables.Add(mDt)

'            For Each mWiaPage In mWiaPages
'                mDt = New Data.DataTable
'                mDt.TableName = "DOCUMENT"

'                mDt.Columns.Add("PropertyID")
'                mDt.Columns.Add("Type")
'                mDt.Columns.Add("Name")
'                mDt.Columns.Add("Value")

'                For Each mProperty As WIA.Property In mWiaPage.Properties
'                    If Not mProperty.IsReadOnly Then
'                        Dim mSv As String = ""
'                        Dim mSc As String = ""
'                        Try
'                            For Each mS As Object In mProperty.SubTypeValues
'                                mSv += mSc & mS
'                                mSc = ";"
'                            Next
'                        Catch ex As Exception
'                        End Try
'                        mDr = mDt.NewRow
'                        mDr.Item("PropertyID") = mProperty.PropertyID
'                        mDr.Item("Type") = mProperty.Type
'                        mDr.Item("Name") = mProperty.Name

'                        Dim mHRes As Integer = -1
'                        Dim mVRes As Integer = -1
'                        Select Case mProperty.Name
'                            Case mHr
'                                If mHRes = -1 Then
'                                    mHRes = mProperty.Value
'                                End If
'                                mDr.Item("Value") = mHRes
'                            Case mVr
'                                If mVRes = -1 Then
'                                    mVRes = mProperty.Value
'                                End If
'                                mDr.Item("Value") = mVRes
'                            Case mHe
'                                If mHRes = -1 Then
'                                    mHRes = 150
'                                End If
'                                Dim _he As Double = 8.3
'                                Try
'                                    _he = (CDbl(mProperty.Value) / mHRes).ToString("#0.00")
'                                Catch ex As Exception

'                                End Try
'                                mDr.Item("Value") = _he
'                            Case mVe
'                                If mVRes = -1 Then
'                                    mVRes = 150
'                                End If
'                                Dim _ve As Double = 11.7
'                                Try
'                                    _ve = (CDbl(mProperty.Value) / mVRes).ToString("#0.00")
'                                Catch ex As Exception

'                                End Try
'                                mDr.Item("Value") = _ve
'                            Case Else
'                                mDr.Item("Value") = mProperty.Value
'                        End Select



'                        mDt.Rows.Add(mDr)
'                    End If
'                Next
'                mDs.Tables.Add(mDt)
'                Exit For
'            Next

'            mDs.DataSetName = "SCANNER.WIA.PROPERTIES"
'            If Not File.Exists(pConfigFile) Then
'                Directory.CreateDirectory(_Library._IO.clsIO.SplitDirFileName(pConfigFile)(0))
'            End If
'            mDs.WriteXml(pConfigFile)
'        End Function

'        Public Enum _ScannerInput
'            _Default = 0
'            _Feeder = 1
'            _FlatBed = 2
'        End Enum

'        Public Function ScanPages(Optional ByVal pScannerInput As _ScannerInput = _ScannerInput._Default) As Boolean
'            If mWiaDevc Is Nothing Then
'                Throw New Exception("Scanner not connected")
'            End If
'            Dim mWiaPage As WIA.Item
'            Dim mWiaPages As WIA.Items = Nothing
'            Dim i As Integer = 0

'            mWiaPages = mWiaDevc.Items

'            For Each mProperty As WIA.Property In mWiaDevc.Properties
'                For Each mR As Data.DataRow In ScannerConfig.Tables("DEVICE").Select(String.Format("Name = '{0}'", mProperty.Name))
'                    If Not mProperty.IsReadOnly Then
'                        mProperty.Value = mR.Item("value").ToString()
'                    End If
'                    Exit For
'                Next

'                If mProperty.Name = mFeederPropName Then
'                    If pScannerInput = _ScannerInput._Feeder Then
'                        mProperty.Value = _ScannerInput._Feeder
'                    ElseIf pScannerInput = _ScannerInput._FlatBed Then
'                        mProperty.Value = _ScannerInput._FlatBed
'                    End If
'                End If
'            Next
'            For Each mWiaPage In mWiaPages

'                Dim Enough As Boolean = False
'                Do While Not Enough
'                    For Each mProperty As WIA.Property In mWiaPage.Properties
'                        For Each mR As Data.DataRow In ScannerConfig.Tables("DOCUMENT").Select(String.Format("Name = '{0}'", mProperty.Name))
'                            If Not mProperty.IsReadOnly Then
'                                Try
'                                    Select Case mProperty.Name
'                                        'Case mHr
'                                        '    mProperty.Value = mR.Item("value").ToString()
'                                        'Case mVr
'                                        '    For Each mRes As Data.DataRow In ScannerConfig.Tables("DOCUMENT").Select(String.Format("Name = '{0}'", mHr))
'                                        '        mProperty.Value = mRes.Item("value").ToString()
'                                        '        Exit For
'                                        '    Next
'                                        Case mHe
'                                            For Each mHRes As Data.DataRow In ScannerConfig.Tables("DOCUMENT").Select(String.Format("Name = '{0}'", mHr))
'                                                mProperty.Value = CDbl(mHRes.Item("value").ToString()) * CDbl(mR.Item("value").ToString())
'                                                Exit For
'                                            Next
'                                        Case mVe
'                                            For Each mVRes As Data.DataRow In ScannerConfig.Tables("DOCUMENT").Select(String.Format("Name = '{0}'", mVr))
'                                                mProperty.Value = CDbl(mVRes.Item("value").ToString()) * CDbl(mR.Item("value").ToString())
'                                                Exit For
'                                            Next
'                                        Case Else
'                                            mProperty.Value = mR.Item("value").ToString()
'                                    End Select
'                                Catch ex As Exception
'                                    Throw New Exception(String.Format("Unable to set {0}. Error:{1}", mProperty.Name, ex.Message))
'                                End Try

'                            End If
'                            Exit For
'                        Next
'                    Next

'                    Try

'                        mWiaImg = mWiaPage.Transfer()
'                    Catch ex As Exception
'                        Enough = True
'                        Exit Do
'                    End Try


'                    Dim mImgProc As New WIA.ImageProcess
'                    mImgProc.Filters.Add(mImgProc.FilterInfos("Convert").FilterID)
'                    mImgProc.Filters(1).Properties("FormatID").Value = WIA.FormatID.wiaFormatJPEG 'converts to JPG format. 
'                    mImgProc.Filters(1).Properties("Quality").Value = JpegCompression
'                    mWiaImg = mImgProc.Apply(mWiaImg)

'                    mImgProc = Nothing

'                    RaiseEvent OnPageComplete(mWiaImg)

'                    i += 1
'                    If pScannerInput <> _ScannerInput._Feeder Then
'                        Enough = True
'                    End If
'                Loop
'                mWiaPage = Nothing
'            Next
'            If i = 0 Then
'                RaiseEvent OnScanningError("No Documents found to scan. Check Scanner, reset if required")
'            End If
'            RaiseEvent OnScanningComplete(i)
'        End Function

'        Public Class WiaImageConverter
'            Private mImg As System.Drawing.Image
'            Private mVector As WIA.Vector
'            Private mBin As Byte()
'            Private mStrm As System.IO.MemoryStream
'            Private Const BLOCKSIZE = 521
'            Private Const MAXBLOCKS = 5

'            Public Sub New(ByVal pWiaImage As WIA.ImageFile)
'                mVector = pWiaImage.FileData
'                mBin = DirectCast(mVector.BinaryData(), Byte())
'                mStrm = New System.IO.MemoryStream(mBin)
'                mImg = System.Drawing.Image.FromStream(mStrm)
'            End Sub

'            Public Sub New(ByVal pStream As System.IO.Stream)
'                _New(pStream)
'            End Sub

'            Public Sub New(ByVal pEncodedImageFile As String)
'                Dim mIo As New System.IO.FileStream(pEncodedImageFile, System.IO.FileMode.Open)
'                _New(mIo)
'                mIo.Dispose()
'                mIo = Nothing
'            End Sub

'            Public Sub _New(ByVal pIoStream As System.IO.Stream)
'                Dim mS As New System.IO.MemoryStream
'                Dim mSourceBuffer As Byte() = {}
'                Dim mTargetBuffer As Byte() = {}
'                Dim mTempBuffer As Byte() = {}
'                Dim mKey As String = ""
'                Dim mEnc As New _Library._System._Security.Protection.Protection
'                Try
'                    mS.SetLength(pIoStream.Length)
'                    pIoStream.Read(mS.GetBuffer(), 0, pIoStream.Length)
'                    mSourceBuffer = mS.ToArray
'                    Array.Reverse(mSourceBuffer)
'                    For i As Integer = 0 To mSourceBuffer.Length - 1
'                        If Chr(mSourceBuffer(i)) = "/" Then
'                            Exit For
'                        End If
'                        mKey += Chr(mSourceBuffer(i))
'                    Next
'                    If mKey <> "" Then
'                        mKey = mEnc.FromBase67(mKey, "AjFas")
'                        Array.Reverse(mSourceBuffer)
'                        Array.Resize(mTargetBuffer, mKey.Split(";")(0))
'                        For Each mStr As String In mKey.Split(";")
'                            If InStr(mStr, ",") > 0 Then
'                                Dim mLen As Long = CLng(mStr.Split(",")(1))
'                                Array.Resize(mTempBuffer, 0)
'                                Array.Resize(mTempBuffer, mLen)
'                                Array.Copy(mSourceBuffer, CLng(mStr.Split(",")(0)), mTempBuffer, 0, mLen)
'                                Array.Reverse(mTempBuffer)
'                                Array.Copy(mTempBuffer, 0, mTargetBuffer, CLng(mStr.Split(",")(0)), mLen)
'                            End If
'                        Next
'                    End If
'                    mStrm = New System.IO.MemoryStream
'                    mStrm.Write(mTargetBuffer, 0, mTargetBuffer.Length)
'                    mImg = System.Drawing.Image.FromStream(mStrm)
'                Catch ex As Exception
'                    Throw ex
'                Finally
'                    mS.Dispose()
'                    mS = Nothing
'                    mSourceBuffer = Nothing
'                    mTargetBuffer = Nothing
'                    mTempBuffer = Nothing
'                    mEnc = Nothing
'                End Try
'            End Sub

'            Public Function Image() As System.Drawing.Image
'                Return mImg
'            End Function

'            Public Function SaveAsEncoded(ByVal pFileName As String, ByVal pImageFormat As System.Drawing.Imaging.ImageFormat) As Boolean
'                Return SaveAsEncoded(Image, pFileName, pImageFormat)
'            End Function


'            Private Shared Function ConcatArray(ByVal pFirst As System.Array, ByVal pSecond As System.Array) As System.Array
'                Dim mType As Type = pFirst.GetType().GetElementType()
'                Dim mResult As System.Array = System.Array.CreateInstance(mType, pFirst.Length + pSecond.Length)
'                pFirst.CopyTo(mResult, 0)
'                pSecond.CopyTo(mResult, pFirst.Length)
'                Return mResult
'            End Function

'            Private Shared Function AddRandomBlock(ByVal pArray As Byte(), ByVal pInsertBefore As Boolean) As Byte()
'                Dim mRet As Byte() = {}
'                Dim mRnd As Random = New Random()
'                Try
'                    Array.Resize(mRet, BLOCKSIZE)
'                    For i As Integer = 0 To BLOCKSIZE - 1
'                        mRet(i) = mRnd.Next(0, 127)
'                    Next
'                Catch ex As Exception
'                    Throw ex
'                Finally
'                    mRnd = Nothing
'                End Try
'                If pInsertBefore Then
'                    mRet = ConcatArray(mRet, pArray)
'                Else
'                    mRet = ConcatArray(pArray, mRet)
'                End If
'                Return mRet
'            End Function

'            Public Shared Function SaveAsEncoded(ByVal pImage As System.Drawing.Image, ByVal pFileName As String, ByVal pImageFormat As System.Drawing.Imaging.ImageFormat) As Boolean
'                Dim mKey As String = ""
'                Dim mIo As New System.IO.FileStream(pFileName, System.IO.FileMode.Create)
'                Dim mS As New System.IO.MemoryStream
'                Dim mBuffer As Byte() = {}
'                Dim mKeyA As Byte() = {}
'                Dim mBlockLength As Long = 0
'                Dim mRestLen As Long = 0
'                Dim mIndex As Long = 0
'                Dim mBlocks As New WiaImageEncodedBlocks
'                Dim mBlock As New WiaImageEncodedBlock
'                Dim mEncoding As New System.Text.ASCIIEncoding
'                Dim mEnc As New _Library._System._Security.Protection.Protection
'                Try
'                    pImage.Save(mS, pImageFormat) '  System.Drawing.Imaging.ImageFormat.Jpeg)
'                    mBuffer = mS.ToArray
'                    mKey = mBuffer.Length
'                    mBlockLength = CInt(mBuffer.Length / MAXBLOCKS)
'                    mRestLen = mBuffer.Length
'                    For i As Integer = 0 To MAXBLOCKS - 1
'                        mBlock = New WiaImageEncodedBlock
'                        System.Array.Resize(mBlock.Content, 0)
'                        If i = (MAXBLOCKS - 1) Then
'                            Array.Resize(mBlock.Content, mRestLen)
'                        Else
'                            Array.Resize(mBlock.Content, mBlockLength)
'                        End If
'                        Array.Copy(mBuffer, mIndex, mBlock.Content, 0, mBlock.Content.Length)
'                        mBlock.Index = mIndex
'                        mBlocks.Add(mBlock)
'                        mRestLen -= mBlockLength
'                        mIndex += mBlockLength
'                    Next
'                    System.Array.Resize(mBuffer, 0)
'                    For Each mBlk As WiaImageEncodedBlock In mBlocks
'                        mKey += ";" & mBlk.Index & "," & mBlk.Content.Length
'                        System.Array.Reverse(mBlk.Content)
'                        mBuffer = ConcatArray(mBuffer, mBlk.Content)
'                    Next
'                    mKey = mEnc.ToBase67(mKey, "AjFas")
'                    mKeyA = mEncoding.GetBytes(mKey)
'                    System.Array.Reverse(mKeyA)
'                    mKeyA = ConcatArray(mEncoding.GetBytes("/"), mKeyA)
'                    mBuffer = ConcatArray(mBuffer, mKeyA)
'                    mIo.Write(mBuffer, 0, mBuffer.Length)
'                Catch ex As Exception
'                    Throw ex
'                Finally
'                    mEnc = Nothing
'                    mEncoding = Nothing
'                    mBlock = Nothing
'                    mBlocks = Nothing
'                    mBuffer = Nothing
'                    mS.Dispose()
'                    mS = Nothing
'                    mIo.Dispose()
'                    mIo = Nothing
'                End Try
'            End Function

'            Public Shared Function GetAsBytesEncoded(ByVal pImage As System.Drawing.Image, ByVal pImageFormat As System.Drawing.Imaging.ImageFormat) As Byte()
'                Dim mRet As Byte() = {}
'                Dim mKey As String = ""
'                Dim mS As New System.IO.MemoryStream
'                Dim mKeyA As Byte() = {}
'                Dim mBlockLength As Long = 0
'                Dim mRestLen As Long = 0
'                Dim mIndex As Long = 0
'                Dim mBlocks As New WiaImageEncodedBlocks
'                Dim mBlock As New WiaImageEncodedBlock
'                Dim mEncoding As New System.Text.ASCIIEncoding
'                Dim mEnc As New _Library._System._Security.Protection.Protection
'                Try
'                    pImage.Save(mS, pImageFormat)
'                    mRet = mS.ToArray
'                    mKey = mRet.Length
'                    mBlockLength = CInt(mRet.Length / MAXBLOCKS)
'                    mRestLen = mRet.Length
'                    For i As Integer = 0 To MAXBLOCKS - 1
'                        mBlock = New WiaImageEncodedBlock
'                        System.Array.Resize(mBlock.Content, 0)
'                        If i = (MAXBLOCKS - 1) Then
'                            Array.Resize(mBlock.Content, mRestLen)
'                        Else
'                            Array.Resize(mBlock.Content, mBlockLength)
'                        End If
'                        Array.Copy(mRet, mIndex, mBlock.Content, 0, mBlock.Content.Length)
'                        mBlock.Index = mIndex
'                        mBlocks.Add(mBlock)
'                        mRestLen -= mBlockLength
'                        mIndex += mBlockLength
'                    Next
'                    System.Array.Resize(mRet, 0)
'                    For Each mBlk As WiaImageEncodedBlock In mBlocks
'                        mKey += ";" & mBlk.Index & "," & mBlk.Content.Length
'                        System.Array.Reverse(mBlk.Content)
'                        mRet = ConcatArray(mRet, mBlk.Content)
'                    Next
'                    mKey = mEnc.ToBase67(mKey, "AjFas")
'                    mKeyA = mEncoding.GetBytes(mKey)
'                    System.Array.Reverse(mKeyA)
'                    mKeyA = ConcatArray(mEncoding.GetBytes("/"), mKeyA)
'                    mRet = ConcatArray(mRet, mKeyA)
'                Catch ex As Exception
'                    Throw ex
'                Finally
'                    mEnc = Nothing
'                    mEncoding = Nothing
'                    mBlock = Nothing
'                    mBlocks = Nothing
'                    mS.Dispose()
'                    mS = Nothing
'                End Try
'                Return mRet
'            End Function

'            Public Sub dispose()
'                mVector = Nothing
'                mBin = Nothing
'                Try
'                    mStrm.Dispose()
'                Catch ex As Exception
'                End Try
'                mStrm = Nothing
'            End Sub

'            Protected Overrides Sub Finalize()
'                MyBase.Finalize()
'            End Sub

'            Private Class WiaImageEncodedBlocks
'                Inherits List(Of WiaImageEncodedBlock)
'            End Class

'            Private Class WiaImageEncodedBlock
'                Private mIndex As Long
'                Public Property Index() As Long
'                    Get
'                        Return mIndex
'                    End Get
'                    Set(ByVal value As Long)
'                        mIndex = value
'                    End Set
'                End Property

'                Private mByte As Byte() = {}
'                Public Property Content() As Byte()
'                    Get
'                        Return mByte
'                    End Get
'                    Set(ByVal value As Byte())
'                        mByte = value
'                    End Set
'                End Property

'            End Class

'        End Class

'    End Class

'End Namespace