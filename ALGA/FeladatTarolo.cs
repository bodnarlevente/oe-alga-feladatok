using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA
{
    public class FeladatTarolo<T> where T : IVegrehajto
    {
        new T[] tarolo;
        new int n;
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
                throw new IndexOutOfRangeException("A tároló megtelt.");
            }
        }
        public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }
    }
}
