'==============================================================================================='
'Classe:        SQLHelper                                                                       '
'Versão:        1.0 - 31/01/2008                                                                '
'Desenvolvedor: Kleber Leonardo Pinto                                                           '
'Telefone:      (12) 9744-6734                                                                  '
'E-mail:        kleber.pinto@acticonsultoria.com.br                                             '
'==============================================================================================='
'Objetivo:      Classe de Acesso ao Banco de Dados (Camada de Dados)                            '
'==============================================================================================='
Imports System.Xml
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Public NotInheritable Class SQLHelper

#Region "::: Métodos / Construtor :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Construtor                                                              '
    'PARAMETRO      :                                                                           '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Private Sub New()
    End Sub

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Método utilizado para Atachar um array SQLParamenters ao SQLComand      '
    'PARAMETRO      :   oSQLCommand - Command a qual sera atachado o Parametro                  '
    '                   oSQLParameters - Array de Parametros que sera atachado ao Command       '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Private Shared Sub AttachParameters(ByVal oSQLCommand As SqlCommand, ByVal oSQLParameters() As SqlParameter)

        'Verifica se foi enviado algum Command
        If (oSQLCommand Is Nothing) Then
            Throw New ArgumentNullException("oSQLCommand")
        End If

        'Verifica se foi passado algum Parametro
        If (Not oSQLParameters Is Nothing) Then

            Dim oSQLParameter As SqlParameter

            For Each oSQLParameter In oSQLParameters

                'Verifica se foi Passado algum Parametro
                If (Not oSQLParameter Is Nothing) Then
                    'Verifica se para o Paramentro Passado o Valor dele é vazio
                    If (oSQLParameter.Direction = ParameterDirection.InputOutput OrElse oSQLParameter.Direction = ParameterDirection.Input) AndAlso oSQLParameter.Value Is Nothing Then
                        oSQLParameter.Value = DBNull.Value
                    End If
                    'Adiciona Paramentro ao Command
                    oSQLCommand.Parameters.Add(oSQLParameter)
                End If

            Next oSQLParameter

        End If

    End Sub

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Método utilizado para atribuir valores de um array de paramentros a um  '
    '                   DataRow                                                                 '
    'PARAMETRO      :                                                                           '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Private Overloads Shared Sub AssignParameterValues(ByVal oSqlParameters() As SqlParameter, ByVal oDataRow As DataRow)

        'Verifica se não há paramentros ou Data Row
        If oSqlParameters Is Nothing OrElse oDataRow Is Nothing Then
            'Sai da Sub
            Exit Sub
        End If

        'Seta os Valores dos Parametros
        Dim oSqlPartameter As SqlParameter
        Dim i As Integer

        For Each oSqlPartameter In oSqlParameters
            'Verifica o Nome do Paramentro
            If (oSqlPartameter.ParameterName Is Nothing OrElse oSqlPartameter.ParameterName.Length <= 1) Then
                Throw New Exception(String.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, oSqlPartameter.ParameterName))
            End If

            If oDataRow.Table.Columns.IndexOf(oSqlPartameter.ParameterName.Substring(1)) <> -1 Then
                oSqlPartameter.Value = oDataRow(oSqlPartameter.ParameterName.Substring(1))
            End If
            i = i + 1
        Next

    End Sub

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Método utilizado para atribuir valores de um array de paramentros a um  '
    'PARAMETRO      :                                                                           '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Private Overloads Shared Sub AssignParameterValues(ByVal oSqlParameters() As SqlParameter, ByVal oParameterValues() As Object)

        'Variaveis Locais
        Dim i As Integer
        Dim j As Integer

        'Verifica se existe Parametros
        If (oSqlParameters Is Nothing) AndAlso (oParameterValues Is Nothing) Then
            'Sai da Sub
            Return
        End If

        If oSqlParameters.Length <> oParameterValues.Length Then
            Throw New ArgumentException("Parameter count does not match Parameter Value count.")
        End If

        'Seta Array
        j = oSqlParameters.Length - 1

        For i = 0 To j

            If TypeOf oSqlParameters(i) Is IDbDataParameter Then

                Dim paramInstance As IDbDataParameter = CType(oParameterValues(i), IDbDataParameter)

                If (paramInstance.Value Is Nothing) Then
                    oParameterValues(i).Value = DBNull.Value
                Else
                    oParameterValues(i).Value = paramInstance.Value
                End If

            ElseIf (oParameterValues(i) Is Nothing) Then
                oSqlParameters(i).Value = DBNull.Value
            Else
                oSqlParameters(i).Value = oParameterValues(i)
            End If
        Next

    End Sub

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Método abre (se necesário) e passa a conexão, transaction, command e    '
    '                   paramentros para o command                                              '
    'PARAMETRO      :   oSqlCommand - o SQL Command usado                                       '
    '                   oSqlConnection - a Conexão valida que ira executar o command            '
    '                   oSqlTransaction - um Transaction válido ou Nullo                        '
    '                   oSqlCommandType - Stored Procedure, Text, etc                           '
    '                   oCommandText - o Nome da Stored Procedure ou o commando SQL             '
    '                   oSqlParameters - um array de Parametros a ser associado ao command ou   '
    '                   nullo se o parametro não for requerido                                  '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'        
    Private Shared Sub PrepareCommand(ByVal oSqlCommand As SqlCommand, ByVal oSqlConnection As SqlConnection, ByVal oSqlTransaction As SqlTransaction, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal oSqlParameters() As SqlParameter, ByRef bCloseConnection As Boolean)

        If (oSqlCommand Is Nothing) Then Throw New ArgumentNullException("command")

        If (sCommandText Is Nothing OrElse sCommandText.Length = 0) Then Throw New ArgumentNullException("commandText")

        'Verifica se a Conexão não esta aberta
        If oSqlConnection.State <> ConnectionState.Open Then
            'Abre a Conexão
            oSqlConnection.Open()
            bCloseConnection = True
        Else
            bCloseConnection = False
        End If

        'Associa a Conexão ao Command
        oSqlCommand.Connection = oSqlConnection

        'Seta o Command Text (o Nome da Stored Procedure ou o SQL statement)
        oSqlCommand.CommandText = sCommandText
        oSqlCommand.CommandTimeout = 0

        'Verifica se a Transaction
        If Not (oSqlTransaction Is Nothing) Then
            If oSqlTransaction.Connection Is Nothing Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
            oSqlCommand.Transaction = oSqlTransaction
        End If

        'Seta o Command Type
        oSqlCommand.CommandType = oCommandType

        'Atacha os Parametros ao Command se eles dorem passados
        If Not (oSqlParameters Is Nothing) Then
            AttachParameters(oSqlCommand, oSqlParameters)
        End If

        Return

    End Sub

#End Region

#Region "::: ExecuteNonQuery :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database specificado pela String de Conexão    '
    '                   (não retorna um resultser e não tem parametros)                         '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   oCommandType - Tipo de Command (stored procedure, text, etc)            '
    '                   sCommandText - Nome da Stored Procedure ou Commando SQL                 '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText As String) As Integer

        'Chama rotina ExecuteNonQuery informando que o SQL Parametro é igual a Nulo
        Return ExecuteNonQuery(sConnectionString, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database specificado pela String de Conexão    '
    '                   usando Parametros passados                                              '
    '                   (não retorna um resultser)                                              '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   oCommandType - Tipo de Command (stored procedure, text, etc)            '
    '                   sCommandText - Nome da Stored Procedure ou Commando SQL                 '
    '                   oSqlParameters - Array de SqlParameters usado para executar o Command   '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As Integer

        'Verifica se a uma String de Conexão
        If (sConnectionString Is Nothing OrElse sConnectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")

        'Cria e abre uma SqlConnection, e libera da memória após ser usada
        Dim oSqlConnection As SqlConnection

        Try

            oSqlConnection = New SqlConnection(sConnectionString)
            oSqlConnection.Open()

            'Chamar a sobrecarga que tenha uma conexão em vez da conexão string
            Return ExecuteNonQuery(oSqlConnection, oCommandType, sCommandText, oSqlParameters)

        Finally

            If Not oSqlConnection Is Nothing Then oSqlConnection.Dispose()

        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure no Database especificado pela Strind de    '
    '                   conexão via um SqlCommand usando os paramentros passados. Este método   '
    '                   devera descobrir os parametros para stored procedure, e atribuir os     '
    '                   os valores baseado na ordem dos parametros                              '
    '                   (não retorna um resultser)                                              '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   oSPName - Nome da Stored Procedure                                      '
    '                   oParameterValues - Array de SqlParameters usado para executar o Command '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal sConnectionString As String, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As Integer

        'Verifica se foi passado a String de Conexçao
        If (sConnectionString Is Nothing OrElse sConnectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        'Verifica se foi passado o nome da Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variavies Locais
        Dim oSqlParameters As SqlParameter()

        'Se for passado valores para parametros
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Puxa os parametros do Cache para a Stored Procedure (ou descobre e povoa o cache)
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(sConnectionString, sSPName)

            'Atribui os valores fornecidos para o Parametro baseado na ordem deles
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Método de sobrecarga que tem uma variedade de SqlParameters
            Return ExecuteNonQuery(sConnectionString, CommandType.StoredProcedure, sSPName, oSqlParameters)

        Else

            'Chama a Stored Procedure sem passar parametros
            Return ExecuteNonQuery(sConnectionString, CommandType.StoredProcedure, sSPName)

        End If

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand que não retorna um resultset e nem precisa de     '
    '                   parametros apenas do Objeto de Conexão                                  '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommandType - Tipo de Commando (stored procedure, text, etc)           '
    '                   oCommandText - O Nome da Stored Procedure ou o Commando SQL             '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlConnection As SqlConnection, ByVal oCommandType As CommandType, ByVal sCommandText As String) As Integer

        'Chama Método ExecuteNonQuery passando Null como Parametros
        Return ExecuteNonQuery(oSqlConnection, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand que não retorna um resultset atraves da Conexão   '
    '                   usandos os parametros fornecidos                                        '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommandType - Tipo de Commando (stored procedure, text, etc)           '
    '                   oCommandText - O Nome da Stored Procedure ou o Commando SQL             '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlConnection As SqlConnection, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As Integer

        'Verifica se foi passado uma conexão
        If (oSqlConnection Is Nothing) Then Throw New ArgumentNullException("connection")

        'Cria um command e prepara ele para a Execução
        Dim oSqlCommand As New SqlCommand
        Dim iRetval As Integer
        Dim bCloseConnection As Boolean = False

        PrepareCommand(oSqlCommand, oSqlConnection, CType(Nothing, SqlTransaction), oCommandType, sCommandText, oSqlParameters, bCloseConnection)

        'Executa o Command
        iRetval = oSqlCommand.ExecuteNonQuery()

        'Detach o SqlParameters do objeto command, para que eles possam ser usados novamente
        oSqlCommand.Parameters.Clear()

        'Verifica a Necessidade de fechar a Conexão
        If (bCloseConnection) Then oSqlConnection.Close()

        'Retorna o Número de linhas afetadas
        Return iRetval

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure via um SqlCommand (não retorna resutset)   '
    '                   através do SqlConnection, usandos os parametros fornecidos. Este Método '
    '                   ira descobrir os parametros da Stored Procedure e atribuir valores      '
    '                   conforme a ordem de parametros passada                                  '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   sSPName - Nome da Stored Procedure                                      '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlConnection As SqlConnection, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As Integer

        'Verifica se a Conexão é válida
        If (oSqlConnection Is Nothing) Then Throw New ArgumentNullException("connection")
        'Verifica se foi passado um nome para a Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variaveis Locais
        Dim oSqlParameters As SqlParameter()

        'Se for passado valores para parametros
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Puxa os parametros do Cache para a Stored Procedure (ou descobre e povoa o cache)
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(oSqlConnection, sSPName)

            'Atribui os valores fornecidos para o Parametro baseado na ordem deles
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Chama método de sobrecarga que necessita do Parametros (SqlParameters)
            Return ExecuteNonQuery(oSqlConnection, CommandType.StoredProcedure, sSPName, oSqlParameters)

        Else

            'Chama a Stored Procedure sem passar parametros
            Return ExecuteNonQuery(oSqlConnection, CommandType.StoredProcedure, sSPName)

        End If

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand que não retorna um resultset e não necessita de   '
    '                   parametros através do SqlTransaction informado                          '
    'PARAMETRO      :   oSqlTransaction - uma transação valida associada a uma conexão          '
    '                   oCommandType - Tipo de Commando (stored procedure, text, etc)           '
    '                   oCommandText - O Nome da Stored Procedure ou o Commando SQL             '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlTransaction As SqlTransaction, ByVal oCommandType As CommandType, ByVal sCommandText As String) As Integer

        'Chama Função ExecuteNonQuery passando SQLParameters = NULL
        Return ExecuteNonQuery(oSqlTransaction, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand que não retorna um resultset atravéd de um        '
    '                   SqlTransaction informado, usando os parametros informados               '
    'PARAMETRO      :   oSqlTransaction - uma transação valida associada a uma conexão          '
    '                   oCommandType - Tipo de Commando (stored procedure, text, etc)           '
    '                   oCommandText - O Nome da Stored Procedure ou o Commando SQL             '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlTransaction As SqlTransaction, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As Integer

        'Verifica se existe uma Transação
        If (oSqlTransaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        'Verifica se a Transação esta aberta
        If Not (oSqlTransaction Is Nothing) AndAlso (oSqlTransaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

        'Variaveis Locais
        Dim oSqlCommand As New SqlCommand
        Dim iRetval As Integer
        Dim bCloseConnection As Boolean = False

        'Prepara o Command
        PrepareCommand(oSqlCommand, oSqlTransaction.Connection, oSqlTransaction, oCommandType, sCommandText, oSqlParameters, bCloseConnection)

        'Executa o Command
        iRetval = oSqlCommand.ExecuteNonQuery()

        'Detach o SqlParameters do objeto command, para que possa ser usado novamente
        oSqlCommand.Parameters.Clear()

        Return iRetval

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure usando um SqlCommand com um SqlTransaction '
    '                   informado, com parametros                                               '
    'PARAMETRO      :   oSqlTransaction - uma transação valida associada a uma conexão          '
    '                   sSPName - Nome da Stored Procedure                                      '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   o Número de Linhas afetadas                                             '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'    
    Public Overloads Shared Function ExecuteNonQuery(ByVal oSqlTransaction As SqlTransaction, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As Integer

        'Verifica se foi informado uma Transação
        If (oSqlTransaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        'Verifica se a Transação esta aberta
        If Not (oSqlTransaction Is Nothing) AndAlso (oSqlTransaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        'Verifica se foi informado o Nome da Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variaveis Locais
        Dim oSqlParameters As SqlParameter()

        'Verifica se foi recebido Parametros
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Puxa do Cache os parametros para Stored Procedure (ou descobre e então povoa o Cache
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(oSqlTransaction.Connection, sSPName)

            'Atribui os valores fornecidos ao SqlParameters através da ordem
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Executa Procedure com passagem de Parametros
            Return ExecuteNonQuery(oSqlTransaction, CommandType.StoredProcedure, sSPName, oSqlParameters)
        Else

            'Executa Procedure sem passagem de Parametros
            Return ExecuteNonQuery(oSqlTransaction, CommandType.StoredProcedure, sSPName)

        End If

    End Function

#End Region

#Region "::: ExecuteDataset :::"

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database informado através de uma String de    '
    '                   Conexão, retorna um resultset e não necessita de Parametros             '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'    
    Public Overloads Shared Function ExecuteDataset(ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText As String) As DataSet

        'Chama Função Execute DataSet passando Null como Parametro
        Return ExecuteDataset(sConnectionString, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database informado através de uma String de    '
    '                   Conexão, retorna um resultset, necessita de Parametros                  '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As DataSet

        'Verifica se foi passado a String de Conexão
        If (sConnectionString Is Nothing OrElse sConnectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")

        'Cria e Abre um SqlConnection, e liberam da memória após ter sido usado
        Dim oSqlConnection As SqlConnection

        Try
            oSqlConnection = New SqlConnection(sConnectionString)
            oSqlConnection.Open()

            'Executa a Função ExecuteDataSet passando a Conexão e não a String
            Return ExecuteDataset(oSqlConnection, oCommandType, sCommandText, oSqlParameters)

        Finally

            'Fecha a Conexão
            If Not oSqlConnection Is Nothing Then oSqlConnection.Dispose()

        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure através de um SqlCommand, usando o Database'
    '                   especificado na String de Conexão, retorna um resultset.                '
    '                   Este Método devera descobrir os Parametros da SP, e atribuir valores    '
    '                   baseados na ordem informada                                             '
    'PARAMETRO      :   sConnectionString - String de Conexão                                   '
    '                   sSPName - Nome da Stored Procedure                                      '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal sConnectionString As String, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As DataSet

        'Verifica se foi informado a String de Conexão
        If (sConnectionString Is Nothing OrElse sConnectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        'Verifica se foi informado a Stored Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variaveis Locais
        Dim oSqlParameters As SqlParameter()

        'Verifica se foi passado algum Parametro
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Carrega os parametros da stored procedure do Cache (ou descobre e povoa o Cache)
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(sConnectionString, sSPName)

            'Atribui valores aos Parametros baseados na ordem do parametros fornecidos
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Chama a Função ExecuteDataSet passando Parametros
            Return ExecuteDataset(sConnectionString, CommandType.StoredProcedure, sSPName, oSqlParameters)

        Else

            'Executa a Stored Procedure sem passar Parametros
            Return ExecuteDataset(sConnectionString, CommandType.StoredProcedure, sSPName)

        End If

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database informado através de uma Conexão,     '
    '                   retorna um resultset, necessita de Parametros                           '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oSqlConnection As SqlConnection, ByVal oCommandType As CommandType, ByVal sCommandText As String) As DataSet

        'Chama Função ExecuteDataSet passando null como SqlParameters
        Return ExecuteDataset(oSqlConnection, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database informado através de uma Conexão      '
    '                   retorna um resultset                                                    '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal OSqlConnection As SqlConnection, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As DataSet

        'Verifica se foi passado uma Conexão
        If (OSqlConnection Is Nothing) Then Throw New ArgumentNullException("connection")

        'Variaveis Locais
        Dim oSqlCommand As New SqlCommand
        Dim oDataSet As New DataSet
        Dim oSqlDataAdapter As SqlDataAdapter
        Dim bCloseConnection As Boolean = False

        'Prepara o Command
        PrepareCommand(oSqlCommand, OSqlConnection, CType(Nothing, SqlTransaction), oCommandType, sCommandText, oSqlParameters, bCloseConnection)

        Try
            'Cria uma DataAdapter e um DataSet
            oSqlDataAdapter = New SqlDataAdapter(oSqlCommand)

            'Preenche o DataSet usando os valores default para o Nome do DataTable
            oSqlDataAdapter.Fill(oDataSet, "main")

            'Detach o SqlParameters do objeto commando, para que possa ser usado novamente
            oSqlCommand.Parameters.Clear()

        Finally

            'Libera da memória o DataAdapter
            If (Not oSqlDataAdapter Is Nothing) Then oSqlDataAdapter.Dispose()

        End Try

        'Fecha a Conexão se necessário
        If (bCloseConnection) Then OSqlConnection.Close()

        'Retorna o DataSet
        Return oDataSet

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no Database informado através de uma Conexão      '
    '                   retorna um resultset                                                    '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oDataSet As DataSet, ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter)

        'Cria e Abre um SqlConnection, e liberam da memória após ter sido usado
        Dim oSqlConnection As SqlConnection

        Try
            oSqlConnection = New SqlConnection(sConnectionString)
            oSqlConnection.Open()

            'Verifica se foi passado uma Conexão
            If (oSqlConnection Is Nothing) Then Throw New ArgumentNullException("connection")

            'Variaveis Locais
            Dim oSqlCommand As New SqlCommand
            Dim oSqlDataAdapter As SqlDataAdapter
            Dim bCloseConnection As Boolean = False

            'Prepara o Command
            PrepareCommand(oSqlCommand, oSqlConnection, CType(Nothing, SqlTransaction), oCommandType, sCommandText, oSqlParameters, bCloseConnection)

            Try
                'Cria uma DataAdapter e um DataSet
                oSqlDataAdapter = New SqlDataAdapter(oSqlCommand)

                'Preenche o DataSet usando os valores default para o Nome do DataTable
                oSqlDataAdapter.Fill(oDataSet, "main")

                'Detach o SqlParameters do objeto commando, para que possa ser usado novamente
                oSqlCommand.Parameters.Clear()

            Finally

                'Libera da memória o DataAdapter
                If (Not oSqlDataAdapter Is Nothing) Then oSqlDataAdapter.Dispose()

            End Try

            'Fecha a Conexão se necessário
            If (bCloseConnection) Then oSqlConnection.Close()

        Finally

            'Fecha a Conexão
            If Not oSqlConnection Is Nothing Then oSqlConnection.Dispose()

        End Try

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure através de um SqlCommand, usando a Conexão '
    '                   especificada, retorna um resultset.                                     '
    '                   Este Método devera descobrir os Parametros da SP, e atribuir valores    '
    '                   baseados na ordem informada                                             '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   sSPName - Nome da Stored Procedure                                      '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '    
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oSqlConnection As SqlConnection, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As DataSet

        'Verifica se foi informado o Objeto de Conexão
        If (oSqlConnection Is Nothing) Then Throw New ArgumentNullException("connection")
        'Verifica se foi informado a Stored Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variaveis Locais
        Dim oSqlParameters As SqlParameter()

        'Verifica se foi recebido Parametros
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Puxa para a stored procedure os Parametros do Cache (ou descobre e povoa o Cache)
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(oSqlConnection, sSPName)

            'Atribui os valores aos parametros baseado na ordem dos parametros fornecidos
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Chama a função ExecuteDataSet passando Parametros
            Return ExecuteDataset(oSqlConnection, CommandType.StoredProcedure, sSPName, oSqlParameters)

        Else

            'Chama a função ExecuteDataSet passando sem passar Parametros
            Return ExecuteDataset(oSqlConnection, CommandType.StoredProcedure, sSPName)

        End If

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no SqlTramnsaction Informado                      '
    '                   retorna um resultset, e não necessita de Parametros                     '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oSqlTransaction As SqlTransaction, ByVal oCommandType As CommandType, ByVal sCommandText As String) As DataSet

        'Chama a Função ExecuteDataSet passando Parametros como Null
        Return ExecuteDataset(oSqlTransaction, oCommandType, sCommandText, CType(Nothing, SqlParameter()))

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa um SqlCommand no SqlTramnsaction Informado                      '
    '                   retorna um resultset, necessita de Parametros                           '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   oCommantType - Tipo de Commando (Stored Procedure, Text, etc.)          '
    '                   sCommandText - Nome da Stored Procedure ou comando SQL                  '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oSqlTransaction As SqlTransaction, ByVal oCommandType As CommandType, ByVal sCommandText As String, ByVal ParamArray oSqlParameters() As SqlParameter) As DataSet

        'Verifica se foi informado a Transação
        If (oSqlTransaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        'Verifica se a Transação esta aberta
        If Not (oSqlTransaction Is Nothing) AndAlso (oSqlTransaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

        'Variaveis Locais
        Dim oSqlCommand As New SqlCommand
        Dim oDataSet As New DataSet
        Dim oSqlDataAdapter As SqlDataAdapter
        Dim bCloseConnection As Boolean = False

        'Prepara o Command
        PrepareCommand(oSqlCommand, oSqlTransaction.Connection, oSqlTransaction, oCommandType, sCommandText, oSqlParameters, bCloseConnection)

        Try

            'Cria um Data Adapter e um DataSet
            oSqlDataAdapter = New SqlDataAdapter(oSqlCommand)

            'Preenche o DataSet usando os valores defalt da DataTable name
            oSqlDataAdapter.Fill(oDataSet, "main")

            'Detach o SqlParameters do Objeto Command, para que possa ser utilizado mais tarde
            oSqlCommand.Parameters.Clear()

        Finally

            'Libera da Memória o DataAdapter
            If (Not oSqlDataAdapter Is Nothing) Then oSqlDataAdapter.Dispose()

        End Try

        'Retorna o DataSet
        Return oDataSet

    End Function

    '-------------------------------------------------------------------------------------------'
    'DESCRIÇÃO      :   Executa uma Stored Procedure através de um SqlCommand, usando um        '
    '                   SQLTransaction especificado, retorna um resultset.                                     '
    '                   Este Método devera descobrir os Parametros da SP, e atribuir valores    '
    '                   baseados na ordem informada                                             '
    'PARAMETRO      :   oSqlConnection - Objeto de Conexão                                      '
    '                   sSPName - Nome da Stored Procedure                                      '
    '                   oSqlParameters - Array de SqlParameter usados para executar o Command   '
    'RETURNS        :   Um DataSet contendo o resultsql gerado pelo command                     '    
    'DATA           :   31/01/2008                                                              '
    'DESENVOLVEDOR  :   Kleber Leonardo Pinto                                                   '
    '-------------------------------------------------------------------------------------------'
    Public Overloads Shared Function ExecuteDataset(ByVal oSqlTransaction As SqlTransaction, ByVal sSPName As String, ByVal ParamArray oParameterValues() As Object) As DataSet

        'Verifica se foi informado uma transação
        If (oSqlTransaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        'Verifica se a transação informada está aberta
        If Not (oSqlTransaction Is Nothing) AndAlso (oSqlTransaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        'Verifica se foi informado uma Stored Procedure
        If (sSPName Is Nothing OrElse sSPName.Length = 0) Then Throw New ArgumentNullException("spName")

        'Variaveis Locais
        Dim oSqlParameters As SqlParameter()

        'Verifica se foi passado algum parametro
        If Not (oParameterValues Is Nothing) AndAlso oParameterValues.Length > 0 Then

            'Carrega para a Stored Procedure os parametros do Cache (ou descobre e povoa o Cache)
            oSqlParameters = SqlHelperParameterCache.GetSpParameterSet(oSqlTransaction.Connection, sSPName)

            'Atribui os valores aos parametros de acordo com a ordem dos parametros fornecidos
            AssignParameterValues(oSqlParameters, oParameterValues)

            'Executa a Função ExecuteDataSet passando SqlParameters
            Return ExecuteDataset(oSqlTransaction, CommandType.StoredProcedure, sSPName, oSqlParameters)

        Else

            'Executa a Função ExecuteDataSet não passando SqlParameters
            Return ExecuteDataset(oSqlTransaction, CommandType.StoredProcedure, sSPName)

        End If

    End Function

    Public Overloads Shared Function ExecuteDatasetChild(ByVal sConnectionString As String, ByVal oCommandType As CommandType, ByVal sCommandText() As String, ByVal sTableName() As String, ByVal ParamArray oSqlParameters() As SqlParameter) As DataSet

        'Verifica se foi informado a String de Conexão
        If (sConnectionString Is Nothing OrElse sConnectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")

        'Variaveis Locais
        Dim oSqlCommand As New SqlCommand
        Dim oDataSet As New DataSet
        Dim oSqlDataAdapter As SqlDataAdapter
        Dim bCloseConnection As Boolean = False
        'Cria e Abre um SqlConnection, e liberam da memória após ter sido usado
        Dim oSqlConnection As SqlConnection

        Try


            oSqlConnection = New SqlConnection(sConnectionString)
            oSqlConnection.Open()

            Dim i As Integer

            For i = 0 To UBound(sCommandText)
                'Prepara o Command
                PrepareCommand(oSqlCommand, oSqlConnection, CType(Nothing, SqlTransaction), oCommandType, sCommandText(i), oSqlParameters, bCloseConnection)
                'Cria um Data Adapter e um DataSet
                oSqlDataAdapter = New SqlDataAdapter(oSqlCommand)
                'Preenche o DataSet usando os valores defalt da DataTable name
                oSqlDataAdapter.Fill(oDataSet, sTableName(i))
                'Detach o SqlParameters do Objeto Command, para que possa ser utilizado mais tarde
                oSqlCommand.Parameters.Clear()
            Next

        Finally

            If Not (oSqlConnection Is Nothing) Then oSqlConnection.Dispose()

            'Libera da Memória o DataAdapter
            If (Not oSqlDataAdapter Is Nothing) Then oSqlDataAdapter.Dispose()

        End Try

        'Retorna o DataSet
        Return oDataSet

    End Function

#End Region

#Region "ExecuteReader"
    ' this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
    ' we can set the appropriate CommandBehavior when calling ExecuteReader()
    Private Enum SqlConnectionOwnership
        ' Connection is owned and managed by SqlHelper
        Internal
        ' Connection is owned and managed by the caller
        [External]
    End Enum ' SqlConnectionOwnership

    ' Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
    ' If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
    ' If the caller provided the connection, we want to leave it to them to manage.
    ' Parameters:
    ' -connection - a valid SqlConnection, on which to execute this command 
    ' -transaction - a valid SqlTransaction, or ' null' 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParameters to be associated with the command or ' null' if no parameters are required 
    ' -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by SqlHelper 
    ' Returns: SqlDataReader containing the results of the command 
    Private Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
                                                    ByVal transaction As SqlTransaction, _
                                                    ByVal commandType As CommandType, _
                                                    ByVal commandText As String, _
                                                    ByVal commandParameters() As SqlParameter, _
                                                    ByVal connectionOwnership As SqlConnectionOwnership) As SqlDataReader

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")

        Dim mustCloseConnection As Boolean = False
        ' Create a command and prepare it for execution
        Dim cmd As New SqlCommand
        Try
            ' Create a reader
            Dim dataReader As SqlDataReader

            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, mustCloseConnection)

            ' Call ExecuteReader with the appropriate CommandBehavior
            If connectionOwnership = SqlConnectionOwnership.External Then
                dataReader = cmd.ExecuteReader()
            Else
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            End If

            ' Detach the SqlParameters from the command object, so they can be used again
            Dim canClear As Boolean = True
            Dim commandParameter As SqlParameter
            For Each commandParameter In cmd.Parameters
                If commandParameter.Direction <> ParameterDirection.Input Then
                    canClear = False
                End If
            Next

            If (canClear) Then cmd.Parameters.Clear()

            Return dataReader
        Catch
            If (mustCloseConnection) Then connection.Close()
            Throw
        End Try
    End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    ' the connection string. 
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As SqlDataReader
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteReader(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")

        ' Create & open a SqlConnection
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)
            connection.Open()
            ' Call the private overload that takes an internally owned connection in place of the connection string
            Return ExecuteReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.Internal)
        Catch
            ' If we fail to return the SqlDatReader, we need to close the connection ourselves
            If Not connection Is Nothing Then connection.Dispose()
            Throw
        End Try
    End Function ' ExecuteReader

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    ' the connection string using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(connString, "GetOrders", 24, 36)
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    'Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
    '                                               ByVal spName As String, _
    '                                               ByVal ParamArray parameterValues() As Object) As SqlDataReader
    '    If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
    '    If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

    '    Dim commandParameters As SqlParameter()

    '    ' If we receive parameter values, we need to figure out where they go
    '    If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
    '        ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
    '        commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

    '        ' Assign the provided values to these parameters based on parameter order
    '        AssignParameterValues(commandParameters, parameterValues)

    '        ' Call the overload that takes an array of SqlParameters
    '        Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters)
    '        ' Otherwise we can just call the SP without params
    '    Else
    '        Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName)
    '    End If
    'End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As SqlDataReader

        Return ExecuteReader(connection, commandType, commandText, CType(Nothing, SqlParameter()))

    End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
        ' Pass through the call to private overload using a null transaction value
        Return ExecuteReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.External)

    End Function ' ExecuteReader

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(conn, "GetOrders", 24, 36)
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
                                                   ByVal spName As String, _
                                                   ByVal ParamArray parameterValues() As Object) As SqlDataReader
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()
        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            AssignParameterValues(commandParameters, parameterValues)

            Return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else ' Otherwise we can just call the SP without params
            Return ExecuteReader(connection, CommandType.StoredProcedure, spName)
        End If

    End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -transaction - a valid SqlTransaction  
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As SqlDataReader
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteReader(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteReader

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    ' using the provided parameters.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -transaction - a valid SqlTransaction 
    ' -commandType - the CommandType (stored procedure, text, etc.)
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: A SqlDataReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        ' Pass through to private overload, indicating that the connection is owned by the caller
        Return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External)
    End Function ' ExecuteReader

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim dr As SqlDataReader = ExecuteReader(trans, "GetOrders", 24, 36)
    ' Parameters:
    ' -transaction - a valid SqlTransaction 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
    ' Returns: A SqlDataReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
                                                   ByVal spName As String, _
                                                   ByVal ParamArray parameterValues() As Object) As SqlDataReader
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()

        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            AssignParameterValues(commandParameters, parameterValues)

            Return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else ' Otherwise we can just call the SP without params
            Return ExecuteReader(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function ' ExecuteReader

#End Region

#Region "ExecuteScalar"

    ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
    ' the connection string. 
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command
    Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As Object
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteScalar(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteScalar

    ' Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim orderCount As Integer = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As Object
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        ' Create & open a SqlConnection, and dispose of it after we are done.
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)
            connection.Open()

            ' Call the overload that takes a connection in place of the connection string
            Return ExecuteScalar(connection, commandType, commandText, commandParameters)
        Finally
            If Not connection Is Nothing Then connection.Dispose()
        End Try
    End Function ' ExecuteScalar

    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
    ' the connection string using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, "GetOrderCount", 24, 36))
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
                                                   ByVal spName As String, _
                                                   ByVal ParamArray parameterValues() As Object) As Object
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()

        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            ' Otherwise we can just call the SP without params
        Else
            Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName)
        End If
    End Function ' ExecuteScalar

    ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As Object
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteScalar(connection, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteScalar

    ' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As Object

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")

        ' Create a command and prepare it for execution
        Dim cmd As New SqlCommand
        Dim retval As Object
        Dim mustCloseConnection As Boolean = False

        PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, mustCloseConnection)

        ' Execute the command & return the results
        retval = cmd.ExecuteScalar()

        ' Detach the SqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear()

        If (mustCloseConnection) Then connection.Close()

        Return retval

    End Function ' ExecuteScalar

    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, "GetOrderCount", 24, 36))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
                                                   ByVal spName As String, _
                                                   ByVal ParamArray parameterValues() As Object) As Object
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()

        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            Return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else ' Otherwise we can just call the SP without params
            Return ExecuteScalar(connection, CommandType.StoredProcedure, spName)
        End If

    End Function ' ExecuteScalar

    ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction.
    ' e.g.:  
    ' Dim orderCount As Integer  = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
    ' Parameters:
    ' -transaction - a valid SqlTransaction 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String) As Object
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteScalar(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteScalar

    ' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
    ' using the provided parameters.
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
    ' Parameters:
    ' -transaction - a valid SqlTransaction  
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
                                                   ByVal commandType As CommandType, _
                                                   ByVal commandText As String, _
                                                   ByVal ParamArray commandParameters() As SqlParameter) As Object
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

        ' Create a command and prepare it for execution
        Dim cmd As New SqlCommand
        Dim retval As Object
        Dim mustCloseConnection As Boolean = False

        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, mustCloseConnection)

        ' Execute the command & return the results
        retval = cmd.ExecuteScalar()

        ' Detach the SqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear()

        Return retval
    End Function ' ExecuteScalar

    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim orderCount As Integer = CInt(ExecuteScalar(trans, "GetOrderCount", 24, 36))
    ' Parameters:
    ' -transaction - a valid SqlTransaction 
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: An object containing the value in the 1x1 resultset generated by the command 
    Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
                                                   ByVal spName As String, _
                                                   ByVal ParamArray parameterValues() As Object) As Object
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()
        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else ' Otherwise we can just call the SP without params
            Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function ' ExecuteScalar

#End Region

#Region "ExecuteXmlReader"

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' Returns: An XmlReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
                                                      ByVal commandType As CommandType, _
                                                      ByVal commandText As String) As XmlReader
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteXmlReader(connection, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteXmlReader

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameters.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: An XmlReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
                                                      ByVal commandType As CommandType, _
                                                      ByVal commandText As String, _
                                                      ByVal ParamArray commandParameters() As SqlParameter) As XmlReader
        ' Pass through the call using a null transaction value
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        ' Create a command and prepare it for execution
        Dim cmd As New SqlCommand
        Dim mustCloseConnection As Boolean = False
        Try
            Dim retval As XmlReader

            PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, mustCloseConnection)

            ' Create the DataAdapter & DataSet
            retval = cmd.ExecuteXmlReader()

            ' Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear()

            Return retval
        Catch
            If (mustCloseConnection) Then connection.Close()
            Throw
        End Try

    End Function ' ExecuteXmlReader

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(conn, "GetOrders", 24, 36)
    ' Parameters:
    ' -connection - a valid SqlConnection 
    ' -spName - the name of the stored procedure using "FOR XML AUTO" 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: An XmlReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
                                                      ByVal spName As String, _
                                                      ByVal ParamArray parameterValues() As Object) As XmlReader
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()

        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters)
            ' Otherwise we can just call the SP without params
        Else
            Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName)
        End If
    End Function ' ExecuteXmlReader


    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders")
    ' Parameters:
    ' -transaction - a valid SqlTransaction
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' Returns: An XmlReader containing the resultset generated by the command 
    Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
                                                      ByVal commandType As CommandType, _
                                                      ByVal commandText As String) As XmlReader
        ' Pass through the call providing null for the set of SqlParameters
        Return ExecuteXmlReader(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
    End Function ' ExecuteXmlReader

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    ' using the provided parameters.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -transaction - a valid SqlTransaction
    ' -commandType - the CommandType (stored procedure, text, etc.) 
    ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
    ' -commandParameters - an array of SqlParamters used to execute the command 
    ' Returns: An XmlReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
                                                      ByVal commandType As CommandType, _
                                                      ByVal commandText As String, _
                                                      ByVal ParamArray commandParameters() As SqlParameter) As XmlReader
        ' Create a command and prepare it for execution
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

        Dim cmd As New SqlCommand

        Dim retval As XmlReader
        Dim mustCloseConnection As Boolean = False

        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, mustCloseConnection)

        ' Create the DataAdapter & DataSet
        retval = cmd.ExecuteXmlReader()

        ' Detach the SqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear()

        Return retval

    End Function ' ExecuteXmlReader

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
    ' using the provided parameter values.  This method will discover the parameters for the 
    ' stored procedure, and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' Dim r As XmlReader = ExecuteXmlReader(trans, "GetOrders", 24, 36)
    ' Parameters:
    ' -transaction - a valid SqlTransaction
    ' -spName - the name of the stored procedure 
    ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
    ' Returns: A dataset containing the resultset generated by the command
    Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
                                                      ByVal spName As String, _
                                                      ByVal ParamArray parameterValues() As Object) As XmlReader
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        Dim commandParameters As SqlParameter()

        ' If we receive parameter values, we need to figure out where they go
        If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
            ' Otherwise we can just call the SP without params
        Else
            Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function ' ExecuteXmlReader

#End Region

#Region "FillDataset"
    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    ' the connection string. 
    ' e.g.:  
    '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
    ' Parameters:    
    ' -connectionString: A valid connection string for a SqlConnection
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '               by a user defined name (probably the actual table name)
    Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String)

        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")

        ' Create & open a SqlConnection, and dispose of it after we are done
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)

            connection.Open()

            ' Call the overload that takes a connection in place of the connection string
            FillDataset(connection, commandType, commandText, dataSet, tableNames)
        Finally
            If Not connection Is Nothing Then connection.Dispose()
        End Try
    End Sub

    ' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
    ' using the provided parameters.
    ' e.g.:  
    '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() = {"orders"}, new SqlParameter("@prodid", 24))
    ' Parameters:    
    ' -connectionString: A valid connection string for a SqlConnection
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '               by a user defined name (probably the actual table name)
    ' -commandParameters: An array of SqlParamters used to execute the command
    Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, _
        ByVal tableNames() As String, ByVal ParamArray commandParameters() As SqlParameter)

        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")

        ' Create & open a SqlConnection, and dispose of it after we are done
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)

            connection.Open()

            ' Call the overload that takes a connection in place of the connection string
            FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters)
        Finally
            If Not connection Is Nothing Then connection.Dispose()
        End Try
    End Sub

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    ' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, 24)
    ' Parameters:
    ' -connectionString: A valid connection string for a SqlConnection
    ' -spName: the name of the stored procedure
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '             by a user defined name (probably the actual table name)
    ' -parameterValues: An array of objects to be assigned As the input values of the stored procedure
    Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableNames As String(), ByVal ParamArray parameterValues() As Object)

        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")

        ' Create & open a SqlConnection, and dispose of it after we are done
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)

            connection.Open()

            ' Call the overload that takes a connection in place of the connection string
            FillDataset(connection, spName, dataSet, tableNames, parameterValues)
        Finally
            If Not connection Is Nothing Then connection.Dispose()
        End Try
    End Sub

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
    ' e.g.:  
    '   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
    ' Parameters:
    ' -connection: A valid SqlConnection
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    ' by a user defined name (probably the actual table name)
    Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames As String())

        FillDataset(connection, commandType, commandText, dataSet, tableNames, Nothing)

    End Sub

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameters.
    ' e.g.:  
    '   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connection: A valid SqlConnection
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    ' by a user defined name (probably the actual table name)
    ' -commandParameters: An array of SqlParamters used to execute the command
    Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
    ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames As String(), _
        ByVal ParamArray commandParameters() As SqlParameter)

        FillDataset(connection, Nothing, commandType, commandText, dataSet, tableNames, commandParameters)

    End Sub

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the provided parameter values.  This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    ' FillDataset (conn, "GetOrders", ds, new string() {"orders"}, 24, 36)
    ' Parameters:
    ' -connection: A valid SqlConnection
    ' -spName: the name of the stored procedure
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '             by a user defined name (probably the actual table name)
    ' -parameterValues: An array of objects to be assigned as the input values of the stored procedure
    Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataSet As DataSet, _
        ByVal tableNames() As String, ByVal ParamArray parameterValues() As Object)

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If we receive parameter values, we need to figure out where they go
        If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters)
        Else ' Otherwise we can just call the SP without params
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames)
        End If

    End Sub

    ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
    ' e.g.:  
    '   FillDataset (trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"})
    ' Parameters:
    ' -transaction: A valid SqlTransaction
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '             by a user defined name (probably the actual table name)
    Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String)

        FillDataset(transaction, commandType, commandText, dataSet, tableNames, Nothing)
    End Sub

    ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
    ' using the provided parameters.
    ' e.g.:  
    '   FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"}, new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -transaction: A valid SqlTransaction
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    ' by a user defined name (probably the actual table name)
    ' -commandParameters: An array of SqlParamters used to execute the command
    Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String, _
        ByVal ParamArray commandParameters() As SqlParameter)

        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters)

    End Sub

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
    ' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' This method provides no access to output parameters or the stored procedure' s return value parameter.
    ' e.g.:  
    '   FillDataset(trans, "GetOrders", ds, new String(){"orders"}, 24, 36)
    ' Parameters:
    ' -transaction: A valid SqlTransaction
    ' -spName: the name of the stored procedure
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '             by a user defined name (probably the actual table name)
    ' -parameterValues: An array of objects to be assigned as the input values of the stored procedure
    Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableNames() As String, ByVal ParamArray parameterValues() As Object)

        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If we receive parameter values, we need to figure out where they go
        If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues)

            ' Call the overload that takes an array of SqlParameters
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters)
        Else ' Otherwise we can just call the SP without params
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames)
        End If
    End Sub

    ' Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
    ' using the provided parameters.
    ' e.g.:  
    '   FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
    ' Parameters:
    ' -connection: A valid SqlConnection
    ' -transaction: A valid SqlTransaction
    ' -commandType: the CommandType (stored procedure, text, etc.)
    ' -commandText: the stored procedure name or T-SQL command
    ' -dataSet: A dataset wich will contain the resultset generated by the command
    ' -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
    '             by a user defined name (probably the actual table name)
    ' -commandParameters: An array of SqlParamters used to execute the command
    Private Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String, _
        ByVal ParamArray commandParameters() As SqlParameter)

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")

        ' Create a command and prepare it for execution
        Dim command As New SqlCommand

        Dim mustCloseConnection As Boolean = False
        PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, mustCloseConnection)

        ' Create the DataAdapter & DataSet
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(command)

        Try
            ' Add the table mappings specified by the user
            If Not tableNames Is Nothing AndAlso tableNames.Length > 0 Then

                Dim tableName As String = "Table"
                Dim index As Integer

                For index = 0 To tableNames.Length - 1
                    If (tableNames(index) Is Nothing OrElse tableNames(index).Length = 0) Then Throw New ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames")
                    dataAdapter.TableMappings.Add(tableName, tableNames(index))
                    tableName = tableName & (index + 1).ToString()
                Next
            End If

            ' Fill the DataSet using default values for DataTable names, etc
            dataAdapter.Fill(dataSet)

            ' Detach the SqlParameters from the command object, so they can be used again
            command.Parameters.Clear()
        Finally
            If (Not dataAdapter Is Nothing) Then dataAdapter.Dispose()
        End Try

        If (mustCloseConnection) Then connection.Close()

    End Sub
#End Region

#Region "UpdateDataset"
    ' Executes the respective command for each inserted, updated, or deleted row in the DataSet.
    ' e.g.:  
    '   UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order")
    ' Parameters:
    ' -insertCommand: A valid transact-SQL statement or stored procedure to insert new records into the data source
    ' -deleteCommand: A valid transact-SQL statement or stored procedure to delete records from the data source
    ' -updateCommand: A valid transact-SQL statement or stored procedure used to update records in the data source
    ' -dataSet: the DataSet used to update the data source
    ' -tableName: the DataTable used to update the data source
    Public Overloads Shared Sub UpdateDataset(ByVal insertCommand As SqlCommand, ByVal deleteCommand As SqlCommand, ByVal updateCommand As SqlCommand, ByVal dataSet As DataSet, ByVal tableName As String)

        If (insertCommand Is Nothing) Then Throw New ArgumentNullException("insertCommand")
        If (deleteCommand Is Nothing) Then Throw New ArgumentNullException("deleteCommand")
        If (updateCommand Is Nothing) Then Throw New ArgumentNullException("updateCommand")
        If (dataSet Is Nothing) Then Throw New ArgumentNullException("dataSet")
        If (tableName Is Nothing OrElse tableName.Length = 0) Then Throw New ArgumentNullException("tableName")

        ' Create a SqlDataAdapter, and dispose of it after we are done
        Dim dataAdapter As New SqlDataAdapter
        Try
            ' Set the data adapter commands
            dataAdapter.UpdateCommand = updateCommand
            dataAdapter.InsertCommand = insertCommand
            dataAdapter.DeleteCommand = deleteCommand

            ' Update the dataset changes in the data source
            dataAdapter.Update(dataSet, tableName)

            ' Commit all the changes made to the DataSet
            dataSet.AcceptChanges()
        Finally
            If (Not dataAdapter Is Nothing) Then dataAdapter.Dispose()
        End Try
    End Sub
#End Region

#Region "CreateCommand"
    ' Simplify the creation of a Sql command object by allowing
    ' a stored procedure and optional parameters to be provided
    ' e.g.:  
    ' Dim command As SqlCommand = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName")
    ' Parameters:
    ' -connection: A valid SqlConnection object
    ' -spName: the name of the stored procedure
    ' -sourceColumns: An array of string to be assigned as the source columns of the stored procedure parameters
    ' Returns:
    ' a valid SqlCommand object
    Public Overloads Shared Function CreateCommand(ByVal connection As SqlConnection, ByVal spName As String, ByVal ParamArray sourceColumns() As String) As SqlCommand

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        ' Create a SqlCommand
        Dim cmd As New SqlCommand(spName, connection)
        cmd.CommandType = CommandType.StoredProcedure

        ' If we receive parameter values, we need to figure out where they go
        If Not sourceColumns Is Nothing AndAlso sourceColumns.Length > 0 Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Assign the provided source columns to these parameters based on parameter order
            Dim index As Integer
            For index = 0 To sourceColumns.Length - 1
                commandParameters(index).SourceColumn = sourceColumns(index)
            Next

            ' Attach the discovered parameters to the SqlCommand object
            AttachParameters(cmd, commandParameters)
        End If

        CreateCommand = cmd
    End Function
#End Region

#Region "ExecuteNonQueryTypedParams"
    ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
    ' the connection string using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -connectionString: A valid connection string for a SqlConnection
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values
    ' Returns:
    ' an int representing the number of rows affected by the command
    Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As Integer
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
    ' using the dataRow column values as the stored procedure' s parameters values.  
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -connection:a valid SqlConnection object
    ' -spName: the name of the stored procedure
    ' -dataRow:The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' an int representing the number of rows affected by the command
    Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As Integer
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified
    ' SqlTransaction using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -transaction:a valid SqlTransaction object
    ' -spName:the name of the stored procedure
    ' -dataRow:The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' an int representing the number of rows affected by the command
    Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As Integer

        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else

            ExecuteNonQueryTypedParams = SQLHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function
#End Region

#Region "ExecuteDatasetTypedParams"
    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    ' the connection string using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -connectionString: A valid connection string for a SqlConnection
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a dataset containing the resultset generated by the command
    Public Overloads Shared Function ExecuteDatasetTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As DataSet
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters)
        Else

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the dataRow column values as the store procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -connection: A valid SqlConnection object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a dataset containing the resultset generated by the command
    Public Overloads Shared Function ExecuteDatasetTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As DataSet

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
    ' Parameters:
    ' -transaction: A valid SqlTransaction object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a dataset containing the resultset generated by the command
    Public Overloads Shared Function ExecuteDatasetTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As DataSet
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else

            ExecuteDatasetTypedParams = SQLHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function
#End Region

#Region "ExecuteReaderTypedParams"
    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
    ' the connection string using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -connectionString: A valid connection string for a SqlConnection
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a SqlDataReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteReaderTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -connection: A valid SqlConnection object
    ' -spName: The name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a SqlDataReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteReaderTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -transaction: A valid SqlTransaction object
    ' -spName" The name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' a SqlDataReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteReaderTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteReaderTypedParams = SQLHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function
#End Region

#Region "ExecuteScalarTypedParams"
    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
    ' the connection string using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -connectionString: A valid connection string for a SqlConnection
    ' -spName: The name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns:
    ' An object containing the value in the 1x1 resultset generated by the command</returns>
    Public Overloads Shared Function ExecuteScalarTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As Object
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")
        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -connection: A valid SqlConnection object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns: 
    ' an object containing the value in the 1x1 resultset generated by the command</returns>
    Public Overloads Shared Function ExecuteScalarTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As Object
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -transaction: A valid SqlTransaction object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns: 
    ' an object containing the value in the 1x1 resultset generated by the command</returns>
    Public Overloads Shared Function ExecuteScalarTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As Object
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteScalarTypedParams = SQLHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function
#End Region

#Region "ExecuteXmlReaderTypedParams"
    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -connection: A valid SqlConnection object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns: 
    ' an XmlReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteXmlReaderTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As XmlReader
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")
        ' If the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteXmlReaderTypedParams = SQLHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteXmlReaderTypedParams = SQLHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName)
        End If
    End Function

    ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
    ' using the dataRow column values as the stored procedure' s parameters values.
    ' This method will query the database to discover the parameters for the 
    ' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
    ' Parameters:
    ' -transaction: A valid SqlTransaction object
    ' -spName: the name of the stored procedure
    ' -dataRow: The dataRow used to hold the stored procedure' s parameter values.
    ' Returns: 
    ' an XmlReader containing the resultset generated by the command
    Public Overloads Shared Function ExecuteXmlReaderTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As XmlReader
        If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")
        If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")
        ' if the row has values, the store procedure parameters must be initialized
        If (Not dataRow Is Nothing AndAlso dataRow.ItemArray.Length > 0) Then

            ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

            ' Set the parameters values
            AssignParameterValues(commandParameters, dataRow)

            ExecuteXmlReaderTypedParams = SQLHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
        Else
            ExecuteXmlReaderTypedParams = SQLHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName)
        End If
    End Function
#End Region

End Class

' SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
' ability to discover parameters for stored procedures at run-time.
Public NotInheritable Class SqlHelperParameterCache

#Region "private methods, variables, and constructors"


    ' Since this class provides only static methods, make the default constructor private to prevent 
    ' instances from being created with "new SqlHelperParameterCache()".
    Private Sub New()
    End Sub ' New 

    Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable)

    ' resolve at run time the appropriate set of SqlParameters for a stored procedure
    ' Parameters:
    ' - connectionString - a valid connection string for a SqlConnection
    ' - spName - the name of the stored procedure
    ' - includeReturnValueParameter - whether or not to include their return value parameter>
    ' Returns: SqlParameter()
    Private Shared Function DiscoverSpParameterSet(ByVal connection As SqlConnection, _
                                                       ByVal spName As String, _
                                                       ByVal includeReturnValueParameter As Boolean, _
                                                       ByVal ParamArray parameterValues() As Object) As SqlParameter()

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")
        Dim cmd As New SqlCommand(spName, connection)
        cmd.CommandType = CommandType.StoredProcedure
        Dim discoveredParameters() As SqlParameter
        connection.Open()
        SqlCommandBuilder.DeriveParameters(cmd)
        connection.Close()
        If Not includeReturnValueParameter Then
            cmd.Parameters.RemoveAt(0)
        End If

        discoveredParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
        cmd.Parameters.CopyTo(discoveredParameters, 0)

        ' Init the parameters with a DBNull value
        Dim discoveredParameter As SqlParameter
        For Each discoveredParameter In discoveredParameters
            discoveredParameter.Value = DBNull.Value
        Next

        Return discoveredParameters

    End Function ' DiscoverSpParameterSet

    ' Deep copy of cached SqlParameter array
    Private Shared Function CloneParameters(ByVal originalParameters() As SqlParameter) As SqlParameter()

        Dim i As Integer
        Dim j As Integer = originalParameters.Length - 1
        Dim clonedParameters(j) As SqlParameter

        For i = 0 To j
            clonedParameters(i) = CType(CType(originalParameters(i), ICloneable).Clone, SqlParameter)
        Next

        Return clonedParameters
    End Function ' CloneParameters

#End Region

#Region "caching functions"

    ' add parameter array to the cache
    ' Parameters
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandText - the stored procedure name or T-SQL command 
    ' -commandParameters - an array of SqlParamters to be cached 
    Public Shared Sub CacheParameterSet(ByVal connectionString As String, _
                                        ByVal commandText As String, _
                                        ByVal ParamArray commandParameters() As SqlParameter)
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (commandText Is Nothing OrElse commandText.Length = 0) Then Throw New ArgumentNullException("commandText")

        Dim hashKey As String = connectionString + ":" + commandText

        paramCache(hashKey) = commandParameters
    End Sub ' CacheParameterSet

    ' retrieve a parameter array from the cache
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -commandText - the stored procedure name or T-SQL command 
    ' Returns: An array of SqlParamters 
    Public Shared Function GetCachedParameterSet(ByVal connectionString As String, ByVal commandText As String) As SqlParameter()
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        If (commandText Is Nothing OrElse commandText.Length = 0) Then Throw New ArgumentNullException("commandText")

        Dim hashKey As String = connectionString + ":" + commandText
        Dim cachedParameters As SqlParameter() = CType(paramCache(hashKey), SqlParameter())

        If cachedParameters Is Nothing Then
            Return Nothing
        Else
            Return CloneParameters(cachedParameters)
        End If
    End Function ' GetCachedParameterSet

#End Region

#Region "Parameter Discovery Functions"
    ' Retrieves the set of SqlParameters appropriate for the stored procedure.
    ' This method will query the database for this information, and then store it in a cache for future requests.
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection 
    ' -spName - the name of the stored procedure 
    ' Returns: An array of SqlParameters
    Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, ByVal spName As String) As SqlParameter()
        Return GetSpParameterSet(connectionString, spName, False)
    End Function ' GetSpParameterSet 

    ' Retrieves the set of SqlParameters appropriate for the stored procedure.
    ' This method will query the database for this information, and then store it in a cache for future requests.
    ' Parameters:
    ' -connectionString - a valid connection string for a SqlConnection
    ' -spName - the name of the stored procedure 
    ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
    ' Returns: An array of SqlParameters 
    Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, _
                                                       ByVal spName As String, _
                                                       ByVal includeReturnValueParameter As Boolean) As SqlParameter()
        If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
        Dim connection As SqlConnection
        Try
            connection = New SqlConnection(connectionString)
            GetSpParameterSet = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter)
        Finally
            If Not connection Is Nothing Then connection.Dispose()
        End Try
    End Function ' GetSpParameterSet

    ' Retrieves the set of SqlParameters appropriate for the stored procedure.
    ' This method will query the database for this information, and then store it in a cache for future requests.
    ' Parameters:
    ' -connection - a valid SqlConnection object
    ' -spName - the name of the stored procedure 
    ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
    ' Returns: An array of SqlParameters 
    Public Overloads Shared Function GetSpParameterSet(ByVal connection As SqlConnection, _
                                                       ByVal spName As String) As SqlParameter()

        GetSpParameterSet = GetSpParameterSet(connection, spName, False)
    End Function ' GetSpParameterSet

    ' Retrieves the set of SqlParameters appropriate for the stored procedure.
    ' This method will query the database for this information, and then store it in a cache for future requests.
    ' Parameters:
    ' -connection - a valid SqlConnection object
    ' -spName - the name of the stored procedure 
    ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
    ' Returns: An array of SqlParameters 
    Public Overloads Shared Function GetSpParameterSet(ByVal connection As SqlConnection, _
                                                       ByVal spName As String, _
                                                       ByVal includeReturnValueParameter As Boolean) As SqlParameter()
        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
        Dim clonedConnection As SqlConnection
        Try
            clonedConnection = CType((CType(connection, ICloneable).Clone), SqlConnection)
            GetSpParameterSet = GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter)
        Finally
            If Not clonedConnection Is Nothing Then clonedConnection.Dispose()
        End Try
    End Function ' GetSpParameterSet

    ' Retrieves the set of SqlParameters appropriate for the stored procedure.
    ' This method will query the database for this information, and then store it in a cache for future requests.
    ' Parameters:
    ' -connection - a valid SqlConnection object
    ' -spName - the name of the stored procedure 
    ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
    ' Returns: An array of SqlParameters 
    Private Overloads Shared Function GetSpParameterSetInternal(ByVal connection As SqlConnection, _
                                                    ByVal spName As String, _
                                                    ByVal includeReturnValueParameter As Boolean) As SqlParameter()

        If (connection Is Nothing) Then Throw New ArgumentNullException("connection")

        Dim cachedParameters() As SqlParameter
        Dim hashKey As String

        If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

        hashKey = connection.ConnectionString + ":" + spName + IIf(includeReturnValueParameter = True, ":include ReturnValue Parameter", "").ToString

        cachedParameters = CType(paramCache(hashKey), SqlParameter())

        If (cachedParameters Is Nothing) Then
            Dim spParameters() As SqlParameter = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter)
            paramCache(hashKey) = spParameters
            cachedParameters = spParameters

        End If

        Return CloneParameters(cachedParameters)

    End Function ' GetSpParameterSet
#End Region

End Class ' SqlHelperParameterCache 
