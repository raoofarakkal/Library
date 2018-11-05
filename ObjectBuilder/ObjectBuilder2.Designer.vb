<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ObjectBuilder2
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ddlDB = New System.Windows.Forms.ComboBox()
        Me.btConnect = New System.Windows.Forms.Button()
        Me.tbPwd = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbUid = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ddlTables = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbServer = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btB67 = New System.Windows.Forms.Button()
        Me.btGenerate = New System.Windows.Forms.Button()
        Me.tbCode = New System.Windows.Forms.TextBox()
        Me.btCopy = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.tbXmlDef = New System.Windows.Forms.TextBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btShell = New System.Windows.Forms.Button()
        Me.btRijndael = New System.Windows.Forms.Button()
        Me.btVer1 = New System.Windows.Forms.Button()
        Me.btSql2Csv = New System.Windows.Forms.Button()
        Me.bthttpReq = New System.Windows.Forms.Button()
        Me.btjScript2StrBldr = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btListOfFindByField = New System.Windows.Forms.Button()
        Me.btThreadGen = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ddlDB)
        Me.GroupBox1.Controls.Add(Me.btConnect)
        Me.GroupBox1.Controls.Add(Me.tbPwd)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.tbUid)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.ddlTables)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.tbServer)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(689, 68)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'ddlDB
        '
        Me.ddlDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlDB.Enabled = False
        Me.ddlDB.FormattingEnabled = True
        Me.ddlDB.Location = New System.Drawing.Point(51, 37)
        Me.ddlDB.Name = "ddlDB"
        Me.ddlDB.Size = New System.Drawing.Size(230, 21)
        Me.ddlDB.TabIndex = 10
        '
        'btConnect
        '
        Me.btConnect.Location = New System.Drawing.Point(588, 13)
        Me.btConnect.Name = "btConnect"
        Me.btConnect.Size = New System.Drawing.Size(90, 23)
        Me.btConnect.TabIndex = 8
        Me.btConnect.Text = "Connect"
        Me.btConnect.UseVisualStyleBackColor = True
        '
        'tbPwd
        '
        Me.tbPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPwd.Location = New System.Drawing.Point(471, 14)
        Me.tbPwd.Name = "tbPwd"
        Me.tbPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tbPwd.Size = New System.Drawing.Size(100, 20)
        Me.tbPwd.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(439, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "PWD"
        '
        'tbUid
        '
        Me.tbUid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbUid.Location = New System.Drawing.Point(325, 13)
        Me.tbUid.Name = "tbUid"
        Me.tbUid.Size = New System.Drawing.Size(100, 20)
        Me.tbUid.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(293, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "UID"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(287, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Table"
        '
        'ddlTables
        '
        Me.ddlTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlTables.Enabled = False
        Me.ddlTables.FormattingEnabled = True
        Me.ddlTables.Location = New System.Drawing.Point(325, 39)
        Me.ddlTables.Name = "ddlTables"
        Me.ddlTables.Size = New System.Drawing.Size(246, 21)
        Me.ddlTables.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(22, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "DB"
        '
        'tbServer
        '
        Me.tbServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbServer.Location = New System.Drawing.Point(51, 13)
        Me.tbServer.Name = "tbServer"
        Me.tbServer.Size = New System.Drawing.Size(230, 20)
        Me.tbServer.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server"
        '
        'btB67
        '
        Me.btB67.Location = New System.Drawing.Point(638, 35)
        Me.btB67.Name = "btB67"
        Me.btB67.Size = New System.Drawing.Size(44, 23)
        Me.btB67.TabIndex = 9
        Me.btB67.Text = "B67"
        Me.btB67.UseVisualStyleBackColor = True
        '
        'btGenerate
        '
        Me.btGenerate.Enabled = False
        Me.btGenerate.Location = New System.Drawing.Point(600, 269)
        Me.btGenerate.Name = "btGenerate"
        Me.btGenerate.Size = New System.Drawing.Size(101, 23)
        Me.btGenerate.TabIndex = 4
        Me.btGenerate.Text = "Generate"
        Me.btGenerate.UseVisualStyleBackColor = True
        '
        'tbCode
        '
        Me.tbCode.Location = New System.Drawing.Point(13, 298)
        Me.tbCode.Multiline = True
        Me.tbCode.Name = "tbCode"
        Me.tbCode.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbCode.Size = New System.Drawing.Size(688, 214)
        Me.tbCode.TabIndex = 5
        '
        'btCopy
        '
        Me.btCopy.Enabled = False
        Me.btCopy.Location = New System.Drawing.Point(600, 518)
        Me.btCopy.Name = "btCopy"
        Me.btCopy.Size = New System.Drawing.Size(101, 23)
        Me.btCopy.TabIndex = 6
        Me.btCopy.Text = "Copy to clipboard"
        Me.btCopy.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(12, 86)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(689, 181)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.tbXmlDef)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(681, 155)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "XML View"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'tbXmlDef
        '
        Me.tbXmlDef.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbXmlDef.Location = New System.Drawing.Point(3, 3)
        Me.tbXmlDef.Multiline = True
        Me.tbXmlDef.Name = "tbXmlDef"
        Me.tbXmlDef.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbXmlDef.Size = New System.Drawing.Size(675, 149)
        Me.tbXmlDef.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(681, 155)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Grid View"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(675, 149)
        Me.DataGridView1.TabIndex = 4
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btShell)
        Me.GroupBox2.Controls.Add(Me.btRijndael)
        Me.GroupBox2.Controls.Add(Me.btVer1)
        Me.GroupBox2.Controls.Add(Me.btSql2Csv)
        Me.GroupBox2.Controls.Add(Me.bthttpReq)
        Me.GroupBox2.Controls.Add(Me.btjScript2StrBldr)
        Me.GroupBox2.Controls.Add(Me.btB67)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 547)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(689, 64)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Other Fuctions"
        '
        'btShell
        '
        Me.btShell.Location = New System.Drawing.Point(116, 35)
        Me.btShell.Name = "btShell"
        Me.btShell.Size = New System.Drawing.Size(56, 23)
        Me.btShell.TabIndex = 15
        Me.btShell.Text = "Shell"
        Me.btShell.UseVisualStyleBackColor = True
        '
        'btRijndael
        '
        Me.btRijndael.Location = New System.Drawing.Point(573, 35)
        Me.btRijndael.Name = "btRijndael"
        Me.btRijndael.Size = New System.Drawing.Size(59, 23)
        Me.btRijndael.TabIndex = 14
        Me.btRijndael.Text = "Rijndael"
        Me.btRijndael.UseVisualStyleBackColor = True
        '
        'btVer1
        '
        Me.btVer1.Location = New System.Drawing.Point(9, 35)
        Me.btVer1.Name = "btVer1"
        Me.btVer1.Size = New System.Drawing.Size(56, 23)
        Me.btVer1.TabIndex = 12
        Me.btVer1.Text = "Ver 1.0"
        Me.btVer1.UseVisualStyleBackColor = True
        '
        'btSql2Csv
        '
        Me.btSql2Csv.Location = New System.Drawing.Point(178, 35)
        Me.btSql2Csv.Name = "btSql2Csv"
        Me.btSql2Csv.Size = New System.Drawing.Size(93, 23)
        Me.btSql2Csv.TabIndex = 13
        Me.btSql2Csv.Text = "SQL 2 CSV"
        Me.btSql2Csv.UseVisualStyleBackColor = True
        '
        'bthttpReq
        '
        Me.bthttpReq.Location = New System.Drawing.Point(277, 35)
        Me.bthttpReq.Name = "bthttpReq"
        Me.bthttpReq.Size = New System.Drawing.Size(93, 23)
        Me.bthttpReq.TabIndex = 12
        Me.bthttpReq.Text = "HTTP request"
        Me.bthttpReq.UseVisualStyleBackColor = True
        '
        'btjScript2StrBldr
        '
        Me.btjScript2StrBldr.Location = New System.Drawing.Point(380, 35)
        Me.btjScript2StrBldr.Name = "btjScript2StrBldr"
        Me.btjScript2StrBldr.Size = New System.Drawing.Size(93, 23)
        Me.btjScript2StrBldr.TabIndex = 11
        Me.btjScript2StrBldr.Text = "jScript 2 StrBldr"
        Me.btjScript2StrBldr.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btListOfFindByField)
        Me.GroupBox3.Controls.Add(Me.btThreadGen)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(12, 618)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(689, 68)
        Me.GroupBox3.TabIndex = 11
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Sample Codes"
        '
        'btListOfFindByField
        '
        Me.btListOfFindByField.Location = New System.Drawing.Point(116, 34)
        Me.btListOfFindByField.Name = "btListOfFindByField"
        Me.btListOfFindByField.Size = New System.Drawing.Size(126, 23)
        Me.btListOfFindByField.TabIndex = 12
        Me.btListOfFindByField.Text = "List(of ) Find by Field"
        Me.btListOfFindByField.UseVisualStyleBackColor = True
        '
        'btThreadGen
        '
        Me.btThreadGen.Location = New System.Drawing.Point(9, 34)
        Me.btThreadGen.Name = "btThreadGen"
        Me.btThreadGen.Size = New System.Drawing.Size(90, 23)
        Me.btThreadGen.TabIndex = 11
        Me.btThreadGen.Text = "Multi-Threads"
        Me.btThreadGen.UseVisualStyleBackColor = True
        '
        'ObjectBuilder2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(708, 695)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btCopy)
        Me.Controls.Add(Me.tbCode)
        Me.Controls.Add(Me.btGenerate)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "ObjectBuilder2"
        Me.Text = "Object Builder Ver 2.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbUid As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbServer As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btConnect As System.Windows.Forms.Button
    Friend WithEvents ddlTables As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btGenerate As System.Windows.Forms.Button
    Friend WithEvents tbCode As System.Windows.Forms.TextBox
    Friend WithEvents btB67 As System.Windows.Forms.Button
    Friend WithEvents btCopy As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents tbXmlDef As System.Windows.Forms.TextBox
    Friend WithEvents ddlDB As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btjScript2StrBldr As System.Windows.Forms.Button
    Friend WithEvents bthttpReq As System.Windows.Forms.Button
    Friend WithEvents btSql2Csv As System.Windows.Forms.Button
    Friend WithEvents btVer1 As System.Windows.Forms.Button
    Friend WithEvents btRijndael As System.Windows.Forms.Button
    Friend WithEvents btShell As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btListOfFindByField As System.Windows.Forms.Button
    Friend WithEvents btThreadGen As System.Windows.Forms.Button

End Class
