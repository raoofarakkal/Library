<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SQL2CSV
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tbCsv = New System.Windows.Forms.TextBox
        Me.btExecute = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbOpCsvFile = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbDB = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbPWD = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbUID = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbServer = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbSQL = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'tbCsv
        '
        Me.tbCsv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbCsv.Location = New System.Drawing.Point(15, 301)
        Me.tbCsv.Multiline = True
        Me.tbCsv.Name = "tbCsv"
        Me.tbCsv.ReadOnly = True
        Me.tbCsv.Size = New System.Drawing.Size(677, 201)
        Me.tbCsv.TabIndex = 27
        '
        'btExecute
        '
        Me.btExecute.Location = New System.Drawing.Point(617, 264)
        Me.btExecute.Name = "btExecute"
        Me.btExecute.Size = New System.Drawing.Size(75, 23)
        Me.btExecute.TabIndex = 26
        Me.btExecute.Text = "Execute"
        Me.btExecute.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 267)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Output CSV File"
        '
        'tbOpCsvFile
        '
        Me.tbOpCsvFile.Location = New System.Drawing.Point(100, 264)
        Me.tbOpCsvFile.Name = "tbOpCsvFile"
        Me.tbOpCsvFile.Size = New System.Drawing.Size(511, 20)
        Me.tbOpCsvFile.TabIndex = 24
        Me.tbOpCsvFile.Text = "C:\temp\File1.csv"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(283, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(22, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "DB"
        '
        'tbDB
        '
        Me.tbDB.Location = New System.Drawing.Point(311, 9)
        Me.tbDB.Name = "tbDB"
        Me.tbDB.Size = New System.Drawing.Size(100, 20)
        Me.tbDB.TabIndex = 22
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(555, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 13)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "PWD"
        '
        'tbPWD
        '
        Me.tbPWD.Location = New System.Drawing.Point(592, 9)
        Me.tbPWD.Name = "tbPWD"
        Me.tbPWD.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbPWD.Size = New System.Drawing.Size(100, 20)
        Me.tbPWD.TabIndex = 20
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(417, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "UID"
        '
        'tbUID
        '
        Me.tbUID.Location = New System.Drawing.Point(449, 9)
        Me.tbUID.Name = "tbUID"
        Me.tbUID.Size = New System.Drawing.Size(100, 20)
        Me.tbUID.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Server"
        '
        'tbServer
        '
        Me.tbServer.Location = New System.Drawing.Point(56, 9)
        Me.tbServer.Name = "tbServer"
        Me.tbServer.Size = New System.Drawing.Size(221, 20)
        Me.tbServer.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "SQL Query"
        '
        'tbSQL
        '
        Me.tbSQL.Location = New System.Drawing.Point(15, 57)
        Me.tbSQL.Multiline = True
        Me.tbSQL.Name = "tbSQL"
        Me.tbSQL.Size = New System.Drawing.Size(677, 201)
        Me.tbSQL.TabIndex = 14
        '
        'SQL2CSV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 511)
        Me.Controls.Add(Me.tbCsv)
        Me.Controls.Add(Me.btExecute)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbOpCsvFile)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbDB)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbPWD)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbUID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbServer)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbSQL)
        Me.Name = "SQL2CSV"
        Me.Text = "SQL2CSV"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbCsv As System.Windows.Forms.TextBox
    Friend WithEvents btExecute As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbOpCsvFile As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbDB As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbPWD As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbUID As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbServer As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbSQL As System.Windows.Forms.TextBox
End Class
