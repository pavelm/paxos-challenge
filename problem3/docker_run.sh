# build docker image
docker build -t problem3 .

# run docker image
docker run --rm problem3 "$@" 
