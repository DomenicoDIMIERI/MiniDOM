using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {
        /// <summary>
        /// Flag validi per un articolo
        /// </summary>
        public enum ArticoloFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            None = 0
        }

        /// <summary>
        /// Definisce un oggetto in magazzino
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class Articolo 
            : minidom.Databases.DBObject
        {
            private string m_Nome;            // Nome dell'articolo
            private string m_Marca;
            private string m_Modello;
            private string m_Descrizione;
            private string m_CategoriaPrincipale;
            private string m_IconURL;
            private string m_ProductPage;
            private string m_SupportPage;
            private string m_UnitaBase;
            private int m_DecimaliValuta;
            private int m_DecimaliQuantita;
            private AttributiArticoloCollection m_Attributi;
            private CCodiciPerArticolo m_Codici;

            /// <summary>
            /// Costruttore
            /// </summary>
            public Articolo()
            {
                m_Nome = "";
                m_Flags = (int) ArticoloFlags.None;
                m_Attributi = null;
                m_Marca = "";
                m_CategoriaPrincipale = "";
                m_Modello = "";
                m_Descrizione = "";
                m_IconURL = "";
                m_Codici = null;
                m_UnitaBase = "";
                m_DecimaliValuta = -1;
                m_DecimaliQuantita = -1;
                m_ProductPage = "";
                m_SupportPage = "";
            }

            /// <summary>
            /// Restituisce o imposta la URL della pagina descrittiva del prodtotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string ProductPage
            {
                get
                {
                    return m_ProductPage;
                }

                set
                {
                    string oldValue = m_ProductPage;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ProductPage = value;
                    DoChanged("ProductPage", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la URL della pagina di supporto del prodotto
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string SupportPage
            {
                get
                {
                    return m_SupportPage;
                }

                set
                {
                    string oldValue = m_SupportPage;
                    value = DMD.Strings.Trim(value);
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_SupportPage = value;
                    DoChanged("SupportPage", value, oldValue);
                }
            }


            /// <summary>
            /// Restituisce o imposta l'unità di misura base per l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string UnitaBase
            {
                get
                {
                    return m_UnitaBase;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_UnitaBase;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UnitaBase = value;
                    DoChanged("UnitaBase", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i decimali validi per i prezzi. Un valore negativo indica al programma di utilizzare tutti i numeri disponibili
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int DecimaliValuta
            {
                get
                {
                    return m_DecimaliValuta;
                }

                set
                {
                    int oldValue = m_DecimaliValuta;
                    if (oldValue == value)
                        return;
                    m_DecimaliValuta = value;
                    DoChanged("DecimaliValuta", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta i decimali validi per le quantità. Un valore negativo indica al programma di utilizzare tutti i numeri disponibili
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public int DecimaliQuantita
            {
                get
                {
                    return m_DecimaliQuantita;
                }

                set
                {
                    int oldValue = m_DecimaliQuantita;
                    if (oldValue == value)
                        return;
                    m_DecimaliQuantita = value;
                    DoChanged("DecimaliQuantita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la categoria principale dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string CategoriaPrincipale
            {
                get
                {
                    return m_CategoriaPrincipale;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_CategoriaPrincipale;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CategoriaPrincipale = value;
                    DoChanged("CategoriaPrincipale", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la marca dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Marca
            {
                get
                {
                    return m_Marca;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Marca;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Marca = value;
                    DoChanged("Marca", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il modello dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Modello
            {
                get
                {
                    return m_Modello;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Modello;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Modello = value;
                    DoChanged("Modello", value, oldValue);
                }
            }

            /// <summary>
            /// Restitusice una collezione di proprietà aggiuntive
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public AttributiArticoloCollection Attributi
            {
                get
                {
                    lock (this)
                    {
                        if (m_Attributi is null)
                            m_Attributi = new AttributiArticoloCollection(this);
                        return m_Attributi;
                    }
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive in dettaglio l'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta la url dell'immagine principale dell'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string IconURL
            {
                get
                {
                    return m_IconURL;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_IconURL;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconURL = value;
                    DoChanged("IconURL", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una serie di flags associati all'articolo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new ArticoloFlags Flags
            {
                get
                {
                    return (ArticoloFlags) base.Flags;
                }

                set
                {
                    var oldValue = this.Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce la collezione dei codici articolo associati
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCodiciPerArticolo Codici
            {
                get
                {
                    lock (this)
                    {
                        if (m_Codici is null)
                            m_Codici = new CCodiciPerArticolo(this);
                        return m_Codici;
                    }
                }
            }

            /// <summary>
            /// Restituisce il valore del codice con nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public string getValoreCodice(string nome)
            {
                var item = Codici.GetItemByKey(nome);
                if (item is null)
                    return "";
                return item.Valore;
            }

            /// <summary>
            /// Imposta il valore del codice con nome
            /// </summary>
            /// <param name="nome"></param>
            /// <param name="valore"></param>
            public void setValoreCodice(string nome, string valore)
            {
                var item = Codici.GetItemByKey(nome);
                if (item is null)
                {
                    item = new CodiceArticolo();
                    item.Nome = nome;
                    Codici.Add(nome, item);
                }

                item.Valore = valore;
                item.Stato = ObjectStatus.OBJECT_VALID;
                item.Save();
            }

            /// <summary>
            /// Restituisce o imposta il valore del codice articolo
            /// </summary>
            public string CodiceArticolo
            {
                get
                {
                    return getValoreCodice("Codice Articolo");
                }

                set
                {
                    setValoreCodice("Codice Articolo", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del codice a barre
            /// </summary>
            public string CodiceABarre
            {
                get
                {
                    return getValoreCodice("Codice a Barre");
                }

                set
                {
                    setValoreCodice("Codice a Barre", value);
                }
            }

            /// <summary>
            /// Restituisce o imposta il valore del codice fornitore
            /// </summary>
            public string CodiceFornitore
            {
                get
                {
                    return getValoreCodice("Codice Fornitore");
                }

                set
                {
                    setValoreCodice("Codice Fornitore", value);
                }
            }


            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("CategoriaPrincipale", m_CategoriaPrincipale);
                writer.WriteAttribute("Marca", m_Marca);
                writer.WriteAttribute("Modello", m_Modello);
                writer.WriteAttribute("IconURL", m_IconURL);
                writer.WriteAttribute("UnitaBase", m_UnitaBase);
                writer.WriteAttribute("DecimaliValuta", m_DecimaliValuta);
                writer.WriteAttribute("DecimaliQuantita", m_DecimaliQuantita);
                writer.WriteAttribute("ProductPage", m_ProductPage);
                writer.WriteAttribute("SupportPage", m_SupportPage);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
                writer.WriteTag("Attributi", Attributi);
                writer.WriteTag("Codici", Codici);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "CategoriaPrincipale":
                        {
                            m_CategoriaPrincipale = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Marca":
                        {
                            m_Marca = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Modello":
                        {
                            m_Modello = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "IconURL":
                        {
                            m_IconURL = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                

                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "UnitaBase":
                        {
                            m_UnitaBase = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DecimaliValuta":
                        {
                            m_DecimaliValuta = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "DecimaliQuantita":
                        {
                            m_DecimaliQuantita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "ProductPage":
                        {
                            m_ProductPage = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "SupportPage":
                        {
                            m_SupportPage = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Attributi":
                        {
                            m_Attributi = (AttributiArticoloCollection)fieldValue;
                            m_Attributi.SetArticolo(this);
                            break;
                        }

                    case "Codici":
                        {
                            m_Codici = (CCodiciPerArticolo)fieldValue;
                            m_Codici.SetArticolo(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Salva l'oggetto
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                base.Save(force);
                if (m_Codici is object)
                    m_Codici.Save(force);
                if (m_Attributi is object)
                    m_Attributi.Save(force);
                
            }

            /// <summary>
            /// Restituisce true se l'oggetto é stato modificato
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                return   base.IsChanged() 
                      || (m_Attributi is object && m_Attributi.IsChanged())
                      || (m_Codici is object && m_Codici.IsChanged());

            }

            /// <summary>
            /// Imposta la proprietà changed
            /// </summary>
            /// <param name="value"></param>
            public override void SetChanged(bool value = true)
            {
                base.SetChanged(value);
                if (!value)
                {
                    if (this.m_Codici is object)
                        DBUtils.SetChanged(this.m_Codici, false);
                    if (this.m_Attributi is object)
                        DBUtils.SetChanged(this.m_Attributi, false);
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_OfficeArticoli";
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Store.Articoli;
            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_Nome = reader.Read("Nome", m_Nome);
                m_CategoriaPrincipale = reader.Read("CategoriaPrincipale", m_CategoriaPrincipale);
                m_Marca = reader.Read("Marca", m_Marca);
                m_Modello = reader.Read("Modello", m_Modello);
                m_IconURL = reader.Read("IconURL", m_IconURL);
                m_Descrizione = reader.Read("Descrizione", m_Descrizione);
                m_UnitaBase = reader.Read("UnitaBase", m_UnitaBase);
                m_DecimaliValuta = reader.Read("DecimaliValuta", m_DecimaliValuta);
                m_DecimaliQuantita = reader.Read("DecimaliQuantita", m_DecimaliQuantita);
                m_ProductPage = reader.Read("ProductPage", m_ProductPage);
                m_SupportPage = reader.Read("SupportPage", m_SupportPage);
                string tmp = reader.Read("Attributi", "");
                if (!string.IsNullOrEmpty(tmp))
                {
                    m_Attributi = (AttributiArticoloCollection)DMD.XML.Utils.Serializer.Deserialize(tmp);
                    m_Attributi.SetArticolo(this);
                }
                 

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("CategoriaPrincipale", m_CategoriaPrincipale);
                writer.Write("Marca", m_Marca);
                writer.Write("Modello", m_Modello);
                writer.Write("IconURL", m_IconURL);
                writer.Write("Descrizione", m_Descrizione);
                writer.Write("UnitaBase", m_UnitaBase);
                writer.Write("DecimaliValuta", m_DecimaliValuta);
                writer.Write("DecimaliQuantita", m_DecimaliQuantita);
                writer.Write("ProductPage", m_ProductPage);
                writer.Write("SupportPage", m_SupportPage);
                writer.Write("Attributi", DMD.XML.Utils.Serializer.Serialize(Attributi));
                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("CategoriaPrincipale", typeof(string), 255);
                c = table.Fields.Ensure("Marca", typeof(string), 255);
                c = table.Fields.Ensure("Modello", typeof(string), 255);
                c = table.Fields.Ensure("IconURL", typeof(string), 255);
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);
                c = table.Fields.Ensure("UnitaBase", typeof(string), 255);
                c = table.Fields.Ensure("DecimaliValuta", typeof(int), 1);
                c = table.Fields.Ensure("DecimaliQuantita", typeof(int), 1);
                c = table.Fields.Ensure("ProductPage", typeof(string), 255);
                c = table.Fields.Ensure("SupportPage", typeof(string), 255);
                c = table.Fields.Ensure("Attributi", typeof(string), 0);
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return m_Nome;
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Nome);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(Databases.DBObject obj)
            {
                return (obj is Articolo) && this.Equals((Articolo)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(Articolo obj)
            {
                return base.Equals(obj)
                    && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                    && DMD.Strings.EQ(this.m_Marca, obj.m_Marca)
                    && DMD.Strings.EQ(this.m_Modello, obj.m_Modello)
                    && DMD.Strings.EQ(this.m_Descrizione, obj.m_Descrizione)
                    && DMD.Strings.EQ(this.m_CategoriaPrincipale, obj.m_CategoriaPrincipale)
                    && DMD.Strings.EQ(this.m_IconURL, obj.m_IconURL)
                    && DMD.Strings.EQ(this.m_ProductPage, obj.m_ProductPage)
                    && DMD.Strings.EQ(this.m_SupportPage, obj.m_SupportPage)
                    && DMD.Strings.EQ(this.m_UnitaBase, obj.m_UnitaBase)
                    && DMD.Integers.EQ(this.m_DecimaliValuta, obj.m_DecimaliValuta)
                    && DMD.Integers.EQ(this.m_DecimaliQuantita, obj.m_DecimaliQuantita)
                    ;
                    //private AttributiArticoloCollection m_Attributi;
                    //private CCodiciPerArticolo m_Codici;
            }

            
        }
    }
}