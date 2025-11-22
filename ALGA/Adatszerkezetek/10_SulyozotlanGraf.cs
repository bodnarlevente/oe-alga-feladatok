using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozotlanGraf
    {
        private readonly bool[,] szomszedsagiMatrix;
        public int CsucsokSzama { get; }

        public SulyozotlanGraf(int csucsokSzama)
        {
            if (csucsokSzama <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(csucsokSzama), "A csúcsok száma pozitív egész szám kell legyen.");
            }

            CsucsokSzama = csucsokSzama;
            szomszedsagiMatrix = new bool[csucsokSzama, csucsokSzama];
        }

        public void Elerheto(int forras, int cel)
        {
            EllenorizCsucsIndex(forras);
            EllenorizCsucsIndex(cel);
            szomszedsagiMatrix[forras, cel] = true;
            szomszedsagiMatrix[cel, forras] = true; // Mivel irányítatlan graf
        }

        public bool ElerhetoE(int forras, int cel)
        {
            EllenorizCsucsIndex(forras);
            EllenorizCsucsIndex(cel);
            return szomszedsagiMatrix[forras, cel];
        }

        private void EllenorizCsucsIndex(int index)
        {
            if (index < 0 || index >= CsucsokSzama)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "A csúcs indexe a megadott tartományon kívül esik.");
            }
        }
    }

}