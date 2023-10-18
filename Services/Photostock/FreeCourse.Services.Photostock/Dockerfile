FROM mcr.microsoft.com/dotnet/sdk:5.0 as DockerAspNetSdk
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Photostock/FreeCourse.Services.Photostock/*.csproj Services/Photostock/FreeCourse.Services.Photostock/
RUN dotnet restore Services/Photostock/FreeCourse.Services.Photostock/*.csproj
COPY . .
RUN dotnet publish Services/Photostock/FreeCourse.Services.Photostock/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=DockerAspNetSdk /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Services.Photostock.dll" ]
