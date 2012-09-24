Public Class MyCustomDialog


    Public Property ResponseText() As String
        Get
            Return ResponseTextBox.Text
        End Get
        Set(value As String)
            ResponseTextBox.Text = value
        End Set
    End Property

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        ResponseTextBox.Focus()
    End Sub
End Class
