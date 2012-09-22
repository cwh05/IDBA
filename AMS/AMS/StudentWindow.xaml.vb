Imports MahApps.Metro.Controls
Imports System.Data.Objects
Imports System.Data
Imports System.Collections.Generic
Imports System.Threading


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

        'start thread to update temperature
        Dim temperatureThread As New Thread(New ThreadStart(AddressOf UpdateTemperature))
        temperatureThread.Name = "TemperatureThread"
        temperatureThread.IsBackground = True
        temperatureThread.SetApartmentState(ApartmentState.STA)
        temperatureThread.Start()




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
        btnEdit.Visibility = Windows.Visibility.Visible
        btnSave.Visibility = Windows.Visibility.Hidden

        'disable editable textboxes
        EditableTxtBox()

        Dim oldPassword As String = txtOldPassword.Password
        Dim newPassword As String = txtNewPassword.Password
        Dim confirmPassword As String = txtConfirmPassword.Password

        'validate student textboxes details
        Dim valid1 = controller.validatePersonalDetail(txtFirstName.Text, txtLastName.Text, dob.SelectedDate.ToString,
                                                       txtContactNumber.Text, txtEmail.Text, radioGenderMale.IsChecked,
                                                       radioGenderFemale.IsChecked)
        Dim valid2 = controller.validateAddressDetail(txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtPostCode.Text,
                                                      txtState.Text, countryCombo.SelectedValue)


        Dim fail = False
        Try
            'update profile details if no errors occur
            If valid1 And valid2 Then
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
                    'update new password with profile details
                    fail = Not controller.UpdateProfile(txtFirstName.Text, txtLastName.Text, gender, dob.SelectedDate,
                                                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtPostCode.Text,
                                                        txtState.Text, countryCombo.SelectedValue, txtContactNumber.Text,
                                                        txtEmail.Text, CInt(StudentUsername.Substring(1)), StudentUsername,
                                                        oldPassword, newPassword)
                Else
                    fail = True
                End If
            Else
                fail = True
            End If
        Catch ex As Exception
            fail = True

        Finally
            If fail Then
                MsgBox("Fail to update.", MsgBoxStyle.Exclamation, "Update Information")
            Else
                MsgBox("Update successfully.", MsgBoxStyle.Information, "Update Information")
            End If

            txtOldPassword.Password = String.Empty
            txtNewPassword.Password = String.Empty
            txtConfirmPassword.Password = String.Empty
        End Try

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
                    MessageBox.Show("Removed course unsuccessfully." _
                           + vbCrLf + enrollCourseList(removeListBoxControl.SelectedIndex).CourseCode.ToString + ": " _
                           + enrollCourseList(removeListBoxControl.SelectedIndex).CourseName.ToString, "AMS",
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



    Private Function UpdateTemperature()
        'connect to WCF Service
        Dim service = New WCFService.ServiceClient()
        Dim weatherResult = service.GetWeather("Melbourne")


        'display temperature
        'lblTemperature.Visibility = Windows.Visibility.Visible
        'lblTemperatureContent.Visibility = Windows.Visibility.Visible
        'lblTemperatureContent.Content = weatherResult.Item("Temperature")




        'Dim sp As New StackPanel
        'sp.Orientation = Orientation.Horizontal
        'sp.HorizontalAlignment = Windows.HorizontalAlignment.Left
        'sp.VerticalAlignment = Windows.VerticalAlignment.Bottom
        'sp.SetValue(Grid.RowProperty, 4)
        'sp.SetValue(Grid.ColumnProperty, 0)
        'sp.SetValue(Grid.ColumnSpanProperty, 2)
        'sp.Visibility = Windows.Visibility.Visible

        'Dim lblTemperature = lblTemperatureContent
        'lblTemperature.Content = "xxxx"
        'lblTemperature.Visibility = Windows.Visibility.Visible

        'sp.Children.Add(lblTemperature)


        MsgBox("True") '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Nothing


    End Function




End Class
