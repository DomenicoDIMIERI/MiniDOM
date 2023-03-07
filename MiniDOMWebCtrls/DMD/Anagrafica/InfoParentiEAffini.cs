using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom.Forms
{
    public class InfoParentiEAffini 
        : IDMDXMLSerializable
    {
        public Anagrafica.CRelazioneParentale Relazione;
        public Anagrafica.CContatto Contatto;
        public string Note;
        public string IconURL1;
        public string IconURL2;

        public InfoParentiEAffini()
        {
            DMDObject.IncreaseCounter(this);
        }

        public void SetFieldInternal(string fieldName, object fieldValue)
        {
            switch (fieldName ?? "")
            {
                case "Relazione":
                    {
                        Relazione = (Anagrafica.CRelazioneParentale)fieldValue;
                        break;
                    }

                case "Contatto":
                    {
                        Contatto = (Anagrafica.CContatto)fieldValue;
                        break;
                    }

                case "Note":
                    {
                        Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IconURL1":
                    {
                        IconURL1 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }

                case "IconURL2":
                    {
                        IconURL2 = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                        break;
                    }
            }
        }

        public void XMLSerialize(XMLWriter writer)
        {
            writer.WriteAttribute("IconURL1", IconURL1);
            writer.WriteAttribute("IconURL2", IconURL2);
            writer.WriteTag("Relazione", Relazione);
            writer.WriteTag("Contatto", Contatto);
            writer.WriteTag("Note", Note);
        }

        ~InfoParentiEAffini()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}