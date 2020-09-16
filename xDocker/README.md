# Images
### Pull an image
```
docker pull getting-started
```

### Run a command in a new container
```
docker run -dp 80:80 docker/getting-started
```
> -p Publish a container's port(s) to the host  
> -d Run container in background and print container ID

### List images
```
docker images
```
> -a Show all images  
> -q Only display numeric IDs

# Containers
### List containers
```
docker ps
```
> -a Show all containers  
> -q Only display numeric IDs

### Stop all running containers
```
docker stop $(docker ps -a -q)
```

### Delete all stopped containers
```
docker rm $(docker ps -a -q)
```

### Remove all images at once
```
docker rmi $(docker images -q)
```

### Shell access
```
docker exec -it CONTAINER_NAME bash
```
> -i	Keep STDIN open even if not attached  
> -t	Allocate a pseudo-TTY

### Logging
```
docker logs CONTAINER_NAME
```

# Compose
### Compose command
```
docker-compose up
```
> -f	Specify an alternate compose file (default: docker-compose.yml)  
> up	Create and start containers  