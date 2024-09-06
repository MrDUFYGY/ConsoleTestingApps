# ConsoleTestingApps

# Console App - Google Drive File Uploader

Esta aplicación permite subir archivos a Google Drive, gestionar permisos y actualizar configuraciones a través de una interfaz de consola. La funcionalidad incluye la autenticación con OAuth2 usando las credenciales de Google Cloud.

## Funcionalidades principales

- Subir archivos a Google Drive.
- Generar archivo de configuración con credenciales y token de Google.
- Actualizar el archivo `App.config` con la configuración desde un archivo `GoogleDriveConfig.txt`.
- Escanear y subir archivos desde una carpeta local.
- Subir archivos de forma sincrónica y asincrónica.
- Gestionar permisos de Google Drive.

## Requisitos

- **Google Cloud Console**: Debes configurar un proyecto en la [Google Cloud Console](https://console.cloud.google.com/) y habilitar la API de Google Drive.
- **.NET Framework**: Este proyecto se desarrolla en .NET, asegúrate de tener el SDK instalado.
- **Credenciales OAuth2**: Debes obtener las credenciales OAuth2, se obtienen dentro de la carpeta \bin\debug\config mediante la libreria DriveUpload y el menu (opcion 2)
- **Librería DriveUpload**: Descarga la librería necesaria desde el siguiente enlace: [GoogleDriveUpdate](https://github.com/MrDUFYGY/GoogleDriveUpdate).


## Configuración de Credenciales

Para autenticarte y permitir que la aplicación suba archivos a Google Drive, debes obtener las credenciales de Google OAuth2 desde la [Google Cloud Console](https://console.cloud.google.com/). Sigue estos pasos:

1. Crea un nuevo proyecto o utiliza uno existente.
2. Ve a la sección **API & Services** y habilita la API de Google Drive.
3. Dirígete a **Credentials** y selecciona **Create Credentials > OAuth 2.0 Client IDs**.
4. Descarga el archivo `credentials.json` y colócalo en la carpeta `bin/Debug` o `bin/Release` (dependiendo de la configuración que estés usando).
5. El archivo debe llamarse `credentials.json`.

## Archivo de Configuración

La aplicación requiere un archivo de configuración llamado `App.config` en la carpeta raíz del proyecto. Este archivo debe contener las siguientes configuraciones para autenticarte y gestionar los tokens de acceso:

```xml
<appSettings>
  <add key="ClientId" value="TU_CLIENT_ID" />
  <add key="ClientSecret" value="TU_CLIENT_SECRET" />
  <add key="AccessToken" value="TU_ACCESS_TOKEN" />
  <add key="RefreshToken" value="TU_REFRESH_TOKEN" />
  <add key="ExpiresInSeconds" value="TU_EXPIRES_IN_SECONDS" />
  <add key="Scope" value="https://www.googleapis.com/auth/drive" />
</appSettings>

```
##Uso
Al iniciar la aplicación desde la consola, aparecerá un menú con varias opciones:

Subir archivo a Google Drive: Permite seleccionar un archivo desde tu máquina local y subirlo a tu Google Drive.
Generar archivo de configuración: Crea un archivo de configuración con tus credenciales OAuth y tokens.
Actualizar App.config desde GoogleDriveConfig.txt: Permite actualizar tu archivo de configuración desde un archivo GoogleDriveConfig.txt existente.
Escanear y subir archivos desde una carpeta: Busca archivos en una carpeta local y permite subirlos a Google Drive.
Subir archivos de forma asincrónica: Escanea una carpeta y sube archivos de forma asincrónica.
Permisos: Gestiona los permisos de Google Drive para los archivos subidos.
Salir: Cierra la aplicación.
Ejemplo de ejecución
bash
Copiar código
=== Menú Principal ===
1. Subir archivo a Google Drive
2. Generar archivo de configuración (credenciales y token)
3. Actualizar app.config desde GoogleDriveConfig.txt
4. Escanear y subir archivos desde una carpeta
5. Subir archivos de forma asincrónica
6. Permisos
7. Salir
Selecciona una opción:
Consideraciones
Asegúrate de que los archivos credentials.json y App.config se encuentren en las rutas correctas (bin/Debug o bin/Release) antes de ejecutar la aplicación.
Si encuentras problemas de autenticación, revisa que las credenciales OAuth2 sean válidas y que los tokens de acceso estén actualizados.



Este archivo `README.md` proporciona una descripción clara de la funcionalidad del programa, cómo configurarlo, y cómo manejar las credenciales para Google Drive. Asegúrate de completar los valores como `TU_CLIENT_ID` y `TU_CLIENT_SECRET` en el `App.config` con la información correspondiente que obtienes desde la Google Cloud Console.

Si necesitas más detalles o alguna modificación, no dudes en pedírmelo.
