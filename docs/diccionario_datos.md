# Diccionario de datos — Sistema de gestión de cine

**Motor:** SQL Server · **Base de datos:** `CineDB`

Convenciones:
- Nombres de tablas en singular y de columnas en `snake_case`.
- Toda PK es `INT IDENTITY(1,1)`.
- `estado`: `1` = activo, `0` = dado de baja (baja lógica; no se borran filas).
- Importes en pesos, con dos decimales.

---

## 1. Usuario

Personas que operan el sistema. Se usa para el login y para registrar quién hizo cada venta.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_usuario | INT IDENTITY | No | PK | Identificador del usuario |
| nombre_usuario | VARCHAR(50) | No | UNIQUE | Nombre con el que inicia sesión |
| clave | VARCHAR(255) | No | | Hash de la contraseña con su salt. Nunca texto plano |
| rol | VARCHAR(30) | No | CHECK IN ('Admin', 'Cajero') | Perfil de permisos |
| estado | BIT | No | DEFAULT 1 | Un usuario inactivo no puede iniciar sesión |

## 2. Pelicula

Catálogo de películas que el cine puede programar.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_pelicula | INT IDENTITY | No | PK | Identificador de la película |
| titulo | VARCHAR(100) | No | | Título comercial |
| genero | VARCHAR(50) | No | | Género principal (la app lo carga desde una lista cerrada) |
| duracion | INT | No | CHECK > 0 | Duración en minutos |
| estado | BIT | No | DEFAULT 1 | Una película inactiva no admite nuevas funciones |

## 3. Sala

Salas físicas del cine.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_sala | INT IDENTITY | No | PK | Identificador de la sala |
| nombre | VARCHAR(50) | No | UNIQUE | Nombre visible (ej. "Sala 1") |
| capacidad | INT | No | CHECK > 0 | Cantidad de butacas; tope de entradas por función |
| estado | BIT | No | DEFAULT 1 | Una sala inactiva no admite nuevas funciones |

## 4. Funcion

Proyección de una película en una sala, en una fecha y hora determinadas.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_funcion | INT IDENTITY | No | PK | Identificador de la función |
| id_pelicula | INT | No | FK → Pelicula(id_pelicula) | Película que se proyecta |
| id_sala | INT | No | FK → Sala(id_sala) | Sala donde se proyecta |
| fecha_hora | DATETIME | No | | Inicio de la función |
| precio_entrada | DECIMAL(10,2) | No | CHECK >= 0 | Precio vigente de cada entrada |
| formato | VARCHAR(10) | No | CHECK IN ('2D', '3D'), DEFAULT '2D' | Formato de proyección |

Restricción de tabla: `UNIQUE (id_sala, fecha_hora)` — una sala no puede tener dos funciones que empiecen al mismo tiempo.

## 5. Caja

Puntos de venta (boleterías) del cine.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_caja | INT IDENTITY | No | PK | Identificador de la caja |
| nombre | VARCHAR(30) | No | UNIQUE | Nombre visible (ej. "Caja 1") |
| estado | BIT | No | DEFAULT 1 | Una caja inactiva no registra ventas |

## 6. Venta

Cada operación de venta de entradas. Es la tabla central para el análisis.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_venta | INT IDENTITY | No | PK | Identificador de la venta |
| id_funcion | INT | No | FK → Funcion(id_funcion) | Función para la que se venden las entradas |
| id_caja | INT | No | FK → Caja(id_caja) | Caja donde se hizo la venta |
| id_usuario | INT | No | FK → Usuario(id_usuario) | Usuario que registró la venta |
| fecha_venta | DATETIME | No | DEFAULT GETDATE() | Momento de la operación |
| cantidad_entradas | INT | No | CHECK > 0 | Entradas vendidas en la operación |
| precio_unitario | DECIMAL(10,2) | No | CHECK >= 0 | Precio de la entrada al momento de vender (copia de `Funcion.precio_entrada`) |
| total | DECIMAL(12,2) | No | CHECK = cantidad_entradas × precio_unitario | Importe total de la operación |
| forma_pago | VARCHAR(20) | No | CHECK IN ('Efectivo', 'Debito', 'Credito', 'QR') | Medio de pago |

Nota: `total` es un dato derivado. Se guarda a propósito, junto con `precio_unitario`, para que la venta conserve su valor histórico aunque después cambie el precio de la función.

## 7. Objetivo_semanal_caja

Meta de recaudación de una caja para una semana.

| Campo | Tipo | Nulo | Clave / Restricción | Descripción |
|---|---|---|---|---|
| id_objetivo | INT IDENTITY | No | PK | Identificador del objetivo |
| id_caja | INT | No | FK → Caja(id_caja) | Caja a la que aplica |
| fecha_inicio | DATE | No | | Primer día de la semana |
| fecha_fin | DATE | No | CHECK > fecha_inicio | Último día de la semana |
| monto_objetivo | DECIMAL(12,2) | No | CHECK > 0 | Recaudación esperada |

Restricción de tabla: `UNIQUE (id_caja, fecha_inicio)` — una caja no puede tener dos objetivos para la misma semana.

---

## Relaciones

| Relación | Cardinalidad | FK |
|---|---|---|
| Pelicula — Funcion | 1 : N | Funcion.id_pelicula |
| Sala — Funcion | 1 : N | Funcion.id_sala |
| Funcion — Venta | 1 : N | Venta.id_funcion |
| Caja — Venta | 1 : N | Venta.id_caja |
| Usuario — Venta | 1 : N | Venta.id_usuario |
| Caja — Objetivo_semanal_caja | 1 : N | Objetivo_semanal_caja.id_caja |

Ninguna FK usa borrado en cascada: las bajas son lógicas.

## Reglas que no se expresan con constraints

Se validan en la capa de negocio (BLL):

1. **Cupo:** la suma de `cantidad_entradas` de una función no puede superar la `capacidad` de su sala.
2. **Solapamiento:** dos funciones de una misma sala no pueden pisarse, considerando `fecha_hora` y la `duracion` de la película.
3. No se venden entradas para funciones que ya empezaron.
4. No se crean funciones con películas o salas inactivas, ni ventas en cajas inactivas.
5. Siempre debe quedar al menos un usuario `Admin` activo.
