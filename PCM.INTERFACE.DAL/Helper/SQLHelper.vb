Imports System.Data
Imports System.Security
Imports Microsoft.Data.SqlClient
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Logging
Imports Polly

Public Class SqlHelper
    Implements ISqlHelper

    Private ReadOnly _connectionString As String
    Private ReadOnly _logger As ILogger(Of SqlHelper)
    Private ReadOnly _retryPolicy As IAsyncPolicy

    Public ReadOnly Property ConnectionString As String Implements ISqlHelper.ConnectionString
        Get
            Return _connectionString
        End Get
    End Property

    Public Sub New(
        config As IConfiguration,
        logger As ILogger(Of SqlHelper),
        connectionName As String
    )
        _connectionString = config.GetConnectionString(connectionName)
        _logger = logger

        _retryPolicy = Policy _
            .Handle(Of SqlException)() _
            .WaitAndRetryAsync(
                retryCount:=3,
                sleepDurationProvider:=Function(r) TimeSpan.FromSeconds(Math.Pow(2, r)),
                onRetry:=Sub(ex, ts, rc, ctx)
                             _logger.LogWarning(ex, $"Retry {rc} after {ts.TotalSeconds}s")
                         End Sub
            )
    End Sub

    ' ===============================
    ' TRANSACTION
    ' ===============================
    Public Async Function BeginTransactionAsync() As Task(Of SqlTransaction) _
        Implements ISqlHelper.BeginTransactionAsync

        Dim conn As New SqlConnection(_connectionString)
        Await conn.OpenAsync()
        Return conn.BeginTransaction()
    End Function

    ' ===============================
    ' EXECUTE NON QUERY
    ' ===============================
    Public Async Function ExecuteNonQueryAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of Integer) Implements ISqlHelper.ExecuteNonQueryAsync

        Return Await _retryPolicy.ExecuteAsync(Async Function()
                                                   Using cmd = CreateCommand(query, commandType, parameters, transaction)
                                                       Return Await cmd.ExecuteNonQueryAsync()
                                                   End Using
                                               End Function)
    End Function

    ' ===============================
    ' EXECUTE SCALAR
    ' ===============================
    Public Async Function ExecuteScalarAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of Object) Implements ISqlHelper.ExecuteScalarAsync

        Return Await _retryPolicy.ExecuteAsync(Async Function()
                                                   Using cmd = CreateCommand(query, commandType, parameters, transaction)
                                                       Return Await cmd.ExecuteScalarAsync()
                                                   End Using
                                               End Function)
    End Function

    ' ===============================
    ' EXECUTE DATATABLE
    ' ===============================
    Public Async Function ExecuteDataTableAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of DataTable) Implements ISqlHelper.ExecuteDataTableAsync

        Return Await _retryPolicy.ExecuteAsync(Async Function()
                                                   Dim dt As New DataTable()
                                                   Using cmd = CreateCommand(query, commandType, parameters, transaction)
                                                       Using reader = Await cmd.ExecuteReaderAsync()
                                                           dt.Load(reader)
                                                       End Using
                                                   End Using
                                                   Return dt
                                               End Function)
    End Function

    ' ===============================
    ' EXECUTE LIST (GENERIC)
    ' ===============================
    Public Async Function ExecuteListAsync(Of T)(
        query As String,
        commandType As CommandType,
        map As Func(Of IDataReader, T),
        Optional parameters As List(Of SqlParameter) = Nothing
    ) As Task(Of List(Of T)) Implements ISqlHelper.ExecuteListAsync

        Return Await _retryPolicy.ExecuteAsync(Async Function()
                                                   Dim list As New List(Of T)

                                                   Using conn As New SqlConnection(_connectionString)
                                                       Await conn.OpenAsync()

                                                       Using cmd As New SqlCommand(query, conn)
                                                           cmd.CommandType = commandType
                                                           cmd.CommandTimeout = 60

                                                           If parameters IsNot Nothing Then
                                                               cmd.Parameters.AddRange(parameters.ToArray())
                                                           End If

                                                           Using reader = Await cmd.ExecuteReaderAsync()
                                                               While Await reader.ReadAsync()
                                                                   list.Add(map(reader))
                                                               End While
                                                           End Using
                                                       End Using
                                                   End Using

                                                   Return list
                                               End Function)
    End Function

    ' ===============================
    ' PRIVATE HELPER
    ' ===============================
    Private Function CreateCommand(
        query As String,
        commandType As CommandType,
        parameters As List(Of SqlParameter),
        transaction As SqlTransaction
    ) As SqlCommand

        Dim conn As SqlConnection =
            If(transaction IsNot Nothing, transaction.Connection, New SqlConnection(_connectionString))

        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If

        Dim cmd As New SqlCommand(query, conn) With {
            .CommandType = commandType,
            .CommandTimeout = 60
        }

        If transaction IsNot Nothing Then
            cmd.Transaction = transaction
        End If

        If parameters IsNot Nothing Then
            cmd.Parameters.AddRange(parameters.ToArray())
        End If

        Return cmd
    End Function

End Class
