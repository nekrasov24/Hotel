FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app

COPY "test1.csproj" "test1.csproj"
RUN dotnet restore "test1.csproj"

COPY . .

FROM build as publish
WORKDIR /publish

COPY --from=build /app .
RUN dotnet publish "test1.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app

COPY --from=publish /publish/out .

ENTRYPOINT [ "dotnet", "test1.dll" ]