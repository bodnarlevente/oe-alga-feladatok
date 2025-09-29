using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

//kaki
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
        public new IEnumerator<T> GetEnumerator()
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
                
                
                return tarolo[aktualisIndex];
                
                

            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // Nincs szükség erőforrások felszabadítására ebben az esetben
        }

        public bool MoveNext()
        {
            while (++aktualisIndex < n)
            {
                if (feltetel == null || feltetel(tarolo[aktualisIndex]))
                {
                    return true; 
                }
            }
            return false;

        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }



}
