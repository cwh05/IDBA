Public Class StaffWindow

    Private controller As StaffWindowController = New StaffWindowController
    Private courseList As List(Of Course)
    Private ReadOnly StaffUsername As String

    Public Sub New(ByRef username As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        StaffUsername = username
    End Sub

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnCreateStudent"
                tabContent.SelectedIndex = 1
            Case "btnAssessmentInfo"
                tabContent.SelectedIndex = 2
            Case "btnViewCourseEnrolment"
                tabContent.SelectedIndex = 3
                btnViewCourseEnrolment_Click()
            Case "btnCourseDetail"
                tabContent.SelectedIndex = 4
        End Select
    End Sub

    Private Sub btnViewCourseEnrolment_Click()

        Try
            'retrieve all courses record
            courseList = controller.GetAllCourseOfStaff(CUInt(StaffUsername))
            staffListboxControl.ItemsSource = courseList

            staffListboxControl.Items.Refresh()

        Catch ex As Exception
        End Try

    End Sub
End Class
