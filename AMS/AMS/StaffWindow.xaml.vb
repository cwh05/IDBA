Public Class StaffWindow

    Private controller As StaffWindowController = New StaffWindowController
    Private courseList As List(Of Course)
    Private ReadOnly StaffUsername As String
    Private enrollCourseList As List(Of Enrollment) = New List(Of Enrollment)
    Private editable As Boolean

    Public Sub New(ByRef username As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        StaffUsername = username
        editable = False

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
                ViewCourseEnrolment(viewCourseListboxControl)
            Case "btnCourseDetail"
                tabContent.SelectedIndex = 4
                ViewCourseEnrolment(courseDetailListboxControl)
        End Select
    End Sub

    Private Sub ViewCourseEnrolment(listboxControl As ListBox)

        Try
            'retrieve all courses record
            courseList = controller.GetAllCourseOfStaff(CUInt(StaffUsername))
            listboxControl.ItemsSource = courseList

            listboxControl.Items.Refresh()

        Catch ex As Exception
        End Try

    End Sub

    Private Sub viewCourseListboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles viewCourseListboxControl.SelectionChanged

        Try
            'retrieve all courses record
            If viewCourseListboxControl.SelectedIndex >= 0 Then
                enrollCourseList = controller.GetStudentEnrollmentByCourseID(CUInt(viewCourseListboxControl.SelectedValue.ToString))
                studentListboxControl.ItemsSource = enrollCourseList

                studentListboxControl.Items.Refresh()
            End If
            

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try
    End Sub

    Private Sub courseDetailListboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles courseDetailListboxControl.SelectionChanged

        Try
            If courseDetailListboxControl.SelectedIndex >= 0 Then
                Dim ent As New Course
                ent = courseDetailListboxControl.SelectedItem

                tbCourseCode.Text = ent.CourseCode
                tbCourseName.Text = ent.CourseName
                tbCourseDesc.Text = ent.CourseDescription

                tbCourseCode.IsEnabled = False
                tbCourseName.IsEnabled = False
                tbCourseDesc.IsEnabled = False
                editable = False
                btnEdit.Content = "edit"
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEdit.Click

        If editable Then
            If tbCourseCode.Text.Length > 0 And tbCourseDesc.Text.Length > 0 And tbCourseName.Text.Length > 0 Then

                If MsgBox("Update course?", MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.Yes Then
                    If controller.UpdateCourse(tbCourseCode.Text, tbCourseName.Text, tbCourseDesc.Text, courseDetailListboxControl.SelectedValue) Then
                        tbCourseCode.IsEnabled = False
                        tbCourseName.IsEnabled = False
                        tbCourseDesc.IsEnabled = False
                        editable = False
                        btnEdit.Content = "edit"
                        ViewCourseEnrolment(courseDetailListboxControl)
                    End If
                End If
            End If
        Else
            tbCourseCode.IsEnabled = True
            tbCourseName.IsEnabled = True
            tbCourseDesc.IsEnabled = True
            editable = True
            btnEdit.Content = "update"
        End If


    End Sub
End Class
