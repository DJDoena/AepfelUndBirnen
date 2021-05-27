using System;

namespace AepfelUndBirnen
{
    internal interface IA
    { }

    internal interface IB : IA
    { }

    internal interface IC
    { }

    internal interface ID : IB, IC
    { }

    internal interface IE : ID, IA
    { }

    internal abstract class InterfaceBase : IE, IA, IServiceProvider
    {
        public abstract object GetService(Type serviceType);
    }

    internal sealed class InterfaceImpl : InterfaceBase
    {
        public override object GetService(Type serviceType)
        {
            return null;
        }
    }
}