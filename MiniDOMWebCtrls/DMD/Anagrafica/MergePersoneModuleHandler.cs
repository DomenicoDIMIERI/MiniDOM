using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class MergePersoneModuleHandler
        : CBaseModuleHandler
    {
        public MergePersoneModuleHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CMergePersonaCursor();
        }

        public string GetGroups(object renderer)
        {
            if (!Anagrafica.Persone.Module.UserCanDoAction("merge"))
                throw new PermissionDeniedException(Anagrafica.Persone.Module, "merge");
            Anagrafica.CRMFindParams filter = (Anagrafica.CRMFindParams)DMD.XML.Utils.Serializer.Deserialize(this.n2str(renderer, "filter"));
            var col = Anagrafica.Persone.Find(filter);
            col.Sort();
            foreach (Anagrafica.CPersonaInfo item in col)
                MakeNotes(item);
            var gruppi = MakeGruppi(col);
            return DMD.XML.Utils.Serializer.Serialize(gruppi);
        }

        public string UnMerge(object renderer)
        {
            if (!Anagrafica.Persone.Module.UserCanDoAction("merge"))
                throw new PermissionDeniedException(Anagrafica.Persone.Module, "merge");
            int item = (int)this.n2int(renderer, "item");
            var persona = Anagrafica.Persone.GetItemById(item);
            if (persona is null)
                throw new ArgumentNullException("persona");
            return DMD.XML.Utils.Serializer.Serialize(persona.UndoMerge());
        }

        public string MegeItems(object renderer)
        {
            if (!Anagrafica.Persone.Module.UserCanDoAction("merge"))
                throw new PermissionDeniedException(Anagrafica.Persone.Module, "merge");
            var items = Strings.Split(this.n2str(renderer, "items"), ",");
            var idsList = new ArrayList();
            foreach (string value in items)
            {
                if (Sistema.Formats.ToInteger(value) != 0)
                    idsList.Add(Sistema.Formats.ToInteger(value));
            }

            int[] ids = (int[])idsList.ToArray(typeof(int));
            if (DMD.Arrays.UBound(ids) > 0)
            {
                var persona = Anagrafica.Persone.GetItemById(ids[0]);
                for (int i = 1, loopTo = DMD.Arrays.UBound(ids); i <= loopTo; i++)
                    persona.MergeWith(Anagrafica.Persone.GetItemById(ids[i]));
            }

            return "";
        }

        public string Rename(object renderer)
        {
            if (!Anagrafica.Persone.Module.UserCanDoAction("merge"))
                throw new PermissionDeniedException(Anagrafica.Persone.Module, "merge");
            int id = (int)this.n2int(renderer, "id");
            string nome = DMD.Strings.Trim(this.n2str(renderer, "n"));
            string cognome = DMD.Strings.Trim(this.n2str(renderer, "c"));
            if (string.IsNullOrEmpty(nome + cognome))
                throw new ArgumentNullException("Non puoi rinominare con nome vuoto");
            var persona = Anagrafica.Persone.GetItemById(id);
            if (persona is null)
                throw new ArgumentNullException("Persona");
            if (persona is Anagrafica.CPersonaFisica)
            {
                {
                    var withBlock = (Anagrafica.CPersonaFisica)persona;
                    withBlock.Cognome = cognome;
                    withBlock.Nome = nome;
                    withBlock.MergeWith(persona);
                }
            }
            else
            {
                {
                    var withBlock1 = (Anagrafica.CAzienda)persona;
                    withBlock1.RagioneSociale = cognome + nome;
                    withBlock1.MergeWith(persona);
                }
            }

            return "";
        }

        private string MakeNotes(Anagrafica.CPersonaInfo info)
        {
            var p = info.Persona;
            string sessoAO = Sistema.IIF(p.Sesso == "F", "a", "o");
            if (p.TipoPersona == Anagrafica.TipoPersona.PERSONA_FISICA)
            {
                {
                    var withBlock = (Anagrafica.CPersonaFisica)p;
                    string strImpiego = "";
                    if (withBlock.ImpiegoPrincipale is object)
                    {
                        if (!string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.NomeAzienda) || !string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.Posizione) || !string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.TipoRapporto))
                        {
                            if (withBlock.ImpiegoPrincipale.TipoRapporto == "H")
                            {
                                strImpiego = "Pensionat" + sessoAO;
                            }
                            else
                            {
                                strImpiego = "Lavora";
                            }

                            if (!string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.NomeAzienda))
                                strImpiego += " presso " + withBlock.ImpiegoPrincipale.NomeAzienda;
                            if (!string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.Posizione))
                                strImpiego += " come " + withBlock.ImpiegoPrincipale.Posizione;
                            if (!string.IsNullOrEmpty(withBlock.ImpiegoPrincipale.TipoRapporto))
                                strImpiego += " (" + withBlock.ImpiegoPrincipale.TipoRapporto + ")";
                        }

                        if (!string.IsNullOrEmpty(strImpiego))
                            info.Notes = DMD.Strings.Combine(info.Notes, strImpiego, ", ");
                    }
                }
            }

            string strContatti = "";
            // For i As Integer = 0 To p.Contatti.Count - 1
            // Dim c As CContatto = p.Contatti(i)
            // If (c.Valore <> "") Then strContatti = Strings.Combine(strContatti, c.Nome & ": " & Formats.FormatPhoneNumber(c.Valore), "; ")
            // Next
            // For i As Integer = 0 To p.ContattiWeb.Count - 1
            // Dim c As CContatto = p.ContattiWeb(i)
            // If (c.Valore <> "") Then strContatti = Strings.Combine(strContatti, c.Nome & ": " & c.Valore, "; ")
            // Next

            for (int i = 0, loopTo = p.Recapiti.Count - 1; i <= loopTo; i++)
            {
                var c = p.Recapiti[i];
                if (!string.IsNullOrEmpty(c.Valore))
                    strContatti = DMD.Strings.Combine(strContatti, c.Nome + ": " + Sistema.Formats.FormatPhoneNumber(c.Valore), "; ");
            }

            if (!string.IsNullOrEmpty(strContatti))
                info.Notes += ", Contatti: " + strContatti;
            return info.Notes;
        }

        private CCollection<MergeModuleGruppoPersona> MakeGruppi(CCollection<Anagrafica.CPersonaInfo> items)
        {
            var ret = new CCollection<MergeModuleGruppoPersona>();
            if (items.Count == 0)
                return ret;
            var current = new MergeModuleGruppoPersona();
            int i = 0;
            while (i < items.Count)
            {
                current = new MergeModuleGruppoPersona();
                current.items.Add(items[i]);
                ret.Add(current);
                int j = i + 1;
                while (j < items.Count)
                {
                    var p = items[j];
                    if (current.IsSameGroup(p))
                    {
                        current.items.Add(p);
                        items.RemoveAt(j);
                    }
                    else
                    {
                        j += 1;
                    }
                }

                i += 1;
            }

            return ret;
        }
    }

    public class MergeModuleGruppoPersona 
        : IDMDXMLSerializable
    {
        public CCollection<Anagrafica.CPersonaInfo> items = new CCollection<Anagrafica.CPersonaInfo>();

        public MergeModuleGruppoPersona()
        {
            DMDObject.IncreaseCounter(this);
        }

        public bool IsSameGroup(Anagrafica.CPersonaInfo p2)
        {
            if (items.Count == 0)
                return true;
            var p1 = items[0];
            if (p1.Persona.TipoPersona != p2.Persona.TipoPersona)
                return false;
            if ((p1.Persona.Nominativo ?? "") == (p2.Persona.Nominativo ?? "") && p1.Persona.DataNascita.HasValue && p2.Persona.DataNascita.HasValue && DMD.DateUtils.Compare(p1.Persona.DataNascita, p2.Persona.DataNascita) == 0)
                return true;
            foreach (Anagrafica.CContatto c1 in p1.Persona.Recapiti)
            {
                if (!string.IsNullOrEmpty(c1.Valore)) // AndAlso (c1.Validated .HasValue =False OrElse c1.Validated = True ) Then
                {
                    foreach (Anagrafica.CContatto c2 in p2.Persona.Recapiti)
                    {
                        if ((c2.Valore ?? "") == (c1.Valore ?? ""))
                            return true;
                    }
                }
            }

            return false;
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "items":
                    {
                        items.Clear();
                        items.AddRange((IEnumerable)fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("items", items);
        }

        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        {
            this.XMLSerialize(writer);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        {
            this.SetFieldInternal(fieldName, fieldValue);
        }


        ~MergeModuleGruppoPersona()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}