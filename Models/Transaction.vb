Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class Transaction
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    <Column("transactionID")>
    Public Property TransactionID As Integer

    <ForeignKey("Block")>
    <Column("blockID")>
    Public Property BlockID As Integer

    <Column("hash", TypeName:="varchar(66)")>
    <MaxLength(66)>
    Public Property Hash As String

    <Column("from", TypeName:="varchar(42)")>
    <MaxLength(42)>
    Public Property From As String

    <Column("to", TypeName:="varchar(42)")>
    <MaxLength(42)>
    Public Property [To] As String

    <Column("value", TypeName:="decimal(50,0)")>
    Public Property Value As Decimal

    <Column("gas", TypeName:="decimal(50,0)")>
    Public Property Gas As Decimal

    <Column("gasPrice", TypeName:="decimal(50,0)")>
    Public Property GasPrice As Decimal

    <Column("transactionIndex")>
    Public Property TransactionIndex As Integer

    Public Overridable Property Block As Block
End Class
