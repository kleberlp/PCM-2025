<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.pagDados = New System.Windows.Forms.TabPage()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.txtEndpoint = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pagLog = New System.Windows.Forms.TabPage()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.tabMain.SuspendLayout()
        Me.pagDados.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pagLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.pagDados)
        Me.tabMain.Controls.Add(Me.pagLog)
        Me.tabMain.Location = New System.Drawing.Point(12, 12)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(852, 547)
        Me.tabMain.TabIndex = 0
        '
        'pagDados
        '
        Me.pagDados.Controls.Add(Me.btnExecute)
        Me.pagDados.Controls.Add(Me.GroupBox1)
        Me.pagDados.Location = New System.Drawing.Point(4, 24)
        Me.pagDados.Name = "pagDados"
        Me.pagDados.Padding = New System.Windows.Forms.Padding(3)
        Me.pagDados.Size = New System.Drawing.Size(844, 519)
        Me.pagDados.TabIndex = 0
        Me.pagDados.Text = "Restaurar Backup - LOG"
        Me.pagDados.UseVisualStyleBackColor = True
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(758, 484)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(75, 23)
        Me.btnExecute.TabIndex = 1
        Me.btnExecute.Text = "Executar"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtQuery)
        Me.GroupBox1.Controls.Add(Me.txtEndpoint)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(824, 472)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Query:"
        '
        'txtQuery
        '
        Me.txtQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQuery.Location = New System.Drawing.Point(12, 86)
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtQuery.Size = New System.Drawing.Size(800, 374)
        Me.txtQuery.TabIndex = 2
        '
        'txtEndpoint
        '
        Me.txtEndpoint.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndpoint.Location = New System.Drawing.Point(12, 42)
        Me.txtEndpoint.Name = "txtEndpoint"
        Me.txtEndpoint.Size = New System.Drawing.Size(800, 23)
        Me.txtEndpoint.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Endpoint:"
        '
        'pagLog
        '
        Me.pagLog.Controls.Add(Me.txtLog)
        Me.pagLog.Location = New System.Drawing.Point(4, 24)
        Me.pagLog.Name = "pagLog"
        Me.pagLog.Padding = New System.Windows.Forms.Padding(3)
        Me.pagLog.Size = New System.Drawing.Size(844, 519)
        Me.pagLog.TabIndex = 1
        Me.pagLog.Text = "Log"
        Me.pagLog.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(12, 12)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(819, 495)
        Me.txtLog.TabIndex = 3
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(876, 571)
        Me.Controls.Add(Me.tabMain)
        Me.Name = "Form1"
        Me.Text = "PCM"
        Me.tabMain.ResumeLayout(False)
        Me.pagDados.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pagLog.ResumeLayout(False)
        Me.pagLog.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tabMain As TabControl
    Friend WithEvents pagDados As TabPage
    Friend WithEvents btnExecute As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtQuery As TextBox
    Friend WithEvents txtEndpoint As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents pagLog As TabPage
    Friend WithEvents txtLog As TextBox
End Class
