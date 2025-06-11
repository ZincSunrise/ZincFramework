using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZincFramework.SpatialPartition.Grid
{
    public class Grid : IPartition
    {
        public int Count => _overlapables.Count;

        public Bounds Bounds { get; private set; }


        private readonly List<IOverlapable> _overlapables = new List<IOverlapable>();

        public Grid(Bounds bounds) 
        {
            Bounds = bounds;
        }

        #region 元素管理相关
        public void AddElement(IOverlapable overlapable)
        {
            _overlapables.Add(overlapable);
            overlapable.Container = this;

#if UNITY_EDITOR
            if (Count > 0)
            {
                ShowingColor = Color.green;
            }
#endif
        }

        public bool RemoveElement(IOverlapable overlapable)
        {
            int index = _overlapables.IndexOf(overlapable);
            if (index < 0)
            {
                return false;
            }

            _overlapables[index] = _overlapables[^1];
            _overlapables.RemoveAt(_overlapables.Count - 1);
            overlapable.Container = null;

#if UNITY_EDITOR
            if (Count <= 0)
            {
                ShowingColor = Color.blue;
            }
#endif

            return true;
        }
        #endregion

        #region 元素查询相关
        public IOverlapable QueryElement(Bounds bounds)
        {
            for (int i = 0; i < _overlapables.Count; i++)
            {
                if (_overlapables[i].Intersects(bounds))
                {
                    return _overlapables[i];
                }
            }

            return null;
        }

        public void QueryElements(Bounds bounds, ref IOverlapable[] overlapables, ref int count)    
        {
            for (int i = 0; i < _overlapables.Count; i++) 
            {
                if (_overlapables[i].Intersects(bounds))
                {
                    if (count + 1 > overlapables.Length)
                    {
                        Array.Resize(ref overlapables, overlapables.Length * 2);
                    }

                    overlapables[count++] = _overlapables[i];
                }
            }
        }

        public IOverlapable QueryElement(CircleBounds circleBounds)
        {
            for (int i = 0; i < _overlapables.Count; i++)
            {
                if (_overlapables[i].Intersects(circleBounds))
                {
                    return _overlapables[i];
                }
            }

            return null;
        }

        public void QueryElements(CircleBounds circleBounds, ref IOverlapable[] overlapables, ref int count)
        {
            for (int i = 0; i < _overlapables.Count; i++)
            {
                if (_overlapables[i].Intersects(circleBounds))
                {
                    if (count + 1 > overlapables.Length)
                    {
                        Array.Resize(ref overlapables, overlapables.Length * 2);
                    }

                    overlapables[count++] = _overlapables[i];
                }
            }
        }

        public IOverlapable QueryElement(IOverlaper overlaper) => QueryElement(overlaper.Bounds);

        public void QueryElements(IOverlaper overlaper, ref IOverlapable[] overlapables, ref int count) => QueryElements(overlaper.Bounds, ref overlapables, ref count);
        #endregion

        public bool Intersects(IOverlapable overlapable) => overlapable.Intersects(Bounds);

        public bool CanUpdate(IOverlapable overlapable)
        {
            return !overlapable.Intersects(Bounds);
        }

        public void UpdateElement(IOverlapable overlapable)
        {
            if (Intersects(overlapable))
            {

            }
        }

#if UNITY_EDITOR
        public Color ShowingColor { get; set; } = Color.blue;

        public void DrawGizmos()
        {
            Gizmos.color = ShowingColor;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
#endif
    }
}