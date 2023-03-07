using System;

namespace minidom.PBX
{

    /// <summary>
    /// Classe base di un dialer
    /// </summary>
    public abstract class DialerBaseClass
    {
        /// <summary>
        /// Evento generato quando inizia la composizione di un numero
        /// </summary>
        public event BeginDialEventHandler BeginDial;


        /// <summary>
        /// Evento generato quando un numero è stato composto 
        /// </summary>
        public event EndDialEventHandler EndDial;



        /// <summary>
        /// Costruttore
        /// </summary>
        public DialerBaseClass()
        {
             
        }

       
        /// <summary>
        /// Restituisce il nome univoco del dialler
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Restituisce rue se il dialler é installato
        /// </summary>
        /// <returns></returns>
        public abstract bool IsInstalled();

        /// <summary>
        /// Compone il numero tramite il dialler
        /// </summary>
        /// <param name="number"></param>
        public abstract void Dial(string number);

        /// <summary>
        /// Genera l'evento BeginDial
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBegidDial(DialEventArgs e)
        {
            BeginDial?.Invoke(this, e);
        }

        /// <summary>
        /// Genera l'evento EndDial
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEndDial(DialEventArgs e)
        {
            BeginDial?.Invoke(this, e);
        }


        /// <summary>
        /// Aggancia
        /// </summary>
        public abstract void HangUp();

        // Private Delegate Function fH(ByVal number As String) As Integer

        // Private Class Handler
        // Public d As DialerBaseClass

        // Public Sub New(ByVal d As DialerBaseClass)
        // DMDObject.IncreaseCounter(Me)
        // Me.d = d
        // End Sub

        // Protected Overrides Sub Finalize()
        // MyBase.Finalize()
        // DMDObject.DecreaseCounter(Me)
        // End Sub
        // End Class

        // Public Sub DialAsync(ByVal number As String)
        // Dim a As fH = AddressOf Me.Dial
        // a.BeginInvoke(number, AddressOf cb, New Handler(Me))
        // End Sub

        // Private Sub cb(ByVal a As IAsyncResult)
        // Dim h As Handler = DirectCast(a.AsyncState, Handler)
        // h.d.doDialComplete(New System.EventArgs)
        // End Sub

        // Friend Sub doDialComplete(ByVal e As System.EventArgs)
        // RaiseEvent EndDial(Me, e)
        // End Sub

        /// <summary>
        /// Funzione di utilità usata per ottenere la cartella predefinita dei programmi
        /// </summary>
        /// <returns></returns>
        protected static string GetProgramFilesFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }

        /// <summary>
        /// Funzione di utilità usata per ottenere la cartella predefinita dei dati applicazione
        /// </summary>
        /// <returns></returns>
        protected static string GetRoamingFilesFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta l'oggetto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Restituisce vero se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(object obj)
        {
            return (obj is DialerBaseClass) && this.Equals((DialerBaseClass)obj);
        }



        /// <summary>
        /// Restituisce vero se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Equals(DialerBaseClass obj)
        {
            return base.Equals(obj) && 
                DMD.Strings.EQ(this.Name, obj.Name
                ;
             
        }


    }
}