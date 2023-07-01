<p align="center"><img src="https://github.com/nali894/Shift-Service/blob/master/ShiftService/Img/Shift.PNG"/></p> 

## Tabla de contenidos:


1. [Descripción y contexto](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#1-descripci%C3%B3n-y-contexto-)
2. [Pre-requisitos](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#2-pre-requisitos-)
3. [Instalación](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#3-instalaci%C3%B3n-)
4. [Ejecución](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#4-ejecuci%C3%B3n)
5. [Subir container](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#5-subir-una-imagen-de-docker-desktop-a-azure-container-registry)
6. [Despliegue](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#6-despliegue-)
7. [Pruebas](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#7-pruebas-)
8. [Pruebas Automatizadas](https://github.com/nali894/Shift-Service/blob/master/ShiftService/README.md#8-pruebas-desde-un-archivo-csv-%EF%B8%8F)



## 1. Descripción y contexto 📄

Desarrollado en .Net 7.0

Este Servicio contiene los métodos para:

- Crear nuevos servicios
- Aceptar Solicitudes de servicios
- Rechazar Solicitudes de servicios
- Consultar todos los servicios creados
- Consultar todos los roles
- Consultar los servicios creados filtrado por el rol del usuario


## 2. Pre-requisitos 📋

- Instalación: Descargar e Instalar IDE de Visual Studio (https://visualstudio.microsoft.com/es/vs/)
- Despliegue: Descargar e Instalar Docker Desktop (https://www.docker.com/products/docker-desktop/)
- Pruebas automatizadas: Instalar Postman (https://www.postman.com/)

## 3. Instalación 🔧

Clonar este Repositorio con el IDE de Visual Studio (https://learn.microsoft.com/es-es/visualstudio/version-control/git-clone-repository?view=vs-2022)

## 4. Ejecución

* Abrir ventana de línea de comandos. En Windows.
* Navega hasta la carpeta raíz del proyecto que deseas compilar. Usa el comando "cd" para cambiar de directorio. Por ejemplo:

         cd C:\Users\Default\source\repos\ShiftService
         
* Ejecuta el comando "dotnet build" seguido del nombre del archivo de proyecto:

        dotnet build ShiftService.csproj
        
* Si deseas compilar el proyecto en modo release, utiliza el siguiente comando:

        dotnet build -c Release

## 5. Subir una imagen de Docker Desktop a Azure Container Registry

Abrir Símbolo del sistema cmd de windows
* Ubicarse en el directorio donde se encuentra el proyecto asp.net clonado, usando el comando "cd". Por ejemplo:

         cd C:\Users\Default\source\repos\Shift-Service\ShiftService
         
* Construir la imagen: ejecutar los siguientes comandos

          docker build -t dotnetshift .
          docker images
                
* Iniciar sesión de docker hub: ejecutar:

          docker login
          
* Etiquetar imagen de Docker Desktop:

          docker tag dotnetshift  <nombre imagen>:<tag> ej: docker tag dotnetshift  nali894/ShiftService:v1

* Iniciar sesión del registro de contenedores de Azure (https://learn.microsoft.com/en-us/azure/container-registry/container-registry-authentication?tabs=azure-cli):

         docker login <login-server del Azure Container Registry> -u <username del Azure Container Registry> -p <password del Azure Container Registry>

Ejemplo:

         docker login shift.azurecr.io -u admin -p 123

* Asignar nombre de registro de contenedores de Azure:

         docker tag <nombre imagen docker desktop> <login-server del Azure Container Registry>/<nombre imagen>:<tag>

  Ejemplo:

         docker tag ShiftService shift.azurecr.io/ShiftService:v1.0


* Subir la imagen etiquetada al contenedor de Azure:

         docker push <login-server del Azure Container Registry>/<image-name>:<tag>


## 6. Despliegue 📦

### Flujo de trabajo en Azure DevOps:

* Continuous integration (CI)

  ```resources:
  repositories:
  - repository: self
    type: git
    ref: refs/heads/main
  jobs:
  - job: Job_1
    displayName: Agent job 1
    pool:
      vmImage: ubuntu-latest
  steps:
  - checkout: self
    clean: true
    fetchTags: false
  - task: DotNetCoreCLI@2
    displayName: dotnet restore
    inputs:
      command: restore
      projects: $(System.DefaultWorkingDirectory)/*.sln
  - task: DotNetCoreCLI@2
    displayName: dotnet build
    inputs:
      projects: $(System.DefaultWorkingDirectory)/*.sln
  - task: DotNetCoreCLI@2
    displayName: dotnet publish
    inputs:
      projects: $(System.DefaultWorkingDirectory)
  - task: DockerInstaller@0
    displayName: Install Docker 17.09.0-ce
  - task: Docker@2
    displayName: buildAndPush
    inputs:
      containerRegistry: c3b26cc3-3d3a-476f-a188-2c23b684ecbb
      repository: prod_innbox
      tags: v$(Build.BuildId)
  - task: PowerShell@2
    displayName: PowerShell Reemplazar valores Deployments
    inputs:
      targetType: inline
      script: "$yamlPath = \"$(System.DefaultWorkingDirectory)/KubernetesManifest/shiftAzure.yml\"\n\n$newImage = \"shift.azurecr.io/ShiftService:v$(Build.BuildId)\"\n$dbname=\"shift\"\n$hostName=\"dbshift.mysql.database.azure.com\"\n$userID=\"admin\"\n$Password=\"123\"\n$SslMode=\"require\"\n\n\n\n$yamlContent = Get-Content -Path $yamlPath -Raw\n\n$yamlContent = $yamlContent -replace '\\[IMAGE_SHIFT\\]', \"$newImage\"\n$yamlContent = $yamlContent -replace '\\[DB_NAME\\]', \"$dbname\"\n$yamlContent = $yamlContent -replace '\\[DB_HOSTNAME\\]', \"$hostName\"\n$yamlContent = $yamlContent -replace '\\[DB_ID\\]', \"$userID\"\n$yamlContent = $yamlContent -replace '\\[DB_PASSWORD\\]', \"$Password\"\n$yamlContent = $yamlContent -replace '\\[DB_SSLMODE\\]', \"$SslMode\"\n \n$yamlContent | Set-Content -Path $yamlPath\nwrite-host \"$yamlContent\"\n\necho \"$yamlContent\""
      pwsh: true
  - task: CopyFiles@2
    displayName: Copy files
    inputs:
      SourceFolder: $(System.DefaultWorkingDirectory)/KubernetesManifest
      TargetFolder: $(Build.ArtifactStagingDirectory)
  - task: PublishBuildArtifacts@1
    displayName: 'Publish Artifact: drop'
    inputs:
      FileCopyOptions: ''
...
```
         

         



## 7. Pruebas 🔩

* Abrir la aplicación Postman
* Desde Postman: Obtener datos de usuario logueado - strUserName: correo del usuario

          content-type:application/json;charset=UTF-8

          POST: localhost:3045/api/Service/GetUserByUserName

          body:
          { 
           "strUserName":"cprieto@abc.com"
          }

* Desde Postman: Consultar los servicios según el rol del usuario logueado strRole: Rol del usuario[roleCode]- strDateTime: Fecha en la que se crea el servicio

          content-type:application/json;charset=UTF-8

          POST: localhost:3045/api/Service/GetServicesByRole

          body:
          {
            "strRole":"5",
             "strDateTime": "2023/02/25"
          }

* Desde Postman: Aceptar Servicio intServiceID: Id del servicio seleccionado [code]-strUserCode: Código del usuario logueado [code]

          content-type:application/json;charset=UTF-8

          POST: localhost:3045/api/Service/AcceptService

          Body:
          { "intServiceID":2,
           "strUserCode":"3"
          }

* Desde Postman: Obtener todos los roles con su respectivo servicio


          content-type:application/json;charset=UTF-8

          GET: localhost:3045/api/Service/GetAllRoles

* Desde Postman: Obtener todos los servicios creados

          content-type:application/json;charset=UTF-8
          
          GET: localhost:3045/api/Service/GetAllServices
          
## 8. Pruebas Automatizadas ⚙️

* Abrir Postman
* Seleccionar la colección que desea ejecutar > Run Collection
* En la sección Data > Select File
* Seleccione el archivo csv que tiene el mismo nombre del método.
* Ejecutar Colección
