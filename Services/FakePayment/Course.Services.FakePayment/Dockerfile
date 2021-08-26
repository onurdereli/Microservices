FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
# dockerların birbirleriyle haberleşmesi için gerekli olan port adresi
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
# nuget package'de olmadığından dolayı bunu manuel olarak işlem yapıyoruz
COPY Services/FakePayment/Course.Services.FakePayment/*.csproj Services/FakePayment/Course.Services.FakePayment/
RUN dotnet restore Services/FakePayment/Course.Services.FakePayment/*.csproj
# docker tarafında her komuta özgü bir katman oluşur ve bu katmanlarda cachelenir.
# Projenin static bir dosyasında değişiklik yapıldığında projenin genel kodlamasında değişiklik olmadığında üstteki dosyalar cacheden gelsin yeni dosyalar kopyalansın mantığında çalışmaktadır.
COPY . .
RUN dotnet publish Services/FakePayment/Course.Services.FakePayment/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
# build image'ından app/out dizinine public dosyalarını kopyala
ENTRYPOINT [ "dotnet","Course.Services.FakePayment.dll" ]