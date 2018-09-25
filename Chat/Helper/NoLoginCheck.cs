using System;

namespace Chat.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoLoginCheck: Attribute
    {
        public NoLoginCheck() { }
    }
}