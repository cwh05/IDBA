﻿Imports System.Windows.Controls.Primitives
Imports System.Text.RegularExpressions

Public Class StaffWindow

    Private controller As StaffWindowController = New StaffWindowController
    Private courseList As List(Of Course)
    Private ReadOnly StaffUsername As String
    Private enrollCourseList As List(Of Enrollment) = New List(Of Enrollment)
    Private editable As Boolean
    Private selectedCourse As Integer

    Dim emailRE As New Regex("^\w+([+-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")

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
            lblTemperatureContent.ToolTip = "Relative Humidity: " & CType(Application.Current, Application).WeatherRelativeHumidity &
                    vbNewLine & "Wind: " & CType(Application.Current, Application).WeatherWind

        End If

    End Sub

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnCreateStudent"
                tabContent.SelectedIndex = 1
            Case "btnAssessmentInfo"
                tabContent.SelectedIndex = 2
                ViewCourseEnrolment(assessmentInfoListboxControl)
                If assessmentInfoStudentListboxControl.Items.Count > 0 Then
                    assessmentInfoStudentListboxControl.ItemsSource = Nothing
                End If
            Case "btnViewCourseEnrolment"
                tabContent.SelectedIndex = 3
                ViewCourseEnrolment(viewCourseListboxControl)
                If studentListboxControl.Items.Count > 0 Then
                    studentListboxControl.ItemsSource = Nothing
                End If

            Case "btnCourseDetail"
                    tabContent.SelectedIndex = 4
                    ViewCourseEnrolment(courseDetailListboxControl)
                    tbCourseCode.Text = ""
                    tbCourseName.Text = ""
                    tbCourseDesc.Text = ""
        End Select
    End Sub

    Private Sub ViewCourseEnrolment(listboxControl As ListBox)

        Try
            'retrieve all courses record
            courseList = controller.GetAllCourseByStaff(CUInt(StaffUsername.Substring(1)))
            listboxControl.ItemsSource = courseList

            listboxControl.Items.Refresh()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try

    End Sub

    Private Sub viewCourseListboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles viewCourseListboxControl.SelectionChanged

        updateStudentListBox(viewCourseListboxControl, studentListboxControl)
    End Sub

    Private Sub assessmentInfoListboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles assessmentInfoListboxControl.SelectionChanged

        updateStudentListBox(assessmentInfoListboxControl, assessmentInfoStudentListboxControl)
    End Sub

    Private Sub courseDetailListboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles courseDetailListboxControl.SelectionChanged

        Try
            If courseDetailListboxControl.SelectedIndex >= 0 Then
                Dim ent As New Course
                ent = courseDetailListboxControl.SelectedItem

                selectedCourse = courseDetailListboxControl.SelectedValue

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

        If courseDetailListboxControl.SelectedIndex >= 0 Then

            If editable Then
                If tbCourseCode.Text.Trim.Length <= 0 Then
                    MsgBox("Please fill in course code field.", MsgBoxStyle.Exclamation)
                    tbCourseCode.Focus()
                ElseIf tbCourseName.Text.Trim.Length <= 0 Then
                    MsgBox("Please fill in course name field.", MsgBoxStyle.Exclamation)
                    tbCourseName.Focus()
                ElseIf tbCourseDesc.Text.Trim.Length <= 0 Then
                    MsgBox("Please fill in course description field.", MsgBoxStyle.Exclamation)
                    tbCourseDesc.Focus()
                Else
                    If MsgBox("Update Course: " & tbCourseCode.Text & "?", MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.Yes Then
                        If controller.UpdateCourse(tbCourseCode.Text, tbCourseName.Text, tbCourseDesc.Text, selectedCourse) Then
                            tbCourseCode.IsEnabled = False
                            tbCourseName.IsEnabled = False
                            tbCourseDesc.IsEnabled = False
                            tbCourseCode.Text = ""
                            tbCourseName.Text = ""
                            tbCourseDesc.Text = ""
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
        Else
            MsgBox("No course is selected.", MsgBoxStyle.Information)
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
                account.LoginPassword = txtPassword.Text

                If controller.InsertStudent(newStud, account) Then
                    MsgBox("New Student ID '" & newStud.StudentLastName & "' has been added successfully!" & vbNewLine & "Username: " & controller.GetLatestLoginUsername(), MsgBoxStyle.Information, "Congratulation")

                    ClearAllField()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try


    End Sub

    Private Function validateStudent() As Boolean

        
        If txtFirstName.Text.Length = 0 Then
            MsgBox("Please fill in first name field.", MsgBoxStyle.Exclamation)
            txtFirstName.Focus()
            Return False
        ElseIf txtLastName.Text.Length = 0 Then
            MsgBox("Please fill in last name field.", MsgBoxStyle.Exclamation)
            txtLastName.Focus()
            Return False
        ElseIf Not (radioGenderMale.IsChecked Or radioGenderFemale.IsChecked) Then
            MsgBox("Please select your gender.", MsgBoxStyle.Exclamation)
            radioGenderMale.Focus()
            Return False
        ElseIf IsNothing(datePickerDateofBirth.SelectedDate) Then
            MsgBox("Please select date of birth.", MsgBoxStyle.Exclamation)
            datePickerDateofBirth.Focus()
            Return False
        ElseIf txtContactNumber.Text.Length = 0 Then
            MsgBox("Please fill in contact number field.", MsgBoxStyle.Exclamation)
            txtContactNumber.Focus()
            Return False
        ElseIf Not IsNumeric(txtContactNumber.Text) Then
            MsgBox("Contact number must be numerical value.", MsgBoxStyle.Exclamation)
            txtContactNumber.Focus()
            Return False
        ElseIf txtEmail.Text.Length = 0 Then
            MsgBox("Please fill in email field.", MsgBoxStyle.Exclamation)
            txtEmail.Focus()
            Return False
        ElseIf Not emailRE.IsMatch(txtEmail.Text) Then
            MsgBox("Email format error.", MsgBoxStyle.Exclamation)
            txtEmail.Focus()
            Return False
        ElseIf txtAddress1.Text.Length = 0 Then
            MsgBox("Please fill in addressline 1 field.", MsgBoxStyle.Exclamation)
            txtAddress1.Focus()
            Return False
        ElseIf txtCity.Text.Length = 0 Then
            MsgBox("Please fill in city field.", MsgBoxStyle.Exclamation)
            txtCity.Focus()
            Return False
        ElseIf txtPostCode.Text.Length = 0 Then
            MsgBox("Please fill in postcode field.", MsgBoxStyle.Exclamation)
            txtPostCode.Focus()
            Return False
        ElseIf Not IsNumeric(txtPostCode.Text) Then
            MsgBox("Postcode must be numerical value.", MsgBoxStyle.Exclamation)
            txtPostCode.Focus()
            Return False
        ElseIf txtState.Text.Length = 0 Then
            MsgBox("Please fill in state province field.", MsgBoxStyle.Exclamation)
            txtState.Focus()
            Return False
        ElseIf txtPassword.Text.Length = 0 Then
            MsgBox("Password field cannot be empty.", MsgBoxStyle.Exclamation)
            txtPassword.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub datePickerDateofBirth_SelectedDateChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles datePickerDateofBirth.SelectedDateChanged

        txtPassword.Text = datePickerDateofBirth.Text

    End Sub

    Private Sub ClearAllField()
        txtFirstName.Text = Nothing
        txtLastName.Text = Nothing
        datePickerDateofBirth.SelectedDate = Nothing
        txtContactNumber.Text = Nothing
        txtEmail.Text = Nothing
        txtAddress1.Text = Nothing
        txtAddress2.Text = Nothing
        txtCity.Text = Nothing
        txtPostCode.Text = Nothing
        txtState.Text = Nothing
        comboboxCountry.SelectedIndex = 0
        comboboxProgram.SelectedIndex = 0
        If radioGenderMale.IsChecked Then
            radioGenderMale.IsChecked = False
        Else
            radioGenderFemale.IsChecked = False
        End If
        txtPassword.Text = Nothing

    End Sub

    Private Sub updateStudentListBox(inputLB As ListBox, outputLB As ListBox)
        Try
            'retrieve all courses record
            If inputLB.SelectedIndex >= 0 Then
                enrollCourseList = controller.GetStudentEnrollmentByCourseID(CUInt(inputLB.SelectedValue.ToString))
                outputLB.ItemsSource = enrollCourseList

                outputLB.Items.Refresh()
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try
    End Sub

    Private Sub assessmentInfoStudentListboxControl_MouseDoubleClick(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles assessmentInfoStudentListboxControl.MouseDoubleClick

        Dim dialog As New MyCustomDialog

        dialog.ShowDialog()

        If dialog.MyReturnResult = 1 Then

            If IsNothing(dialog.ResponseText) Or dialog.ResponseText.Length = 0 Then
                MsgBox("Please fill in the marks.", MsgBoxStyle.Exclamation)
            ElseIf Not IsNumeric(dialog.ResponseText) Then
                MsgBox("Please enter numerical value only.", MsgBoxStyle.Exclamation)
            ElseIf (CInt(dialog.ResponseText) > 100 Or CInt(dialog.ResponseText) < 0) Then
                MsgBox("Please enter value in range [0-100].", MsgBoxStyle.Exclamation)
            Else
                Try
                    If controller.UpdateStudentMark(assessmentInfoListboxControl.SelectedValue, assessmentInfoStudentListboxControl.SelectedValue, dialog.ResponseText) Then
                        If (CInt(dialog.ResponseText) <> -1) Then
                            txtAverage.Text = dialog.ResponseText
                        End If
                    End If

                    updateStudentListBox(assessmentInfoListboxControl, assessmentInfoStudentListboxControl)

                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
                End Try
            End If
        End If

    End Sub

End Class
