Public Class AdminWindowController

    Private amsEntities As AMSEntities = New AMSEntities()

    Public Sub CreateDepartment(ByRef department As Department)
        amsEntities.AddToDepartments(department)
        amsEntities.SaveChanges()
    End Sub

    Public Sub CreateProgram(ByRef program As Program)
        amsEntities.AddToPrograms(program)
        amsEntities.SaveChanges()
    End Sub

    Public Sub CreateAccount(ByRef account As Account, ByRef employee As Employee)
        amsEntities.AddToEmployees(employee)
        amsEntities.AddToAccounts(account)
        employee.Account = account
        amsEntities.SaveChanges()
    End Sub

    Public Sub AssginProgramManage(ByRef program As Program, ByRef employee As Employee)
        program.Employee = employee
        amsEntities.SaveChanges()
    End Sub

    Public Function GetAllProgramForLookUp() As IEnumerable(Of Program)
        Dim programList = From programs In amsEntities.Programs
                          Select programs
        Return programList
    End Function

    Public Function GetAllEmployeeForLookUp() As IEnumerable(Of Employee)
        Dim employeeList = From employees In amsEntities.Employees
                           Select employees
        Return employeeList
    End Function
End Class
