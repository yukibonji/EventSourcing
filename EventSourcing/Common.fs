﻿namespace EventSourcing

type EntityId = System.Guid
type Version  = int

type ITransactionScope =
    inherit System.IDisposable

type IEventRepository =
    abstract beginTransaction : unit -> ITransactionScope
    abstract commit           : ITransactionScope -> unit
    abstract rollback         : ITransactionScope -> unit
    abstract exists           : EntityId -> bool
    abstract restore          : ITransactionScope * EntityId * Projection.T<'e,_,'a> -> ('a * Version)
    abstract add              : ITransactionScope * EntityId * Version option * 'a -> Version

exception HandlerException of exn

[<AutoOpen>]
module internal Common =

    open System.Collections.Generic

    let (|Contains|_|) (k : 'k) (d : Dictionary<'k,'v>) =
        match d.TryGetValue k with
        | (true, v)  -> Some v
        | (false, _) -> None

