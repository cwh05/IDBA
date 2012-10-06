Public Class ProgramWindow

    Private programWindowController As ProgramWindowController = New ProgramWindowController()
    Private programManager As String

    ''' <summary>
    ''' Constructor 
    ''' </summary>
    ''' <param name="username">Username</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        txtCourseName.IsReadOnly = True
        txtCourseCode.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False

        programManager = username

        ' Add any initialization after the InitializeComponent() call.
        Dim programList = programWindowController.GetProgramByUsername(username)

        'Refresh the combobox that binding with program list
        comboboxProgram.ItemsSource = programList
        comboboxProgramStaff.ItemsSource = programList
        comboboxProgramCourse.ItemsSource = programList
        comboboxProgramEnrollment.ItemsSource = programList

        'Set the defualt value for combobox
        comboboxProgram.SelectedIndex = 0
        comboboxProgramStaff.SelectedIndex = 0
        comboboxProgramCourse.SelectedIndex = 0
        comboboxProgramEnrollment.SelectedIndex = 0

        Dim program = CType(comboboxProgram.SelectedItem, Program)
        If program IsNot Nothing Then
            listboxCourseProgram.ItemsSource = program.Courses
            listboxCourse.ItemsSource = program.Courses
        End If

        listboxStaff.ItemsSource = programWindowController.GetEmpolyeeByStaffRole()
    End Sub

    Private Sub MetroWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'set weather temperature
        If CType(Application.Current, Application).WeatherTemperature <> String.Empty Then
            lblTemperatureContent.Content = CType(Application.Current, Application).WeatherTemperature
            lblTemperatureContent.ToolTip = "Relative Humidity: " & CType(Application.Current, Application).WeatherRelativeHumidity &
                    vbNewLine & "Wind: " & CType(Application.Current, Application).WeatherWind

        End If
    End Sub

    ''' <summary>
    ''' Swicthing for tab on program windows
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked As Button = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnProgramInfo"
                tabContent.SelectedIndex = 1
            Case "btnAssignCourse"
                tabContent.SelectedIndex = 2
            Case "btnViewProgramEnrolment"
                tabContent.SelectedIndex = 3
            Case "btnViewCourseEnrolment"
                tabContent.SelectedIndex = 4
        End Select
    End Sub

    ''' <summary>
    ''' Event handler for program save button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Validating program fields
            If ValidateProgram() Then
                Dim program = CType(comboboxProgram.SelectedItem, Program)
                program.ProgramName = txtProgramName.Text
                program.ProgramDescription = txtProgramDescription.Text

                'Update the program detial to database
                programWindowController.UpdateProgram(program)

                MsgBox("Program detail has been updated.", MsgBoxStyle.Information)

                'Refresh the combobox that binding with program list
                Dim programList = programWindowController.GetProgramByUsername(programManager)
                comboboxProgram.ItemsSource = programList
                comboboxProgramStaff.ItemsSource = programList
                comboboxProgramCourse.ItemsSource = programList
                comboboxProgramEnrollment.ItemsSource = programList
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for course save button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Validating course fields
            If ValidateCourse() Then
                Dim course = New Course With {
                    .CourseName = txtCourseName.Text,
                    .CourseCode = txtCourseCode.Text,
                    .CourseDescription = txtCourseDescription.Text
                }

                Dim program = CType(comboboxProgram.SelectedItem, Program)

                If program IsNot Nothing Then
                    programWindowController.CreateCourse(course, program)
                    program.Courses.Load()
                Else
                    MsgBox("Program is not selected, please select program.")
                End If

                ClearCourseForm()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Clear program form
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Clear course form
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearCourseForm()
        txtCourseName.Text = String.Empty
        txtCourseCode.Text = String.Empty
        txtCourseDescription.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Update information in the data grid following by the selected course
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub comboboxCourse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxCourse.SelectedItem IsNot Nothing Then
            datagridCourse.ItemsSource = programWindowController.GetEnrollmentByCourse(CType(comboboxCourse.SelectedItem, Course).CourseID)
        End If
    End Sub

    ''' <summary>
    ''' Update the program fields when selected program change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub comboboxProgram_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxProgram.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgram.SelectedItem, Program)
            txtProgramName.Text = program.ProgramName
            txtProgramDescription.Text = program.ProgramDescription
            listboxCourseProgram.ItemsSource = program.Courses
        End If
    End Sub

    ''' <summary>
    ''' Add course button handlers
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddCoures_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'Enable textbox on the form
        txtCourseName.IsReadOnly = False
        txtCourseCode.IsReadOnly = False
        txtCourseDescription.IsReadOnly = False
        btnClearCourse.IsEnabled = True
        btnSaveCourse.IsEnabled = True
    End Sub

    ''' <summary>
    ''' Clear course button handlers
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'Disable textbox on the form
        txtCourseName.IsReadOnly = True
        txtCourseCode.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False
        ClearCourseForm()
    End Sub

    ''' <summary>
    ''' Clear program form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ClearProgramForm()
    End Sub

    ''' <summary>
    ''' Update information in the data grid following by the selected program
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub comboboxProgramEnrollment_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxProgramEnrollment.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgramEnrollment.SelectedItem, Program)
            datagridProgram.ItemsSource = programWindowController.GetEnrollmentByProgram(program.ProgramID)
        End If
    End Sub

    ''' <summary>
    ''' Update information in course combobox following by the selected program
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub comboboxProgramCourse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxProgramCourse.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgramCourse.SelectedItem, Program)
            comboboxCourse.ItemsSource = Program.Courses
            comboboxCourse.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Event handler for assign button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAssignStaffToCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            'Check both lists have selected 
            If listboxStaff.SelectedItem IsNot Nothing And listboxCourse.SelectedItem IsNot Nothing Then
                Dim staff = CType(listboxStaff.SelectedItem, Employee)
                Dim course = CType(listboxCourse.SelectedItem, Course)

                'Assign course to staff
                programWindowController.AssginStaffToCourse(staff, course)

                'Get the selected program and reload the course information from database
                Dim program = CType(comboboxProgramStaff.SelectedItem, Program)
                program.Courses.Load()

                MsgBox("Course " & course.CourseName & " managed by " & course.Employee.EmployeeFirstName, MsgBoxStyle.Information)
            Else
                MsgBox("Please select both staff and course before assign.")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Update course list when selected program change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub comboboxProgramStaff_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxProgramStaff.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgramStaff.SelectedItem, Program)
            listboxCourse.ItemsSource = program.Courses
        End If
    End Sub

    ''' <summary>
    ''' Validate the program form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateProgram() As Boolean
        If txtProgramName.Text.Length = 0 Then
            MsgBox("Please fill program name field.", MsgBoxStyle.Exclamation)
            txtProgramName.Focus()
            Return False
        ElseIf txtProgramDescription.Text.Length = 0 Then
            MsgBox("Please fill program description field.", MsgBoxStyle.Exclamation)
            txtProgramDescription.Focus()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Validate the coruse form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateCourse() As Boolean
        If txtCourseName.Text.Length = 0 Then
            MsgBox("Please fill course name field.", MsgBoxStyle.Exclamation)
            txtCourseName.Focus()
            Return False
        ElseIf txtCourseCode.Text.Length = 0 Then
            MsgBox("Please fill course code field.", MsgBoxStyle.Exclamation)
            txtCourseCode.Focus()
            Return False
        ElseIf txtCourseDescription.Text.Length = 0 Then
            MsgBox("Please fill course description field.", MsgBoxStyle.Exclamation)
            txtCourseDescription.Focus()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Updatet refesh information when selected index tab change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tabContent_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles tabContent.SelectionChanged
        If comboboxProgram.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgram.SelectedItem, Program)
            txtProgramName.Text = program.ProgramName
            txtProgramDescription.Text = program.ProgramDescription
            listboxCourseProgram.ItemsSource = program.Courses
        End If
    End Sub
End Class
