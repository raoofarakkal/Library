<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class b67
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
        Me.tbString = New System.Windows.Forms.TextBox()
        Me.tbKey = New System.Windows.Forms.TextBox()
        Me.tbB67String = New System.Windows.Forms.TextBox()
        Me.btFromB67 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btToB67 = New System.Windows.Forms.Button()
        Me.btAjnEncrypt = New System.Windows.Forms.Button()
        Me.btAjnDecrypt = New System.Windows.Forms.Button()
        Me.btAjnEncKey = New System.Windows.Forms.Button()
        Me.btB67Key = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbString
        '
        Me.tbString.Location = New System.Drawing.Point(12, 25)
        Me.tbString.Multiline = True
        Me.tbString.Name = "tbString"
        Me.tbString.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbString.Size = New System.Drawing.Size(500, 60)
        Me.tbString.TabIndex = 0
        '
        'tbKey
        '
        Me.tbKey.Location = New System.Drawing.Point(12, 119)
        Me.tbKey.Multiline = True
        Me.tbKey.Name = "tbKey"
        Me.tbKey.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.tbKey.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbKey.Size = New System.Drawing.Size(500, 60)
        Me.tbKey.TabIndex = 1
        Me.tbKey.Text = "jazeera"
        '
        'tbB67String
        '
        Me.tbB67String.Location = New System.Drawing.Point(12, 216)
        Me.tbB67String.Multiline = True
        Me.tbB67String.Name = "tbB67String"
        Me.tbB67String.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbB67String.Size = New System.Drawing.Size(500, 96)
        Me.tbB67String.TabIndex = 2
        '
        'btFromB67
        '
        Me.btFromB67.Location = New System.Drawing.Point(345, 318)
        Me.btFromB67.Name = "btFromB67"
        Me.btFromB67.Size = New System.Drawing.Size(75, 23)
        Me.btFromB67.TabIndex = 3
        Me.btFromB67.Text = "From B67"
        Me.btFromB67.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "String"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Key"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 200)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "B67 String / AJN Encrypted String"
        '
        'btToB67
        '
        Me.btToB67.Location = New System.Drawing.Point(437, 318)
        Me.btToB67.Name = "btToB67"
        Me.btToB67.Size = New System.Drawing.Size(75, 23)
        Me.btToB67.TabIndex = 7
        Me.btToB67.Text = "To B67"
        Me.btToB67.UseVisualStyleBackColor = True
        '
        'btAjnEncrypt
        '
        Me.btAjnEncrypt.Location = New System.Drawing.Point(107, 318)
        Me.btAjnEncrypt.Name = "btAjnEncrypt"
        Me.btAjnEncrypt.Size = New System.Drawing.Size(89, 23)
        Me.btAjnEncrypt.TabIndex = 9
        Me.btAjnEncrypt.Text = "AJN-Encrypt"
        Me.btAjnEncrypt.UseVisualStyleBackColor = True
        '
        'btAjnDecrypt
        '
        Me.btAjnDecrypt.Location = New System.Drawing.Point(12, 318)
        Me.btAjnDecrypt.Name = "btAjnDecrypt"
        Me.btAjnDecrypt.Size = New System.Drawing.Size(89, 23)
        Me.btAjnDecrypt.TabIndex = 8
        Me.btAjnDecrypt.Text = "AJN-Decrypt"
        Me.btAjnDecrypt.UseVisualStyleBackColor = True
        '
        'btAjnEncKey
        '
        Me.btAjnEncKey.Location = New System.Drawing.Point(121, 93)
        Me.btAjnEncKey.Name = "btAjnEncKey"
        Me.btAjnEncKey.Size = New System.Drawing.Size(111, 23)
        Me.btAjnEncKey.TabIndex = 11
        Me.btAjnEncKey.Text = "AJN-Encrypt key"
        Me.btAjnEncKey.UseVisualStyleBackColor = True
        '
        'btB67Key
        '
        Me.btB67Key.Location = New System.Drawing.Point(40, 93)
        Me.btB67Key.Name = "btB67Key"
        Me.btB67Key.Size = New System.Drawing.Size(75, 23)
        Me.btB67Key.TabIndex = 10
        Me.btB67Key.Text = "b67 key"
        Me.btB67Key.UseVisualStyleBackColor = True
        '
        'b67
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(526, 366)
        Me.Controls.Add(Me.btAjnEncKey)
        Me.Controls.Add(Me.btB67Key)
        Me.Controls.Add(Me.btAjnEncrypt)
        Me.Controls.Add(Me.btAjnDecrypt)
        Me.Controls.Add(Me.btToB67)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btFromB67)
        Me.Controls.Add(Me.tbB67String)
        Me.Controls.Add(Me.tbKey)
        Me.Controls.Add(Me.tbString)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "b67"
        Me.Text = "b67"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbString As System.Windows.Forms.TextBox
    Friend WithEvents tbKey As System.Windows.Forms.TextBox
    Friend WithEvents tbB67String As System.Windows.Forms.TextBox
    Friend WithEvents btFromB67 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btToB67 As System.Windows.Forms.Button
    Friend WithEvents btAjnEncrypt As System.Windows.Forms.Button
    Friend WithEvents btAjnDecrypt As System.Windows.Forms.Button
    Friend WithEvents btAjnEncKey As System.Windows.Forms.Button
    Friend WithEvents btB67Key As System.Windows.Forms.Button
End Class
