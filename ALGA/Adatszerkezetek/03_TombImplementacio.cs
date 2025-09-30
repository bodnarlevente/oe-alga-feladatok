using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OE.ALGA.Adatszerkezetek
{
    public class TömbVerem<T> : Verem<T>
    {
        private T[] E; 
        private int n;



        public bool Ures
        {
            get { return n == 0; }
        }

        public T Felso()
        {
            throw new NotImplementedException();
        }

        public void Verembe(T ertek)
        {
            if (E.Length >= n)
            {
                 
                throw new NincsHelyKivetel();
            }

            
            E.Append(ertek);
        }

        public T Verembol()
        {
            throw new NotImplementedException();
        }
        public TömbVerem(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "A verem mérete pozitív egész szám kell legyen.");
            }
            
            this.E = new T[n];
            this.n = n;
        }
    }
}
