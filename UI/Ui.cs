using BLL;

namespace UI;

/// <summary>Ítem de un combo de filtro; Id 0 se usa para "Todas".</summary>
internal record Opcion(int Id, string Nombre);

/// <summary>Ayudas comunes a todos los formularios.</summary>
internal static class Ui
{
    /// <summary>
    /// Ejecuta una acción y muestra el error si falla. Los errores de negocio
    /// (NegocioException) se muestran como advertencia con su mensaje; el resto,
    /// como error técnico.
    /// </summary>
    public static void Intentar(Action accion)
    {
        try
        {
            accion();
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            string detalle = ex.InnerException?.Message ?? ex.Message;
            MessageBox.Show("Ocurrió un error inesperado.\n\n" + detalle, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static bool Confirmar(string pregunta)
    {
        return MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }

    /// <summary>Asigna un valor a un NumericUpDown sin salirse de su rango.</summary>
    public static void Poner(NumericUpDown control, decimal valor)
    {
        control.Value = Math.Min(control.Maximum, Math.Max(control.Minimum, valor));
    }

    /// <summary>Deja un combo sin selección.</summary>
    public static void Deseleccionar(ComboBox combo)
    {
        // Con combos enlazados a datos, la primera asignación a veces no toma.
        combo.SelectedIndex = -1;
        combo.SelectedIndex = -1;
    }

    /// <summary>Id de la fila seleccionada de una grilla, o null si no hay.</summary>
    public static int? IdSeleccionado(DataGridView grilla)
    {
        if (grilla.SelectedRows.Count == 0)
            return null;
        return (int)grilla.SelectedRows[0].Cells["Id"].Value;
    }
}
