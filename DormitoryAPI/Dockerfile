# GIAI ĐOẠN 1: Build (Dùng bộ SDK để biên dịch code)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# --- BẠN CẦN SỬA TÊN FILE Ở DÒNG DƯỚI ---
# Hãy thay "DormitoryAPI.csproj" bằng tên file project thật của bạn
COPY ["DormitoryAPI.csproj", "./"]
RUN dotnet restore "DormitoryAPI.csproj"

COPY . .
# --- BẠN CẦN SỬA TÊN FILE Ở DÒNG DƯỚI ---
RUN dotnet publish "DormitoryAPI.csproj" -c Release -o /app/publish

# GIAI ĐOẠN 2: Run (Dùng bộ runtime nhẹ để chạy ứng dụng)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Cấu hình để Render nhận đúng cổng
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# --- BẠN CẦN SỬA TÊN FILE Ở DÒNG DƯỚI ---
# Lưu ý: Lúc này đuôi file là .dll chứ không phải .csproj nữa
ENTRYPOINT ["dotnet", "DormitoryAPI.dll"]