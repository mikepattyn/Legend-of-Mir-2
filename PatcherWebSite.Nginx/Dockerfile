FROM nginx:alpine-slim

# Nginx site config
COPY PatcherWebSite.Nginx/default.conf /etc/nginx/conf.d/default.conf

COPY PatcherWebSite /usr/share/nginx/html

EXPOSE 80
