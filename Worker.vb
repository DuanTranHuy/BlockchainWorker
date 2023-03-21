Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports System.Threading

Namespace WorkerService
    Public Class Worker
        Inherits BackgroundService
        Private ReadOnly _logger As ILogger(Of Worker)
        Private ReadOnly _context As AppDbContext
        Private ReadOnly _alchemyApiClient As IAlchemyApiClient
        Private ReadOnly _blockRepository As IGenericRepository(Of Block)
        Private ReadOnly _transactionRepository As IGenericRepository(Of Transaction)
        Private ReadOnly _batchSize As Integer
        Private ReadOnly _startBlock As Decimal
        Private ReadOnly _endBlock As Decimal
        Private ReadOnly _retryCount As Integer
        Public Sub New(logger As ILogger(Of Worker),
                       dbContext As AppDbContext,
                       apiClient As IAlchemyApiClient,
                       config As IConfiguration,
                       blockRepository As IGenericRepository(Of Block),
        transactionRepository As IGenericRepository(Of Transaction))
            _logger = logger
            _context = dbContext
            _alchemyApiClient = apiClient
            _batchSize = config.GetValue(Of Decimal)("BatchSize")
            _startBlock = config.GetValue(Of Decimal)("StartBlock")
            _endBlock = config.GetValue(Of Decimal)("EndBlock")
            _retryCount = config.GetValue(Of Integer)("AlchemyApiClientRetryCount")
            _blockRepository = blockRepository
            _transactionRepository = transactionRepository
        End Sub

        Protected Overrides Async Function ExecuteAsync(ByVal stoppingToken As CancellationToken) As Task
            While Not stoppingToken.IsCancellationRequested
                Await SyncBlock()
                Await Task.Delay(1000, stoppingToken)
            End While
        End Function

        ''' <summary>
        ''' Check block for syncing
        ''' </summary>
        ''' <returns></returns>
        Async Function SyncBlock() As Task
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now)
            Dim oldestBlock = _context.Blocks.OrderByDescending(Function(b) b.BlockNumber).FirstOrDefault()
            Dim startBlock = _startBlock
            If oldestBlock IsNot Nothing Then
                If oldestBlock.BlockNumber >= _startBlock AndAlso oldestBlock.BlockNumber < _endBlock Then
                    startBlock = oldestBlock.BlockNumber + 1D
                ElseIf oldestBlock.BlockNumber >= _endBlock Then
                    _logger.LogInformation($"All blocks are synced to {_endBlock}")
                    Return
                End If
            End If
            For index = startBlock To _endBlock
                _logger.LogInformation($"Start syncing block: {index}...")
                Await IndexBlockAndTransactionInformationAsync(index)
            Next
        End Function

        ''' <summary>
        ''' Sync a specific block
        ''' </summary>
        ''' <param name="blockNumber"></param>
        ''' <returns></returns>
        Async Function IndexBlockAndTransactionInformationAsync(blockNumber As Decimal) As Task
            Dim blockNumberInHex = HexDecimal.DecimalToHexDecimal(blockNumber)
            Dim rpcBlock = Await _alchemyApiClient.GetBlockByNumberAsync(blockNumberInHex)
            If rpcBlock IsNot Nothing Then
                ' Add Block to database
                Dim block = Await _blockRepository.AddAsync(Mapping.RpcBlockToEntity(blockNumber, rpcBlock))
                ' Get the count of transactions
                Dim count = Await _alchemyApiClient.GetBlockTransactionCountByNumberAsync(blockNumberInHex)
                _logger.LogInformation($"Block {blockNumber}: There are {count} transactions")
                If count <> 0 Then
                    Dim batches = Math.Floor(count / _batchSize)
                    _logger.LogInformation($"Block {blockNumber}: Chunking {count} transactions into {batches} processes")
                    For i = 0D To batches
                        Dim retryCount As Integer = 0
                        Dim transactions = Await ProcessingTransaction(count, block, i, blockNumberInHex)
                        Do While transactions.Count < _batchSize AndAlso i <> batches AndAlso retryCount < _retryCount
                            _logger.LogInformation($"Block {blockNumber}:Retrying batch {i} with {transactions.Count} transactions")
                            transactions = Await ProcessingTransaction(count, block, i, blockNumberInHex)
                            retryCount += 1
                        Loop
                        If retryCount = _retryCount AndAlso transactions.Count < _batchSize Then
                            _logger.LogError($"Block {blockNumber}: Error when trying to get transactions")
                            Return
                        End If
                        _logger.LogInformation($"Block {blockNumber}: Inserting batch {i} with {transactions.Count} transactions")
                        Await _transactionRepository.AddRangeAsync(transactions)
                    Next
                    ' Save the list of transactions to the database
                    Await _context.SaveChangesAsync()
                End If
            End If
        End Function

        ''' <summary>
        ''' Call APIs in a batch
        ''' </summary>
        ''' <param name="count"></param>
        ''' <param name="block"></param>
        ''' <param name="i"></param>
        ''' <param name="blockNumberInHex"></param>
        ''' <returns></returns>
        Async Function ProcessingTransaction(count As Integer, block As Block, i As Decimal, blockNumberInHex As String) As Task(Of List(Of Transaction))
            Dim transactions As New List(Of Transaction)
            Dim tasks = Enumerable.Range(i * _batchSize, _batchSize).Select(
            Async Function(index)
                If index < count Then
                    Dim transaction = Await _alchemyApiClient.GetTransactionByBlockNumberAndIndexAsync(blockNumberInHex, HexDecimal.DecimalToHexDecimal(index))
                    If transaction IsNot Nothing Then
                        transactions.Add(Mapping.RpcTransactionToEntity(block, transaction))
                    End If
                End If
            End Function).ToArray()
            Await Task.WhenAll(tasks)
            Return transactions
        End Function
    End Class
End Namespace