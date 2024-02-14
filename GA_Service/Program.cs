using GA_Service;
using Nucleo;
using Nucleo.Interface;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<AppSettingsManager>();
        services.AddHostedService<Worker>().AddSingleton<ISegundoPLano, SegundoPlano>();
    })
    .Build();




await host.RunAsync();
