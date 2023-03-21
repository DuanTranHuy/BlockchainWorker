Public Class EthBlock
    Public Property number As String
    Public Property hash As String
    Public Property parentHash As String
    Public Property nonce As String
    Public Property sha3Uncles As String
    Public Property logsBloom As String
    Public Property transactionsRoot As String
    Public Property stateRoot As String
    Public Property miner As String
    Public Property difficulty As String
    Public Property totalDifficulty As String
    Public Property extraData As String
    Public Property size As String
    Public Property gasLimit As String
    Public Property gasUsed As String
    Public Property timestamp As String
    Public Property blockReward As String = Nothing
    Public Property transactions As List(Of String)
    Public Property uncles As List(Of String)
End Class
