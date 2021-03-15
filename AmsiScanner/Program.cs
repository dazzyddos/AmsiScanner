using System;
using System.Runtime.InteropServices;

namespace AmsiScanner
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a string.");
                return -1;
            }

            //Console.WriteLine("Hello World!");
            IntPtr amsiContext;
            IntPtr session;
            AmsiResult result;

            int returnValue = Amsi.AmsiInitialize(@"AmsiScanner", out amsiContext);
            returnValue = Amsi.AmsiOpenSession(amsiContext, out session);

            //Console.WriteLine($"Return value is: {returnValue}");


            string test = args[0];
            returnValue = Amsi.AmsiScanString(amsiContext, "amsiutils", "EICAR", session, out result);

            Console.WriteLine(result);

            return 0;
        }
    }

    internal class Amsi
    {
        [DllImport("Amsi.dll", EntryPoint = "AmsiInitialize", CallingConvention = CallingConvention.StdCall)]
        public static extern int AmsiInitialize([MarshalAs(UnmanagedType.LPWStr)] string appName, out IntPtr amsiContext);

        [DllImport("Amsi.dll", EntryPoint = "AmsiUninitialize", CallingConvention = CallingConvention.StdCall)]
        public static extern void AmsiUninitialize(IntPtr amsiContext);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("Amsi.dll", EntryPoint = "AmsiScanString", CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiScanString(IntPtr amsiContext, [In, MarshalAs(UnmanagedType.LPWStr)] string payload, [In, MarshalAs(UnmanagedType.LPWStr)] string contentName, IntPtr session, out AmsiResult result);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("Amsi.dll", EntryPoint = "AmsiOpenSession", CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiOpenSession(IntPtr amsiContext, out IntPtr session);
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
