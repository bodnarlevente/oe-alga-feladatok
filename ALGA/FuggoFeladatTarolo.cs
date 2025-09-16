using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA
{
    abstract class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajto, IFuggo
    {
        public FuggoFeladatTarolo(int meret) : base(meret)
        {
        }
        public void FuggosegSzerintVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

    }
}
