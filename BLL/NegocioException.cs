namespace BLL;

/// <summary>
/// Error de una regla de negocio o de validación. Su mensaje está pensado
/// para mostrarse tal cual al usuario (a diferencia de un error técnico).
/// </summary>
public class NegocioException : Exception
{
    public NegocioException(string mensaje) : base(mensaje) { }
}
