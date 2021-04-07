namespace EQXMedia.TxFileSystem.Tests.Operations.FileStreams.Utils
{
    using System;

    internal static class UnitTestUtils
    {
        public static string GetTempFileName(TxFileSystem txFileSystem)
        {
            var tempPath = txFileSystem.Path.GetTempPath();
            var fileName = tempPath + "filetocreatefilestreamfor-" + Guid.NewGuid().ToString() + ".txt";
            return fileName;
        }
    }
}
