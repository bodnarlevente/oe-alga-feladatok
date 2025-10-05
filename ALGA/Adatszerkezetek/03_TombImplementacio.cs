using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {
        private T[] E;
        int n = 0;

        public bool Ures
        {
            get { return n <= 0; }
        }

        public T Felso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return E[n - 1];
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            E[n] = ertek;
            n++;
        }

        public T Verembol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T elem = E[n - 1];
            E = E.Take(n - 1).ToArray();
            n--;
            return elem;
        }
        public TombVerem(int meret)
        {
            if (meret <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(meret), "A verem mérete pozitív egész szám kell legyen.");
            }

            this.E = new T[meret];

        }
        
    }
    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int n;
        private int e;
        private int u;

        public TombSor(int meret)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(meret), "A sor mérete pozitív egész szám kell legyen.");
            }

            this.E = new T[meret];
            this.e = 0;
            this.u = 0;
            this.n = 0;
        }

        public bool Ures
        {
            get { return n <= 0; }
        }

        public T Elso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return E[(e % E.Length) + 1];
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            n++;
            u = (u % E.Length) + 1;
            E[u] = ertek;
        }

        public T Sorbol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }

            n--;
            e = (e % E.Length) + 1;
            return E[e];
        }
    }
    public class TombLista<T> : Lista<T>
    {
        private T[] E;
        private int n;

        public TombLista(int meret)
        {
            if (meret <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(meret), "A lista mérete pozitív egész szám kell legyen.");
            }

            this.E = new T[meret];
            this.n = 0;
        }

        public bool Ures
        {
            get { return n <= 0; }
        }

        public int Elemszam => throw new NotImplementedException();

        public T Elso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return E[0];
        }

        public T Utolso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return E[n - 1];
        }

        public void Elejere(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            for (int i = n; i > 0; i--)
            {
                E[i] = E[i - 1];
            }
            E[0] = ertek;
            n++;
        }

        public void Vegere(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            E[n] = ertek;
            n++;
        }

        public T Elejerol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T elem = E[0];
            for (int i = 0; i < n - 1; i++)
            {
                E[i] = E[i + 1];
            }
            n--;
            return elem;
        }

        public T Vegerol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T elem = E[n - 1];
            n--;
            return elem;
        }

        public void Felszabadit()
        {
            E = new T[n];
            n = 0;
        }

        public T Kiolvas(int index)
        {
            throw new NotImplementedException();
        }

        public void Modosit(int index, T ertek)
        {
            throw new NotImplementedException();
        }

        public void Hozzafuz(T ertek)
        {
            throw new NotImplementedException();
        }

        public void Beszur(int index, T ertek)
        {
            throw new NotImplementedException();
        }

        public void Torol(T ertek)
        {
            throw new NotImplementedException();
        }

        public void Bejar(Action<T> muvelet)
        {
            throw new NotImplementedException();
        }
    }
}
