using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottGraf
    {
        private readonly int?[,] szomszedsagiMatrix;
        public int CsucsokSzama { get; }

        public SulyozottGraf(int csucsokSzama)
        {
            if (csucsokSzama <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(csucsokSzama), "A csúcsok száma pozitív egész szám kell legyen.");
            }

            CsucsokSzama = csucsokSzama;
            szomszedsagiMatrix = new int?[csucsokSzama, csucsokSzama];
        }

        public void Elerheto(int forras, int cel, int suly)
        {
            EllenorizCsucsIndex(forras);
            EllenorizCsucsIndex(cel);
            szomszedsagiMatrix[forras, cel] = suly;
            szomszedsagiMatrix[cel, forras] = suly; // Mivel irányítatlan graf
        }

        public int? ElerhetoE(int forras, int cel)
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
