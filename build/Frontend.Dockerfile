FROM node:15.5.1-alpine3.10 AS build

WORKDIR /app

COPY ./src/Frontend/package.json .

RUN npm install

COPY ./src/Frontend/ .

RUN npm run build --prod