using System;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
  

    
    internal class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        private SzotarElem<K, T>?[] E;
        private Func<K, int> h;
        private LinkedList<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>?[meret];
            U = new LinkedList<SzotarElem<K, T>>();
            h = x => Math.Abs(hasitoFuggveny(x)) % E.Length;
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret)
            : this(meret, x => x.GetHashCode())
        {
        }

        private SzotarElem<K, T>? KulcsKeres(K kulcs)
        {
            int i = h(kulcs);
            if (E[i] != null && E[i].kulcs.Equals(kulcs))
            {
                return E[i];
            }

            foreach (SzotarElem<K, T> elem in U)
            {
                if (elem.kulcs.Equals(kulcs))
                {
                    return elem;
                }
            }

            return null;
        }

        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                elem.tart = ertek;
                return;
            }

            SzotarElem<K, T> ujElem = new SzotarElem<K, T>(kulcs, ertek);
            int i = h(kulcs);
            if (E[i] == null)
            {
                E[i] = ujElem;
            }
            else
            {
                U.AddLast(ujElem);
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                return elem.tart;
            }
            throw new HibasKulcsKivetel();
        }

        public void Torol(K kulcs)
        {
            int i = h(kulcs);
            if (E[i] != null && E[i].kulcs.Equals(kulcs))
            {
                E[i] = null;
                return;
            }

            SzotarElem<K, T>? torlendo = null;
            foreach (var elem in U)
            {
                if (elem.kulcs.Equals(kulcs))
                {
                    torlendo = elem;
                    break;
                }
            }

            if (torlendo != null)
            {
                U.Remove(torlendo);
                return;
            }

            throw new HibasKulcsKivetel();
        }
    }
}
