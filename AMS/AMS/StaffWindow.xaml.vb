Public Class StaffWindow

    Private controller As StaffWindowController = New StaffWindowController
    Private courseList As List(Of Course)
    Private ReadOnly StaffUsername As String
    Private enrollCourseList As List(Of Enrollment) = New List(Of Enrollment)
    Private editable As Boolean

    Public Sub New(ByRef username As String)
        ' This call is required by the designer.
        InitializeComponent()

        comboboxCountry.ItemsSource = controller.GetAllCountryForLookUp()
        comboboxProgram.ItemsSource = controller.GetAllProgramForLookUp()
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
            courseList = controller.GetAllCourseByStaff(CUInt(StaffUsername))
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

                If MsgBox("Update Course: " & tbCourseCode.Text & "?", MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.Yes Then
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

    Private Sub btnSave_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSave.Click

        Dim newStud As New Student

        Try
            newStud.StudentFirstName = txtFirstName.Text
            newStud.StudentLastName = txtLastName.Text
            newStud.DateOfBirth = datePickerDateofBirth.SelectedDate
            newStud.ContactNumber = txtContactNumber.Text
            newStud.Email = txtEmail.Text
            newStud.Address1 = txtAddress1.Text
            newStud.Address2 = txtAddress2.Text
            newStud.City = txtCity.Text
            newStud.PostCode = txtPostCode.Text
            newStud.StateProvince = txtState.Text
            newStud.CountryCode = comboboxCountry.SelectedValue
            newStud.ProgramID = comboboxProgram.SelectedValue
            If radioGenderMale.IsChecked Then
                newStud.Gender = False
            Else
                newStud.Gender = True
            End If

            If validateStudent(newStud) Then

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try
        

    End Sub

    Private Function validateStudent(ByVal stud As Student) As Boolean

        If stud.StudentFirstName.Length = 0 Then

        ElseIf stud.StudentLastName.Length = 0 Then

        ElseIf stud.StateProvince.Length = 0 Then

        ElseIf stud.PostCode.Length = 0 Then

        ElseIf stud.Email.Length = 0 Then

        ElseIf IsNothing(stud.DateOfBirth) Then

        ElseIf stud.ContactNumber.Length = 0 Then

        ElseIf stud.City.Length = 0 Then

        ElseIf stud.Address1.Length = 0 Then

        End If

        Return True
    End Function
End Class
