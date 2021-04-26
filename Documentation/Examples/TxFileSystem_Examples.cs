namespace EQXMedia.TxFileSystem.Examples
{
    internal class TxFileSystem_Examples
    {
        public void Constructor_ExampleOne()
        {
            #region Constructor_ExampleOne

            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;

            // Because the below TxFileSystem is not created inside a transaction scope,
            // it will not make use of the operation journal that provides backup and rollback
            // functionality.
            //
            // NOTE: Hence all operations performed on the TxFileSystem this way will NOT
            //       be transactional, and thus not maintain data integrity.
            var txFileSystem = new TxFileSystem(new FileSystem());

            #endregion
        }

        public void Constructor_ExampleTwo()
        {
            #region Constructor_ExampleTwo

            using EQXMedia.TxFileSystem;

            // Because the below TxFileSystem is not created inside a transaction scope,
            // it will not make use of the operation journal that provides backup and rollback
            // functionality.
            //
            // NOTE: Hence all operations performed on the TxFileSystem this way will NOT
            //       be transactional, and thus not maintain data integrity.
            var txFileSystem = new TxFileSystem();

            #endregion
        }

        public void Constructor_ExampleThree()
        {
            #region Constructor_ExampleThree

            using System.IO.Abstractions.TestingHelpers;
            using EQXMedia.TxFileSystem;
            
            var mockFileSystem = new MockFileSystem();

            // Because the below TxFileSystem is not created inside a transaction scope,
            // it will not make use of the operation journal that provides backup and rollback
            // functionality.
            //
            // NOTE: Hence all operations performed on the TxFileSystem this way will NOT
            //       be transactional, and thus not maintain data integrity.
            var txFileSystem = new TxFileSystem(mockFileSystem);

            #endregion
        }

        public void Constructor_ExampleFour()
        {
            #region Constructor_ExampleFour

            using System.Transactions;
            using System.IO.Abstractions;
            using EQXMedia.TxFileSystem;

            using (var transactionScope = new TransactionScope())
            {
                // Because the below TxFileSystem is created inside a transaction scope, it will
                // make use of the operation journal that provides backup and rollback
                // functionality.
                //
                // NOTE: Hence all operations performed on the TxFileSystem this way WILL
                //       be transactional, if required, and thus maintain data integrity.
                var txFileSystem = new TxFileSystem(new FileSystem());
                
                // Here you perform any operation on the wrapped file system by using the
                // properties TxFileSystem exposes.

                transactionScope.Complete();
            }

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
