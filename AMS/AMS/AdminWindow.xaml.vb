Imports MahApps.Metro.Controls

Public Class AdminWindow : Inherits MetroWindow

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked As Button = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnCreateDepartment"
                tabContent.SelectedIndex = 0
            Case "btnCreateProgram"
                tabContent.SelectedIndex = 1
            Case "btnCreateAccount"
                tabContent.SelectedIndex = 2
        End Select
    End Sub
End Class
