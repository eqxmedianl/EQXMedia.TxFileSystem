namespace EQXMedia.TxFileSystem.Examples
{
    internal class TxFileSystem_Examples
    {
        public void Constructor_ExampleOne()
        {
            #region Constructor_ExampleOne

            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;

            var txFileSystem = new TxFileSystem(new FileSystem());

            #endregion
        }

        public void Constructor_ExampleTwo()
        {
            #region Constructor_ExampleTwo

            using EQXMedia.TxFileSystem;

            var txFileSystem = new TxFileSystem();

            #endregion
        }

        public void Constructor_ExampleThree()
        {
            #region Constructor_ExampleThree

            using System.IO.Abstractions.TestingHelpers;
            using EQXMedia.TxFileSystem;
            
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            #endregion
        }

        public void Property_File_ExampleOne()
        {
            #region Property_File_ExampleOne

            using System.Transactions;
            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(new FileSystem());
                txFileSystem.File.Create("/tmp/somefile.txt");
                transactionScope.Complete();
            }

            #endregion
        }

        public void Property_File_ExampleTwo()
        {
            #region Property_File_ExampleTwo

            using System.Transactions;
            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;
            
            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(new FileSystem());
                if (!txFileSystem.File.Exists("/tmp/somefile.txt") {
                    txFileSystem.File.Create("/tmp/somefile.txt");
                }
                transactionScope.Complete();
            }

            #endregion
        }

        public void Property_Directory_ExampleOne()
        {
            #region Property_Directory_ExampleOne

            using System.Transactions;
            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;
            
            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(new FileSystem());
                txFileSystem.Directory.Create("/data/downloads/audiobooks");
                transactionScope.Complete();
            }

            #endregion
        }

        public void Property_Directory_ExampleTwo()
        {
            #region Property_Directory_ExampleTwo

            using System.Transactions;
            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;
            
            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(new FileSystem());
                if (!txFileSystem.Directory.Exists("/data/downloads/audiobooks") {
                    txFileSystem.Directory.Create("/data/downloads/audiobooks");
                }
                transactionScope.Complete();
            }

            #endregion
        }
    }
}
