<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HttpReq
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.tbReferer = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.btGo = New System.Windows.Forms.Button
        Me.tbPWD = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbUID = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbProxy = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbURL = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbResult = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbReferer)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.btGo)
        Me.GroupBox1.Controls.Add(Me.tbPWD)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.tbUID)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.tbProxy)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.tbURL)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(924, 86)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'tbReferer
        '
        Me.tbReferer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbReferer.Location = New System.Drawing.Point(512, 19)
        Me.tbReferer.Name = "tbReferer"
        Me.tbReferer.Size = New System.Drawing.Size(396, 20)
        Me.tbReferer.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(464, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Referer"
        '
        'btGo
        '
        Me.btGo.Location = New System.Drawing.Point(871, 49)
        Me.btGo.Name = "btGo"
        Me.btGo.Size = New System.Drawing.Size(37, 23)
        Me.btGo.TabIndex = 10
        Me.btGo.Text = "GO"
        Me.btGo.UseVisualStyleBackColor = True
        '
        'tbPWD
        '
        Me.tbPWD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPWD.Location = New System.Drawing.Point(639, 52)
        Me.tbPWD.Name = "tbPWD"
        Me.tbPWD.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.tbPWD.Size = New System.Drawing.Size(166, 20)
        Me.tbPWD.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(600, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "PWD"
        '
        'tbUID
        '
        Me.tbUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbUID.Location = New System.Drawing.Point(347, 52)
        Me.tbUID.Name = "tbUID"
        Me.tbUID.Size = New System.Drawing.Size(166, 20)
        Me.tbUID.TabIndex = 7
        Me.tbUID.Text = "aljazeera\arakkala"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(315, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "UID"
        '
        'tbProxy
        '
        Me.tbProxy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbProxy.Location = New System.Drawing.Point(43, 52)
        Me.tbProxy.Name = "tbProxy"
        Me.tbProxy.Size = New System.Drawing.Size(191, 20)
        Me.tbProxy.TabIndex = 5
        Me.tbProxy.Text = "10.10.50.20:80"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Proxy"
        '
        'tbURL
        '
        Me.tbURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbURL.Location = New System.Drawing.Point(43, 19)
        Me.tbURL.Name = "tbURL"
        Me.tbURL.Size = New System.Drawing.Size(396, 20)
        Me.tbURL.TabIndex = 3
        Me.tbURL.Text = "http://www.microsoft.com"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "URL"
        '
        'tbResult
        '
        Me.tbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbResult.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbResult.Location = New System.Drawing.Point(12, 104)
        Me.tbResult.Multiline = True
        Me.tbResult.Name = "tbResult"
        Me.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbResult.Size = New System.Drawing.Size(439, 594)
        Me.tbResult.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(458, 105)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(478, 593)
        Me.Panel1.TabIndex = 5
        '
        'HttpReq
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(948, 710)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.tbResult)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "HttpReq"
        Me.Text = "HTTP request and Response"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbPWD As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbUID As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbProxy As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbURL As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btGo As System.Windows.Forms.Button
    Friend WithEvents tbResult As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tbReferer As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
