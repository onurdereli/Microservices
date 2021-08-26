FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
# dockerların birbirleriyle haberleşmesi için gerekli olan port adresi
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
# nuget package'de olmadığından dolayı bunu manuel olarak işlem yapıyoruz
COPY Services/Order/Course.Services.Order.Application/*.csproj Services/Order/Course.Services.Order.Application/
COPY Services/Order/Course.Services.Order.Domain/*.csproj Services/Order/Course.Services.Order.Domain/
COPY Services/Order/Course.Services.Order.Domain.Core/*.csproj Services/Order/Course.Services.Order.Domain.Core/
COPY Services/Order/Course.Services.Order.Instrastructure/*.csproj Services/Order/Course.Services.Order.Instrastructure/
COPY Services/Order/Course.Services.Order.API/*.csproj Services/Order/Course.Services.Order.API/
# her bir katman ayrı library olmasının avantajı bir libraryde yapılan değişiklik diğerlerini bu şekilde copy yaparak etkilemeyecek cacheden gelecek
RUN dotnet restore Services/Order/Course.Services.Order.API/*.csproj
# docker tarafında her komuta özgü bir katman oluşur ve bu katmanlarda cachelenir.
# Projenin static bir dosyasında değişiklik yapıldığında projenin genel kodlamasında değişiklik olmadığında üstteki dosyalar cacheden gelsin yeni dosyalar kopyalansın mantığında çalışmaktadır.
COPY . .
RUN dotnet publish Services/Order/Course.Services.Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
# build image'ından app/out dizinine public dosyalarını kopyala
ENTRYPOINT [ "dotnet","Course.Services.Order.API.dll" ]