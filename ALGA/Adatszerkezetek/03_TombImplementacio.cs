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
        private int max_meret;
        public bool Ures
        {
            get { return n == 0; }
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
            if (E.Length >= max_meret)
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
            this.max_meret = meret;
        }
        public void Felszabadit()
        {
            E = new T[n];
        }
    }
    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int n = 0;
        private int max_meret;
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
            
        }

        public bool Ures
        {
            get { return n == 0; }
        }

        public T Elso()
        {
            return E[e];    
        }

        public void Sorba(T ertek)
        {
            if (n >= max_meret)
            {
                throw new NincsHelyKivetel();
            }
            E[n] = ertek;
            n++;
        }

        public T Sorbol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T ertek = E[e];   
            n--;
            e++;
            return ertek;
        }
        public void Felszabadit()
        {
            E = new T[n];
            e = 0;
            u = -1;
        }
    }
}
