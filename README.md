Шаг 0 (Настраиваем сеть)

Сразу после создания виртуальной машины, до установки ОС, заходим в ее настройки - сеть - меняем тип подключения на сетевой мост - ок.

Шаг 1 (устанавливам .NET 6)

```
sudo apt install -y wget apt-transport-https nginx
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-6.0 aspnetcore-runtime-6.0
```

Шаг 2 (Создаем нужные директории)


```
sudo mkdir -p /var/www/app
sudo chown vi:vi /var/www/app (необязательно)
sudo chmod 755 /var/www/app
mkdir -p ~/app
```

Шаг 3 (Скачиваем приложение)

```
cd ~/app
git clone https://github.com/nekrochan/admOs2.git
cd admOs2
```

Шаг 4 (Сборка и публикация)

```
sudo dotnet publish -c Release -o /var/www/app
```

Шаг 5 (Создаем сервис)

```
sudo nano /etc/systemd/system/admos2.service
```

```
[Unit]
Description=.NET Web API
After=network.target

[Service]
WorkingDirectory=/var/www/app
ExecStart=/usr/bin/dotnet /var/www/app/ApiService.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-api
User=vi
Environment=ASPNETCORE_URLS=http://0.0.0.0:5000
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

Шаг 6 (Настройка Nginx)

```
sudo cp /etc/nginx/sites-available/default /etc/nginx/sites-available/admos2
```

```
sudo nano /etc/nginx/sites-available/admos2
```

убираем default server возле адреса и меняем в location:

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

внутри меняем порт с 80 на 81, чтобы не было конфликта портов

```
sudo ln -s /etc/nginx/sites-available/admos2 /etc/nginx/sites-enabled/
sudo systemctl restart nginx
```

Шаг 7 (Тестируем)

В виртуальной машине вводим команду ip address show, берем адрес после inet (не loopback).

В браузере:

```
http://[ip-адрес]/api/cars
```

```
http://[ip-адрес]/api/owners
```

```
http://[ip-адрес]/health
```

В PowerShell (Windows):

```
Invoke-RestMethod http://[ip-адрес]/api/cars

Invoke-RestMethod http://[ip-адрес]/api/owners

Invoke-RestMethod http://[ip-адрес]/health
```

В терминале Linux:

```
curl http://[ip-адрес]/api/cars

curl http://[ip-адрес]/api/owners

curl http://[ip-адрес]/health
```
