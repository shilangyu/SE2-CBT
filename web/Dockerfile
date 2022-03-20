FROM node:17-alpine AS builder

ARG VITE_API_URL
ENV VITE_API_URL $VITE_API_URL
ENV NODE_ENV production

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install --include=dev

COPY . .
RUN npm run build


FROM nginx:1.21-alpine

ENV NODE_ENV production

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
