using System;
using DMD;
using DMD.XML;

namespace minidom
{
    public partial class Sistema
    {
        //public class CIntervalloData : IDMDXMLSerializable, ISupportInitializeFrom
        //{
        //    public string Tipo = "";
        //    public DateTime? Inizio = default;
        //    public DateTime? Fine = default;

        //    public CIntervalloData()
        //    {
        //        DMDObject.IncreaseCounter(this);
        //    }

        //    public CIntervalloData(string tipo, DateTime? fromDate, DateTime? toDate) : this()
        //    {
        //        Tipo = DMD.Strings.Trim(tipo);
        //        Inizio = fromDate;
        //        Fine = toDate;
        //    }

        //    void IDMDXMLSerializable.XMLSerialize(XMLWriter writer)
        //    {
        //        this.XMLSerialize(writer);
        //    }

        //    void IDMDXMLSerializable.SetFieldInternal(string fieldName, object fieldValue)
        //    {
        //        this.SetFieldInternal(fieldName, fieldValue);
        //    }

        //    public virtual bool IsSet()
        //    {
        //        return Types.IsNull(Inizio) == false | Types.IsNull(Fine) == false | !string.IsNullOrEmpty(Tipo);
        //    }

        //    protected virtual void XMLSerialize(XMLWriter writer)
        //    {
        //        writer.WriteAttribute("m_Tipo", Tipo);
        //        writer.WriteAttribute("m_Inizio", Inizio);
        //        writer.WriteAttribute("m_Fine", Fine);
        //    }

        //    protected virtual void SetFieldInternal(string fieldName, object fieldValue)
        //    {
        //        switch (fieldName ?? "")
        //        {
        //            case "m_Tipo":
        //                {
        //                    Tipo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
        //                    break;
        //                }

        //            case "m_Inizio":
        //                {
        //                    Inizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
        //                    break;
        //                }

        //            case "m_Fine":
        //                {
        //                    Fine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
        //                    break;
        //                }
        //        }
        //    }

        //    public virtual void InitializeFrom(object value)
        //    {
        //        {
        //            var withBlock = (CIntervalloData)value;
        //            Fine = withBlock.Fine;
        //            Inizio = withBlock.Inizio;
        //            Tipo = withBlock.Tipo;
        //        }
        //    }

        //    public virtual void CopyFrom(object value) => InitializeFrom(value);

        //    public virtual void Clear()
        //    {
        //        Fine = default;
        //        Inizio = default;
        //        Tipo = "";
        //    }

        //    ~CIntervalloData()
        //    {
        //        DMDObject.DecreaseCounter(this);
        //    }
        //}
    }
}