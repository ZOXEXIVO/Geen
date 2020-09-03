ARG BUILD_NUMBER

FROM geen-frontend:$BUILD_NUMBER as frontend

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build

WORKDIR /app

COPY ./src/Backend/ ./

WORKDIR /app/Geen.Web

COPY --from=frontend /app/dist wwwroot

RUN dotnet publish Geen.Web -c Release -o /app/out -r linux-musl-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true --packages packages

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1.7-alpine3.12 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./Geen.Web"]
