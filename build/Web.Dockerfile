ARG BUILD_NUMBER

FROM geen-frontend:$BUILD_NUMBER as frontend

FROM mcr.microsoft.com/dotnet/sdk:5.0.300-alpine3.13-amd64 AS build

WORKDIR /app

COPY ./src/Backend/ ./

WORKDIR /app/Geen.Web

COPY --from=frontend /app/dist wwwroot

RUN dotnet publish -c Release -o /app/out -r linux-musl-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true --packages packages

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.6-alpine3.13-amd64 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./Geen.Web"]
