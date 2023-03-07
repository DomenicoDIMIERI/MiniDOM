using System;

namespace minidom.S300
{

    /// <summary>
    /// Racchiude le informazioni su un'impronta digitale di un utente
    /// </summary>
    public class S300FingerPrint
    {
        internal S300PersonInfo m_User;
        internal int m_FPID;
        internal byte[] m_Data;

        public S300FingerPrint()
        {
            m_User = null;
            m_FPID = 0;
            m_Data = Array.Empty<byte>();
        }

        /// <summary>
        /// Carica i dati dell'impronta dal file
        /// </summary>
        /// <param name="fileName"></param>
        public S300FingerPrint(string fileName) : this()
        {
            LoadFromFile(fileName);
        }

        /// <summary>
        /// Carica i dati dell'impronta dallo stream
        /// </summary>
        /// <param name="stream"></param>
        public S300FingerPrint(System.IO.Stream stream) : this()
        {
            LoadFromStream(stream);
        }

        /// <summary>
        /// Restituisce l'utente a cui appartiene questa impronta
        /// </summary>
        /// <returns></returns>
        public S300PersonInfo User
        {
            get
            {
                return m_User;
            }
        }

        /// <summary>
        /// Restituisce l'ID dell'impronta
        /// </summary>
        /// <returns></returns>
        public int FPID
        {
            get
            {
                return m_FPID;
            }
        }

        /// <summary>
        /// Restituisce un array contenente i dati dell'impronta
        /// </summary>
        /// <returns></returns>
        public byte[] GetDataAsArray()
        {
            return (byte[])m_Data.Clone();
        }

        /// <summary>
        /// Imposta i dati dell'imprtona
        /// </summary>
        /// <param name="data"></param>
        public void SetDataAsArray(byte[] data)
        {
            if (data is null)
                data = Array.Empty<byte>();
            m_Data = (byte[])data.Clone();
        }

        /// <summary>
        /// Salva i dati relativi all'impronta su un file
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveToFile(string fileName)
        {
            var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            SaveToStream(fs);
            fs.Dispose();
        }

        /// <summary>
        /// Carica i dati relativi all'impronta da un file
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            LoadFromStream(fs);
            fs.Dispose();
        }

        /// <summary>
        /// Salve i dati dell'impronta nello stream specificato
        /// </summary>
        /// <param name="stream"></param>
        public void SaveToStream(System.IO.Stream stream)
        {
            stream.Write(m_Data, 0, m_Data.Length);
        }

        /// <summary>
        /// Carica i dati dell'impronta dallo stream specificato
        /// </summary>
        /// <param name="stream"></param>
        public void LoadFromStream(System.IO.Stream stream)
        {
            int len = (int)(stream.Length - stream.Position);
            if (len == 0)
            {
                m_Data = Array.Empty<byte>();
            }
            else
            {
                m_Data = new byte[len];
                stream.Read(m_Data, 0, len);
            }
        }
    }
}