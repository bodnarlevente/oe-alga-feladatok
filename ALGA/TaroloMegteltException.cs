using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA
{
    internal class TaroloMegteltException : Exception
    {
        public TaroloMegteltException() : base("A tároló megtelt.")
        {
        }
        public TaroloMegteltException(string message) : base(message)
        {
        }
        public TaroloMegteltException(string message, Exception inner) : base(message, inner)
        {
        }
       

    }
}
