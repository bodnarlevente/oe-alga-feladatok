using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA
{
    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private T[] tarolo;
        private int n;
        private int pozicio;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
            this.pozicio = -1;
        }

        public T Current
        {
            get
            {
                if (pozicio < 0 || pozicio >= n)
                {
                    throw new InvalidOperationException();
                }
                return tarolo[pozicio];
            }
        }

        object System.Collections.IEnumerator.Current => Current;

        public void Dispose()
        {
            // Nincs szükség erőforrások felszabadítására
        }

        public bool MoveNext()
        {
            if (pozicio < n - 1)
            {
                pozicio++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            pozicio = -1;
        }
    }
}
