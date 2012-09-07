Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow

    Private Sub btnLogin_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        'validate input textboxes
        Dim adminWindow As New AdminWindow()
        adminWindow.Visibility = Windows.Visibility.Visible

    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub MetroWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtUsername.Focus()
    End Sub
End Class
