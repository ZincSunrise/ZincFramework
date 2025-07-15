using System;
using System.Linq;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static UnityEngine.LowLevel.PlayerLoopSystem;



namespace ZincFramework.Loop.Internal
{
    /// <summary>
    /// 循环插入工具类，用于在 Unity PlayerLoop 中插入和移除自定义循环回调，
    /// 支持基于类型定位插入位置，避免依赖 MonoBehaviour 实现 Update 等生命周期函数。
    /// </summary>
    internal static class LoopSystemTool
    {        
        /// <summary>
        /// 当前 Unity PlayerLoop 系统快照，用于在其基础上插入或移除子系统
        /// </summary>
        public static ref PlayerLoopSystem CurrentLoopSystem => ref _currentLoopSystem;

        /// <summary>
        /// 当前 Unity PlayerLoop 系统快照，用于在其基础上插入或移除子系统
        /// </summary>
        private static PlayerLoopSystem _currentLoopSystem;

        static LoopSystemTool()
        {
            _currentLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
        }

        internal static void SetPlayerLoop()
        {
            PlayerLoop.SetPlayerLoop(_currentLoopSystem);
        }

        /// <summary>
        /// 向指定的 PlayerLoop 子系统（如 Update、LateUpdate 等）中批量插入多个 ILoopModule。
        /// 根据 isLast 参数控制插入位置（头部或尾部），实现模块级更新逻辑注入。
        /// </summary>
        /// <param name="loopModules">需要插入的自定义循环模块数组</param>
        /// <param name="loopType">要插入的 PlayerLoop 生命周期类型（例如 Update、FixedUpdate 等）</param>
        /// <param name="isLast">为 true 时表示插入到目标子系统的末尾，否则插入到开头</param>
        /// <returns>修改后的 PlayerLoopSystem，用于最终提交</returns>
        internal static PlayerLoopSystem AddSameLoopTypePlayerLoops(ILoopModule[] loopModules, E_LoopType loopType, bool isLast)
        {
            // 将逻辑枚举类型转换为实际的 PlayerLoopSystem 类型
            var flagType = SelectType(loopType);

            // 在当前主 PlayerLoopSystem 中查找指定生命周期对应的子系统索引
            int needLoopIndex = GetLoopTypeIndex(_currentLoopSystem.subSystemList, flagType);

            // 使用 ref 引用获取到该生命周期的子系统，以支持原地修改
            ref PlayerLoopSystem needLoop = ref _currentLoopSystem.subSystemList[needLoopIndex];

            int preLength = needLoop.subSystemList.Length;           // 原有子系统数量
            int newLength = preLength + loopModules.Length;          // 扩展后的总数量

            // 扩展子系统列表空间，以容纳新的 loopModules
            Array.Resize(ref needLoop.subSystemList, newLength);

            if (isLast)
            {
                // 如果是追加到末尾，从 preLength 开始依次填充新模块
                for (int i = preLength; i < newLength; i++)
                {
                    ref ILoopModule loopModule = ref loopModules[i - preLength];

                    CheckType(loopModule, loopType);

                    // 构造新的 PlayerLoopSystem 项，绑定模块的 Tick 委托和标识类型
                    PlayerLoopSystem newLoopSystem = new PlayerLoopSystem()
                    {
                        updateDelegate = loopModule.Tick,
                        type = loopModule.FlagType ?? flagType,
                    };

                    needLoop.subSystemList[i] = newLoopSystem;
                }
            }
            else
            {
                // 如果插入到开头，先将旧元素整体向后移动，为新模块腾出位置
                Array.Copy(needLoop.subSystemList, 0, needLoop.subSystemList, loopModules.Length, preLength);

                // 再从 index = 0 开始插入 loopModules
                for (int i = 0; i < loopModules.Length; i++)
                {
                    ref ILoopModule loopModule = ref loopModules[i];

                    CheckType(loopModule, loopType);

                    PlayerLoopSystem newLoopSystem = new PlayerLoopSystem()
                    {
                        updateDelegate = loopModule.Tick,
                        type = loopModule.FlagType ?? flagType,
                    };

                    needLoop.subSystemList[i] = newLoopSystem;
                }
            }

            // 返回修改后的主循环结构
            return _currentLoopSystem;
        }

        private static void CheckType(ILoopModule loopModule, E_LoopType loopType)
        {
            if(loopModule.LoopType != loopType)
            {
                throw new ArgumentException($"该循环模块的类型不等于其所需类型\n所需类型{loopType} 模块类型{loopModule.LoopType}");
            }
        }

        /// <summary>
        /// 向指定的 Unity 循环类型中添加一个自定义的 PlayerLoopSystem，
        /// 不依赖 MonoBehaviour 实现 Update 等函数。
        /// </summary>
        /// <returns>返回更新后的完整 PlayerLoopSystem，调用者需要自行调用 PlayerLoop.SetPlayerLoop 以生效</returns>
        internal static PlayerLoopSystem AddPlayerLoop(ILoopModule loopModule, bool isLast)
        {
            return AddPlayerLoop(loopModule.Tick, SelectType(loopModule.LoopType), loopModule.FlagType, isLast);
        }

        /// <summary>
        /// 向指定的 Unity 循环类型中添加一个自定义的 PlayerLoopSystem，
        /// 不依赖 MonoBehaviour 实现 Update 等函数。
        /// </summary>
        /// <param name="updateFunction">每个循环中调用的委托函数</param>
        /// <param name="loopType">参与的 Unity 循环类型（枚举）</param>
        /// <param name="flagType">标志位类型，一般用于后续精准删除，若为空则使用默认的 loopType 类型</param>
        /// <returns>返回更新后的完整 PlayerLoopSystem，调用者需要自行调用 PlayerLoop.SetPlayerLoop 以生效</returns>
        internal static PlayerLoopSystem AddPlayerLoop(UpdateFunction updateFunction, E_LoopType loopType, Type flagType, bool isLast)
        {
            return AddPlayerLoop(updateFunction, SelectType(loopType), flagType, isLast);
        }

        /// <summary>
        /// 向指定的 Unity 循环类型中添加一个自定义的 PlayerLoopSystem，
        /// 不依赖 MonoBehaviour 实现 Update 等函数。
        /// </summary>
        /// <param name="updateFunction">每个循环中调用的委托函数</param>
        /// <param name="loopType">参与的 Unity 循环类型（类型）</param>
        /// <param name="flagType">标志位类型，一般用于后续精准删除，若为空则使用默认的 loopType 类型</param>
        /// <returns>返回更新后的完整 PlayerLoopSystem，调用者需要自行调用 PlayerLoop.SetPlayerLoop 以生效</returns>
        internal static PlayerLoopSystem AddPlayerLoop(UpdateFunction updateFunction, Type loopType, Type flagType, bool isLast)
        {
            // 构造一个新的 PlayerLoopSystem，包含自定义的更新回调和类型标识
            PlayerLoopSystem newLoopSystem = new PlayerLoopSystem()
            {
                updateDelegate = updateFunction,
                type = flagType ?? loopType,
            };

            // 在当前 PlayerLoop 的子系统列表中查找指定的循环类型索引
            int needLoopIndex = GetLoopTypeIndex(_currentLoopSystem.subSystemList, loopType);

            // 通过 ref 直接操作对应的生命周期子系统
            ref PlayerLoopSystem needLoop = ref _currentLoopSystem.subSystemList[needLoopIndex];

            if (isLast)
            {
                // 向该生命周期的子系统列表中添加新的 PlayerLoopSystem
                ArrayListUtility.AddLast(ref needLoop.subSystemList, newLoopSystem);
            }
            else
            {
                // 向该生命周期的子系统列表中添加新的 PlayerLoopSystem
                ArrayListUtility.AddFirst(ref needLoop.subSystemList, newLoopSystem);
            }

            return _currentLoopSystem;
        }


        /// <summary>
        /// 从指定的 Unity 循环类型中删除特定的 PlayerLoopSystem（执行器），
        /// 通过标志类型精确定位要删除的子系统。
        /// </summary>
        /// <param name="loopType">生命周期循环类型，方便查找</param>
        /// <param name="flagType">标志类型，通常为插入时使用的类型，用于精准定位删除</param>
        /// <returns>返回更新后的完整 PlayerLoopSystem，调用者需要自行调用 PlayerLoop.SetPlayerLoop 以生效</returns>
        internal static PlayerLoopSystem RemovePlayerLoop(E_LoopType loopType, Type flagType)
        {
            return RemovePlayerLoop(SelectType(loopType), flagType);
        }

        /// <summary>
        /// 从指定的 Unity 循环类型中删除特定的 PlayerLoopSystem（执行器），
        /// 通过标志类型精确定位要删除的子系统。
        /// </summary>
        /// <param name="loopType">生命周期循环类型</param>
        /// <param name="flagType">标志类型，通常为插入时使用的类型，用于精准定位删除</param>
        /// <returns>返回更新后的完整 PlayerLoopSystem，调用者需要自行调用 PlayerLoop.SetPlayerLoop 以生效</returns>
        internal static PlayerLoopSystem RemovePlayerLoop(Type loopType, Type flagType)
        {
            // 查找生命周期循环类型的索引
            int needLoopIndex = GetLoopTypeIndex(_currentLoopSystem.subSystemList, loopType);
            ref var needLoopSystem = ref _currentLoopSystem.subSystemList[needLoopIndex];

            // 使用 LINQ 过滤掉匹配标志类型的子系统，剩余的子系统重新赋值
            needLoopSystem.subSystemList = needLoopSystem.subSystemList
                .Where(need => flagType == null ? need.type != loopType : need.type != flagType)
                .ToArray();

            return _currentLoopSystem;
        }

        /// <summary>
        /// 根据指定的循环类型，从 PlayerLoopSystem 数组中查找对应的索引。
        /// </summary>
        /// <param name="playerLoopSystems">PlayerLoopSystem 数组</param>
        /// <param name="type">要查找的循环类型</param>
        /// <returns>找到则返回索引，找不到返回 -1</returns>
        private static int GetLoopTypeIndex(PlayerLoopSystem[] playerLoopSystems, Type type)
        {
            for (int i = 0; i < playerLoopSystems.Length; i++)
            {
                ref PlayerLoopSystem playerLoopSystem = ref playerLoopSystems[i];
                if (playerLoopSystem.type == type)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 根据枚举的循环类型选择对应的 Unity 内置 PlayerLoopSystem 类型。
        /// </summary>
        /// <param name="loopType">循环类型枚举</param>
        /// <returns>对应的 Unity PlayerLoopSystem 类型</returns>
        /// <exception cref="NotSupportedException">不支持的循环类型会抛出异常</exception>
        private static Type SelectType(E_LoopType? loopType) => loopType switch
        {
            not null when loopType == E_LoopType.Initialization => typeof(Initialization),
            not null when loopType == E_LoopType.Update => typeof(Update),
            not null when loopType == E_LoopType.FixedUpdate => typeof(FixedUpdate),
            not null when loopType == E_LoopType.PreLateUpdate => typeof(PreLateUpdate),
            not null when loopType == E_LoopType.EndOfFrame => typeof(PostLateUpdate),
            _ => throw new NotSupportedException("其他类型还不支持")
        };
    }
}

