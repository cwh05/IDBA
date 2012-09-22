Imports System.Xml
Imports System.IO

Public Class Service
    Implements IService

    Public Function GetWeather(ByVal city As String) As Dictionary(Of String, String) Implements IService.GetWeather
        Dim weatherMap = New Dictionary(Of String, String)

        Try
            'connect to web service
            Using service = New GlobalWeather.GlobalWeatherSoapClient("GlobalWeatherSoap")

                'create request that contains 2 parameters
                Dim request As GlobalWeather.GetWeatherRequest = New GlobalWeather.GetWeatherRequest()
                request.CityName = city
                request.CountryName = String.Empty

                'retrieve weather info
                Dim response = service.GetWeather(request)

                Try
                    Using reader As XmlTextReader = New XmlTextReader(New StringReader(response.GetWeatherResult))
                        reader.XmlResolver = Nothing

                        'read the entire XML string
                        While (reader.Read())
                            'check for start element
                            If reader.IsStartElement() Then

                                'record few element data
                                Select Case reader.Name
                                    Case "Temperature"
                                        weatherMap.Add("Temperature", reader.ReadElementContentAsString)

                                    Case "RelativeHumidity"
                                        weatherMap.Add("RelativeHumidity", reader.ReadElementContentAsString)

                                    Case "Wind"
                                        weatherMap.Add("Wind", reader.ReadElementContentAsString)

                                    Case "SkyConditions"
                                        weatherMap.Add("SkyConditions", reader.ReadElementContentAsString)
                                End Select
                            End If
                        End While

                        Return weatherMap
                    End Using

                Catch ex As Exception
                    'clear data in dictionary
                    weatherMap.Clear()
                    weatherMap = Nothing
                End Try
            End Using

        Catch ex As Exception
            'clear data in dictionary
            weatherMap.Clear()
            weatherMap = Nothing
        End Try

        Return Nothing
    End Function

End Class
