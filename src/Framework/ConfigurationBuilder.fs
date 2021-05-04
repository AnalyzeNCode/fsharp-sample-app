namespace Framework

module ConfigurationBuilder =
    open System
    open System.IO

    let private parseLine (line: string) =
        match String.IsNullOrEmpty line with
        | true -> ()
        | _ ->
            Console.WriteLine(sprintf "Parsing: %s ..." line)

            match line.Split('=', StringSplitOptions.RemoveEmptyEntries) with
            | args when args.Length = 2 ->
                Environment.SetEnvironmentVariable(args.[0].Trim(), args.[1].Trim())
            | _ -> ()

    let private load() =
        lazy (
            Console.WriteLine "Trying to load Settings.env file..."
            // TODO: Function can be generalized by passing fileNameWithPath instead of hard-coded file-name in this function
            let dir = Directory.GetCurrentDirectory()
            let filePath = Path.Combine(dir, "Settings.env")

            filePath
            |> File.Exists
            |> function
                | false -> Console.WriteLine "No Settings.env file exist"
                | true ->
                    filePath
                    |> File.ReadAllLines
                    |> Seq.iter parseLine
        )

    let init = load().Value