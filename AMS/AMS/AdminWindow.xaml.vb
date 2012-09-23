Imports MahApps.Metro.Controls

Public Class AdminWindow : Inherits MetroWindow

    Private adminWindowController As AdminWindowController = New AdminWindowController()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        comboboxCountry.ItemsSource = adminWindowController.GetAllCountryForLookUp()
        comboboxProgram.ItemsSource = adminWindowController.GetAllProgramForLookUp()
        comboboxStaff.ItemsSource = adminWindowController.GetAllEmployeeForLookUp()
        comboboxRole.ItemsSource = adminWindowController.GetAllRoleForLookUp()
    End Sub

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
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

    Private Sub btnSaveDepartment(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim department = New Department With {.DepartmentName = txtDepartmentName.Text}
        adminWindowController.CreateDepartment(Department)
    End Sub

    Private Sub btnSaveProgram(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim program = New Program With {
            .ProgramName = txtProgramName.Text,
            .ProgramDescription = txtProgramDescription.Text
        }
        adminWindowController.CreateProgram(program)
    End Sub

    Private Sub btnSaveAccount(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
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
        'assgin value to the account and employee
        adminWindowController.CreateAccount(employee)
    End Sub
End Class
