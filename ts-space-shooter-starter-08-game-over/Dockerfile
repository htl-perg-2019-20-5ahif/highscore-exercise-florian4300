FROM node as build-stage

EXPOSE 8080

WORKDIR /app

COPY package*.json /app/

RUN npm install

COPY ./ /app/

RUN npm run build -- --output-path=./dist/out

FROM nginx

COPY --from=build-stage /app/dist/out/ /usr/share/nginx/html
COPY --from=build-stage /app/nginx.conf /etc/nginx/conf.d/default.conf

