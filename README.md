# Sistema de gestión de cine

Proyecto final de Base de Datos II. Aplicación de escritorio para la boletería de un cine: login con roles, administración de películas, salas, funciones, cajas y usuarios, venta de entradas con control de cupo, y vistas SQL con indicadores para analizar los datos en Power BI o Excel.

**Tecnologías:** C# (.NET 8, Windows Forms) · Entity Framework Core 8 · SQL Server

## Requisitos

- SQL Server (probado con SQL Server Express, instancia `.\SQLEXPRESS`) y `sqlcmd`.
- Visual Studio 2022 o el SDK de .NET 8 o superior.
- Power BI Desktop o Excel, para el análisis.

## Puesta en marcha

1. **Crear la base con datos de prueba:** ejecutar [sql/regenerar_base.bat](sql/regenerar_base.bat). Corre los scripts 01 a 05 en orden y deja unos 6 meses de actividad hasta el día de hoy.
2. **Revisar la conexión:** [UI/appsettings.json](UI/appsettings.json) apunta a `.\SQLEXPRESS` con autenticación de Windows. Si la instancia tiene otro nombre, cambiarlo ahí y en `SERVIDOR` dentro del `.bat`.
3. **Abrir `CineApp.sln`**, elegir el proyecto **UI** como proyecto de inicio y ejecutar.
4. **Primer uso:** como no hay administrador, la aplicación pide crearlo.
5. **Cajeros:** los datos de prueba traen `cajero1` a `cajero4` sin clave utilizable (SQL no puede calcular el hash). Para usarlos, entrar como administrador y asignarles una clave en Administración → Usuarios.

Regenerar la base borra todo, incluidos los usuarios. Conviene hacerlo justo antes de presentar, para que los datos lleguen hasta ese día.

## Estructura

```
CineApp.sln
├── BE     Entidades (una clase por tabla)
├── DAL    Acceso a datos con Entity Framework (CineContext + una clase por tabla)
├── BLL    Reglas de negocio, validaciones, hash de claves y sesión
├── UI     Formularios Windows Forms
├── sql    Scripts de la base de datos
├── docs   Diccionario de datos, diagrama y guías
└── entrega/CineDB.bak   Copia de seguridad de la base
```

Las referencias respetan las capas: UI → BLL → DAL → BE.

## Scripts SQL

| Script | Qué hace |
|---|---|
| `01_crear_bd.sql` | Crea la base, las tablas, las restricciones y los índices |
| `01b_probar_constraints.sql` | Prueba que las restricciones rechacen datos inválidos (no deja datos) |
| `02_datos_maestros.sql` | Salas, cajas, películas y cajeros |
| `03_datos_prueba_ventas.sql` | Funciones, ventas y objetivos semanales (6 meses, con patrones realistas) |
| `04_vistas_analisis.sql` | Vistas de detalle para el análisis |
| `05_kpis.sql` | Vistas de indicadores (KPI) |
| `regenerar_base.bat` | Ejecuta 01 a 05 en orden |

## Restaurar el backup

`entrega/CineDB.bak` es una copia de la base recién generada. SQL Server no suele tener permiso para leer archivos dentro de la carpeta del usuario, así que primero hay que copiarlo a una carpeta accesible (por ejemplo `C:\Users\Public`) y restaurarlo desde SSMS (**Bases de datos → Restaurar base de datos**) o con:

```sql
RESTORE DATABASE CineDB FROM DISK = N'C:\Users\Public\CineDB.bak' WITH REPLACE;
```

Los datos del backup llegan hasta la fecha en que se generó. Para tenerlos al día, usar `regenerar_base.bat`.

## Documentación

- [docs/diccionario_datos.md](docs/diccionario_datos.md) — tablas, campos, restricciones y reglas de negocio.
- [docs/diagrama_persistencia.md](docs/diagrama_persistencia.md) y `Diagram de persistencia tablero de cine.drawio` — modelo de datos.
- [docs/guia_analisis.md](docs/guia_analisis.md) — cómo conectar Power BI o Excel, vistas e indicadores.
- [docs/guion_presentacion.md](docs/guion_presentacion.md) — guion de la presentación y preguntas posibles.
