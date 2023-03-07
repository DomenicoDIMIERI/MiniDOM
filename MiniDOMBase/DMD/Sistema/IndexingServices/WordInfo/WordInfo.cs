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
using minidom.internals;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Info su una parola indicizzabile
        /// </summary>
        [Serializable]
        public class WordInfo
            : IComparable, ICloneable, IComparable<WordInfo>
        {

            /// <summary>
            /// Parola
            /// </summary>
            public string Word;

            /// <summary>
            /// Restituisce la prima posizione in cui la parola è stata trovata nella frase
            /// </summary>
            public int Position;

            /// <summary>
            /// Se true la parola viene cercata nel db con il metodo "like" e non con il metodo "eq"
            /// </summary>
            public bool IsLike = false;

            /// <summary>
            /// Se true la parola non è indicizzata
            /// </summary>
            public bool IsNew = false;

            /// <summary>
            /// Frequenza di utilizzo
            /// </summary>
            public int? Frequenza;

            /// <summary>
            /// Costruttore
            /// </summary>
            public WordInfo()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="word"></param>
            public WordInfo(string word)
            {
                this.Word = word;
            }

            public int CompareTo(WordInfo other)
            {
                int a1 = (IsLike)? 1 : 0;
                int a2 = (other.IsLike)? 1 : 0;
                int ret = a1 - a2;
                if (ret == 0) ret = DMD.Integers.Compare(this.Frequenza , other.Frequenza);
                if (ret == 0) ret = DMD.Strings.Len(other.Word) - DMD.Strings.Len(Word);
                return ret;
            }

            int IComparable.CompareTo(object o)
            {
                return this.CompareTo((WordInfo)o);
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public WordInfo Clone()
            {
                return (WordInfo) this.MemberwiseClone();
            }

            object ICloneable.Clone()
            {
                return this.Clone();
            }
        }


    }
}