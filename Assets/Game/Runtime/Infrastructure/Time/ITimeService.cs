using System;

namespace Infrastructure.Time
{
    public interface ITimeService
    {
        DateTime CurrentTime { get; }
        void Initialize();
    }
}