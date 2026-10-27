using System.Security.Cryptography;

namespace BLL;

/// <summary>
/// Hash de contraseñas con PBKDF2 (SHA-256).
///
/// Un hash es irreversible: de la contraseña se obtiene el hash, pero del
/// hash no se puede volver a la contraseña. Para verificar un login se
/// vuelve a calcular el hash con la clave ingresada y se comparan.
///
/// - Salt: bytes aleatorios distintos para cada usuario. Hace que dos
///   usuarios con la misma contraseña tengan hashes distintos.
/// - Iteraciones: el cálculo se repite muchas veces a propósito, para que
///   probar millones de contraseñas por fuerza bruta sea lento.
///
/// Lo que se guarda en Usuario.clave tiene este formato:
///   PBKDF2$iteraciones$salt$hash      (salt y hash en Base64)
/// </summary>
public static class HashClave
{
    private const string Prefijo = "PBKDF2";
    private const int Iteraciones = 100_000;
    private const int BytesSalt = 16;
    private const int BytesHash = 32;

    public static string Generar(string clave)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(BytesSalt);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, salt, Iteraciones, HashAlgorithmName.SHA256, BytesHash);
        return $"{Prefijo}${Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string clave, string guardado)
    {
        string[] partes = guardado.Split('$');
        if (partes.Length != 4 || partes[0] != Prefijo || !int.TryParse(partes[1], out int iteraciones))
            return false;

        try
        {
            byte[] salt = Convert.FromBase64String(partes[2]);
            byte[] hashGuardado = Convert.FromBase64String(partes[3]);
            byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(clave, salt, iteraciones, HashAlgorithmName.SHA256, hashGuardado.Length);
            // Comparación en tiempo constante: no revela cuántos bytes coincidieron.
            return CryptographicOperations.FixedTimeEquals(hashIngresado, hashGuardado);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
