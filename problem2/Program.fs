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

let pause () =
    Console.ReadLine() |> ignore

// Using binary search O(log n) finds the sku with the 
// highest price less than the maximum specified
// Assumes all the sku's are in ascending order by price
let binSearch max (sku : Sku []) = 
    let skuMaxPrice (left:Sku) (right:Sku) =
        // find the max of two Sku's if they are equal return the first one
        if left.price >= right.price then
            left
        else 
            right

    let rec loop acc left right = 
        // printfn "Entered loop acc=%O left=%d right=%d" acc left right
        // pause()
        if left > right then 
            acc
        elif left = right then
            let midpoint = sku.[left]
            if midpoint.price <= max then
                skuMaxPrice acc midpoint
            else
                acc
        else
            let midpoint = (right - left)/2 + left 
            let item = sku.[midpoint]
            if item.price = max then 
                item
            elif item.price < max then 
                // go right
                let acc = skuMaxPrice acc item
                loop acc (midpoint+1) right
            else
                // go left
                loop acc left (midpoint-1)
    if sku.Length = 0 || sku.[0].price > max then 
        None
    else
        loop sku.[0] 0 (sku.Length-1) |> Some


let findLargest (search : int -> Sku[] -> Sku option)  balance (skus : Sku []) =
    let rec loop acc rem = function
        | [] -> acc
        | x::xs -> 
            let nextRem = rem - x.price 
            printfn "nextRem=%d currentItem=%O" nextRem x
            pause()
            if nextRem > 0 then 
                //take the first element, find the largest next element
                match search nextRem (xs |> List.toArray) with
                | None -> loop acc balance xs
                | Some largest -> loop ((x,largest) :: acc) balance xs 
            else 
                printfn "nextRem=0"
                loop (acc) balance xs

    let createPairs arr = 
        let rec loop acc = function 
            | [] -> acc |> List.rev
            | x::xs -> loop ((x,xs)::acc) xs
        loop [] (arr |> Array.toList)


    let rec sequence depth maxPrice (xs:Sku[]) = 
        match depth with 
        | 0 | _ when xs.Length = 0 -> Seq.empty
        | 1 -> 
            match search maxPrice xs with
            | Some x -> [x] |> Seq.singleton
            | None -> Seq.empty
        | d  -> 
            seq {
                for (head,tail) in createPairs xs do
                    let nextMax = maxPrice - head.price
                    if nextMax <= 0 then 
                        yield! Seq.empty
                    else 
                        printfn "Depth=%d looking for matches for %O with remaining balance=%d" d head nextMax
                        let results = sequence (d-1) nextMax (tail |> List.toArray)
                        printfn "found %d results" (Seq.length results)
                        if Seq.isEmpty results then
                            yield! Seq.empty
                        else 
                            for r in results do 
                                yield head :: r
            }

    loop [] balance (skus |> Array.toList)


let go num skus = 
    let search = binSearch
    findLargest search num skus |> Seq.iter (fun (x,y) -> printfn "(%s,%d)-(%s,%d)" x.name x.price y.name y.price)

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let filename = Path.Combine("problem2","prices.txt")
    // let filename = argv.[0]
    let balance = Int32.Parse argv.[1] 
    if not <| File.Exists filename then
        printfn "File %s does not exist" filename
    else
        let file = File.ReadAllLines(filename)
        let skus = file |> parse
        
        go 2000 skus
        ()
    0 // return an integer exit code
