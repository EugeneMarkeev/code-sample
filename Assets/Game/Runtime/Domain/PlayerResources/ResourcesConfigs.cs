using System;
using System.Collections.Generic;

namespace Domain.PlayerResources
{
    [Serializable]
    public class ResourcesConfigs
    {
        public List<ResourceConfig> ResourceConfigs;

        public ResourceConfig GetResourceConfig(string resourceId)
        {
            foreach (var config in ResourceConfigs)
                if (config.Id == resourceId)
                    return config;

            throw new ArgumentException($"Config for resource id {resourceId} is not exists");
        }
    }
}