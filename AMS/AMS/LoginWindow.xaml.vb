Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'validate input textboxes
        Dim adminWindow As New ProgramWindow()
        adminWindow.Visibility = Windows.Visibility.Visible
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub MetroWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtUsername.Focus()
    End Sub
End Class
