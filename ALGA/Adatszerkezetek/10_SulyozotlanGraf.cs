using System;
using System.Collections.Generic; // A Queue (Sor) hasznalatahoz a szelessegi bejarasnal

namespace OE.ALGA.Adatszerkezetek
{
    // 1. EgeszGrafEl osztaly
    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public int Honnan { get; }
        public int Hova { get; }

        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        // IOsszehasonlithato (IComparable) megvalositasa
        public int CompareTo(EgeszGrafEl other)
        {
            if (other == null) return 1;

            // Ha a.Honnan != b.Honnan
            if (this.Honnan != other.Honnan)
            {
                return this.Honnan.CompareTo(other.Honnan);
            }
            // Ha a.Honnan == b.Honnan, akkor a Hova dont
            return this.Hova.CompareTo(other.Hova);
        }

        public override string ToString()
        {
            return string.Format("{0}->{1}", Honnan, Hova);
        }
    }

    // 2. CsucsmatrixSulyozatlanEgeszGraf osztaly
    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private int n;
        private bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            this.M = new bool[n, n]; // Alapertelmezetten hamis ertekekkel toltodik fel
        }

        public int CsucsokSzama
        {
            get { return n; }
        }

        public int ElekSzama
        {
            get
            {
                int szamlalo = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            szamlalo++;
                        }
                    }
                }
                return szamlalo;
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

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            elek.Beszur(new EgeszGrafEl(i, j));
                        }
                    }
                }
                return elek;
            }
        }

        public void UjEl(int honnan, int hova)
        {
            if (honnan >= 0 && honnan < n && hova >= 0 && hova < n)
            {
                M[honnan, hova] = true;
            }
        }

        public bool VezetEl(int honnan, int hova)
        {
            if (honnan >= 0 && honnan < n && hova >= 0 && hova < n)
            {
                return M[honnan, hova];
            }
            return false;
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            if (csucs >= 0 && csucs < n)
            {
                for (int j = 0; j < n; j++)
                {
                    if (M[csucs, j])
                    {
                        szomszedok.Beszur(j);
                    }
                }
            }
            return szomszedok;
        }
    }

    // 3. GrafBejarasok osztaly
    public class GrafBejarasok
    {
        // (a) Szelessegi bejaras
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet)
            where V : IComparable<V> // Megszoritas: V legyen osszehasonlithato
            where E : GrafEl<V>
        {
            FaHalmaz<V> F = new FaHalmaz<V>(); // Feldolgozott csucsok halmaza
            Queue<V> S = new Queue<V>();       // Sor a bejarashoz

            // Start pont felvetele
            F.Beszur(start);
            S.Enqueue(start);
            muvelet(start);

            while (S.Count > 0)
            {
                V k = S.Dequeue();

                // Szomszedok lekerese
                Halmaz<V> szomszedok = g.Szomszedai(k);

                // Iteralas a szomszedokon
                szomszedok.Bejar(x =>
                {
                    if (!F.Eleme(x))
                    {
                        F.Beszur(x);
                        S.Enqueue(x);
                        muvelet(x);
                    }
                });
            }

            return F;
        }

        // (b) Melysegi bejaras
        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet)
            where V : IComparable<V>
            where E : GrafEl<V>
        {
            FaHalmaz<V> F = new FaHalmaz<V>(); // Feldolgozott csucsok halmaza

            // Rekurziv segedfuggveny hivasa
            MelysegiBejarasRekurzio(g, start, F, muvelet);

            return F;
        }

        // (c) Melysegi bejaras rekurzio
        private static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
            where V : IComparable<V>
            where E : GrafEl<V>
        {
            F.Beszur(k);   // Megjeloljuk, hogy jartunk itt
            muvelet(k);     // Feldolgozas

            Halmaz<V> szomszedok = g.Szomszedai(k);

            szomszedok.Bejar(x =>
            {
                if (!F.Eleme(x))
                {
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
                }
            });
        }
    }
}