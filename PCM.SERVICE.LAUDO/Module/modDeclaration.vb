Module modDeclaration

    'Constantes
    Public Const gDatabase As String = "Data Source=VMI264701\MSSQLSERVER01;Initial Catalog=PCM;Persist Security Info=False;User ID=sa;Password=p@ssw0rd013459;"
    Public Const _Minute = 60000

    Public Structure FibMessage
        Public lCodigo As Long
        Public sTitle As String
        Public sMessage As String
        Public sPriority As String
        Public sToken As String
    End Structure

    Public Structure FibMessagePWA
        Public lCodigo As Long
        Public sUnidade As String
        Public sTitle As String
        Public sBody As String
        Public sTopic As String
        Public sToken As String
    End Structure

    Public Structure Email
        Public lCodigo As Long
        Public sOrdemServico As String
        Public sTo As String
        Public sBody As String
    End Structure

End Module
