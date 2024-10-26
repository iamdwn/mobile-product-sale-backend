# Use the .NET 8.0 SDK for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory within the container
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["src/ProductSale.Api/ProductSale.Api/ProductSale.Api.csproj", "ProductSale.Api/"]
COPY ["src/ProductSale.Api/ProductSale.Data/ProductSale.Data.csproj", "ProductSale.Data/"]

# Restore dependencies
RUN dotnet restore "ProductSale.Api/ProductSale.Api.csproj"

# Copy all project files and build the application
COPY . .
WORKDIR "/src/ProductSale.Api"
RUN dotnet build "ProductSale.Api.csproj" -c Release -o /app/build

# Publish the application to a folder named /app/publish
RUN dotnet publish "ProductSale.Api.csproj" -c Release -o /app/publish

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published files from the build container
COPY --from=build /app/publish .

# Expose the port that your app will run on
EXPOSE 80
EXPOSE 443

# Run the app
ENTRYPOINT ["dotnet", "ProductSale.Api.dll"]
