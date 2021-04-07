namespace EQXMedia.TxFileSystem.NativeMethods.Win32
{
    using System;
    using System.IO;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    [SuppressUnmanagedCodeSecurity()]
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes fileAttributes,
            IntPtr templateFileHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int GetFinalPathNameByHandle(
            IntPtr handle,
            [In, Out] StringBuilder path,
            int bufLen,
            int flags);

        internal static string GetFinalPathNameByHandle(IntPtr handle)
        {
            var pathBuilder = new StringBuilder(512);
            GetFinalPathNameByHandle(handle, pathBuilder, pathBuilder.Capacity, 0);

            var path = pathBuilder.ToString();

            return path.Replace(@"\\?\", "");
        }
    }
}
