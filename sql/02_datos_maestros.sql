/* =====================================================================
   02_datos_maestros.sql
   Carga los datos "fijos" del cine: salas, cajas, peliculas y cajeros.
   Ejecutar DESPUES de 01_crear_bd.sql, con la base recien creada.

   Orden completo para dejar la base lista:
     1) 01_crear_bd.sql
     2) 02_datos_maestros.sql        (este)
     3) 03_datos_prueba_ventas.sql
     4) Abrir la aplicacion: como todavia no hay administrador, pide
        crearlo en ese momento.

   SOBRE LOS USUARIOS
   La clave se guarda como un hash que calcula la aplicacion (PBKDF2), y
   SQL no puede generarlo. Por eso:
     - El administrador NO se crea aca: lo crea la aplicacion al abrirla.
     - Los cajeros se cargan con la clave 'SIN_CLAVE', que no es un hash
       valido: existen (hacen falta para asignarles ventas) pero no
       pueden iniciar sesion hasta que el administrador les ponga una
       clave desde Administracion > Usuarios.
   ===================================================================== */

USE CineDB;
GO

SET NOCOUNT ON;

-- Proteccion: si ya hay datos, no se carga nada (evita duplicados).
IF EXISTS (SELECT 1 FROM Sala) OR EXISTS (SELECT 1 FROM Caja) OR EXISTS (SELECT 1 FROM Pelicula)
BEGIN
    RAISERROR('La base ya tiene datos maestros. Ejecuta primero 01_crear_bd.sql para empezar de cero.', 16, 1);
    RETURN;
END

/* ---------------------------------------------------------------------
   Salas: de distinta capacidad, para que la ocupacion se pueda comparar.
   --------------------------------------------------------------------- */
INSERT INTO Sala (nombre, capacidad) VALUES
    ('Sala 1', 180),
    ('Sala 2', 150),
    ('Sala 3', 120),
    ('Sala 4', 90),
    ('Sala 5', 60);

/* ---------------------------------------------------------------------
   Cajas (boleterias)
   --------------------------------------------------------------------- */
INSERT INTO Caja (nombre) VALUES
    ('Caja 1'),
    ('Caja 2'),
    ('Caja 3');

/* ---------------------------------------------------------------------
   Cajeros (sin clave utilizable: ver nota del encabezado)
   --------------------------------------------------------------------- */
INSERT INTO Usuario (nombre_usuario, clave, rol) VALUES
    ('cajero1', 'SIN_CLAVE', 'Cajero'),
    ('cajero2', 'SIN_CLAVE', 'Cajero'),
    ('cajero3', 'SIN_CLAVE', 'Cajero'),
    ('cajero4', 'SIN_CLAVE', 'Cajero');

/* ---------------------------------------------------------------------
   Peliculas: titulos inventados. Los generos son los mismos de la lista
   cerrada de la aplicacion (BE/Pelicula.cs), sin tildes.
   Ninguna supera los 160 minutos: asi entran en los horarios de funcion
   que arma el script 03 (17:00, 20:00, 22:45) sin superponerse.
   --------------------------------------------------------------------- */
INSERT INTO Pelicula (titulo, genero, duracion) VALUES
    ('El ultimo tren a Bariloche',    'Drama',            128),
    ('Guardianes del Aconcagua',      'Aventura',         135),
    ('Operacion Rio de la Plata',     'Accion',           118),
    ('Mate para dos',                 'Comedia',           98),
    ('La casa de la calle Defensa',   'Terror',           104),
    ('Estacion Orion',                'Ciencia ficcion',  142),
    ('Pampa y el zorro valiente',     'Animacion',         92),
    ('Cartas desde Ushuaia',          'Romance',          110),
    ('El caso Belgrano',              'Suspenso',         121),
    ('Glaciares: el fin del hielo',   'Documental',        88),
    ('La leyenda del Nahuel',         'Fantasia',         138),
    ('Persecucion en la 9 de Julio',  'Accion',           112),
    ('Los vecinos del 4to B',         'Comedia',           95),
    ('Robotin y la ciudad perdida',   'Animacion',         89),
    ('No abras el sotano',            'Terror',            99),
    ('Colonia Marte',                 'Ciencia ficcion',  150),
    ('Un verano en Mar del Plata',    'Romance',          105),
    ('El testigo silencioso',         'Suspenso',         117),
    ('Tierra de nadie',               'Drama',            133),
    ('El reino bajo el lago',         'Fantasia',         126);

SELECT  (SELECT COUNT(*) FROM Sala)     AS salas,
        (SELECT COUNT(*) FROM Caja)     AS cajas,
        (SELECT COUNT(*) FROM Pelicula) AS peliculas,
        (SELECT COUNT(*) FROM Usuario WHERE rol = 'Cajero') AS cajeros;

PRINT 'Datos maestros cargados.';
GO
