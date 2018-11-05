Imports Library2.Db
Imports Library2.IO

Public Class SQL2CSV

    Private Sub btExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btExecute.Click
        Dim mDbSet As New oDbSettings(Me.tbServer.Text, Me.tbDB.Text, Me.tbUID.Text, Me.tbPWD.Text)
        Dim mDb As New MyDB(mDbSet)
        Try
            If Mid(Me.tbSQL.Text.Trim, 1, 6).ToUpper = "SELECT" Then
                Dim mDt As DataTable
                Dim mCsv As String = ""
                Dim Comma As String = ""
                Dim mMaxCols As Integer = 0
                mDt = mDb.GetTable(Me.tbSQL.Text)
                If mDt.Rows.Count > 0 Then
                    For Each mC As DataColumn In mDt.Columns
                        mCsv += Comma & mC.ColumnName
                        Comma = ","
                        mMaxCols += 1
                    Next
                    mCsv += vbCrLf

                    For Each mR As DataRow In mDt.Rows
                        Comma = ""
                        For i As Integer = 0 To mMaxCols - 1
                            mCsv += Comma & Filter(mR.Item(i).ToString)
                            Comma = ","
                        Next
                        mCsv += vbCrLf
                    Next
                    Me.tbCsv.Text = mCsv
                    Dim mWr As New Writer(Me.tbOpCsvFile.Text, System.Text.Encoding.UTF8)
                    mWr.Write(mCsv)
                    mWr.Dispose()
                    mWr = Nothing
                    MsgBox("Done")
                End If
            Else
                MsgBox("SELECT statement required")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            mDb.Dispose()
            mDb = Nothing
            mDbSet = Nothing
        End Try
    End Sub

    Private Function Filter(ByVal pString As String) As String
        Dim mRet As String = ""
        mRet = pString.Replace(",", " - ")
        mRet = mRet.Replace(vbCrLf, " - ")
        Return mRet
    End Function

End Class