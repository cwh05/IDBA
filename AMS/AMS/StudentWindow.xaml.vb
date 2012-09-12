Imports MahApps.Metro.Controls
Imports System.Data.Objects


Public Class StudentWindow
    Private controller As StudentWindowController = New StudentWindowController
    Private ReadOnly StudentUsername As String


    Public Sub New(ByRef username As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        StudentUsername = username
    End Sub

    Private Sub MetroWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'select empty tab
        StudentTabControl.SelectedIndex = 0

        'access to entity model to retrieve all Country data
        countryCombo.ItemsSource = controller.GetAllCountry()

    End Sub

    Private Sub btnEnrolCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEnrolCourse.Click
        StudentTabControl.SelectedIndex = 1

        'If dt Is Nothing = False Then
        '    dt.Clear()
        'End If

        ''get all courses into datagrid
        'dt = db.GetAllCourse
        'listboxControl.ItemsSource = dt




        'Dim dt2 As DataTable = db.GetStudentEnrolCourse(CInt(StudentUsername.Substring(1)))
        'Dim enrolledCourseList As List(Of Int32) = New List(Of Int32)

        ''insert enrolled courseID into list

        'If dt2 Is Nothing = False Then
        '    For Each i In dt2.Rows
        '        Dim dr As DataGridRow = i
        '        enrolledCourseList.Add(CInt(dr.Item("CourseID")))

        '        dr.IsEnabled = False


        '    Next
        'End If

        'Dim temp As New DataTable
        'For Each i In dt.Rows
        '    Dim dr As DataRow = i

        '    temp.Rows.Add(dr.Item(0))
        'Next

        'For Each i In dt.Rows
        '    Dim dr As DataGridRow = DirectCast(i, DataGridRow)

        '    'Dim id = CInt(dr.Item("CourseID"))

        '    dr.IsEnabled = False

        '    ' Dim dc As DataGridCell = dr

        '    'If enrolledCourseList.BinarySearch(id) >= 0 Then

        '    'End If
        'Next
        'enrolledCourseList.Clear()

    End Sub

    Private Sub btnRemoveCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnRemoveCourse.Click
        StudentTabControl.SelectedIndex = 2


    End Sub

    Private Sub btnViewAssessedCourse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnViewAssessedCourse.Click
        StudentTabControl.SelectedIndex = 3


    End Sub

    Private Sub btnUpdateProfile_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnUpdateProfile.Click
        'forward to profile tab
        StudentTabControl.SelectedIndex = 4

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
        Dim valid1 = controller.validatePersonalDetail(txtFirstName.Text, txtLastName.Text, dob.Text, txtContactNumber.Text,
                                                      txtEmail.Text, radioGenderMale.IsChecked, radioGenderFemale.IsChecked)
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
                    fail = Not controller.UpdateProfile(txtFirstName.Text, txtLastName.Text, gender, dob.DisplayDate,
                                                        txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtPostCode.Text,
                                                        txtState.Text, countryCombo.SelectedValue, txtContactNumber.Text,
                                                        txtEmail.Text, CInt(StudentUsername.Substring(1)), Nothing, Nothing, Nothing)

                ElseIf newPassword.Equals(confirmPassword) Then
                    'update new password with profile details
                    fail = Not controller.UpdateProfile(txtFirstName.Text, txtLastName.Text, gender, dob.DisplayDate,
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

    End Sub
End Class
