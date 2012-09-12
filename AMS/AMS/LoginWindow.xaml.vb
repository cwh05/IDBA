Imports MahApps.Metro.Controls

Class LoginWindow : Inherits MetroWindow
    Private controller As LoginWindowContoller = New LoginWindowContoller()

    Private Sub btnLogin_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogin.Click
        'validate input textboxes
        If txtUsername.Text.Length = 0 And txtPassword.Password.Length = 0 Then
            FailToLogin()

        Else
            Try
                Dim forwardTo As Byte = controller.UserVerification(txtUsername.Text, txtPassword.Password)

                ''forward to corresponding page
                'If forwardTo = 0 Then
                '    FailToLogin()
                'Else
                '    If forwardTo = 1 Then
                '        'Administrator
                '        Dim window As New AdminWindow
                '        window.Show()

                '    ElseIf forwardTo = 2 Then
                '        'Program Manager
                '        Dim window As New ProgramWindow
                '        window.Show()

                '    ElseIf forwardTo = 3 Then
                '        'Staff

                '    ElseIf forwardTo = 4 Then
                '        'Student
                '        'Dim window As New StudentWindow(txtUsername.Text)
                '        'window.Show()

                '    End If

                '    Me.Finalize()
                '    Me.Close()
                'End If

                '''''''''''''''TEMPORARY'''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim window As New AdminWindow
                window.Show()
                Dim window1 = New ProgramWindow()
                window1.Show()
                Dim window3 = New StudentWindow(txtUsername.Text)
                window3.Show()
                Me.Finalize()
                Me.Close()
                '''''''''''''''TEMPORARY'''''''''''''''''''''''''''''''''''''''''''''''''''

            Catch ex As Exception
                FailToLogin()
            End Try
        End If

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
