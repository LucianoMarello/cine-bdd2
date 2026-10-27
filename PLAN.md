# Plan del MVP — Sistema de gestión de cine

**Stack:** C# WinForms · SQL Server · Entity Framework · arquitectura en capas (BE / DAL / BLL / UI)
**Análisis (2da instancia):** herramienta externa (Power BI / Excel) conectada a vistas SQL
**Alcance:** login con roles + ABM completo + venta de entradas

---

## 0. Decisiones técnicas

| Tema | Decisión | Motivo |
|---|---|---|
| Plataforma | .NET 8 + EF Core 8 (`Microsoft.EntityFrameworkCore.SqlServer`) | Si la cátedra exige .NET Framework 4.8, usar EF6; el plan no cambia |
| Creación de la BD | **Script SQL a mano** (no migraciones de EF) | Es una materia de BD: el DDL, las constraints y las vistas son parte de lo que se evalúa |
| Mapeo EF | Entidades escritas a mano en BE + Fluent API en DAL | Son 7 tablas; mantiene BE sin dependencias de EF |
| Contraseñas | Hash PBKDF2 (`Rfc2898DeriveBytes`) o BCrypt, con salt | Nunca texto plano |
| Bajas | Lógicas (`estado = 0`) | No se pierde historial para el análisis |
| Roles | `Admin` y `Cajero` | Admin: todo. Cajero: solo ventas |

---

## 1. Modelo de datos corregido

Cambios respecto del diagrama original marcados con **(nuevo)** o **(cambio)**.

### Usuario
| Campo | Tipo | Restricciones |
|---|---|---|
| id_usuario **(cambio de nombre)** | INT IDENTITY | PK |
| nombre_usuario | VARCHAR(50) | NOT NULL **(nuevo)**, UNIQUE |
| clave | VARCHAR(255) | NOT NULL (hash + salt) |
| rol | VARCHAR(30) | NOT NULL, CHECK IN ('Admin','Cajero') **(nuevo)** |
| estado | BIT **(cambio)** | NOT NULL, DEFAULT 1 |

### Pelicula
| Campo | Tipo | Restricciones |
|---|---|---|
| id_pelicula | INT IDENTITY | PK |
| titulo | VARCHAR(100) | NOT NULL |
| genero | VARCHAR(50) | NOT NULL (en la app: combo con lista cerrada) |
| duracion | INT | NOT NULL, CHECK > 0 (minutos) |
| estado | BIT | NOT NULL, DEFAULT 1 |

### Sala
| Campo | Tipo | Restricciones |
|---|---|---|
| id_sala | INT IDENTITY | PK |
| nombre | VARCHAR(50) | NOT NULL, UNIQUE **(nuevo)** |
| capacidad | INT | NOT NULL, CHECK > 0 |
| estado | BIT | NOT NULL, DEFAULT 1 |

### Funcion
| Campo | Tipo | Restricciones |
|---|---|---|
| id_funcion | INT IDENTITY | PK |
| id_pelicula | INT | NOT NULL **(nuevo)**, FK → Pelicula |
| id_sala | INT | NOT NULL **(nuevo)**, FK → Sala |
| fecha_hora | DATETIME | NOT NULL |
| precio_entrada | DECIMAL(10,2) | NOT NULL, CHECK >= 0 |
| formato *(opcional)* | VARCHAR(10) | '2D' / '3D' — dimensión extra para el análisis |
| | | UNIQUE (id_sala, fecha_hora) **(nuevo)** |

### Caja
| Campo | Tipo | Restricciones |
|---|---|---|
| id_caja | INT IDENTITY | PK |
| nombre | VARCHAR(30) | NOT NULL, UNIQUE **(nuevo)** |
| estado | BIT | NOT NULL, DEFAULT 1 |

### Venta
| Campo | Tipo | Restricciones |
|---|---|---|
| id_venta | INT IDENTITY | PK |
| id_funcion | INT | NOT NULL, FK → Funcion |
| id_caja | INT | NOT NULL, FK → Caja |
| id_usuario **(nuevo)** | INT | NOT NULL, FK → Usuario |
| fecha_venta | DATETIME | NOT NULL, DEFAULT GETDATE() |
| cantidad_entradas | INT | NOT NULL, CHECK > 0 |
| precio_unitario **(nuevo)** | DECIMAL(10,2) | NOT NULL (copia del precio al momento de vender) |
| total | DECIMAL(12,2) | NOT NULL |
| forma_pago *(opcional)* | VARCHAR(20) | 'Efectivo' / 'Débito' / 'Crédito' / 'QR' |

### Objetivo_semanal_caja
| Campo | Tipo | Restricciones |
|---|---|---|
| id_objetivo | INT IDENTITY | PK |
| id_caja | INT | NOT NULL, FK → Caja |
| fecha_inicio | DATE | NOT NULL |
| fecha_fin | DATE | NOT NULL, CHECK > fecha_inicio |
| monto_objetivo | DECIMAL(12,2) | NOT NULL, CHECK > 0 |
| | | UNIQUE (id_caja, fecha_inicio) **(nuevo)** |

### Reglas de negocio (se validan en BLL)
1. **Cupo:** entradas vendidas de una función + cantidad nueva ≤ capacidad de la sala. Dentro de una transacción.
2. **Solapamiento:** una sala no puede tener dos funciones cuyos horarios se pisen (`fecha_hora` + `duracion`).
3. No se venden entradas de funciones ya pasadas ni de películas/salas inactivas.
4. No se puede dar de baja al último Admin activo.

---

## 2. Estructura de la solución

```
CineApp.sln
├── BE    Entidades (Usuario, Pelicula, Sala, Funcion, Caja, Venta, ObjetivoSemanalCaja)
├── DAL   CineContext (DbContext), configuración Fluent API, repositorios
├── BLL   Reglas de negocio, validaciones, hash de claves, sesión
└── UI    Formularios WinForms
/sql
├── 01_crear_bd.sql
├── 02_datos_maestros.sql
├── 03_datos_prueba_ventas.sql
└── 04_vistas_analisis.sql
/docs
├── diagrama_persistencia.png
└── diccionario_datos.md
```

Referencias: UI → BLL → DAL → BE (BE no referencia a nadie).

---

## 3. Fases

### Fase 1 — Documentación del modelo
- [ ] Actualizar el diagrama de persistencia con las correcciones de la sección 1.
- [ ] Escribir el diccionario de datos (tabla, campo, tipo, nulabilidad, restricción, descripción).

**Listo cuando:** diagrama y diccionario coinciden campo por campo.

### Fase 2 — Base de datos
- [ ] `01_crear_bd.sql`: CREATE DATABASE, tablas en orden de dependencia, PK, FK, UNIQUE, CHECK, DEFAULT.
- [ ] Índices en las FK de `Venta` y `Funcion` y en `Venta.fecha_venta`.
- [ ] Probar las constraints con inserts inválidos (deben fallar).

**Listo cuando:** el script corre de cero sin errores y se puede re-ejecutar (DROP IF EXISTS al inicio).

### Fase 3 — Esqueleto de la solución
- [ ] Crear la solución con los 4 proyectos y sus referencias.
- [ ] Clases de BE.
- [ ] `CineContext` con Fluent API (nombres de tabla/columna en snake_case, tipos decimales).
- [ ] Cadena de conexión en `appsettings.json` / `App.config` (no hardcodeada).
- [ ] Prueba de humo: listar películas desde la UI.

### Fase 4 — Login y seguridad
- [ ] `UsuarioBLL`: hash y verificación de clave, login, validación de usuario activo.
- [ ] Creación automática del usuario Admin inicial si la tabla está vacía (el hash se calcula en C#, por eso no va en el script SQL).
- [ ] `FrmLogin`: mensajes de error genéricos ("usuario o clave incorrectos").
- [ ] Clase `Sesion` con el usuario logueado.
- [ ] `FrmPrincipal` con menú que se habilita según el rol.

**Listo cuando:** Admin ve todo el menú, Cajero solo Ventas, usuario inactivo no entra.

### Fase 5 — ABMs (solo Admin)
Orden sugerido, de más simple a más complejo. Un formulario por entidad con grilla + alta / edición / baja lógica.

- [ ] Salas
- [ ] Cajas
- [ ] Películas
- [ ] Usuarios (alta con clave, reseteo de clave, cambio de rol)
- [ ] Funciones (combos de película y sala activas, validación de solapamiento)
- [ ] Objetivos semanales por caja (validación de semana duplicada)

Conviene hacer el primero completo (Salas) y usarlo de molde para el resto.

### Fase 6 — Venta de entradas
- [ ] `FrmVenta`: elegir función (solo futuras), mostrar disponibilidad, cantidad, caja, forma de pago, total calculado.
- [ ] `VentaBLL.Registrar`: transacción con nivel de aislamiento `Serializable`, revalida cupo, copia `precio_unitario`, calcula `total`, guarda `id_usuario` de la sesión.
- [ ] Listado de ventas con filtros por fecha, caja y película.

**Listo cuando:** no se puede vender más que la capacidad, ni siquiera con dos instancias de la app abiertas a la vez.

### Fase 7 — Datos de prueba
Para que el análisis tenga sentido hacen falta volumen y variedad.

- [ ] `02_datos_maestros.sql`: ~20 películas de distintos géneros, 4–6 salas de distinta capacidad, 3 cajas.
- [ ] `03_datos_prueba_ventas.sql`: 3–6 meses de funciones y unas miles de ventas generadas con un script (bucles + `NEWID()`/`RAND()`), con patrones realistas: más ventas en fin de semana y horario nocturno, películas más y menos exitosas.
- [ ] Objetivos semanales para todo el período, algunos cumplidos y otros no.
- [ ] Usuarios cajeros de prueba creados desde la app.

### Fase 8 — Preparación para el análisis (2da instancia)
- [ ] `04_vistas_analisis.sql`:
  - `vw_ventas_detalle`: venta + función + película + sala + caja + usuario, ya desnormalizada.
  - `vw_ocupacion_funcion`: entradas vendidas / capacidad por función.
  - `vw_objetivo_vs_real`: monto objetivo vs recaudado por caja y semana.
- [ ] Conectar Power BI / Excel a las vistas y verificar que cargan.
- [ ] Preguntas que el tablero debería responder:
  - Recaudación por día / semana / mes.
  - Películas y géneros más vendidos.
  - Ocupación promedio por sala y por franja horaria.
  - Cumplimiento del objetivo semanal por caja.
  - Ventas por cajero y por forma de pago.

### Fase 9 — Cierre
- [ ] Manejo de errores: mensajes claros, sin excepciones crudas en pantalla.
- [ ] Prueba completa desde cero: correr scripts → abrir app → login → cargar → vender → ver en Power BI.
- [ ] Backup de la BD (`.bak`) o scripts para la entrega.
- [ ] Guion corto para la presentación.

---

## 4. Fuera del MVP
- Selección de butacas numeradas.
- Apertura y cierre de caja por turno.
- Anulación / devolución de ventas.
- Promociones y tipos de entrada (menor, jubilado).
- Bitácora de auditoría.
