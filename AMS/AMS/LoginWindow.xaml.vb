Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim adminWindow As New AdminWindow()
        adminWindow.Visibility = Windows.Visibility.Visible
    End Sub
End Class
