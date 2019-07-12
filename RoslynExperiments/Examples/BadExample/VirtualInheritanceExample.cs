using System;

namespace BadExample
{
    public interface ISomeGrandParentInterfaceA
    {
    }

    public interface ISomeGrandParentInterfaceB
    {
    }

    public interface ISomeGrandParentInterfaceC
    {
    }

    public interface ISomeParentInterfaceA : ISomeGrandParentInterfaceA, ISomeGrandParentInterfaceC
    {
    }

    public interface ISomeParentInterfaceB : ISomeGrandParentInterfaceB, ICloneable
    {
    }

    public interface ISomeParentInterfaceC : ICloneable
    {
    }

    public interface ISomeChildInterfaceA : ISomeParentInterfaceA, ISomeParentInterfaceC
    {
    }

    public interface ISomeChildInterfaceB : ISomeParentInterfaceB, ICloneable
    {
    }

    public class SomeClassA : ISomeChildInterfaceA
    {
        public Object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class SomeClassB : ISomeChildInterfaceB
    {
        public Object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class VirtualInheritanceExample : ICloneable, IDisposable
    {
        public Object Clone()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
