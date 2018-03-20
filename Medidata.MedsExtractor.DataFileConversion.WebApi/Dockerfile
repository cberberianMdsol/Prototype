FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Medidata.MedsExtractor.DataFileConversion.WebApi/Medidata.MedsExtractor.DataFileConversion.WebApi.csproj Medidata.MedsExtractor.DataFileConversion.WebApi/
COPY Medidata.MedsExtractor.DataFileConversion.Contracts/Medidata.MedsExtractor.DataFileConversion.Contracts.csproj Medidata.MedsExtractor.DataFileConversion.Contracts/
COPY Medidata.MedsExtractor.DataFileTransformEngine/Medidata.MedsExtractor.DataFileTransformEngine.csproj Medidata.MedsExtractor.DataFileTransformEngine/
COPY Medidata.MedsExtractor.CacheAccess/Medidata.MedsExtractor.CacheAccess.csproj Medidata.MedsExtractor.CacheAccess/
COPY Medidata.MedsExtractor.FileAccess/Medidata.MedsExtractor.FileAccess.csproj Medidata.MedsExtractor.FileAccess/
COPY Medidata.MedsExtractor.DataFileConversionManager/Medidata.MedsExtractor.DataFileConversionManager.csproj Medidata.MedsExtractor.DataFileConversionManager/
RUN dotnet restore
COPY . .
WORKDIR /src/Medidata.MedsExtractor.DataFileConversion.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Medidata.MedsExtractor.DataFileConversion.WebApi.dll"]
