Public Class AdminWindowController

    Private amsEntities As AMSEntities = New AMSEntities()

    Public Sub CreateDepartment(ByRef department As Department)
        amsEntities.AddToDepartments(department)
        amsEntities.SaveChanges()
    End Sub

    Public Sub CreateProgram(ByRef program As Program)
        program.ProgramID = amsEntities.Programs.Count + 99
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
                          Order By programs.CreatedDate
                          Select programs
        Return programList
    End Function

    Public Function GetAllEmployeeForLookUp() As IEnumerable(Of Employee)
        Dim employeeList = From employees In amsEntities.Employees
                           Order By employees.EmployeeFirstName
                           Select employees
        Return employeeList
    End Function

    Public Function GetAllCountryForLookUp() As IEnumerable(Of Country)
        Dim countryList = From countries In amsEntities.Countries
                          Order By countries.CountryTitle
                          Select countries
        Return countryList
    End Function

    Public Function GetAllRoleForLookUp() As IEnumerable(Of RoleCategory)
        Return amsEntities.RoleCategories
    End Function

    Public Function GetEmployeeCount() As Integer
        Return amsEntities.Employees.Count
    End Function
End Class
