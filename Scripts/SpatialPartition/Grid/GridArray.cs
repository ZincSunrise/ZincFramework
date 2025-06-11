using System.Collections.Generic;


namespace ZincFramework.SpatialPartition.Grid
{
    public class GridArray : IPartition
    {
        private readonly List<Grid> _grids = new List<Grid>();

        public void AddGrid(Grid grid)
        {
            _grids.Add(grid);
        }

        public void RemoveGrid(Grid grid)
        {
            _grids.Remove(grid);
        }

        public bool Intersects(IOverlapable overlapable)
        {
            bool isIntersects = true;
            for(int i = 0; i < _grids.Count; i++)
            {
                isIntersects |= _grids[i].Intersects(overlapable);
            }

            return isIntersects;
        }


        public bool CanUpdate(IOverlapable overlapable) => !Intersects(overlapable);

        void IPartition.AddElement(IOverlapable overlapable)
        {
            throw new System.NotImplementedException();
        }

        IOverlapable IPartition.QueryElement(IOverlaper overlaper)
        {
            throw new System.NotImplementedException();
        }

        void IPartition.QueryElements(IOverlaper overlaper, ref IOverlapable[] overlapables, ref int count)
        {
            throw new System.NotImplementedException();
        }

        bool IPartition.RemoveElement(IOverlapable overlapable)
        {
            if(_grids?.Count == 0)
            {
                return false;
            }

            for(int i = 0; i < _grids.Count; i++)
            {
                _grids[i].RemoveElement(overlapable);
            }

            _grids.Clear();
            return true;
        }
    }
}