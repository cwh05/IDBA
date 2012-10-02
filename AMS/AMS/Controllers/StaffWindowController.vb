Imports System.Transactions
Imports System.Data.Objects

Public Class StaffWindowController

    Private db As New AMSEntities()

    Public Function GetAllCourseByStaff(ByRef staffID As Int32) As List(Of Course)
        Try
            Dim list As New List(Of Course)
            For Each i In db.GetAllCourseOfStaff(staffID)
                Dim course As New Course

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
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try
        Return Nothing
    End Function

    Public Function GetStudentEnrollmentByCourseID(ByRef courseID As Int32) As List(Of Enrollment)
        Try
            Dim enrollmentList As New List(Of Enrollment)
            For Each i In db.GetStudentEnrollmentByCourseID(courseID)
                Dim enrollment As New Enrollment

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
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try

        Return Nothing
    End Function

    Public Function UpdateCourse(ByRef CourseCode As String, ByRef CourseName As String, ByRef CourseDescription As String,
                                ByRef CourseID As Integer) As Boolean

        Try

            db.UpdateCourse(CourseID, CourseName, CourseCode, CourseDescription)
            db.SaveChanges()

            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try

        Return False
    End Function

    Public Function UpdateStudentMark(ByVal courseid As Integer, ByVal studentid As Integer, ByRef marks As Double) As Boolean

        Try

            For Each f As String In db.UpdateStudentMarks(courseid, studentid, marks)
                db.SaveChanges()
                marks = CDbl(f)
                Return True
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try

        Return False
    End Function

    Public Function InsertStudent(ByVal student As Student, ByVal account As Account) As Boolean

        Try
            db.InsertStudent(account.LoginPassword, student.StudentFirstName, student.StudentLastName, student.Gender, student.DateOfBirth, student.Address1, student.Address2, student.City, _
                             student.PostCode, student.StateProvince, student.CountryCode, student.ContactNumber, student.Email, student.ProgramID)

            db.SaveChanges()
            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Exception")
        End Try


        Return False
    End Function

    Public Function GetAllCountryForLookUp() As IEnumerable(Of Country)
        Dim countryList = From countries In db.Countries
                          Order By countries.CountryTitle
                          Select countries
        Return countryList
    End Function

    Public Function GetAllProgramForLookUp() As IEnumerable(Of Program)
        Dim programList = From programs In db.Programs
                          Order By programs.ProgramID
                          Select programs
        Return programList
    End Function

    Public Function GeNewStudentID() As Integer
        Dim lastid = (From students In db.Students
                          Select students.StudentID).Max()
        If IsNothing(lastid) Then
            lastid = 0
        End If
        Return lastid + 1
    End Function
End Class
