FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR ShiftService

EXPOSE 80
EXPOSE 3045

#Copy proyect file
copy ./*.csproj ./
RUN dotnet restore
copy . .
RUN dotnet publish -c Release -o out

#build image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /ShiftService
COPY --from=build /ShiftService/out .
ENTRYPOINT ["dotnet","ShiftService.dll"]
