Public Class ProgramWindowController

    Private amsEnitities As AMSEntities = New AMSEntities()

    Public Sub UpdateProgram(ByRef program As Program)
        amsEnitities.SaveChanges()
    End Sub

    Public Function GetEnrollmentByProgram() As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.ProgramID = 1
                          Select student
        Return studentList
    End Function

    Public Function GetEnrollmentByCourse(ByVal courseId As Integer) As IEnumerable(Of Enrollment)
        Dim studentList = From student In amsEnitities.Enrollments
                          Where student.CourseID = courseId
                          Select student
        Return studentList
    End Function

    Public Function GetCourseForLookup() As IEnumerable(Of Course)
        Dim courseList = From course In amsEnitities.Courses
                         Select course
        Return courseList
    End Function
End Class
