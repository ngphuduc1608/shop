FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/proj_tt.Web.Mvc/proj_tt.Web.Mvc.csproj", "src/proj_tt.Web.Mvc/"]
COPY ["src/proj_tt.Web.Core/proj_tt.Web.Core.csproj", "src/proj_tt.Web.Core/"]
COPY ["src/proj_tt.Application/proj_tt.Application.csproj", "src/proj_tt.Application/"]
COPY ["src/proj_tt.Core/proj_tt.Core.csproj", "src/proj_tt.Core/"]
COPY ["src/proj_tt.EntityFrameworkCore/proj_tt.EntityFrameworkCore.csproj", "src/proj_tt.EntityFrameworkCore/"]
WORKDIR "/src/src/proj_tt.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/proj_tt.Web.Mvc", "src/proj_tt.Web.Mvc"]
COPY ["src/proj_tt.Web.Core", "src/proj_tt.Web.Core"]
COPY ["src/proj_tt.Application", "src/proj_tt.Application"]
COPY ["src/proj_tt.Core", "src/proj_tt.Core"]
COPY ["src/proj_tt.EntityFrameworkCore", "src/proj_tt.EntityFrameworkCore"]
WORKDIR "/src/src/proj_tt.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "proj_tt.Web.Mvc.dll"]
