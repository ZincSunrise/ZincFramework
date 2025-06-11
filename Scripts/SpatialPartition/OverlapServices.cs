using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public static class OverlapServices
    {
        public static IPartitionRunner MainRunner { get; private set; }

        private readonly static List<IOverlaper> _overlapers = new List<IOverlaper>();

        public static void Initialize(IPartitionRunner runner) 
        {
            MainRunner = runner;
        }

        public static void RegistOverlaper(IOverlaper overlaper)
        {
            _overlapers.Add(overlaper);
        }

        public static void UnRegistOverlaper(IOverlaper overlaper)
        {
            _overlapers.Remove(overlaper);
        }

        public static void UpdateRunner()
        {
            for (int i = 0; i < _overlapers.Count; i++) 
            {
                _overlapers[i].UpdateOverlaper(MainRunner);
            }
        }

        public static IOverlapable OverlapCircle(Vector2 center, float radius) => MainRunner.OverlapCircle(center, radius);

        public static IOverlapable OverlapBox(Vector2 center, Vector2 size) => MainRunner.OverlapBox(center, size);

        public static ReadOnlySpan<IOverlapable> OverlapCircleAll(Vector2 center, float radius) => MainRunner.OverlapCircleAll(center, radius);

        public static ReadOnlySpan<IOverlapable> OverlapBoxAll(Vector2 center, Vector2 size) => MainRunner.OverlapBoxAll(center, size);
    }
}
