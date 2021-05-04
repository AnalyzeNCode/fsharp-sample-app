namespace Framework

module AppSettings =
    open System

    let private get key =
        ConfigurationBuilder.init
        Environment.GetEnvironmentVariable key

    let applicationName = get "ApplicationName"
    let apiVersion = get "APIVersion"