Public Class AdminWindowController

    Private amsEntities As AMSEntities = New AMSEntities()

    ''' <summary>
    ''' Insert department into the database
    ''' </summary>
    ''' <param name="department"></param>
    ''' <remarks></remarks>
    Public Sub CreateDepartment(ByRef department As Department)
        amsEntities.InsertDepartment(department.DepartmentName)
    End Sub

    ''' <summary>
    ''' Insert prgram into the database
    ''' </summary>
    ''' <param name="program"></param>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Assign program manager to program
    ''' </summary>
    ''' <param name="program"></param>
    ''' <remarks></remarks>
    Public Sub AssginProgramManager(ByRef program As Program)
        amsEntities.UpdateProgramManager(program.ProgramID, program.Employee.EmployeeID)
    End Sub

    ''' <summary>
    ''' Get all department from database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllDepartmentForLookUp() As IEnumerable(Of Department)
        Dim departmentList = From department In amsEntities.Departments
                             Order By department.CreatedDate
                             Select department
        Return departmentList
    End Function

    ''' <summary>
    ''' Get all program from databse
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllProgramForLookUp() As IEnumerable(Of Program)
        Dim programList = From programs In amsEntities.Programs
                          Order By programs.CreatedDate
                          Select programs
        Return programList
    End Function

    ''' <summary>
    ''' Get all program manager from database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllEmployeeForLookUp() As IEnumerable(Of Employee)
        Dim employeeList = From employees In amsEntities.Employees _
                           Where employees.RoleCategory.RoleTitle = "Program Manager" _
                           Order By employees.EmployeeFirstName _
                           Select employees
        Return employeeList
    End Function

    ''' <summary>
    ''' Get all country form database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllCountryForLookUp() As IEnumerable(Of Country)
        Dim countryList = From countries In amsEntities.Countries
                          Order By countries.CountryTitle
                          Select countries
        Return countryList
    End Function

    ''' <summary>
    ''' Get all role form database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllRoleForLookUp() As IEnumerable(Of RoleCategory)
        Return amsEntities.RoleCategories
    End Function

    ''' <summary>
    ''' Get latest username form database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLatestUsername() As String
        Dim username As GetLatestLoginUsername_Result = CType(amsEntities.GetLatestLoginUsername().Single(), GetLatestLoginUsername_Result)
        Return username.loginusername
    End Function
End Class
