Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow

    Private Sub btnLogin_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        'validate input textboxes
        If txtUsername.Text.Length = 0 And txtPassword.Password.Length = 0 Then
            errorMsg.Visibility = Windows.Visibility.Visible
            txtUsername.Focus()

        Else
            'connect to database


        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub MetroWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtUsername.Focus()
    End Sub
End Class
