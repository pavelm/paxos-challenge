# Solution

The strategy here was to find all occurences of `X`s and to compute the total number of combinations substituting each `X` with a `0` or `1`. Then, for each combination, it breaks down the iteration number to binary representation in which each binary digit maps to whether to put a `0` or `1` in that location for the `X`. The input characters are then mapped over and replace the `X`s with the appropriate binary digit for a specific iteration. 

## Performance

The runtime of the algorithm is **O(2^n)** where **n** represents the number of `X`s in the input. The iterations are using lazy sequence generators for performance reasons and otherwise using recursion or collecting all possible values before printing them can cause the program to run out of memory on very large inputs.


## Build and test the application

### Linux/macOS

Run the `run.sh` script in order to restore, build, and run the application:

```
$ ./run.sh
```

## Docker 
Run the `docker_run.sh` script to run the project build in a docker container. 

Pass input 

```
$ ./docker_run.sh 10X10X0
```
