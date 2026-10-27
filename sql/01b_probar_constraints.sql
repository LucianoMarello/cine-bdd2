/* =====================================================================
   01b_probar_constraints.sql
   Verifica que las restricciones de CineDB rechacen datos invalidos.
   Cada prueba intenta una operacion que DEBE fallar.
   Todo corre dentro de una transaccion que se revierte al final:
   no deja datos en la base.
   ===================================================================== */

USE CineDB;
GO

SET NOCOUNT ON;

DECLARE @resultados TABLE (
    nro        INT IDENTITY(1,1),
    prueba     VARCHAR(100),
    resultado  VARCHAR(10),
    detalle    VARCHAR(300)
);

BEGIN TRANSACTION;

/* --- Datos validos de base ------------------------------------------ */
INSERT INTO Usuario (nombre_usuario, clave, rol) VALUES ('test_admin', 'hash', 'Admin');
DECLARE @id_usuario INT = SCOPE_IDENTITY();

INSERT INTO Pelicula (titulo, genero, duracion) VALUES ('Pelicula de prueba', 'Drama', 120);
DECLARE @id_pelicula INT = SCOPE_IDENTITY();

INSERT INTO Sala (nombre, capacidad) VALUES ('Sala de prueba', 100);
DECLARE @id_sala INT = SCOPE_IDENTITY();

INSERT INTO Caja (nombre) VALUES ('Caja de prueba');
DECLARE @id_caja INT = SCOPE_IDENTITY();

INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada)
VALUES (@id_pelicula, @id_sala, '2026-12-01T20:00:00', 5000);
DECLARE @id_funcion INT = SCOPE_IDENTITY();

INSERT INTO Venta (id_funcion, id_caja, id_usuario, cantidad_entradas, precio_unitario, total, forma_pago)
VALUES (@id_funcion, @id_caja, @id_usuario, 2, 5000, 10000, 'Efectivo');

INSERT INTO Objetivo_semanal_caja (id_caja, fecha_inicio, fecha_fin, monto_objetivo)
VALUES (@id_caja, '2026-11-30', '2026-12-06', 500000);

INSERT INTO @resultados VALUES ('Carga de datos validos', 'OK', '');

/* --- Usuario --------------------------------------------------------- */
BEGIN TRY
    INSERT INTO Usuario (nombre_usuario, clave, rol) VALUES ('test_admin', 'hash', 'Cajero');
    INSERT INTO @resultados VALUES ('Usuario: nombre_usuario duplicado', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Usuario: nombre_usuario duplicado', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Usuario (nombre_usuario, clave, rol) VALUES ('test_rol', 'hash', 'Gerente');
    INSERT INTO @resultados VALUES ('Usuario: rol inexistente', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Usuario: rol inexistente', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Usuario (nombre_usuario, clave, rol) VALUES (NULL, 'hash', 'Cajero');
    INSERT INTO @resultados VALUES ('Usuario: nombre_usuario NULL', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Usuario: nombre_usuario NULL', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Pelicula -------------------------------------------------------- */
BEGIN TRY
    INSERT INTO Pelicula (titulo, genero, duracion) VALUES ('Sin duracion', 'Drama', 0);
    INSERT INTO @resultados VALUES ('Pelicula: duracion 0', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Pelicula: duracion 0', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Sala ------------------------------------------------------------ */
BEGIN TRY
    INSERT INTO Sala (nombre, capacidad) VALUES ('Sala de prueba', 50);
    INSERT INTO @resultados VALUES ('Sala: nombre duplicado', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Sala: nombre duplicado', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Sala (nombre, capacidad) VALUES ('Sala vacia', 0);
    INSERT INTO @resultados VALUES ('Sala: capacidad 0', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Sala: capacidad 0', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Caja ------------------------------------------------------------ */
BEGIN TRY
    INSERT INTO Caja (nombre) VALUES ('Caja de prueba');
    INSERT INTO @resultados VALUES ('Caja: nombre duplicado', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Caja: nombre duplicado', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Funcion --------------------------------------------------------- */
BEGIN TRY
    INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada)
    VALUES (-1, @id_sala, '2026-12-02T20:00:00', 5000);
    INSERT INTO @resultados VALUES ('Funcion: pelicula inexistente (FK)', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Funcion: pelicula inexistente (FK)', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada)
    VALUES (@id_pelicula, @id_sala, '2026-12-01T20:00:00', 5000);
    INSERT INTO @resultados VALUES ('Funcion: misma sala y horario', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Funcion: misma sala y horario', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada)
    VALUES (@id_pelicula, @id_sala, '2026-12-03T20:00:00', -1);
    INSERT INTO @resultados VALUES ('Funcion: precio negativo', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Funcion: precio negativo', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Funcion (id_pelicula, id_sala, fecha_hora, precio_entrada, formato)
    VALUES (@id_pelicula, @id_sala, '2026-12-04T20:00:00', 5000, '4D');
    INSERT INTO @resultados VALUES ('Funcion: formato invalido', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Funcion: formato invalido', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Venta ----------------------------------------------------------- */
BEGIN TRY
    INSERT INTO Venta (id_funcion, id_caja, id_usuario, cantidad_entradas, precio_unitario, total, forma_pago)
    VALUES (@id_funcion, @id_caja, -1, 1, 5000, 5000, 'Efectivo');
    INSERT INTO @resultados VALUES ('Venta: usuario inexistente (FK)', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Venta: usuario inexistente (FK)', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Venta (id_funcion, id_caja, id_usuario, cantidad_entradas, precio_unitario, total, forma_pago)
    VALUES (@id_funcion, @id_caja, @id_usuario, 0, 5000, 0, 'Efectivo');
    INSERT INTO @resultados VALUES ('Venta: cantidad 0', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Venta: cantidad 0', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Venta (id_funcion, id_caja, id_usuario, cantidad_entradas, precio_unitario, total, forma_pago)
    VALUES (@id_funcion, @id_caja, @id_usuario, 2, 5000, 9999, 'Efectivo');
    INSERT INTO @resultados VALUES ('Venta: total mal calculado', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Venta: total mal calculado', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Venta (id_funcion, id_caja, id_usuario, cantidad_entradas, precio_unitario, total, forma_pago)
    VALUES (@id_funcion, @id_caja, @id_usuario, 1, 5000, 5000, 'Cheque');
    INSERT INTO @resultados VALUES ('Venta: forma de pago invalida', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Venta: forma de pago invalida', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Objetivo_semanal_caja ------------------------------------------- */
BEGIN TRY
    INSERT INTO Objetivo_semanal_caja (id_caja, fecha_inicio, fecha_fin, monto_objetivo)
    VALUES (@id_caja, '2026-11-30', '2026-12-06', 300000);
    INSERT INTO @resultados VALUES ('Objetivo: semana duplicada para la caja', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Objetivo: semana duplicada para la caja', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Objetivo_semanal_caja (id_caja, fecha_inicio, fecha_fin, monto_objetivo)
    VALUES (@id_caja, '2026-12-13', '2026-12-07', 300000);
    INSERT INTO @resultados VALUES ('Objetivo: fecha_fin anterior a fecha_inicio', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Objetivo: fecha_fin anterior a fecha_inicio', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

BEGIN TRY
    INSERT INTO Objetivo_semanal_caja (id_caja, fecha_inicio, fecha_fin, monto_objetivo)
    VALUES (@id_caja, '2026-12-14', '2026-12-20', 0);
    INSERT INTO @resultados VALUES ('Objetivo: monto 0', 'FALLO', 'Se acepto el dato invalido');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Objetivo: monto 0', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

/* --- Borrado con dependencias ---------------------------------------- */
BEGIN TRY
    DELETE FROM Pelicula WHERE id_pelicula = @id_pelicula;
    INSERT INTO @resultados VALUES ('Pelicula: borrar con funciones asociadas', 'FALLO', 'Se permitio el borrado');
END TRY
BEGIN CATCH
    INSERT INTO @resultados VALUES ('Pelicula: borrar con funciones asociadas', 'OK', LEFT(ERROR_MESSAGE(), 300));
END CATCH;

ROLLBACK TRANSACTION;

SELECT nro, prueba, resultado, detalle FROM @resultados ORDER BY nro;

SELECT
    SUM(CASE WHEN resultado = 'OK' THEN 1 ELSE 0 END)    AS pruebas_ok,
    SUM(CASE WHEN resultado = 'FALLO' THEN 1 ELSE 0 END) AS pruebas_fallidas
FROM @resultados;
GO
