Public Class ProgramWindowController

    Private amsEnitities As AMSEntities = New AMSEntities()
    Private Const StaffRole As Integer = 3

    Public Sub UpdateProgram(ByRef program As Program)
        amsEnitities.SaveChanges()
    End Sub

    Public Sub CreateCourse(ByRef course As Course, ByRef program As Program)
        amsEnitities.InsertCourse(course.CourseName, course.CourseCode, course.CourseDescription, program.ProgramID)
    End Sub

    Public Sub AssginStaffToCourse(ByRef staff As Employee, ByRef course As Course)
        course.Employee = staff
        amsEnitities.SaveChanges()
    End Sub

    Public Function GetEnrollmentByProgram(ByVal programId As Integer) As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.ProgramID = programId
                          Select student
        Return studentList
    End Function

    Public Function GetEnrollmentByCourse(ByVal courseId As Integer) As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.CourseID = courseId
                          Select student
        Return studentList
    End Function

    Public Function GetEmpolyeeByStaffRole() As IEnumerable(Of Employee)
        Dim staffList = From staff In amsEnitities.Employees
                        Where staff.RoleID = 3
                        Select staff
        Return staffList
    End Function

    Public Function GetProgramByUsername(ByVal username As String) As IEnumerable(Of Program)
        Dim programList = From program In amsEnitities.Programs
                          Where program.Employee.Account.LoginUsername = username
                          Select program
        Return programList
    End Function
End Class
