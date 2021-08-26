FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
# dockerların birbirleriyle haberleşmesi için gerekli olan port adresi
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
# nuget package'de olmadığından dolayı bunu manuel olarak işlem yapıyoruz
COPY IdentityServer/Course.IdentityServer/*.csproj IdentityServer/Course.IdentityServer/
RUN dotnet restore IdentityServer/Course.IdentityServer/*.csproj
# docker tarafında her komuta özgü bir katman oluşur ve bu katmanlarda cachelenir.
# Projenin static bir dosyasında değişiklik yapıldığında projenin genel kodlamasında değişiklik olmadığında üstteki dosyalar cacheden gelsin yeni dosyalar kopyalansın mantığında çalışmaktadır.
COPY . .
# geri kalan dosyaları kopyalar
RUN dotnet publish IdentityServer/Course.IdentityServer/*.csproj -c Release -o out
# kopyalanan release modda out klasörüne çıktısı alınır
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
# build image'ından app/out dizinine public dosyalarını kopyala
ENTRYPOINT [ "dotnet","Course.IdentityServer.dll" ]
