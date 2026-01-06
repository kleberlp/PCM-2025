Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Text
Imports Oracle.ManagedDataAccess.Client
Imports PCM.WEB.DAL.SQLHelper

Module modFuncion

    Public Function Cripitografar(ByVal sTexto As String) As String

        Try

            'Variaveis Locais
            Dim sLetra As String
            Dim sTextoCriptografada As String = ""
            Dim i As Integer

            sTexto = Trim(sTexto)

            For i = 1 To Len(sTexto)
                sLetra = Asc(Mid(sTexto, i, 1))
                sLetra = Chr(Trim(Str(Val(sLetra) Xor (i * 2))))
                sTextoCriptografada = sTextoCriptografada & sLetra
            Next i

            Return sTextoCriptografada

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Criptografar16(ByVal vstrTextToBeEncrypted As String, ByVal vstrEncryptionKey As String) As String

        Dim bytValue() As Byte
        Dim bytKey() As Byte
        Dim bytEncoded() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim objMemoryStream As New MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim objRijndaelManaged As RijndaelManaged


        ' ***************************************************************
        ' ****** Descarta todos os caracteres nulos da palavra a ser cifrada              
        ' ***************************************************************

        vstrTextToBeEncrypted = RetirarCaracteresNulos(vstrTextToBeEncrypted)

        ' ***************************************************************
        ' ****** O valor deve estar dentro da tabela ASCII (i.e., no DBCS chars)    
        ' ***************************************************************

        bytValue = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted.ToCharArray)

        intLength = Len(vstrEncryptionKey)

        ' ****************************************************************
        ' ****** A chave cifrada será de 256 bits long (32 bytes)                             
        ' ****** Se for maior que 32 bytes então será truncado.                               
        ' ****** Se for menor que 32 bytes será alocado.                                        
        ' ****** Usando upper-case Xs.                                                                  
        ' ****************************************************************

        If intLength >= 32 Then
            vstrEncryptionKey = Strings.Left(vstrEncryptionKey, 32)
        Else
            intLength = Len(vstrEncryptionKey)
            intRemaining = 32 - intLength
            vstrEncryptionKey = vstrEncryptionKey & Strings.StrDup(intRemaining, "X")
        End If

        bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray)

        objRijndaelManaged = New RijndaelManaged

        ' **************************************************************
        ' ****** Cria o valor a ser crifrado e depois escreve                                  
        ' ****** Convertido em uma disposição do byte                                       
        ' **************************************************************

        Try

            objCryptoStream = New CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write)
            objCryptoStream.Write(bytValue, 0, bytValue.Length)

            objCryptoStream.FlushFinalBlock()

            bytEncoded = objMemoryStream.ToArray
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch

        End Try

        ' **************************************************************
        ' ****** Retorna o valor cifrado (convertido de byte para base64)           
        ' **************************************************************

        Return Convert.ToBase64String(bytEncoded)

    End Function

    Public Function Descriptografar16(ByVal vstrStringToBeDecrypted As String, ByVal vstrDecryptionKey As String) As String

        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte

        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim strReturnString As String = String.Empty

        ' ***************************************************************
        ' ****** Convert base64 cifrada para byte array                                
        ' ****** Convert base64 cifrada para byte array                                
        ' ***************************************************************

        bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted)

        ' ***************************************************************
        ' ****** A chave cifrada sera de 256 bits long (32 bytes)                           
        ' ****** Se for maior que 32 bytes então será truncado.                              
        ' ****** Se for menor que 32 bytes será alocado.                                       
        ' ****** Usando upper-case Xs.                                                              
        ' ***************************************************************

        intLength = Len(vstrDecryptionKey)

        If intLength >= 32 Then
            vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
        Else
            intLength = Len(vstrDecryptionKey)
            intRemaining = 32 - intLength
            vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
        End If

        bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)

        ReDim bytTemp(bytDataToBeDecrypted.Length)

        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)

        ' ***************************************************************
        ' ****** Escrever o valor decifrado depois que é convertido                      
        ' ***************************************************************

        Try

            objCryptoStream = New CryptoStream(objMemoryStream,
            objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV),
            CryptoStreamMode.Read)

            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

            'objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()

        Catch

        End Try

        ' ***************************************************************
        ' ****** Retorna o valor decifrado                                    
        ' ***************************************************************
        Return RetirarCaracteresNulos(Encoding.ASCII.GetString(bytTemp))

    End Function

    Public Function RetirarCaracteresNulos(ByVal vstrStringWithNulls As String) As String

        Dim intPosition As Integer
        Dim strStringWithOutNulls As String

        intPosition = 1
        strStringWithOutNulls = vstrStringWithNulls

        Do While intPosition > 0
            intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

            If intPosition > 0 Then
                strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) &
                Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
            End If

            If intPosition > strStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop

        Return strStringWithOutNulls

    End Function

    Public Function ExecuteQueryReturn(ByVal sQuery As String,
                                       ByVal sConnectionString As String) As Object

        'Variaveis Locais
        Dim sResult As String

        Try

            'Obtem o DataSet
            sResult = ExecuteScalar(sConnectionString, CommandType.Text, "EXECUTE " & sQuery)

            Return sResult

        Catch SqlEx As SqlException
            Throw SqlEx
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ProcessarCampoHoras(oSqlDataReader As SqlDataReader, campo As String) As String
        Dim minutos As Integer = If(IsDBNull(oSqlDataReader.Item(campo)), 0, Convert.ToInt32(oSqlDataReader.Item(campo)))
        Dim tempo As TimeSpan = TimeSpan.FromMinutes(minutos)
        Return $"{Math.Floor(tempo.TotalHours)}:{tempo.Minutes:D2}"
    End Function

    Public Function GetCampoNumerico(oSqlDataReader As SqlDataReader, campo As String) As Integer
        Return If(IsDBNull(oSqlDataReader.Item(campo)), 0, Convert.ToInt32(oSqlDataReader.Item(campo)))
    End Function

    Public Function GetCampoDouble(oSqlDataReader As SqlDataReader, campo As String) As Double
        Return If(IsDBNull(oSqlDataReader.Item(campo)), 0, Convert.ToDouble(oSqlDataReader.Item(campo)))
    End Function

    Public Function ConverterParaMinutos(horasFormatadas As String) As Integer
        If String.IsNullOrEmpty(horasFormatadas) Then Return 0

        Dim partes = horasFormatadas.Split(":")
        Dim horas = Convert.ToInt32(partes(0))
        Dim minutos = Convert.ToInt32(partes(1))

        Return (horas * 60) + minutos
    End Function

    Public Function ConverterParaHorasFormatadas(minutos As Integer) As String
        Dim tempo As TimeSpan = TimeSpan.FromMinutes(minutos)
        Return $"{Math.Floor(tempo.TotalHours)}:{tempo.Minutes:D2}"
    End Function

    Public Function CriarParametro(nome As String, tipo As SqlDbType, valor As Object, Optional ByVal direction As ParameterDirection = ParameterDirection.Input) As SqlParameter

        Dim parametro As New SqlParameter With {
            .ParameterName = nome,
            .Direction = direction,
            .SqlDbType = tipo,
            .Value = If(valor Is Nothing, DBNull.Value, valor)
        }
        Return parametro

    End Function

    Public Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return reader.Item(reader.GetOrdinal(columnName)).ToString()
        End If

    End Function

    Public Function SafeGetString(ByVal reader As OracleDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return String.Empty
        Else
            Return reader.Item(reader.GetOrdinal(columnName)).ToString()
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
            Return 0
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

    Public Function SafeGetBooleanSimNao(ByVal reader As SqlDataReader, ByVal columnName As String) As String

        If reader.IsDBNull(reader.GetOrdinal(columnName)) Then
            Return ""
        Else
            Return IIf(reader.GetSqlBoolean(reader.GetOrdinal(columnName)) = True, "SIM", "NÃO")
        End If

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
