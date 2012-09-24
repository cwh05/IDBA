Imports MahApps.Metro.Controls
Imports System.Activities
Imports System.IO



Class LoginWindow : Inherits MetroWindow
    Private controller As LoginWindowContoller = New LoginWindowContoller()
    Private inputs As New Dictionary(Of String, Object)
    Private writer As StringWriter = New StringWriter()
    Private wf As WorkflowInvoker

    Private Sub btnLogin_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        'validate input textboxes
        'If txtUsername.Text.Length = 0 And txtPassword.Password.Length = 0 Then
        '    FailToLogin()

        'Else

        'Dim dll As New DLLResource.EncryptionProvider()

        ''pass arguments to workflow
        'inputs.Add("inputUsername", txtUsername.Text)
        'inputs.Add("inputPassword", dll.Encrypt(txtPassword.Password))

        'Try
        '    'call workflow
        '    wf = New WorkflowInvoker(New LoginActivity())
        '    wf.Extensions.Add(writer)
        '    wf.Invoke(inputs)

        '    'if workflow fails to authenticate
        '    If writer.ToString.Length > 0 Then
        '        If CByte(writer.ToString) = 0 Then
        '            FailToLogin()
        '            inputs.Remove("inputUsername")
        '            inputs.Remove("inputPassword")
        '            writer.Flush()
        '        End If
        '    End If
        'Catch ex As Exception
        '    FailToLogin()
        '    inputs.Remove("inputUsername")
        '    inputs.Remove("inputPassword")
        '    writer.Flush()
        'End Try

        'End If
        

        '''''''''''''''TEMPORARY'''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim window As New AdminWindow(txtUsername.Text)
        window.Show()
        Dim window2 As New ProgramWindow()
        window2.Show()
        Dim window3 = New StudentWindow(txtUsername.Text)
        window3.Show()
        Dim window4 = New StaffWindow(txtUsername.Text)
        window4.Show()
        Me.Finalize()
        Me.Close()
        '''''''''''''''TEMPORARY'''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub LoginWindow_ContentRendered(sender As Object, e As System.EventArgs) Handles Me.ContentRendered
        txtUsername.Focus()
    End Sub

    Private Sub MetroWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'txtUsername.Focus()

        'set variables to workflow
        inputs.Add("controller", controller)
        inputs.Add("adminwindow", Nothing)
        inputs.Add("pmwindow", Nothing)
        inputs.Add("staffwindow", Nothing)
        inputs.Add("studentwindow", Nothing)
        inputs.Add("loginwindow", Me)

    End Sub

    ''' <summary>
    ''' This function clears 2 textboxes' content 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RestoreDefault()
        txtUsername.Text = String.Empty
        txtPassword.Password = String.Empty
    End Sub

    ''' <summary>
    ''' This function displays error message and sets focus on textbox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FailToLogin()
        RestoreDefault()
        errorMsg.Visibility = Windows.Visibility.Visible
        txtUsername.Focus()
    End Sub

    Private Sub MetroWindow_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        'clear memory
        inputs.Clear()
        writer.Dispose()
        GC.Collect()
    End Sub



End Class
