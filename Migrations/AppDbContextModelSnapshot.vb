﻿' <auto-generated />
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Migrations
Imports WorkerBlockchain

Namespace Global.WorkerBlockchain.Migrations
    <DbContext(GetType(AppDbContext))>
    Partial Class AppDbContextModelSnapshot
        Inherits ModelSnapshot

        Protected Overrides Sub BuildModel(modelBuilder As ModelBuilder)
            modelBuilder.
                HasAnnotation("ProductVersion", "7.0.4").
                HasAnnotation("Relational:MaxIdentifierLength", 64)

            modelBuilder.Entity("WorkerBlockchain.Block",
                Sub(b)
                    b.Property(Of Integer)("BlockID").
                        ValueGeneratedOnAdd().
                        HasColumnType("int").
                        HasColumnName("blockID")

                    b.Property(Of Integer)("BlockNumber").
                        HasColumnType("int").
                        HasColumnName("blockNumber")

                    b.Property(Of Decimal)("BlockReward").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("blockReward")

                    b.Property(Of Decimal)("GasLimit").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("gasLimit")

                    b.Property(Of Decimal)("GasUsed").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("gasUsed")

                    b.Property(Of String)("Hash").
                        HasMaxLength(66).
                        HasColumnType("varchar(66)").
                        HasColumnName("hash")

                    b.Property(Of String)("Miner").
                        HasMaxLength(42).
                        HasColumnType("varchar(42)").
                        HasColumnName("miner")

                    b.Property(Of String)("ParentHash").
                        HasMaxLength(66).
                        HasColumnType("varchar(66)").
                        HasColumnName("parentHash")

                    b.HasKey("BlockID")

                    b.ToTable("Blocks")
                End Sub)

            modelBuilder.Entity("WorkerBlockchain.Transaction",
                Sub(b)
                    b.Property(Of Integer)("TransactionID").
                        ValueGeneratedOnAdd().
                        HasColumnType("int").
                        HasColumnName("transactionID")

                    b.Property(Of Integer)("BlockID").
                        HasColumnType("int").
                        HasColumnName("blockID")

                    b.Property(Of String)("From").
                        HasMaxLength(42).
                        HasColumnType("varchar(42)").
                        HasColumnName("from")

                    b.Property(Of Decimal)("Gas").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("gas")

                    b.Property(Of Decimal)("GasPrice").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("gasPrice")

                    b.Property(Of String)("Hash").
                        HasMaxLength(66).
                        HasColumnType("varchar(66)").
                        HasColumnName("hash")

                    b.Property(Of String)("To").
                        HasMaxLength(42).
                        HasColumnType("varchar(42)").
                        HasColumnName("to")

                    b.Property(Of Integer)("TransactionIndex").
                        HasColumnType("int").
                        HasColumnName("transactionIndex")

                    b.Property(Of Decimal)("Value").
                        HasColumnType("decimal(50,0)").
                        HasColumnName("value")

                    b.HasKey("TransactionID")

                    b.HasIndex("BlockID")

                    b.ToTable("Transactions")
                End Sub)

            modelBuilder.Entity("WorkerBlockchain.Transaction",
                Sub(b)
                    b.HasOne("WorkerBlockchain.Block", "Block").
                        WithMany().
                        HasForeignKey("BlockID").
                        OnDelete(DeleteBehavior.Cascade).
                        IsRequired()
                    b.Navigation("Block")
                End Sub)
        End Sub
    End Class
End Namespace
