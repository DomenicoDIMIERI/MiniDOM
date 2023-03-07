using System;

namespace minidom.Forms
{
    public class PrendiInCaricoAnagraficaImportata : Sistema.AzioneEseguibile
    {
        public override string Description
        {
            get
            {
                return "Prende in carico l'anagrafica importata";
            }
        }

        protected override string ExecuteInternal(Sistema.Notifica notifica, string parameters)
        {
            if (notifica.SourceName != "CPersonaFisica" && notifica.SourceName != "CAzienda")
                throw new ArgumentException("L'azione non è definita sul tipo [" + notifica.SourceName + "]");
            notifica.StatoNotifica = Sistema.StatoNotifica.LETTA;
            return "";
        }

        public override string Name
        {
            get
            {
                return "ANAIMPPRENDIINCARICO";
            }
        }

        public override object Render(Sistema.Notifica notifica, object context)
        {
            return null;
        }
    }
}