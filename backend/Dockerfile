FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o build --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

WORKDIR /app

RUN groupadd --gid 1000 appuser && \
    useradd --gid 1000 --uid 1000 --create-home --no-log-init appuser && \
    chown 1000:1000 .

USER 1000:1000

COPY --from=build --chown=1000:1000 /app/build .

EXPOSE 80
VOLUME [ "/app/keys" ]

ENTRYPOINT [ "./CbtBackend" ]
