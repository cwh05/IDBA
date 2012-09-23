Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Private weatherTemp As String
    Private weatherRH As String
    Private weatherW As String
    'Private weatherSC As String

    'weather temperature property
    Public Property WeatherTemperature() As String
        Get
            Return weatherTemp
        End Get
        Set(value As String)
            weatherTemp = value
        End Set
    End Property

    'weather relative humidity
    Public Property WeatherRelativeHumidity() As String
        Get
            Return weatherRH
        End Get
        Set(value As String)
            weatherRH = value
        End Set
    End Property

    'weather wind
    Public Property WeatherWind() As String
        Get
            Return weatherW
        End Get
        Set(value As String)
            weatherW = value
        End Set
    End Property

    ''weather sky condition
    'Public Property WeatherSkyConditions() As String
    '    Get
    '        Return weatherSC
    '    End Get
    '    Set(value As String)
    '        weatherSC = value
    '    End Set
    'End Property



End Class
