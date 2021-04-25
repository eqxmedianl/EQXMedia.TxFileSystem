namespace EQXMedia.TxFileSystem.NativeMethods.Win32
{
    using System;
    using System.IO;
#if !NET5_0
    using System.Runtime.ConstrainedExecution;
#endif
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    // TODO: Move this to the Unit Test project, because it is only used there. This will minimize the binary size of this library.

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
#if !NET5_0
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetFinalPathNameByHandle(
            IntPtr handle,
            [In, Out] StringBuilder path,
            int bufLen,
            int flags);

        internal static string GetFinalPathNameByHandle(IntPtr handle)
        {
            var pathBuilder = new StringBuilder(512);
            var result = GetFinalPathNameByHandle(handle, pathBuilder, pathBuilder.Capacity, 0);
            if (result != 0)
            {
                var path = pathBuilder.ToString();

                return path.Replace(@"\\?\", "");
            }

            return null;
        }
    }
}
