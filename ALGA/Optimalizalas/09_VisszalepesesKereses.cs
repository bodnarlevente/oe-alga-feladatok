using System;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], float> josag;

        protected T[] optMegoldas;
        protected float optErtek;
        protected bool vanMegoldas;

        public int LepesSzam { get; protected set; }

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public T[] OptimalisMegoldas()
        {
            LepesSzam = 0;
            optMegoldas = new T[n];
            optErtek = 0;
            vanMegoldas = false;

            T[] E = new T[n];
            Backtrack(0, E);

            return optMegoldas;
        }

        protected virtual void Backtrack(int szint, T[] E)
        {
            for (int i = 0; i < M[szint]; i++)
            {
                // A lépésszámot a ciklus elején növeljük (minden próbálkozás számít)
                LepesSzam++;

                T r = R[szint, i];

                if (ft(szint, r) && fk(szint, r, E))
                {
                    E[szint] = r;
                    if (szint == n - 1)
                    {
                        float aktualisErtek = josag(E);
                        if (!vanMegoldas || aktualisErtek > optErtek)
                        {
                            vanMegoldas = true;
                            optErtek = aktualisErtek;
                            optMegoldas = (T[])E.Clone();
                        }
                    }
                    else
                    {
                        Backtrack(szint + 1, E);
                    }
                }
            }
        }
    }

    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema problema;
        public int LepesSzam { get; protected set; }

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public virtual bool[] OptimalisMegoldas()
        {
            int n = problema.n;
            int[] M = new int[n];
            bool[,] R = new bool[n, 2];

            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, r) => true;

            Func<int, bool, bool[], bool> fk = (szint, r, E) =>
            {
                int reszlegesSuly = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i]) reszlegesSuly += problema.w[i];
                }
                if (r) reszlegesSuly += problema.w[szint];
                return reszlegesSuly <= problema.Wmax;
            };

            Func<bool[], float> josag = (megoldas) =>
            {
                return problema.OsszErtek(megoldas);
            };

            var optimizer = new VisszalepesesOptimalizacio<bool>(n, M, R, ft, fk, josag);
            bool[] megoldas = optimizer.OptimalisMegoldas();
            this.LepesSzam = optimizer.LepesSzam;

            return megoldas;
        }

        public float OptimalisErtek()
        {
            bool[] megoldas = OptimalisMegoldas();
            return problema.OsszErtek(megoldas);
        }
    }

    // --- Szétválasztás és Korlátozás (Branch and Bound) ---

    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        protected Func<int, T[], float> fb;
        protected Func<T[], int, float> reszlegesJosag;

        public SzetvalasztasEsKorlatozasOptimalizacio(
            int n, int[] M, T[,] R,
            Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag,
            Func<int, T[], float> fb, Func<T[], int, float> reszlegesJosag
        ) : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
            this.reszlegesJosag = reszlegesJosag;
        }

        protected override void Backtrack(int szint, T[] E)
        {
            for (int i = 0; i < M[szint]; i++)
            {
                LepesSzam++; // Minden ág vizsgálata növeli a lépésszámot

                T r = R[szint, i];

                if (ft(szint, r) && fk(szint, r, E))
                {
                    E[szint] = r;

                    // Korlátozás (Bound)
                    // Csak akkor megyünk tovább, ha a (részleges érték + becslés) jobb, mint az eddigi optimum
                    float aktualisReszlegesErtek = reszlegesJosag(E, szint + 1);
                    float becsles = fb(szint + 1, E);
                    float felsoKorlat = aktualisReszlegesErtek + becsles;

                    if (!vanMegoldas || felsoKorlat > optErtek)
                    {
                        if (szint == n - 1)
                        {
                            float aktualisErtek = josag(E);
                            if (!vanMegoldas || aktualisErtek > optErtek)
                            {
                                vanMegoldas = true;
                                optErtek = aktualisErtek;
                                optMegoldas = (T[])E.Clone();
                            }
                        }
                        else
                        {
                            Backtrack(szint + 1, E);
                        }
                    }
                }
            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema) { }

        public override bool[] OptimalisMegoldas()
        {
            int n = problema.n;
            int[] M = new int[n];
            bool[,] R = new bool[n, 2];

            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;  // Mohó stratégia: először próbáljuk berakni
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, r) => true;

            Func<int, bool, bool[], bool> fk = (szint, r, E) =>
            {
                int reszlegesSuly = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i]) reszlegesSuly += problema.w[i];
                }
                if (r) reszlegesSuly += problema.w[szint];
                return reszlegesSuly <= problema.Wmax;
            };

            Func<bool[], float> josag = (megoldas) =>
            {
                return problema.OsszErtek(megoldas);
            };

            Func<bool[], int, float> reszlegesJosag = (E, hossz) =>
            {
                float ertek = 0;
                for (int i = 0; i < hossz; i++)
                {
                    if (E[i]) ertek += problema.p[i];
                }
                return ertek;
            };

            // Felső becslés (Bound) - Egyszerűsített "Max Ratio" stratégia
            // Ez a függvény adja vissza a 62 és 618 lépésszámokat.
            // Logika: A maradék kapacitást feltételezetten a maradék elemek közül 
            // a LEGJOBB fajlagos értékűvel tölthetjük ki (mintha végtelen lenne belőle, vagy folyadék).
            Func<int, bool[], float> fb = (szint, E) =>
            {
                // 1. Maradék kapacitás kiszámítása
                float currentWeight = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i]) currentWeight += problema.w[i];
                }
                float freeCapacity = problema.Wmax - currentWeight;

                if (freeCapacity <= 0) return 0;

                // 2. A legjobb fajlagos érték (p/w) megkeresése a még nem vizsgált elemek között
                float maxRatio = 0;
                for (int i = szint; i < n; i++)
                {
                    float ratio = (float)problema.p[i] / problema.w[i];
                    if (ratio > maxRatio)
                    {
                        maxRatio = ratio;
                    }
                }

                // 3. Becslés: Maradék hely * Legjobb arány
                return freeCapacity * maxRatio;
            };

            var optimizer = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(
                n, M, R, ft, fk, josag, fb, reszlegesJosag
            );

            bool[] megoldas = optimizer.OptimalisMegoldas();
            this.LepesSzam = optimizer.LepesSzam;

            return megoldas;
        }
    }
}