Imports System.Threading

Public Class SplashWindow

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'start thread to update temperature
        Dim temperatureThread As New Thread(New ThreadStart(AddressOf UpdateTemperature))
        temperatureThread.Name = "TemperatureThread"
        temperatureThread.IsBackground = True
        temperatureThread.SetApartmentState(ApartmentState.MTA)
        temperatureThread.Start()

    End Sub

    Private Sub UpdateTemperature()
        'invoke delegate method
        Me.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                                  New Action(AddressOf TemperatureChanged))
    End Sub

    Private Sub TemperatureChanged()
        Try
            'connect to WCF Service
            Dim service = New WCFService.ServiceClient()
            Dim weatherResult = service.GetWeather("Melbourne")

            'save value to application
            CType(Application.Current, Application).WeatherTemperature = weatherResult.Item("Temperature")
            CType(Application.Current, Application).WeatherRelativeHumidity = weatherResult.Item("RelativeHumidity")
            CType(Application.Current, Application).WeatherWind = weatherResult.Item("Wind")

        Catch ex As Exception
            'save empty values if error occurs
            CType(Application.Current, Application).WeatherTemperature = String.Empty
            CType(Application.Current, Application).WeatherRelativeHumidity = String.Empty
            CType(Application.Current, Application).WeatherWind = String.Empty

        Finally
            Dim login As New LoginWindow
            login.Show()

            Me.Close()
            GC.Collect()
        End Try
    End Sub


End Class
