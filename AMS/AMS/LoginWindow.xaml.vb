Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow

    Dim loginWindowController As LoginWindowContoller = New LoginWindowContoller()

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'validate input textboxes
        If (loginWindowController.UserVerification(txtUsername.Text, txtPassword.Password)) Then
            Dim adminWindow As New AdminWindow()
            adminWindow.Visibility = Windows.Visibility.Visible
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub MetroWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtUsername.Focus()
    End Sub
End Class
