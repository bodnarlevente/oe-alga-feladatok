using System;

namespace OE.ALGA.Adatszerkezetek
{
   

    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobb;

        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            this.nagyobb = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Bal(int i)
        {
            return 2 * i + 1;
        }

        public static int Jobb(int i)
        {
            return 2 * i + 2;
        }

        public static int Szulo(int i)
        {
            return (i - 1) / 2;
        }

        protected void Kupacol(int i)
        {
            int b = Bal(i);
            int j = Jobb(i);
            int max = i;

            if (b < n && nagyobb(E[b], E[max]))
            {
                max = b;
            }

            if (j < n && nagyobb(E[j], E[max]))
            {
                max = j;
            }

            if (max != i)
            {
                T temp = E[i];
                E[i] = E[max];
                E[max] = temp;
                Kupacol(max);
            }
        }

        protected void KupacotEpit()
        {
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] A)
            : base(A, A.Length, (x, y) => x.CompareTo(y) > 0)
        {
        }

        public void Rendezes()
        {
            int eredetiN = n;
            for (int i = n - 1; i > 0; i--)
            {
                T temp = E[0];
                E[0] = E[i];
                E[i] = temp;
                n--;
                Kupacol(0);
            }
            n = eredetiN;
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas)
            : base(new T[meret], 0, nagyobbPrioritas)
        {
        }

        public bool Ures
        {
            get { return n == 0; }
        }

        private void KulcsotFelvisz(int i)
        {
            while (i > 0 && nagyobb(E[i], E[Szulo(i)]))
            {
                int sz = Szulo(i);
                T temp = E[i];
                E[i] = E[sz];
                E[sz] = temp;
                i = sz;
            }
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }

            E[n] = ertek;
            n++;
            KulcsotFelvisz(n - 1);
        }

        public T Sorbol()
        {
            if (n == 0)
            {
                throw new NincsElemKivetel();
            }

            T elso = E[0];
            E[0] = E[n - 1];
            n--;
            Kupacol(0);
            return elso;
        }

        public T Elso()
        {
            if (n == 0)
            {
                throw new NincsElemKivetel();
            }
            return E[0];
        }

        public void Frissit(T ertek)
        {
            int index = -1;
            for (int i = 0; i < n; i++)
            {
                if (E[i].Equals(ertek))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                throw new NincsElemKivetel();
            }

            KulcsotFelvisz(index);
            Kupacol(index);
        }
    }
}