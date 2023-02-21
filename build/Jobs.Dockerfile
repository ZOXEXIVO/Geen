ARG BUILD_NUMBER

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY ./src/Backend/ ./

WORKDIR /app/Geen.Jobs

RUN dotnet publish -c Release -o /app/out -r linux-x64 --self-contained true /p:PublishSingleFile=true --packages packages

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./Geen.Jobs"]
