using System;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; private set; }
        public int Wmax { get; private set; }
        public int[] w { get; private set; }
        public float[] p { get; private set; }

        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = wmax;
            // Biztonságos másolat
            this.w = (int[])w.Clone();
            this.p = (float[])p.Clone();
        }

        public int OsszSuly(bool[] pakolas)
        {
            int suly = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    suly += w[i];
                }
            }
            return suly;
        }

        public float OsszErtek(bool[] pakolas)
        {
            float ertek = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    ertek += p[i];
                }
            }
            return ertek;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        private int m;
        private Func<int, T> generator;
        private Func<T, float> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            this.LepesSzam = 0;
        }

        public T OptimalisMegoldas()
        {
            // JAVÍTÁS: Ha nincs megoldástér (m=0), akkor default értéket adunk vissza,
            // nem próbáljuk meg generálni az 1. elemet.
            if (m <= 0)
            {
                LepesSzam = 0;
                return default(T); // int esetén ez 0, amit a teszt vár
            }

            // 1. Kezdőérték: az első megoldás (1-től indexelve a feladat szerint)
            T legjobbMegoldas = generator(1);
            float legjobbErtek = josag(legjobbMegoldas);

            LepesSzam = 0;

            // 2. Végigiterálunk a maradék lehetőségen (2-től m-ig)
            for (int i = 2; i <= m; i++)
            {
                T aktualisJelolt = generator(i);
                float aktualisErtek = josag(aktualisJelolt);

                LepesSzam++; // Minden összehasonlításnál növeljük

                if (aktualisErtek > legjobbErtek)
                {
                    legjobbErtek = aktualisErtek;
                    legjobbMegoldas = aktualisJelolt;
                }
            }

            return legjobbMegoldas;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problema;

        public int LepesSzam { get; private set; }

        private bool[] _cachedResult;

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            this.LepesSzam = 0;
        }

        public bool[] Generator(int i)
        {
            // i bitjei reprezentálják a kiválasztást
            bool[] pakolas = new bool[problema.n];

            for (int bitIndex = 0; bitIndex < problema.n; bitIndex++)
            {
                // Jobbra toljuk és megnézzük az utolsó bitet
                pakolas[bitIndex] = ((i >> bitIndex) & 1) == 1;
            }
            return pakolas;
        }

        public float Josag(bool[] pakolas)
        {
            if (problema.Ervenyes(pakolas))
            {
                return problema.OsszErtek(pakolas);
            }
            return -1f;
        }

        public bool[] OptimalisMegoldas()
        {
            // Lehetséges megoldások száma: 2^n
            // (Ha n=0, akkor m=1, tehát az üres hátizsáknál is lefut a ciklus egyszer,
            // ami helyes, mert az üres halmaz is egy megoldás).
            int m = 1 << problema.n;

            var nyersEroAlgoritmus = new NyersEro<bool[]>(m, Generator, Josag);

            _cachedResult = nyersEroAlgoritmus.OptimalisMegoldas();
            LepesSzam = nyersEroAlgoritmus.LepesSzam;

            return _cachedResult;
        }

        public float OptimalisErtek()
        {
            if (_cachedResult == null)
            {
                OptimalisMegoldas();
            }
            return problema.OsszErtek(_cachedResult);
        }
    }
}