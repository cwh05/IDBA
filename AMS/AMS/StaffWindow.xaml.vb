Imports System.Windows.Controls.Primitives

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

        'set weather temperature
        If CType(Application.Current, Application).WeatherTemperature <> String.Empty Then
            lblTemperatureContent.Content = CType(Application.Current, Application).WeatherTemperature
        End If

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
        Dim account As New Account

        Try
            If validateStudent() Then
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
                account.LoginUsername = controller.GeNewStudentID()
                account.LoginPassword = txtPassword.Text

                controller.InsertStudent(newStud, account)
                'controller
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try


    End Sub

    Private Function validateStudent() As Boolean


        If txtFirstName.Text.Length = 0 Then
            MsgBox("Please fill in first name field.", MsgBoxStyle.Information)
            txtFirstName.Focus()
            Return False
        ElseIf txtLastName.Text.Length = 0 Then
            MsgBox("Please fill in last name field.", MsgBoxStyle.Information)
            txtLastName.Focus()
            Return False
        ElseIf Not (radioGenderMale.IsChecked Or radioGenderFemale.IsChecked) Then
            MsgBox("Please select your gender.", MsgBoxStyle.Information)
            radioGenderMale.Focus()
            Return False
        ElseIf IsNothing(datePickerDateofBirth.SelectedDate) Then
            MsgBox("Please select date of birth.", MsgBoxStyle.Information)
            datePickerDateofBirth.Focus()
            Return False
        ElseIf txtContactNumber.Text.Length = 0 Then
            MsgBox("Please fill in contact number field.", MsgBoxStyle.Information)
            txtContactNumber.Focus()
            Return False
        ElseIf txtEmail.Text.Length = 0 Then
            MsgBox("Please fill in email field.", MsgBoxStyle.Information)
            txtEmail.Focus()
            Return False
        ElseIf txtAddress1.Text.Length = 0 Then
            MsgBox("Please fill in addressline 1 field.", MsgBoxStyle.Information)
            txtAddress1.Focus()
            Return False
        ElseIf txtCity.Text.Length = 0 Then
            MsgBox("Please fill in city field.", MsgBoxStyle.Information)
            txtCity.Focus()
            Return False
        ElseIf txtPostCode.Text.Length = 0 Then
            MsgBox("Please fill in postcode field.", MsgBoxStyle.Information)
            txtPostCode.Focus()
            Return False
        ElseIf txtState.Text.Length = 0 Then
            MsgBox("Please fill in state province field.", MsgBoxStyle.Information)
            txtState.Focus()
            Return False
        ElseIf txtPassword.Text.Length = 0 Then
            MsgBox("Password field cannot be empty.", MsgBoxStyle.Information)
            txtPassword.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub datePickerDateofBirth_SelectedDateChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles datePickerDateofBirth.SelectedDateChanged

        txtPassword.Text = datePickerDateofBirth.Text

    End Sub
End Class
