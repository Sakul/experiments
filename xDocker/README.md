# Pull an image
```
docker pull getting-started
```

# Run an image
```
docker run -dp 80:80 docker/getting-started
```
> -p Publish a container's port(s) to the host  
> -d Run container in background and print container ID

# Images
```
docker images
```
> -a Show all images  
> -q Only display numeric IDs

# Containers
```
docker ps
```
> -a Show all containers  
> -q Only display numeric IDs

# Stop all running containers
```
docker stop $(docker ps -a -q)
```

# Delete all stopped containers
```
docker rm $(docker ps -a -q)
```

# Remove all images at once
```
docker rmi $(docker images -q)
```