Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports WorkerBlockchain.WorkerService

Module Program
    Sub Main(args As String())
        Dim builder = Host.CreateDefaultBuilder(args)
        builder.ConfigureLogging(
            Sub(loggingBuilder As ILoggingBuilder)
                loggingBuilder.ClearProviders()

                loggingBuilder.AddSystemdConsole(
                Sub(options)
                    options.IncludeScopes = False
                End Sub)
            End Sub)
        builder.ConfigureAppConfiguration(
            Sub(configBuilder)
                configBuilder.AddJsonFile("appsettings.json", optional:=False)
            End Sub)
        builder.ConfigureServices(
        Sub(services)
            services.AddHostedService(Of Worker)()
            services.AddDbContext(Of AppDbContext)(
                Function(serviceProvider, optionsBuilder)
                    Return New AppDbContext(optionsBuilder.Options, serviceProvider.GetService(Of IConfiguration)())
                End Function
            )
            services.AddSingleton(Of IAlchemyApiClient)(
                Function(provider) New AlchemyApiClient(
                    provider.GetService(Of IConfiguration)(),
                    AlchemyApiEndpoint.Mainnet
                )
            )
            services.AddScoped(GetType(IGenericRepository(Of Block)), GetType(GenericRepository(Of Block, AppDbContext)))
            services.AddScoped(GetType(IGenericRepository(Of Transaction)), GetType(GenericRepository(Of Transaction, AppDbContext)))

            Using scope = services.BuildServiceProvider().CreateScope()
                Dim context = scope.ServiceProvider.GetService(Of AppDbContext)()
                context.Database.EnsureCreated()
            End Using
        End Sub)
        builder.Build().Run()
    End Sub
End Module
