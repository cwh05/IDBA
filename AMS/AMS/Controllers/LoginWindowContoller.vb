Public Class LoginWindowContoller
    ''' <summary>
    ''' This function authenticates username and password supplied by user
    ''' </summary>
    ''' <param name="username">A username</param>
    ''' <param name="password">A password</param>
    ''' <returns>a scalar byte value that determines the next forwarding page</returns>
    ''' <remarks></remarks>
    Public Function UserVerification(ByRef username As String, ByRef password As String) As Byte
        'connect to database
        Dim db As New AMSEntities()

        'return scalar value from authentication process 
        Return db.LoginVerification(username, password).First.Value
    End Function

End Class
