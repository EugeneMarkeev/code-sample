using System;

namespace Domain.PlayerResources
{
    [Serializable]
    public struct Resource
    {
        public string Id;
        public ulong Count;

        public Resource(string id, ulong count)
        {
            Id = id;
            Count = count;
        }
    }
}