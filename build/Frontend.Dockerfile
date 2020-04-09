FROM node:12.7-alpine AS build

WORKDIR /app

COPY ./src/Frontend/package.json .

RUN npm install

COPY ./src/Frontend/ .

RUN npm run build --prod