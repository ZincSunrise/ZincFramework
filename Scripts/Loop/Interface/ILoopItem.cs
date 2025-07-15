namespace ZincFramework.Loop
{
    /// <summary>
    /// 循环类总接口
    /// </summary>
    public interface ILoopItem
    {
        /// <summary>
        /// 每循环自动调用的函数
        /// </summary>
        /// <returns>在循环系统中是否还能够留在循环，用于自动移除</returns>
        bool Tick();
    }
}