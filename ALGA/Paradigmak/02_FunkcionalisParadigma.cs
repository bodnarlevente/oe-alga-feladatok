using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable<T> where T : IVegrehajthato
    {
        public Func<T, bool>? BejaroFeltetel { get; set; }
        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
            
        }
        public FeltetelesFeladatTarolo(int meret, Func<T, bool> bejaroFeltetel) : base(meret)
        {
            this.BejaroFeltetel = bejaroFeltetel;
        }
        public void FeltetelesVegrehajtas (Func<T, bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]) is true )
                {
                    tarolo[i].Vegrehajtas();
                }
                
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            
            if (BejaroFeltetel is null)
            {
                return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel=>true);
            }
            else 
            {
                return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
            }
            

        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private T[] tarolo;
        private int n;
        private int aktualisIndex;
        private Func<T, bool> feltetel;

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> feltetel)
        {
            this.tarolo = tarolo;
            this.n = n;
            this.aktualisIndex = -1;
            this.feltetel = feltetel;
        }

        public T Current
        {
            get
            {
                if (aktualisIndex < 0 || aktualisIndex >= n)
                {
                    throw new InvalidOperationException();
                }
                if (feltetel(tarolo[aktualisIndex]) is true)
                {
                    return tarolo[aktualisIndex];
                }
                else
                {
                    throw new InvalidOperationException("A feltétel nem teljesül az aktuális elemre.");
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // Nincs szükség erőforrások felszabadítására ebben az esetben
        }

        public bool MoveNext()
        {
            if (aktualisIndex < n - 1)
            {
                aktualisIndex++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }



}
