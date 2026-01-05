FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoursesPlatform.Api/CoursesPlatform.Api.csproj", "CoursesPlatform.Api/"]
COPY ["1-Application/1-Application.csproj", "1-Application/"]
COPY ["2-Domain/2-Domain.csproj", "2-Domain/"]
COPY ["3-Infrastructure/3-Infrastructure.csproj", "3-Infrastructure/"]
RUN dotnet restore "CoursesPlatform.Api/CoursesPlatform.Api.csproj"
COPY . .
WORKDIR "/src/CoursesPlatform.Api"
RUN dotnet build "./CoursesPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoursesPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoursesPlatform.Api.dll"]
