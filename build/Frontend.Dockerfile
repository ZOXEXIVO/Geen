FROM node:15.14.0-alpine3.13 AS build

WORKDIR /app

COPY ./src/Frontend/package.json .

RUN npm install --legacy-peer-deps

COPY ./src/Frontend/ .

RUN npm run build --prod