Imports System.Net.Http
Imports System.Text
Imports Microsoft.Extensions.Configuration
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class AlchemyApiClient
    Implements IAlchemyApiClient
    Private ReadOnly _apiKey As String
    Private ReadOnly _retryCount As Integer
    Private ReadOnly _baseUrl As String
    Private ReadOnly _httpClient As HttpClient

    Public Sub New(config As IConfiguration, endpoint As AlchemyApiEndpoint)
        _apiKey = If(String.IsNullOrWhiteSpace(_apiKey), config.GetValue(Of String)("AlchemyApiKey"), _apiKey)
        _baseUrl = $"https://eth-{endpoint.ToString().ToLower()}.alchemyapi.io/v2/"
        _retryCount = config.GetValue(Of Integer)("AlchemyApiClientRetryCount")
        _httpClient = New HttpClient()
    End Sub

    Public Async Function GetBlockByNumberAsync(blockNumber As String, Optional includeTransactions As Boolean = False) As Task(Of EthBlock) Implements IAlchemyApiClient.GetBlockByNumberAsync
        Return Await CallJsonRpcAsync(Of EthBlock)("eth_getBlockByNumber", blockNumber, includeTransactions)
    End Function

    Public Async Function GetTransactionByBlockNumberAndIndexAsync(blockNumber As String, index As String) As Task(Of EthTransaction) Implements IAlchemyApiClient.GetTransactionByBlockNumberAndIndexAsync
        Return Await CallJsonRpcAsync(Of EthTransaction)("eth_getTransactionByBlockNumberAndIndex", blockNumber, index)
    End Function

    Public Async Function GetBlockTransactionCountByNumberAsync(blockNumber As String) As Task(Of Integer) Implements IAlchemyApiClient.GetBlockTransactionCountByNumberAsync
        Dim data = Await CallJsonRpcAsync(Of String)("eth_getBlockTransactionCountByNumber", blockNumber)
        Return CInt("&H" & data)
    End Function

    Public Async Function CallJsonRpcAsync(Of T)(method As String, ParamArray params As Object()) As Task(Of T) Implements IAlchemyApiClient.CallJsonRpcAsync
        Dim url = $"{_baseUrl}{_apiKey}"
        Dim requestBody = JObject.FromObject(New With {.jsonrpc = "2.0", .id = 0, method, params})
        For retry = 1 To _retryCount
            Try
                Dim response = Await _httpClient.PostAsync(url, New StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"))
                If response.IsSuccessStatusCode Then
                    Dim responseContent = Await response.Content.ReadAsStringAsync()
                    Dim resultObject = JObject.Parse(responseContent)("result")
                    Return resultObject.ToObject(Of T)()
                Else
                    Throw New Exception($"Failed to retrieve data for {method}. Status code: {response.StatusCode}")
                End If
            Catch ex As HttpRequestException
                If retry >= _retryCount Then
                    Throw New Exception($"Failed to retrieve data for {method}. Maximum retries ({_retryCount}) exceeded.", ex)
                End If
            End Try
        Next
        ' This line should never execute, but the compiler requires a return statement.
        Return Nothing
    End Function
End Class

Public Enum AlchemyApiEndpoint
    Mainnet
End Enum

Public Interface IAlchemyApiClient
    Function GetBlockByNumberAsync(blockNumber As String, Optional includeTransactions As Boolean = False) As Task(Of EthBlock)
    Function GetTransactionByBlockNumberAndIndexAsync(blockNumber As String, index As String) As Task(Of EthTransaction)
    Function GetBlockTransactionCountByNumberAsync(blockNumber As String) As Task(Of Integer)
    Function CallJsonRpcAsync(Of T)(method As String, ParamArray params As Object()) As Task(Of T)
End Interface
