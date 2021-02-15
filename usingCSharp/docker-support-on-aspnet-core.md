# DockerSupportから生成されるDockerfileのImage Tag名を確認していく。
## 前提条件
今回はubuntu 20用にセットアップしたい。

## DockerSupportから生成されるDockerfile(吊るし状態)
```
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebApplicationTest/WebApplicationTest.csproj", "WebApplicationTest/"]
RUN dotnet restore "WebApplicationTest/WebApplicationTest.csproj"
COPY . .
WORKDIR "/src/WebApplicationTest"
RUN dotnet build "WebApplicationTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplicationTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationTest.dll"]
```

## 直したいところ
<b>〇ASP NET Core Runtime</b>
```
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
```
上記だとDebian 10 なので、ubuntu 20に設定したい。

<b>〇 .NET SDK</b>
```
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
```
上記だとDebian 10 なので、ubuntu 20に設定したい。

## Image Tagの確認
### ASP.NET Core Runtime(実行環境用)
[https://hub.docker.com/_/microsoft-dotnet-aspnet/]

### .NET SDK（build環境用）
[https://hub.docker.com/_/microsoft-dotnet-sdk/]

