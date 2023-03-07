using System;
using System.Data;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class AnaXFonteInfo 
        : IComparable, IDMDXMLSerializable
    {
        public string TipoFonte;
        public int Conteggio;
        [NonSerialized] private object m_Fonte;
        public int IDFonte;
        public string NomeFonte;

        public AnaXFonteInfo(string tipo, object fonte, int conteggio)
        {
            DMDObject.IncreaseCounter(this);
            TipoFonte = tipo;
            Fonte = fonte;
            Conteggio = conteggio;
        }

        public int CompareTo(object obj)
        {
            AnaXFonteInfo tmp = (AnaXFonteInfo)obj;
            return DMD.Arrays.Compare(tmp.Conteggio, Conteggio);
        }

        public object Fonte
        {
            get
            {
                if (m_Fonte is null)
                    m_Fonte = Anagrafica.Fonti.GetItemById(TipoFonte, TipoFonte, IDFonte);
                return m_Fonte;
            }

            set
            {
                if (ReferenceEquals(Fonte, value))
                    return;
                m_Fonte = value;
                IDFonte = Databases.GetID((Databases.IDBObjectBase)value);
                if (value is object)
                {
                    NomeFonte = ((IFonte)value).Nome;
                }
                else
                {
                    NomeFonte = "";
                }
            }
        }

        protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "TipoFonte":
                    {
                        TipoFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "NomeFonte":
                    {
                        NomeFonte = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IDFonte":
                    {
                        IDFonte = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }

                case "Conteggio":
                    {
                        Conteggio = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                        break;
                    }
            }
        }

        protected virtual void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("TipoFonte", TipoFonte);
            writer.WriteAttribute("IDFonte", IDFonte);
            writer.WriteAttribute("NomeFonte", NomeFonte);
            writer.WriteAttribute("Conteggio", Conteggio);
        }

        ~AnaXFonteInfo()
        {
            DMDObject.DecreaseCounter(this);
        }

        void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue) { this.SetFieldInternal(fieldName, fieldValue);  }
        void IDMDXMLSerializable.XMLSerialize(XMLWriter writer) { this.XMLSerialize(writer);  }
    }

    public class AnaXFonteHandler 
        : CQSPDBaseStatsHandler
    {
        public AnaXFonteHandler()
        {
        }

        public override Databases.DBObjectCursorBase CreateCursor()
        {
            return new Anagrafica.CPersonaCursor();
        }

        public string GetStats()
        {
            string dbSQL;
            if (!Module.UserCanDoAction("list"))
                throw new PermissionDeniedException(Module, "list");

            using (var cursor = new Anagrafica.CPersonaFisicaCursor())
            {
                cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID;
                dbSQL = "SELECT [TipoFonte], [IDFonte], Count(*) As [Conteggio] FROM (" + cursor.GetSQL() + ") GROUP BY [TipoFonte], [IDFonte]";
            }

            var col = new CCollection<AnaXFonteInfo>();
            using (var dbRis = Databases.APPConn.ExecuteReader(dbSQL))
            {
                while (dbRis.Read())
                {
                    string tipoFonte = Sistema.Formats.ToString(dbRis["TipoFonte"]);
                    int idFonte = Sistema.Formats.ToInteger(dbRis["IDFonte"]);
                    var fonte = Anagrafica.Fonti.GetItemById(tipoFonte, tipoFonte, idFonte);
                    int conteggio = Sistema.Formats.ToInteger(dbRis["Conteggio"]);
                    var info = new AnaXFonteInfo(tipoFonte, fonte, conteggio);
                    col.Add(info);
                }

            }

            return DMD.XML.Utils.Serializer.Serialize(col);             
        }
    }
}