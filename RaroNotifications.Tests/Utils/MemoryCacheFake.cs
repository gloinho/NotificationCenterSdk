using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using RaroNotifications.Models;

namespace RaroNotifications.Tests.Utils
{
    public sealed class MemoryCacheFake : IMemoryCache
    {
        private readonly string _token;
        public MemoryCacheFake(string token)
        {
            _token = token;
        }
        public MemoryCacheFake()
        {
            
        }
        public ICacheEntry CreateEntry(object key)
        {
            return new CacheEntry() { Key = key };
        }

        public void Dispose()
        {
        }

        public void Remove(object key)
        {

        }

        public bool TryGetValue(object key, out object value)
        {
            value = _token;
            return true;
        }

        private sealed class CacheEntry : ICacheEntry
        {
            public DateTimeOffset? AbsoluteExpiration { get; set; }
            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

            public IList<IChangeToken> ExpirationTokens { get; set; }

            public object Key { get; set; }

            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; set; }

            public CacheItemPriority Priority { get; set; }
            public long? Size { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            public object Value { get; set; }

            public void Dispose()
            {

            }
        }
    }
}
