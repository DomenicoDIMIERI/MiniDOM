using System.IO;
using DMD.XML;

namespace minidom.PBX
{

    /// <summary>
    /// Dialer tramite l'applicazione desktop ExpressTalk
    /// </summary>
    public class ExpressTalkDialer 
        : DialerBaseClass
    {
        private string m_Path;

        /// <summary>
        /// Costruttore
        /// </summary>
        public ExpressTalkDialer()
        {
            m_Path = GetTalkerPath();
        }

        /// <summary>
        /// Prepara il numero
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string PrepareNumber(string number)
        {
            return DMD.Strings.Trim(number);
        }

        /// <summary>
        /// Compone il numero
        /// </summary>
        /// <param name="number"></param>
        public override void Dial(string number)
        {
            if (!IsInstalled())
                return;
            DMD.IOUtils.Shell(m_Path + " -dial " + PrepareNumber(number));
        }

        /// <summary>
        /// Restituisce true se il dialer è installato
        /// </summary>
        /// <returns></returns>
        public override bool IsInstalled()
        {
            return !string.IsNullOrEmpty(m_Path);
        }

        /// <summary>
        /// Restituisce il percorso dell'applicazione
        /// </summary>
        /// <returns></returns>
        private static string GetTalkerPath()
        {
            string p = Path.Combine(GetRoamingFilesFolder(), @"NCH Software\Program Files\Talk\talk.exe");
            if (File.Exists(p))
                return p;
            return DMD.Strings.vbNullString;
        }

        /// <summary>
        /// Restituisce il nome dell'applicazione
        /// </summary>
        public override string Name
        {
            get
            {
                return "NCH Express Talk VoIP Phone";
            }
        }

        /// <summary>
        /// Aggancia
        /// </summary>
        public override void HangUp()
        {
            if (!IsInstalled())
                return;
            DMD.IOUtils.Shell(m_Path + " -hangup");
        }

        /// <summary>
        /// Restituisce true se i due oggetti sono uguali
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ExpressTalkDialer))
                return false;
            return base.Equals(obj) && (((ExpressTalkDialer)obj).m_Path ?? "") == (m_Path ?? "");
        }

        /// <summary>
        /// Restituisce il codice hash dell'oggetto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DMD.HashCalculator.Calculate(this.m_Path, this.Name);
        }


    }
}


// ---------------

// NCH Software Home	
// Home | Support | Products
// Express Talk Software Integration API
// NOTE: This information Is intended For programmers only.
// We have attempted To make controlling Express Talk from within your programs To be As simple As possible And available from many languages, the command line And even telnet If required.

// It Is controlled by running the talk.exe with arguments. Express Talk will automatically detect if it Is already running And process the arguments correctly - you will Not end up with multiple instances of Express Talk.

// For testing purposes you can execute Express Talk commands from the command line. From most programs you would control trx using the WinExec command And specify the correct path To the exe Like this In C++...

// WinExec(
// "\"c: \\Program Files\\NCH Software\\Express Talk\\talk.exe" -hangup", 
// SW_SHOWNORMAL
// );
// Because there Is a space In the path name it Is important To include it between "".

// Back to top
// Locating talk.exe
// After you have run the setup file talk.exe will be located In

// C:\Program Files\NCH Software\Express Talk\talk.exe

// Back to top
// Commands
// -dial [number]
// This will dial the SIP number you enter In the "[number]" parameter.

// Example:

// "C:\Program Files\NCH Software\Express Talk\talk.exe" -dial "17000000@sipphone.com"
// -hangup
// This hangs-up the current Call

// -answer
// Answer an incoming phone Call.

// -hold
// Puts the current Call On hold.

// -offhold
// Takes the On-hold Call off hold. If there are multiple lines On hold, calling this multiple times will cycle through all On-hold calls.

// -dtmf [key]
// This sends a DTMF tone down the line during a phone Call. For the "[key]" parameter, you can use any numbers from 0 To 9 inclusive, As well As * And #.

// -hide
// Hides the window Of Express Talk.

// -show
// Reveals the Express Talk Window

// -logon
// This starts Express Talk (If it Is Not open already) And minimizes the program To the system tray.

// -exit
// This closes Express Talk (complete Exit).

// -recordstart
// Starts recording the current Call.

// -recordstop
// Stops the current recording.

// To open an existing Express Talk instance at any time simply run the "talk.exe" executable file (Or use the -show argument from above).

// Example:

// "C:\Program Files\NCH Software\Express Talk\talk.exe"
// Back to top
// Problems
// If you have any problems With the above you can contact us Using the form at Technical Support Questions And Contacts.

// We also accept suggestions For New features at www.nch.com.au/suggestions.

// Back to top
// More Information...
// For more information see:

// Express Talk Home Page
// Frequently Asked Questions
// Technical Support Questions And Contacts 

// Back to top
// Useful links
// Express Talk home page
// Download Express Talk
// Screenshots
// Questions(FAQs)
// Technical Support
// Top | Uninstall | Privacy | Legal & EULA | Contact Us | SiteMap | Home
// © NCH Software
