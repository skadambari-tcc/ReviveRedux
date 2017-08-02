using System;
using System.Runtime.Caching;

namespace Revive.Redux.Commn
{

public abstract class ReviveCachingBase
{
    public ReviveCachingBase()
    {
       
    }

    protected MemoryCache cache = new MemoryCache("CachingProvider");

    static readonly object padlock = new object();

    protected virtual void AddItem(string key, object value)
    {
        lock (padlock)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(90);
            
            cache.Add(key, value,policy);
        }
    }

    protected virtual void RemoveItem(string key)
    {
        lock (padlock)
        {
            cache.Remove(key);
        }
    }

    protected virtual object GetItem(string key, bool remove)
    {
        lock (padlock)
        {
            var res = cache[key];

            if (res != null)
            {
                if (remove == true)
                    cache.Remove(key);
            }            

            return res;
        }
    }

  }

}