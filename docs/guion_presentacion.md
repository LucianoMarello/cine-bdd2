# Guion de la presentación

Duración estimada: 12 a 15 minutos de exposición más preguntas.

## Antes de presentar

- [ ] Ejecutar `sql/regenerar_base.bat`, así los datos llegan hasta ese día.
- [ ] Abrir la aplicación, crear el administrador y asignarle clave a `cajero1`.
- [ ] Abrir Power BI con el tablero conectado y actualizado (**Inicio → Actualizar**).
- [ ] Tener abiertos en SSMS `01_crear_bd.sql` y `05_kpis.sql`, y el diagrama de draw.io.

## 1. El problema (1 minuto)

Un cine necesita registrar la venta de entradas en sus boleterías y después analizar cómo le va: qué películas funcionan, qué tan llenas están las salas, qué días y horarios convienen y si cada caja cumple su meta semanal.

El sistema tiene dos partes:
- una **aplicación de escritorio** para cargar datos (administración y ventas), con login y roles;
- una **capa de análisis** en la base de datos (vistas e indicadores) que se consume desde Power BI.

## 2. El modelo de datos (3 minutos)

Mostrar el diagrama.

- **7 tablas:** Usuario, Película, Sala, Función, Caja, Venta, Objetivo_semanal_caja.
- **Función** resuelve la relación muchos a muchos entre Película y Sala: una película en una sala, en un horario, con un precio.
- **Venta** es la tabla central para el análisis: dice qué se vendió, dónde, quién, cuándo, cuánto y cómo se pagó.
- Cambios respecto del primer diagrama: se relacionó Usuario con Venta (antes estaba aislado), `bool` pasó a `BIT`, las claves foráneas son obligatorias y se agregaron `precio_unitario`, `forma_pago` y `formato`.

## 3. Las reglas viven en la base (2 minutos)

Mostrar `01_crear_bd.sql`.

- **Restricciones con nombre:** PK, FK, UNIQUE, CHECK y DEFAULT. Ejemplos para mostrar:
  - `UQ_Funcion_sala_fecha_hora`: una sala no puede tener dos funciones a la misma hora.
  - `CK_Venta_total`: el total tiene que ser cantidad × precio unitario.
  - `CK_Usuario_rol`: solo existen los roles Admin y Cajero.
- **Índices** sobre las claves foráneas y la fecha de venta, porque SQL Server no indexa las FK solo.
- **Bajas lógicas** (`estado = 0`) en lugar de borrar, para no perder historial.
- Opcional: ejecutar `01b_probar_constraints.sql` y mostrar que las 20 pruebas dan OK.

## 4. Demo de la aplicación (4 minutos)

1. **Login** como administrador. Mostrar que una clave incorrecta da un mensaje genérico.
2. **Administración → Funciones:** intentar crear una función que se superponga con otra en la misma sala → la rechaza indicando con cuál choca.
3. **Cerrar sesión** y entrar como `cajero1`: el menú Administración ya no aparece.
4. **Ventas → Nueva venta:** elegir una función, caja, cantidad y forma de pago; el total se calcula solo. Registrar la venta.
5. Intentar vender más entradas que las disponibles → la rechaza indicando cuántas quedan.
6. **Ventas → Listado:** el cajero ve solo sus ventas, con totales al pie.

## 5. Datos de prueba (1 minuto)

- `03_datos_prueba_ventas.sql` genera 6 meses de actividad: unas 3.400 funciones y 34.000 ventas.
- Se generan "por conjuntos" (INSERT ... SELECT), no con bucles: tarda unos 3 segundos.
- Siguen patrones realistas para que el análisis muestre algo: más público el fin de semana y a las 20:00, miércoles a mitad de precio, películas que pierden público semana a semana, éxitos y fracasos.
- Respetan todas las reglas: ninguna sobreventa ni funciones superpuestas.

## 6. Análisis e indicadores (3 minutos)

Mostrar el tablero de Power BI.

- **Vistas de detalle** (`04_vistas_analisis.sql`): ya traen las tablas unidas y las columnas calculadas (día de la semana, ocupación, cumplimiento).
- **Vistas de KPI** (`05_kpis.sql`): cada indicador se calcula en SQL y Power BI solo lo muestra. Así el cálculo queda en un único lugar.
- Recorrer los indicadores:
  - Recaudación y entradas por semana, con variación contra la semana anterior.
  - Ticket promedio.
  - Ocupación por sala y por día/horario (mapa de calor).
  - Ranking de películas, por entradas y por ocupación.
  - Cumplimiento del objetivo semanal por caja.
  - Funciones de baja ocupación.
- Conclusiones que se pueden contar con los datos: el sábado a las 20:00 es el mejor horario; la promoción de los miércoles duplica las entradas pero no la recaudación; hay películas con muchas entradas solo porque tuvieron muchas funciones (el ranking por ocupación las pone en su lugar).

## Preguntas posibles

**¿Por qué los id no son consecutivos o no empiezan en 1?**
`IDENTITY` es un contador que solo avanza: si una fila se borra o una transacción se revierte, ese número no se reutiliza. El id solo tiene que ser único, no consecutivo.

**¿Cómo evitan vender más entradas que butacas si dos cajas venden a la vez?**
La venta se hace en una transacción que primero bloquea la fila de la función (`UPDLOCK, HOLDLOCK`). La segunda caja espera a que la primera termine y recién ahí lee el cupo actualizado. Se probó con 80 ventas simultáneas sobre 200 butacas: se vendieron exactamente 200. Está en `DAL/VentaDAL.cs`.

**¿Por qué no usaron una transacción `SERIALIZABLE`?**
Con `SERIALIZABLE`, dos ventas simultáneas para la misma función pueden terminar en deadlock. Bloqueando la fila desde el principio, una espera a la otra y no hay deadlock. Las ventas de otras funciones no se frenan entre sí.

**¿Por qué guardan el total si se puede calcular?**
Para que la venta conserve su valor histórico aunque después cambie el precio de la función; por eso también se guarda `precio_unitario`. Un CHECK garantiza que el total siempre sea cantidad × precio.

**¿Cómo guardan las contraseñas?**
Nunca en texto: se guarda un hash PBKDF2 con SHA-256, 100.000 iteraciones y un salt aleatorio por usuario. Al iniciar sesión se recalcula el hash con la clave ingresada y se compara.

**¿Por qué los cajeros de prueba no tienen clave?**
El hash lo calcula la aplicación y SQL no puede generarlo. Los cajeros se cargan con un valor que no es un hash válido y el administrador les asigna la clave desde la aplicación.

**¿Por qué usaron Entity Framework si la materia es de base de datos?**
La base se diseñó y se creó a mano con scripts SQL (no con migraciones), y toda la lógica de análisis está en vistas SQL. Entity Framework solo se usa para leer y escribir desde la aplicación.

**¿Qué pasa si se borra una película que tiene funciones?**
No se puede: la clave foránea lo impide (no hay borrado en cascada). Por eso la aplicación hace bajas lógicas.

**¿Por qué la ocupación es un promedio ponderado?**
Para que una sala de 60 butacas no pese lo mismo que una de 180: se suman todas las entradas vendidas y se dividen por todas las butacas ofrecidas.

**¿Qué mejorarían?**
Butacas numeradas, apertura y cierre de caja por turno, anulación de ventas, promociones y tipos de entrada, y una bitácora de auditoría.
