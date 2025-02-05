# Use the official Microsoft .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["RideShare/RideShare.csproj", "RideShare/"]
RUN dotnet restore "RideShare/RideShare.csproj"
COPY . .
WORKDIR "/src/RideShare"
RUN dotnet build "RideShare.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "RideShare.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RideShare.dll"]
