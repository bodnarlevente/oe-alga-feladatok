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
            return ReszfaEleme(gyoker, ertek);
        }
        public void Torol(T ertek)
        {
            var ujGyoker = ReszfabolTorol(gyoker, ertek);

            
            gyoker = ujGyoker;
        }
        protected static FaElem<T>? ReszfabolTorol(FaElem<T>? p, T ertek)
        {
            if (p == null)
            {
                
                throw new NincsElemKivetel();
            }

            int osszehasonlitas = ertek.CompareTo(p.tart);

            if (osszehasonlitas < 0)
            {
                p.bal = ReszfabolTorol(p.bal, ertek);
            }
            else if (osszehasonlitas > 0)
            {
                p.jobb = ReszfabolTorol(p.jobb, ertek);
            }
            else 
            {
                
                if (p.bal == null)
                {
                    return p.jobb; 
                }
                else if (p.jobb == null)
                {
                    return p.bal; 
                }

                
                p = KetGyerekesTorles(p);
            }

            return p;
        }
        protected static FaElem<T> KetGyerekesTorles(FaElem<T> torlendoElem)
        {
            
            FaElem<T> utodParent = torlendoElem;
            FaElem<T> utod = torlendoElem.jobb!;

            while (utod.bal != null)
            {
                utodParent = utod;
                utod = utod.bal;
            }

            
            torlendoElem.tart = utod.tart;

            
            
            if (utodParent == torlendoElem)
            {
                torlendoElem.jobb = utod.jobb;
            }
            else 
            {
                utodParent.bal = utod.jobb;
            }

            return torlendoElem;
        }
        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }
        protected static void ReszfaBejarasPreOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        public void BejarInOrder(Action<T> muvelet)
        {
            ReszfaBejarasInOrder(gyoker, muvelet);
        }
        protected static void ReszfaBejarasInOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasInOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasInOrder(p.jobb, muvelet);
            }
        }
        protected static void ReszfaBejarasPostOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPostOrder(p.bal, muvelet);
                ReszfaBejarasPostOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
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
        bool ReszfaEleme(FaElem<T>? p, T ertek)
        {
            if (p == null)
            {
                return false; 
            }

            int osszehasonlitas = ertek.CompareTo(p.tart);

            if (osszehasonlitas == 0)
            {
                return true; 
            }
            else if (osszehasonlitas < 0)
            {
                return ReszfaEleme(p.bal, ertek); 
            }
            else
            {
                return ReszfaEleme(p.jobb, ertek); 
            }
        }



    }
 }
