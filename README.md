# Pokedex API

The Pokedex API makes use of the https://pokeapi.co/ to retrieve Pokemon related information and also https://funtranslations.com/api/ to apply fun translations of a Pokemon's description.

# Details
- The API was developed using .NET CORE (.NET 5), please ensure you have installed the latest .NET SDK and runtime https://dotnet.microsoft.com/download/dotnet/5.0

# How to run

**Visual Studio / JetBrains Rider IDE:**
- Install Visual Studio (https://visualstudio.microsoft.com/) or JetBrains Rider (https://www.jetbrains.com/rider/)
- Open Pokedex.sln and build the Pokedex.Api and Pokedex.Services projects then run the Pokedex.Api project application normally.

- Hit the endpoint through the auto-generated SwaggerUI at https://localhost:{port}/swagger/index.html (once the app is started)

- Alternatively, you can use a tool such as Postman and hit the endpoint: https://localhost:{port}/pokemon/{PokemonName} or https://localhost:{port}/pokemon/translated/{PokemonName} - you may need to switch off the SSL certification verification (under Settings) in order to hit the endpoint.

**Docker:**
- Install Docker environment (Docker Desktop / Docker Engine): https://www.docker.com/get-started
- Run the application in a container through the command: **docker-compose up --build** (execute in a command prompt/terminal at the solution level). This will run the application under port 8080 (browse http://localhost:8080/pokemon/{PokemonName} or http://localhost:8080/pokemon/translated/{PokemonName}.

- The SEQ logging UI will be available to browse on http://localhost:5341/ (Please note: events will only be logged when running/debugging the application through IDE - run SEQ separately (making sure no existing SEQ instance is running): **docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest**


# Testing

- The project contains unit test and integration test projects for the API.
- For the integration test, please ensure the API is running under http://localhost:8080 (in Docker container) before running the test locally.
- For test environments, I would like to setup stubbed responses using a tool such as Wiremock (http://wiremock.org/) in order to test against different responses easily.



# Production

- For production, the API would require authentication in order to access the endpoints in order to ensure authorised access. I would look to implement JWT authentication over Basic Authentication as this has several benefits such as: no need to pass in credentials, can specifically grant scopes to allow access to different endpoints for example.

- I would also look to implement caching on the APIs so that we do not require extra unnecessary requests hitting the backend services which are usually rate limited (translation API as an example).

- Configure CI/CD pipeline to automate the build and deployment process.

- Deploy API in a Platform As A Service (PAAS) such as Azure App Service/AWS Elastic Beanstalk, or alternatively to a Kubernetes cluster.

- Add enhanced logging and monitoring to diagnose any errors encountered in production such as using Splunk, Kibana/ElasticSearch etc.