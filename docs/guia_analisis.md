# Guía para el análisis de datos

El análisis se hace desde una herramienta externa (Power BI o Excel) conectada a tres vistas de `CineDB`. Las vistas se crean con `sql/04_vistas_analisis.sql`.

## Las vistas

| Vista | Una fila por | Para qué sirve |
|---|---|---|
| `vw_ventas_detalle` | venta | Recaudación y entradas por cualquier criterio: fecha, película, género, sala, caja, vendedor, forma de pago, horario |
| `vw_ocupacion_funcion` | función | Qué tan llenas estuvieron las salas; incluye las funciones sin ventas |
| `vw_objetivo_vs_real` | caja y semana | Si cada caja cumplió su objetivo semanal |

Los textos (días, géneros, formas de pago) van sin tildes: "Miercoles", "Sabado", "Debito".

## Conectar Power BI Desktop

1. **Obtener datos → SQL Server**.
2. Servidor: `.\SQLEXPRESS` — Base de datos: `CineDB` — Modo: **Importar**.
3. Autenticación: **Windows** (usar las credenciales actuales).
4. En el navegador, tildar las tres vistas `vw_...` y **Cargar**. No hace falta cargar las tablas.
5. No hay que crear relaciones entre las vistas: cada una ya trae todo lo que necesita.

Si el servidor de otra máquina tiene otro nombre de instancia, cambiar `.\SQLEXPRESS` por el que corresponda.

## Conectar Excel

**Datos → Obtener datos → Desde base de datos → Desde una base de datos de SQL Server**, con el mismo servidor y base. Elegir una vista y cargarla como tabla o como tabla dinámica.

## Preguntas que se pueden responder

### Con `vw_ventas_detalle`

| Pregunta | Cómo armarla |
|---|---|
| ¿Cómo evolucionó la recaudación? | Líneas: eje `fecha` (o `semana_inicio`), valor suma de `total` |
| ¿Qué películas vendieron más? | Barras: `pelicula`, suma de `cantidad_entradas` |
| ¿Qué géneros prefiere el público? | Barras o torta: `genero`, suma de `cantidad_entradas` |
| ¿Qué días se vende más? | Columnas: `funcion_dia` (ordenar por `funcion_dia_nro`), suma de `cantidad_entradas` |
| ¿Sirve la promoción de los miércoles? | Comparar por `funcion_dia`: suma de `cantidad_entradas` contra suma de `total` |
| ¿Qué caja y qué vendedor venden más? | Barras: `caja` o `vendedor`, suma de `total` |
| ¿Cómo paga la gente? | Torta: `forma_pago`, suma de `total` |
| ¿Cuánto se compra por anticipado? | Torta: `tipo_venta`, recuento de `id_venta` |
| ¿El 3D recauda más? | Barras: `formato`, suma de `total` y promedio de `precio_unitario` |

### Con `vw_ocupacion_funcion`

Filtrar siempre `ya_realizada = "Si"`: las funciones futuras todavía pueden vender y bajarían el promedio.

| Pregunta | Cómo armarla |
|---|---|
| ¿Qué tan llenas están las salas? | Barras: `sala`, promedio de `ocupacion_pct` |
| ¿Qué horario conviene? | Columnas: `horario`, promedio de `ocupacion_pct` |
| ¿Qué día y horario rinde más? | Matriz: filas `dia`, columnas `horario`, valor promedio de `ocupacion_pct` |
| ¿Cuántas funciones se agotaron? | Tarjeta: recuento filtrando `agotada = "Si"` |
| ¿Hay funciones que no vendieron nada? | Tabla filtrando `entradas_vendidas = 0` |

### Con `vw_objetivo_vs_real`

| Pregunta | Cómo armarla |
|---|---|
| ¿Qué caja cumple más seguido? | Barras apiladas: `caja`, recuento por `resultado` |
| ¿Cómo viene cada caja semana a semana? | Líneas: eje `fecha_inicio`, valores `monto_objetivo` y `recaudado`, filtrando una `caja` |
| ¿Por cuánto se cumplió o faltó? | Tabla: `caja`, `fecha_inicio`, `diferencia`, `cumplimiento_pct` |

Las semanas con `resultado = "En curso"` todavía no terminaron: conviene excluirlas al contar cumplimientos.

## Indicadores clave (KPI)

Se crean con `sql/05_kpis.sql`, después de `04_vistas_analisis.sql`. Ya vienen calculados: en Power BI solo hay que cargarlos y mostrarlos.

| Vista | KPI | Visual sugerido |
|---|---|---|
| `vw_kpi_resumen` | Recaudación, entradas, ticket promedio, ocupación, % de baja ocupación, tasa de cumplimiento, película más vista | Tarjetas en la parte superior |
| `vw_kpi_recaudacion_semanal` | Recaudación y entradas por semana, ticket promedio, variación % contra la semana anterior | Líneas (eje `semana_inicio`) + tabla con la variación |
| `vw_kpi_ocupacion_sala` | Ocupación %, butacas vacías y recaudación por butaca de cada sala | Barras horizontales |
| `vw_kpi_ocupacion_dia_horario` | Ocupación % por día y horario | Matriz con formato condicional (mapa de calor), ordenar por `dia_nro` |
| `vw_kpi_peliculas` | Entradas, recaudación, ocupación y dos rankings por película | Tabla o barras, ordenar por `ranking_entradas` |
| `vw_kpi_objetivos_caja` | Tasa de cumplimiento y cumplimiento promedio por caja | Barras o medidor |
| `vw_kpi_baja_ocupacion_mensual` | % de funciones con menos del 10% de ocupación, por mes | Columnas |

La ocupación es ponderada: entradas vendidas / butacas ofrecidas, sumando todas las funciones del grupo. Solo cuenta funciones ya realizadas.

## Probar las vistas sin Power BI

Desde SSMS:

```sql
SELECT TOP 20 * FROM vw_ventas_detalle ORDER BY fecha_venta DESC;

SELECT dia, AVG(ocupacion_pct) AS ocupacion_promedio
FROM   vw_ocupacion_funcion
WHERE  ya_realizada = 'Si'
GROUP BY dia, dia_nro
ORDER BY dia_nro;

SELECT caja, resultado, COUNT(*) AS semanas
FROM   vw_objetivo_vs_real
GROUP BY caja, resultado
ORDER BY caja, resultado;
```
