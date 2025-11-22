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
            T[] ideiglenes = new T[E.Length];
            for (int i = 0; i < n - 1; i++)
            {
                ideiglenes[i] = E[i];
            }
            E = ideiglenes;
            n--;
            return elem;
        }
        public TombVerem(int meret)
        {
            if (meret < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(meret), "A verem mérete pozitív egész szám kell legyen.");
            }

            this.E = new T[meret];

        }
        
    }
    //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
    //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int n;
        private int e;
        private int u;

        public TombSor(int meret)
        {
            if (n < 0)
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
            return E[(e % E.Length) ];
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            n++;
            if (u == E.Length)
            {
                u= 0;
            }
            else
            {
                u++;
            }
            E[u] = ertek;
            
        }

        public T Sorbol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }

            n--;
            T ertek =E[e];
            if (e == E.Length -1)
            {
                e = 0;
            }
            else
            {
                e++;
            }
            return ertek;
            
        }
    }
    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        private T[] E;
        private int n;
        public int Elemszam => n;
        public TombLista()
        {
            
            int goji = 16;
            
            
             this.E = new T[goji];
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
            if (index < 0 || index >= n)
            {
                throw new HibasIndexKivetel();
            }
            return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index >= n)
            {
                throw new HibasIndexKivetel();
            }
            E[index] = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            if (n >= E.Length)
            {
                T[] ideiglenes = new T[E.Length * 2];
                for (int i = 0; i < E.Length; i++)
                {
                    ideiglenes[i] = E[i];
                }
                E = ideiglenes;
            }
            E[n] = ertek;
            n++;
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n)
            {
                throw new HibasIndexKivetel();
            }
            if (n >= E.Length)
            {
                T[] ideiglenes  = new T[E.Length * 2];
                for (int i = 0; i < E.Length; i++)
                {
                    ideiglenes[i] = E[i];
                }
                E = ideiglenes;
            }
            for (int i = n; i > index; i--)
            {
                E[i] = E[i - 1];
            }
            E[index] = ertek;
            n++;
        }

        public void Torol(T ertek)
        {
            if (n >= E.Length)
            {
                throw new NincsHelyKivetel();
            }
            int i = 0;
            while (i < n)
            {
                if (E[i].Equals(ertek))
                {
                    for (int j = i; j < n - 1; j++)
                    {
                        E[j] = E[j + 1];
                    }
                    n--;
                }
                else
                {
                    i++;
                }
            }

        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }


        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < n; i++)
            {
                yield return E[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
