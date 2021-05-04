namespace WebAPI

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Configuration
open Startup
open Framework

module Program =
    // ---------------------------------
    // Logging Configuration
    // ---------------------------------
    let configureLogging (builder: ILoggingBuilder) =
        builder.AddConsole().AddDebug() |> ignore

    // ---------------------------------
    // Creating WebHostBuilder
    // ---------------------------------
    let CreateHostBuilder args =
        Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(fun hostContext config ->
                config.AddEnvironmentVariables() |> ignore

                match args with
                | null -> () |> ignore
                | _ -> config.AddCommandLine(args) |> ignore)
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                |> ignore)


    [<EntryPoint>]
    let main args =
        // // NOTE: I have to call init like this otherwise those properties will remain uninitialize
        // // until first time access of any one property
        // ConfigurationBuilder.init
        // Console.WriteLine($"Accessing property from main function. Application Name: {Framework.AppSettings.applicationName}")

        CreateHostBuilder(args).Build().Run()

        0
