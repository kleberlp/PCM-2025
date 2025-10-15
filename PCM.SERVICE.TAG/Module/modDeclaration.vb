Module modDeclaration

    'Constantes
    Public Const gDatabase As String = "Data Source=VMI264701\MSSQLSERVER01;Initial Catalog=PCM;Persist Security Info=False;User ID=sa;Password=p@ssw0rd013459;"
    Public Const _Minute = 60000

    'Structure
    Public Structure Equipamento
        Public iCodigoEmpresa As Integer
        Public iCodigoUnidade As Integer
        Public lCodigo As Long
        Public sCode As String
    End Structure

    Public Structure Rotina
        Public iCodigoEmpresa As Integer
        Public iCodigoUnidade As Integer
        Public lCodigo As Long
        Public sCode As String
    End Structure

    Public Structure Apartamento
        Public sUniqueId As String
    End Structure

End Module
