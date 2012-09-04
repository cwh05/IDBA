Imports MahApps.Metro.Controls

Public Class AdminWindow : Inherits MetroWindow

    Private Sub btnCreateDepartment_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 0
    End Sub

    Private Sub btnCreateProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 1
    End Sub

    Private Sub btnCreateAccount_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 2
    End Sub
End Class
