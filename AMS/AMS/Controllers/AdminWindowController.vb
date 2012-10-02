Public Class AdminWindowController

    Private amsEntities As AMSEntities = New AMSEntities()

    Public Sub CreateDepartment(ByRef department As Department)
        amsEntities.InsertDepartment(department.DepartmentName)
    End Sub

    Public Sub CreateProgram(ByRef program As Program)
        amsEntities.InsertProgram(program.ProgramName, program.ProgramDescription)
    End Sub

    Public Sub CreateAccount(ByRef employee As Employee)

        Dim encryptor As AMS.Utilities.EncryptionProvider = New AMS.Utilities.EncryptionProvider()

        amsEntities.InsertEmployee(employee.EmployeeFirstName, _
                                   employee.EmployeeLastName, _
                                   employee.Gender, employee.DateOfBirth, _
                                   employee.Address1, employee.Address2, _
                                   employee.City, _
                                   employee.PostCode, _
                                   employee.StateProvince, _
                                   employee.Country.CountryCode, _
                                   employee.ContactNumber, _
                                   employee.Email, _
                                   encryptor.Encrypt(employee.Account.LoginPassword), _
                                   employee.RoleID)
    End Sub

    Public Sub AssginProgramManager(ByRef program As Program)
        amsEntities.UpdateProgramManager(program.ProgramID, program.Employee.EmployeeID)
    End Sub

    Public Function GetAllProgramForLookUp() As IEnumerable(Of Program)
        Dim programList = From programs In amsEntities.Programs
                          Order By programs.CreatedDate
                          Select programs
        Return programList
    End Function

    Public Function GetAllEmployeeForLookUp() As IEnumerable(Of Employee)
        Dim employeeList = From employees In amsEntities.Employees _
                           Where employees.RoleCategory.RoleTitle = "Program Manager" _
                           Order By employees.EmployeeFirstName _
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
