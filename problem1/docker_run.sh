# build docker image
docker build -t problem1 .

# run docker image
docker run -d -p 8080:8080 --rm problem1 