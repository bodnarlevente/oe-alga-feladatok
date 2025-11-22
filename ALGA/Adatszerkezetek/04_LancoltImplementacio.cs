using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;

        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }
   

    public class LancoltVerem<T> : Verem<T>
    {
        private LancElem<T>? fej;

        public LancoltVerem()
        {
            fej = null;
        }

        public bool Ures => fej == null;

        public void Felszabadit()
        {
            fej = null;
        }

        public void Verembe(T ertek)
        {
            fej = new LancElem<T>(ertek, fej);
        }

        public T Verembol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T ertek = fej!.tart;
            fej = fej.kov;
            return ertek;
        }

        public T Felso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return fej!.tart;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        private LancElem<T>? fej;
        private LancElem<T>? vege;

        public LancoltSor()
        {
            fej = null;
            vege = null;
        }

        public bool Ures => fej == null;

        public void Felszabadit()
        {
            fej = null;
            vege = null;
        }

        public void Sorba(T ertek)
        {
            var ujElem = new LancElem<T>(ertek, null);
            if (Ures)
            {
                fej = ujElem;
            }
            else
            {
                vege!.kov = ujElem;
            }
            vege = ujElem;
        }

        public T Sorbol()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            T ertek = fej!.tart;
            fej = fej.kov;
            if (fej == null)
            {
                vege = null;
            }
            return ertek;
        }

        public T Elso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return fej!.tart;
        }
    }
    //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
    //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
    //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        private LancElem<T>? fej;
        private int elemszam;

        public int Elemszam => elemszam;

        public LancoltLista()
        {
            fej = null;
            elemszam = 0;
        }
        
        public void Torol(T elem)
        {
            for (int i = 0; i < elemszam; )
            {
                if (Kiolvas(i)!.Equals(elem))
                {
                    Torles(i);
                }
                else
                {
                    i++; 
                        
                }
            }
        }

        public void Felszabadit()
        {
            fej = null;
            elemszam = 0;
        }

        private LancElem<T> ElemLekerdez(int index)
        {
            if (index < 0 || index >= elemszam )
            {
                throw new HibasIndexKivetel();
            }

            LancElem<T> aktualis = fej!;
            for (int i = 0; i < index; i++)
            {
                aktualis = aktualis.kov!;
            }
            return aktualis;
        }

        public T Kiolvas(int index)
        {
            return ElemLekerdez(index).tart;
        }

        public void Modosit(int index, T ertek)
        {
            ElemLekerdez(index).tart = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(elemszam, ertek);
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > elemszam)
            {
                throw new HibasIndexKivetel();
            }

            if (index == 0)
            {
                fej = new LancElem<T>(ertek, fej);
            }
            else
            {
                LancElem<T> elozo = ElemLekerdez(index - 1);
                
                elozo.kov = new LancElem<T>(ertek, elozo.kov);
            }
            elemszam++;
        }

        public void Torles(int index)
        {
            if (index < 0 || index >= elemszam)
            {
                throw new HibasIndexKivetel();
            }

            if (index == 0)
            {
                fej = fej!.kov;
            }
            else
            {
                LancElem<T> elozo = ElemLekerdez(index - 1);
                elozo.kov = elozo.kov!.kov;
            }
            elemszam--;
        }

        public void Bejar(Action<T> muvelet)
        {
            for (LancElem<T>? p = fej; p != null; p = p.kov)
            {
                muvelet(p.tart);
            }
        }

        public IEnumerator<T> BejaroLetrehozas()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        private readonly LancElem<T>? fej;
        private LancElem<T>? aktualisElem;

        public LancoltListaBejaro(LancElem<T>? fej)
        {
            this.fej = fej;
            Alaphelyzet();
        }

        public T Current
        {
            get
            {
                if (aktualisElem == null)
                {
                    throw new InvalidOperationException("A bejaras meg nem kezdodott el vagy mar veget ert.");
                }
                return aktualisElem.tart;
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Alaphelyzet()
        {
            aktualisElem = new LancElem<T>(default(T)!, fej);
        }

        public void Dispose()
        {
            // Nincs eroforras, amit fel kellene szabaditani
        }

        public bool Kovetkezo()
        {
            if (aktualisElem?.kov != null)
            {
                aktualisElem = aktualisElem.kov;
                return true;
            }
            return false;
        }

        public bool MoveNext()
        {
            if (aktualisElem?.kov != null)
            {
                aktualisElem = aktualisElem.kov;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            aktualisElem = new LancElem<T>(default(T)!, fej);
        }
    }

    


}