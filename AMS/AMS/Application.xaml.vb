Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Private weatherInfo As String

    Public Property WeatherTemperature() As String
        Get
            Return weatherInfo
        End Get
        Set(value As String)
            weatherInfo = value
        End Set
    End Property


End Class
