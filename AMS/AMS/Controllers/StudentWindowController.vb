Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Objects
Imports System.Data.EntityClient
Imports System.Transactions

Public Class StudentWindowController
    Private db As AMSEntities = New AMSEntities()

    ''' <summary>
    ''' This function validates student personal details
    ''' </summary>
    ''' <param name="FirstName">A first name</param>
    ''' <param name="LastName">A last name</param>
    ''' <param name="dob">A date of birth</param>
    ''' <param name="ContactNumber">A contact number</param>
    ''' <param name="Email">An E-mail</param>
    ''' <param name="maleOption">A boolean of male selected</param>
    ''' <param name="femaleOption">A boolean of female selected</param>
    ''' <returns>True is no error occur or False if it has error</returns>
    ''' <remarks></remarks>
    Public Function validatePersonalDetail(ByRef FirstName As String, ByRef LastName As String, ByRef dob As String,
                                           ByRef ContactNumber As String, ByRef Email As String, ByRef maleOption As Boolean,
                                           ByRef femaleOption As Boolean) As Boolean
        Dim numberRE As New Regex("\d+")
        Dim emailRE As New Regex("^\w+([+-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")

        'validate all textboxes content
        If FirstName.Length = 0 Or LastName.Length = 0 Or dob.Length = 0 Or
            numberRE.IsMatch(ContactNumber) = False Or emailRE.IsMatch(Email) = False Then
            Return False

            'validate gender option
        ElseIf maleOption = False And femaleOption = False Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' This function validates student address detail
    ''' </summary>
    ''' <param name="Address1">An address 1</param>
    ''' <param name="Address2">An address 2</param>
    ''' <param name="City">A city</param>
    ''' <param name="PostCode">A postcode</param>
    ''' <param name="State">A state province</param>
    ''' <param name="countryCombo">A country string</param>
    ''' <returns>True is no error occur or False if it has error</returns>
    ''' <remarks></remarks>
    Public Function validateAddressDetail(ByRef Address1 As String, ByRef Address2 As String, ByRef City As String,
                                          ByRef PostCode As String, ByRef State As String, ByRef countryCombo As String) As Boolean
        'validate address details of a student
        If Address1.Length = 0 Or Address2.Length = 0 Or City.Length = 0 Or
            PostCode.Length = 0 Or State.Length = 0 Or countryCombo.Length = 0 Then
            Return False
        End If

        Return True
    End Function


    ''' <summary>
    ''' This function retrieves all country data
    ''' </summary>
    ''' <returns>A list of country</returns>
    ''' <remarks></remarks>
    Public Function GetAllCountry() As IEnumerable
        Try
            'access to get all country data
            Return db.GetAllCountry()

        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' This function retrieves student profile details
    ''' </summary>
    ''' <param name="str">A student username</param>
    ''' <returns>A student object that consists of a student profile data</returns>
    ''' <remarks></remarks>
    Public Function GetStudentDetails(ByRef str As String) As Student
        Try
            For Each i In db.GetUserPersonalDetail(str)
                Dim stud As New Student

                'assign values to student obj
                stud.Address1 = i.Address1
                stud.Address2 = i.Address2
                stud.City = i.City
                stud.ContactNumber = i.ContactNumber
                stud.CountryCode = i.CountryCode
                stud.DateOfBirth = i.DateOfBirth
                stud.Email = i.Email
                stud.PostCode = i.PostCode
                stud.StudentFirstName = i.StudentFirstName
                stud.StudentLastName = i.StudentLastName
                stud.StateProvince = i.StateProvince
                stud.Gender = i.Gender

                Return stud
            Next
        Catch ex As Exception
        End Try
        Return Nothing
    End Function


    ''' <summary>
    ''' This function updates student profile details, address details and password
    ''' </summary>
    ''' <param name="FirstName">A first name string</param>
    ''' <param name="LastName">A last name string</param>
    ''' <param name="Gender">A gender bit</param>
    ''' <param name="DOB">A date of birth</param>
    ''' <param name="Address1">An address 1 string</param>
    ''' <param name="Address2">An address 2 string</param>
    ''' <param name="City">A city</param>
    ''' <param name="PostCode">A postcode string</param>
    ''' <param name="StateProvince">A state province string</param>
    ''' <param name="CountryCode">A country code</param>
    ''' <param name="ContactNumber">A string of contact number</param>
    ''' <param name="Email">An email address</param>
    ''' <param name="StudentID">A student ID</param>
    ''' <param name="StudentUsername">A student username</param>
    ''' <param name="OldPassword">A student's old password</param>
    ''' <param name="NewPassword">A student's new password</param>
    ''' <returns>True if updated successfully or false if it is failed</returns>
    ''' <remarks></remarks>
    Public Function UpdateProfile(ByRef FirstName As String, ByRef LastName As String, ByRef Gender As Boolean,
                                ByRef DOB As Date, ByRef Address1 As String, ByRef Address2 As String,
                                ByRef City As String, ByRef PostCode As String, ByRef StateProvince As String,
                                ByRef CountryCode As String, ByRef ContactNumber As String,
                                ByRef Email As String, ByRef StudentID As Int32, ByRef StudentUsername As String,
                                ByRef OldPassword As String, ByRef NewPassword As String) As Boolean
        'update 2 statements in a transaction
        Using transaction As New TransactionScope()
            Try
                'update personal detail
                db.UpdateStudentProfile(FirstName, LastName, Gender, DOB, Address1, Address2, City, PostCode,
                                        StateProvince, CountryCode, ContactNumber, Email, StudentID)

                If Not String.IsNullOrEmpty(StudentUsername) And Not String.IsNullOrEmpty(OldPassword) And
                    Not String.IsNullOrEmpty(NewPassword) Then

                    'update address detail
                    db.UpdateUserPassword(StudentUsername, OldPassword, NewPassword)

                End If

                db.SaveChanges()
                transaction.Complete()
                Return True

            Catch ex As Exception
            End Try
        End Using

        Return False
    End Function


    ''' <summary>
    ''' This function retrives all courses data
    ''' </summary>
    ''' <returns>A list of all course objects</returns>
    ''' <remarks></remarks>
    Public Function GetAllCourseOfProgram(ByRef studentID As Int32) As List(Of Course)
        Try
            Dim list As New List(Of Course)
            For Each i In db.GetAllCourseOfProgram(studentID)
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
    ''' This function retrieves a student enrollment records
    ''' </summary>
    ''' <param name="studentID">A student ID</param>
    ''' <returns>A list of enrollment courses</returns>
    ''' <remarks></remarks>
    Public Function GetStudentEnrollment(ByRef studentID As Int32) As List(Of Enrollment)
        Try
            Dim enrollmentList As New List(Of Enrollment)
            For Each i In db.GetStudentEnrollment(studentID)
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
    ''' This function enrols a course for student
    ''' </summary>
    ''' <param name="StudentID">A student ID</param>
    ''' <param name="CourseID">A course ID</param>
    ''' <returns>True if it enrols successfully or false if it fails</returns>
    ''' <remarks></remarks>
    Public Function EnrolStudentCourse(ByRef StudentID As Int32, ByRef CourseID As Int32) As Boolean
        Try
            'update to particular entity table
            db.EnrolStudentCourse(StudentID, CourseID)
            db.SaveChanges()

            Return True

        Catch ex As Exception
        End Try

        Return False
    End Function

    ''' <summary>
    ''' This function removes an enrolled course for student
    ''' </summary>
    ''' <param name="StudentID">A student ID</param>
    ''' <param name="CourseID">A course ID</param>
    ''' <returns>True if it removes a course successfully or false if it fails to remove</returns>
    ''' <remarks></remarks>
    Public Function RemoveStudentCourse(ByRef StudentID As Int32, ByRef CourseID As Int32) As Boolean
        Try
            'delete course from view entity
            db.RemoveStudentCourse(StudentID, CourseID)
            db.SaveChanges()

            Return True

        Catch ex As Exception
        End Try

        Return False
    End Function


End Class
