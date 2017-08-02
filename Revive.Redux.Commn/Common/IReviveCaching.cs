using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Commn
{
    public interface IReviveCaching
    {
        void AddItem(string key, object value);
        object GetItem(string key);
    } 
}
