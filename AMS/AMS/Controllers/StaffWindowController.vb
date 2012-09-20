Public Class StaffWindowController
    Private db As New AMSEntities()

    ''' <summary>
    ''' This function retrives all courses data
    ''' </summary>
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
End Class
