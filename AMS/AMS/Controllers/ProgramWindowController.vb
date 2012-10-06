Public Class ProgramWindowController

    Private amsEnitities As AMSEntities = New AMSEntities()
    Private Const StaffRole As Integer = 3

    ''' <summary>
    ''' Update the program detail
    ''' </summary>
    ''' <param name="program"></param>
    ''' <remarks></remarks>
    Public Sub UpdateProgram(ByRef program As Program)
        amsEnitities.SaveChanges()
    End Sub

    ''' <summary>
    ''' Insert new coruse into the database
    ''' </summary>
    ''' <param name="course"></param>
    ''' <param name="program"></param>
    ''' <remarks></remarks>
    Public Sub CreateCourse(ByRef course As Course, ByRef program As Program)
        amsEnitities.InsertCourse(course.CourseName, course.CourseCode, course.CourseDescription, program.ProgramID)
    End Sub

    ''' <summary>
    ''' Assign staff to course
    ''' </summary>
    ''' <param name="staff"></param>
    ''' <param name="course"></param>
    ''' <remarks></remarks>
    Public Sub AssginStaffToCourse(ByRef staff As Employee, ByRef course As Course)
        course.Employee = staff
        amsEnitities.SaveChanges()
    End Sub

    ''' <summary>
    ''' Get program enrollment by program id
    ''' </summary>
    ''' <param name="programId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEnrollmentByProgram(ByVal programId As Integer) As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.ProgramID = programId
                          Select student
        Return studentList
    End Function

    ''' <summary>
    ''' Get course enrollment by course id
    ''' </summary>
    ''' <param name="courseId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEnrollmentByCourse(ByVal courseId As Integer) As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.CourseID = courseId
                          Select student
        Return studentList
    End Function

    ''' <summary>
    ''' Get employee by staff role id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpolyeeByStaffRole() As IEnumerable(Of Employee)
        Dim staffList = From staff In amsEnitities.Employees
                        Where staff.RoleID = 3
                        Select staff
        Return staffList
    End Function

    ''' <summary>
    ''' Get program by program manager user name
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProgramByUsername(ByVal username As String) As IEnumerable(Of Program)
        Dim programList = From program In amsEnitities.Programs
                          Where program.Employee.Account.LoginUsername = username
                          Select program
        Return programList
    End Function
End Class
