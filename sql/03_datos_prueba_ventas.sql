/* =====================================================================
   03_datos_prueba_ventas.sql
   Genera unos 6 meses de actividad del cine: funciones, ventas y
   objetivos semanales. Ejecutar DESPUES de 02_datos_maestros.sql.

   Los datos son aleatorios (cada ejecucion da numeros distintos), pero
   siguen patrones realistas para que el analisis tenga algo que mostrar:

     - Mas publico los fines de semana y en el horario de las 20:00.
     - Miercoles a mitad de precio: mas entradas, menos recaudacion.
     - Cada pelicula esta unas 7 semanas en cartelera y va perdiendo
       publico semana a semana.
     - Hay peliculas exitosas y fracasos (popularidad distinta).
     - Los precios suben un 3% por mes; las funciones 3D cuestan mas.
     - Caja 1 vende mas que Caja 2, y esta mas que Caja 3.
     - Dos cajeros trabajan a la tarde y dos a la noche.
     - Algunos objetivos semanales se cumplen y otros no.

   El periodo es relativo a la fecha en que se ejecuta: 26 semanas hacia
   atras con ventas, mas 14 dias de funciones futuras SIN ventas, para
   poder vender desde la aplicacion.

   Se respetan todas las reglas del sistema: ninguna funcion supera la
   capacidad de su sala, ninguna se superpone con otra en la misma sala,
   y toda venta es anterior al comienzo de su funcion.

   TECNICA
   Casi todo se resuelve "por conjuntos" (INSERT ... SELECT) en lugar de
   con bucles: es la forma natural de trabajar en SQL y es mucho mas
   rapida. Los numeros al azar salen de ABS(CHECKSUM(NEWID())) % N, que
   da un entero entre 0 y N-1 distinto para cada fila. Se guardan primero
   en tablas temporales (#...) para que cada fila use siempre el mismo
   numero en todos los calculos.
   ===================================================================== */

USE CineDB;
GO

SET NOCOUNT ON;

-- Protecciones
IF NOT EXISTS (SELECT 1 FROM Pelicula) OR NOT EXISTS (SELECT 1 FROM Sala)
   OR NOT EXISTS (SELECT 1 FROM Caja) OR NOT EXISTS (SELECT 1 FROM Usuario WHERE rol = 'Cajero')
BEGIN
    RAISERROR('Faltan datos maestros. Ejecuta primero 02_datos_maestros.sql.', 16, 1);
    RETURN;
END

IF EXISTS (SELECT 1 FROM Funcion)
BEGIN
    RAISERROR('Ya hay funciones cargadas. Para regenerar los datos, empeza de nuevo desde 01_crear_bd.sql.', 16, 1);
    RETURN;
END

/* ---------------------------------------------------------------------
   PASO 1: periodo
   El 01/01/1900 fue lunes: los dias transcurridos desde esa fecha,
   modulo 7, dan el dia de la semana (0 = lunes ... 6 = domingo) sin
   depender de la configuracion regional del servidor.
   --------------------------------------------------------------------- */
DECLARE @ahora        DATETIME = GETDATE();
DECLARE @hoy          DATE     = CAST(@ahora AS DATE);
DECLARE @lunes_actual DATE     = DATEADD(DAY, -(DATEDIFF(DAY, '19000101', @hoy) % 7), @hoy);
DECLARE @inicio       DATE     = DATEADD(WEEK, -26, @lunes_actual);
DECLARE @fin          DATE     = DATEADD(DAY, 14, @hoy);

-- Tabla de numeros 1..400: sirve para "fabricar" filas (dias, ventas).
SELECT TOP (400) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
INTO   #numeros
FROM   sys.all_objects;

-- Un renglon por dia del periodo.
SELECT  DATEADD(DAY, n - 1, @inicio)                                          AS fecha,
        DATEDIFF(DAY, '19000101', DATEADD(DAY, n - 1, @inicio)) % 7            AS dia_semana
INTO    #dias
FROM    #numeros
WHERE   DATEADD(DAY, n - 1, @inicio) <= @fin;

/* ---------------------------------------------------------------------
   PASO 2: cartelera
   Cada pelicula se estrena 10 dias despues de la anterior y queda 7
   semanas. "popularidad" (0 a 1) es que tan bien le va: no es un dato
   del modelo, solo se usa aca para generar ventas creibles.
   --------------------------------------------------------------------- */
SELECT  p.id_pelicula,
        p.genero,
        x.popularidad,
        DATEADD(DAY, (x.orden - 1) * 10 - 30, @inicio)       AS estreno,
        DATEADD(DAY, (x.orden - 1) * 10 - 30 + 48, @inicio)  AS fin_cartelera
INTO    #cartelera
FROM    (VALUES
            ('El ultimo tren a Bariloche',    1, 0.60),
            ('Guardianes del Aconcagua',      2, 0.85),
            ('Mate para dos',                 3, 0.70),
            ('La casa de la calle Defensa',   4, 0.55),
            ('Estacion Orion',                5, 0.95),
            ('Pampa y el zorro valiente',     6, 1.00),
            ('Cartas desde Ushuaia',          7, 0.45),
            ('El caso Belgrano',              8, 0.65),
            ('Glaciares: el fin del hielo',   9, 0.25),
            ('Operacion Rio de la Plata',    10, 0.90),
            ('La leyenda del Nahuel',        11, 0.75),
            ('Los vecinos del 4to B',        12, 0.80),
            ('No abras el sotano',           13, 0.50),
            ('Robotin y la ciudad perdida',  14, 0.95),
            ('Un verano en Mar del Plata',   15, 0.40),
            ('Colonia Marte',                16, 0.85),
            ('El testigo silencioso',        17, 0.55),
            ('Tierra de nadie',              18, 0.35),
            ('Persecucion en la 9 de Julio', 19, 0.90),
            ('El reino bajo el lago',        20, 0.70)
        ) AS x (titulo, orden, popularidad)
JOIN    Pelicula p ON p.titulo = x.titulo;

IF NOT EXISTS (SELECT 1 FROM #cartelera)
BEGIN
    RAISERROR('Las peliculas cargadas no son las de 02_datos_maestros.sql.', 16, 1);
    RETURN;
END

/* ---------------------------------------------------------------------
   PASO 3: grilla de funciones
   Horarios fijos por sala. La funcion de las 14:00 solo se da de viernes
   a domingo. Como ninguna pelicula dura mas de 160 minutos, los horarios
   nunca se pisan.
   --------------------------------------------------------------------- */
SELECT  IDENTITY(INT, 1, 1)                                       AS id_base,
        d.fecha,
        d.dia_semana,
        s.id_sala,
        s.nombre                                                  AS sala,
        s.capacidad,
        h.factor_horario,
        DATEADD(MINUTE, h.minutos, CAST(d.fecha AS DATETIME))     AS fecha_hora,
        ABS(CHECKSUM(NEWID())) % 100                              AS azar_formato,
        ABS(CHECKSUM(NEWID())) % 1000                             AS azar_ruido
INTO    #grilla
FROM    #dias d
CROSS JOIN Sala s
CROSS JOIN (VALUES  (14 * 60,       0.55, 1),     -- 14:00, solo fin de semana
                    (17 * 60,       0.80, 0),     -- 17:00
                    (20 * 60,       1.00, 0),     -- 20:00
                    (22 * 60 + 45,  0.70, 0)      -- 22:45
           ) AS h (minutos, factor_horario, solo_finde)
WHERE   s.estado = 1
  AND   (h.solo_finde = 0 OR d.dia_semana >= 4);

-- Que pelicula va en cada funcion: una al azar entre las que estan en
-- cartelera ese dia. Dividir por la popularidad hace que las exitosas
-- salgan sorteadas mas seguido.
SELECT  id_base, id_pelicula, genero, popularidad, estreno
INTO    #eleccion
FROM    (
            SELECT  g.id_base, c.id_pelicula, c.genero, c.popularidad, c.estreno,
                    ROW_NUMBER() OVER (PARTITION BY g.id_base
                                       ORDER BY (ABS(CHECKSUM(NEWID())) % 1000 + 1) / c.popularidad) AS orden
            FROM    #grilla g
            JOIN    #cartelera c ON g.fecha BETWEEN c.estreno AND c.fin_cartelera
        ) x
WHERE   orden = 1;

/* ---------------------------------------------------------------------
   PASO 4: insertar las funciones
   Precio: $6000 de base, +3% por cada mes transcurrido, +25% si es 3D,
   y mitad de precio los miercoles. Se redondea a la centena.
   3D: solo en Sala 1 y Sala 2, para generos "de pantalla grande", y en
   la mitad de los casos.
   --------------------------------------------------------------------- */
INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada, formato)
SELECT  e.id_pelicula,
        g.id_sala,
        g.fecha_hora,
        CAST(ROUND(   6000
                    * POWER(CAST(1.03 AS FLOAT), DATEDIFF(MONTH, @inicio, g.fecha))
                    * CASE WHEN f.es_3d = 1 THEN 1.25 ELSE 1 END
                    * CASE WHEN g.dia_semana = 2 THEN 0.5 ELSE 1 END
                  , -2) AS DECIMAL(10,2)),
        CASE WHEN f.es_3d = 1 THEN '3D' ELSE '2D' END
FROM    #grilla g
JOIN    #eleccion e ON e.id_base = g.id_base
CROSS APPLY (SELECT CASE WHEN g.sala IN ('Sala 1', 'Sala 2')
                          AND e.genero IN ('Animacion', 'Accion', 'Aventura', 'Ciencia ficcion', 'Fantasia')
                          AND g.azar_formato < 50
                         THEN 1 ELSE 0 END AS es_3d) f
ORDER BY g.fecha_hora, g.id_sala;

/* ---------------------------------------------------------------------
   PASO 5: cuantas entradas se venden en cada funcion ya pasada

   ocupacion = popularidad de la pelicula
             x factor del dia de la semana
             x factor del horario
             x desgaste (semanas en cartelera)
             x ruido al azar (entre 0.7 y 1.3)

   Nunca pasa de 1 (sala llena). Las funciones futuras no reciben ventas.
   --------------------------------------------------------------------- */
SELECT  fu.id_funcion,
        fu.fecha_hora,
        fu.precio_entrada,
        CAST(FLOOR(g.capacidad *
            CASE WHEN o.ocupacion > 1 THEN 1 ELSE o.ocupacion END) AS INT) AS entradas_objetivo
INTO    #cupo
FROM    Funcion fu
JOIN    #grilla   g ON g.id_sala = fu.id_sala AND g.fecha_hora = fu.fecha_hora
JOIN    #eleccion e ON e.id_base = g.id_base
CROSS APPLY (SELECT   e.popularidad
                    * CASE g.dia_semana WHEN 0 THEN 0.35 WHEN 1 THEN 0.35 WHEN 2 THEN 0.65
                                        WHEN 3 THEN 0.45 WHEN 4 THEN 0.80 WHEN 5 THEN 1.00
                                        ELSE 0.90 END
                    * g.factor_horario
                    * CASE DATEDIFF(DAY, e.estreno, g.fecha) / 7
                           WHEN 0 THEN 1.00 WHEN 1 THEN 0.85 WHEN 2 THEN 0.70
                           WHEN 3 THEN 0.55 WHEN 4 THEN 0.45 WHEN 5 THEN 0.35
                           ELSE 0.30 END
                    * (0.7 + g.azar_ruido / 1000.0 * 0.6) AS ocupacion) o
WHERE   fu.fecha_hora < @ahora;

/* ---------------------------------------------------------------------
   PASO 6: armar las ventas

   6a) Por cada funcion se generan "candidatas": tantas filas como
       entradas a vender (sobran, porque cada venta lleva 1 o mas).
       Cada candidata guarda sus numeros al azar.
   --------------------------------------------------------------------- */
SELECT  c.id_funcion,
        c.fecha_hora,
        c.precio_entrada,
        c.entradas_objetivo,
        n.n,
        ABS(CHECKSUM(NEWID())) % 100    AS azar_cantidad,
        ABS(CHECKSUM(NEWID())) % 100    AS azar_caja,
        ABS(CHECKSUM(NEWID())) % 100    AS azar_pago,
        ABS(CHECKSUM(NEWID())) % 100    AS azar_anticipada,
        ABS(CHECKSUM(NEWID())) % 10000  AS azar_minutos,
        ABS(CHECKSUM(NEWID())) % 2      AS azar_cajero
INTO    #candidatas
FROM    #cupo c
JOIN    #numeros n ON n.n <= c.entradas_objetivo;

/* 6b) Se calcula cada dato de la venta a partir de su numero al azar:
       - cantidad: lo mas comun es comprar 2 entradas.
       - fecha: 3 de cada 4 ventas son el mismo dia, hasta 4 horas antes
         de la funcion; el resto, anticipadas (1 a 3 dias antes, entre
         las 12:00 y las 22:00).
       - acumulado: suma corrida de entradas dentro de la funcion. Es lo
         que permite cortar justo cuando se llega al cupo.              */
SELECT  id_funcion, precio_entrada, entradas_objetivo, azar_caja, azar_pago, azar_cajero,
        v.cantidad,
        v.fecha_venta,
        SUM(v.cantidad) OVER (PARTITION BY id_funcion ORDER BY n ROWS UNBOUNDED PRECEDING) AS acumulado
INTO    #ventas
FROM    #candidatas
CROSS APPLY (SELECT
                CASE WHEN azar_cantidad < 15 THEN 1 WHEN azar_cantidad < 60 THEN 2
                     WHEN azar_cantidad < 75 THEN 3 WHEN azar_cantidad < 90 THEN 4
                     WHEN azar_cantidad < 96 THEN 5 ELSE 6 END AS cantidad,
                CASE WHEN azar_anticipada < 75
                     THEN DATEADD(MINUTE, -(5 + azar_minutos % 236), fecha_hora)
                     ELSE DATEADD(MINUTE, 720 + azar_minutos % 600,
                                  CAST(DATEADD(DAY, -(1 + azar_minutos % 3), CAST(fecha_hora AS DATE)) AS DATETIME))
                END AS fecha_venta) v;

-- Cajas y cajeros numerados 1, 2, 3... segun su id, para poder elegirlos
-- por posicion sin depender de los valores de IDENTITY.
SELECT id_caja, ROW_NUMBER() OVER (ORDER BY id_caja) AS posicion
INTO   #cajas FROM Caja WHERE estado = 1;

SELECT id_usuario, ROW_NUMBER() OVER (ORDER BY id_usuario) AS posicion
INTO   #cajeros FROM Usuario WHERE rol = 'Cajero';

DECLARE @cant_cajas   INT = (SELECT COUNT(*) FROM #cajas);
DECLARE @cant_cajeros INT = (SELECT COUNT(*) FROM #cajeros);

/* 6c) Insertar. Solo entran las candidatas cuyo acumulado no pasa el
       cupo de la funcion: asi es imposible sobrevender.
       - Caja: 50% la primera, 30% la segunda, 20% la tercera.
       - Cajero: los dos primeros atienden hasta las 18:00 y los otros
         dos despues.
       - Forma de pago: 25% efectivo, 30% debito, 25% credito, 20% QR.   */
INSERT INTO Venta (id_funcion, id_caja, id_usuario, fecha_venta, cantidad_entradas, precio_unitario, total, forma_pago)
SELECT  v.id_funcion,
        ca.id_caja,
        cj.id_usuario,
        v.fecha_venta,
        v.cantidad,
        v.precio_entrada,
        v.cantidad * v.precio_entrada,
        CASE WHEN v.azar_pago < 25 THEN 'Efectivo' WHEN v.azar_pago < 55 THEN 'Debito'
             WHEN v.azar_pago < 80 THEN 'Credito' ELSE 'QR' END
FROM    #ventas v
JOIN    #cajas ca
        ON ca.posicion = CASE WHEN @cant_cajas < 3 THEN 1
                              WHEN v.azar_caja < 50 THEN 1
                              WHEN v.azar_caja < 80 THEN 2 ELSE 3 END
JOIN    #cajeros cj
        ON cj.posicion = CASE WHEN @cant_cajeros < 4 THEN 1
                              WHEN DATEPART(HOUR, v.fecha_venta) < 18 THEN 1 + v.azar_cajero
                              ELSE 3 + v.azar_cajero END
WHERE   v.acumulado <= v.entradas_objetivo
ORDER BY v.fecha_venta;

/* ---------------------------------------------------------------------
   PASO 7: objetivos semanales por caja
   - Semanas ya cerradas: lo que la caja realmente recaudo, multiplicado
     por un factor al azar entre 0.85 y 1.20. Si el factor es menor a 1
     el objetivo quedo por debajo de lo vendido (cumplido); si es mayor,
     no se cumplio.
   - Semana actual y la siguiente: el promedio de las ultimas 4 semanas
     cerradas, mas un 5%.
   Se redondea a miles.
   --------------------------------------------------------------------- */
SELECT  DATEADD(WEEK, n - 1, @inicio) AS lunes
INTO    #semanas
FROM    #numeros
WHERE   DATEADD(WEEK, n - 1, @inicio) <= DATEADD(WEEK, 1, @lunes_actual);

SELECT  s.lunes, c.id_caja, SUM(ISNULL(v.total, 0)) AS recaudado
INTO    #recaudado
FROM    #semanas s
CROSS JOIN Caja c
LEFT JOIN Venta v ON v.id_caja = c.id_caja
                 AND v.fecha_venta >= s.lunes
                 AND v.fecha_venta <  DATEADD(DAY, 7, s.lunes)
GROUP BY s.lunes, c.id_caja;

INSERT INTO Objetivo_semanal_caja (id_caja, fecha_inicio, fecha_fin, monto_objetivo)
SELECT  r.id_caja,
        r.lunes,
        DATEADD(DAY, 6, r.lunes),
        CASE WHEN r.lunes < @lunes_actual
             THEN ROUND(r.recaudado * (0.85 + (ABS(CHECKSUM(NEWID())) % 351) / 1000.0), -3)
             ELSE ROUND(ult.promedio * 1.05, -3)
        END
FROM    #recaudado r
CROSS APPLY (SELECT AVG(r2.recaudado) AS promedio
             FROM   #recaudado r2
             WHERE  r2.id_caja = r.id_caja
               AND  r2.lunes >= DATEADD(WEEK, -4, @lunes_actual)
               AND  r2.lunes <  @lunes_actual) ult
WHERE   CASE WHEN r.lunes < @lunes_actual THEN r.recaudado ELSE ult.promedio END >= 1000
ORDER BY r.lunes, r.id_caja;

/* ---------------------------------------------------------------------
   Resumen
   --------------------------------------------------------------------- */
SELECT  @inicio                                                   AS desde,
        @fin                                                      AS hasta,
        (SELECT COUNT(*) FROM Funcion)                            AS funciones,
        (SELECT COUNT(*) FROM Funcion WHERE fecha_hora > @ahora)  AS funciones_futuras,
        (SELECT COUNT(*) FROM Venta)                              AS ventas,
        (SELECT SUM(cantidad_entradas) FROM Venta)                AS entradas,
        (SELECT SUM(total) FROM Venta)                            AS recaudacion,
        (SELECT COUNT(*) FROM Objetivo_semanal_caja)              AS objetivos;

PRINT 'Datos de prueba generados.';
GO
