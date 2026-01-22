.PHONY: help restore build build-server build-client run-server run-client run-all

# Config
CONFIG ?= Debug
DOTNET ?= dotnet
PWSH ?= pwsh
RUN_SERVER_BOOT_SECONDS ?= 2

# Projects (relative to repo root)
SOLUTION := Legend of Mir.sln
SERVER_PROJECT := Server.MirForms/Server.csproj
CLIENT_PROJECT := Client/Client.csproj

help:
	@echo ""
	@echo "Crystal dev commands"
	@echo ""
	@echo "Usage:"
	@echo "  make build            Restore + build solution ($(CONFIG))"
	@echo "  make run-server       Build + run server ($(SERVER_PROJECT))"
	@echo "  make run-client       Build + run client ($(CLIENT_PROJECT))"
	@echo "  make run-all          Build + start server and client (separate processes)"
	@echo ""
	@echo "Options:"
	@echo "  CONFIG=Debug|Release"
	@echo "  RUN_SERVER_BOOT_SECONDS=2"
	@echo ""

restore:
	"$(DOTNET)" restore "$(SOLUTION)"

build: restore
	"$(DOTNET)" build "$(SOLUTION)" -c "$(CONFIG)"

build-server: restore
	"$(DOTNET)" build "$(SERVER_PROJECT)" -c "$(CONFIG)"

build-client: restore
	"$(DOTNET)" build "$(CLIENT_PROJECT)" -c "$(CONFIG)"

run-server: build-server
	"$(DOTNET)" run --project "$(SERVER_PROJECT)" -c "$(CONFIG)" --no-build

run-client: build-client
	"$(DOTNET)" run --project "$(CLIENT_PROJECT)" -c "$(CONFIG)" --no-build

run-all: build-server build-client
	@$(PWSH) -NoProfile -ExecutionPolicy Bypass -Command "$$ErrorActionPreference='Stop'; $$root=(Resolve-Path '.').Path; Start-Process '$(DOTNET)' -WorkingDirectory $$root -ArgumentList @('run','--no-build','-c','$(CONFIG)','--project','$(SERVER_PROJECT)'); Start-Sleep -Seconds $(RUN_SERVER_BOOT_SECONDS); Start-Process '$(DOTNET)' -WorkingDirectory $$root -ArgumentList @('run','--no-build','-c','$(CONFIG)','--project','$(CLIENT_PROJECT)')"
