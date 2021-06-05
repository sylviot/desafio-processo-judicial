FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . /src
RUN dotnet restore /src/api.csproj
RUN dotnet build /src/api.csproj -c Release -o /app/build
RUN dotnet publish /src/api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "api.dll"]