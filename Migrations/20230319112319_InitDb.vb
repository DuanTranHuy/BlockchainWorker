Imports Microsoft.EntityFrameworkCore.Migrations
Imports MySql.EntityFrameworkCore.Metadata

Namespace Global.WorkerBlockchain.Migrations
    ''' <inheritdoc />
    Partial Public Class InitDb
        Inherits Migration

        ''' <inheritdoc />
        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)
            migrationBuilder.AlterDatabase().
                    Annotation("MySQL:Charset", "utf8mb4")

            migrationBuilder.CreateTable(
                name:="Blocks",
                columns:=Function(table) New With {
                    .blockID = table.Column(Of Integer)(type:="int", nullable:=False).
                        Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    .blockNumber = table.Column(Of Integer)(type:="int", nullable:=False),
                    .hash = table.Column(Of String)(type:="varchar(66)", maxLength:=66, nullable:=True),
                    .parentHash = table.Column(Of String)(type:="varchar(66)", maxLength:=66, nullable:=True),
                    .miner = table.Column(Of String)(type:="varchar(42)", maxLength:=42, nullable:=True),
                    .blockReward = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False),
                    .gasLimit = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False),
                    .gasUsed = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False)
                },
                constraints:=Sub(table)
                    table.PrimaryKey("PK_Blocks", Function(x) x.blockID)
                End Sub).
                    Annotation("MySQL:Charset", "utf8mb4")

            migrationBuilder.CreateTable(
                name:="Transactions",
                columns:=Function(table) New With {
                    .transactionID = table.Column(Of Integer)(type:="int", nullable:=False).
                        Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    .blockID = table.Column(Of Integer)(type:="int", nullable:=False),
                    .hash = table.Column(Of String)(type:="varchar(66)", maxLength:=66, nullable:=True),
                    .from = table.Column(Of String)(type:="varchar(42)", maxLength:=42, nullable:=True),
                    .[to] = table.Column(Of String)(name:="to", type:="varchar(42)", maxLength:=42, nullable:=True),
                    .value = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False),
                    .gas = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False),
                    .gasPrice = table.Column(Of Decimal)(type:="decimal(50,0)", nullable:=False),
                    .transactionIndex = table.Column(Of Integer)(type:="int", nullable:=False)
                },
                constraints:=Sub(table)
                    table.PrimaryKey("PK_Transactions", Function(x) x.transactionID)
                    table.ForeignKey(
                        name:="FK_Transactions_Blocks_blockID",
                        column:=Function(x) x.blockID,
                        principalTable:="Blocks",
                        principalColumn:="blockID",
                        onDelete:=ReferentialAction.Cascade)
                End Sub).
                    Annotation("MySQL:Charset", "utf8mb4")

            migrationBuilder.CreateIndex(
                name:="IX_Transactions_blockID",
                table:="Transactions",
                column:="blockID")
        End Sub

        ''' <inheritdoc />
        Protected Overrides Sub Down(migrationBuilder As MigrationBuilder)
            migrationBuilder.DropTable(
                name:="Transactions")

            migrationBuilder.DropTable(
                name:="Blocks")
        End Sub
    End Class
End Namespace
