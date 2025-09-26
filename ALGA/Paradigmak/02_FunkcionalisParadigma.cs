using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
        }
        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i] is IFuggo f && f.FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
                else if (!(tarolo[i] is IFuggo))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

    }

    

}
