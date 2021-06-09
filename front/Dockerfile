FROM node:16-alpine
WORKDIR /app

COPY ./dist /app
RUN yarn global add serve
EXPOSE 5000

CMD ["serve", "-s", "/app"]