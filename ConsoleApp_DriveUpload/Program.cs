using Google.Apis.Auth.OAuth2;
using GoogleDriveUpload;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menú Principal ===");
                Console.WriteLine("1. Subir archivo a Google Drive");
                Console.WriteLine("2. Generar archivo de configuración (credenciales y token)");
                Console.WriteLine("3. Actualizar app.config desde GoogleDriveConfig.txt");
                Console.WriteLine("4. Escanear y subir archivos desde una carpeta");
                Console.WriteLine("5. Subir archivos de forma asincrónica");
                Console.WriteLine("6. Permisos");
                Console.WriteLine("7. Salir");
                Console.Write("Selecciona una opción: ");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SubirArchivo();
                        break;
                    case "2":
                        GenerarConfiguracion();
                        break;
                    case "3":
                        ActualizarAppConfig();
                        break;
                    case "4":
                        EscanearYSubirArchivos();
                        break;
                    case "5":
                        EscanearYSubirArchivosAsync().GetAwaiter().GetResult();
                        break;
                    case "6":
                        Permissions();
                        break;
                    case "7":
                        Console.WriteLine("Saliendo del programa...");
                        return;
                    default:
                        Console.WriteLine("Opción no válida, por favor intenta de nuevo.");
                        break;
                }

                Console.WriteLine("Presiona cualquier tecla para volver al menú...");
                Console.ReadKey();
            }
        }

        static void Permissions()
        {
            // Autenticar usando GoogleGetCredentials
            GoogleGetCredentials googleGetCredentials = new GoogleGetCredentials();
            UserCredential credential = googleGetCredentials.Get();

            // Si la autenticación es exitosa, gestionar permisos
            if (credential != null)
            {
                GoogleDrivePermissionManager.GestionarPermisos();
            }
            else
            {
                Console.WriteLine("Autenticación fallida. No se pueden gestionar los permisos.");
            }
        }

        static void SubirArchivo()
        {
            try
            {
                Console.Write("Ingresa la ruta completa del archivo que deseas subir: ");
                string fullPath = Console.ReadLine();

                var result = Init.ProcessFileUpload(fullPath);

                if (result.Correct)
                {
                    Console.WriteLine("Se realizó una correcta subida y eliminación del archivo.");
                }
                else
                {
                    Console.WriteLine($"Ha habido un error: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        static void GenerarConfiguracion()
        {
            try
            {
                GoogleGetCredentials getCredentials = new GoogleGetCredentials();
                getCredentials.GetCredentials();

                Console.WriteLine("Archivo de configuración generado correctamente. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al generar el archivo de configuración: {ex.Message}");
            }
        }
             
        static void ActualizarAppConfig()
        {
            try
            {
                string configFolderPath = "Config"; // Carpeta donde se guardará el archivo de configuración
                string configFilePath = Path.Combine(configFolderPath, "GoogleDriveConfig.txt");

                if (File.Exists(configFilePath))
                {
                    RefreshCredentials.UpdateAppConfigFromTxt(configFilePath);
                    Console.WriteLine("app.config actualizado correctamente desde GoogleDriveConfig.txt.");
                }
                else
                {
                    Console.WriteLine($"No se encontró el archivo {configFilePath}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al actualizar el app.config: {ex.Message}");
            }
        }

        static void EscanearYSubirArchivos()
        {
            try
            {
                Console.Write("Ingresa la ruta completa de la carpeta que deseas escanear: ");
                string folderPath = Console.ReadLine();

                if (Directory.Exists(folderPath))
                {
                    // Obtener los primeros 10 archivos en la carpeta
                    var archivos = Directory.GetFiles(folderPath).Take(10).ToList();

                    if (archivos.Count == 0)
                    {
                        Console.WriteLine("No se encontraron archivos en la carpeta.");
                        return;
                    }

                    Console.WriteLine("Archivos encontrados:");
                    foreach (var archivo in archivos)
                    {
                        Console.WriteLine(archivo);
                    }

                    Console.Write("¿Deseas subir estos archivos a Google Drive? (s/n): ");
                    var respuesta = Console.ReadLine();

                    if (respuesta?.ToLower() == "s")
                    {
                        SubirArchivos(archivos);
                    }
                    else
                    {
                        Console.WriteLine("Operación cancelada.");
                    }
                }
                else
                {
                    Console.WriteLine("La carpeta no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al escanear y subir archivos: {ex.Message}");
            }
        }

        static void SubirArchivos(List<string> archivos)
        {
            try
            {
                var tiemposDeSubida = new List<double>();
                foreach (var archivo in archivos)
                {
                    var stopwatch = Stopwatch.StartNew();

                    var result = Init.ProcessFileUpload(archivo);
                    stopwatch.Stop();

                    if (result.Correct)
                    {
                        Console.WriteLine($"Archivo {Path.GetFileName(archivo)} subido correctamente en {stopwatch.Elapsed.TotalSeconds} segundos.");
                        tiemposDeSubida.Add(stopwatch.Elapsed.TotalSeconds);
                    }
                    else
                    {
                        Console.WriteLine($"Error al subir el archivo {Path.GetFileName(archivo)}: {result.Message}");
                    }
                }

                if (tiemposDeSubida.Any())
                {
                    var promedio = tiemposDeSubida.Average();
                    var maximo = tiemposDeSubida.Max();
                    var total = tiemposDeSubida.Sum();

                    Console.WriteLine($"El tiempo promedio de subida fue de {promedio} segundos.");
                    Console.WriteLine($"El tiempo máximo de subida para un solo archivo fue de {maximo} segundos.");
                    Console.WriteLine($"El tiempo total de subida sumando todos los archivos fue de {total} segundos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al subir los archivos: {ex.Message}");
            }
        }

        static async Task EscanearYSubirArchivosAsync()
        {
            try
            {
                Console.Write("Ingresa la ruta completa de la carpeta que deseas escanear: ");
                string folderPath = Console.ReadLine();

                if (Directory.Exists(folderPath))
                {
                    // Obtener los primeros 10 archivos en la carpeta
                    var archivos = Directory.GetFiles(folderPath).Take(10).ToList();

                    if (archivos.Count == 0)
                    {
                        Console.WriteLine("No se encontraron archivos en la carpeta.");
                        return;
                    }

                    Console.WriteLine("Archivos encontrados:");
                    foreach (var archivo in archivos)
                    {
                        Console.WriteLine(archivo);
                    }

                    Console.Write("¿Deseas subir estos archivos a Google Drive? (s/n): ");
                    var respuesta = Console.ReadLine();

                    if (respuesta?.ToLower() == "s")
                    {
                        var stopwatch = Stopwatch.StartNew();

                        await SubirArchivosAsync(archivos);

                        stopwatch.Stop();
                        Console.WriteLine($"El tiempo total para subir todos los archivos fue de {stopwatch.Elapsed.TotalSeconds} segundos.");
                    }
                    else
                    { 
                        Console.WriteLine("Operación cancelada.");
                    }
                } 
                else
                {
                    Console.WriteLine("La carpeta no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al escanear y subir archivos: {ex.Message}");
            }
        }

        static async Task SubirArchivosAsync(List<string> archivos)
        {
            var tasks = archivos.Select(async archivo =>
            {
                var result = await Task.Run(() => Init.ProcessFileUpload(archivo));
                if (result.Correct)
                {
                    Console.WriteLine($"Archivo {Path.GetFileName(archivo)} subido correctamente.");
                }
                else
                {
                    Console.WriteLine($"Error al subir el archivo {Path.GetFileName(archivo)}: {result.Message}");
                }
            });

            await Task.WhenAll(tasks);
        }



    }


}

