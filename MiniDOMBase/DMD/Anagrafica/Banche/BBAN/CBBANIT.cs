

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Classe che rappresenta un codice BBAN intaliano
        /// </summary>
        /// <remarks></remarks>
        public class CBBAN_IT 
            : CBBAN
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBBAN_IT()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="codice"></param>
            public CBBAN_IT(string codice) : base(codice)
            {
            }

            /// <summary>
            /// Codice CIN
            /// </summary>
            public string CIN
            {
                get
                {
                    return DMD.Strings.Left(Codice, 1);
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 1);
                    if (string.IsNullOrEmpty(Codice))
                        Codice = DMD.Strings.NChars(27, "0");
                    Codice = value + DMD.Strings.Mid(Codice, 2);
                }
            }

            /// <summary>
            /// Codice ABI
            /// </summary>
            public string ABI
            {
                get
                {
                    return DMD.Strings.Mid(Codice, 2, 5);
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 5);
                    if (string.IsNullOrEmpty(Codice))
                        Codice = DMD.Strings.NChars(27, "0");
                    Codice = DMD.Strings.Left(Codice, 1) + value + DMD.Strings.Mid(Codice, 7);
                }
            }

            /// <summary>
            /// Codice CAB
            /// </summary>
            public string CAB
            {
                get
                {
                    return DMD.Strings.Mid(Codice, 7, 5);
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 5);
                    if (string.IsNullOrEmpty(Codice))
                        Codice = DMD.Strings.NChars(27, "0");
                    Codice = DMD.Strings.Left(Codice, 6) + value + DMD.Strings.Mid(Codice, 12);
                }
            }

            /// <summary>
            /// NumeroDiContoCorrente
            /// </summary>
            public string NumeroDiContoCorrente
            {
                get
                {
                    return DMD.Strings.Right(Codice, 12);
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 12);
                    if (string.IsNullOrEmpty(Codice))
                        Codice = DMD.Strings.NChars(27, "0");
                    Codice = DMD.Strings.Left(Codice, 6) + value + DMD.Strings.Mid(Codice, 12);
                }
            }

            /// <summary>
            /// Restituisce true se il codice é valido
            /// </summary>
            /// <returns></returns>
            public override bool IsValid()
            {
                if (DMD.Strings.Len(Codice) != 27)
                    return false;
                return true;
            }
        }
    }
}