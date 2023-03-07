
namespace minidom
{
    public partial class Finanziaria
    {
        public class CQSPDVisualizzaPratica 
            : Sistema.AzioneEseguibile
        {
            public override string Description
            {
                get
                {
                    return "Visualizza la pratica";
                }
            }

            protected override string ExecuteInternal(Sistema.Notifica notifica, string parameters)
            {
                return "";
            }

            public override string Name
            {
                get
                {
                    return "CQSPDVISPRAT";
                }
            }

            public override object Render(Sistema.Notifica notifica, object context)
            {
                return null;
            }
        }
    }
}