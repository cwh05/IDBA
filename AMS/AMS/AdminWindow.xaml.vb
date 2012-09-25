Imports MahApps.Metro.Controls

Public Class AdminWindow : Inherits MetroWindow

    Private adminWindowController As AdminWindowController = New AdminWindowController()

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
    End Sub

    Private Sub btnAssignProgramManagerClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        If listboxProgram.SelectedItem IsNot Nothing And listboxStaff.SelectedItem IsNot Nothing Then
            Dim program = CType(listboxProgram.SelectedItem, Program)
            program.Employee = CType(listboxStaff.SelectedItem, Employee)
            adminWindowController.AssginProgramManager(program)
            RefreshLookItem()
        Else
            MsgBox("Please select both staff and program before assign.")
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
End Class
