FROM nginx:alpine

# Nginx site config
COPY PatcherWebSite.Nginx/default.conf /etc/nginx/conf.d/default.conf

# Serve the existing patch site at /patchsite/ (and keep /mir2-patchsite/ as an alias)
# (Build with repo root as context: `docker build -f PatcherWebSite.Nginx/Dockerfile .`)
COPY PatcherWebSite/mir2-patchsite /usr/share/nginx/html
RUN ln -s patchsite /usr/share/nginx/html/mir2-patchsite

EXPOSE 80
