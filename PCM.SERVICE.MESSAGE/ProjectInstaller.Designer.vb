<System.ComponentModel.RunInstaller(True)> Partial Class prjInstaller
    Inherits System.Configuration.Install.Installer

    'Descartar substituições de instalador para limpar lista de componentes.
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

    'Exigido pelo Designer de Componentes
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Designer de Componentes
    'Pode ser modificado usando o Designer de Componentes.
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.srvProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.srvInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'srvProcessInstaller
        '
        Me.srvProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService
        Me.srvProcessInstaller.Password = Nothing
        Me.srvProcessInstaller.Username = Nothing
        '
        'srvInstaller
        '
        Me.srvInstaller.Description = "PCM Push Notification"
        Me.srvInstaller.DisplayName = "PCM.SERVICE.MESSAGE"
        Me.srvInstaller.ServiceName = "PCM.SERVICE.MESSAGE"
        Me.srvInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'prjInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.srvProcessInstaller, Me.srvInstaller})

    End Sub

    Friend WithEvents srvProcessInstaller As ServiceProcess.ServiceProcessInstaller
    Friend WithEvents srvInstaller As ServiceProcess.ServiceInstaller
End Class
