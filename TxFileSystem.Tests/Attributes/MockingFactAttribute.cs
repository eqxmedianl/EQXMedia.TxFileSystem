namespace EQXMedia.TxFileSystem.Tests.Attributes
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public abstract class MockingFactAttribute : BeforeAfterTestAttribute
    {
        public static readonly object MocksLock = new object();

        protected MockingFactAttribute([CallerMemberName] string unitTestMethodName = "") : base()
        {
            this.UnitTestMethodName = unitTestMethodName;

            lock (MocksLock)
            {
                if (Mocks == default)
                {
                    Mocks = new Dictionary<string, IList<Mock>>();
                }
            }
        }

        public static Dictionary<string, IList<Mock>> Mocks { get; set; }

        public string UnitTestMethodName { get; private set; }

        public override void After(MethodInfo methodUnderTest)
        {
            CleanMocks();

            base.After(methodUnderTest);
        }

        protected void AddMock(Mock mock)
        {
            lock (MocksLock)
            {
                if (!Mocks.ContainsKey(this.UnitTestMethodName))
                {
                    Mocks.Add(this.UnitTestMethodName, new List<Mock>());
                }

                Mocks[this.UnitTestMethodName].Add(mock);
            }
        }

        internal void CleanMocks()
        {
            lock (MocksLock)
            {
                if (Mocks.ContainsKey(this.UnitTestMethodName))
                {
                    Mocks.Remove(this.UnitTestMethodName);
                }
            }
        }

        public Mock<TMock> GetMock<TMock>() where TMock : class
        {
            lock (MocksLock)
            {
                var mock = Mocks[this.UnitTestMethodName]
                    .First(m => m.GetType() == typeof(Mock<TMock>) || m.Object.GetType().GetInterfaces().Contains(typeof(TMock)));

                return mock as Mock<TMock>;
            }
        }

        protected dynamic GetMock(Type mockedType)
        {
            lock (MocksLock)
            {
                if (!mockedType.Assembly.FullName.Contains("Moq"))
                {
                    mockedType = typeof(Mock<>).MakeGenericType(mockedType);
                    var found = Mocks[this.UnitTestMethodName]
                        .FirstOrDefault(m => m.GetType() == mockedType || m.Object.GetType().GetInterfaces().Contains(mockedType));

                    return found as dynamic;
                }

                var mock = Mocks[this.UnitTestMethodName]
                    .FirstOrDefault(m =>
                    {
                        var t1 = m.Object.GetType().AssemblyQualifiedName;
                        var t2 = mockedType.GenericTypeArguments[0].AssemblyQualifiedName;

                        return t1.Equals(t2);
                    });

                return mock as dynamic;
            }
        }
    }
}
