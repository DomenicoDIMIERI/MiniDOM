using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using DMD;
using DMD.XML;

namespace minidom.Forms
{

    [Serializable]
    public class ControlPanelInfo 
        : IDMDXMLSerializable
    {
        public CCollection<ControlPanelOfficeInfo> Uffici;
        public CCollection<CustomerCalls.ContattoInAttesaInfo> VisiteRicevute;
        public CCollection Azioni;

        public ControlPanelInfo()
        {
            DMDObject.IncreaseCounter(this);
        }

        /// <summary>
        /// Carica tutte le informazioni iniziali
        /// </summary>
        /// <remarks></remarks>
        public void Load(DateTime fromDate)
        {
            var t = DMD.DateUtils.Now();
            this.Uffici = new CCollection<ControlPanelOfficeInfo>();
            this.VisiteRicevute = new CCollection<CustomerCalls.ContattoInAttesaInfo>();
            this.Azioni = new CCollection();
            var items = Anagrafica.Uffici.GetPuntiOperativiConsentiti();
            foreach (Anagrafica.CUfficio ufficio in items)
            {
                if (ufficio.IsValid())
                {
                    var info = new ControlPanelOfficeInfo(ufficio);
                    info.Load(fromDate);
                    Uffici.Add(info);
                }
            }

            Debug.Print("CControlPanelOfficeInfo.Load1: " + (DMD.DateUtils.Now() - t).TotalSeconds);

            string dbSQL = "SELECT * FROM [tbl_Telefonate] WHERE [Stato]=" + ((int)Databases.ObjectStatus.OBJECT_VALID).ToString() + " AND (";
            dbSQL += "([ClassName]='CTelefonata' AND ([Data]>=" + Databases.DBUtils.DBDate(fromDate) + " OR [StatoConversazione]=" + ((int)CustomerCalls.StatoConversazione.INCORSO).ToString() + ")) OR ";
            dbSQL += "([ClassName]='CVisita' AND [Ricevuta]=True AND ([Data]>=" + Databases.DBUtils.DBDate(fromDate) + " OR [StatoConversazione]<>" + ((int)CustomerCalls.StatoConversazione.CONCLUSO).ToString() + "))";
            dbSQL += ")";

            using (var dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL))
            {
                while (dbRis.Read())
                {
                    CustomerCalls.CContattoUtente c;
                    if (Sistema.Formats.ToString(dbRis["ClassName"]) == "CTelefonata")
                    {
                        c = new CustomerCalls.CTelefonata();
                        CustomerCalls.CRM.TelDB.Load(c, dbRis);
                        Azioni.Add(new CustomerCalls.ContattoInAttesaInfo(c));
                    }
                    else
                    {
                        c = new CustomerCalls.CVisita();
                        Databases.APPConn.Load(c, dbRis);
                        if (c.StatoConversazione == CustomerCalls.StatoConversazione.CONCLUSO)
                        {
                            VisiteRicevute.Add(new CustomerCalls.ContattoInAttesaInfo(c));
                        }
                        else
                        {
                            Azioni.Add(new CustomerCalls.ContattoInAttesaInfo(c));
                        }
                    }
                }

            }

            Debug.Print("CControlPanelOfficeInfo.Load2: " + (DMD.DateUtils.Now() - t).TotalSeconds);
             
        }

        public void Update(DateTime fromDate)
        {
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Uffici":
                    {
                        Uffici = new CCollection<ControlPanelOfficeInfo>();
                        if (fieldValue is IEnumerable)
                        {
                            Uffici.AddRange((IEnumerable)fieldValue);
                        }
                        else if (fieldValue is ControlPanelOfficeInfo)
                        {
                            Uffici.Add((ControlPanelOfficeInfo)fieldValue);
                        }

                        break;
                    }

                case "VisiteRicevute":
                    {
                        VisiteRicevute = new CCollection<CustomerCalls.ContattoInAttesaInfo>();
                        if (fieldValue is IEnumerable)
                        {
                            VisiteRicevute.AddRange((IEnumerable)fieldValue);
                        }
                        else if (fieldValue is CustomerCalls.CVisita)
                        {
                            VisiteRicevute.Add((CustomerCalls.ContattoInAttesaInfo)fieldValue);
                        }

                        break;
                    }

                case "Azioni":
                    {
                        Azioni = new CCollection();
                        if (fieldValue is IEnumerable)
                        {
                            Azioni.AddRange((IEnumerable)fieldValue);
                        }
                        else if (fieldValue is object)
                        {
                            Azioni.Add(fieldValue);
                        }

                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteTag("Uffici", Uffici);
            writer.WriteTag("VisiteRicevute", VisiteRicevute);
            writer.WriteTag("Azioni", Azioni);
        }

        ~ControlPanelInfo()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}