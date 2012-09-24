Imports System.Threading
Imports System.ComponentModel

Public Class SplashWindow

    Private Sub SplashWindow_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        GC.Collect()

        'forward to login window
        Dim login As New LoginWindow
        login.Show()

    End Sub

    Private Sub SplashWindow_ContentRendered(sender As Object, e As System.EventArgs) Handles Me.ContentRendered
        ' run background thread to get weather information
        Dim bw As New BackgroundWorker()

        'add event handlers
        AddHandler bw.DoWork, AddressOf bw_DoWork
        AddHandler bw.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted

        'run background worker
        bw.WorkerSupportsCancellation = True
        bw.RunWorkerAsync()

    End Sub

    Private Sub bw_DoWork(sender As Object, ByVal e As DoWorkEventArgs)
        TemperatureChanged()
    End Sub

    Private Sub bw_RunWorkerCompleted(sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        CType(sender, BackgroundWorker).CancelAsync()
        CType(sender, BackgroundWorker).Dispose()
        Me.Close()

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

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

        End Try
    End Sub


End Class
