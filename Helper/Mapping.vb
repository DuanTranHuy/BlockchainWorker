Public Class Mapping
    Public Shared Function RpcBlockToEntity(blockNumber As Decimal, ethBlock As EthBlock) As Block
        Return New Block With {
                .BlockNumber = blockNumber,
                .ParentHash = ethBlock.parentHash,
                .GasLimit = HexDecimal.HexToDecimal(ethBlock.gasLimit),
                .GasUsed = HexDecimal.HexToDecimal(ethBlock.gasUsed),
                .Hash = ethBlock.hash,
                .Miner = ethBlock.miner,
                .BlockReward = HexDecimal.HexToDecimal(ethBlock.blockReward)
        }
    End Function

    Public Shared Function RpcTransactionToEntity(block As Block, transaction As EthTransaction, Optional index As Integer = 0) As Transaction
        Return New Transaction With {
                                                    .Hash = transaction.hash,
                                                    .From = transaction.from,
                                                    .To = transaction.to,
                                                    .Value = HexDecimal.HexToDecimal(transaction.value),
                                                    .Block = block,
                                                    .GasPrice = HexDecimal.HexToDecimal(transaction.gasPrice),
                                                    .Gas = HexDecimal.HexToDecimal(transaction.gas),
                                                    .TransactionIndex = index}
    End Function

End Class
