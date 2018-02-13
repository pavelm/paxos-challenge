﻿// Learn more about F# at http://fsharp.org

open System

let numToBaseDigits b num = 
    let rec loop acc = function 
        | x when x < b -> x :: acc
        | rem -> loop ((rem % b)::acc) (rem/b)
    loop [] num 


let padList elem totalCount xs = 
    let len = List.length xs 
    let count = totalCount - len 
    if count > 0 then
        List.append (List.replicate count elem) xs
    else
        xs

let countCharsOf c (s:string) = 
    s |> Seq.filter ((=) c) |> Seq.length


[<EntryPoint>]
let main argv =
    let input = argv.[0]
    let totalXs = input |> countCharsOf 'X'
    let totalIterations = pown 2 totalXs
    
    let toBase2Digits = numToBaseDigits 2 >> padList 0 totalXs 

    let toChar = function 0 -> '0' | 1 -> '1' | _ -> failwith "shouldn't occur"

    let replaceX iteration : string = 
        let digits = toBase2Digits iteration 
        input
        |> Seq.mapFold (fun xs c -> 
            match c with 
            | 'X' -> 
                let (x::xs) = xs
                toChar x,xs
            | bit -> bit,xs) digits
        |> fst 
        |> Seq.toArray
        |> String

    Seq.init totalIterations replaceX
    |> Seq.iter (printfn "%s")

    0 // return an integer exit code
