using System;

namespace minidom.Forms
{
    public class RichiestaPermessoFerieHandler : CBaseModuleHandler
    {
        public RichiestaPermessoFerieHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.RichiestaPermessoFerieCursor();
        }

        public override object GetInternalItemById(int id)
        {
            return Office.RichiestePermessiFerie.GetItemById(id);
        }

        public string Richiedi(object renderer)
        {
            Office.RichiestaPermessoFerie item = (Office.RichiestaPermessoFerie)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item", ""));
            if (!CanEdit(item))
                throw new PermissionDeniedException(Module, "edit");
            item.Richiedi();
            var grp = Sistema.Groups.GetItemByName("Responsabili Richieste Permessi/Ferie");
            if (grp is object)
            {
                string text = "";
                text += "<span class=\"bold blue\">RICHIESTA PERMESSO</span><br/>";
                text += "Operatore: <b>" + item.NomeRichiedente + "</b><br/>";
                text += "Motivo: <b>" + item.MotivoRichiesta + "</b><br/>";
                text += "Periodo: <b>" + Sistema.Formats.FormatUserDateTime(item.DataInizio) + "</b> a <b>" + Sistema.Formats.FormatUserDateTime(item.DataFine) + "</b><br/>";
                var d = DMD.DateUtils.Now();
                if (item.DataRichiesta.HasValue)
                    d = (DateTime)item.DataRichiesta;
                foreach (Sistema.CUser u in grp.Members)
                    Sistema.Notifiche.ProgramAlert(u, text, d, item, "Richiesta Permesso");
            }

            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Annulla(object renderer)
        {
            Office.RichiestaPermessoFerie item = (Office.RichiestaPermessoFerie)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item", ""));
            string dettaglio = this.n2str(renderer, "dettaglio");
            if (!CanEdit(item))
                throw new PermissionDeniedException(Module, "edit");
            item.Annulla(dettaglio);
            var grp = Sistema.Groups.GetItemByName("Responsabili Richieste Permessi/Ferie");
            if (grp is object)
            {
                string text = "";
                text += "<span class=\"bold green\">RICHIESTA PERMESSO ANNULLATA</span><br/>";
                text += "Operatore: <b>" + item.NomeRichiedente + "</b><br/>";
                text += "Motivo: <b>" + item.MotivoRichiesta + "</b><br/>";
                text += "Periodo: <b>" + Sistema.Formats.FormatUserDateTime(item.DataInizio) + "</b> a <b>" + Sistema.Formats.FormatUserDateTime(item.DataFine) + "</b><br/>";
                text += "<hr>";
                text += "<i>" + item.DettaglioEsitoRichiesta + "</i>";
                var d = DMD.DateUtils.Now();
                if (item.DataPresaInCarico.HasValue)
                    d = (DateTime)item.DataPresaInCarico;
                foreach (Sistema.CUser u in grp.Members)
                    Sistema.Notifiche.ProgramAlert(u, text, d, item, "Richiesta Permesso");
            }

            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Accetta(object renderer)
        {
            Office.RichiestaPermessoFerie item = (Office.RichiestaPermessoFerie)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item", ""));
            string dettaglio = this.n2str(renderer, "dettaglio");
            if (!CanLavorare(item))
                throw new PermissionDeniedException(Module, "edit");
            item.Accetta(dettaglio);
            if (item.Richiedente is object)
            {
                string text = "";
                text += "<span class=\"bold green\">PERMESSO ACETTATO</span><br/>";
                text += "Motivo: <b>" + item.MotivoRichiesta + "</b><br/>";
                text += "Periodo: <b>" + Sistema.Formats.FormatUserDateTime(item.DataInizio) + "</b> a <b>" + Sistema.Formats.FormatUserDateTime(item.DataFine) + "</b><br/>";
                text += "<hr>";
                text += "Supervisore: <b>" + item.NomeInCaricoA + "</b><br/>";
                text += "Data: <b>" + Sistema.Formats.FormatUserDate(item.DataPresaInCarico) + "</b><br/>";
                text += "<br/>";
                text += "<i>" + item.DettaglioEsitoRichiesta + "</i>";
                var d = DMD.DateUtils.Now();
                if (item.DataPresaInCarico.HasValue)
                    d = (DateTime)item.DataPresaInCarico;
                Sistema.Notifiche.ProgramAlert(item.Richiedente, text, d, item, "Richiesta Permesso");
            }

            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public string Rifiuta(object renderer)
        {
            Office.RichiestaPermessoFerie item = (Office.RichiestaPermessoFerie)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "item", ""));
            string dettaglio = this.n2str(renderer, "dettaglio");
            if (!CanLavorare(item))
                throw new PermissionDeniedException(Module, "edit");
            item.Rifiuta(dettaglio);
            if (item.Richiedente is object)
            {
                string text = "";
                text += "<span class=\"bold red\">PERMESSO RIFIUTATO</span><br/>";
                text += "Motivo: <b>" + item.MotivoRichiesta + "</b><br/>";
                text += "Periodo: <b>" + Sistema.Formats.FormatUserDateTime(item.DataInizio) + "</b> a <b>" + Sistema.Formats.FormatUserDateTime(item.DataFine) + "</b><br/>";
                text += "<hr>";
                text += "Supervisore: <b>" + item.NomeInCaricoA + "</b><br/>";
                text += "Data: <b>" + Sistema.Formats.FormatUserDate(item.DataPresaInCarico) + "</b><br/>";
                text += "<br/>";
                text += "<i>" + item.DettaglioEsitoRichiesta + "</i>";
                var d = DMD.DateUtils.Now();
                if (item.DataPresaInCarico.HasValue)
                    d = (DateTime)item.DataPresaInCarico;
                Sistema.Notifiche.ProgramAlert(item.Richiedente, text, d, item, "Richiesta Permesso");
            }

            return DMD.XML.Utils.Serializer.Serialize(item);
        }

        public bool CanLavorare(object item)
        {
            return Module.UserCanDoAction("lavorare");
        }
    }
}