# Sistema de Gestión de Candidatos - Pandape

Esta es una prueba técnica desarrollada con ASP.NET Core MVC para gestionar información de candidatos y sus experiencias laborales. El objetivo principal fue demostrar la aplicación de buenas prácticas de arquitectura de software, patrones de diseño y principios de código limpio, enfocándose en la creación de una solución robusta, modular y escalable.

## 🚀 Funcionalidades

- Crear, editar y listar candidatos  
- Gestión de experiencias laborales por candidato  
- Separación clara de responsabilidades usando CQRS
- Enfoque Code First con Entity Framework Core  
- Principios SOLID y prácticas de Código Limpio  

## 🛠️ Tecnologías Utilizadas

- **Framework:** ASP.NET Core MVC (.NET 8)  
- **Base de datos:** SQL Server (LocalDB)  
- **ORM:** Entity Framework Core  
- **Arquitectura:** MVC + CQRS (con MediatR)  
- **Lenguaje:** C#  

## 📁 Estructura del Proyecto

```
Pandape.CandidateManageSystem/
├── Application/                        # Capa de aplicación: Commands, Queries, DTOs  
├── Domain/                             # Entidades del dominio 
├── Infrastructure/                     # Contexto de base de datos y configuración de EF  
├── Web/                                # Interfaz web ASP.NET MVC (Controladores, Vistas)
├── Application.Tests/                  # Pruebas unitarias de la capa Application
├── Domain.Tests/                       # Pruebas unitarias de la capa Domain
├── Web.Tests/                          # Pruebas unitarias de la capa Web
├── Pandape.CandidateManageSystem.sln   # Archivo de solución  
└── README.md                           # Documentación del proyecto  
```

## ⚙️ Cómo Empezar

### Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQL Server Express LocalDB](https://learn.microsoft.com/es-es/sql/database-engine/configure-windows/sql-server-express-localdb)  

### Instalación

1. Clona el repositorio:

   ```git clone https://github.com/mauriyi/Pandape.CandidateManageSystem.git```  
   ```cd Pandape.CandidateManageSystem```  

2. Compila el proyecto:

   ```dotnet build```  

3. Ejecuta la aplicación:

   ```dotnet run --project Web```  

Luego abre `https://localhost:7224` en tu navegador.

## 🧪 Ejecución de Tests

Las pruebas unitarias están implementadas con **xUnit** y organizadas en tres proyectos dentro de la carpeta `test`:

- **Application.Tests**: Pruebas de la lógica de aplicación, comandos y consultas.  
- **Domain.Tests**: Pruebas de las entidades y reglas de negocio del dominio.  
- **Web.Tests**: Pruebas de los controladores y la capa de presentación.

## 🧠 Principios de Diseño

- **CQRS** para separar operaciones de lectura y escritura, facilitando la escalabilidad  
- **SOLID** para garantizar un código mantenible y extensible  
- **Clean Code**: nombres claros, responsabilidad única y estructura modular  

## 🤝 Contribuciones

¡Las contribuciones son bienvenidas! Siéntete libre de crear un pull request o abrir un issue.

## 👤 Autor

Desarrollado por Mauricio Bedoya como parte de una prueba técnica para Pandapé Colombia.

