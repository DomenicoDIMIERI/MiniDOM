using System;
using DMD;
using DMD.XML;

namespace minidom.Forms
{
    public sealed partial class Utils
    {
        private Utils()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~Utils()
        {
            DMDObject.DecreaseCounter(this);
        }

        public sealed class CFormsUtilsClass
        {
            internal CFormsUtilsClass()
            {
                DMDObject.IncreaseCounter(this);
            }

            public string ToRGBSTR(System.Drawing.Color c)
            {
                return Strings.Right("00" + DMD.Integers.Hex(c.R), 2) + Strings.Right("00" + DMD.Integers.Hex(c.G), 2) + Strings.Right("00" + DMD.Integers.Hex(c.B), 2);
            }

            public System.Drawing.Color FromRGBSTR(string str)
            {
                str = Strings.LCase(Strings.Replace(str, " ", ""));
                if (string.IsNullOrEmpty(str))
                    return System.Drawing.Color.Empty;
                switch (str ?? "")
                {
                    case "aliceblue":
                        {
                            return System.Drawing.Color.AliceBlue;
                        }

                    case "antiquewhite":
                        {
                            return System.Drawing.Color.AntiqueWhite;
                        }

                    case "aqua":
                        {
                            return System.Drawing.Color.Aqua;
                        }

                    case "aquamarine":
                        {
                            return System.Drawing.Color.Aquamarine;
                        }

                    case "azure":
                        {
                            return System.Drawing.Color.Azure;
                        }

                    case "beige":
                        {
                            return System.Drawing.Color.Beige;
                        }

                    case "bisque":
                        {
                            return System.Drawing.Color.Bisque;
                        }

                    case "black":
                        {
                            return System.Drawing.Color.Black;
                        }

                    case "blanchedalmond":
                        {
                            return System.Drawing.Color.BlanchedAlmond;
                        }

                    case "blue":
                        {
                            return System.Drawing.Color.Blue;
                        }

                    case "blueviolet":
                        {
                            return System.Drawing.Color.BlueViolet;
                        }

                    case "brown":
                        {
                            return System.Drawing.Color.Brown;
                        }

                    case "burlywood":
                        {
                            return System.Drawing.Color.BurlyWood;
                        }

                    case "cadetblue":
                        {
                            return System.Drawing.Color.CadetBlue;
                        }

                    case "chartreuse":
                        {
                            return System.Drawing.Color.Chartreuse;
                        }

                    case "chocolate":
                        {
                            return System.Drawing.Color.Chocolate;
                        }

                    case "coral":
                        {
                            return System.Drawing.Color.Coral;
                        }

                    case "cornflowerBlue":
                        {
                            return System.Drawing.Color.CornflowerBlue;
                        }

                    case "cornsilk":
                        {
                            return System.Drawing.Color.Cornsilk;
                        }

                    case "crimson":
                        {
                            return System.Drawing.Color.Crimson;
                        }

                    case "cyan":
                        {
                            return System.Drawing.Color.Cyan;
                        }

                    case "darkblue":
                        {
                            return System.Drawing.Color.DarkBlue;
                        }

                    case "darkcyan":
                        {
                            return System.Drawing.Color.DarkCyan;
                        }

                    case "darkgoldenrod":
                        {
                            return System.Drawing.Color.DarkGoldenrod;
                        }

                    case "darkgray":
                        {
                            return System.Drawing.Color.DarkGray;
                        }

                    case "darkgreen":
                        {
                            return System.Drawing.Color.DarkGreen;
                        }

                    case "darkkhaki":
                        {
                            return System.Drawing.Color.DarkKhaki;
                        }

                    case "darkmagenta":
                        {
                            return System.Drawing.Color.DarkMagenta;
                        }

                    case "darkolivegreen":
                        {
                            return System.Drawing.Color.DarkOliveGreen;
                        }

                    case "darkorange":
                        {
                            return System.Drawing.Color.DarkOrange;
                        }

                    case "darkorchid":
                        {
                            return System.Drawing.Color.DarkOrchid;
                        }

                    case "darkred":
                        {
                            return System.Drawing.Color.DarkRed;
                        }

                    case "darksalmon":
                        {
                            return System.Drawing.Color.DarkSalmon;
                        }

                    case "darkseagreen":
                        {
                            return System.Drawing.Color.DarkSeaGreen;
                        }

                    case "darkslateblue":
                        {
                            return System.Drawing.Color.DarkSlateBlue;
                        }

                    case "darkslategray":
                        {
                            return System.Drawing.Color.DarkSlateGray;
                        }

                    case "darkturquoise":
                        {
                            return System.Drawing.Color.DarkTurquoise;
                        }

                    case "darkviolet":
                        {
                            return System.Drawing.Color.DarkViolet;
                        }

                    case "deeppink":
                        {
                            return System.Drawing.Color.DeepPink;
                        }

                    case "deepskyblue":
                        {
                            return System.Drawing.Color.DeepSkyBlue;
                        }

                    case "dimgray":
                        {
                            return System.Drawing.Color.DimGray;
                        }

                    case "dodgerblue":
                        {
                            return System.Drawing.Color.DodgerBlue;
                        }

                    case "empty":
                        {
                            return System.Drawing.Color.Empty;
                        }

                    case "firebrick":
                        {
                            return System.Drawing.Color.Firebrick;
                        }

                    case "floralwhite":
                        {
                            return System.Drawing.Color.FloralWhite;
                        }

                    case "forestgreen":
                        {
                            return System.Drawing.Color.ForestGreen;
                        }

                    case "fuchsia":
                        {
                            return System.Drawing.Color.Fuchsia;
                        }

                    case "gainsboro":
                        {
                            return System.Drawing.Color.Gainsboro;
                        }

                    case "ghostwhite":
                        {
                            return System.Drawing.Color.GhostWhite;
                        }

                    case "gold":
                        {
                            return System.Drawing.Color.Gold;
                        }

                    case "goldenrod":
                        {
                            return System.Drawing.Color.Goldenrod;
                        }

                    case "gray":
                        {
                            return System.Drawing.Color.Gray;
                        }

                    case "green":
                        {
                            return System.Drawing.Color.Green;
                        }

                    case "greenyellow":
                        {
                            return System.Drawing.Color.GreenYellow;
                        }

                    case "honeydew":
                        {
                            return System.Drawing.Color.Honeydew;
                        }

                    case "hotpink":
                        {
                            return System.Drawing.Color.HotPink;
                        }

                    case "indianred":
                        {
                            return System.Drawing.Color.IndianRed;
                        }

                    case "indigo":
                        {
                            return System.Drawing.Color.Indigo;
                        }

                    case "ivory":
                        {
                            return System.Drawing.Color.Ivory;
                        }

                    case "khaki":
                        {
                            return System.Drawing.Color.Khaki;
                        }

                    case "lavender":
                        {
                            return System.Drawing.Color.Lavender;
                        }

                    case "lavenderblush":
                        {
                            return System.Drawing.Color.LavenderBlush;
                        }

                    case "lawngreen":
                        {
                            return System.Drawing.Color.LawnGreen;
                        }

                    case "lemonchiffon":
                        {
                            return System.Drawing.Color.LemonChiffon;
                        }

                    case "lightblue":
                        {
                            return System.Drawing.Color.LightBlue;
                        }

                    case "lightcoral":
                        {
                            return System.Drawing.Color.LightCoral;
                        }

                    case "lightcyan":
                        {
                            return System.Drawing.Color.LightCyan;
                        }

                    case "lightgoldenrodyellow":
                        {
                            return System.Drawing.Color.LightGoldenrodYellow;
                        }

                    case "lightgray":
                        {
                            return System.Drawing.Color.LightGray;
                        }

                    case "lightgreen":
                        {
                            return System.Drawing.Color.LightGreen;
                        }

                    case "lightpink":
                        {
                            return System.Drawing.Color.LightPink;
                        }

                    case "lightsalmon":
                        {
                            return System.Drawing.Color.LightSalmon;
                        }

                    case "lightseagreen":
                        {
                            return System.Drawing.Color.LightSeaGreen;
                        }

                    case "lightskyblue":
                        {
                            return System.Drawing.Color.LightSkyBlue;
                        }

                    case "lightslategray":
                        {
                            return System.Drawing.Color.LightSlateGray;
                        }

                    case "lightsteelblue":
                        {
                            return System.Drawing.Color.LightSteelBlue;
                        }

                    case "lightyellow":
                        {
                            return System.Drawing.Color.LightYellow;
                        }

                    case "lime":
                        {
                            return System.Drawing.Color.Lime;
                        }

                    case "limegreen":
                        {
                            return System.Drawing.Color.LimeGreen;
                        }

                    case "linen":
                        {
                            return System.Drawing.Color.Linen;
                        }

                    case "magenta":
                        {
                            return System.Drawing.Color.Magenta;
                        }

                    case "maroon":
                        {
                            return System.Drawing.Color.Maroon;
                        }

                    case "mediumaquamarine":
                        {
                            return System.Drawing.Color.MediumAquamarine;
                        }

                    case "mediumblue":
                        {
                            return System.Drawing.Color.MediumBlue;
                        }

                    case "mediumorchid":
                        {
                            return System.Drawing.Color.MediumOrchid;
                        }

                    case "mediumpurple":
                        {
                            return System.Drawing.Color.MediumPurple;
                        }

                    case "mediumseagreen":
                        {
                            return System.Drawing.Color.MediumSeaGreen;
                        }

                    case "mediumslateblue":
                        {
                            return System.Drawing.Color.MediumSlateBlue;
                        }

                    case "mediumspringgreen":
                        {
                            return System.Drawing.Color.MediumSpringGreen;
                        }

                    case "mediumturquoise":
                        {
                            return System.Drawing.Color.MediumTurquoise;
                        }

                    case "mediumvioletRed":
                        {
                            return System.Drawing.Color.MediumVioletRed;
                        }

                    case "midnightblue":
                        {
                            return System.Drawing.Color.MidnightBlue;
                        }

                    case "mintcream":
                        {
                            return System.Drawing.Color.MintCream;
                        }

                    case "mistyrose":
                        {
                            return System.Drawing.Color.MistyRose;
                        }

                    case "moccasin":
                        {
                            return System.Drawing.Color.Moccasin;
                        }

                    case "navajowhite":
                        {
                            return System.Drawing.Color.NavajoWhite;
                        }

                    case "navy":
                        {
                            return System.Drawing.Color.Navy;
                        }

                    case "oldlace":
                        {
                            return System.Drawing.Color.OldLace;
                        }

                    case "olive":
                        {
                            return System.Drawing.Color.Olive;
                        }

                    case "olivedrab":
                        {
                            return System.Drawing.Color.OliveDrab;
                        }

                    case "orange":
                        {
                            return System.Drawing.Color.Orange;
                        }

                    case "orangered":
                        {
                            return System.Drawing.Color.OrangeRed;
                        }

                    case "orchid":
                        {
                            return System.Drawing.Color.Orchid;
                        }

                    case "palegoldenrod":
                        {
                            return System.Drawing.Color.PaleGoldenrod;
                        }

                    case "palegreen":
                        {
                            return System.Drawing.Color.PaleGreen;
                        }

                    case "paleturquoise":
                        {
                            return System.Drawing.Color.PaleTurquoise;
                        }

                    case "palevioletred":
                        {
                            return System.Drawing.Color.PaleVioletRed;
                        }

                    case "papayawhip":
                        {
                            return System.Drawing.Color.PapayaWhip;
                        }

                    case "peachpuff":
                        {
                            return System.Drawing.Color.PeachPuff;
                        }

                    case "peru":
                        {
                            return System.Drawing.Color.Peru;
                        }

                    case "pink":
                        {
                            return System.Drawing.Color.Pink;
                        }

                    case "plum":
                        {
                            return System.Drawing.Color.Plum;
                        }

                    case "powderblue":
                        {
                            return System.Drawing.Color.PowderBlue;
                        }

                    case "purple":
                        {
                            return System.Drawing.Color.Purple;
                        }

                    case "red":
                        {
                            return System.Drawing.Color.Red;
                        }

                    case "rosybrown":
                        {
                            return System.Drawing.Color.RosyBrown;
                        }

                    case "royalblue":
                        {
                            return System.Drawing.Color.RoyalBlue;
                        }

                    case "saddlebrown":
                        {
                            return System.Drawing.Color.SaddleBrown;
                        }

                    case "salmon":
                        {
                            return System.Drawing.Color.Salmon;
                        }

                    case "sandybrown":
                        {
                            return System.Drawing.Color.SandyBrown;
                        }

                    case "seagreen":
                        {
                            return System.Drawing.Color.SeaGreen;
                        }

                    case "seashell":
                        {
                            return System.Drawing.Color.SeaShell;
                        }

                    case "sienna":
                        {
                            return System.Drawing.Color.Sienna;
                        }

                    case "silver":
                        {
                            return System.Drawing.Color.Silver;
                        }

                    case "skyblue":
                        {
                            return System.Drawing.Color.SkyBlue;
                        }

                    case "slateblue":
                        {
                            return System.Drawing.Color.SlateBlue;
                        }

                    case "slategray":
                        {
                            return System.Drawing.Color.SlateGray;
                        }

                    case "snow":
                        {
                            return System.Drawing.Color.Snow;
                        }

                    case "springgreen":
                        {
                            return System.Drawing.Color.SpringGreen;
                        }

                    case "steelblue":
                        {
                            return System.Drawing.Color.SteelBlue;
                        }

                    case "tan":
                        {
                            return System.Drawing.Color.Tan;
                        }

                    case "teal":
                        {
                            return System.Drawing.Color.Teal;
                        }

                    case "thistle":
                        {
                            return System.Drawing.Color.Thistle;
                        }

                    case "tomato":
                        {
                            return System.Drawing.Color.Tomato;
                        }

                    case "transparent":
                        {
                            return System.Drawing.Color.Transparent;
                        }

                    case "turquoise":
                        {
                            return System.Drawing.Color.Turquoise;
                        }

                    case "violet":
                        {
                            return System.Drawing.Color.Violet;
                        }

                    case "wheat":
                        {
                            return System.Drawing.Color.Wheat;
                        }

                    case "white":
                        {
                            return System.Drawing.Color.White;
                        }

                    case "whitesmoke":
                        {
                            return System.Drawing.Color.WhiteSmoke;
                        }

                    case "yellow":
                        {
                            return System.Drawing.Color.Yellow;
                        }

                    case "yellowgreen":
                        {
                            return System.Drawing.Color.YellowGreen;
                        }

                    default:
                        {
                            try
                            {
                                int blue = DMD.Integers.ValueOf("&H0" + Strings.Right(str, 2));
                                str = Strings.Left(str, Strings.Len(str) - 2);
                                int green = DMD.Integers.ValueOf("&H0" + Strings.Right(str, 2));
                                str = Strings.Left(str, Strings.Len(str) - 2);
                                int red = DMD.Integers.ValueOf("&H0" + Strings.Right(str, 2));
                                str = Strings.Left(str, Strings.Len(str) - 2);
                                return System.Drawing.Color.FromArgb(255, red, green, blue);
                            }
                            catch (Exception ex)
                            {
                                throw new ArgumentException("Colore non valido: " + str);
                            }

                            break;
                        }
                }
            }

            // Public Sub WriteCBOOps(ByVal writer As HTMLWriter, ByVal cboName As String, ByVal value As String, ByVal width As String, ByVal height As String)
            // CreateCBOOps(writer, cboName, value, width, height)
            // End Sub

            // Public Sub CreateCBOOps(ByVal writer As HTMLWriter, ByVal cboName As String, ByVal value As String, ByVal width As String, ByVal height As String)
            // Dim cbo As New COperatorsComboBox
            // cbo.Name = cboName
            // cbo.Width = width
            // cbo.Height = height
            // cbo.SelectedValue = value
            // cbo.CreateHTML(writer)
            // End Sub

            public string GetValidControlName(string value)
            {
                string ch;
                int i;
                var ret = new System.Text.StringBuilder();

                value = Strings.Trim(value);
                ch = DMD.Strings.Left(value, 1);
                if (DMD.Strings.Compare(Strings.UCase(ch), "A", false) < 0 || DMD.Strings.Compare(Strings.UCase(ch), "Z", false) > 0)
                {
                    ret.Append("_");
                }
                else
                {
                    ret.Append(ch);
                }

                var loopTo = Strings.Len(value);
                for (i = 2; i <= loopTo; i++)
                {
                    ch = Strings.Mid(value, i, 1);
                    if (
                          (DMD.Strings.Compare(Strings.UCase(ch), "A", false) < 0 || DMD.Strings.Compare(Strings.UCase(ch), "Z", false) > 0) 
                        & (DMD.Strings.Compare(ch, "0", false) < 0 || DMD.Strings.Compare(ch, "9", false) > 0)
                        )
                    {
                        ret.Append("_");
                    }
                    else
                    {
                        ret.Append(ch);
                    }
                }

                return ret.ToString();
            }

            public System.Drawing.Color GetRandomColor()
            {
                byte r, g, b;
                r = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
                g = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
                b = (byte)(DMD.RunTime.GetRandomDouble(0, 255));
                return System.Drawing.Color.FromArgb(255, r, g, b);
            }

            public string GenerateRandomColor()
            {
                string r, g, b;
                r = Strings.Right("00" + DMD.Integers.Hex((int)(DMD.RunTime.GetRandomDouble(0, 255))), 2);
                g = Strings.Right("00" + DMD.Integers.Hex((int)(DMD.RunTime.GetRandomDouble(0, 255))), 2);
                b = Strings.Right("00" + DMD.Integers.Hex((int)(DMD.RunTime.GetRandomDouble(0, 255))), 2);
                return r + g + b;
            }

            public string GetTextAlignStyle(TextAlignEnum value)
            {
                switch (value)
                {
                    case TextAlignEnum.TEXTALIGN_DEFAULT:
                        {
                            return "";
                        }

                    case TextAlignEnum.TEXTALIGN_LEFT:
                        {
                            return "text-align:left;";
                        }

                    case TextAlignEnum.TEXTALIGN_RIGHT:
                        {
                            return "text-align:right;";
                        }

                    case TextAlignEnum.TEXTALIGN_CENTER:
                        {
                            return "text-align:center;";
                        }

                    case TextAlignEnum.TEXTALIGN_JUSTIFY:
                        {
                            return "text-align:justify;";
                        }
                }

                return DMD.Strings.vbNullString;
            }

            ~CFormsUtilsClass()
            {
                DMDObject.DecreaseCounter(this);
            }
        }

        private static CFormsUtilsClass m_FormUtils = null;

        public static CFormsUtilsClass FormsUtils
        {
            get
            {
                if (m_FormUtils is null)
                    m_FormUtils = new CFormsUtilsClass();
                return m_FormUtils;
            }
        }
    }
}