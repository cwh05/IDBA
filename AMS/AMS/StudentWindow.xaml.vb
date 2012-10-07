Imports MahApps.Metro.Controls
Imports System.Data.Objects
Imports System.Data
Imports System.Collections.Generic
Imports System.Threading
Imports System.Text.RegularExpressions


Public Class StudentWindow
    Private controller As StudentWindowController = New StudentWindowController
    Private ReadOnly StudentUsername As String
    Private enrollCourseIDList As List(Of UInteger) = New List(Of UInteger)
    Private courseList As List(Of Course)
    Private enrollCourseList As List(Of Enrollment) = New List(Of Enrollment)


    Public Sub New(ByRef username As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        StudentUsername = username
    End Sub

    Private Sub MetroWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'select empty tab
        StudentTabControl.SelectedIndex = 0

        'set weather temperature
        If CType(Application.Current, Application).WeatherTemperature <> String.Empty Then
            lblTemperatureContent.Content = CType(Application.Current, Application).WeatherTemperature
            lblTemperatureContent.ToolTip = "Relative Humidity: " & CType(Application.Current, Application).WeatherRelativeHumidity &
                    vbNewLine & "Wind: " & CType(Application.Current, Application).WeatherWind

        End If


    End Sub

    Private Sub btnEnrolCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEnrolCourse.Click
        StudentTabControl.SelectedIndex = 1

        Try
            'retrieve all courses record
            courseList = controller.GetAllCourseOfProgram(CUInt(StudentUsername.Substring(1)))
            listboxControl.ItemsSource = courseList

            listboxControl.Items.Refresh()
            enrollCourseIDList.Clear()

            'retrieve student enrollment courses ID
            For Each i In controller.GetStudentEnrollment(CInt(StudentUsername.Substring(1)))
                enrollCourseIDList.Add(i.CourseID)
            Next

        Catch ex As Exception
        End Try

        btnEnrol.IsEnabled = False

    End Sub

    Private Sub btnRemoveCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnRemoveCourse.Click
        StudentTabControl.SelectedIndex = 2

        Try
            enrollCourseList.Clear()

            'get all course enrollment data
            For Each i In controller.GetStudentEnrollment(CInt(StudentUsername.Substring(1)))
                enrollCourseList.Add(i)
            Next

            'bind source item
            removeListBoxControl.ItemsSource = enrollCourseList
            removeListBoxControl.Items.Refresh()

            btnRemove.IsEnabled = False
            btnRemove.Content = "Remove"
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnViewAssessedCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnViewAssessedCourse.Click
        StudentTabControl.SelectedIndex = 3

        Try
            enrollCourseList.Clear()

            'retrieves all student enrollment courses
            For Each i In controller.GetStudentEnrollment(CInt(StudentUsername.Substring(1)))
                enrollCourseList.Add(i)
            Next

            viewListBoxCntrl.ItemsSource = enrollCourseList
            viewListBoxCntrl.Items.Refresh()

        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnUpdateProfile_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnUpdateProfile.Click
        'forward to profile tab
        StudentTabControl.SelectedIndex = 4

        'access to entity model to retrieve all Country data
        countryCombo.ItemsSource = controller.GetAllCountry()

        'display edit button & disable all textboxes
        btnEdit.Visibility = Windows.Visibility.Visible
        btnSave.Visibility = Windows.Visibility.Hidden
        EditableTxtBox()

        Try
            'retrieve student personal information
            Dim stud As Student = controller.GetStudentDetails(StudentUsername)
            If IsNothing(stud) = False Then
                labelStudentID.Content = StudentUsername

                txtFirstName.DataContext = stud
                txtLastName.DataContext = stud

                dob.DataContext = stud
                txtContactNumber.DataContext = stud
                txtEmail.DataContext = stud
                txtAddress1.DataContext = stud
                txtAddress2.DataContext = stud
                txtCity.DataContext = stud
                txtPostCode.DataContext = stud
                txtState.DataContext = stud

                'set selected value for country combobox
                countryCombo.SelectedValue = stud.CountryCode

                'select gender type
                If stud.Gender = True Then
                    radioGenderMale.IsChecked = False
                    radioGenderFemale.IsChecked = True
                Else
                    radioGenderMale.IsChecked = True
                    radioGenderFemale.IsChecked = False
                End If

            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEdit.Click
        btnEdit.Visibility = Windows.Visibility.Hidden
        btnSave.Visibility = Windows.Visibility.Visible

        'enable editable textboxes
        EditableTxtBox(True)

    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSave.Click        
        'if no validation errors
        If validateStudent() = True Then
            btnEdit.Visibility = Windows.Visibility.Visible
            btnSave.Visibility = Windows.Visibility.Hidden

            'disable editable textboxes
            EditableTxtBox()

            Dim oldPassword As String = txtOldPassword.Password
            Dim newPassword As String = txtNewPassword.Password
            Dim confirmPassword As String = txtConfirmPassword.Password

            Dim fail = False
            Try
                Dim gender As SByte

                If radioGenderMale.IsChecked Then
                    gender = 0
                Else
                    gender = 1
                End If

                'save data without password
                If oldPassword.Length = 0 And newPassword.Length = 0 And confirmPassword.Length = 0 Then
                    fail = Not controller.UpdateProfile(txtFirstName.Text, txtLastName.Text, gender, dob.SelectedDate,
                                                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtPostCode.Text,
                                                        txtState.Text, countryCombo.SelectedValue, txtContactNumber.Text,
                                                        txtEmail.Text, CInt(StudentUsername.Substring(1)), Nothing, Nothing, Nothing)

                ElseIf newPassword.Equals(confirmPassword) Then
                    Dim dll As New AMS.Utilities.EncryptionProvider()

                    'update new password with profile details
                    fail = Not controller.UpdateProfile(txtFirstName.Text, txtLastName.Text, gender, dob.SelectedDate,
                                                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtPostCode.Text,
                                                        txtState.Text, countryCombo.SelectedValue, txtContactNumber.Text,
                                                        txtEmail.Text, CInt(StudentUsername.Substring(1)), StudentUsername,
                                                        dll.Encrypt(oldPassword), dll.Encrypt(newPassword))
                Else
                    fail = True
                End If

            Catch ex As Exception
                fail = True

            Finally
                If fail Then
                    MsgBox("Invalid old password!", MsgBoxStyle.Exclamation, "Update Information")
                Else
                    MsgBox("Update successfully.", MsgBoxStyle.Information, "Update Information")
                End If

                txtOldPassword.Password = String.Empty
                txtNewPassword.Password = String.Empty
                txtConfirmPassword.Password = String.Empty
            End Try
        End If

        Me.Finalize()
    End Sub

    Private Sub EditableTxtBox(Optional ByRef allow As Boolean = False)
        'allow/disable all textboxes
        txtFirstName.IsEnabled = allow
        txtLastName.IsEnabled = allow
        radioGenderMale.IsEnabled = allow
        radioGenderFemale.IsEnabled = allow
        dob.IsEnabled = allow
        txtContactNumber.IsEnabled = allow
        txtEmail.IsEnabled = allow
        txtAddress1.IsEnabled = allow
        txtAddress2.IsEnabled = allow
        txtCity.IsEnabled = allow
        txtPostCode.IsEnabled = allow
        txtState.IsEnabled = allow
        countryCombo.IsEnabled = allow

        txtOldPassword.IsEnabled = allow
        txtNewPassword.IsEnabled = allow
        txtConfirmPassword.IsEnabled = allow
    End Sub

    Private Sub btnEnrol_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If listboxControl.SelectedIndex >= 0 Then
            'display confirmation window
            If MessageBox.Show("Are you sure you want to enrol this course ? " _
                           + vbCrLf + courseList(listboxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + courseList(listboxControl.SelectedIndex).CourseName.ToString, "AMS",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then

                Try
                    'enrol student course
                    If controller.EnrolStudentCourse(CUInt(StudentUsername.Substring(1)), CUInt(listboxControl.SelectedValue)) Then
                        MessageBox.Show("Enrolled course successfully.", "AMS",
                            MessageBoxButton.OK, MessageBoxImage.Information)

                        'disable enrol button
                        btnEnrol.IsEnabled = False
                        btnEnrol.Content = "Enrolled"

                        enrollCourseIDList.Clear()

                        'retrieve student enrollment courses ID
                        For Each i In controller.GetStudentEnrollment(CInt(StudentUsername.Substring(1)))
                            enrollCourseIDList.Add(i.CourseID)
                        Next
                    Else
                        MessageBox.Show("Enrolled course unsuccessfully." _
                           + vbCrLf + courseList(listboxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + courseList(listboxControl.SelectedIndex).CourseName.ToString, "AMS",
                        MessageBoxButton.OK, MessageBoxImage.Error)

                    End If
                Catch ex As Exception
                    MessageBox.Show("Enrolled course unsuccessfully." _
                           + vbCrLf + courseList(listboxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + courseList(listboxControl.SelectedIndex).CourseName.ToString, "AMS",
                        MessageBoxButton.OK, MessageBoxImage.Error)
                End Try

            End If
        End If
    End Sub

    Private Sub listboxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles listboxControl.SelectionChanged
        Try
            If enrollCourseIDList.BinarySearch(CUInt(listboxControl.SelectedValue)) >= 0 Then
                btnEnrol.IsEnabled = False
                btnEnrol.Content = "Enrolled"
            Else
                btnEnrol.IsEnabled = True
                btnEnrol.Content = "Enroll"
            End If

        Catch ex As Exception
            btnEnrol.IsEnabled = True
            btnEnrol.Content = "Enroll"
        End Try
    End Sub


    Private Sub btnRemove_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If removeListBoxControl.SelectedIndex >= 0 Then
            'display confirmation window
            If MessageBox.Show("Are you sure you want to remove this course ? " _
                           + vbCrLf + enrollCourseList(removeListBoxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + enrollCourseList(removeListBoxControl.SelectedIndex).CourseName.ToString, "AMS",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then

                Try
                    'remove student course
                    If controller.RemoveStudentCourse(CUInt(StudentUsername.Substring(1)), CUInt(removeListBoxControl.SelectedValue)) Then
                        MessageBox.Show("Removed course successfully.", "AMS",
                            MessageBoxButton.OK, MessageBoxImage.Information)

                        enrollCourseList.Clear()

                        'retrieve student enrollment courses ID
                        For Each i In controller.GetStudentEnrollment(CInt(StudentUsername.Substring(1)))
                            enrollCourseList.Add(i)
                        Next

                        'removeListBoxControl

                        'bind control with new source
                        removeListBoxControl.ItemsSource = enrollCourseList
                        removeListBoxControl.Items.Refresh()

                    Else
                        MessageBox.Show("Removed course unsuccessfully." _
                           + vbCrLf + enrollCourseList(removeListBoxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + enrollCourseList(removeListBoxControl.SelectedIndex).CourseName.ToString, "AMS",
                           MessageBoxButton.OK, MessageBoxImage.Error)

                    End If
                Catch ex As Exception

                    MessageBox.Show("Removed course unsuccessfully.", "AMS",
                        MessageBoxButton.OK, MessageBoxImage.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub removeListBoxControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles removeListBoxControl.SelectionChanged
        Try
            'change the button's content if selected a particular row
            If removeListBoxControl.SelectedIndex >= 0 Then
                If enrollCourseList(removeListBoxControl.SelectedIndex).Marks Is Nothing Then
                    'enable remove button
                    btnRemove.IsEnabled = True
                    btnRemove.Content = "Remove"

                Else
                    'disable remove button
                    btnRemove.IsEnabled = False
                    btnRemove.Content = "Assessed"
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Function validateStudent() As Boolean
        Dim emailRE As New Regex("^\w+([+-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")

        ' validate student personal information
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

        ElseIf IsNothing(dob.SelectedDate) Then
            MsgBox("Please select date of birth.", MsgBoxStyle.Exclamation)
            dob.Focus()
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

            ' validate address fields
        ElseIf txtAddress1.Text.Length = 0 Then
            MsgBox("Please fill in address 1 field.", MsgBoxStyle.Exclamation)
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

        End If

        ' validate passwords textboxes fields
        If txtOldPassword.Password.Length = 0 And txtNewPassword.Password.Length = 0 And
            txtConfirmPassword.Password.Length = 0 Then
            Return True

        Else
            If txtOldPassword.Password.Length = 0 Then
                MsgBox("Old password field is empty.", MsgBoxStyle.Exclamation)
                txtOldPassword.Focus()
                Return False

            ElseIf txtNewPassword.Password.Length = 0 Then
                MsgBox("New password field is empty.", MsgBoxStyle.Exclamation)
                txtNewPassword.Focus()
                Return False

            ElseIf txtConfirmPassword.Password.Length = 0 Then
                MsgBox("Confirm password field is empty.", MsgBoxStyle.Exclamation)
                txtConfirmPassword.Focus()
                Return False

            ElseIf txtNewPassword.Password.ToString <> txtConfirmPassword.Password.ToString Then
                MsgBox("New passwords are not matched.", MsgBoxStyle.Exclamation)
                txtNewPassword.Focus()
                Return False

            End If

        End If

        Return True
    End Function

End Class
