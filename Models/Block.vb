Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class Block
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    <Column("blockID")>
    Public Property BlockID As Integer

    <Column("blockNumber")>
    Public Property BlockNumber As Integer

    <Column("hash", TypeName:="varchar(66)")>
    <MaxLength(66)>
    Public Property Hash As String

    <Column("parentHash", TypeName:="varchar(66)")>
    <MaxLength(66)>
    Public Property ParentHash As String

    <Column("miner", TypeName:="varchar(42)")>
    <MaxLength(42)>
    Public Property Miner As String

    <Column("blockReward", TypeName:="decimal(50,0)")>
    Public Property BlockReward As Decimal

    <Column("gasLimit", TypeName:="decimal(50,0)")>
    Public Property GasLimit As Decimal

    <Column("gasUsed", TypeName:="decimal(50,0)")>
    Public Property GasUsed As Decimal
End Class
