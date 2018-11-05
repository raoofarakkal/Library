<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Shell
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbUID = New System.Windows.Forms.TextBox()
        Me.tbPWD = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbShellCmd = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btExec = New System.Windows.Forms.Button()
        Me.tbDomain = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbPara = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User name"
        '
        'tbUID
        '
        Me.tbUID.Location = New System.Drawing.Point(70, 53)
        Me.tbUID.Name = "tbUID"
        Me.tbUID.Size = New System.Drawing.Size(234, 20)
        Me.tbUID.TabIndex = 1
        '
        'tbPWD
        '
        Me.tbPWD.Location = New System.Drawing.Point(70, 90)
        Me.tbPWD.Name = "tbPWD"
        Me.tbPWD.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.tbPWD.Size = New System.Drawing.Size(234, 20)
        Me.tbPWD.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Password"
        '
        'tbShellCmd
        '
        Me.tbShellCmd.Location = New System.Drawing.Point(9, 151)
        Me.tbShellCmd.Name = "tbShellCmd"
        Me.tbShellCmd.Size = New System.Drawing.Size(295, 20)
        Me.tbShellCmd.TabIndex = 4
        Me.tbShellCmd.Text = "explorer.exe"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 135)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Shell Command"
        '
        'btExec
        '
        Me.btExec.Location = New System.Drawing.Point(229, 214)
        Me.btExec.Name = "btExec"
        Me.btExec.Size = New System.Drawing.Size(75, 23)
        Me.btExec.TabIndex = 6
        Me.btExec.Text = "Execute"
        Me.btExec.UseVisualStyleBackColor = True
        '
        'tbDomain
        '
        Me.tbDomain.Location = New System.Drawing.Point(70, 16)
        Me.tbDomain.Name = "tbDomain"
        Me.tbDomain.Size = New System.Drawing.Size(234, 20)
        Me.tbDomain.TabIndex = 8
        Me.tbDomain.Text = "aljazeera.tv"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Domain"
        '
        'tbPara
        '
        Me.tbPara.Location = New System.Drawing.Point(70, 177)
        Me.tbPara.Name = "tbPara"
        Me.tbPara.Size = New System.Drawing.Size(234, 20)
        Me.tbPara.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 180)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Parameter"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbDomain)
        Me.GroupBox1.Controls.Add(Me.btExec)
        Me.GroupBox1.Controls.Add(Me.tbPara)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.tbShellCmd)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.tbUID)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.tbPWD)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(316, 249)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        '
        'Shell
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 275)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Shell"
        Me.Text = "Shell"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbUID As System.Windows.Forms.TextBox
    Friend WithEvents tbPWD As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbShellCmd As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btExec As System.Windows.Forms.Button
    Friend WithEvents tbDomain As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbPara As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
