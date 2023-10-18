FROM mcr.microsoft.com/dotnet/sdk:5.0 as DockerAspNetSdk
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Discount/FreeCourse.Services.Discount/*.csproj Services/Discount/FreeCourse.Services.Discount/
RUN dotnet restore Services/Discount/FreeCourse.Services.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/FreeCourse.Services.Discount/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=DockerAspNetSdk /app/out .
ENTRYPOINT [ "dotnet", "FreeCourse.Services.Discount.dll" ]
