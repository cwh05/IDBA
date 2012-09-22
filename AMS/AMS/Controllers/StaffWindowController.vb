Public Class StaffWindowController
    Private db As New AMSEntities()

    ''' <summary>
    ''' This function retrives all courses data by staffID
    ''' </summary>
    ''' <param name="staffID">A staff ID</param>
    ''' <returns>A list of all course objects</returns>
    ''' <remarks></remarks>
    Public Function GetAllCourseOfStaff(ByRef staffID As Int32) As List(Of Course)
        Try
            Dim list As New List(Of Course)
            For Each i In db.GetAllCourseOfStaff(staffID)
                Dim course As New Course

                'assign values to object
                course.CourseID = i.CourseID
                course.CourseName = i.CourseName
                course.CourseCode = i.CourseCode
                course.StaffID = i.StaffID
                course.CreatedDate = i.CreatedDate
                course.ModifiedDate = i.ModifiedDate
                course.CourseDescription = i.CourseDescription

                list.Add(course)
            Next
            Return list

        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' This function retrieves a student enrollment records by Course taught by the logged in lecturer
    ''' </summary>
    ''' <param name="courseID">A course ID</param>
    ''' <returns>A list of enrollment courses</returns>
    ''' <remarks></remarks>
    Public Function GetStudentEnrollmentByCourseID(ByRef courseID As Int32) As List(Of Enrollment)
        Try
            Dim enrollmentList As New List(Of Enrollment)
            For Each i In db.GetStudentEnrollmentByCourseID(courseID)
                Dim enrollment As New Enrollment

                'assign values to enrollment object
                enrollment.StudentID = i.StudentID
                enrollment.StudentFirstName = i.StudentFirstName
                enrollment.StudentLastName = i.StudentLastName
                enrollment.Email = i.Email
                enrollment.CourseID = i.CourseID
                enrollment.CourseName = i.CourseName
                enrollment.CourseCode = i.CourseCode
                enrollment.Marks = i.Marks

                enrollmentList.Add(enrollment)
            Next
            Return enrollmentList

        Catch ex As Exception
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' This function updates course details
    ''' </summary>
    ''' <param name="CourseCode">A course code string</param>
    ''' <param name="CourseName">A course name string</param>
    ''' <param name="CourseDescription">A course description string</param>
    ''' <param name="CourseID">A course id</param>
    ''' <returns>True if updated successfully or false if it is failed</returns>
    ''' <remarks></remarks>
    Public Function UpdateCourse(ByRef CourseCode As String, ByRef CourseName As String, ByRef CourseDescription As String,
                                ByRef CourseID As Integer) As Boolean
        'update 2 statements in a transaction

        Try

            db.UpdateCourse(CourseID, CourseName, CourseCode, CourseDescription)
            db.SaveChanges()

            Return True

        Catch ex As Exception
        End Try

        Return False
    End Function
End Class
