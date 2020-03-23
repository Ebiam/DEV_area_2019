FROM mcr.microsoft.com/dotnet/core/sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN ls
RUN dotnet add package Microsoft.EntityFrameworkCore.Analyzers --version 3.1.2
RUN dotnet build /restore Area_api.sln
RUN dotnet add package Microsoft.EntityFrameworkCore.Analyzers --version 3.1.2
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/database.db .
ENTRYPOINT ["dotnet", "Area_api.dll"]

