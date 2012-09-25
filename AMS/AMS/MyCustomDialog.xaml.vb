Public Class MyCustomDialog

    Private ReturnResult As Integer = 0
    Public Property ResponseText() As String
        Get
            Return ResponseTextBox.Text
        End Get
        Set(value As String)
            ResponseTextBox.Text = value
        End Set
    End Property

    Public Property MyReturnResult() As Integer
        Get
            Return ReturnResult
        End Get
        Set(value As Integer)
            ReturnResult = value
        End Set
    End Property

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click

        Me.MyReturnResult = 1
        Close()

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        ResponseTextBox.Focus()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Close()
    End Sub

End Class
