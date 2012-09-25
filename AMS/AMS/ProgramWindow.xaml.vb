Public Class ProgramWindow

    Private programWindowController As ProgramWindowController = New ProgramWindowController()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = programWindowController.GetEnrollmentByProgram()
        comboboxCourse.ItemsSource = programWindowController.GetCourseForLookup()
    End Sub

    Public Sub New(ByRef username As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = programWindowController.GetEnrollmentByProgram()
    End Sub

    Private Sub btnMenu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim btnClicked As Button = CType(sender, Button)
        Select Case btnClicked.Name
            Case "btnProgramInfo"
                tabContent.SelectedIndex = 0
            Case "btnAssignCourse"
                tabContent.SelectedIndex = 1
            Case "btnViewProgramEnrolment"
                tabContent.SelectedIndex = 2
            Case "btnViewCourseEnrolment"
                tabContent.SelectedIndex = 3
        End Select
    End Sub

    Private Sub btnSaveProgramClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim program = New Program With {
            .ProgramName = txtProgramName.Text,
            .ProgramDescription = txtProgramDescription.Text
        }
        programWindowController.UpdateProgram(program)
        ClearProgramForm()
    End Sub

    Public Sub ClearProgramForm()
        txtProgramName.Text = String.Empty
        txtProgramDescription.Text = String.Empty
    End Sub
End Class
