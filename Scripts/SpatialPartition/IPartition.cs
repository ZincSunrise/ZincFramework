namespace ZincFramework.SpatialPartition
{
    public interface IPartition
    {
        void AddElement(IOverlapable overlapable);

        bool RemoveElement(IOverlapable overlapable);

        bool Intersects(IOverlapable overlapable);

        bool CanUpdate(IOverlapable overlapable);

        IOverlapable QueryElement(IOverlaper overlaper);

        void QueryElements(IOverlaper overlaper, ref IOverlapable[] overlapables, ref int count);
    }
}