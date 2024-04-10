#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ../AcmeCorp.Api/wait-for-it.sh ./
RUN chmod +x ./wait-for-it.sh

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#COPY ["./AcmeCorp.sln", "./"]
COPY ["AcmeCorp.Api/AcmeCorp.Api.csproj" "AcmeCorp.Api/"]
COPY ["./AcmeCorp.Domain/AcmeCorp.Domain.csproj", "./AcmeCorp.Domain/"]
COPY ["./AcmeCorp.Infrastructure/AcmeCorp.Infrastructure.csproj", "./AcmeCorp.Infrastructure/"]

COPY . .

WORKDIR /AcmeCorp.API
RUN dotnet restore "AcmeCorp.API.csproj"

WORKDIR /AcmeCorp.Domain
RUN dotnet restore "AcmeCorp.Domain.csproj"

WORKDIR /AcmeCorp.Infrastructure
RUN dotnet restore "AcmeCorp.Infrastructure.csproj"

COPY . .

WORKDIR /AcmeCorp.API
RUN dotnet build "AcmeCorp.API.csproj" -c %BUILD_CONFIGURATION% -o /app/build

WORKDIR /AcmeCorp.Domain
RUN dotnet build "AcmeCorp.Domain.csproj" -c %BUILD_CONFIGURATION% -o /app/build

WORKDIR /AcmeCorp.Infrastructure
RUN dotnet build "AcmeCorp.Infrastructure.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "/AcmeCorp.API/AcmeCorp.API.dll"]