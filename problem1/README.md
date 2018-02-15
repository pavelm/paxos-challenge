# Solution

Straight forward implementation of computing the SHA256 hash using library functions. 

## Performance

The major bottleneck is the CPU computing the SHA256 hash, the more users start using the service, the more CPU this will take and eventually could start starving out users. 

I ended up storing the state in memory using a dictionary, but in reality this should be store in a more durable storage such as Azure Blob storage or AWS S3 or some distributed datastore (e.g. Redis), where the key would be the computed hash and the content would be the message. 

The microservices can be scaled horizontally and run behind a load balancer to even the load and storage of hash -> message mapping would be partitioned by the hash and taken care of by the storage mechanism.


## Build and test the application

### Linux/macOS

Run the `run.sh` script in order to restore, build, and run the application:

```
$ ./run.sh
```

After the application has started visit [http://localhost:8080](http://localhost:8080) in your preferred browser. Or visit [http://159.203.122.126:8080](http://159.203.122.126:8080)

Examples

```
curl -X POST -H "Content-Type: application/json" -d '{"message": "foo"}' http://159.203.122.126:8080/messages
{"digest":"2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae"}


curl -i  http://159.203.122.126:8080/messages/2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae
HTTP/1.1 200 OK
Date: Thu, 15 Feb 2018 14:14:17 GMT
Content-Type: application/json
Server: Kestrel
Content-Length: 17

{"message":"foo"}
```

## Docker 
Run the `docker_run.sh` script to run the project build in a docker container. It will also launch the web server and expose port 8080.