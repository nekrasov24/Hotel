FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app

COPY "Scheduler.csproj" "Scheduler.csproj"
RUN dotnet restore "Scheduler.csproj"

COPY . .

FROM build as publish
WORKDIR /publish

COPY --from=build /app .
RUN dotnet publish "Scheduler.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app

COPY --from=publish /publish/out .

ENTRYPOINT [ "dotnet", "Scheduler.dll" ]