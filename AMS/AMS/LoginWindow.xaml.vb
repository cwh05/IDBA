Imports MahApps.Metro.Controls
Imports System.Activities

Class LoginWindow : Inherits MetroWindow
    Private controller As LoginWindowContoller = New LoginWindowContoller()

    Private Sub btnLogin_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        'Dim s = New LoginFlow()
        'Dim w = New WorkflowInvoker(s)
        'w.Invoke()

        'WorkflowInvoker.Invoke(s)


        'validate input textboxes
        'If txtUsername.Text.Length = 0 And txtPassword.Password.Length = 0 Then
        '    FailToLogin()
        'Else

        'End If

        '''''''''''''''TEMPORARY'''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim window As New AdminWindow
        window.Show()
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

    Private Sub MetroWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtUsername.Focus()
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
End Class
