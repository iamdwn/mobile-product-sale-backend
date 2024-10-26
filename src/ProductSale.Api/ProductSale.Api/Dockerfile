# Set base image for building the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["src/ProductSale.Api/ProductSale.Api/ProductSale.Api.csproj", "ProductSale.Api/"]
COPY ["src/ProductSale.Api/ProductSale.Data/ProductSale.Data.csproj", "ProductSale.Data/"]
RUN dotnet restore "./ProductSale.Api/ProductSale.Api.csproj"

# Copy the entire project files and build the application
COPY . .
WORKDIR /src/ProductSale.Api
RUN dotnet build "ProductSale.Api.csproj" -c $BUILD_CONFIGURATION --no-restore
RUN dotnet publish "ProductSale.Api.csproj" -c $BUILD_CONFIGURATION -o /app --no-restore

# Set the base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ProductSale.Api.dll"]
