/* =====================================================================
   01_crear_bd.sql
   Crea la base de datos CineDB con todas sus tablas, restricciones
   e indices. Se puede volver a ejecutar: borra la base si ya existe.

   ATENCION: al re-ejecutarlo se pierden todos los datos cargados.

   (Los comentarios van sin tildes a proposito, para que el archivo se
   vea bien en cualquier editor sin importar la codificacion.)

   ---------------------------------------------------------------------
   COMO LEER ESTE SCRIPT

   1. Estructura general
      - Primero se borra y se vuelve a crear la base.
      - Despues se crean las tablas en ORDEN DE DEPENDENCIA: primero las
        que no apuntan a nadie (Usuario, Pelicula, Sala, Caja) y despues
        las que tienen claves foraneas (Funcion, Venta, Objetivo).
        Si se invierte el orden falla, porque una FK no puede apuntar a
        una tabla que todavia no existe.
      - Al final se crean los indices.

   2. Que es GO
      No es una instruccion de SQL: es un separador que entienden SSMS y
      sqlcmd. Corta el script en "lotes" que se envian al servidor de a
      uno. Hace falta porque algunas instrucciones (CREATE DATABASE,
      USE) tienen que terminar de ejecutarse antes de que arranque la
      siguiente.

   3. Tipos de restriccion (constraint) que se usan
      - PRIMARY KEY (PK): identifica cada fila. No admite repetidos ni
        NULL.
      - FOREIGN KEY (FK): obliga a que el valor exista en otra tabla.
        Es lo que garantiza la integridad referencial.
      - UNIQUE (UQ): no admite valores repetidos en esa columna (o
        combinacion de columnas), aunque no sea la PK.
      - CHECK (CK): una condicion que cada fila debe cumplir.
      - DEFAULT (DF): valor que se usa cuando el INSERT no informa la
        columna.
      - NOT NULL: la columna es obligatoria.

   4. Por que todas las constraints tienen nombre
      Si no se les pone nombre, SQL Server inventa uno del estilo
      "CK__Venta__total__3B75D760". Con nombre propio:
        - los mensajes de error se entienden (dicen que regla se rompio),
        - se pueden modificar o borrar despues con ALTER TABLE.
      Convencion usada: <prefijo>_<Tabla>_<columna o descripcion>
        PK_ clave primaria    FK_ clave foranea    UQ_ unique
        CK_ check             DF_ default          IX_ indice

   5. Por que las reglas estan en la base y no solo en la aplicacion
      Porque asi se cumplen siempre, venga el dato de la app en C#, de
      un script de carga o de alguien escribiendo un INSERT a mano. La
      aplicacion valida para dar mensajes amigables; la base valida
      para que el dato invalido no pueda existir.
   ===================================================================== */


/* ---------------------------------------------------------------------
   PASO 1: borrar la base si ya existe y crearla de nuevo
   --------------------------------------------------------------------- */

-- Nos paramos en "master", la base del sistema. No se puede borrar una
-- base de datos mientras se esta parado dentro de ella.
USE master;
GO

-- DB_ID devuelve el identificador de la base, o NULL si no existe.
-- Asi el script funciona tanto la primera vez como las siguientes.
IF DB_ID('CineDB') IS NOT NULL
BEGIN
    -- SQL Server no deja borrar una base que tiene conexiones abiertas
    -- (por ejemplo la aplicacion o una ventana de SSMS).
    -- SINGLE_USER deja una sola conexion (la nuestra) y
    -- ROLLBACK IMMEDIATE corta las demas y deshace lo que tuvieran a medias.
    ALTER DATABASE CineDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CineDB;
END
GO

CREATE DATABASE CineDB;
GO

-- A partir de aca, todo lo que se crea queda dentro de CineDB.
USE CineDB;
GO


/* ---------------------------------------------------------------------
   PASO 2: tablas sin dependencias
   No tienen claves foraneas, por eso se crean primero.
   --------------------------------------------------------------------- */

-- Usuario: quienes pueden entrar al sistema. Se usa para el login y
-- para saber quien registro cada venta.
CREATE TABLE Usuario (
    -- IDENTITY(1,1): la base genera el numero sola. Empieza en 1 y
    -- suma de a 1. En los INSERT esta columna no se informa.
    id_usuario      INT IDENTITY(1,1) NOT NULL,
    nombre_usuario  VARCHAR(50)       NOT NULL,
    -- Aca NO se guarda la contrasenia sino su hash (un resumen
    -- irreversible que calcula la aplicacion). Por eso mide 255: el
    -- hash es mucho mas largo que la contrasenia original.
    clave           VARCHAR(255)      NOT NULL,
    rol             VARCHAR(30)       NOT NULL,
    -- BIT es el booleano de SQL Server (no existe el tipo "bool").
    -- 1 = activo, 0 = dado de baja. Si no se informa, queda en 1.
    estado          BIT               NOT NULL CONSTRAINT DF_Usuario_estado DEFAULT (1),

    CONSTRAINT PK_Usuario PRIMARY KEY (id_usuario),
    -- No puede haber dos usuarios con el mismo nombre de login.
    CONSTRAINT UQ_Usuario_nombre_usuario UNIQUE (nombre_usuario),
    -- Solo existen estos dos roles. Cualquier otro texto se rechaza.
    CONSTRAINT CK_Usuario_rol CHECK (rol IN ('Admin', 'Cajero'))
);
GO

-- Pelicula: catalogo de peliculas que se pueden programar.
CREATE TABLE Pelicula (
    id_pelicula  INT IDENTITY(1,1) NOT NULL,
    titulo       VARCHAR(100)      NOT NULL,
    genero       VARCHAR(50)       NOT NULL,
    -- Duracion en minutos.
    duracion     INT               NOT NULL,
    estado       BIT               NOT NULL CONSTRAINT DF_Pelicula_estado DEFAULT (1),

    CONSTRAINT PK_Pelicula PRIMARY KEY (id_pelicula),
    -- Una pelicula de 0 o menos minutos no tiene sentido.
    CONSTRAINT CK_Pelicula_duracion CHECK (duracion > 0)
);
GO

-- Sala: salas fisicas del cine.
CREATE TABLE Sala (
    id_sala    INT IDENTITY(1,1) NOT NULL,
    nombre     VARCHAR(50)       NOT NULL,
    -- Cantidad de butacas. Es el tope de entradas que se pueden vender
    -- para cada funcion de esta sala (eso lo controla la aplicacion).
    capacidad  INT               NOT NULL,
    estado     BIT               NOT NULL CONSTRAINT DF_Sala_estado DEFAULT (1),

    CONSTRAINT PK_Sala PRIMARY KEY (id_sala),
    -- No puede haber dos salas con el mismo nombre.
    CONSTRAINT UQ_Sala_nombre UNIQUE (nombre),
    CONSTRAINT CK_Sala_capacidad CHECK (capacidad > 0)
);
GO

-- Caja: puntos de venta (boleterias).
CREATE TABLE Caja (
    id_caja  INT IDENTITY(1,1) NOT NULL,
    nombre   VARCHAR(30)       NOT NULL,
    estado   BIT               NOT NULL CONSTRAINT DF_Caja_estado DEFAULT (1),

    CONSTRAINT PK_Caja PRIMARY KEY (id_caja),
    CONSTRAINT UQ_Caja_nombre UNIQUE (nombre)
);
GO


/* ---------------------------------------------------------------------
   PASO 3: tablas dependientes
   Tienen claves foraneas hacia las tablas del paso 2.

   Sobre las FK: ninguna usa ON DELETE CASCADE. El comportamiento por
   defecto es NO ACTION, es decir: si se intenta borrar una pelicula que
   tiene funciones, la base lo RECHAZA. Es lo que queremos, porque las
   bajas son logicas (estado = 0) y no se debe perder historial.
   --------------------------------------------------------------------- */

-- Funcion: una pelicula, en una sala, en una fecha y hora.
-- Resuelve la relacion "muchos a muchos" entre Pelicula y Sala.
CREATE TABLE Funcion (
    id_funcion      INT IDENTITY(1,1) NOT NULL,
    id_pelicula     INT               NOT NULL,
    id_sala         INT               NOT NULL,
    fecha_hora      DATETIME          NOT NULL,
    -- DECIMAL(10,2): hasta 10 digitos en total, 2 de ellos decimales.
    -- Para plata se usa DECIMAL y nunca FLOAT, porque FLOAT redondea
    -- y los centavos dejan de cerrar.
    precio_entrada  DECIMAL(10,2)     NOT NULL,
    formato         VARCHAR(10)       NOT NULL CONSTRAINT DF_Funcion_formato DEFAULT ('2D'),

    CONSTRAINT PK_Funcion PRIMARY KEY (id_funcion),
    -- La pelicula y la sala tienen que existir en sus tablas.
    CONSTRAINT FK_Funcion_Pelicula FOREIGN KEY (id_pelicula) REFERENCES Pelicula (id_pelicula),
    CONSTRAINT FK_Funcion_Sala FOREIGN KEY (id_sala) REFERENCES Sala (id_sala),
    -- UNIQUE sobre DOS columnas: lo que no se puede repetir es la
    -- combinacion. Una sala puede tener muchas funciones y un horario
    -- puede tener muchas funciones, pero no la misma sala a la misma
    -- hora.
    -- Limite: solo detecta el mismo horario EXACTO. Que una funcion de
    -- las 20:00 se pise con otra de las 20:30 lo valida la aplicacion,
    -- porque depende de la duracion de la pelicula (otra tabla) y un
    -- CHECK no puede mirar otras filas ni otras tablas.
    CONSTRAINT UQ_Funcion_sala_fecha_hora UNIQUE (id_sala, fecha_hora),
    -- Se permite 0 (funcion gratuita), pero no negativo.
    CONSTRAINT CK_Funcion_precio_entrada CHECK (precio_entrada >= 0),
    CONSTRAINT CK_Funcion_formato CHECK (formato IN ('2D', '3D'))
);
GO

-- Venta: cada operacion de venta de entradas.
-- Es la tabla central para el analisis: cada fila dice que se vendio
-- (funcion), donde (caja), quien (usuario), cuando, cuanto y como.
CREATE TABLE Venta (
    id_venta           INT IDENTITY(1,1) NOT NULL,
    id_funcion         INT               NOT NULL,
    id_caja            INT               NOT NULL,
    id_usuario         INT               NOT NULL,
    -- Si el INSERT no informa la fecha, se usa la fecha y hora del
    -- servidor en ese momento.
    fecha_venta        DATETIME          NOT NULL CONSTRAINT DF_Venta_fecha_venta DEFAULT (GETDATE()),
    cantidad_entradas  INT               NOT NULL,
    -- Copia del precio de la funcion AL MOMENTO de vender. Parece
    -- redundante con Funcion.precio_entrada, pero es a proposito: si
    -- manana cambia el precio de la funcion, las ventas ya hechas
    -- conservan el valor al que realmente se vendieron.
    precio_unitario    DECIMAL(10,2)     NOT NULL,
    -- DECIMAL(12,2), mas grande que el precio, porque es precio x cantidad.
    total              DECIMAL(12,2)     NOT NULL,
    forma_pago         VARCHAR(20)       NOT NULL,

    CONSTRAINT PK_Venta PRIMARY KEY (id_venta),
    CONSTRAINT FK_Venta_Funcion FOREIGN KEY (id_funcion) REFERENCES Funcion (id_funcion),
    CONSTRAINT FK_Venta_Caja FOREIGN KEY (id_caja) REFERENCES Caja (id_caja),
    CONSTRAINT FK_Venta_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuario (id_usuario),
    CONSTRAINT CK_Venta_cantidad_entradas CHECK (cantidad_entradas > 0),
    CONSTRAINT CK_Venta_precio_unitario CHECK (precio_unitario >= 0),
    -- "total" es un dato derivado (se puede calcular). Se guarda igual
    -- para que las consultas de analisis sean simples, y este CHECK
    -- garantiza que nunca quede guardado un total que no coincida con
    -- cantidad x precio. Un CHECK si puede comparar columnas de la
    -- MISMA fila.
    CONSTRAINT CK_Venta_total CHECK (total = cantidad_entradas * precio_unitario),
    -- Sin tildes ('Debito', 'Credito') para evitar problemas de
    -- codificacion entre la base, C# y Power BI.
    CONSTRAINT CK_Venta_forma_pago CHECK (forma_pago IN ('Efectivo', 'Debito', 'Credito', 'QR'))
);
GO

-- Objetivo_semanal_caja: cuanto se espera recaudar en una caja durante
-- una semana. Se compara despues contra lo realmente vendido.
CREATE TABLE Objetivo_semanal_caja (
    id_objetivo     INT IDENTITY(1,1) NOT NULL,
    id_caja         INT               NOT NULL,
    -- DATE y no DATETIME: aca solo importa el dia, no la hora.
    fecha_inicio    DATE              NOT NULL,
    fecha_fin       DATE              NOT NULL,
    monto_objetivo  DECIMAL(12,2)     NOT NULL,

    CONSTRAINT PK_Objetivo_semanal_caja PRIMARY KEY (id_objetivo),
    CONSTRAINT FK_Objetivo_semanal_caja_Caja FOREIGN KEY (id_caja) REFERENCES Caja (id_caja),
    -- Una caja no puede tener dos objetivos que empiecen el mismo dia.
    CONSTRAINT UQ_Objetivo_semanal_caja_caja_inicio UNIQUE (id_caja, fecha_inicio),
    CONSTRAINT CK_Objetivo_semanal_caja_fechas CHECK (fecha_fin > fecha_inicio),
    CONSTRAINT CK_Objetivo_semanal_caja_monto CHECK (monto_objetivo > 0)
);
GO


/* ---------------------------------------------------------------------
   PASO 4: indices

   Un indice es como el indice de un libro: permite encontrar filas por
   una columna sin recorrer la tabla entera. Acelera las busquedas y
   los JOIN; a cambio ocupa espacio y hace un poco mas lentos los INSERT.

   SQL Server crea indices automaticamente para PRIMARY KEY y UNIQUE,
   pero NO para las FOREIGN KEY. Como las FK son justamente las
   columnas por las que se hacen los JOIN, se indexan a mano.

   No hace falta indexar Funcion.id_sala ni Objetivo_semanal_caja.id_caja:
   ya son la primera columna de un UNIQUE, y ese indice sirve.

   Venta.fecha_venta no es FK, pero casi todas las consultas de analisis
   filtran o agrupan por fecha ("ventas de la semana", "por mes").
   --------------------------------------------------------------------- */

CREATE INDEX IX_Funcion_id_pelicula ON Funcion (id_pelicula);
CREATE INDEX IX_Venta_id_funcion    ON Venta (id_funcion);
CREATE INDEX IX_Venta_id_caja       ON Venta (id_caja);
CREATE INDEX IX_Venta_id_usuario    ON Venta (id_usuario);
CREATE INDEX IX_Venta_fecha_venta   ON Venta (fecha_venta);
GO

PRINT 'Base de datos CineDB creada correctamente.';
GO
