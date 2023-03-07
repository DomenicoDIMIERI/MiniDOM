using System;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Interfaccia implementata dagli oggetti che possono estinguere un finanziamento in corso
    /// </summary>
    /// <remarks></remarks>
        public interface IEstintore
        {
            CEstinzioniXEstintoreCollection Estinzioni { get; }
            DateTime DataCaricamento { get; }
            DateTime? DataDecorrenza { get; set; }
            Anagrafica.CPersonaFisica Cliente { get; set; }
        }
    }
}