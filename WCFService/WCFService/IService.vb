<ServiceContract()>
Public Interface IService

    <OperationContract()>
    Function GetWeather(ByVal city As String) As Dictionary(Of String, String)

End Interface

