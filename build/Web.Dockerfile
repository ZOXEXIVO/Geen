FROM node:19-alpine3.16 AS build-frontend

WORKDIR /app

COPY ./src/Frontend/package.json .

RUN npm install --legacy-peer-deps

COPY ./src/Frontend/ .

RUN npm run build

ARG BUILD_NUMBER

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend

WORKDIR /app

COPY ./src/Backend/ ./

WORKDIR /app/Geen.Web

RUN dotnet publish -c Release -o /app/out -r linux-x64 --self-contained true /p:PublishSingleFile=true --packages packages

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS runtime
WORKDIR /app

COPY --from=build-backend /app/out .
COPY --from=build-frontend /app/dist wwwroot

ENTRYPOINT ["./Geen.Web"]
