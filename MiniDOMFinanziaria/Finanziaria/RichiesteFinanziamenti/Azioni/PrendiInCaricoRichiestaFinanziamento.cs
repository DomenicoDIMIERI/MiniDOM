using System;
using DMD;

namespace minidom
{
    public partial class Finanziaria
    {
        public class PrendiInCaricoRichiestaFinanziamento : Sistema.AzioneEseguibile
        {
            public override string Description
            {
                get
                {
                    return "Prende in carico la richiesta di finanziamento e programma un ricontatto nel CRM";
                }
            }

            protected override string ExecuteInternal(Sistema.Notifica notifica, string parameters)
            {
                if (notifica.SourceName != "CRichiestaFinanziamento")
                    throw new ArgumentException("L'azione non è definita sul tipo [" + notifica.SourceName + "]");
                var richFin = RichiesteFinanziamento.GetItemById(notifica.SourceID);
                if (richFin is null)
                    throw new ArgumentNullException("Richiesta di finaniamento");
                richFin.PrendiInCarico();
                var ric = Anagrafica.Ricontatti.ProgrammaRicontatto(richFin.Cliente, DMD.DateUtils.Now(), "Ricontatto programmato per la richiesta online: " + richFin.ToString(), DMD.RunTime.vbTypeName(richFin), DBUtils.GetID(richFin).ToString(), "", richFin.PuntoOperativo, Sistema.Users.CurrentUser);
                notifica.StatoNotifica = Sistema.StatoNotifica.LETTA;
                return DBUtils.GetID(ric).ToString();
            }

            public override string Name
            {
                get
                {
                    return "RICHFINPRENDIINCARICO";
                }
            }

            public override object Render(Sistema.Notifica notifica, object context)
            {
                return null;
            }
        }
    }
}