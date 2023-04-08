FROM mcr.microsoft.com/dotnet/sdk:5.0 as DockerAspNetSdk
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Basket/FreeCourse.Services.Basket/*.csproj Services/Basket/FreeCourse.Services.Basket/
RUN dotnet restore Services/Basket/FreeCourse.Services.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/FreeCourse.Services.Basket/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=DockerAspNetSdk /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Services.Basket.dll" ]
