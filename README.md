# 🚀 Asisya Platform - README

## 📌 Descripción General

**Asisya** es una plataforma de gestión de productos construida bajo una arquitectura moderna. Utiliza:

* **.NET 8** para el backend
* **React 18** para el frontend

La solución implementa **Clean Architecture**, garantizando una clara separación de responsabilidades entre:

* Capa de presentación
* Lógica de negocio
* Acceso a datos
* Dominio

---

## 🔐 Acceso a la Aplicación

* **URL:** http://localhost:3000/login
* **Usuario:** juan.pérez
* **Contraseña:** 123456

---

## 🏗️ Arquitectura

La solución sigue una variante de **Clean Architecture**, organizada en capas independientes:

* **Presentation Layer:** API Controllers / Frontend
* **Application Layer:** Servicios, DTOs, lógica de negocio
* **Domain Layer:** Entidades y reglas de negocio
* **Infrastructure Layer:** Acceso a datos, ORM, repositorios

🔁 **Principio clave:**
Las dependencias apuntan hacia el dominio, manteniéndolo independiente de frameworks externos.

---

## 📁 Organización del Proyecto

```
Asisya/
├── 1- DistributedServices/
│   └── Asisya/                    # Web API (Controllers, Program.cs)
├── 2- Core/
│   ├── CoreLibrary/               # Lógica de negocio, DTOs, Servicios
│   └── domain/                    # Entidades y consultas SQL
├── 3- Infrastructure/
│   └── infrastructure/            # EF Core, Repositorios, Unit of Work
├── 4- Test/
│   └── Asisya.Test/               # Pruebas unitarias y de integración
└── Front/
    └── Asisya-web/                # Aplicación React
```

---

## 🗄️ Estrategia de Acceso a Datos

Se implementa una estrategia híbrida para optimizar rendimiento y mantenibilidad:

* **Entity Framework Core**

  * CRUD
  * Gestión de transacciones
  * `ServiceContext`

* **Dapper**

  * Consultas de lectura optimizadas
  * Definidas en `SqlQueries`

* **Repository Pattern**

  * Implementación genérica (`GenericRepository`)

* **Unit of Work**

  * Control de transacciones (`UnitOfWork`)

---

## ⚙️ Stack Tecnológico

### 🧠 Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core 9
* PostgreSQL (Npgsql)
* Dapper
* AutoMapper
* BCrypt.Net (hash de contraseñas)

### 🎨 Frontend

* React 18
* Vite
* Node.js 22

### 🗃️ Base de Datos

* PostgreSQL 16

---

## 🔧 Configuración del DbContext

El `ServiceContext` detecta automáticamente entidades que implementan `IEntity`:

```csharp
services.AddDbContext<ServiceContext>(options =>
    options.UseNpgsql(conn, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    }));
```

---

## 🔗 Relaciones del Dominio

Principales entidades:

* **Product**

  * Relacionado con `Category` y `Supplier`

* **OrderDetail**

  * Maneja relaciones compuestas de órdenes

* Otras entidades:

  * Category
  * Customer
  * Employee
  * Supplier

---

## 🐳 Ejecución con Docker

La solución utiliza **Docker Compose** con tres servicios:

| Servicio | Imagen             | Puerto | Rol              |
| -------- | ------------------ | ------ | ---------------- |
| db       | postgres:16        | 5432   | Base de datos    |
| api      | build (Dockerfile) | 5000   | API .NET         |
| front    | build (React)      | 3000   | Frontend (Nginx) |

### ▶️ Comandos

```bash
# Construir y ejecutar
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d

# Detener servicios
docker-compose down
```



## 🔍 Notas Importantes

* CORS habilitado para:
  `http://localhost:3000`

* CI/CD:

  * Valida backend, frontend y Docker antes de merges

* Base de datos:

  * Inicializada automáticamente con `backup.sql`

* Red de servicios:

  * Comunicación mediante red `goodnet`

---

## 📚 Recursos Adicionales

* Solution Structure & Project Layout
* CI/CD & Deployment

---

## ✅ Conclusión

Asisya es una plataforma robusta, escalable y mantenible, diseñada con buenas prácticas modernas:

* Clean Architecture
* Acceso a datos optimizado
* Contenerización completa

Aunque el sistema fue diseñado con principios de Clean Architecture, se optó por un Monolito Modular en lugar de una arquitectura de microservicios distribuida por las siguientes razones técnicas:

* Integridad y Transaccionalidad: Al gestionar productos, categorías y proveedores, se priorizó la consistencia ACID de los datos. Esto permite asegurar la integridad referencial mediante el patrón Unit of Work sin la complejidad de gestionar consistencia eventual o patrones distribuidores (Saga).

* Eficiencia Operativa: Se eliminó la latencia de red innecesaria (comunicación inter-servicio) y se simplificó la infraestructura, permitiendo que un solo contenedor de API gestione la lógica de negocio de manera optimizada y con menor consumo de recursos (RAM/CPU).

* Mantenibilidad y Despliegue: Un monolito bien estructurado reduce significativamente la complejidad del pipeline de CI/CD y el monitoreo, facilitando un despliegue ágil sin necesidad de implementar API Gateways o Service Discovery en esta etapa del proyecto.

* Escalabilidad Futura: Gracias al desacoplamiento en capas (Core, Infrastructure, DistributedServices), el sistema está listo para ser "segmentado". Si un módulo específico requiriera escalar de forma independiente en el futuro, la lógica de negocio ya se encuentra aislada para ser extraída como un microservicio sin reescritura masiva.

En resumen: Se priorizó la robustez y la velocidad de desarrollo, entregando una arquitectura limpia que es fácil de mantener hoy y está preparada para evolucionar mañana.

---
