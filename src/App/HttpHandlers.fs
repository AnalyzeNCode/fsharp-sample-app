module WebAPI.Controllers.HttpHandlers

open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Giraffe
open WebAPI.Models
open Framework

let handleGetHello =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let applicationName = AppSettings.applicationName
            let apiVersion = AppSettings.apiVersion

            let response = { Text = $"From HttpHandlers: Currently accessing version:{apiVersion} of app:{applicationName}..." }
            return! json response next ctx
        }

let testHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            failwith "Throwing test exception to test Error Handler configurations"

            let response =
                { Text = "Test handler...created to test Custom Exception Handler..." }

            return! json response next ctx
        }

        // let config = ctx.GetService<IConfiguration>()

            // let appSettings =
            //     config
            //         .GetSection("AppSettings")
            //         .Get<AppSettings>()

            // printfn "Application Name is: %s" appSettings.ApplicationName
            // printfn "App Version is: %s" appSettings.APIVersion