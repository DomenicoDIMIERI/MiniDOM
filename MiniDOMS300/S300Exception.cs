using System;
using DMD;

namespace minidom.S300
{
    public enum S300ExceptionCodes : int
    {
        // Ok = 1
        CKT_ERROR_INVPARAM = -1,
        CKT_ERROR_NETDAEMONREADY = -1,
        CKT_ERROR_CHECKSUMERR = -2,
        CKT_ERROR_MEMORYFULL = -1,
        CKT_ERROR_INVFILENAME = -3,
        CKT_ERROR_FILECANNOTOPEN = -4,
        CKT_ERROR_FILECONTENTBAD = -5,
        CKT_ERROR_FILECANNOTCREATED = -2,
        CKT_ERROR_NOTHISPERSON = -1
    }

    /// <summary>
    /// Classe che rappresenta una eccezione della libreria
    /// </summary>
    public class S300Exception : Exception
    {
        private S300ExceptionCodes m_Code;

        public S300Exception()
        {
        }

        public S300Exception(S300ExceptionCodes code) : base(DMD.Strings.CStr(GetCodeMsg(code)))
        {
            m_Code = code;
        }

        public S300Exception(S300ExceptionCodes code, string message) : base(message)
        {
            m_Code = code;
        }



        /// <summary>
        /// Restituisce il codice dell'eccezione
        /// </summary>
        /// <returns></returns>
        public S300ExceptionCodes Code
        {
            get
            {
                return m_Code;
            }
        }

        private static object GetCodeMsg(S300ExceptionCodes code)
        {
            switch (code)
            {
                case S300ExceptionCodes.CKT_ERROR_CHECKSUMERR:
                    {
                        return "Errore di comunicazione";
                    }

                case var @case when @case == S300ExceptionCodes.CKT_ERROR_FILECANNOTCREATED:
                    {
                        return "Impossibile creare il file";
                    }

                case S300ExceptionCodes.CKT_ERROR_FILECANNOTOPEN:
                    {
                        return "Impossibile aprire il file";
                    }

                case S300ExceptionCodes.CKT_ERROR_FILECONTENTBAD:
                    {
                        return "Contenuto del file non valido";
                    }

                case S300ExceptionCodes.CKT_ERROR_INVFILENAME:
                    {
                        return "Nome del file non valido";
                    }

                case S300ExceptionCodes.CKT_ERROR_INVPARAM:
                    {
                        return "Valore del parametro non valido";
                    }

                case var case1 when case1 == S300ExceptionCodes.CKT_ERROR_MEMORYFULL:
                    {
                        return "Memoria piena";
                    }

                case var case2 when case2 == S300ExceptionCodes.CKT_ERROR_NETDAEMONREADY:
                    {
                        return "Demone di rete non pronto";
                    }

                case var case3 when case3 == S300ExceptionCodes.CKT_ERROR_NOTHISPERSON:
                    {
                        return "Utente inesistente";
                    }

                default:
                    {
                        return "Sconosciuto";
                    }
            }
        }
    }
}