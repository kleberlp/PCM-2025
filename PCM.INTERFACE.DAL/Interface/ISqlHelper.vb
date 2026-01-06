Imports System.Data
Imports Microsoft.Data.SqlClient

Public Interface ISqlHelper

    ReadOnly Property ConnectionString As String

    ' ===============================
    ' BASIC COMMANDS
    ' ===============================
    Function ExecuteNonQueryAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of Integer)

    Function ExecuteScalarAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of Object)

    Function ExecuteDataTableAsync(
        query As String,
        commandType As CommandType,
        Optional parameters As List(Of SqlParameter) = Nothing,
        Optional transaction As SqlTransaction = Nothing
    ) As Task(Of DataTable)

    ' ===============================
    ' GENERIC METHODS
    ' ===============================
    Function ExecuteListAsync(Of T)(
        query As String,
        commandType As CommandType,
        map As Func(Of IDataReader, T),
        Optional parameters As List(Of SqlParameter) = Nothing
    ) As Task(Of List(Of T))

    ' ===============================
    ' TRANSACTIONS
    ' ===============================
    Function BeginTransactionAsync() As Task(Of SqlTransaction)

End Interface
