FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy file dự án của TẤT CẢ các lớp (Layer)
# Giúp Docker nhận diện được cấu trúc dependencies
COPY ["DormitoryAPI/DormitoryAPI.csproj", "DormitoryAPI/"]
COPY ["Dormitory.BUS/Dormitory.BUS.csproj", "Dormitory.BUS/"]
COPY ["Dormitory.DAO/Dormitory.DAO.csproj", "Dormitory.DAO/"]
COPY ["Dormitory.DTO/Dormitory.DTO.csproj", "Dormitory.DTO/"]
# Nếu có lớp Models riêng thì bỏ comment dòng dưới:
# COPY ["Dormitory.Models/Dormitory.Models.csproj", "Dormitory.Models/"]

# 2. Restore dựa trên file API chính
RUN dotnet restore "DormitoryAPI/DormitoryAPI.csproj"

# 3. Copy toàn bộ Source Code của tất cả các lớp vào
COPY . .

# 4. Build và Publish
WORKDIR "/src/DormitoryAPI"
RUN dotnet publish "DormitoryAPI.csproj" -c Release -o /app/publish

# Giai đoạn chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "DormitoryAPI.dll"]