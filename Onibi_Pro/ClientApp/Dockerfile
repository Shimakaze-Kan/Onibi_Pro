FROM node:latest as build
WORKDIR /usr/local/app
COPY ./ /usr/local/app
RUN npm install
RUN npm run build

FROM nginx:latest
# angular routing
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/local/app/dist/onibi-client-app /usr/share/nginx/html
EXPOSE 80