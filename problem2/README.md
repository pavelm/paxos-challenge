# Solution 

The general idea is to use a binary search **O(log n)** to find the largest item less than a certain target value since the array is sorted in ascending order. The base case is how to maximize the gift for one person. That's just the largest item less than or equal to the balance of the gift card. Cases for two people, essentially requires pairing one gift with the maximum value gift remaining less the balance remaining after taking the first gift in order to maximize the total value of the two gifts. The total number of combinations of gifts for two people is N x (N-1) where N is the number of gifts available. However, since we're not iterating through each possible combination and picking the highest under the target balance the resulting asymptotic run time is **O(n * log n)**.

## Bonus Question
Expanding upon the same reasoning the case with 3 people is essentially similar in that you fix one gift and find the best solution for the 2 people case reducing the total balance by the amount of the fixed gift. Going through and fixing each gift while solving the 2 case for the remaining elements will result in a **O(n^2 * log n)** run time. More generatlly, if _p_ is the number of people then the runtime becomes **O(n^(p-1) * log n)**.


### Additional commentary
The code has support for allowing an arbitrary amount of people but I've hard coded it to two as per the question.

There's some conversion between different types of collection data types (Seq,List,Array) which can create some memory pressure as N grows larger. The best fix for this would be to use a **Span<T>** data structure as a virtual array so as to not add to the overhead of copying data and converting types. I didn't want to introduce more complexity and intentionally tried to keep the code simpler and use basic types. In a realistic scenario this can be optimized further by reducing the amount of heap allocation and copying of data.
