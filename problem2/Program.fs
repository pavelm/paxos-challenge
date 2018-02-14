// Learn more about F# at http://fsharp.org

open System
open System.IO


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


let findLargest (search : int -> Sku[] -> Sku option) numSkus  balance (skus : Sku []) =
    let rec pairGen arr =
        seq {
            match arr with 
            | [||] -> yield! Seq.empty
            | xs -> 
                let head = Array.head xs
                let tail = Array.tail xs
                yield (head,tail)
                yield! pairGen tail 
        }

    let rec sequence depth maxPrice (xs:Sku[]) = 
        match depth with 
        | 0 | _ when xs.Length = 0 -> Seq.empty
        | 1 -> 
            match search maxPrice xs with
            | Some x -> [x] |> Seq.singleton
            | None -> Seq.empty
        | d  -> 
            seq {
                for (head,tail) in pairGen xs do
                    let nextMax = maxPrice - head.price
                    if nextMax <= 0 then 
                        yield! Seq.empty
                    else 
                        let results = sequence (d-1) nextMax (tail)
                        if Seq.isEmpty results then
                            yield! Seq.empty
                        else 
                            for r in results do 
                                yield head :: r
            }
    sequence numSkus balance skus

[<EntryPoint>]
let main argv =
    let filename = argv.[0]
    let balance = Int32.Parse argv.[1] 
    if not <| File.Exists filename then
        printfn "File %s does not exist" filename
    else
        let file = File.ReadAllLines(filename)
        let skus = file |> parse
        
        let badSearch max skus = 
            let filtered = skus |> Array.filter (fun sku -> sku.price <= max) 
            if Array.isEmpty filtered then
                None
            else
                filtered |> Array.maxBy (fun sku -> sku.price ) |> Some

        let searchAlgo = binSearch
        let sw = System.Diagnostics.Stopwatch()
        sw.Start()
        let results = findLargest searchAlgo 2 balance skus |> Seq.toArray //iterate through all
        sw.Stop()
        results |> Seq.length |> printfn "SOLUTION results %O"
        printfn "Total elapsed time=%O ticks=%d" sw.ElapsedMilliseconds sw.ElapsedTicks

    0 // return an integer exit code
