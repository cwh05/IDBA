Public Class ProgramWindow

    Private programWindowController As ProgramWindowController = New ProgramWindowController()

    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        txtCourseName.IsReadOnly = True
        txtCourseCode.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False

        ' Add any initialization after the InitializeComponent() call.
        Dim programList = programWindowController.GetProgramByUsername(username)

        comboboxProgram.ItemsSource = programList
        comboboxProgramStaff.ItemsSource = programList
        comboboxProgramCourse.ItemsSource = programList
        comboboxProgramEnrollment.ItemsSource = programList

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

    Private Sub btnSaveProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If ValidateProgram() Then
                Dim program = New Program With {
                    .ProgramName = txtProgramName.Text,
                    .ProgramDescription = txtProgramDescription.Text
                }
                programWindowController.UpdateProgram(program)
                ClearProgramForm()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub btnSaveCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
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

    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub

    Public Sub ClearCourseForm()
        txtCourseName.Text = String.Empty
        txtCourseCode.Text = String.Empty
        txtCourseDescription.Text = String.Empty
    End Sub

    Private Sub comboboxCourse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxCourse.SelectedItem IsNot Nothing Then
            datagridCourse.ItemsSource = programWindowController.GetEnrollmentByCourse(CType(comboboxCourse.SelectedItem, Course).CourseID)
        End If
    End Sub

    Private Sub comboboxProgram_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim program = CType(comboboxProgram.SelectedItem, Program)
        txtProgramName.Text = program.ProgramName
        txtProgramDescription.Text = program.ProgramDescription
        listboxCourseProgram.ItemsSource = program.Courses
    End Sub

    Private Sub btnAddCoures_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        txtCourseName.IsReadOnly = False
        txtCourseCode.IsReadOnly = False
        txtCourseDescription.IsReadOnly = False
        btnClearCourse.IsEnabled = True
        btnSaveCourse.IsEnabled = True
    End Sub

    Private Sub btnClearCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        txtCourseName.IsReadOnly = True
        txtCourseCode.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False
        ClearCourseForm()
    End Sub

    Private Sub btnClearProgram_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ClearProgramForm()
    End Sub

    Private Sub comboboxProgramEnrollment_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim program = CType(comboboxProgramEnrollment.SelectedItem, Program)
        datagridProgram.ItemsSource = programWindowController.GetEnrollmentByProgram(program.ProgramID)
    End Sub

    Private Sub comboboxProgramCourse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim program = CType(comboboxProgramCourse.SelectedItem, Program)
        comboboxCourse.ItemsSource = program.Courses
        comboboxCourse.SelectedIndex = 0
    End Sub

    Private Sub btnAssignStaffToCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If listboxStaff.SelectedItem IsNot Nothing And listboxCourse.SelectedItem IsNot Nothing Then
                Dim staff = CType(listboxStaff.SelectedItem, Employee)
                Dim course = CType(listboxCourse.SelectedItem, Course)
                programWindowController.AssginStaffToCourse(staff, course)

                Dim program = CType(comboboxProgramStaff.SelectedItem, Program)
                program.Courses.Load()

                MsgBox("Course " & program.ProgramName & " managed by " & program.Employee.EmployeeFirstName, MsgBoxStyle.Information)
            Else
                MsgBox("Please select both staff and course before assign.")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub comboboxProgramStaff_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        If comboboxProgramStaff.SelectedItem IsNot Nothing Then
            Dim program = CType(comboboxProgramStaff.SelectedItem, Program)
            listboxCourse.ItemsSource = program.Courses
        End If
    End Sub

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
End Class
