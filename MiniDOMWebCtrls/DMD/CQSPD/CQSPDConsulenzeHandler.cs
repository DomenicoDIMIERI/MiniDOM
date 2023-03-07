using System;
using DMD.XML;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CQSPDConsulenzeHandler : CBaseModuleHandler
    {
        public CQSPDConsulenzeHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Finanziaria.CQSPDConsulenzaCursor();
        }

        public string GetConsulenzeByPersona(object renderer)
        {
            int idPersona = (int)this.n2int(renderer, "pid");
            var items = Finanziaria.Consulenze.GetConsulenzeByPersona(idPersona);
            if (items.Count > 0)
            {
                return DMD.XML.Utils.Serializer.Serialize(items.ToArray(), XMLSerializeMethod.Document);
            }
            else
            {
                return "";
            }
        }

        public string Proponi(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            if (CanEdit(consulenza) == false)
                throw new PermissionDeniedException(Module, "edit");
            consulenza.Proponi();
            return "";
        }

        public string Accetta(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            if (CanEdit(consulenza) == false)
                throw new PermissionDeniedException(Module, "edit");
            consulenza.Accetta();
            return "";
        }

        public string Rifiuta(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            string motivo = this.n2str(renderer, "mt");
            string dettaglio = this.n2str(renderer, "dt");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            if (CanEdit(consulenza) == false)
                throw new PermissionDeniedException(Module, "edit");
            consulenza.Rifiuta(motivo, dettaglio);
            return "";
        }

        public string Boccia(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            string motivo = this.n2str(renderer, "mt");
            string dettaglio = this.n2str(renderer, "dt");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            if (CanEdit(consulenza) == false)
                throw new PermissionDeniedException(Module, "edit");
            consulenza.Boccia(motivo, dettaglio);
            return "";
        }

        public string Sollecita(object renderer)
        {
            int cid = (int) this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            consulenza.Sollecita();
            return DMD.XML.Utils.Serializer.Serialize(consulenza.RichiestaApprovazione);
        }

        public string RichiediApprovazione(object renderer)
        {
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            string motivo = this.n2str(renderer, "mot");
            string dettaglio = this.n2str(renderer, "det");
            string parametri = this.n2str(renderer, "par");
            var rich = consulenza.RichiediApprovazione(motivo, dettaglio, parametri);
            return DMD.XML.Utils.Serializer.Serialize(rich);
        }

        public string PrendiInCarico(object renderer)
        {
            if (Finanziaria.Pratiche.Module.UserCanDoAction("approva_sconto") == false)
                throw new PermissionDeniedException(Module, "approva_sconto");
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            var ret = consulenza.PrendiInCarico();
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string ConfermaApprovazione(object renderer)
        {
            if (Finanziaria.Pratiche.Module.UserCanDoAction("approva_sconto") == false)
                throw new PermissionDeniedException(Module, "approva_sconto");
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            string motivoApprovazione = this.n2str(renderer, "mot");
            string dettaglioApprovazione = this.n2str(renderer, "det");
            var motivo = consulenza.RichiestaApprovazione.MotivoRichiesta;
            if (motivo is null)
                throw new ArgumentException("Il motivo di sconto indicato non è previsto");
            if (motivo.Privilegiato && !Sistema.Users.CurrentUser.IsInGroup(Finanziaria.GruppoAutorizzatori.GroupName))
            {
                throw new PermissionDeniedException("L'utente corrente non è abilitato ad approvare uno sconto privilegiato");
            }

            var ret = consulenza.Approva(motivoApprovazione, dettaglioApprovazione);
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string NegaApprovazione(object renderer)
        {
            if (Finanziaria.Pratiche.Module.UserCanDoAction("approva_sconto") == false)
                throw new PermissionDeniedException(Module, "approva_sconto");
            int cid = (int)this.n2int(renderer, "cid");
            var consulenza = Finanziaria.Consulenze.GetItemById(cid);
            string motivo = this.n2str(renderer, "mot");
            string dettaglio = this.n2str(renderer, "det");
            var ret = consulenza.Nega(motivo, dettaglio);
            return DMD.XML.Utils.Serializer.Serialize(ret);
        }

        public string GetUltimaConsulenza(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            var ret = Finanziaria.Consulenze.GetUltimaConsulenza(pid);
            if (ret is null)
            {
                return "";
            }
            else
            {
                return DMD.XML.Utils.Serializer.Serialize(ret);
            }
        }

        public string CreateElencoConsulenze(object renderer)
        {
            int idCliente = (int)this.n2int(renderer, "pid");
            int id = (int)this.n2int(renderer, "id");
            return Utils.CQSPDUtils.CreateElencoConsulenze(idCliente, id);
        }
    }
}