using System;

namespace OE.ALGA.Adatszerkezetek
{
   public class FaElem<T> where T : IComparable<T>
   {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;

        public FaElem(T tartalom, FaElem<T>? left, FaElem<T>? right)
        {
            tart = tartalom;
            if (left != null)
                bal = left;
            else
                bal = null;
            if (right != null)
                jobb = right;
            else
                jobb = null;
        }
   }
    public class FaHalmaz<T>  : Halmaz<T> where T : IComparable<T>
    {
        
        private FaElem<T>? gyoker;
        public FaHalmaz()
        {
            gyoker = null;
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }
        public bool Eleme(T ertek)
        {
            throw new NotImplementedException();
        }
        public void Torol(T ertek)
        {
            throw new NotImplementedException();
        }
        public void Bejar(Action<T> muvelet)
        {
            throw new NotImplementedException();
        }
        FaElem<T>? ReszfabaBeszur(FaElem<T>? p, T ertek)
        {
            if (p == null)
            {
                return new FaElem<T>(ertek, null, null);
            }
            if (ertek.CompareTo(p.tart) < 0)
            {
                p.bal = ReszfabaBeszur(p.bal, ertek);
            }
            else if (ertek.CompareTo(p.tart) > 0)
            {
                p.jobb = ReszfabaBeszur(p.jobb, ertek);
            }
            return p;
        }
        
    }
 }
