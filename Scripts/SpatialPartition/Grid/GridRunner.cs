using System;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework.SpatialPartition.Grid
{
    public class GridRunner : IPartitionRunner
    {
        private readonly List<Grid> _grids = new List<Grid>();

        private GridSettings _gridSettings;

        private IOverlapable[] _overlapables = new IOverlapable[16];

        public void Initialize(GridSettings gridSettings)
        {
            _gridSettings = gridSettings;
            float gridWidth = gridSettings.WidthPerGrid;
            float gridHeight = gridSettings.HeightPerGrid;
            Vector2 size = new Vector2(gridWidth, gridHeight);

            Vector2 originPoint = gridSettings.OriginPosition;

            for (int i = 0; i < _gridSettings.HeightCount; i++)
            {
                for (int j = 0; j < _gridSettings.WidthCount; j++)
                {
                    Vector2 center = originPoint + new Vector2((j + 0.5f) * gridWidth, (-i - 0.5f) * gridHeight);
                    Grid grid = new Grid(new Bounds(center, size));
                    _grids.Add(grid);
                }
            }
        }


        #region 元素操作相关
        public void AddElement(IOverlapable overlapable)
        {
            if (overlapable is BoxOverlaper boxOverlaper)
            {
                boxOverlaper.Container ??= GridServices.RentArray();
                GridArray gridArray = boxOverlaper.Container as GridArray;

                var (minRow, maxRow, minCol, maxCol) = CaculateIndices(boxOverlaper.Bounds);
                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        int index = row * _gridSettings.WidthCount + col;
                        if (index >= 0 && index < _grids.Count)
                        {
                            Grid grid = _grids[index];
                            grid.AddElement(overlapable);
                            gridArray.AddGrid(grid);
                        }
                    }
                }

                boxOverlaper.Container = gridArray;
            }
            else if (overlapable is CircleOverlaper circleOverlaper) 
            {
                circleOverlaper.Container ??= GridServices.RentArray();
                GridArray gridArray = circleOverlaper.Container as GridArray;

                var (minRow, maxRow, minCol, maxCol) = CaculateIndices(circleOverlaper.Bounds);

                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        int index = row * _gridSettings.WidthCount + col;
                        if (index >= 0 && index < _grids.Count)
                        {
                            Grid grid = _grids[index];
                            if (circleOverlaper.CircleBounds.Intersects(grid.Bounds))
                            {
                                grid.AddElement(overlapable);
                                gridArray.AddGrid(grid);
                            }
                        }
                    }
                }

                circleOverlaper.Container = gridArray;
            }
            else
            {
                int index = CaculateIndex(overlapable.Position);
                if (index >= 0 && index < _grids.Count)
                {
                    Grid grid = _grids[index];
                    grid.AddElement(overlapable);
                }
            }
        }

        public void RemoveElement(IOverlapable overlapable)
        {
            if (overlapable is BoxOverlaper || overlapable is CircleOverlaper)
            {
                overlapable.Container.RemoveElement(overlapable);
                GridServices.ReturnValue(overlapable.Container as GridArray);
            }
            else
            {
                int index = CaculateIndex(overlapable.Position);

                if (index >= 0 && index < _grids.Count)
                {
                    Grid grid = _grids[index];
                    grid.RemoveElement(overlapable);
                }
            }

            overlapable.Container = null;
        }

        public void UpdateElement(IOverlapable overlapable)
        {
            overlapable.UpdatePosition(out _);

            if(overlapable is CircleOverlaper || overlapable is BoxOverlaper)
            {
                if (overlapable.Container.CanUpdate(overlapable))
                {
                    if (!overlapable.Intersects(_gridSettings.GridBounds))
                    {
                        RemoveElement(overlapable);
                    }
                    else
                    {
                        overlapable.Container.RemoveElement(overlapable);
                        AddElement(overlapable);
                    }
                }
            }
            else
            {
                int index = CaculateIndex(overlapable.Position);

                if (index >= 0 && index < _grids.Count)
                {
                    Grid grid = _grids[index];

                    if (grid != overlapable.Container)
                    {
                        overlapable.Container?.RemoveElement(overlapable);
                        grid.AddElement(overlapable);
                    }
                }
                else
                {
                    overlapable.Container?.RemoveElement(overlapable);
                }
            }
        }
        #endregion

        public IOverlapable OverlapBox(Vector2 center, Vector2 size)
        {
            Bounds bounds = new Bounds(center, size);
            var (minRow, maxRow, minCol, maxCol) = CaculateIndices(bounds);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    int index = row * _gridSettings.WidthCount + col;
                    if (index >= 0 && index < _grids.Count)
                    {
                        Grid grid = _grids[index];
                        IOverlapable overlapable = grid.QueryElement(bounds);
                        if (overlapable != null)
                        {
                            return overlapable;
                        }
                    }
                }
            }

            return null;
        }

        public IOverlapable OverlapCircle(Vector2 center, float radius)
        {
            CircleBounds circleBounds = new CircleBounds(center, radius);
            Bounds bounds = new Bounds(center, new Vector3(radius * 2, radius * 2, 0));
            var (minRow, maxRow, minCol, maxCol) = CaculateIndices(bounds);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    int index = row * _gridSettings.WidthCount + col;
                    if (index >= 0 && index < _grids.Count)
                    {
                        Grid grid = _grids[index];
                        if (circleBounds.Intersects(grid.Bounds))
                        {
                            IOverlapable overlapable = grid.QueryElement(circleBounds);
                            if (overlapable != null)
                            {
                                return overlapable;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public Span<IOverlapable> OverlapBoxAll(Vector2 center, Vector2 size)
        {
            Bounds bounds = new Bounds(center, size);
            int count = 0;

            var (minRow, maxRow, minCol, maxCol) = CaculateIndices(bounds);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    int index = row * _gridSettings.WidthCount + col;
                    if (index >= 0 && index < _grids.Count)
                    {
                        Grid grid = _grids[index];
                        // 调用网格查询，传入的是边界框
                        grid.QueryElements(bounds, ref _overlapables, ref count);
                    }
                }
            }

            return new Span<IOverlapable>(_overlapables, 0, count);
        }

        public Span<IOverlapable> OverlapCircleAll(Vector2 center, float radius)
        {
            int count = 0;

            // 创建一个包围圆形的边界框
            CircleBounds circleBounds = new CircleBounds(center, radius);
            // 方形边界框，边长为半径的两倍，用于初步筛选
            Bounds bounds = new Bounds(center, new Vector3(radius * 2, radius * 2, 0));  

            var (minRow, maxRow, minCol, maxCol) = CaculateIndices(bounds);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    int index = row * _gridSettings.WidthCount + col;
                    if (index >= 0 && index < _grids.Count)
                    {
                        Grid grid = _grids[index];
                        if (circleBounds.Intersects(grid.Bounds))
                        {
                            // 调用网格查询，传入的是边界框
                            grid.QueryElements(circleBounds, ref _overlapables, ref count);
                        }
                    }
                }
            }

            return new Span<IOverlapable>(_overlapables, 0, count);
        }

        private BoundsIndices CaculateIndices(in Bounds bounds)
        {
            // 网格的宽度和高度
            float gridWidth = _gridSettings.WidthPerGrid;
            float gridHeight = _gridSettings.HeightPerGrid;
            Vector2 origin = _gridSettings.OriginPosition;

            // 获取查询范围的最大和最小坐标
            float xMax = bounds.max.x;
            float yMax = bounds.max.y;
            float xMin = bounds.min.x;
            float yMin = bounds.min.y;

            // 计算网格的最小和最大列行索引
            int minCol = Mathf.FloorToInt((xMin - origin.x) / gridWidth);
            int minRow = Mathf.FloorToInt((origin.y - yMax) / gridHeight);  // 注意 Y 轴翻转

            int maxCol = Mathf.FloorToInt((xMax - origin.x) / gridWidth);
            int maxRow = Mathf.FloorToInt((origin.y - yMin) / gridHeight);  // 注意 Y 轴翻转

            // 限制范围，避免越界
            minCol = Mathf.Clamp(minCol, 0, _gridSettings.WidthCount - 1);
            minRow = Mathf.Clamp(minRow, 0, _gridSettings.HeightCount - 1);
            maxCol = Mathf.Clamp(maxCol, 0, _gridSettings.WidthCount - 1);
            maxRow = Mathf.Clamp(maxRow, 0, _gridSettings.HeightCount - 1);

            return new BoundsIndices(minRow, maxRow, minCol, maxCol);
        }

        private int CaculateIndex(Vector2 position)
        {
            if (!_gridSettings.GridBounds.Contains(position))
            {
                return -1; 
            }

            Vector2 localPosition = position - _gridSettings.OriginPosition;

            int xIndex = Mathf.FloorToInt(localPosition.x / _gridSettings.WidthPerGrid);
            int yIndex = Mathf.FloorToInt(-localPosition.y / _gridSettings.HeightPerGrid);

            int index = xIndex + yIndex * _gridSettings.WidthCount;
            return index;
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            for(int i = 0; i < _grids.Count; i++)
            {
                var grid = _grids[i];
                grid.DrawGizmos();
            }
        }
#endif

        private readonly struct BoundsIndices
        {
            public int MinRow { get; }
            public int MaxRow { get; }
            public int MinCol { get; }
            public int MaxCol { get; }

            public BoundsIndices(int minRow, int maxRow, int minCol, int maxCol)
            {
                MinRow = minRow;
                MaxRow = maxRow;
                MinCol = minCol;
                MaxCol = maxCol;
            }

            public void Deconstruct(out int minRow, out int maxRow, out int minCol, out int maxCol)
            {
                minRow = MinRow;
                maxRow = MaxRow;
                minCol = MinCol;
                maxCol = MaxCol;
            }
        }
    }
}