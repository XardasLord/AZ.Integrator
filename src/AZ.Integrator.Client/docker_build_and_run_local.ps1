# Builds and runs docker image on local PC
docker build -f Dockerfile.dev -t az-integrator-client:dev .
docker run -d -it -p 8001:80/tcp --name az-integrator-client az-integrator-client:dev
