<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Rijndael
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
        Me.btEncrypt = New System.Windows.Forms.Button()
        Me.btDecrypt = New System.Windows.Forms.Button()
        Me.btDecode = New System.Windows.Forms.Button()
        Me.lblEncryptedString = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblString = New System.Windows.Forms.Label()
        Me.btEncode = New System.Windows.Forms.Button()
        Me.tbEncryptedString = New System.Windows.Forms.TextBox()
        Me.tbKey = New System.Windows.Forms.TextBox()
        Me.tbString = New System.Windows.Forms.TextBox()
        Me.tbEncodedString = New System.Windows.Forms.TextBox()
        Me.lblEncodedString = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btEncrypt
        '
        Me.btEncrypt.Location = New System.Drawing.Point(15, 400)
        Me.btEncrypt.Name = "btEncrypt"
        Me.btEncrypt.Size = New System.Drawing.Size(89, 23)
        Me.btEncrypt.TabIndex = 21
        Me.btEncrypt.Text = "Encrypt"
        Me.btEncrypt.UseVisualStyleBackColor = True
        '
        'btDecrypt
        '
        Me.btDecrypt.Location = New System.Drawing.Point(110, 400)
        Me.btDecrypt.Name = "btDecrypt"
        Me.btDecrypt.Size = New System.Drawing.Size(89, 23)
        Me.btDecrypt.TabIndex = 20
        Me.btDecrypt.Text = "Decrypt"
        Me.btDecrypt.UseVisualStyleBackColor = True
        '
        'btDecode
        '
        Me.btDecode.Location = New System.Drawing.Point(440, 400)
        Me.btDecode.Name = "btDecode"
        Me.btDecode.Size = New System.Drawing.Size(75, 23)
        Me.btDecode.TabIndex = 19
        Me.btDecode.Text = "Decode"
        Me.btDecode.UseVisualStyleBackColor = True
        '
        'lblEncryptedString
        '
        Me.lblEncryptedString.AutoSize = True
        Me.lblEncryptedString.Location = New System.Drawing.Point(12, 167)
        Me.lblEncryptedString.Name = "lblEncryptedString"
        Me.lblEncryptedString.Size = New System.Drawing.Size(126, 13)
        Me.lblEncryptedString.TabIndex = 18
        Me.lblEncryptedString.Text = "Rijndael Encrypted String"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Key"
        '
        'lblString
        '
        Me.lblString.AutoSize = True
        Me.lblString.Location = New System.Drawing.Point(12, 9)
        Me.lblString.Name = "lblString"
        Me.lblString.Size = New System.Drawing.Size(34, 13)
        Me.lblString.TabIndex = 16
        Me.lblString.Text = "String"
        '
        'btEncode
        '
        Me.btEncode.Location = New System.Drawing.Point(359, 400)
        Me.btEncode.Name = "btEncode"
        Me.btEncode.Size = New System.Drawing.Size(75, 23)
        Me.btEncode.TabIndex = 15
        Me.btEncode.Text = "Encode"
        Me.btEncode.UseVisualStyleBackColor = True
        '
        'tbEncryptedString
        '
        Me.tbEncryptedString.Location = New System.Drawing.Point(15, 183)
        Me.tbEncryptedString.Multiline = True
        Me.tbEncryptedString.Name = "tbEncryptedString"
        Me.tbEncryptedString.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbEncryptedString.Size = New System.Drawing.Size(500, 96)
        Me.tbEncryptedString.TabIndex = 14
        '
        'tbKey
        '
        Me.tbKey.Location = New System.Drawing.Point(15, 104)
        Me.tbKey.Multiline = True
        Me.tbKey.Name = "tbKey"
        Me.tbKey.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.tbKey.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbKey.Size = New System.Drawing.Size(500, 60)
        Me.tbKey.TabIndex = 13
        Me.tbKey.Text = "jazeera"
        '
        'tbString
        '
        Me.tbString.Location = New System.Drawing.Point(15, 25)
        Me.tbString.Multiline = True
        Me.tbString.Name = "tbString"
        Me.tbString.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbString.Size = New System.Drawing.Size(500, 60)
        Me.tbString.TabIndex = 12
        '
        'tbEncodedString
        '
        Me.tbEncodedString.Location = New System.Drawing.Point(15, 298)
        Me.tbEncodedString.Multiline = True
        Me.tbEncodedString.Name = "tbEncodedString"
        Me.tbEncodedString.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbEncodedString.Size = New System.Drawing.Size(500, 96)
        Me.tbEncodedString.TabIndex = 22
        '
        'lblEncodedString
        '
        Me.lblEncodedString.AutoSize = True
        Me.lblEncodedString.Location = New System.Drawing.Point(12, 282)
        Me.lblEncodedString.Name = "lblEncodedString"
        Me.lblEncodedString.Size = New System.Drawing.Size(121, 13)
        Me.lblEncodedString.TabIndex = 23
        Me.lblEncodedString.Text = "Rijndael Encoded String"
        '
        'Rijndael
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 440)
        Me.Controls.Add(Me.lblEncodedString)
        Me.Controls.Add(Me.tbEncodedString)
        Me.Controls.Add(Me.btEncrypt)
        Me.Controls.Add(Me.btDecrypt)
        Me.Controls.Add(Me.btDecode)
        Me.Controls.Add(Me.lblEncryptedString)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblString)
        Me.Controls.Add(Me.btEncode)
        Me.Controls.Add(Me.tbEncryptedString)
        Me.Controls.Add(Me.tbKey)
        Me.Controls.Add(Me.tbString)
        Me.Name = "Rijndael"
        Me.Text = "Rijndael Encryption"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btEncrypt As System.Windows.Forms.Button
    Friend WithEvents btDecrypt As System.Windows.Forms.Button
    Friend WithEvents btDecode As System.Windows.Forms.Button
    Friend WithEvents lblEncryptedString As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblString As System.Windows.Forms.Label
    Friend WithEvents btEncode As System.Windows.Forms.Button
    Friend WithEvents tbEncryptedString As System.Windows.Forms.TextBox
    Friend WithEvents tbKey As System.Windows.Forms.TextBox
    Friend WithEvents tbString As System.Windows.Forms.TextBox
    Friend WithEvents tbEncodedString As System.Windows.Forms.TextBox
    Friend WithEvents lblEncodedString As System.Windows.Forms.Label
End Class
