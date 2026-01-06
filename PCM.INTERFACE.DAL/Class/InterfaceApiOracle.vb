Imports System.Data
Imports Microsoft.Data.SqlClient
Imports Microsoft.Extensions.Logging
Imports Oracle.ManagedDataAccess.Client

Public Class InterfaceApiOracle

    Private ReadOnly _oracleConnectionString As String
    Private ReadOnly _sqlHelper As ISqlHelper
    Private ReadOnly _logger As ILogger(Of InterfaceApiOracle)

    Public Sub New(
        oracleConnectionString As String,
        sqlHelper As ISqlHelper,
        logger As ILogger(Of InterfaceApiOracle)
    )
        _oracleConnectionString = oracleConnectionString
        _sqlHelper = sqlHelper
        _logger = logger
    End Sub

#Region "::: STATUS UH :::"

    ' ==========================================================
    ' CARREGA LISTA DE HOTÉIS (SQL SERVER)
    ' ==========================================================
    Public Async Function LoadHotelIdAsync(codigoEmpresa As Integer) As Task(Of List(Of String))

        Dim result As New List(Of String)
        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@codigo_empresa", codigoEmpresa)
        }

        Try

            Dim dataRow = Await _sqlHelper.ExecuteDataTableAsync("sp_select_interface_uh_hotelId", CommandType.StoredProcedure, parameters)

            For Each row As DataRow In dataRow.Rows
                result.Add(row("hotelId").ToString())
            Next

        Catch ex As Exception
            _logger.LogError(ex, "Erro ao carregar lista de hotéis")
            Throw
        End Try

        Return result

    End Function

    ' ==========================================================
    ' OBTÉM STATUS UH (ORACLE → SQL SERVER)
    ' ==========================================================
    Public Async Function GetStatusUH(codigoEmpresa As Integer, hotelId As String) As Task

        Try
            ' ============================================
            ' 1) Abre UMA ÚNICA conexão SQL Server
            ' ============================================
            Using sqlConn As New SqlConnection(_sqlHelper.ConnectionString)
                Await sqlConn.OpenAsync()

                ' --------------------------------------------
                ' TRUNCATE tabela de staging
                ' --------------------------------------------
                Using cmdTruncate As New SqlCommand("sp_truncate_table_tb_interface_uh_status_stg", sqlConn)
                    cmdTruncate.CommandType = CommandType.StoredProcedure
                    Await cmdTruncate.ExecuteNonQueryAsync()
                End Using

                ' ============================================
                ' 2) Abre conexão ORACLE
                ' ============================================
                Using oracleConn As New OracleConnection(_oracleConnectionString)
                    Await oracleConn.OpenAsync()

                    Dim query As String =
                "SELECT DISTINCT
                    :CodigoEmpresa AS codigo_empresa,
                    U.IDHOTEL AS hotel_id,
                    LPAD(LTRIM(RTRIM(U.CODUH)), 8, ' ') AS uh,
                    SU.DESCSTATUSUH  AS status_uh,
                    SG.DESCSTATUSGOV AS status_gov
                 FROM UH U
                 JOIN STATUSGOVFULL SG ON SG.IDSTATUSGOV = U.IDSTATUSGOV
                 JOIN STATUSUHFULL SU ON SU.IDSTATUSUH = U.IDSTATUSUH
                 WHERE U.IDHOTEL = :HotelId
                   AND U.UHPOOL = 'S'
                   AND U.FLGATIVA = 'S'
                 ORDER BY 2,3,5"

                    Using oracleCmd As New OracleCommand(query, oracleConn)
                        oracleCmd.BindByName = True
                        oracleCmd.Parameters.Add(New OracleParameter("CodigoEmpresa", OracleDbType.Int32)).Value = codigoEmpresa
                        oracleCmd.Parameters.Add(New OracleParameter("HotelId", OracleDbType.Varchar2)).Value = hotelId

                        Using reader As OracleDataReader = Await oracleCmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess)

                            ' --------------------------------------------
                            ' BULK COPY (reutilizando a MESMA conexão SQL)
                            ' --------------------------------------------
                            Using bulk As New SqlBulkCopy(sqlConn, SqlBulkCopyOptions.TableLock, Nothing)
                                bulk.DestinationTableName = "dbo.tb_interface_uh_status_stg"
                                bulk.BatchSize = 5000
                                bulk.BulkCopyTimeout = 5000

                                bulk.ColumnMappings.Clear()
                                bulk.ColumnMappings.Add("codigo_empresa", "codigo_empresa")
                                bulk.ColumnMappings.Add("hotel_id", "hotel_id")
                                bulk.ColumnMappings.Add("uh", "uh")
                                bulk.ColumnMappings.Add("status_uh", "status_uh")
                                bulk.ColumnMappings.Add("status_gov", "status_gov")

                                Await bulk.WriteToServerAsync(reader)
                            End Using

                        End Using
                    End Using
                End Using

                ' --------------------------------------------
                ' 3) Atualização final (MESMA conexão SQL)
                ' --------------------------------------------
                Using cmdUpdate As New SqlCommand("sp_update_interface_uh_status_stg", sqlConn)
                    cmdUpdate.CommandType = CommandType.StoredProcedure
                    cmdUpdate.Parameters.Add("@codigo_empresa", SqlDbType.Int).Value = codigoEmpresa
                    cmdUpdate.Parameters.Add("@hotel_id", SqlDbType.VarChar, 20).Value = hotelId

                    Await cmdUpdate.ExecuteNonQueryAsync()
                End Using

            End Using

        Catch ex As Exception
            _logger.LogError(ex, "Erro no GetStatusUH (Bulk). HotelId={hotelId}", hotelId)
            Throw ' opcional: mantém stack trace
        End Try

    End Function

    ' ==========================================================
    ' OBTÉM RESERVAS UH (ORACLE → SQL SERVER)
    ' ==========================================================
    Public Async Function GetReservasUH(codigoEmpresa As Integer, hotelId As String) As Task

        Try
            ' ============================================
            ' 1) Abre UMA ÚNICA conexão SQL Server
            ' ============================================
            Using sqlConn As New SqlConnection(_sqlHelper.ConnectionString)
                Await sqlConn.OpenAsync()

                ' --------------------------------------------
                ' TRUNCATE tabela de staging
                ' --------------------------------------------
                Using cmdTruncate As New SqlCommand("sp_truncate_table_tb_interface_uh_reservas_stg", sqlConn)
                    cmdTruncate.CommandType = CommandType.StoredProcedure
                    Await cmdTruncate.ExecuteNonQueryAsync()
                End Using

                ' ============================================
                ' 2) Abre conexão ORACLE
                ' ============================================
                Using oracleConn As New OracleConnection(_oracleConnectionString)
                    Await oracleConn.OpenAsync()

                    Dim query As String =
                "SELECT 
                    :CodigoEmpresa AS codigo_empresa,
                    :HotelId AS hotel_id,
                    CODUH AS uh,
                    MAX(NVL(DATACHEGADAREAL, DATACHEGPREVISTA)) AS data_chegada,
                    MAX(NVL(DATAPARTIDAREAL, DATAPARTPREVISTA)) AS data_saida
                 FROM RESERVASFRONT
                 WHERE IDHOTEL = :HotelId
                   AND CODUH IS NOT NULL
                   AND SYSDATE BETWEEN NVL(DATACHEGADAREAL, DATACHEGPREVISTA)
                                   AND NVL(DATAPARTIDAREAL, DATAPARTPREVISTA)
                 GROUP BY CODUH"

                    Using oracleCmd As New OracleCommand(query, oracleConn)
                        oracleCmd.BindByName = True
                        oracleCmd.Parameters.Add(New OracleParameter("CodigoEmpresa", OracleDbType.Int32)).Value = codigoEmpresa
                        oracleCmd.Parameters.Add(New OracleParameter("HotelId", OracleDbType.Varchar2)).Value = hotelId

                        Using reader As OracleDataReader = Await oracleCmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess)

                            ' --------------------------------------------
                            ' BULK COPY (reutilizando a MESMA conexão SQL)
                            ' --------------------------------------------
                            Using bulk As New SqlBulkCopy(sqlConn, SqlBulkCopyOptions.TableLock, Nothing)
                                bulk.DestinationTableName = "dbo.tb_interface_uh_reservas_stg"
                                bulk.BatchSize = 5000
                                bulk.BulkCopyTimeout = 5000
                                bulk.EnableStreaming = True

                                bulk.ColumnMappings.Clear()
                                bulk.ColumnMappings.Add("codigo_empresa", "codigo_empresa")
                                bulk.ColumnMappings.Add("hotel_id", "hotel_id")
                                bulk.ColumnMappings.Add("uh", "uh")
                                bulk.ColumnMappings.Add("data_chegada", "data_chegada")
                                bulk.ColumnMappings.Add("data_saida", "data_saida")

                                Await bulk.WriteToServerAsync(reader)
                            End Using

                        End Using
                    End Using
                End Using

                ' --------------------------------------------
                ' 3) Atualização final (MESMA conexão SQL)
                ' --------------------------------------------
                Using cmdUpdate As New SqlCommand("sp_update_interface_uh_reservas_stg", sqlConn)
                    cmdUpdate.CommandType = CommandType.StoredProcedure
                    cmdUpdate.Parameters.Add("@codigo_empresa", SqlDbType.Int).Value = codigoEmpresa
                    cmdUpdate.Parameters.Add("@hotel_id", SqlDbType.VarChar, 20).Value = hotelId

                    Await cmdUpdate.ExecuteNonQueryAsync()
                End Using

            End Using

        Catch ex As Exception
            _logger.LogError(ex, "Erro no GetReservasUH (Bulk). HotelId={hotelId}", hotelId)
            Throw ' mantém stack trace (recomendado em Worker)
        End Try

    End Function

    ' ==========================================================
    ' UPDATE STATUS UH (ORACLE)
    ' ==========================================================
    Public Sub UpdateStatusUH(hotelId As String, uh As String, status As String)

        Using connection As New OracleConnection(_oracleConnectionString)
            connection.Open()

            Using command As New OracleCommand(
                "UPDATE UH
                 SET IDSTATUSGOV = :NovoStatus
                 WHERE IDHOTEL = :HotelId
                   AND LTRIM(RTRIM(CODUH)) = :CodUH",
                connection)

                command.Parameters.Add(New OracleParameter(":NovoStatus", status))
                command.Parameters.Add(New OracleParameter(":HotelId", hotelId))
                command.Parameters.Add(New OracleParameter(":CodUH", uh))

                command.ExecuteNonQuery()
            End Using
        End Using

    End Sub

#End Region

End Class
