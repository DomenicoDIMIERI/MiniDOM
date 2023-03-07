using System;
using DMD.XML;

namespace minidom.PBX
{
    public class DialTPInterpreter
    {
        public string Action;
        public string Protocol;
        public string Number;
        public string DialPrefix;
        public string Options;
        public string ID;

        /// <summary>
        /// Costruttore
        /// </summary>
        public DialTPInterpreter()
        {
             
        }

        public bool Parse(string argument)
        {
            const string dtp = "dialtp:";
            if ((DMD.Strings.Left(DMD.Strings.LCase(argument), DMD.Strings.Len(dtp)) ?? "") == dtp)
                argument = DMD.Strings.Trim(DMD.Strings.Mid(argument, DMD.Strings.Len(dtp) + 1));
            argument = Uri.UnescapeDataString(argument); // Replace(argument, "%20", " ")

            // For Each argument As String In args
            if (DMD.Strings.InStr(argument, " ") > 0)
            {
                var subs = DMD.Strings.Split(argument, " ");
                foreach (string a in subs)
                    ProcessArgument(a);
                // Else
                // ProcessArgument(argument, o)
            }
            // Next

            return true;
        }

        private void ProcessArgument(string arguemnt)
        {
            int i;
            string n, v;
            i = DMD.Strings.InStr(arguemnt, "=");
            if (i > 0)
            {
                n = DMD.Strings.Trim(DMD.Strings.Left(arguemnt, i - 1));
                v = DMD.Strings.Trim(DMD.Strings.Mid(arguemnt, i + 1));
            }
            else
            {
                n = DMD.Strings.Trim(arguemnt);
                v = "";
            }

            switch (DMD.Strings.LCase(n) ?? "")
            {
                case "action":
                    {
                        Action = v;
                        break;
                    }

                case "protocol":
                    {
                        Protocol = v;
                        break;
                    }

                case "number":
                    {
                        Number = v;
                        break;
                    }

                case "dialprefix":
                    {
                        DialPrefix = v;
                        break;
                    }

                case "id":
                    {
                        ID = v;
                        break;
                    }

                case "options":
                    {
                        Options = v;
                        break;
                    }

                default:
                    {
                        Sistema.ApplicationContext.Log("DialTPInterpreter: Argomento non riconosciuto: " + arguemnt + " (ignoro)");
                        break;
                    }
            }
        }

        ~DialTPInterpreter()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}