using System;
using System.Collections;
using DMD;

namespace minidom.Forms
{
    public class FinestreDiLavorazioneHandler : CBaseModuleHandler
    {
        public FinestreDiLavorazioneHandler() : base(ModuleSupportFlags.SAnnotations | ModuleSupportFlags.SCreate | ModuleSupportFlags.SDelete | ModuleSupportFlags.SDuplicate | ModuleSupportFlags.SEdit)
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            var cursor = new Finanziaria.FinestraLavorazioneCursor();
            return cursor;
        }

        public override object GetInternalItemById(int id)
        {
            return Finanziaria.FinestreDiLavorazione.GetItemById(id);
        }

        public string GetUltimaFinestraLavorata(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            var w = Finanziaria.FinestreDiLavorazione.GetUltimaFinestraLavorata(p);
            if (w is object)
            {
                return DMD.XML.Utils.Serializer.Serialize(w);
            }
            else
            {
                return "";
            }
        }

        public string GetProssimaFinestra(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            var w = Finanziaria.FinestreDiLavorazione.GetProssimaFinestra(p);
            if (w is object)
            {
                return DMD.XML.Utils.Serializer.Serialize(w);
            }
            else
            {
                return "";
            }
        }

        public string GetFinestraCorrente(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            var w = Finanziaria.FinestreDiLavorazione.GetFinestraCorrente(p);
            if (w is object)
            {
                return DMD.XML.Utils.Serializer.Serialize(w);
            }
            else
            {
                return "";
            }
        }

        public string GetFinestreByPersona(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            var col = Finanziaria.FinestreDiLavorazione.GetFinestreByPersona(p);
            return DMD.XML.Utils.Serializer.Serialize(col);
        }

        public string AggiornaFinestraLavorazione(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            int wid = (int)this.n2int(renderer, "wid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            var w = Finanziaria.FinestreDiLavorazione.GetItemById(wid);
            Finanziaria.FinestreDiLavorazione.AggiornaFinestraLavorazione(p, w);
            return "";
        }

        public string CalcolaDataLavorabilita(object renderer)
        {
            int pid = (int)this.n2int(renderer, "pid");
            Anagrafica.CPersonaFisica p = (Anagrafica.CPersonaFisica)Anagrafica.Persone.GetItemById(pid);
            return DMD.XML.Utils.Serializer.SerializeDate(Finanziaria.FinestreDiLavorazione.CalcolaDataLavorabilita(p));
        }

        private int[] StrToArrI(string str)
        {
            var ret = new ArrayList();
            var arr = Strings.Split(str, ",");
            if (DMD.Arrays.Len(arr) > 0)
            {
                foreach (string s in arr)
                {
                    int id = Sistema.Formats.ToInteger(s);
                    if (id != 0)
                        ret.Add(id);
                }
            }

            return (int[])ret.ToArray(typeof(int));
        }

        private string MakeDateNib(string fildName, DateTime? di, DateTime? df)
        {
            string ret = "";
            if (di.HasValue)
                ret += "[" + fildName + "]>=" + Databases.DBUtils.DBDate(di.Value);
            if (df.HasValue)
                ret = DMD.Strings.Combine(ret, "[" + fildName + "]<=" + Databases.DBUtils.DBDate(df.Value), " AND ");
            if (string.IsNullOrEmpty(ret))
                return "";
            return ret;
        }

        private string JoinPunti(int[] items)
        {
            var buffer = new System.Text.StringBuilder();
            foreach (int i in items)
            {
                if (buffer.Length > 0)
                    buffer.Append(",");
                buffer.Append(Databases.DBUtils.DBNumber(i));
            }

            return buffer.ToString();
        }

        public string GetClientiContattati(object renderer)
        {
            var di = this.n2date(renderer, "di");
            var df = this.n2date(renderer, "df");
            var punti = StrToArrI(this.n2str(renderer, "po"));
            if (df.HasValue == false && df.Value > DMD.DateUtils.ToMorrow())
                df = DMD.DateUtils.ToMorrow();
            df = DMD.DateUtils.Min(df, DMD.DateUtils.ToMorrow());
            var cursor = new Finanziaria.FinestraLavorazioneCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.IDPuntoOperativo.ValueIn(punti);
            // cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
            // cursor.StatoFinestra.Operator = OP.OP_NE

            string dbSQL = "";
            string wherePart = "";
            string tmpN;
            dbSQL += "SELECT * FROM (" + cursor.GetSQL() + ") ";
            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }


            // tmpN = Me.MakeDateNib("DataInizioLavorabilita", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            // tmpN = Me.MakeDateNib("DataInizioLavorazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            // tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            // tmpN = Me.MakeDateNib("DataBustaPaga", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = MakeDateNib("DataContatto", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            // tmpN = Me.MakeDateNib("DataRichiestaCertificato", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            // tmpN = Me.MakeDateNib("DataEsportazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)

            if (!string.IsNullOrEmpty(wherePart))
                dbSQL += " WHERE (" + wherePart + ")";
            var finestre = new CKeyCollection<Finanziaria.FinestraLavorazione>();
            var dbRis = Finanziaria.Database.ExecuteReader(dbSQL);
            while (dbRis.Read())
            {
                var w = new Finanziaria.FinestraLavorazione();
                Finanziaria.Database.Load(w, dbRis);
                int pid = w.IDCliente;
                if (pid != 0)
                    finestre.Add("K" + pid, w);
                // cursor.MoveNext()
            }

            cursor.Dispose();
            finestre = RimuoviFinestreNonValide(finestre);
            return DMD.XML.Utils.Serializer.Serialize(finestre);
        }

        private CKeyCollection<Finanziaria.FinestraLavorazione> RimuoviFinestreNonValide(CKeyCollection<Finanziaria.FinestraLavorazione> items)
        {
            // Estraiamo solo gli id delle persone eliminate o non valide
            var buffer = new System.Text.StringBuilder();
            foreach (Finanziaria.FinestraLavorazione w in items)
            {
                if (buffer.Length > 0)
                    buffer.Append(",");
                buffer.Append(Databases.DBUtils.DBNumber(w.IDCliente));
            }

            if (buffer.Length == 0)
                return items;
            var ret = new CKeyCollection<Finanziaria.FinestraLavorazione>();
            string dbSQL = "SELECT [ID] FROM [tbl_Persone] WHERE [ID] In (" + buffer.ToString() + ") AND [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString();
            var dbRis = Databases.APPConn.ExecuteReader(dbSQL);
            while (dbRis.Read())
            {
                int pid = Sistema.Formats.ToInteger(dbRis["ID"]);
                foreach (Finanziaria.FinestraLavorazione w in items)
                {
                    if (w.IDCliente == pid)
                    {
                        ret.Add("K" + pid, w);
                    }
                }
            }

            dbRis.Dispose();
            return ret;
        }

        public string GetClientiLavorabili(object renderer)
        {
            var di = this.n2date(renderer, "di");
            var df = this.n2date(renderer, "df");
            var punti = StrToArrI(this.n2str(renderer, "po"));
            var finestre = new CKeyCollection<Finanziaria.FinestraLavorazione>();


            using (var cursor = new Finanziaria.FinestraLavorazioneCursor())
            { 
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                cursor.IDPuntoOperativo.ValueIn(punti);
                // cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
                // cursor.StatoFinestra.Operator = OP.OP_NE

                string dbSQL = "";
                string wherePart = "";
                string tmpN;
                dbSQL += "SELECT * FROM (" + cursor.GetSQL() + ") ";
                cursor.Reset1();
                tmpN = MakeDateNib("DataInizioLavorabilita", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                tmpN = MakeDateNib("DataInizioLavorazione", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                // tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
                tmpN = MakeDateNib("DataBustaPaga", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                tmpN = MakeDateNib("DataContatto", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                tmpN = MakeDateNib("DataRichiestaCertificato", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                tmpN = MakeDateNib("DataEsportazione", di, df);
                wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
                if (!string.IsNullOrEmpty(wherePart))
                    dbSQL += " WHERE (" + wherePart + ")";

                using (var dbRis = Finanziaria.Database.ExecuteReader(dbSQL))
                {
                    while (dbRis.Read())
                    {
                        var w = new Finanziaria.FinestraLavorazione();
                        Finanziaria.Database.Load(w, dbRis);
                        int pid = w.IDCliente;
                        if (pid != 0)
                            finestre.Add("K" + pid, w);
                        cursor.MoveNext();
                    }
                }
            }

            finestre = RimuoviFinestreNonValide(finestre);
            return DMD.XML.Utils.Serializer.Serialize(finestre);
        }

        public string GetClientiRinnovabili(object renderer)
        {
            var di = this.n2date(renderer, "di");
            var df = this.n2date(renderer, "df");
            var punti = StrToArrI(this.n2str(renderer, "po"));
            var cursor = new Finanziaria.FinestraLavorazioneCursor();
            cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
            cursor.Flags.Value = Finanziaria.FinestraLavorazioneFlags.Rinnovo;
            cursor.Flags.Operator = OP.OP_ALLBITAND;
            cursor.IDPuntoOperativo.ValueIn(punti);
            string dbSQL = "";
            string wherePart = "";
            string tmpN;
            dbSQL += "SELECT * FROM (" + cursor.GetSQL() + ") ";
            cursor.Reset1();
            tmpN = MakeDateNib("DataInizioLavorabilita", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            tmpN = MakeDateNib("DataInizioLavorazione", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            // tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = MakeDateNib("DataBustaPaga", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            tmpN = MakeDateNib("DataContatto", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            tmpN = MakeDateNib("DataRichiestaCertificato", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            tmpN = MakeDateNib("DataEsportazione", di, df);
            wherePart = DMD.Strings.Combine(wherePart, tmpN, " OR ", true);
            if (!string.IsNullOrEmpty(wherePart))
                dbSQL += " WHERE (" + wherePart + ")";
            var finestre = new CKeyCollection<Finanziaria.FinestraLavorazione>();
            var dbRis = Finanziaria.Database.ExecuteReader(dbSQL);
            while (dbRis.Read())
            {
                var w = new Finanziaria.FinestraLavorazione();
                Finanziaria.Database.Load(w, dbRis);
                int pid = w.IDCliente;
                if (pid != 0)
                    finestre.Add("K" + pid, w);
                cursor.MoveNext();
            }

            if (cursor is object)
            {
                cursor.Dispose();
                cursor = null;
            }

            finestre = RimuoviFinestreNonValide(finestre);
            return DMD.XML.Utils.Serializer.Serialize(finestre);
        }
    }
}