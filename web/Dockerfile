FROM node:17-alpine AS builder
ENV NODE_ENV production

WORKDIR /app

COPY package.json .
COPY package-lock.json .

RUN npm install --include=dev

COPY . .

RUN npm run build

FROM nginx:1.21.6-alpine AS production
ENV NODE_ENV production

COPY --from=builder /app/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
