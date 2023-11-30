docker network create onibi-network
docker run -d --name onibi-redis-container -p 6379:6379 --network onibi-network redis:latest
docker run -d --name onibi-app -e ASPNETCORE_ENVIRONMENT=Production -p 8080:80 --network onibi-network onibi-pro:lastest
docker run -d --name onibi-client -p 8081:80 --network onibi-network onibi-client-app:latest


