<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.txtLargura = New System.Windows.Forms.TextBox()
        Me.txtAltura = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(19, 35)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(488, 23)
        Me.txtPath.TabIndex = 0
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(19, 77)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(766, 361)
        Me.txtLog.TabIndex = 1
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(710, 35)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(75, 23)
        Me.btnExecute.TabIndex = 2
        Me.btnExecute.Text = "Executar"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'txtLargura
        '
        Me.txtLargura.Location = New System.Drawing.Point(513, 35)
        Me.txtLargura.Name = "txtLargura"
        Me.txtLargura.Size = New System.Drawing.Size(83, 23)
        Me.txtLargura.TabIndex = 3
        '
        'txtAltura
        '
        Me.txtAltura.Location = New System.Drawing.Point(602, 35)
        Me.txtAltura.Name = "txtAltura"
        Me.txtAltura.Size = New System.Drawing.Size(83, 23)
        Me.txtAltura.TabIndex = 4
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtAltura)
        Me.Controls.Add(Me.txtLargura)
        Me.Controls.Add(Me.btnExecute)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.txtPath)
        Me.Name = "frmMain"
        Me.Text = "PCM"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtPath As TextBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents btnExecute As Button
    Friend WithEvents txtLargura As TextBox
    Friend WithEvents txtAltura As TextBox
End Class
