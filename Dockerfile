#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
ENV ASPNETCORE_URLS=https://+:5005 
WORKDIR /app

COPY . ./
WORKDIR /app/Fitverse.AgreementsService
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
EXPOSE 5005
COPY --from=build-env /app/Fitverse.AgreementsService/out .
ENTRYPOINT ["dotnet", "Fitverse.AgreementsService.dll"]

