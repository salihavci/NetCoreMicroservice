FROM mcr.microsoft.com/dotnet/sdk:5.0 as DockerAspNetSdk
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Gateways/FreeCourse.Gateway/*.csproj Services/Gateways/FreeCourse.Gateway/
RUN dotnet restore Services/Gateways/FreeCourse.Gateway/*.csproj
COPY . .
RUN dotnet publish Services/Gateways/FreeCourse.Gateway/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=DockerAspNetSdk /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Gateway.dll" ]
