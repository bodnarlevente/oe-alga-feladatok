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
        // Fontos: a mező neve 'm', ahogy a konstruktorban is hivatkozunk rá
        private int m;
        private Func<int, T> generator;
        private Func<T, float> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            // ITT VOLT A HIBA LEHETŐSÉGE: 
            // Biztosítani kell, hogy a privát mező megkapja a paraméter értékét.
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            this.LepesSzam = 0;
        }

        public T OptimalisMegoldas()
        {
            // Ha 0 vagy kevesebb lehetőség van, nincs miből választani -> default
            // Ez kezeli az "Expected: 0 But was: 4" típusú hibákat üres bemenetnél.
            if (m <= 0)
            {
                LepesSzam = 0;
                return default(T);
            }

            // 1. Az első elem (1-es indexű) feltételezése legjobbnak
            T legjobbMegoldas = generator(1);
            float legjobbErtek = josag(legjobbMegoldas);

            LepesSzam = 0;

            // 2. A többi elem (2-től m-ig) vizsgálata
            // Ha m=1, ez a ciklus nem fut le, és helyesen visszaadjuk az elsőt.
            // Ha m=4 (és a várt érték 4), akkor ez a ciklus meg fogja találni.
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
            bool[] pakolas = new bool[problema.n];
            for (int bitIndex = 0; bitIndex < problema.n; bitIndex++)
            {
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
            // 2 a hatodikon helyett bitshift: 1 << n
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