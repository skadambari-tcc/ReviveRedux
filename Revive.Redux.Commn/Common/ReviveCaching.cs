using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Commn
{
    public class ReviveCaching : ReviveCachingBase, IReviveCaching
    {
        #region Singleton

        protected ReviveCaching()
        {
        }

        public static ReviveCaching Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        class Singleton
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Singleton()
            {
            }
            internal static readonly ReviveCaching instance = new ReviveCaching();
        }

        #endregion

        #region ICachingProvider

        public virtual new void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        public virtual object GetItem(string key)
        {
            return base.GetItem(key, true);//Remove default is true because it's Global Cache!
        }

        public virtual new object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

        #endregion

    } 
}
