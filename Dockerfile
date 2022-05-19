FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/StrEnum.AspNetCore/StrEnum.AspNetCore.csproj ./src/StrEnum.AspNetCore/StrEnum.AspNetCore.csproj
COPY test/StrEnum.AspNetCore.IntegrationTests/StrEnum.AspNetCore.IntegrationTests.csproj ./test/StrEnum.AspNetCore.IntegrationTests/StrEnum.AspNetCore.IntegrationTests.csproj
RUN dotnet restore

# copy everything else and build app
COPY ./ ./
WORKDIR /source
RUN dotnet build -c release -o /out/package --no-restore /p:maxcpucount=1

FROM build as test
RUN dotnet test /p:maxcpucount=1

FROM build as pack-and-push
WORKDIR /source

ARG PackageVersion
ARG NuGetApiKey

RUN dotnet pack ./src/StrEnum.AspNetCore/StrEnum.AspNetCore.csproj -o /out/package -c Release
RUN dotnet nuget push /out/package/StrEnum.AspNetCore.$PackageVersion.nupkg -k $NuGetApiKey -s https://api.nuget.org/v3/index.json