using System;

namespace ZincFramework.AI.Clustering
{
    public class ClusterAgent
    {
        public static ClusterAgent Instance => _instance.Value;

        protected static Lazy<ClusterAgent> _instance = new Lazy<ClusterAgent>(() => new ClusterAgent());

        private ClusterAgent() { }
    }
}