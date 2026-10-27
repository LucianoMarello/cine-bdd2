using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ObjetivoSemanalCajaDAL
{
    public List<ObjetivoSemanalCaja> Listar()
    {
        using var db = new CineContext();
        return db.ObjetivosSemanales.AsNoTracking()
            .Include(o => o.Caja)
            .OrderByDescending(o => o.FechaInicio)
            .ThenBy(o => o.Caja.Nombre)
            .ToList();
    }

    public ObjetivoSemanalCaja? Obtener(int id)
    {
        using var db = new CineContext();
        return db.ObjetivosSemanales.AsNoTracking().FirstOrDefault(o => o.IdObjetivo == id);
    }

    public bool Existe(int idCaja, DateTime fechaInicio, int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.ObjetivosSemanales.Any(o => o.IdCaja == idCaja && o.FechaInicio == fechaInicio && o.IdObjetivo != exceptoId);
    }

    public void Agregar(ObjetivoSemanalCaja objetivo)
    {
        using var db = new CineContext();
        db.ObjetivosSemanales.Add(objetivo);
        db.SaveChanges();
    }

    public void Modificar(ObjetivoSemanalCaja objetivo)
    {
        using var db = new CineContext();
        db.ObjetivosSemanales.Update(objetivo);
        db.SaveChanges();
    }

    public void Eliminar(int id)
    {
        using var db = new CineContext();
        db.ObjetivosSemanales.Where(o => o.IdObjetivo == id).ExecuteDelete();
    }
}
