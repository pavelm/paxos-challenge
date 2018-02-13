module Messages

open System
open System.Text
open System.Security.Cryptography
open System

module Utils = 
  let bytesToHex (bts:byte[]) = 
    let all = 
      bts 
      |> Array.fold (fun sb b -> Printf.bprintf sb "%x" b; sb) (StringBuilder(64))
    all.ToString()

  let hashMessage (msg:string) = 
    use digest = SHA256.Create()
    let bytes = digest.ComputeHash(Encoding.UTF8.GetBytes(msg))
    bytes  |> bytesToHex



type OriginalMessage = {
    message : string
}

type MessageDigest = {
    digest : string
}


let api = 
  let memCache = new System.Collections.Concurrent.ConcurrentDictionary<string,string>()

  let toHash = 
    fun (req:OriginalMessage) ->
      let msg = req.message
      let digest = Utils.hashMessage msg
      memCache.[digest] <- msg
      { 
        digest = digest
      }
  
  let fromHash = 
    fun (digest:string) -> 
      match memCache.TryGetValue(digest) with
      | (true,v) -> Some { message = v }
      | _ -> None
  
  toHash,fromHash