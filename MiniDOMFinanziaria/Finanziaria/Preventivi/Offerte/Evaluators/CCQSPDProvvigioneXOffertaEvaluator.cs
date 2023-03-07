
namespace minidom
{
    public partial class Finanziaria
    {
        public abstract class CCQSPDProvvigioneXOffertaEvaluator
        {
            /// <summary>
            /// Valuta il valore delle estinzioni
            /// </summary>
            /// <param name="pxo"></param>
            /// <param name="offerta"></param>
            /// <param name="estinzioni"></param>
            /// <returns></returns>
            public abstract decimal Evaluate(CCQSPDProvvigioneXOfferta pxo, COffertaCQS offerta, CCollection<EstinzioneXEstintore> estinzioni);
        }
    }
}