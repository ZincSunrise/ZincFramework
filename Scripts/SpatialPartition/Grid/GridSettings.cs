using UnityEngine;


namespace ZincFramework.SpatialPartition.Grid
{
    public class GridSettings
    {
        /// <summary>
        /// ���и��ӵ��ܱ߽�
        /// </summary>
        public Bounds GridBounds
        { 
            get => _gridBounds; 
            init 
            {
                _gridBounds = value;
                float originX = value.center.x - value.extents.x;
                float originY = value.center.y + value.extents.y;
                CoodiateOffset = new Vector2(-value.extents.x, value.extents.y);

                OriginPosition = new Vector2(originX, originY);
            } 
        }


        private Bounds _gridBounds;
        /// <summary>
        /// ��ȵ���Ŀ
        /// </summary>
        public int WidthCount { get; init; }

        /// <summary>
        /// �߶ȵ���Ŀ
        /// </summary>
        public int HeightCount { get; init; }


        public float HeightPerGrid { get; init; }


        public float WidthPerGrid { get; init; }


        public Vector2 OriginPosition { get; init; }


        public Vector2 CoodiateOffset { get; init; }
    }
}