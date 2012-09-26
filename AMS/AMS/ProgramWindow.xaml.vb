Public Class ProgramWindow

    Private programWindowController As ProgramWindowController = New ProgramWindowController()
    Private programModelContainer As ProgramWindowModel = New ProgramWindowModel()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        txtCourseName.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False

        ' Add any initialization after the InitializeComponent() call.
        comboboxProgram.ItemsSource = programWindowController.GetProgramByUsername("e1")
        comboboxProgram.SelectedIndex = 0

        Dim program = CType(comboboxProgram.SelectedItem, Program)
        listboxCourseInProgram.ItemsSource = program.Courses

        listboxStaff.ItemsSource = programWindowController.GetEmpolyeeByStaffRole()
        comboboxCourse.ItemsSource = programWindowController.GetCourseForLookup()
        comboboxCourse.SelectedIndex = 0

        datagridCourse.ItemsSource = programWindowController.GetEnrollmentByCourse(CType(comboboxCourse.SelectedItem, Course).CourseID)
        datagridProgram.ItemsSource = programWindowController.GetEnrollmentByProgram()
    End Sub

    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        comboboxCourse.ItemsSource = programWindowController.GetCourseForLookup()
        comboboxCourse.SelectedIndex = 0

        datagridCourse.ItemsSource = programWindowController.GetEnrollmentByCourse(CType(comboboxCourse.SelectedItem, Course).CourseID)
        datagridProgram.ItemsSource = programWindowController.GetEnrollmentByProgram()
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
        Dim program = New Program With {
            .ProgramName = txtProgramName.Text,
            .ProgramDescription = txtProgramDescription.Text
        }
        programWindowController.UpdateProgram(program)
        ClearProgramForm()
    End Sub

    Private Sub btnSaveCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim course = New Course With {
            .CourseName = txtCourseName.Text,
            .CourseCode = txtCourseCode.Text,
            .CourseDescription = txtCourseDescription.Text
        }

        Dim program = CType(comboboxProgram.SelectedItem, Program)

        programWindowController.CreateCourse(course, program)
        program.Courses.Load()
        listboxCourseInProgram.ItemsSource = program.Courses
    End Sub

    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub

    Public Sub ClearCourseForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
        txtCourseName.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False
    End Sub

    Private Sub comboboxCourse_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        datagridCourse.ItemsSource = programWindowController.GetEnrollmentByCourse(CType(comboboxCourse.SelectedItem, Course).CourseID)
        datagridProgram.ItemsSource = programWindowController.GetEnrollmentByProgram()
    End Sub

    Private Sub comboboxProgram_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim selectedProgram = CType(comboboxProgram.SelectedItem, Program)
        txtProgramName.Text = selectedProgram.ProgramName
        txtProgramDescription.Text = selectedProgram.ProgramDescription
    End Sub

    Private Sub btnAddCoures_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        txtCourseName.IsReadOnly = False
        txtCourseDescription.IsReadOnly = False
        btnClearCourse.IsEnabled = True
        btnSaveCourse.IsEnabled = True
    End Sub

    Private Sub btnClearCourse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        txtCourseName.IsReadOnly = True
        txtCourseDescription.IsReadOnly = True
        btnClearCourse.IsEnabled = False
        btnSaveCourse.IsEnabled = False
    End Sub
End Class

Public Class ProgramWindowModel
    Public Property EnrollmentByProgram() As IEnumerable(Of Enrollment)
    Public Property EnrollmentByCourse() As IEnumerable(Of Enrollment)
End Class
