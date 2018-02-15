# Paxos Challenge Solutions

[problem1 solution](problem1/)

[problem2 solution](problem2/)

[problem3 solution](problem3/)

## Build and test the application

[.NET Core SDK](https://www.microsoft.com/net/learn/get-started) is required to build all of the solutions. 

An alternative to installing the SDK is to use the [Docker build](#docker).


### Windows

Run the `build.bat` script in order to restore, build the application:

```
> ./build.bat
```

### Linux/macOS

Run the `build.sh` script in order to restore, build the application:

```
$ ./build.sh
```

## Running a program

After a successful build you can start an web application by executing the following command in your terminal:

```
dotnet run --project problem1
# or 
cd problem1
dotnet run
```

After the application has started visit [http://localhost:8080](http://localhost:8080) in your preferred browser. Or visit [http://159.203.122.126:8080](http://159.203.122.126:8080)

## Docker 
Run the `docker_run.sh` script in each of the problem directories to run the project build in a docker container. That script also runs the entrypoint and you can pass arguments to the program on the command line. 