FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Presentation/CustomForms/CustomForms.csproj", "Presentation/CustomForms/"]
COPY ["Infrastructure/CustomForms.Persistence/CustomForms.Persistence.csproj", "Infrastructure/CustomForms.Persistence/"]
COPY ["Infrastructure/CustomForms.Infrastructure/CustomForms.Infrastructure.csproj", "Infrastructure/CustomForms.Infrastructure/"]
COPY ["Core/CustomForms.Domain/CustomForms.Domain.csproj", "Core/CustomForms.Domain/"]
COPY ["Core/CustomForms.Application/CustomForms.Application.csproj", "Core/CustomForms.Application/"]
RUN dotnet restore "Presentation/CustomForms/CustomForms.csproj"

COPY . .
WORKDIR "/src/Presentation/CustomForms"
RUN dotnet build "CustomForms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomForms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomForms.dll"]