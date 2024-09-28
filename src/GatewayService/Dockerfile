FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

#copy all .csproj and restore as distinct layers. use the same copy command 
#for every docker file in the project to take advantage of docker caching
COPY Leilao.sln Leilao.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

#restore package dependencies
RUN dotnet restore Leilao.sln

#copy the app folders over
COPY src/GatewayService src/GatewayService

#publish
WORKDIR /app/src/GatewayService

#Execute
RUN dotnet publish -c Release -o /app/src/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "GatewayService.dll" ]

#docker build -f src/AuctionService/Dockerfile -t testedocker123 .
#docker run -d testedocker123