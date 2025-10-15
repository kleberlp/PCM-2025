Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

Module modAux

    Public Sub AddSqlParameter(ByRef paramList As List(Of SqlParameter), paramName As String, dbType As SqlDbType, size As Integer, value As Object)

        Dim param As New SqlParameter With {
            .ParameterName = paramName,
            .SqlDbType = dbType,
            .Size = size,
            .Value = value
        }

        paramList.Add(param)

    End Sub

    Public Sub AddSqlParameterOutput(ByRef paramList As List(Of SqlParameter), paramName As String, dbType As SqlDbType, size As Integer)

        Dim param As New SqlParameter With {
            .ParameterName = paramName,
            .SqlDbType = dbType,
            .Size = size,
            .Direction = ParameterDirection.Output
        }

        paramList.Add(param)

    End Sub

    Public Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return reader.Item(reader.GetOrdinal(columnName)).ToString()
        End If

    End Function

    Public Function SafeIntSimNao(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return IIf(reader.Item(reader.GetOrdinal(columnName)).ToString() = "0", "NÃO", "SIM")
        End If

    End Function

    Public Function SafeGetDate(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return CType(reader.Item(reader.GetOrdinal(columnName)), DateTime).ToString("dd/MM/yyyy")
        End If

    End Function

    Public Function SafeGetTime(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return CType(reader.Item(reader.GetOrdinal(columnName)), DateTime).ToString("HH:mm")
        End If

    End Function

    Public Function SafeGetFloat(ByVal reader As SqlDataReader, ByVal columnName As String) As Double

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return 0
        Else
            Return reader.Item(reader.GetOrdinal(columnName))
        End If

    End Function

    Public Function SafeGetLong(ByVal reader As SqlDataReader, ByVal columnName As String) As Long

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return -1
        Else
            Return reader.Item(reader.GetOrdinal(columnName))
        End If

    End Function

    Public Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String) As Boolean

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return False
        Else
            Return reader.GetSqlBoolean(reader.GetOrdinal(columnName))
        End If

    End Function

    Public Sub PopulateObject(Of T)(ByVal reader As SqlDataReader, ByVal obj As T)
        For Each prop In GetType(T).GetProperties()
            If reader.HasColumn(prop.Name) AndAlso Not IsDBNull(reader(prop.Name)) Then
                Debug.Print(prop.Name)
                prop.SetValue(obj, Convert.ChangeType(reader(prop.Name), prop.PropertyType))
            End If
        Next
    End Sub

    <Extension()>
    Public Function HasColumn(ByVal reader As SqlDataReader, ByVal columnName As String) As Boolean
        For i As Integer = 0 To reader.FieldCount - 1
            If reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function NumberToColumnExcel(ByVal number As Integer) As String

        Dim column As String = String.Empty

        While number > 0
            Dim resto As Integer = (number - 1) Mod 26
            column = Chr(65 + resto) & column
            number = (number - 1) \ 26
        End While

        Return column

    End Function


End Module
