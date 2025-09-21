using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajto
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
                throw new TaroloMegteltKivetel("A tároló megtelt.");
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
    public interface IFuggo
    {
        bool FuggosegTeljesul { get; }
    }
    public interface IVegrehajto
    {
        void Vegrehajtas();
    }
    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel() : base("A tároló megtelt.")
        {
        }
        public TaroloMegteltKivetel(string message) : base(message)
        {
        }
        public TaroloMegteltKivetel(string message, Exception inner) : base(message, inner)
        {
        }


    }
    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajto, IFuggo
    {
        public FuggoFeladatTarolo(int meret) : base(meret)
        {
        }
        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

    }
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

