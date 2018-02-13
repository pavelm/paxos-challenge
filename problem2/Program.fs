// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Collections.Generic


type Sku = {
    name : string
    price : int
} with 
    override x.ToString() = sprintf "(%s,%d)" x.name x.price
    static member create(name,price) = { name = name; price = Int32.Parse price}



let parse (lines : string []) =
    let split (s:string) = 
        match s.Split(',') with
        | [|name;price|] -> Some (name,price)
        | _ -> None

    lines
    |> Array.choose split
    |> Array.map Sku.create


let binarySearch maxPrice (skus : Sku []) = 
    printfn "started in binary search total=%d" skus.Length
    let rec loop acc left right  = 
        printfn "entered acc=%A left=%d right=%d" acc left right
        if right = left then 
            if skus.[left].price > acc.price then 
                skus.[left]
            else 
                acc
        elif right < left then
            acc
        else
            let midpoint = (right-left + 1)/2 + left
            printfn "entered loop acc=%A left=%d right=%d midpoint=%d" acc left right midpoint
            let sku = skus.[midpoint]
            if sku.price = maxPrice then 
                printfn "found max price sku=%A" sku
                sku 
            elif sku.price > maxPrice then
                printfn "its greater than max, going left"
                loop acc left (midpoint - 1)
            elif sku.price > acc.price then
                printfn "Found greater accumulator than max, going right"
                loop sku (midpoint + 1) right
            else
                printfn "Going right"
                loop acc (midpoint+1) right
    loop skus.[0] 0 (skus.Length-1)

let findPairing maxItems balance (skus : Sku []) = 
    let rec loop acc depth maxBalance (xs:Sku list) = 
        if depth = 0 then
            acc
        else
            match xs with
            | [] -> acc
            | x::xs -> 
                let remainingBalance = maxBalance - x.price
                printfn "remaining balance=%d price=%d"  remainingBalance x.price
                let best = binarySearch remainingBalance (xs |> List.toArray)
                printfn "found best=%A" best
                loop (best::acc) (depth-1) remainingBalance xs
    loop [] maxItems balance (skus |> List.ofArray)
        
let findLargest balance (skus : Sku []) =
    let rec loop acc rem = function
        | [] -> acc
        | x::xs -> 
            let nextRem = rem - x.price 
            printfn "nextRem=%d currentItem=%O" nextRem x
            if nextRem > 0 then 
                //take the first element, find the largest next element
                let filtered = xs |> List.filter (fun x -> x.price <= nextRem) 
                if Seq.isEmpty filtered then 
                    printfn "Nothing less than nextRem=%d" nextRem
                    loop acc balance xs
                else
                    let largest = filtered |> List.maxBy (fun x -> x.price)
                    printfn "found largest=%O" largest
                    loop ((x,largest) :: acc) nextRem xs 
            else 
                printfn "nextRem=0"
                loop (acc) balance xs
    loop [] balance (skus |> Array.toList)

let test skus = 
    let go num = findLargest num skus |> Seq.iter (fun (x,y) -> printfn "(%s,%d)-(%s,%d)" x.name x.price y.name y.price)
    go 2000

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let testSegment = 
        let arr = [|0..9|]
        let seg = new ArraySegment<_>(arr)
        let ilist : IList<_> = upcast seg
        ilist.RemoveAt(5)
        let res = ilist |> Seq.toArray
        printfn "res=%A\nseq=%A\narr=%A" res seg arr

    Environment.Exit(0)
    let filename = Path.Combine("problem2","prices.txt")
    let filename = argv.[0]
    let balance = Int32.Parse argv.[1] 
    if not <| File.Exists filename then
        printfn "File %s does not exist" filename
    else
        let file = File.ReadAllLines(filename)
        let skus = file |> parse
        
        binarySearch 800 skus
        ()
    0 // return an integer exit code
