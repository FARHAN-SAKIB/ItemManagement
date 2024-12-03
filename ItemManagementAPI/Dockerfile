# Use the .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

# Copy and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the code and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copy the published app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 5284
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5284;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Development

# Start the app
ENTRYPOINT ["dotnet", "ItemManagementAPI.dll"]
