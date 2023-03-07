using System;
using System.Collections;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using DMD.XML;
using static minidom.Office;

namespace minidom.Forms
{
    /// <summary>
    /// Handler del modulo commissioni
    /// </summary>
    public class CommissioniHandler
        : CBaseModuleHandler
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CommissioniHandler() 
            : base(ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SEdit | ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SDuplicate)
        {
        }

        /// <summary>
        /// Crea il cursore
        /// </summary>
        /// <returns></returns>
        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Office.CommissioneCursor();
        }

        /// <summary>
        /// Carica le attività giornaliere
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public string LoadDaylyActivities(object renderer)
        {
            string testo = this.n2str(renderer, "testo", "");
            using (var cursor = (Office.CommissioneCursor)DMD.XML.Utils.Serializer.Deserialize(testo))
            {
                var ret = new CCollection<Sistema.CCalendarActivity>();
                DateTime d = (DateTime)this.n2date(renderer, "sd");

                // Tutte le commissioni non completate prima della data selezionata
                d = (DateTime)DMD.DateUtils.GetDatePart(d);
                cursor.DataPrevista.Value = d;
                cursor.DataPrevista.Operator = OP.OP_LT;
                cursor.StatoCommissione.ValueIn(new object[] { Office.StatoCommissione.Iniziata, Office.StatoCommissione.NonIniziata, Office.StatoCommissione.Rimandata });
                while (cursor.Read())
                {
                    var c = cursor.Item;
                    var a = new Sistema.CCalendarActivity();
                    a.Tag = c;
                    if (c.DataPrevista.HasValue)
                    {
                        a.DataInizio = (DateTime)c.DataPrevista;
                        a.GiornataIntera = false;
                    }
                    else
                    {
                        a.GiornataIntera = true;
                    }

                    a.DataFine = default;
                    a.Descrizione = c.Motivo + DMD.Strings.vbCr + c.NomePersonaIncontrata + DMD.Strings.vbCr + c.NomeAzienda;
                    ret.Add(a);

                }

                cursor.Reset1();

                // Tutte le commissioni (completate e non) della sola data selezionata
                cursor.DataPrevista.IncludeNulls = false;
                cursor.DataPrevista.Between(d, DMD.DateUtils.DateAdd(DMD.DateTimeInterval.Day, 1d, d));
                cursor.StatoCommissione.Clear();
                while (cursor.Read())
                {
                    var c = cursor.Item;
                    var a = new Sistema.CCalendarActivity();
                    a.Tag = c;
                    a.GiornataIntera = c.GiornataIntera;
                    if (c.DataPrevista.HasValue)
                    {
                        a.DataInizio = (DateTime)c.DataPrevista;
                    }
                    else
                    {
                        a.DataInizio = DMD.DateUtils.Now();
                    }

                    a.DataFine = default;
                    a.Descrizione = c.Motivo + DMD.Strings.vbCr + c.NomePersonaIncontrata + DMD.Strings.vbCr + c.NomeAzienda;
                    ret.Add(a);

                }

                ret.Sort();
                return DMD.XML.Utils.Serializer.Serialize(ret.ToArray());
            }
        }

        /// <summary>
        /// Carica le attività settimanali
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public string LoadWeeklyActivities(object renderer)
        {
            string testo = this.n2str(renderer, "testo", "");
            int cy = (int)this.n2int(renderer, "cy");
            int cw = (int)this.n2int(renderer, "cw");
            var ret = new CCollection<Sistema.CCalendarActivity>();

            using (var cursor = DMD.XML.Utils.Deserialize<CommissioneCursor>(testo))
            {
                // Tutte le commissioni non completate prima della data selezionata
                var d = DMD.DateUtils.GetFirstWeekDay(cy, cw);
                cursor.DataPrevista.Value = d;
                cursor.DataPrevista.Operator = OP.OP_LT;
                cursor.StatoCommissione.ValueIn(new object[] { Office.StatoCommissione.Iniziata, Office.StatoCommissione.NonIniziata, Office.StatoCommissione.Rimandata });
                while (cursor.Read())
                {
                    var c = cursor.Item;
                    var a = new Sistema.CCalendarActivity();
                    a.Tag = c;
                    if (c.DataPrevista.HasValue)
                    {
                        a.DataInizio = (DateTime)c.DataPrevista;
                        a.GiornataIntera = false;
                    }
                    else
                    {
                        a.GiornataIntera = true;
                    }

                    a.DataFine = default;
                    a.Descrizione = c.Motivo + DMD.Strings.vbCr + c.NomePersonaIncontrata + DMD.Strings.vbCr + c.NomeAzienda;
                    ret.Add(a);
                }

                cursor.Reset1();

                // Tutte le commissioni nell'intervallo selezionato
                cursor.DataPrevista.IncludeNulls = false;
                cursor.DataPrevista.Between(d, DMD.DateUtils.DateAdd(DMD.DateTimeInterval.Day, 7d, d));
                cursor.StatoCommissione.Clear();
                while (cursor.Read())
                {
                    var c = cursor.Item;
                    var a = new Sistema.CCalendarActivity();
                    a.Tag = c;
                    a.GiornataIntera = c.GiornataIntera;
                    if (c.DataPrevista.HasValue)
                    {
                        a.DataInizio = (DateTime)c.DataPrevista;
                    }
                    else
                    {
                        a.DataInizio = DMD.DateUtils.Now();
                    }

                    a.DataFine = default;
                    a.Descrizione = c.Motivo + DMD.Strings.vbCr + c.NomePersonaIncontrata + DMD.Strings.vbCr + c.NomeAzienda;
                    ret.Add(a);
                }

            }

            ret.Sort();
            return DMD.XML.Utils.Serializer.Serialize(ret.ToArray());
        }

         

        /// <summary>
        /// Restituisce la collezione dei campi esportabili
        /// </summary>
        /// <returns></returns>
        public override CCollection<ExportableColumnInfo> GetExportableColumnsList()
        {
            var ret = base.GetExportableColumnsList();
            ret.Add(new ExportableColumnInfo("ID", "ID", TypeCode.Int32, true));
            ret.Add(new ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("NomeOperatore", "Operatore", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("StatoCommissione", "StatoCommissione", TypeCode.String, true));
            // ret.Add(New ExportableColumnInfo("OraUscita", "OraUscita", TypeCode.DateTime, True))
            // ret.Add(New ExportableColumnInfo("OraRientro", "OraRientro", TypeCode.DateTime, True))
            // ret.Add(New ExportableColumnInfo("Durata", "Durata", TypeCode.String, True))
            ret.Add(new ExportableColumnInfo("Motivo", "Motivo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Luogo", "Luogo", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("NomeAzienda", "Azienda", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("NomePersonaIncontrata", "Contatto", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("Esito", "Esito", TypeCode.String, true));
            ret.Add(new ExportableColumnInfo("DataPrevista", "DataPrevista", TypeCode.DateTime, true));
            ret.Add(new ExportableColumnInfo("DistanzaPercorsa", "DistanzaPercorsa", TypeCode.Double, true));
            ret.Add(new ExportableColumnInfo("Uscite", "Uscite", TypeCode.String, true));
            return ret;
        }

        /// <summary>
        /// Restituisce il valore del parametro esportabile
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override object GetColumnValue(object renderer, object item, string key)
        {
            Office.Commissione c = (Office.Commissione)item;
            switch (key ?? "")
            {
                case "Durata":
                    {
                        int durata = 0;
                        foreach (Office.CommissionePerUscita u in c.Uscite)
                        {
                            if (u.Uscita is object && u.Uscita.Stato == Databases.ObjectStatus.OBJECT_VALID && u.Uscita.Durata.HasValue)
                                durata = (int)(durata + u.Uscita.Durata);
                        }

                        return Sistema.Formats.FormatDurata(c.Durata);
                    }

                case "DistanzaPercorsa":
                    {
                        double distanza = 0d;
                        foreach (Office.CommissionePerUscita u in c.Uscite)
                        {
                            if (u.Uscita is object && u.Stato == Databases.ObjectStatus.OBJECT_VALID && u.Uscita.DistanzaPercorsa.HasValue)
                                distanza = (double)(distanza + u.Uscita.DistanzaPercorsa);
                        }

                        return distanza;
                    }

                case "Uscite":
                    {
                        string text = "";
                        foreach (Office.CommissionePerUscita u in c.Uscite)
                        {
                            if (u.Uscita is object && u.Stato == Databases.ObjectStatus.OBJECT_VALID)
                                text += Sistema.Formats.FormatUserDateTime(u.Uscita.OraUscita) + " - " + u.Uscita.Descrizione + DMD.Strings.vbCrLf;
                        }

                        return text;
                    }

                case "StatoCommissione":
                    {
                        return FormatStatoCommissione(((Office.Commissione)item).StatoCommissione);
                    }

                default:
                    {
                        return base.GetColumnValue(renderer, item, key);
                    }
            }
        }

        /// <summary>
        /// Imposta il valore del parametro importabile
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void SetColumnValue(object renderer, object item, string key, object value)
        {
            Office.Commissione tmp = (Office.Commissione)item;
            string tmpStr;
            switch (key ?? "")
            {
                case "Durata":
                    {
                        break;
                    }

                case "OraUscita":
                    {
                        break;
                    }

                case "OraRientro":
                    {
                        break;
                    }

                case "DistanzaPercorsa":
                    {
                        break;
                    }

                case "Uscite":
                    {
                        break;
                    }

                case "StatoCommissione":
                    {
                        tmp.StatoCommissione = ParseStatoCommissione(DMD.Strings.CStr(value));
                        break;
                    }

                case "NomePuntoOperativo":
                    {
                        tmpStr = Strings.Trim(key);
                        if (string.IsNullOrEmpty(tmpStr))
                        {
                            tmp.PuntoOperativo = null;
                        }
                        else
                        {
                            tmp.PuntoOperativo = Anagrafica.Uffici.GetItemByName(tmpStr);
                        }

                        break;
                    }

                default:
                    {
                        base.SetColumnValue(renderer, item, key, value);
                        break;
                    }
            }
        }

        /// <summary>
        /// Formatta lo stato della commissione
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatStatoCommissione(Office.StatoCommissione value)
        {
            var items = new Office.StatoCommissione[] { Office.StatoCommissione.Annullata, Office.StatoCommissione.Completata, Office.StatoCommissione.Iniziata, Office.StatoCommissione.NonIniziata, Office.StatoCommissione.Rimandata };
            var names = new string[] { "Annullata", "Completata", "In corso", "In attesa", "Rimandata" };
            return names[DMD.Arrays.IndexOf(items, value)];
        }

        /// <summary>
        /// Interpreta lo stato della commissione
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Office.StatoCommissione ParseStatoCommissione(string value)
        {
            var items = new Office.StatoCommissione[] { Office.StatoCommissione.Annullata, Office.StatoCommissione.Completata, Office.StatoCommissione.Iniziata, Office.StatoCommissione.NonIniziata, Office.StatoCommissione.Rimandata };
            var names = new string[] { "Annullata", "Completata", "In corso", "In attesa", "Rimandata" };
            return items[DMD.Arrays.IndexOf(names, value)];
        }

        /// <summary>
        /// Restituisce true se l'elemento può essere modificato
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CanEdit(object item)
        {
            return base.CanEdit(item) || ((Office.Commissione)item).IDOperatore == Databases.GetID(Sistema.Users.CurrentUser) && Module.UserCanDoAction("edit_assigned");
        }

        /// <summary>
        /// Restituisce true se l'elemento può essere eliminato
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CanDelete(object item)
        {
            return base.CanDelete(item) || ((Office.Commissione)item).IDOperatore == Databases.GetID(Sistema.Users.CurrentUser) && Module.UserCanDoAction("delete_assigned");
        }

        /// <summary>
        /// Restituisce l'elenco delle commissioni da fare
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public Sistema.MethodResults GetCommissioniDaFare(object renderer)
        {
            int opid = (int)this.n2int(renderer, "opid");
            var finoa = (DateTime)this.n2date(renderer, "finoa");
            var ret = Office.Commissioni.GetCommissioniDaFare(opid, finoa);
            return new Sistema.MethodResults(ret);
        }

        /// <summary>
        /// Restituisce l'elenco delle commissioni in corso
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public Sistema.MethodResults GetCommissioniInCorso(object renderer)
        {
            int opid = (int)this.n2int(renderer, "opid");
            var ret = Office.Commissioni.GetCommissioniInCorso(opid);
            return new Sistema.MethodResults(ret);
        }

        /// <summary>
        /// Restituisce l'elenco delle commissioni suggerite
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        public Sistema.MethodResults GetCommissioniSuggerite(object renderer)
        {
            string p = this.n2str(renderer, "p", "");
            string l = this.n2str(renderer, "l", "");
            bool strictMode = (bool)this.n2bool(renderer, "sm");
            int[] aziende = null;
            var luoghi = new CCollection<Office.LuogoDaVisitare>();
            if (!string.IsNullOrEmpty(p))
                aziende = (int[])DMD.Arrays.Convert<int>(DMD.XML.Utils.Serializer.Deserialize(p));
            if (!string.IsNullOrEmpty(l))
                luoghi.AddRange((IEnumerable)DMD.XML.Utils.Serializer.Deserialize(l));
            var ret = Office.Commissioni.GetCommissioniSuggerite(aziende, luoghi, strictMode);
            return new Sistema.MethodResults(ret);
        }
    }
}