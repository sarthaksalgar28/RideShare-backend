# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj first to leverage Docker cache
COPY ["RideShare/RideShare.csproj", "RideShare/"]
RUN dotnet restore "./RideShare/RideShare.csproj"

# Now copy the rest of the project files and build
COPY . .
WORKDIR "/src/RideShare"
RUN dotnet build "./RideShare.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./RideShare.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the port for your app
EXPOSE 80

ENTRYPOINT ["dotnet", "RideShare.dll"]
