using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class CPersoneGiuridicheModuleHandler : CBaseModuleHandler
    {
        public CPersoneGiuridicheModuleHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override object GetInternalItemById(int id)
        {
            return Anagrafica.Aziende.GetItemById(id);
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var ret = new Anagrafica.CPersonaCursor();
            ret.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_GIURIDICA;
            // ret.TipoPersona.Operator = OP.OP_NE
            return ret;
        }

        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("RagioneSociale", "Ragione Sociale", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("CodiceFiscale", "Codice Fiscale", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("PartitaIVA", "Partita IVA", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("DataNascita", "Data Fondazione", TypeCode.DateTime, true));
            ret.Add(new ExportableColumnInfo("FonteContatto", "Nome Fonte", TypeCode.DateTime, true));
            return ret;
        }

        public string getAziendaPrincipale(object renderer)
        {
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(Anagrafica.Aziende.AziendaPrincipale, XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string getImpiegati(object renderer)
        {
            int aid = (int)this.n2int(renderer, "aid");
            var azienda = Anagrafica.Aziende.GetItemById(aid);
            if (azienda is null)
                throw new ArgumentNullException("Azienda");
            if (azienda.Impiegati.Count == 0)
                return "";
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(azienda.Impiegati.ToArray(), XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string GetArrayFormeGiuridiche(object renderer)
        {
            var col = new CCollection<Anagrafica.CFormaGiuridicaAzienda>();
            var cursor = new Anagrafica.CFormeGiuridicheAziendaCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.IgnoreRights = true;
            while (!cursor.EOF())
            {
                col.Add(cursor.Item);
                cursor.MoveNext();
            }

            cursor.Dispose();
            if (col.Count == 0)
                return "";
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(col.ToArray(), XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string GetArrayCategorie(object renderer)
        {
            var col = new CCollection<Anagrafica.CCategoriaAzienda>();
            var cursor = new Anagrafica.CCategorieAziendaCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.IgnoreRights = true;
            while (!cursor.EOF())
            {
                col.Add(cursor.Item);
                cursor.MoveNext();
            }

            cursor.Dispose();
            if (col.Count == 0)
                return "";
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(col.ToArray(), XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public string GetArrayTipologie(object renderer)
        {
            var col = new CCollection<Anagrafica.CTipologiaAzienda>();
            var cursor = new Anagrafica.CTipologiaAziendaCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Nome.SortOrder = Databases.SortEnum.SORT_ASC;
            cursor.IgnoreRights = true;
            while (!cursor.EOF())
            {
                col.Add(cursor.Item);
                cursor.MoveNext();
            }

            cursor.Dispose();
            if (col.Count == 0)
                return "";
            var serializer = new XMLSerializer();
            try
            {
                return serializer.Serialize(col.ToArray(), XMLSerializeMethod.Document);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // serializer.Dispose()
            }
        }

        public override bool CanEdit(object item)
        {
            bool ret = base.CanEdit(item);
            Anagrafica.CPersona p = (Anagrafica.CPersona)item;
            if ((ret == false && Module.UserCanDoAction("list_assigned")) & (p.IDReferente1 == Databases.GetID(Sistema.Users.CurrentUser) || p.IDReferente2 == Databases.GetID(Sistema.Users.CurrentUser)))
            {
                ret = true;
            }

            return ret;
        }

        public override bool CanList(object item)
        {
            bool ret = base.CanList(item);
            Anagrafica.CPersona p = (Anagrafica.CPersona)item;
            if ((ret == false && Module.UserCanDoAction("list_assigned")) & (p.IDReferente1 == Databases.GetID(Sistema.Users.CurrentUser) || p.IDReferente2 == Databases.GetID(Sistema.Users.CurrentUser)))
            {
                ret = true;
            }

            return ret;
        }
    }
}