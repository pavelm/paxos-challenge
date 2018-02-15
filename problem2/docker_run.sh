# build docker image
docker build -t problem2 .

# run docker image
docker run --rm problem2 "$@" 
