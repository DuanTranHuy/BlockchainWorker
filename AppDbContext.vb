Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Logging

Public Class AppDbContext
    Inherits DbContext

    Private ReadOnly config As IConfiguration

    Public Sub New(options As DbContextOptions(Of AppDbContext), config As IConfiguration)
        MyBase.New(options)
        Me.config = config
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseMySQL(config.GetConnectionString("MySQLConnection"))
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(Function(builder) builder.AddFilter(
     Function(category, level) category = "Microsoft.EntityFrameworkCore.Database.Command" AndAlso level = LogLevel.Error
 ).AddConsole()))
    End Sub

    Public Property Blocks As DbSet(Of Block)
    Public Property Transactions As DbSet(Of Transaction)
End Class