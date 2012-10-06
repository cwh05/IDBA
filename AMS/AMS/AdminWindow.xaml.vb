Imports MahApps.Metro.Controls
Imports System.Text.RegularExpressions

Public Class AdminWindow : Inherits MetroWindow

    Private adminWindowController As AdminWindowController = New AdminWindowController()
    Private emailRegularExpression As New Regex("^\w+([+-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")

    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        RefreshLookItem()
    End Sub

    Private Sub MetroWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'set weather temperature
        If CType(Application.Current, Application).WeatherTemperature <> String.Empty Then
            lblTemperatureContent.Content = CType(Application.Current, Application).WeatherTemperature
            lblTemperatureContent.ToolTip = "Relative Humidity: " & CType(Application.Current, Application).WeatherRelativeHumidity &
                    vbNewLine & "Wind: " & CType(Application.Current, Application).WeatherWind

        End If
    End Sub

    ''' <summary>
    ''' Swicthing for tab on Admin windows
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Event handler for department save button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveDepartment_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Validating department fields
            If ValidateDepartment() Then
                'New department obeject
                Dim department = New Department With {.DepartmentName = txtDepartmentName.Text}
                'Save department
                adminWindowController.CreateDepartment(department)
                MsgBox("Department " & txtDepartmentName.Text & " has been created.", MsgBoxStyle.Information)
                ClearDeparmentForm()
                RefreshLookItem()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for program save button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Validating program fields
            If ValidateProgram() Then
                Dim program = New Program With {
                    .ProgramName = txtProgramName.Text,
                    .ProgramDescription = txtProgramDescription.Text
                }

                'Save program detail to database
                adminWindowController.CreateProgram(program)
                MsgBox("Program " & txtProgramName.Text & " has been created.", MsgBoxStyle.Information)

                'Clear program form
                ClearProgramForm()

                'Reload the updated detail in for every forms
                RefreshLookItem()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for account save button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveAccount_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Validating employee fields
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
                Dim department = CType(comboboxDepartment.SelectedItem, Department)

                'Assign department to employee
                employee.Departments.Add(department)

                'Save employee detail to database
                adminWindowController.CreateAccount(employee)
                MsgBox("New username " & adminWindowController.GetLatestUsername() & " was created.", MsgBoxStyle.Information)

                'Clear program form
                ClearEmployeeForm()

                'Reload the updated detail in for every forms
                RefreshLookItem()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for assign button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAssignProgramManagerClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Check both lists have selected 
            If listboxProgram.SelectedItem IsNot Nothing And listboxStaff.SelectedItem IsNot Nothing Then
                Dim program = CType(listboxProgram.SelectedItem, Program)
                program.Employee = CType(listboxStaff.SelectedItem, Employee)

                'Assign program to employee
                adminWindowController.AssginProgramManager(program)

                'Reload the updated detail in for every forms
                RefreshLookItem()
                MsgBox("Program " & program.ProgramName & " managed by " & program.Employee.EmployeeFirstName, MsgBoxStyle.Information)
            Else
                MsgBox("Please select both staff and program before assign.", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Refresh data in the windows
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshLookItem()
        Try
            'Retrieve fresh information from database
            comboboxCountry.ItemsSource = adminWindowController.GetAllCountryForLookUp()
            comboboxRole.ItemsSource = adminWindowController.GetAllRoleForLookUp()
            comboboxDepartment.ItemsSource = adminWindowController.GetAllDepartmentForLookUp()
            listboxProgram.ItemsSource = adminWindowController.GetAllProgramForLookUp()
            listboxStaff.ItemsSource = adminWindowController.GetAllEmployeeForLookUp()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Clear department form
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearDeparmentForm()
        txtDepartmentName.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Clear program form
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Clear employee form
    ''' </summary>
    ''' <remarks></remarks>
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
        txtConfirmPassword.Password = String.Empty
        txtContactNumber.Text = String.Empty
        comboboxRole.SelectedIndex = 0
        comboboxCountry.SelectedIndex = 0
        comboboxDepartment.SelectedItem = 0
        datePickerDateofBirth.SelectedDate = DateTime.Today
    End Sub

    ''' <summary>
    ''' Validate the account form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
            comboboxRole.Focus()
            Return False
        ElseIf comboboxDepartment.SelectedItem Is Nothing Then
            MsgBox("Please select department for employee.", MsgBoxStyle.Exclamation)
            comboboxDepartment.Focus()
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

    ''' <summary>
    ''' Validate the department form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateDepartment() As Boolean
        If txtDepartmentName.Text.Length = 0 Then
            MsgBox("Please fill department name field.", MsgBoxStyle.Exclamation)
            txtDepartmentName.Focus()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Validate the program form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateProgram() As Boolean
        If txtProgramName.Text.Length = 0 Then
            MsgBox("Please fill program name field.", MsgBoxStyle.Exclamation)
            txtProgramName.Focus()
            Return False
        ElseIf txtProgramDescription.Text.Length = 0 Then
            MsgBox("Please fill program description field.", MsgBoxStyle.Exclamation)
            txtProgramDescription.Focus()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Clear button handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearDepartment_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ClearDeparmentForm()
    End Sub

    ''' <summary>
    ''' Clear button handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ClearProgramForm()
    End Sub

    ''' <summary>
    ''' Clear button handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearAccount_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ClearEmployeeForm()
    End Sub
End Class
