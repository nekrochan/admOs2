**Сетевые настройки и дополнительные установки**

```
sudo apt update
sudo apt install openssh-server
sudo systemctl status shh
sudo systemctl start ssh
sudo systemctl enable ssh
```
```
sudo apt update
sudo apt install isc-dhcp-client –y
sudo ip link set enp0s8 up
sudo dhclient enp0s8
```
```
ip a
```
```
sudo ufw allow 80/tcp
```
```
sudo apt install -y nginx
sudo apt-get install -y lynx (не особо нужен)
```

**Шаг 1 (устанавливам .NET 6)**
```
sudo apt install -y wget apt-transport-https
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update (обязательно, иначе следующая команда не отработает)
sudo apt install -y dotnet-sdk-6.0 aspnetcore-runtime-6.0
dotnet –version
```

**Шаг 2 (Создаем нужные директории)**
```
sudo mkdir -p /var/www/app
sudo chown ursus:ursus /var/www/app (необязательно)
sudo chmod 755 /var/www/app или sudo chmod –R 755 /var/www
mkdir -p ~/app
```


**Шаг 3 (Скачиваем приложение)**
```
cd ~/app
git clone https://github.com/BlackCoffeeCoding/MySimpleApi.git
cd MySimpleApi
```

**Шаг 4 (Сборка и публикация)**
```
sudo dotnet publish -c Release -o /var/www/app
```

**Шаг 5 (Создаем сервис)**
```
sudo nano /etc/systemd/system/mysimpleapi.service
```
```
[Unit]
Description=.NET Web API
After=network.target

[Service]
WorkingDirectory=/var/www/app
ExecStart=/usr/bin/dotnet /var/www/app/MySimpleApi.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-api
User=dmitry
Environment=ASPNETCORE_URLS=http://0.0.0.0:5000
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```
```
sudo systemctl daemon-reexec
sudo systemctl daemon-reload
sudo systemctl enable mysimpleapi
sudo systemctl start mysimpleapi
sudo systemctl status mysimpleapi
```

**Шаг 6 (Настройка Nginx)**
```
sudo cp /etc/nginx/sites-available/default /etc/nginx/sites-available/mysimpleapi
```
```
sudo nano /etc/nginx/sites-available/mysimpleapi
```
убираем там default server возле адреса порта и дополняем в location:
```
location / {
    proxy_pass         http://localhost:5000;
    proxy_http_version 1.1;
    proxy_set_header   Upgrade $http_upgrade;
    proxy_set_header   Connection keep-alive;
    proxy_set_header   Host $host;
    proxy_cache_bypass $http_upgrade;
}
```
```
sudo nano /etc/nginx/sites-available/default
```
внутри меняем порт с 80 на 81 чтобы не было конфликта портов
```
sudo ln -s /etc/nginx/sites-available/mysimpleapi /etc/nginx/sites-enabled/
sudo nginx –t
sudo systemctl restart nginx
```

**Шаг 7 (Тестируем)**

http://192.168.1.103/api/products

http://192.168.1.103/api/customers

http://192.168.1.103/health

Invoke-RestMethod http://192.168.1.103/api/products

Invoke-RestMethod http://192.168.1.103/api/customers

Invoke-RestMethod http://192.168.1.103/health