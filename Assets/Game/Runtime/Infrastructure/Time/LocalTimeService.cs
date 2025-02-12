using System;
using UnityEngine.Scripting;

namespace Infrastructure.Time
{
    public class LocalTimeService : ITimeService
    {
        [Preserve]
        public LocalTimeService()
        {
        }

        public DateTime CurrentTime => DateTime.UtcNow;

        public void Initialize()
        {
        }
    }
}