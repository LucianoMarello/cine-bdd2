/* =====================================================================
   04_vistas_analisis.sql
   Crea las vistas que usa la herramienta de analisis (Power BI / Excel).
   Se puede ejecutar las veces que haga falta: no toca los datos.

   QUE ES UNA VISTA
   Una consulta guardada con nombre. Se usa como si fuera una tabla
   (SELECT * FROM vw_...), pero no guarda datos propios: cada vez que se
   la consulta, lee las tablas reales. Por eso siempre esta al dia.

   POR QUE USAR VISTAS PARA EL ANALISIS
   - El modelo esta normalizado (bien para cargar datos, incomodo para
     analizar): para saber "recaudacion por genero" hay que unir Venta,
     Funcion y Pelicula. La vista ya trae todo unido en una sola fila.
   - Los calculos (ocupacion, cumplimiento, dia de la semana) quedan
     definidos una sola vez, en la base, y cualquier herramienta que se
     conecte obtiene el mismo resultado.
   - La herramienta de analisis no necesita conocer las tablas.

   LAS TRES VISTAS
     vw_ventas_detalle     una fila por venta, con todos sus datos
     vw_ocupacion_funcion  una fila por funcion, con lo que vendio
     vw_objetivo_vs_real   una fila por caja y semana, objetivo vs real

   Nota: el dia de la semana se calcula contando dias desde el 01/01/1900
   (que fue lunes), para no depender del idioma del servidor.
   CREATE OR ALTER crea la vista o la reemplaza si ya existe.
   ===================================================================== */

USE CineDB;
GO

/* ---------------------------------------------------------------------
   vw_ventas_detalle
   Una fila por venta. Es la vista principal: sirve para casi cualquier
   pregunta sobre recaudacion y entradas (por dia, pelicula, genero,
   sala, caja, vendedor, forma de pago, horario...).
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_ventas_detalle
AS
SELECT
    v.id_venta,

    -- Cuando se vendio
    v.fecha_venta,
    CAST(v.fecha_venta AS DATE)                              AS fecha,
    YEAR(v.fecha_venta)                                      AS anio,
    MONTH(v.fecha_venta)                                     AS mes,
    DATEPART(HOUR, v.fecha_venta)                            AS hora_venta,
    DATEADD(DAY, -(DATEDIFF(DAY, '19000101', v.fecha_venta) % 7),
            CAST(v.fecha_venta AS DATE))                     AS semana_inicio,   -- lunes de esa semana

    -- La funcion
    f.id_funcion,
    f.fecha_hora                                             AS funcion_fecha_hora,
    CAST(f.fecha_hora AS DATE)                               AS funcion_fecha,
    CONVERT(CHAR(5), f.fecha_hora, 108)                      AS funcion_horario, -- '20:00'
    DATEDIFF(DAY, '19000101', f.fecha_hora) % 7 + 1          AS funcion_dia_nro, -- 1 = lunes ... 7 = domingo
    CASE DATEDIFF(DAY, '19000101', f.fecha_hora) % 7
        WHEN 0 THEN 'Lunes'   WHEN 1 THEN 'Martes' WHEN 2 THEN 'Miercoles'
        WHEN 3 THEN 'Jueves'  WHEN 4 THEN 'Viernes' WHEN 5 THEN 'Sabado'
        ELSE 'Domingo' END                                   AS funcion_dia,
    CASE WHEN DATEDIFF(DAY, '19000101', f.fecha_hora) % 7 >= 4
         THEN 'Fin de semana' ELSE 'Dia de semana' END       AS tipo_dia,        -- viernes a domingo
    f.formato,

    -- La pelicula
    p.id_pelicula,
    p.titulo                                                 AS pelicula,
    p.genero,
    p.duracion                                               AS duracion_minutos,

    -- La sala
    s.id_sala,
    s.nombre                                                 AS sala,
    s.capacidad                                              AS sala_capacidad,

    -- Donde y quien vendio
    c.id_caja,
    c.nombre                                                 AS caja,
    u.id_usuario,
    u.nombre_usuario                                         AS vendedor,

    -- Los numeros
    v.cantidad_entradas,
    v.precio_unitario,
    v.total,
    v.forma_pago,

    -- Venta anticipada = se compro un dia anterior al de la funcion
    CASE WHEN CAST(v.fecha_venta AS DATE) < CAST(f.fecha_hora AS DATE)
         THEN 'Anticipada' ELSE 'Mismo dia' END              AS tipo_venta
FROM        Venta    v
JOIN        Funcion  f ON f.id_funcion  = v.id_funcion
JOIN        Pelicula p ON p.id_pelicula = f.id_pelicula
JOIN        Sala     s ON s.id_sala     = f.id_sala
JOIN        Caja     c ON c.id_caja     = v.id_caja
JOIN        Usuario  u ON u.id_usuario  = v.id_usuario;
GO

/* ---------------------------------------------------------------------
   vw_ocupacion_funcion
   Una fila por funcion, tenga o no ventas (por eso LEFT JOIN: con JOIN
   comun, las funciones sin ventas desaparecerian y la ocupacion
   promedio daria mas alta de lo real).

   Las ventas se suman primero en una subconsulta y despues se unen a la
   funcion: una fila por funcion, sin riesgo de duplicar.

   ocupacion_pct = entradas vendidas / capacidad de la sala x 100
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_ocupacion_funcion
AS
SELECT
    f.id_funcion,
    f.fecha_hora,
    CAST(f.fecha_hora AS DATE)                               AS fecha,
    YEAR(f.fecha_hora)                                       AS anio,
    MONTH(f.fecha_hora)                                      AS mes,
    CONVERT(CHAR(5), f.fecha_hora, 108)                      AS horario,
    DATEDIFF(DAY, '19000101', f.fecha_hora) % 7 + 1          AS dia_nro,
    CASE DATEDIFF(DAY, '19000101', f.fecha_hora) % 7
        WHEN 0 THEN 'Lunes'   WHEN 1 THEN 'Martes' WHEN 2 THEN 'Miercoles'
        WHEN 3 THEN 'Jueves'  WHEN 4 THEN 'Viernes' WHEN 5 THEN 'Sabado'
        ELSE 'Domingo' END                                   AS dia,
    CASE WHEN DATEDIFF(DAY, '19000101', f.fecha_hora) % 7 >= 4
         THEN 'Fin de semana' ELSE 'Dia de semana' END       AS tipo_dia,
    p.titulo                                                 AS pelicula,
    p.genero,
    s.nombre                                                 AS sala,
    s.capacidad,
    f.formato,
    f.precio_entrada,
    ISNULL(t.entradas, 0)                                    AS entradas_vendidas,
    s.capacidad - ISNULL(t.entradas, 0)                      AS entradas_libres,
    CAST(100.0 * ISNULL(t.entradas, 0) / s.capacidad AS DECIMAL(5,2)) AS ocupacion_pct,
    ISNULL(t.recaudado, 0)                                   AS recaudacion,
    ISNULL(t.ventas, 0)                                      AS cantidad_ventas,
    CASE WHEN ISNULL(t.entradas, 0) >= s.capacidad THEN 'Si' ELSE 'No' END AS agotada,
    -- Las funciones futuras todavia pueden vender: conviene filtrarlas
    -- al calcular ocupacion promedio.
    CASE WHEN f.fecha_hora < GETDATE() THEN 'Si' ELSE 'No' END             AS ya_realizada
FROM        Funcion  f
JOIN        Pelicula p ON p.id_pelicula = f.id_pelicula
JOIN        Sala     s ON s.id_sala     = f.id_sala
LEFT JOIN  (SELECT  id_funcion,
                    SUM(cantidad_entradas) AS entradas,
                    SUM(total)             AS recaudado,
                    COUNT(*)               AS ventas
            FROM    Venta
            GROUP BY id_funcion) t ON t.id_funcion = f.id_funcion;
GO

/* ---------------------------------------------------------------------
   vw_objetivo_vs_real
   Una fila por objetivo (caja + semana), comparado con lo que esa caja
   recaudo en esa semana.

   Una venta pertenece a la semana si su fecha esta entre fecha_inicio y
   fecha_fin INCLUSIVE. Como fecha_venta tiene hora, se compara con
   "menor al dia siguiente de fecha_fin" para no perder las ventas del
   domingo despues de las 00:00.

   resultado:
     'Cumplido'     recaudado >= objetivo
     'No cumplido'  la semana ya termino y no se llego
     'En curso'     la semana todavia no termino y aun no se llego
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_objetivo_vs_real
AS
SELECT
    o.id_objetivo,
    c.id_caja,
    c.nombre                                                 AS caja,
    o.fecha_inicio,
    o.fecha_fin,
    YEAR(o.fecha_inicio)                                     AS anio,
    MONTH(o.fecha_inicio)                                    AS mes,
    o.monto_objetivo,
    ISNULL(t.recaudado, 0)                                   AS recaudado,
    ISNULL(t.recaudado, 0) - o.monto_objetivo                AS diferencia,
    CAST(100.0 * ISNULL(t.recaudado, 0) / o.monto_objetivo AS DECIMAL(7,2)) AS cumplimiento_pct,
    ISNULL(t.ventas, 0)                                      AS cantidad_ventas,
    ISNULL(t.entradas, 0)                                    AS entradas_vendidas,
    CASE WHEN ISNULL(t.recaudado, 0) >= o.monto_objetivo THEN 'Cumplido'
         WHEN o.fecha_fin >= CAST(GETDATE() AS DATE)      THEN 'En curso'
         ELSE 'No cumplido' END                              AS resultado
FROM        Objetivo_semanal_caja o
JOIN        Caja c ON c.id_caja = o.id_caja
OUTER APPLY (SELECT  SUM(v.total)             AS recaudado,
                     COUNT(*)                 AS ventas,
                     SUM(v.cantidad_entradas) AS entradas
             FROM    Venta v
             WHERE   v.id_caja     = o.id_caja
               AND   v.fecha_venta >= o.fecha_inicio
               AND   v.fecha_venta <  DATEADD(DAY, 1, o.fecha_fin)) t;
GO

PRINT 'Vistas de analisis creadas: vw_ventas_detalle, vw_ocupacion_funcion, vw_objetivo_vs_real.';
GO
