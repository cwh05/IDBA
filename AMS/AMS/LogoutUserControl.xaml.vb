Public Class LogoutUserControl

    Private Sub btnLogout_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnLogout.Click
        Dim window As New LoginWindow()

        'close all active windows
        For Each i As Window In Application.Current.Windows
            If Not i.Title.Equals("Academic Management System") Then
                i.Close()
            End If
        Next

        GC.Collect()

        'launch login window
        window.Show()
    End Sub

End Class
