namespace Communication.Agent
open System.Net
open Communication.Udp

type AgentState = { x: int }

type AgentMessage =
    | Hello of From: string * Secret: string
    | Other of From: string * Message: string

type Agent(communicationProvider: IUdpCommunicationClientWrapper) =
        
    member this.Start() =
        let mailboxProcessor = MailboxProcessor<struct(IPEndPoint * string)>.Start(fun inbox ->
            let rec loop state = async {
                    printfn "Total rcvd: %d" state.x
                    
                    let! message = inbox.Receive()
                    match message with
                    | (from, message) -> printfn "\nFrom: %s:%s\nMessage: %s"
                                             (from.Address.ToString()) (from.Port.ToString()) message
                                         communicationProvider.Send("СЛЫШ СУКА СЮДА ИДИ", from)
                    return! loop {x = state.x + 1} 
                }
            loop { x = 0 }
            )

        communicationProvider.OnMessageReceive.Add(fun x -> mailboxProcessor.Post(x))
        ()

(*
module Say =
    let hello name =
        printfn "Hello %s" name
        *)
