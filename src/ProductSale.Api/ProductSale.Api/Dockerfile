FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ProductSale.Api/ProductSale.Api/ProductSale.Api.csproj", "ProductSale.Api/ProductSale.Api/"]
COPY ["src/ProductSale.Api/ProductSale.Data/ProductSale.Data.csproj", "ProductSale.Api/ProductSale.Data/"]

RUN dotnet restore "ProductSale.Api/ProductSale.Api/ProductSale.Api.csproj"

COPY . .

# Build
WORKDIR "/src/ProductSale.Api/ProductSale.Api"
RUN dotnet build "ProductSale.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductSale.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductSale.Api.dll"]