using System;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            int osszSuly = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszSuly += w[i];
                }
            }
            return osszSuly;
        }

        public double OsszErtek(bool[] pakolas)
        {
            double osszErtek = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszErtek += p[i];
                }
            }
            return osszErtek;
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
        private Func<T, double> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, double> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            this.LepesSzam = 0;
        }

        public T OptimalisMegoldas()
        {
            LepesSzam = 0;
            if (m <= 0)
            {
                return default(T);
            }

            T optimalis = generator(1);
            double maxErtek = josag(optimalis);

            for (int i = 2; i <= m; i++)
            {
                T aktualis = generator(i);
                double aktualisErtek = josag(aktualis);

                LepesSzam++;
                if (aktualisErtek > maxErtek)
                {
                    maxErtek = aktualisErtek;
                    optimalis = aktualis;
                }
            }

            return optimalis;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problama;
        private bool[] optimalisPakolas;

        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problama)
        {
            this.problama = problama;
            this.LepesSzam = 0;
            this.optimalisPakolas = null;
        }

        public bool[] Generator(int i)
        {
            bool[] pakolas = new bool[problama.n];
            int index = i - 1;

            for (int j = 0; j < problama.n; j++)
            {
                pakolas[j] = (index % 2) == 1;
                index /= 2;
            }
            return pakolas;
        }

        public double Josag(bool[] pakolas)
        {
            if (!problama.Ervenyes(pakolas))
            {
                return -1;
            }
            return problama.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            int m = (int)Math.Pow(2, problama.n);

            NyersEro<bool[]> megoldasKereso = new NyersEro<bool[]>(m, Generator, Josag);

            this.optimalisPakolas = megoldasKereso.OptimalisMegoldas();
            this.LepesSzam = megoldasKereso.LepesSzam;

            return this.optimalisPakolas;
        }

        public double OptimalisErtek()
        {
            if (this.optimalisPakolas == null)
            {
                this.OptimalisMegoldas();
            }

            if (this.optimalisPakolas == null)
            {
                return -1;
            }

            return Josag(this.optimalisPakolas);
        }
    }
}
