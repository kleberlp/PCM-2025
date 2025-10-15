Imports System.Data.SqlClient
Imports System.Xml
Imports Newtonsoft.Json
Imports Oracle.ManagedDataAccess.Client
Imports PCM.WEB.DAL.SQLHelper
Imports PCM.WEB.MODELS.Models

Public Class InterfaceApiOracle

    Private sConnection As String

    Sub New(ByVal sCon As String)
        sConnection = sCon
    End Sub

#Region "::: STATUS UH :::"

    Public Function StatusUH(sHotelId As String) As InterfaceStatusUH

        Dim oResult As New InterfaceStatusUH

        oResult.rows = New List(Of InterfaceStatusUHDetails)

        Using connection As New OracleConnection(sConnection)
            Dim query As String = "SELECT DISTINCT
                U.IDHOTEL,         
                LPAD (LTRIM (RTRIM (u.CODUH)), 8, ' ') AS CODUH,
                SG.DESCSTATUSGOV,
                SU.DESCSTATUSUH
                FROM TIPOUH T,
                     STATUSGOVFULL SG,
                     STATUSUHFULL SU,
                     UH U,
                     RESERVASFRONT R,
                     MOVIMENTOHOSPEDES M,
                     HOSPEDE H,
                     TIPOHOSPEDE TH,
                     PARAMHOTEL PAR,
                     BLOQUEIOUH BLOQ,
                     PESSOA P,
                     ROOMLISTVHF RL,
                     RESERVAGRUPO G,
                     orcamentoreserva o
               WHERE U.IDHOTEL = :HotelId
                     AND (U.UHPOOL = 'S')
                     AND (RL.IDROOMLIST(+) = R.IDROOMLIST)
                     AND (G.IDRESERVAGRUPO(+) = RL.IDRESERVAGRUPO)
                     AND (T.IDHOTEL = U.IDHOTEL)
                     AND (T.IDTIPOUH = U.IDTIPOUH)
                     AND (SG.IDSTATUSGOV = U.IDSTATUSGOV)
                     AND (SU.IDSTATUSUH = U.IDSTATUSUH)
                     AND (PAR.IDHOTEL = U.IDHOTEL)
                     AND (U.FLGATIVA = 'S')
                     AND (BLOQ.IDHOTEL(+) = U.IDHOTEL)
                     AND (R.STATUSRESERVA(+) <= 2)
                     AND (R.IDHOTEL(+) = U.IDHOTEL)
                     AND (R.CODUH(+) = U.CODUH)
                     AND (M.IDHOTEL(+) = R.IDHOTEL)
                     AND (M.IDRESERVASFRONT(+) = R.IDRESERVASFRONT)
                     AND (H.IDHOSPEDE(+) = M.IDHOSPEDE)
                     AND (TH.IDHOTEL(+) = M.IDHOTEL)
                     AND (TH.IDTIPOHOSPEDE(+) = M.IDTIPOHOSPEDE)
                     AND (P.IDPESSOA(+) = R.CLIENTERESERVANTE)
                     AND (M.DATAPARTREAL IS NULL)
                     AND (r.idreservasfront = o.idreservasfront(+))
                     AND (O.DATA(+) =
                             TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))
            ORDER BY 1,
                     2,
                     3"



            'AND (BLOQ.DATAINICIO(+) <=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))
            'AND (BLOQ.DATAFIM(+) >=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))
            'AND (BLOQ.CODUH(+) = U.CODUH)
            'AND (R.DATACHEGPREVISTA(+) <=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))

            connection.Open()

            Using command As New OracleCommand(query, connection)
                command.Parameters.Add(New OracleParameter(":HotelId", sHotelId))

                Using reader As OracleDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim row As New InterfaceStatusUHDetails

                        row.IDHOTEL = reader("IDHOTEL")
                        row.UH = reader("CODUH")
                        row.STATUSDAUH = reader("DESCSTATUSUH")
                        row.STATUSDAGOV = reader("DESCSTATUSGOV")

                        oResult.rows.Add(row)

                    End While
                End Using
            End Using
        End Using

        Return oResult

    End Function

    Public Function ReservaUH(sHotelId As String) As List(Of ReservasUH)

        Dim oResult As New List(Of ReservasUH)

        Using connection As New OracleConnection(sConnection)
            Dim query As String = "SELECT 
                                CODUH AS apartamento, 
                                    MAX(NVL(DATACHEGADAREAL, DATACHEGPREVISTA)) AS dataChegada, 
                                    MAX(NVL(DATAPARTIDAREAL, DATAPARTPREVISTA)) AS dataPartida
                                FROM RESERVASFRONT
                                WHERE IDHOTEL = :HotelId
                                  AND CODUH IS NOT NULL
                                  AND SYSDATE BETWEEN NVL(DATACHEGADAREAL, DATACHEGPREVISTA)
                                                  AND NVL(DATAPARTIDAREAL, DATAPARTPREVISTA)
                                GROUP BY
                                CODUH"

            connection.Open()

            Using command As New OracleCommand(query, connection)
                command.Parameters.Add(New OracleParameter(":HotelId", sHotelId))

                Using reader As OracleDataReader = command.ExecuteReader()
                    While reader.Read()

                        Dim row As New ReservasUH

                        row.UH = reader("apartamento")
                        row.dataChegada = reader("dataChegada")
                        row.dataPartida = reader("dataPartida")

                        oResult.Add(row)

                    End While
                End Using

            End Using

        End Using

        Return oResult

    End Function

    Public Function ListStatusUH() As statusUH

        Dim oResult As New statusUH

        oResult.rows = New List(Of statusUHInfo)

        Using connection As New OracleConnection(sConnection)
            Dim query As String = "SELECT DISTINCT
                SG.DESCSTATUSGOV,
                SG.IDSTATUSGOV
                FROM 
                    STATUSGOVFULL SG"



            'AND (BLOQ.DATAINICIO(+) <=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))
            'AND (BLOQ.DATAFIM(+) >=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))
            'AND (BLOQ.CODUH(+) = U.CODUH)
            'AND (R.DATACHEGPREVISTA(+) <=
            '        TO_DATE ('08/08/2024 00:00:00', 'DD/MM/YYYY HH24:MI:SS'))

            connection.Open()

            Using command As New OracleCommand(query, connection)

                Using reader As OracleDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim row As New statusUHInfo

                        row.status = reader("IDSTATUSGOV")
                        row.description = reader("DESCSTATUSGOV")

                        oResult.rows.Add(row)

                    End While
                End Using
            End Using
        End Using

        Return oResult

    End Function

    Public Sub UpdateStatusUH(sHotelId As String,
                              sUH As String,
                              sStatus As String)

        Using connection As New OracleConnection(sConnection)
            Dim query As String = "UPDATE UH
      SET IDSTATUSGOV = :NovoIDSTATUSGOV
    WHERE IDHOTEL = :HotelId
      AND (LTRIM(RTRIM(CODUH)) = :CodUH)"

            connection.Open()

            Using command As New OracleCommand(query, connection)
                command.Parameters.Add(New OracleParameter(":NovoIDSTATUSGOV", sStatus))
                command.Parameters.Add(New OracleParameter(":HotelId", sHotelId))
                command.Parameters.Add(New OracleParameter(":CodUH", sUH))

                command.ExecuteNonQuery()
            End Using

        End Using

    End Sub

#End Region

End Class
