# pokemonapi with fun translations
## RESTful API with ASP.NET Core 3

DOCKER
Open a terminal and run the following to download the image from the docker hub and run the container 
docker run --rm -p 5000:5000/tcp konstas/kubia:pokemonapi

ENDPOINTS
- http://localhost:5000/pokemon/translated/mewtwo
- http://localhost:5000/pokemon/mewtwo
- http://localhost:5000/swagger/ OpenAPI (You can use it for manual testing)

### More improvements (for production release)
- Writing and automating API Tests with Postman and Newman
- Adding versioning
- Adding security if required (Authentication, access control)
- Adding audit control (logging)
- Adding rate limiting and throttling
- Adding caching 
- Adding ETags for concurrency handling (race conditions) 
- Improve documentation
