FROM node:17-alpine3.14 AS build

WORKDIR /app

COPY ./src/Frontend/package.json .

RUN npm install --legacy-peer-deps

COPY ./src/Frontend/ .

RUN npm run build