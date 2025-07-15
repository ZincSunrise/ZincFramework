using ZincFramework.Pools;


namespace ZincFramework.SpatialPartition.Grid
{
    public static class GridServices
    {
        private static readonly ObjectPool<GridArray> _arrayPool = new ObjectPool<GridArray>(() => new GridArray());

        public static GridArray RentArray()
        {
            return _arrayPool.RentValue();
        }

        public static void ReturnValue(GridArray gridArray) 
        {
            _arrayPool.ReturnValue(gridArray);
        }
    }
}