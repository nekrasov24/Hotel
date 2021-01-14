FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app

COPY "BookingService.csproj" "BookingService.csproj"
RUN dotnet restore "BookingService.csproj"

COPY . .

FROM build as publish
WORKDIR /publish

COPY --from=build /app .
RUN dotnet publish "BookingService.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app

COPY --from=publish /publish/out .

ENTRYPOINT [ "dotnet", "BookingService.dll" ]