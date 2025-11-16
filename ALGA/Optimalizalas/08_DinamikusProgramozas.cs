using System;

namespace OE.ALGA.Optimalizalas
{



    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; private set; }
        private float[,] F_cache; // A kiszamitott tablazat tarolasara

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            this.F_cache = null; // Kezdetben nincs kiszamolt tablazat
        }

        // Ez a metodus most mar csak akkor szamol, ha szukseges
        public float[,] TablazatFeltoltes()
        {
            // Ha mar egyszer kiszamoltuk a tablazatot, adjuk vissza azt
            if (F_cache != null)
            {
                return F_cache;
            }

            int n = problema.n;
            int Wmax = problema.Wmax;
            float[,] F = new float[n + 1, Wmax + 1];
            int lepes = 0;

            for (int t = 0; t <= n; t++)
            {
                for (int h = 0; h <= Wmax; h++)
                {
                    if (t == 0 || h == 0)
                    {
                        F[t, h] = 0;
                    }
                    else
                    {
                        // A lepesszamot csak a tenyleges szamitasi lepeseknel noveljuk
                        lepes++;
                        if (problema.w[t - 1] <= h)
                        {
                            F[t, h] = Math.Max(problema.p[t - 1] + F[t - 1, h - problema.w[t - 1]], F[t - 1, h]);
                        }
                        else
                        {
                            F[t, h] = F[t - 1, h];
                        }
                    }
                }
            }

            // A szamitast csak egyszer vegezzuk el, ezert a LepesSzam-ot is csak ekkor allitjuk be
            LepesSzam = lepes;

            // Eltaroljuk a kiszamitott tablazatot a kesobbi hasznalatra
            F_cache = F;

            return F_cache;
        }

        public float OptimalisErtek()
        {
            // Meghivja a TablazatFeltoltes-t, ami vagy szamol, vagy a tarolt erteket adja vissza
            float[,] F = TablazatFeltoltes();
            return F[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            // Meghivja a TablazatFeltoltes-t, ami vagy szamol, vagy a tarolt erteket adja vissza
            float[,] F = TablazatFeltoltes();
            int n = problema.n;
            int W = problema.Wmax;
            bool[] megoldas = new bool[n];
            int maradekKapacitas = W;

            for (int i = n; i > 0 && maradekKapacitas > 0; i--)
            {
                if (F[i, maradekKapacitas] != F[i - 1, maradekKapacitas])
                {
                    megoldas[i - 1] = true;
                    maradekKapacitas -= problema.w[i - 1];
                }
            }
            return megoldas;
        }
    }
}
