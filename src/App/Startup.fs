namespace WebAPI

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Cors.Infrastructure
open Giraffe
open WebAPI.Controllers.HttpHandlers

module Startup =
    // ---------------------------------
    // Web app
    // ---------------------------------
    let webApp =
        choose [ subRoute
                     "/api"
                     (choose [ GET
                               >=> choose [ route "/hello" >=> handleGetHello
                                            route "/test" >=> testHandler ] ])
                 setStatusCode 404 >=> text "Not Found" ]

    // ---------------------------------
    // CORS
    // ---------------------------------
    let private configureCors (builder: CorsPolicyBuilder) =
        builder
            .WithOrigins("http://localhost:5000", "https://localhost:5001")
            .AllowAnyMethod()
            .AllowAnyHeader()
        |> ignore

    // ---------------------------------
    // Error handler
    // ---------------------------------
    let errorHandler (ex: Exception) (logger: ILogger) =
        logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

        clearResponse
        >=> setStatusCode 500
        >=> text ex.Message

    // ---------------------------------
    // Configure - App & Services
    // ---------------------------------
    let configureServices (services: IServiceCollection) =
        services.AddCors() |> ignore
        services.AddGiraffe() |> ignore

    let configureApp (app: IApplicationBuilder) =
        let env =
            app.ApplicationServices.GetService<IWebHostEnvironment>()

        (match env.IsDevelopment() with
         | true -> app.UseDeveloperExceptionPage()
         | false ->
                //  app.UseGiraffeErrorHandler(errorHandler)
                app.UseHttpsRedirection())
            .UseCors(configureCors)
            .UseGiraffe(webApp)
