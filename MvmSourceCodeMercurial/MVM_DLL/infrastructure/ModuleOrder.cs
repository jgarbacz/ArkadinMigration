using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    // Each module order is a list of string decimals meaning
    // (
	//  'BEFORE|DURING|AFTER|DONE',
	//  explicit order,
	//  implicit order
    // )

    public class ModuleOrder: IComparable<ModuleOrder>
    {
        public static readonly int ORD_BEFORE = 0;
        public static readonly int ORD_DURING = 1;
        public static readonly int ORD_AFTER = 2;
        public static readonly int ORD_DONE = 3;
        public static readonly StringDecimal SD_BEFORE = new StringDecimal(ORD_BEFORE);
        public static readonly StringDecimal SD_AFTER = new StringDecimal(ORD_AFTER);

        public static ModuleOrder DoneOrder = CreateDoneOrder();

        // Proc orders are just lists of string decimals. This list should be immutable
        // even though we do not have a way to make is so.
        public readonly List<StringDecimal> orders;

        // speed up the hashing
        private int h = 0;
        override public int GetHashCode()
        {
            if (h != 0) return h;
            h = 1;
            for (int i = 0; i < this.orders.Count; i++)
            {
                StringDecimal s = orders[i];
                h = 31 * h + (s == null ? 0 : s.GetHashCode());
            }
            return h;
        }

        public override bool Equals(object obj)
        {
            ModuleOrder that = obj as ModuleOrder;
            if (that == null) return false;
            return this.Equals(that);
        }

        #region constructors

        private ModuleOrder()
        {
            throw new Exception("no parameterless constructor");
        }

        public ModuleOrder(List<StringDecimal> orders)
        {
            this.orders = orders;
        }

        public ModuleOrder(params StringDecimal[] orders)
        {
            this.orders = new List<StringDecimal>(orders);
        }

        // Is called once to instanciate the special done newOrder
        public static ModuleOrder CreateDoneOrder()
        {
            ModuleOrder o = new ModuleOrder(new StringDecimal(ORD_DONE));
            return o;
        }

        // Creates a before newOrder with the passed implicitOrder
        public static ModuleOrder CreateBeforeOrder(int explicitOrder,int implicitOrder )
        {
            return CreateBeforeOrder(implicitOrder.ToString(),explicitOrder.ToString());
        }

        // Creates a before newOrder with the passed implicitOrder
        public static ModuleOrder CreateBeforeOrder(string explicitOrder, string implicitOrder)
        {
            ModuleOrder o = new ModuleOrder(
                new StringDecimal(ORD_BEFORE),
                new StringDecimal(explicitOrder.ToString()),
                new StringDecimal(implicitOrder.ToString())
                );
            return o;
        }



        // Creates a after newOrder with the passed implicitOrder
        public static ModuleOrder CreateAfterOrder(string explicitOrder, string implicitOrder)
        {
            ModuleOrder o = new ModuleOrder(
                new StringDecimal(ORD_AFTER),
                new StringDecimal(explicitOrder.ToString()),
                new StringDecimal(implicitOrder.ToString())
                );
            return o;
        }

        // Creates a during newOrder with the passed implicitOrder
        public static ModuleOrder CreateDuringOrder(int implicitOrder)
        {
            return CreateDuringOrder(implicitOrder.ToString());
        }

        // Creates a during newOrder with the passed implicitOrder
        public static ModuleOrder CreateDuringOrder(string implicitOrder){
            ModuleOrder o = new ModuleOrder(
                new StringDecimal(ORD_DURING),
                new StringDecimal(0),
                new StringDecimal(implicitOrder.ToString())
                );
            return o;
        }

        // Creates a during newOrder relative the to passed parent newOrder
        public static ModuleOrder CreateDuringOrder(ModuleOrder parentOrder, int implicitOrder)
        {
            List<StringDecimal> orders = new List<StringDecimal>(parentOrder.orders.Count + 3);
            foreach (StringDecimal sd in parentOrder.orders)
            {
                orders.Add(sd);
            }
            orders.Add(new StringDecimal(ORD_DURING));
            orders.Add(new StringDecimal(0));
            orders.Add(new StringDecimal(implicitOrder));
            ModuleOrder o = new ModuleOrder(orders);
            return o;
        }

        #endregion


        // string version of the moduleOrder for debugging
        override public string ToString(){
            return "ModuleOrder("+orders.Join(',')+")";
        }

        // compare one to another 
        public int CompareTo(ModuleOrder that)
        {
            if (this == that) return 0; 
            for (int i = 0; i < this.orders.Count; i++)
            {
                if (that.orders.Count < (i + 1)) return 1;
                StringDecimal thisOrder = this.orders[i];
                StringDecimal thatOrder = that.orders[i];
                int test = thisOrder.CompareTo(thatOrder);
                if (test != 0) return test;
            }
            if (that.orders.Count > this.orders.Count) return -1;
            return 0;
        }

        // returns true if the newModules orders are equal
        public bool Equals(ModuleOrder that)
        {
            if (this == that) return true; 
            if (this.GetHashCode() != that.GetHashCode()) return false;
           bool result=this.CompareTo(that)==0;
           return result;
        }

    }
}
