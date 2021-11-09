ARG BUILD_NUMBER

FROM geen-frontend:$BUILD_NUMBER as frontend

FROM mcr.microsoft.com/dotnet/sdk:6.0.100-alpine3.14-amd64 AS build

WORKDIR /app

COPY ./src/Backend/ ./

WORKDIR /app/Geen.Web

COPY --from=frontend /app/dist wwwroot

RUN dotnet publish -c Release -o /app/out -r linux-musl-x64 --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true /p:PublishTrimmed=true --packages packages

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.0-alpine3.14-amd64 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./Geen.Web"]
