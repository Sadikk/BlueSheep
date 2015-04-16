using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public static class Injection
{
    private const uint MEM_COMMIT = 0x1000;
    private const uint MEM_DECOMMIT = 0x4000;
    private const uint MEM_RELEASE = 0x8000;
    private const uint MEM_RESERVE = 0x2000;
    private const uint PAGE_EXECUTE_READWRITE = 0x40;
    private const uint PAGE_READWRITE = 4;
    private const uint PROCESS_ALL_ACCESS = 0x1f0fff;
    private const uint TH32CS_SNAPPROCESS = 2;
    private const uint WAIT_ABANDONED = 0x80;
    private const uint WAIT_FAILED = uint.MaxValue;
    private const uint WAIT_OBJECT_0 = 0;
    private const uint WAIT_TIMEOUT = 0x102;

    [DllImport("KERNEL32.DLL")]
    private static extern bool CloseHandle(IntPtr hObject);
    [DllImport("KERNEL32.DLL")]
    private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr se, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, uint lpThreadId);
    [DllImport("KERNEL32.DLL")]
    private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);
    [DllImport("KERNEL32.DLL")]
    private static extern int GetLastError();
    [DllImport("KERNEL32.DLL", CharSet=CharSet.Ansi)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("KERNEL32.DLL", CharSet=CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    public static Process[] GetProcessId(string proc)
    {
        return Process.GetProcessesByName(proc);
    }

    [DllImport("KERNEL32.DLL")]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
    [DllImport("KERNEL32.DLL")]
    private static extern bool Process32First(IntPtr hSnapShot, ref PROCESSENTRY32 pe);
    [DllImport("KERNEL32.DLL")]
    private static extern bool Process32Next(IntPtr Handle, ref PROCESSENTRY32 lppe);
    public static bool StartInjection(string DllName, uint ProcessID)
    {
        bool flag;
        try
        {
            IntPtr hProcess = new IntPtr(0);
            IntPtr lpBaseAddress = new IntPtr(0);
            IntPtr lpStartAddress = new IntPtr(0);
            IntPtr hHandle = new IntPtr(0);
            int nSize = DllName.Length + 1;
            hProcess = OpenProcess(0x1f0fff, false, ProcessID);
            if (!(hProcess != IntPtr.Zero))
            {
                throw new Exception("Processus non ouvert...injection \x00e9chou\x00e9e");
            }
            lpBaseAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (UIntPtr) nSize, 0x1000, 0x40);
            if (!(lpBaseAddress != IntPtr.Zero))
            {
                throw new Exception("M\x00e9moire non allou\x00e9e...injection \x00e9chou\x00e9e");
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            int lpNumberOfBytesWritten = 0;
            if (!WriteProcessMemory(hProcess, lpBaseAddress, encoding.GetBytes(DllName), nSize, lpNumberOfBytesWritten))
            {
                throw new Exception("Erreur d'\x00e9criture dans le processus...injection \x00e9chou\x00e9e");
            }
            lpStartAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (!(lpStartAddress != IntPtr.Zero))
            {
                throw new Exception("Adresse LoadLibraryA non trouv\x00e9e...injection \x00e9chou\x00e9e");
            }
            hHandle = CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpBaseAddress, 0, 0);
            if (!(hHandle != IntPtr.Zero))
            {
                throw new Exception("Probl\x00e8me au lancement du thread...injection \x00e9chou\x00e9e");
            }
            uint num3 = WaitForSingleObject(hHandle, 0x2710);
            if (((num3 == uint.MaxValue) && (num3 == 0x80)) && ((num3 == 0) && (num3 == 0x102)))
            {
                throw new Exception("WaitForSingle \x00e9chou\x00e9 : " + num3.ToString() + "...injection \x00e9chou\x00e9e");
            }
            if (!VirtualFreeEx(hProcess, lpBaseAddress, 0, 0x8000))
            {
                throw new Exception("Probl\x00e8me lib\x00e8ration de m\x00e9moire...injection \x00e9chou\x00e9e");
            }
            if (hHandle == IntPtr.Zero)
            {
                throw new Exception("Mauvais Handle du thread...injection \x00e9chou\x00e9e");
            }
            CloseHandle(hHandle);
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            flag = false;
        }
        return flag;
    }

    [DllImport("KERNEL32.DLL")]
    private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAdress, UIntPtr dwSize, uint flAllocationType, uint flProtect);
    [DllImport("KERNEL32.DLL")]
    private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAdress, uint dwSize, uint dwFreeType);
    [DllImport("KERNEL32.DLL")]
    private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliSeconds);
    [DllImport("KERNEL32.DLL")]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESSENTRY32
    {
        public int dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string szExeFile;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }
}
