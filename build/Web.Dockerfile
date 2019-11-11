FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./ ./

WORKDIR /app/src/Geen.Web/App

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - && apt-get install -yq nodejs
RUN npm install

RUN npm run build

WORKDIR /app/src/Geen.Web

RUN dotnet publish -c Release -o out -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/Geen.Web/out .
ENTRYPOINT ["./Geen.Web"]
