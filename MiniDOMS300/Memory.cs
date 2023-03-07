// Imports Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Runtime.InteropServices;

namespace minidom.S300
{
    public partial class CKT_DLL
    {

        // public  Declare Sub PCopyMemory Lib "kernel32"  Alias "RtlMoveMemory"(ByRef Destination As Any, ByVal Source As Integer, ByVal Length As Integer)
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref int Destination, IntPtr Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref byte Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref PERSONINFO Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref CLOCKINGRECORD Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref NETINFO Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref RINGTIME Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(ref TIMESECT Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PCopyMemory(int Source, ref TIMESECT Destination, int Length);
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void PPCopyMemory(int Destination, int Source, int Length);
        [DllImport("kernel32", EntryPoint = "Sleep", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void Sleep(int dwMilliseconds);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseco1nds;
        }

        [DllImport("kernel32", EntryPoint = "GetLocalTime", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void GetLocalTime(ref SYSTEMTIME lpSystemTime);
    }
}