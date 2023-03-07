using System;

namespace minidom.Forms
{
    public class ListeRicontattiHandler : CBaseModuleHandler
    {
        public ListeRicontattiHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CListaRicontattiCursor();
        }

        public string SaveLista(object renderer)
        {
            string nome = DMD.Strings.Trim(this.n2str(renderer, "nml"));
            bool sovrascrivi = (bool)this.n2bool(renderer, "ovw");
            DateTime data = (DateTime)this.n2date(renderer, "dt");
            string note = DMD.Strings.Trim(this.n2str(renderer, "note"));
            CCollection persone = (CCollection)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "items"));
            if (string.IsNullOrEmpty(nome))
                throw new ArgumentNullException("nml", "Il nome della lista non può essere vuoto");
            var list = Anagrafica.ListeRicontatto.GetItemByName(nome);
            if (list is null)
            {
                list = new Anagrafica.CListaRicontatti();
                list.Name = nome;
                list.Descrizione = note;
                list.DataInserimento = DMD.DateUtils.Now();
            }

            list.Stato = Databases.ObjectStatus.OBJECT_VALID;
            list.Save();
            var ricontatti = new CCollection<Anagrafica.ListaRicontattoItem>();
            foreach (Anagrafica.CPersona p in persone)
            {
                var ric = new Anagrafica.ListaRicontattoItem();
                ric.Persona = p;
                ric.DataPrevista = data;
                ric.NomeLista = list.Name;
                ric.Note = note;
                // ric.setCategoria(list.getName());
                ric.Stato = Databases.ObjectStatus.OBJECT_VALID;
                ric.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO;
                ric.Save();
                ricontatti.Add(ric);
            }

            return DMD.XML.Utils.Serializer.Serialize(ricontatti);
        }
    }
}