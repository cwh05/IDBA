Imports MahApps.Metro.Controls

Public Class AdminWindow : Inherits MetroWindow

    Private adminWindowController As AdminWindowController = New AdminWindowController()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

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
        Dim department = New Department()
        department.DepartmentName = txtDepartmentName.Text
        adminWindowController.CreateDepartment(department)
    End Sub

    Private Sub btnSaveProgram(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim program = New Program()
        program.ProgramName = txtProgramName.Text
        program.ProgramDescription = ""
        adminWindowController.CreateProgram(program)
    End Sub

    Private Sub btnSaveAccount(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim account = New Account()
        Dim employee = New Employee()
        'assgin value to the account and employee
        adminWindowController.CreateAccount(account, employee)
    End Sub
End Class
