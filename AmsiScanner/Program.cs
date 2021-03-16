using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AmsiScanner
{
    class Program
    {
        static int Main(string[] args)
        {
            /*if (args.Length == 0)
            {
                Console.WriteLine("Please enter a string.");
                return -1;
            }*/

            //Console.WriteLine("Hello World!");
            IntPtr amsiContext;
            IntPtr session;
            AmsiResult result;

            int returnValue = Amsi.AmsiInitialize(@"AmsiScanner", out amsiContext);
            returnValue = Amsi.AmsiOpenSession(amsiContext, out session);

            //string test = args[0];
            //var sample = Encoding.UTF8.GetBytes("Invoke-Expression");
            //var sample = Encoding.UTF8.GetBytes("Invoke-Mimikatz 'AMSI Test Sample: 7e72c3ce-861b-4339-8740-0ac1484c1386'");
            string sample = "Invoke-Mimikatz 'AMSI Test Sample: 7e72c3ce-861b-4339-8740-0ac1484c1386'";
            returnValue = Amsi.AmsiScanString(amsiContext, sample, "maal", session, out result);

            //returnValue = Amsi.AmsiScanBuffer(amsiContext, sample, (uint)sample.Length, "sample", session, out result);

            Console.WriteLine(result);

            return 0;
        }
    }

    internal class Amsi
    {
        [DllImport("amsi.dll", EntryPoint = "AmsiInitialize", CallingConvention = CallingConvention.StdCall)]
        public static extern int AmsiInitialize([MarshalAs(UnmanagedType.LPWStr)] string appName, out IntPtr amsiContext);

        [DllImport("amsi.dll", EntryPoint = "AmsiUninitialize", CallingConvention = CallingConvention.StdCall)]
        public static extern void AmsiUninitialize(IntPtr amsiContext);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("amsi.dll", EntryPoint = "AmsiScanString", CallingConvention = CallingConvention.StdCall)]
        public static extern int AmsiScanString(IntPtr amsiContext, [In, MarshalAs(UnmanagedType.LPWStr)] string payload, [In, MarshalAs(UnmanagedType.LPWStr)] string contentName, IntPtr session, out AmsiResult result);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("amsi.dll", EntryPoint = "AmsiOpenSession", CallingConvention = CallingConvention.StdCall)]
        public static extern int AmsiOpenSession(IntPtr amsiContext, out IntPtr session);

        [DllImport("amsi.dll", EntryPoint = "AmsiScanBuffer", CallingConvention = CallingConvention.StdCall)]
        public static extern int AmsiScanBuffer(IntPtr amsiContext, byte[] buffer, uint length, string contentName, IntPtr session, out AmsiResult result);
    }

    enum AmsiResult
    {
        AMSI_RESULT_CLEAN = 0,
        AMSI_RESULT_NOT_DETECTED = 1,
        AMSI_RESULT_BLOCKED_BY_ADMIN_START = 16384,
        AMSI_RESULT_BLOCKED_BY_ADMIN_END = 20479,
        AMSI_RESULT_DETECTED = 32768,
    }
}
