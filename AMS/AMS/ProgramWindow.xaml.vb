Public Class ProgramWindow

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked As Button = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnProgramInfo"
                tabContent.SelectedIndex = 0
            Case "btnAssignCourse"
                tabContent.SelectedIndex = 1
            Case "btnViewProgramEnrolment"
                tabContent.SelectedIndex = 2
            Case "btnViewCourseEnrolment"
                tabContent.SelectedIndex = 3
        End Select
    End Sub
End Class
