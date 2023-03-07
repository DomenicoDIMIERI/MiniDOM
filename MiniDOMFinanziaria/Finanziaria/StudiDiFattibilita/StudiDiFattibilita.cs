using System;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using internals;
using static minidom.Sistema;
using static minidom.Finanziaria;
using static minidom.Databases;

namespace internals
{

    [Serializable]
    public sealed class CStudiDiFattibilitaClass
          : CModulesClass<CQSPDStudioDiFattibilita>
    {
        internal CStudiDiFattibilitaClass() : base("modGruppiConsulenzeCQS", typeof(CQSPDStudiDiFattibilitaCursor))
        {
        }

        public CQSPDStudioDiFattibilita GetUltimoStudioDiFattibilita(Anagrafica.CPersonaFisica persona)
        {
            if (persona is null)
                throw new ArgumentNullException("persona");
            return GetUltimoStudioDiFattibilita(minidom.DBUtils.GetID(persona));
        }

        public CCollection<OggettoAnomalo> GetAnomalie(int idUfficio, int idOperatore, DateTime? dal, DateTime? al, int ritardoConsentito = 1)
        {
            IDataReader dbRis = null;
            CQSPDConsulenza consulenza;
            string dbSQL;
            var ret = new CCollection<OggettoAnomalo>();

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            dbSQL = "";
            dbSQL += "SELECT * FROM [tbl_CQSPDConsulenze] LEFT JOIN (";
            dbSQL += "SELECT [ID] As [IDPrat], [IDConsulenza] FROM [tbl_Pratiche] WHERE [Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString() + " AND [IDAzienda]=" + minidom.DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale);
            dbSQL += ") As [T1] ON [tbl_CQSPDConsulenze].[ID]=[T1].[IDConsulenza]";
            dbSQL += " WHERE [T1].[IDPrat] Is Null And [tbl_CQSPDConsulenze].[Stato] = " + ((int)ObjectStatus.OBJECT_VALID).ToString();
            if (idUfficio != 0)
                dbSQL += " AND [tbl_CQSPDConsulenze].[IDPuntoOperativo] = " + minidom.DBUtils.DBNumber(idUfficio);
            if (idOperatore != 0)
                dbSQL += " AND [tbl_CQSPDConsulenze].[IDConfermataDa] = " + minidom.DBUtils.DBNumber(idOperatore);
            if (dal.HasValue)
                dbSQL += " AND [tbl_CQSPDConsulenze].[DataConsulenza] >= " + minidom.DBUtils.DBDate(dal.Value);
            if (al.HasValue)
                dbSQL += " AND [tbl_CQSPDConsulenze].[DataConsulenza] <= " + minidom.DBUtils.DBDate(al.Value);
            dbSQL += " And [tbl_CQSPDConsulenze].[IDProduttore] = " + minidom.DBUtils.GetID(Anagrafica.Aziende.AziendaPrincipale) + " And [tbl_CQSPDConsulenze].[StatoConsulenza] = " + ((int)StatiConsulenza.ACCETTATA).ToString();
            dbRis = Database.ExecuteReader(dbSQL);
            while (dbRis.Read())
            {
                consulenza = new CQSPDConsulenza();
                Database.Load(consulenza, dbRis);
                var oggetto = new OggettoAnomalo();
                oggetto.Oggetto = consulenza;
                // oggetto.Operatore = consulenza.ConfermataDa

                var d1 = consulenza.CreatoIl;
                if (consulenza.DataConferma.HasValue)
                    d1 = consulenza.DataConferma.Value;
                int ritardo = (int)DMD.DateUtils.DateDiff(DateTimeInterval.Day, d1, DMD.DateUtils.Now());
                oggetto.AggiungiAnomalia("La proposta è stata accettata da " + ritardo + " e non è stata ancora generato il secci", 0);
                ret.Add(oggetto);
            }

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            if (dbRis is object)
                dbRis.Dispose();
            dbRis = null;
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */

            return ret;
        }

        public CQSPDStudioDiFattibilita GetUltimoStudioDiFattibilita(int idPersona)
        {
            if (idPersona == 0)
                return null;

            using (var cursor = new CQSPDStudiDiFattibilitaCursor())
            {
                cursor.PageSize = 1;
                cursor.IDCliente.Value = idPersona;
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.Data.SortOrder = SortEnum.SORT_DESC;
                return cursor.Item;
            }
        }

        public CCollection<CQSPDStudioDiFattibilita> GetStudiDiFattibilitaByPersona(Anagrafica.CPersona persona)
        {
            if (persona is null)
                throw new ArgumentNullException("persona");

            var ret = new CCollection<CQSPDStudioDiFattibilita>();
            if (minidom.DBUtils.GetID(persona) == 0)
                return ret;

            using (var cursor = new CQSPDStudiDiFattibilitaCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDCliente.Value = GetID(persona);
                cursor.Data.SortOrder = SortEnum.SORT_DESC;
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    var sf = cursor.Item;
                    sf.SetCliente(persona);
                    ret.Add(sf);
                    cursor.MoveNext();
                }

            }

            return ret;
        }

        public CCollection<CQSPDStudioDiFattibilita> GetStudiDiFattibilitaByPersona(int idPersona)
        {
            return GetStudiDiFattibilitaByPersona(Anagrafica.Persone.GetItemById(idPersona));
        }

        public CCollection<CQSPDStudioDiFattibilita> GetStudiDiFattibilitaByRichiesta(CRichiestaFinanziamento richiesta)
        {
            if (richiesta is null)
                throw new ArgumentNullException("richiesta");
            return GetStudiDiFattibilitaByPersona(minidom.DBUtils.GetID(richiesta));
        }

        public CCollection<CQSPDStudioDiFattibilita> GetStudiDiFattibilitaByRichiesta(int idRichiesta)
        {
            var ret = new CCollection<CQSPDStudioDiFattibilita>();
            if (idRichiesta != 0)
            {
                using (var cursor = new CQSPDStudiDiFattibilitaCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDRichiesta.Value = idRichiesta;
                    cursor.Data.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                }
            }

            return ret;
        }

        public string ParseTemplate(string template, CQSPDStudioDiFattibilita consulenza, string baseURL)
        {
            string ret = template;
            ret = Strings.Replace(ret, "%%NOMECONSULENTE%%", consulenza.NomeConsulente);
            ret = Strings.Replace(ret, "%%NOMECLIENTE%%", consulenza.NomeCliente);
            ret = Strings.Replace(ret, "%%ID%%", minidom.DBUtils.GetID(consulenza).ToString());
            ret = Strings.Replace(ret, "%%BASEURL%%", Sistema.ApplicationContext.BaseURL);
            return ret;
        }

        public void AggiornaPratiche(int idConsulenza)
        {
            if (idConsulenza == 0)
                return;
            AggiornaPratiche(Consulenze.GetItemById(idConsulenza));
        }

        public void AggiornaPratiche(CQSPDConsulenza consulenza)
        {
            if (consulenza is null)
                throw new ArgumentNullException("consulenza");
            if (consulenza.StudioDiFattibilita is object)
                AggiornaPratiche(consulenza.StudioDiFattibilita);
        }

        /// <summary>
        /// Aggiorna la tabella delle statistiche relative alla pratiche generate da questo studio di fattibilità
        /// </summary>
        /// <param name="studiof"></param>
        /// <remarks></remarks>
        public void AggiornaPratiche(CQSPDStudioDiFattibilita studiof)
        {
            lock (this)
            {
                if (studiof is null)
                    throw new ArgumentNullException("studiof");
                int cnt = 0;
                int cntOk = 0;
                int cntNo = 0;
                var stLiquidata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA);
                var stAnnullata = StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA);
                foreach (CQSPDConsulenza consulenza in studiof.Proposte)
                {
                    cnt += Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [IDConsulenza]=" + minidom.DBUtils.GetID(consulenza) + " AND [Stato]=" + ((int)minidom.ObjectStatus.OBJECT_VALID).ToString()));
                    cntOk += Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [IDConsulenza]=" + minidom.DBUtils.GetID(consulenza) + " AND [Stato]=" + ((int)minidom.ObjectStatus.OBJECT_VALID).ToString() + " AND [IDStatoAttuale]=" + minidom.DBUtils.GetID(stLiquidata)));
                    cntNo += Sistema.Formats.ToInteger(Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Pratiche] WHERE [IDConsulenza]=" + minidom.DBUtils.GetID(consulenza) + " AND [Stato]=" + ((int)minidom.ObjectStatus.OBJECT_VALID).ToString() + " AND [IDStatoAttuale]=" + minidom.DBUtils.GetID(stAnnullata)));
                }

                Database.ExecuteCommand("UPDATE [tbl_CQSPDGrpConsulenze] Set [CntPratiche]=" + cnt + ", [CntPraticheOk]=" + cntOk + ", [CntPraticheNo]=" + cntNo + " WHERE [ID]=" + minidom.DBUtils.GetID(studiof));
            }
        }
    }


}

namespace minidom
{
    public partial class Finanziaria
    {

      
        private static CStudiDiFattibilitaClass m_StudiDiFattibilita = null;

        public static CStudiDiFattibilitaClass StudiDiFattibilita
        {
            get
            {
                if (m_StudiDiFattibilita is null)
                    m_StudiDiFattibilita = new CStudiDiFattibilitaClass();
                return m_StudiDiFattibilita;
            }
        }
    }
}