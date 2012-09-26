﻿Imports MahApps.Metro.Controls
Imports System.Text.RegularExpressions

Public Class AdminWindow : Inherits MetroWindow

    Private adminWindowController As AdminWindowController = New AdminWindowController()
    Private emailRegularExpression As New Regex("^\w+([+-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        RefreshLookItem()
    End Sub

    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        RefreshLookItem()
    End Sub

    Private Sub btnMenuClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnCreateDepartment"
                tabContent.SelectedIndex = 1
            Case "btnCreateProgram"
                tabContent.SelectedIndex = 2
            Case "btnCreateAccount"
                tabContent.SelectedIndex = 3
            Case "btnAssignPM"
                tabContent.SelectedIndex = 4
        End Select
    End Sub

    Private Sub btnSaveDepartmentClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim department = New Department With {.DepartmentName = txtDepartmentName.Text}
        adminWindowController.CreateDepartment(department)
        ClearDeparmentForm()
    End Sub

    Private Sub btnSaveProgramClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim program = New Program With {
            .ProgramName = txtProgramName.Text,
            .ProgramDescription = txtProgramDescription.Text
        }
        adminWindowController.CreateProgram(program)
        ClearProgramForm()
    End Sub

    Private Sub btnSaveAccountClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If ValidateEmployee() Then
            Dim employee = New Employee With {
                .EmployeeFirstName = txtFirstName.Text,
                .EmployeeLastName = txtLastName.Text,
                .City = txtCity.Text,
                .Email = txtEmail.Text,
                .Address1 = txtAddress1.Text,
                .Address2 = txtAddress2.Text,
                .PostCode = txtPostCode.Text,
                .StateProvince = txtState.Text,
                .ContactNumber = txtContactNumber.Text,
                .DateOfBirth = datePickerDateofBirth.SelectedDate,
                .Country = CType(comboboxCountry.SelectedItem, Country),
                .Gender = True,
                .Account = New Account With {.LoginPassword = txtPassword.Password},
                .RoleID = CType(comboboxRole.SelectedItem, RoleCategory).RoleID
            }
            adminWindowController.CreateAccount(employee)
            ClearEmployeeForm()
        End If  
    End Sub

    Private Sub btnAssignProgramManagerClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        If listboxProgram.SelectedItem IsNot Nothing And listboxStaff.SelectedItem IsNot Nothing Then
            Dim program = CType(listboxProgram.SelectedItem, Program)
            program.Employee = CType(listboxStaff.SelectedItem, Employee)
            adminWindowController.AssginProgramManager(program)
            RefreshLookItem()
        Else
            MsgBox("Please select both staff and program before assign.", MsgBoxStyle.Exclamation)
        End If

    End Sub

    Public Sub RefreshLookItem()
        comboboxCountry.ItemsSource = adminWindowController.GetAllCountryForLookUp()
        comboboxRole.ItemsSource = adminWindowController.GetAllRoleForLookUp()
        listboxProgram.ItemsSource = adminWindowController.GetAllProgramForLookUp()
        listboxStaff.ItemsSource = adminWindowController.GetAllEmployeeForLookUp()
    End Sub

    Public Sub ClearDeparmentForm()
        txtDepartmentName.Text = String.Empty
    End Sub

    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub

    Public Sub ClearEmployeeForm()
        txtCity.Text = String.Empty
        txtEmail.Text = String.Empty
        txtState.Text = String.Empty
        txtLastName.Text = String.Empty
        txtAddress1.Text = String.Empty
        txtAddress2.Text = String.Empty
        txtPostCode.Text = String.Empty
        txtFirstName.Text = String.Empty
        txtPassword.Password = String.Empty
        txtContactNumber.Text = String.Empty
        comboboxRole.SelectedIndex = 0
        comboboxCountry.SelectedIndex = 0
        datePickerDateofBirth.SelectedDate = DateTime.Today
    End Sub

    Private Function ValidateEmployee() As Boolean
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
        ElseIf Not emailRegularExpression.IsMatch(txtEmail.Text) Then
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
        ElseIf comboboxRole.SelectedItem Is Nothing Then
            MsgBox("Please select role for employee.", MsgBoxStyle.Exclamation)
            txtPassword.Focus()
            Return False
        ElseIf txtPassword.Password.Length = 0 Then
            MsgBox("Password field cannot be empty.", MsgBoxStyle.Exclamation)
            txtPassword.Focus()
            Return False
        ElseIf txtPassword.Password <> txtConfirmPassword.Password Then
            MsgBox("Confirm password is not match.", MsgBoxStyle.Exclamation)
            txtPassword.Focus()
            Return False
        End If
        Return True
    End Function
End Class
