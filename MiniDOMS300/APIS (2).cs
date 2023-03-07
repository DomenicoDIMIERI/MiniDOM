// Imports Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Runtime.InteropServices;

namespace minidom.S300
{
    public partial class CKT_DLL
    {

        // Consts

        public const short CKT_ERROR_INVPARAM = -1;
        public const short CKT_ERROR_NETDAEMONREADY = -1;
        public const short CKT_ERROR_CHECKSUMERR = -2;
        public const short CKT_ERROR_MEMORYFULL = -1;
        public const short CKT_ERROR_INVFILENAME = -3;
        public const short CKT_ERROR_FILECANNOTOPEN = -4;
        public const short CKT_ERROR_FILECONTENTBAD = -5;
        public const short CKT_ERROR_FILECANNOTCREATED = -2;
        public const short CKT_ERROR_NOTHISPERSON = -1;
        public const short CKT_RESULT_OK = 1;
        public const short CKT_RESULT_ADDOK = 1;
        public const short CKT_RESULT_HASMORECONTENT = 2;


        // Public Const PERSONINFOSIZE As Short = 44
        public static short CLOCKINGRECORDSIZE;

        static CKT_DLL()
        {
            var clocking = new CLOCKINGRECORD();
            CLOCKINGRECORDSIZE = Convert.ToInt16(Marshal.SizeOf(clocking));
        }

        // Types


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NETINFO
        {
            [MarshalAs(UnmanagedType.I4)]
            public int ID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] IP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Mask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Gateway;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ServerIP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] MAC;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DATETIMEINFO
        {
            public int ID;
            public short Year_Renamed;
            public byte Month_Renamed;
            public byte Day_Renamed;
            public byte Hour_Renamed;
            public byte Minute_Renamed;
            public byte Second_Renamed;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PERSONINFO
        {
            [MarshalAs(UnmanagedType.I4)]
            public int PersonID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Password;
            [MarshalAs(UnmanagedType.I4)]
            public int CardNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] Name;
            [MarshalAs(UnmanagedType.I4)]
            public int Dept; // ²¿ÃÅ
            [MarshalAs(UnmanagedType.I4)]
            public int Group; // ²¿ÃÅ
            [MarshalAs(UnmanagedType.I4)]
            public int KQOption; // ¿¼ÇÚÄ£Ê½
            [MarshalAs(UnmanagedType.I4)]
            public int FPMark;
            [MarshalAs(UnmanagedType.I4)]
            public int Other; // 0 = Normal User, 1 = Administrator
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CLOCKINGRECORD
        {
            [MarshalAs(UnmanagedType.I4)]
            public int ID;
            [MarshalAs(UnmanagedType.I4)]
            public int PersonID;
            [MarshalAs(UnmanagedType.I4)]
            public int Stat;
            [MarshalAs(UnmanagedType.I4)]
            public int BackupCode;
            [MarshalAs(UnmanagedType.I4)]
            public int WorkTyte;
            // <VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=20)> _
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Time;
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVICEINFO
        {
            public int ID; // Device serial number
            public int MajorVersion; // Device firmware version
            public int MinorVersion; // Device firmware version
            public int SpeakerVolume; // Speaker volume
            public int Parameter; // 
            public int DefaultAuth; // 'Public DoorLockDelay As Integer ' Lock control delay
            public int FixWGHead; // Fixed wiegand area code 
            public int WGOption; // wiegand Option
            public int AutoUpdateAllow; // Allow To update the fingerprint template intelligently
            public int KQRepeatTime; // Options For the repeated clocking
            public int RealTimeAllow; // Allow To transfer data real-time.
            public int RingAllow; // Allow To make ringing
            public int LockDelayTime; // Lock delay time
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] AdminPassword;
            public int Capacity; // Capacity For the staffer information
        }

        public enum WeekDaysEnum : int
        {
            SUN = 1,
            MON = 2,
            TUE = 4,
            WED = 8,
            THU = 16,
            FRI = 32,
            SAT = 64
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RINGTIME
        {
            [MarshalAs(UnmanagedType.I4)]
            public int hour;
            [MarshalAs(UnmanagedType.I4)]
            public int minute;
            [MarshalAs(UnmanagedType.I4)]
            public int week;

            public bool TestWeekDay(WeekDaysEnum wd)
            {
                return (week & (int)wd) == (int)wd;
            }

            public void SetWeekDay(WeekDaysEnum wd, bool value)
            {
                if (value)
                {
                    week = week | (int)wd;
                }
                else
                {
                    week = week & ~(int)wd;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct TIMESECT
        {
            public byte bHour;
            public byte bMinute;
            public byte eHour;
            public byte eMinute;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CKT_MessageInfo
        {
            [MarshalAs(UnmanagedType.I4)]
            public int PersonID;
            [MarshalAs(UnmanagedType.I4)]
            public int sYear;
            [MarshalAs(UnmanagedType.I4)]
            public int sMon;
            [MarshalAs(UnmanagedType.I4)]
            public int sDay;
            [MarshalAs(UnmanagedType.I4)]
            public int eYear;
            [MarshalAs(UnmanagedType.I4)]
            public int eMon;
            [MarshalAs(UnmanagedType.I4)]
            public int eDay;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public byte[] msg;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CKT_MessageHead
        {
            [MarshalAs(UnmanagedType.I4)]
            public int PersonID;
            [MarshalAs(UnmanagedType.I4)]
            public int sYear;
            [MarshalAs(UnmanagedType.I4)]
            public int sMon;
            [MarshalAs(UnmanagedType.I4)]
            public int sDay;
            [MarshalAs(UnmanagedType.I4)]
            public int eYear;
            [MarshalAs(UnmanagedType.I4)]
            public int eMon;
            [MarshalAs(UnmanagedType.I4)]
            public int eDay;
        }

        // Routines

        [DllImport("tc400.dll", EntryPoint = "CKT_FreeMemory", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_FreeMemory(IntPtr memory);
        [DllImport("tc400.dll", EntryPoint = "CKT_RegisterSno", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_RegisterSno(int Sno, int ComPort);

        // Public Declare Function CKT_RegisterNet Lib "tc400.dll" (ByVal Sno As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal Addr As String) As Integer
        [DllImport("tc400.dll", EntryPoint = "CKT_RegisterNet", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_RegisterNet(int Sno, string Addr);
        [DllImport("tc400.dll", EntryPoint = "CKT_UnregisterSnoNet", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void CKT_UnregisterSnoNet(int Sno);
        [DllImport("tc400.dll", EntryPoint = "CKT_NetDaemon", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_NetDaemon();
        [DllImport("tc400.dll", EntryPoint = "CKT_ComDaemon", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ComDaemon();
        [DllImport("tc400.dll", EntryPoint = "CKT_Disconnect", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void CKT_Disconnect();
        [DllImport("tc400.dll", EntryPoint = "CKT_ReportConnections", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ReportConnections(ref IntPtr ppSno);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetDeviceNetInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetDeviceNetInfo(int Sno, ref NETINFO pNetInfo);

        // Public Declare Function CKT_SetDeviceIPAddr Lib "tc400.dll" (ByVal Sno As Integer, ByRef IP As Byte) As Integer
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceIPAddr", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceIPAddr(int Sno, byte[] IP);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceMask", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceMask(int Sno, ref byte Mask);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceMask", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceMask(int Sno, byte[] Mask);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceGateway", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceGateway(int Sno, ref byte Gate);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceGateway", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceGateway(int Sno, byte[] Gate);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceServerIPAddr", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceServerIPAddr(int Sno, ref byte Svr);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceServerIPAddr", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceServerIPAddr(int Sno, byte[] Svr);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceMAC", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceMAC(int Sno, ref byte MAC);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceMAC", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceMAC(int Sno, byte[] MAC);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetDeviceClock", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetDeviceClock(int Sno, ref DATETIMEINFO pDateTimeInfo);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceDate", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceDate(int Sno, short Year_Renamed, byte Month_Renamed, byte Day_Renamed);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceTime", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceTime(int Sno, byte Hour_Renamed, byte Minute_Renamed, byte Second_Renamed);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetFPTemplate", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetFPTemplate(int Sno, int PersonID, int FPID, ref IntPtr pFPData, ref int FPDataLen);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetFPTemplate", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetFPTemplate(int Sno, int PersonID, int FPID, byte[] pFPData, ref int FPDataLen);
        [DllImport("tc400.dll", EntryPoint = "CKT_PutFPTemplate", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_PutFPTemplate(int Sno, int PersonID, int FPID, byte[] pFPData, int FPDataLen);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetFPTemplateSaveFile", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetFPTemplateSaveFile(int Sno, int PersonID, int FPID, string FPDataFilename);
        [DllImport("tc400.dll", EntryPoint = "CKT_PutFPTemplateLoadFile", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_PutFPTemplateLoadFile(int Sno, int PersonID, int FPID, string FPDataFilename);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetFPRawData", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetFPRawData(int Sno, int PersonID, int FPID, ref byte FPRawData);
        [DllImport("tc400.dll", EntryPoint = "CKT_PutFPRawData", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_PutFPRawData(int Sno, int PersonID, int FPID, ref byte FPRawData);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetFPRawDataSaveFile", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetFPRawDataSaveFile(int Sno, int PersonID, int FPID, string FPDataFilename);
        [DllImport("tc400.dll", EntryPoint = "CKT_PutFPRawDataLoadFile", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_PutFPRawDataLoadFile(int Sno, int PersonID, int FPID, string FPDataFilename);
        [DllImport("tc400.dll", EntryPoint = "CKT_ListPersonInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ListPersonInfo(int Sno, ref int pRecordCount, ref int ppPersons);
        [DllImport("tc400.dll", EntryPoint = "CKT_ListPersonInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ListPersonInfo(int Sno, ref int pRecordCount, ref PERSONINFO ppPersons);
        [DllImport("tc400.dll", EntryPoint = "CKT_ModifyPersonInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ModifyPersonInfo(int Sno, ref PERSONINFO person);
        [DllImport("tc400.dll", EntryPoint = "CKT_DeletePersonInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_DeletePersonInfo(int Sno, int PersonID, int backupID);
        [DllImport("tc400.dll", EntryPoint = "CKT_DeleteAllPersonInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_DeleteAllPersonInfo(int Sno);
        [DllImport("tc400.dll", EntryPoint = "CKT_ListPersonInfoEx", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ListPersonInfoEx(int Sno, ref int ppLongRun);
        [DllImport("tc400.dll", EntryPoint = "CKT_ListPersonProgress", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ListPersonProgress(int pLongRun, ref int pRecCount, ref int pRetCount, ref IntPtr ppPersons);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetCounts", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetCounts(int Sno, ref int pPersonCount, ref int pFPCount, ref int pClockingsCount);
        [DllImport("tc400.dll", EntryPoint = "CKT_ClearClockingRecord", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ClearClockingRecord(int Sno, int type, int count);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetClockingRecordEx", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetClockingRecordEx(int Sno, ref IntPtr ppLongRun);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetClockingNewRecordEx", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetClockingNewRecordEx(int Sno, ref IntPtr ppLongRun);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetClockingRecordProgress", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetClockingRecordProgress(int pLongRun, ref int pRecCount, ref int pRetCount, ref IntPtr ppPersons);
        [DllImport("tc400.dll", EntryPoint = "CKT_ResetDevice", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ResetDevice(int Sno);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetDeviceInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetDeviceInfo(int Sno, ref DEVICEINFO devinfo);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDefaultAuth", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDefaultAuth(int Sno, int Auth);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDoor", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDoor(int Sno, int Second_Renamed);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetSpeakerVolume", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetSpeakerVolume(int Sno, int Volume);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetDeviceAdminPassword", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetDeviceAdminPassword(int Sno, [MarshalAs(UnmanagedType.LPStr)] string Password);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetRealtimeMode", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetRealtimeMode(int Sno, int RealMode);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetFixWGHead", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetFixWGHead(int Sno, int WGHead);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetWG", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetWG(int Sno, int WGMode);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetRingAllow", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetRingAllow(int Sno, int type);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetRepeatKQ", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetRepeatKQ(int Sno, int time);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetAutoUpdate", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetAutoUpdate(int Sno, int AutoUpdate);
        [DllImport("tc400.dll", EntryPoint = "CKT_ForceOpenLock", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ForceOpenLock(int Sno);
        [DllImport("tc400.dll", EntryPoint = "CKT_ReadRealtimeClocking", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_ReadRealtimeClocking(ref int ppClockings);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetTimeSection", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetTimeSection(int Sno, int ord, out TIMESECT[] ts);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetTimeSection", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetTimeSection(int Sno, int ord, [In] TIMESECT[] ts);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetGroup", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetGroup(int Sno, int ord, out int[] grp);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetGroup", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetGroup(int Sno, int ord, int[] grp);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetHitRingInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetHitRingInfo(int Sno, out RINGTIME[] array);
        [DllImport("tc400.dll", EntryPoint = "CKT_SetHitRingInfo", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_SetHitRingInfo(int Sno, int ord, ref RINGTIME ring);
        [DllImport("tc400.dll", EntryPoint = "CKT_GetMessageByIndex", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetMessageByIndex(int Sno, int idx, ref CKT_MessageInfo msg);
        [DllImport("tc400.dll", EntryPoint = "CKT_AddMessage", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_AddMessage(int Sno, ref CKT_MessageInfo msg);

        // Public Declare Function CKT_GetAllMessageHead Lib "tc400.dll" (ByVal Sno As Integer, <[In](), Out()> ByVal mh As CKT_MessageHead()) As Integer
        [DllImport("tc400.dll", EntryPoint = "CKT_GetAllMessageHead", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_GetAllMessageHead(int Sno, out CKT_MessageHead[] mh);
        [DllImport("tc400.dll", EntryPoint = "CKT_DelMessageByIndex", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int CKT_DelMessageByIndex(int Sno, int idx);
    }
}