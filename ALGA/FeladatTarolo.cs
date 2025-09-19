using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA
{
    public abstract class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajto 
    {
        public T[] tarolo;
        public int n;
        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
            n = 0;
        }
        public void Felvesz(T feladat)
        {
            if (n < tarolo.Length)
            {
                tarolo[n] = feladat;
                n++;
            }
            else
            {
                throw new TaroloMegteltException("A tároló megtelt.");
            }
        }
        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
           return new FeladatTaroloBejaro<T>(tarolo, n);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
