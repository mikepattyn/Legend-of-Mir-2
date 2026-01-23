# How to Docker Guide

This guide will help you install Docker on your system and run the PatcherWebSite using Docker and Docker Compose.

## Table of Contents

- [Installing Docker](#installing-docker)
  - [Windows](#windows)
  - [macOS](#macos)
  - [Linux](#linux)
- [Building and Running](#building-and-running)
  - [Using Docker Compose (Recommended)](#using-docker-compose-recommended)
  - [Using Docker Directly](#using-docker-directly)
- [Troubleshooting](#troubleshooting)

---

## Installing Docker

### Windows

#### Option 1: Docker Desktop (Recommended)

1. **Download Docker Desktop**
   - Visit [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/)
   - Download the installer (Docker Desktop Installer.exe)

2. **System Requirements**
   - Windows 10 64-bit: Pro, Enterprise, or Education (Build 19041 or higher)
   - Windows 11 64-bit: Home or Pro version 21H2 or higher
   - WSL 2 feature enabled
   - Virtualization enabled in BIOS

3. **Installation Steps**
   - Run the installer
   - Follow the installation wizard
   - When prompted, ensure "Use WSL 2 instead of Hyper-V" is checked (recommended)
   - Restart your computer when prompted

4. **Verify Installation**
   - Open Docker Desktop from the Start menu
   - Wait for Docker to start (whale icon in system tray)
   - Open PowerShell or Command Prompt and run:
     ```powershell
     docker --version
     docker-compose --version
     ```

#### Option 2: WSL 2 with Docker Engine

If you prefer not to use Docker Desktop:

1. **Install WSL 2**
   ```powershell
   wsl --install
   ```

2. **Install Docker in WSL 2**
   - Follow the [Linux installation instructions](#linux) within your WSL 2 distribution

---

### macOS

#### Option 1: Docker Desktop (Recommended)

1. **Download Docker Desktop**
   - Visit [Docker Desktop for Mac](https://www.docker.com/products/docker-desktop/)
   - Choose the version for your Mac:
     - **Apple Silicon (M1/M2/M3)**: Download "Mac with Apple Silicon"
     - **Intel**: Download "Mac with Intel chip"

2. **Installation Steps**
   - Open the downloaded `.dmg` file
   - Drag Docker to your Applications folder
   - Open Docker from Applications
   - Follow the setup wizard
   - Enter your password when prompted to install networking components

3. **Verify Installation**
   - Open Terminal and run:
     ```bash
     docker --version
     docker-compose --version
     ```

#### Option 2: Homebrew

```bash
brew install --cask docker
```

Then open Docker Desktop from Applications.

---

### Linux

#### Ubuntu/Debian

1. **Update package index**
   ```bash
   sudo apt-get update
   ```

2. **Install prerequisites**
   ```bash
   sudo apt-get install \
       ca-certificates \
       curl \
       gnupg \
       lsb-release
   ```

3. **Add Docker's official GPG key**
   ```bash
   sudo mkdir -p /etc/apt/keyrings
   curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
   ```

4. **Set up the repository**
   ```bash
   echo \
     "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
     $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
   ```

5. **Install Docker Engine**
   ```bash
   sudo apt-get update
   sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
   ```

6. **Start Docker**
   ```bash
   sudo systemctl start docker
   sudo systemctl enable docker
   ```

7. **Add your user to the docker group** (optional, to run Docker without sudo)
   ```bash
   sudo usermod -aG docker $USER
   ```
   - Log out and log back in for this to take effect

8. **Verify Installation**
   ```bash
   docker --version
   docker compose version
   ```

#### Fedora/RHEL/CentOS

1. **Install prerequisites**
   ```bash
   sudo dnf install -y dnf-plugins-core
   ```

2. **Add Docker repository**
   ```bash
   sudo dnf config-manager --add-repo https://download.docker.com/linux/fedora/docker-ce.repo
   ```

3. **Install Docker Engine**
   ```bash
   sudo dnf install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
   ```

4. **Start Docker**
   ```bash
   sudo systemctl start docker
   sudo systemctl enable docker
   ```

5. **Add your user to the docker group** (optional)
   ```bash
   sudo usermod -aG docker $USER
   ```

6. **Verify Installation**
   ```bash
   docker --version
   docker compose version
   ```

#### Arch Linux

```bash
sudo pacman -S docker docker-compose
sudo systemctl start docker
sudo systemctl enable docker
sudo usermod -aG docker $USER
```

---

## Building and Running

### Using Docker Compose (Recommended)

Docker Compose is the easiest way to build and run the PatcherWebSite service.

1. **Navigate to the project root**
   ```bash
   cd /path/to/Crystal
   ```

2. **Build and start the service**
   ```bash
   docker-compose up --build
   ```
   
   Or to run in detached mode (background):
   ```bash
   docker-compose up --build -d
   ```

3. **Access the website**
   - Open your browser and navigate to: `http://localhost:8080`
   - The service maps port 8080 on your host to port 80 in the container

4. **Stop the service**
   ```bash
   docker-compose down
   ```

5. **View logs**
   ```bash
   docker-compose logs -f
   ```

6. **Rebuild after changes**
   ```bash
   docker-compose up --build
   ```

### Using Docker Directly

If you prefer to use Docker commands directly:

1. **Build the image**
   ```bash
   docker build -t patchsite -f PatcherWebSite.Nginx/Dockerfile .
   ```

2. **Run the container**
   ```bash
   docker run -d -p 8080:80 --name patchsite patchsite
   ```

3. **Access the website**
   - Open your browser and navigate to: `http://localhost:8080`

4. **Stop the container**
   ```bash
   docker stop patchsite
   ```

5. **Remove the container**
   ```bash
   docker rm patchsite
   ```

6. **View logs**
   ```bash
   docker logs -f patchsite
   ```

---

## Troubleshooting

### Port Already in Use

If you get an error that port 8080 is already in use:

**Option 1: Change the port in docker-compose.yml**
```yaml
ports:
  - "8081:80"  # Change 8080 to any available port
```

**Option 2: Stop the service using the port**
- On Windows: Use Resource Monitor or Task Manager to find the process
- On Linux/Mac: `lsof -i :8080` or `netstat -tulpn | grep 8080`

### Permission Denied (Linux)

If you get permission denied errors on Linux:

```bash
sudo usermod -aG docker $USER
# Log out and log back in
```

Or run commands with `sudo`:
```bash
sudo docker-compose up --build
```

### Docker Desktop Not Starting (Windows/Mac)

1. **Check WSL 2 (Windows)**
   ```powershell
   wsl --status
   ```
   If WSL 2 is not the default, update it:
   ```powershell
   wsl --set-default-version 2
   ```

2. **Restart Docker Desktop**
   - Right-click the Docker icon in the system tray
   - Select "Restart Docker Desktop"

3. **Check system requirements**
   - Ensure virtualization is enabled in BIOS
   - Ensure Hyper-V (Windows) or Hypervisor framework (Mac) is available

### Build Fails

If the build fails:

1. **Check Dockerfile path**
   - Ensure you're running commands from the project root
   - Verify `PatcherWebSite.Nginx/Dockerfile` exists

2. **Check source files**
   - Ensure `PatcherWebSite` directory exists
   - Ensure `PatcherWebSite.Nginx/default.conf` exists

3. **Clean and rebuild**
   ```bash
   docker-compose down
   docker-compose build --no-cache
   docker-compose up
   ```

### Container Exits Immediately

1. **Check logs**
   ```bash
   docker-compose logs patchsite
   ```

2. **Run interactively to debug**
   ```bash
   docker run -it --rm patchsite sh
   ```

### View Container Status

```bash
docker-compose ps
```

Or with Docker directly:
```bash
docker ps -a
```

---

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [Docker Desktop Documentation](https://docs.docker.com/desktop/)

---

## Quick Reference

| Command | Description |
|---------|-------------|
| `docker-compose up --build` | Build and start services |
| `docker-compose up -d` | Start services in background |
| `docker-compose down` | Stop and remove services |
| `docker-compose logs -f` | View logs (follow mode) |
| `docker-compose ps` | List running services |
| `docker-compose restart` | Restart services |
| `docker ps` | List running containers |
| `docker images` | List images |
| `docker logs <container>` | View container logs |
