/* =====================================================================
   05_kpis.sql
   Indicadores clave (KPI) del cine, calculados en la base de datos.
   Ejecutar DESPUES de 04_vistas_analisis.sql: estas vistas se apoyan en
   aquellas. Se puede ejecutar las veces que haga falta: no toca datos.

   Cada KPI es una vista, asi Power BI / Excel la carga como una tabla
   mas y solo tiene que mostrarla. El calculo queda en un unico lugar.

     Vista                           KPI
     ------------------------------  --------------------------------------
     vw_kpi_resumen                  Tarjetas generales (una sola fila)
     vw_kpi_recaudacion_semanal      Recaudacion, entradas, ticket promedio
                                     y variacion respecto de la semana anterior
     vw_kpi_ocupacion_sala           Ocupacion y rendimiento por sala
     vw_kpi_ocupacion_dia_horario    Ocupacion por dia de la semana y horario
     vw_kpi_peliculas                Ranking de peliculas (entradas y ocupacion)
     vw_kpi_objetivos_caja           Cumplimiento de objetivos por caja
     vw_kpi_baja_ocupacion_mensual   Funciones con baja ocupacion, por mes

   COMO SE MIDE LA OCUPACION
   Ocupacion = entradas vendidas / butacas ofrecidas x 100, sumando todas
   las funciones del grupo (promedio PONDERADO). Ejemplo: una funcion
   llena en una sala de 60 y una vacia en una de 180 dan 60/240 = 25%,
   no (100% + 0%) / 2 = 50%. Asi una sala chica no pesa lo mismo que una
   grande. Solo se cuentan funciones ya realizadas: las futuras todavia
   pueden vender y bajarian el resultado.

   Las semanas van de lunes a domingo, igual que los objetivos.
   ===================================================================== */

USE CineDB;
GO

/* ---------------------------------------------------------------------
   KPI 1, 2, 3 y 4: recaudacion semanal, entradas semanales, variacion
   y ticket promedio.

   LAG(x) OVER (ORDER BY semana) es una "funcion de ventana": devuelve el
   valor de x en la fila anterior (la semana previa) sin tener que unir
   la tabla consigo misma. Con eso se calcula la variacion porcentual.
   NULLIF(..., 0) evita dividir por cero si la semana anterior no vendio.

   semana_cerrada = 'No' marca la semana actual: todavia no termino, asi
   que su variacion no es comparable.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_recaudacion_semanal
AS
SELECT
    s.semana_inicio,
    DATEADD(DAY, 6, s.semana_inicio)                                AS semana_fin,
    s.ventas,
    s.entradas,
    s.recaudacion,
    CAST(s.recaudacion / s.ventas AS DECIMAL(12,2))                 AS ticket_promedio,
    CAST(1.0 * s.entradas / s.ventas AS DECIMAL(5,2))               AS entradas_por_venta,
    CAST(s.recaudacion / s.entradas AS DECIMAL(12,2))               AS precio_promedio_entrada,
    LAG(s.recaudacion) OVER (ORDER BY s.semana_inicio)              AS recaudacion_semana_anterior,
    CAST(100.0 * (s.recaudacion - LAG(s.recaudacion) OVER (ORDER BY s.semana_inicio))
         / NULLIF(LAG(s.recaudacion) OVER (ORDER BY s.semana_inicio), 0)
         AS DECIMAL(7,2))                                           AS variacion_recaudacion_pct,
    LAG(s.entradas) OVER (ORDER BY s.semana_inicio)                 AS entradas_semana_anterior,
    CAST(100.0 * (s.entradas - LAG(s.entradas) OVER (ORDER BY s.semana_inicio))
         / NULLIF(LAG(s.entradas) OVER (ORDER BY s.semana_inicio), 0)
         AS DECIMAL(7,2))                                           AS variacion_entradas_pct,
    CASE WHEN DATEADD(DAY, 7, s.semana_inicio) <= CAST(GETDATE() AS DATE)
         THEN 'Si' ELSE 'No' END                                    AS semana_cerrada
FROM (
        SELECT  semana_inicio,
                COUNT(*)               AS ventas,
                SUM(cantidad_entradas) AS entradas,
                SUM(total)             AS recaudacion
        FROM    vw_ventas_detalle
        GROUP BY semana_inicio
     ) s;
GO

/* ---------------------------------------------------------------------
   KPI 5: ocupacion promedio por sala.
   Agrega dos datos que ayudan a comparar salas de distinto tamanio:
     butacas_vacias          capacidad que no se vendio
     recaudacion_por_butaca  cuanto rinde cada butaca ofrecida; combina
                             precio y ocupacion en un solo numero
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_ocupacion_sala
AS
SELECT
    sala,
    capacidad,
    COUNT(*)                                                        AS funciones,
    SUM(capacidad)                                                  AS butacas_ofrecidas,
    SUM(entradas_vendidas)                                          AS entradas_vendidas,
    SUM(entradas_libres)                                            AS butacas_vacias,
    CAST(100.0 * SUM(entradas_vendidas) / SUM(capacidad) AS DECIMAL(5,2)) AS ocupacion_pct,
    SUM(recaudacion)                                                AS recaudacion,
    CAST(SUM(recaudacion) / SUM(capacidad) AS DECIMAL(12,2))        AS recaudacion_por_butaca,
    SUM(CASE WHEN agotada = 'Si' THEN 1 ELSE 0 END)                 AS funciones_agotadas
FROM    vw_ocupacion_funcion
WHERE   ya_realizada = 'Si'
GROUP BY sala, capacidad;
GO

/* ---------------------------------------------------------------------
   KPI 6: ocupacion por dia de la semana y horario.
   Una fila por combinacion dia + horario: en Power BI se arma como
   matriz (filas = dia, columnas = horario) o mapa de calor.
   dia_nro sirve para ordenar de lunes a domingo.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_ocupacion_dia_horario
AS
SELECT
    dia_nro,
    dia,
    horario,
    COUNT(*)                                                        AS funciones,
    SUM(entradas_vendidas)                                          AS entradas_vendidas,
    CAST(100.0 * SUM(entradas_vendidas) / SUM(capacidad) AS DECIMAL(5,2)) AS ocupacion_pct,
    CAST(1.0 * SUM(entradas_vendidas) / COUNT(*) AS DECIMAL(7,2))   AS entradas_por_funcion,
    SUM(recaudacion)                                                AS recaudacion
FROM    vw_ocupacion_funcion
WHERE   ya_realizada = 'Si'
GROUP BY dia_nro, dia, horario;
GO

/* ---------------------------------------------------------------------
   KPI 7: peliculas mas vistas y ocupacion por pelicula.

   Dos rankings, porque cuentan cosas distintas:
     ranking_entradas   la que mas gente llevo en total. Favorece a las
                        que tuvieron mas funciones o mas semanas en cartel.
     ranking_ocupacion  la que mejor lleno las salas en las funciones que
                        tuvo. Es la comparacion mas justa entre peliculas.

   RANK() numera segun el orden indicado; si dos empatan, comparten el
   puesto.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_peliculas
AS
SELECT
    pelicula,
    genero,
    COUNT(*)                                                        AS funciones,
    MIN(fecha)                                                      AS primera_funcion,
    MAX(fecha)                                                      AS ultima_funcion,
    SUM(entradas_vendidas)                                          AS entradas_vendidas,
    SUM(recaudacion)                                                AS recaudacion,
    CAST(100.0 * SUM(entradas_vendidas) / SUM(capacidad) AS DECIMAL(5,2)) AS ocupacion_pct,
    RANK() OVER (ORDER BY SUM(entradas_vendidas) DESC)              AS ranking_entradas,
    RANK() OVER (ORDER BY 1.0 * SUM(entradas_vendidas) / SUM(capacidad) DESC) AS ranking_ocupacion
FROM    vw_ocupacion_funcion
WHERE   ya_realizada = 'Si'
GROUP BY pelicula, genero;
GO

/* ---------------------------------------------------------------------
   KPI 8: cumplimiento del objetivo semanal por caja.
   El detalle semana por semana ya esta en vw_objetivo_vs_real; aca se
   resume por caja. Se excluyen las semanas "En curso" porque todavia
   pueden cumplirse.
     tasa_cumplimiento_pct       de cada 100 semanas, cuantas se cumplieron
     cumplimiento_promedio_pct   en promedio, que % del objetivo se recaudo
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_objetivos_caja
AS
SELECT
    caja,
    COUNT(*)                                                        AS semanas_evaluadas,
    SUM(CASE WHEN resultado = 'Cumplido' THEN 1 ELSE 0 END)         AS semanas_cumplidas,
    SUM(CASE WHEN resultado = 'No cumplido' THEN 1 ELSE 0 END)      AS semanas_no_cumplidas,
    CAST(100.0 * SUM(CASE WHEN resultado = 'Cumplido' THEN 1 ELSE 0 END) / COUNT(*)
         AS DECIMAL(5,2))                                           AS tasa_cumplimiento_pct,
    CAST(AVG(cumplimiento_pct) AS DECIMAL(7,2))                     AS cumplimiento_promedio_pct,
    SUM(monto_objetivo)                                             AS objetivo_acumulado,
    SUM(recaudado)                                                  AS recaudado_acumulado
FROM    vw_objetivo_vs_real
WHERE   resultado <> 'En curso'
GROUP BY caja;
GO

/* ---------------------------------------------------------------------
   KPI 9: funciones de baja ocupacion, por mes.
   "Baja ocupacion" = menos del 10% de la sala vendida (incluye las que
   no vendieron nada). Sirve para detectar horarios o peliculas que
   conviene sacar de la grilla. La lista de esas funciones se obtiene de
   vw_ocupacion_funcion filtrando ocupacion_pct < 10.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_baja_ocupacion_mensual
AS
SELECT
    anio,
    mes,
    COUNT(*)                                                        AS funciones,
    SUM(CASE WHEN ocupacion_pct < 10 THEN 1 ELSE 0 END)             AS funciones_baja_ocupacion,
    SUM(CASE WHEN entradas_vendidas = 0 THEN 1 ELSE 0 END)          AS funciones_sin_ventas,
    CAST(100.0 * SUM(CASE WHEN ocupacion_pct < 10 THEN 1 ELSE 0 END) / COUNT(*)
         AS DECIMAL(5,2))                                           AS baja_ocupacion_pct
FROM    vw_ocupacion_funcion
WHERE   ya_realizada = 'Si'
GROUP BY anio, mes;
GO

/* ---------------------------------------------------------------------
   Resumen: una sola fila con los numeros principales, pensada para las
   "tarjetas" de la parte superior del tablero.
   Cada valor sale de una subconsulta independiente sobre las vistas.
   --------------------------------------------------------------------- */
CREATE OR ALTER VIEW vw_kpi_resumen
AS
SELECT
    v.primera_venta,
    v.ultima_venta,
    v.recaudacion_total,
    v.entradas_total,
    v.ventas_total,
    CAST(v.recaudacion_total / NULLIF(v.ventas_total, 0) AS DECIMAL(12,2))         AS ticket_promedio,
    CAST(v.recaudacion_total / NULLIF(v.entradas_total, 0) AS DECIMAL(12,2))       AS precio_promedio_entrada,
    o.funciones_realizadas,
    CAST(100.0 * o.entradas / NULLIF(o.butacas, 0) AS DECIMAL(5,2))                AS ocupacion_pct,
    o.funciones_agotadas,
    CAST(100.0 * o.baja_ocupacion / NULLIF(o.funciones_realizadas, 0) AS DECIMAL(5,2)) AS baja_ocupacion_pct,
    CAST(100.0 * ob.cumplidas / NULLIF(ob.evaluadas, 0) AS DECIMAL(5,2))           AS tasa_cumplimiento_objetivos_pct,
    p.pelicula                                                                      AS pelicula_mas_vista,
    p.entradas_vendidas                                                             AS pelicula_mas_vista_entradas
FROM
    (SELECT MIN(fecha) AS primera_venta, MAX(fecha) AS ultima_venta,
            SUM(total) AS recaudacion_total, SUM(cantidad_entradas) AS entradas_total,
            COUNT(*) AS ventas_total
     FROM   vw_ventas_detalle) v
CROSS JOIN
    (SELECT COUNT(*) AS funciones_realizadas,
            SUM(entradas_vendidas) AS entradas, SUM(capacidad) AS butacas,
            SUM(CASE WHEN agotada = 'Si' THEN 1 ELSE 0 END) AS funciones_agotadas,
            SUM(CASE WHEN ocupacion_pct < 10 THEN 1 ELSE 0 END) AS baja_ocupacion
     FROM   vw_ocupacion_funcion
     WHERE  ya_realizada = 'Si') o
CROSS JOIN
    (SELECT SUM(CASE WHEN resultado = 'Cumplido' THEN 1 ELSE 0 END) AS cumplidas,
            COUNT(*) AS evaluadas
     FROM   vw_objetivo_vs_real
     WHERE  resultado <> 'En curso') ob
OUTER APPLY
    (SELECT TOP (1) pelicula, entradas_vendidas
     FROM   vw_kpi_peliculas
     ORDER BY entradas_vendidas DESC) p;
GO

PRINT 'Vistas de KPI creadas.';
GO

/* ---------------------------------------------------------------------
   Consultas de ejemplo (para probar en SSMS)

   SELECT * FROM vw_kpi_resumen;

   SELECT * FROM vw_kpi_recaudacion_semanal ORDER BY semana_inicio DESC;

   SELECT * FROM vw_kpi_ocupacion_sala ORDER BY ocupacion_pct DESC;

   SELECT * FROM vw_kpi_ocupacion_dia_horario ORDER BY dia_nro, horario;

   SELECT TOP 10 * FROM vw_kpi_peliculas ORDER BY ranking_entradas;

   SELECT * FROM vw_kpi_objetivos_caja ORDER BY tasa_cumplimiento_pct DESC;

   SELECT * FROM vw_kpi_baja_ocupacion_mensual ORDER BY anio, mes;
   --------------------------------------------------------------------- */
