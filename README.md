# Banking App

Aplicación para mostrar estados de cuentas de una tarjeta de crédito, registrar y listar compras, registrar pagos y también listarlos.

![image](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![image](https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white)
![image](https://img.shields.io/badge/CSS3-1572B6?style=for-the-badge&logo=css3&logoColor=white)
![image](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)

## Features

- Inicio de sesión
- Estados de cuenta
- Compras
- Pagos

## Pantallas

![App Screenshot](https://i.imgur.com/5cW7PNX.png)

![App Screenshot](https://i.imgur.com/TMa0FwJ.png)

![App Screenshot](https://i.imgur.com/uph4A65.png)

![App Screenshot](https://i.imgur.com/jZLNpGT.png)

![App Screenshot](https://i.imgur.com/vRC4GFD.png)

![App Screenshot](https://i.imgur.com/8TPHRZK.png)

![App Screenshot](https://i.imgur.com/7OmqK1d.png)

## Probar proyecto localmente

Clonar el proyecto

```bash
  git clone https://github.com/diegovelasquez-g/BankingApp.git
```

Ir al directorio del proyecto

```bash
  cd BankingApp
```

Ejecutar script de creación de base de datos

```sql
  BakingApp.sql
```

Pasos para correr la API

```bash
  cd src
  cd BakingApp.Api
  dotnet run BakingApp.Api.csproj
```

Pasos para correr la Web

```bash
  cd ..
  cd BakingApp.Client
  dotnet run App.razor
```
