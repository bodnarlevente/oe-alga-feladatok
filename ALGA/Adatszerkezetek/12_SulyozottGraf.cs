using System;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    public static class Kiterjesztesek
    {
        public static T[] Tombbe<T>(this Halmaz<T> halmaz)
        {
            List<T> lista = new List<T>();
            halmaz.Bejar(x => lista.Add(x));
            return lista.ToArray();
        }
    }

    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; private set; }

        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            Suly = suly;
        }
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        private int n;
        private float[,] M;

        public int CsucsokSzama
        {
            get { return n; }
        }

        public int ElekSzama
        {
            get
            {
                int count = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            elek.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                        }
                    }
                }
                return elek;
            }
        }

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !double.IsNaN(M[honnan, hova]);
        }

        public float Suly(int honnan, int hova)
        {
            if (float.IsNaN(M[honnan, hova]))
            {
                throw new NincsElKivetel();
            }
            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (!double.IsNaN(M[csucs, i]))
                {
                    szomszedok.Beszur(i);
                }
            }
            return szomszedok;
        }
    }


    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
            where E : SulyozottGrafEl<V>
            where V : IComparable<V>
        {
            HasitoSzotarTulcsordulasiTerulettel<V, float> d = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.ElekSzama);
            V[] csucsokTomb = g.Csucsok.Tombbe();

            for (int i = 0; i < csucsokTomb.Length; i++)
            {
                d.Beir(csucsokTomb[i], float.PositiveInfinity);
            }
            d.Beir(start, 0);

            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(
                g.CsucsokSzama,
                (v1, v2) => d.Kiolvas(v1) < d.Kiolvas(v2)
            );

            for (int i = 0; i < csucsokTomb.Length; i++)
            {
                S.Sorba(csucsokTomb[i]);
            }

            while (!S.Ures)
            {
                V u = S.Sorbol();
                V[] szomszedok = g.Szomszedai(u).Tombbe();

                for (int i = 0; i < szomszedok.Length; i++)
                {
                    V v = szomszedok[i];
                    float tavolsag = d.Kiolvas(u) + g.Suly(u, v);
                    if (tavolsag < d.Kiolvas(v))
                    {
                        d.Beir(v, tavolsag);
                        S.Frissit(v);
                    }
                }
            }

            return d;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start)
            where V : IComparable<V>
            where E : SulyozottGrafEl<V>
        {
            HasitoSzotarTulcsordulasiTerulettel<V, float> d = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.ElekSzama);
            HasitoSzotarTulcsordulasiTerulettel<V, V> pi = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.ElekSzama);
            FaHalmaz<V> H = new FaHalmaz<V>();
            V[] csucsokTomb = g.Csucsok.Tombbe();

            for (int i = 0; i < csucsokTomb.Length; i++)
            {
                V v = csucsokTomb[i];
                d.Beir(v, float.PositiveInfinity);
                H.Beszur(v);
            }
            d.Beir(start, 0);

            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(
                g.CsucsokSzama,
                (v1, v2) => d.Kiolvas(v1) < d.Kiolvas(v2)
            );

            for (int i = 0; i < csucsokTomb.Length; i++)
            {
                S.Sorba(csucsokTomb[i]);
            }

            while (!S.Ures)
            {
                V u = S.Sorbol();
                H.Torol(u);

                V[] szomszedok = g.Szomszedai(u).Tombbe();
                for (int i = 0; i < szomszedok.Length; i++)
                {
                    V v = szomszedok[i];
                    if (H.Eleme(v) && g.Suly(u, v) < d.Kiolvas(v))
                    {
                        d.Beir(v, g.Suly(u, v));

                        try
                        {
                            pi.Torol(v);
                        }
                        catch { }
                        pi.Beir(v, u);

                        S.Frissit(v);
                    }
                }
            }

            return pi;
        }

        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g)
            where E : SulyozottGrafEl<V>, IComparable<E>
            where V : IComparable<V>
        {

            
            FaHalmaz<E> A = new FaHalmaz<E>();
            HasitoSzotarTulcsordulasiTerulettel<V, V> szulo = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.ElekSzama);

            V[] csucsok = g.Csucsok.Tombbe();
            for (int i = 0; i < csucsok.Length; i++)
            {
                szulo.Beir(csucsok[i], csucsok[i]);
            }

            List<E> elekLista = new List<E>();
            E[] elekTomb = g.Elek.Tombbe();
            for (int i = 0; i < elekTomb.Length; i++)
            {
                elekLista.Add(elekTomb[i]);
            }

            elekLista.Sort((e1, e2) => e1.Suly.CompareTo(e2.Suly));

            foreach (E el in elekLista)
            {
                V u = el.Honnan;
                V v = el.Hova;

                V gyokerU = HolVan(szulo, u);
                V gyokerV = HolVan(szulo, v);

                if (!gyokerU.Equals(gyokerV))
                {
                    A.Beszur(el);
                    Unio(szulo, gyokerU, gyokerV);
                }
            }

            return A;
        }

        private static V HolVan<V>(Szotar<V, V> szulo, V v) where V : IComparable<V>
        {
            V aktualis = v;
            while (!szulo.Kiolvas(aktualis).Equals(aktualis))
            {
                aktualis = szulo.Kiolvas(aktualis);
            }
            return aktualis;
        }

        private static void Unio<V>(HasitoSzotarTulcsordulasiTerulettel<V, V> szulo, V u, V v) where V : IComparable<V>
        {
            szulo.Beir(u, v);
        }
    }
}