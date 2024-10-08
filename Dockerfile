FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /app
EXPOSE 80
EXPOSE 433

# Copy solution
COPY ./*.sln  ./

# Copy the main source project files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Restore dependencies
RUN dotnet restore src/CaravelTemplate.Api

# Copy everything else and build
COPY . .

# Publish application in Release
WORKDIR /app/src/CaravelTemplate.Api

RUN dotnet publish -c Release -o dist

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=builder /app/src/CaravelTemplate.Api/dist .

ENTRYPOINT ["dotnet", "CaravelTemplate.Api.dll"]
