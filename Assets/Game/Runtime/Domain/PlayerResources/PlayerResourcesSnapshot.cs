using System;
using System.Collections.Generic;

namespace Domain.PlayerResources
{
    [Serializable]
    public struct PlayerResourcesSnapshot
    {
        public Dictionary<string, ulong> Resources;
    }
}