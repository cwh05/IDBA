Public Class ProgramWindow

    Private Sub btnProgramInfo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 0
    End Sub

    Private Sub btnAssignCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 1
    End Sub

    Private Sub btnViewProgramEnrolment_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 2
    End Sub

    Private Sub btnViewCourseEnrolment_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        tabContent.SelectedIndex = 3
    End Sub
End Class
