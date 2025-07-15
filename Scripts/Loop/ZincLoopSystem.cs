using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using ZincFramework.Loop.Internal;


namespace ZincFramework.Loop
{
    /// <summary>
    /// ZincFramework 的主循环系统工具类，用于非 MonoBehaviour 类注册 Update/FixedUpdate/LateUpdate 回调，
    /// 并支持协程管理与模块注入到 Unity 的 PlayerLoop 系统。
    /// </summary>
    public static class ZincLoopSystem
    {
        #region 内部类型标记（用于 PlayerLoop 注入唯一识别）

        /// <summary> 标记用于 Update 阶段的 PlayerLoop 注入类型 </summary>
        internal struct ZincUpdate { }

        /// <summary> 标记用于 FixedUpdate 阶段的 PlayerLoop 注入类型 </summary>
        internal struct ZincFixedUpdate { }

        /// <summary> 标记用于 LateUpdate 阶段的 PlayerLoop 注入类型 </summary>
        internal struct ZincLateUpdate { }

        /// <summary> 标记用于 一帧最后 阶段的 PlayerLoop 注入类型 </summary>
        internal struct ZincEndOfFrame { }

        /// <summary>
        /// 根据枚举值选择对应的 Loop 标记类型（用于注入时识别）
        /// </summary>
        internal static Type SelectUpdateType(E_LoopType? loopType) => loopType switch
        {
            not null when loopType == E_LoopType.Update => typeof(ZincUpdate),
            not null when loopType == E_LoopType.FixedUpdate => typeof(ZincFixedUpdate),
            not null when loopType == E_LoopType.PreLateUpdate => typeof(ZincLateUpdate),
            not null when loopType == E_LoopType.EndOfFrame => typeof(ZincEndOfFrame),
            _ => throw new NotSupportedException("其他类型还不支持")
        };


        #endregion
        // 命名协程管理容器
        private static readonly Dictionary<string, CoroutineRunner> _coroutineObjects = new Dictionary<string, CoroutineRunner>();

        // 三个 MonoLoopModule 分别对应 Update、FixedUpdate、LateUpdate
        private static readonly MonoLoopModule _updateModules = new MonoLoopModule(E_LoopType.Update);

        private static readonly MonoLoopModule _fixedUpdateModules = new MonoLoopModule(E_LoopType.FixedUpdate);

        private static readonly MonoLoopModule _lateUpdateModules = new MonoLoopModule(E_LoopType.PreLateUpdate);

        #region 注入模块（程序启动后立即执行）
        /// <summary>
        /// 初始化并将内置的 Update、FixedUpdate、LateUpdate 模块注入到 Unity 的 PlayerLoop 中。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(loadType: RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Inject()
        {
            LoopSystemTool.AddPlayerLoop(_updateModules.Tick, _updateModules.LoopType, _updateModules.FlagType, true);
            LoopSystemTool.AddPlayerLoop(_fixedUpdateModules.Tick, _fixedUpdateModules.LoopType, _fixedUpdateModules.FlagType, true);
            var loopSystem = LoopSystemTool.AddPlayerLoop(_lateUpdateModules.Tick, _lateUpdateModules.LoopType, _lateUpdateModules.FlagType, true);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.EnteredEditMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    _updateModules.Tick();
                    _updateModules.Clear();

                    _fixedUpdateModules.Tick();
                    _fixedUpdateModules.Clear();

                    _lateUpdateModules.Tick();
                    _lateUpdateModules.Clear();
                }
            };
#endif
            PlayerLoop.SetPlayerLoop(loopSystem);
        }

        #endregion

        #region 动态模块注册/移除（扩展支持）


        /// <summary>
        /// 向指定的 Loop 阶段添加一个 ILoopModule 实例
        /// </summary>
        /// <typeparam name="TLoopModule"></typeparam>
        /// <param name="loopModule">ILoopModule 实例</param>
        /// <param name="isLast">是否把LoopModule的方法插入到循环的末端</param>
        public static void AddModule<TLoopModule>(TLoopModule loopModule, bool isLast = true) where TLoopModule : ILoopModule
        {
            var loopSystem = LoopSystemTool.AddPlayerLoop(loopModule.Tick, loopModule.LoopType, loopModule.FlagType, isLast);
            PlayerLoop.SetPlayerLoop(loopSystem);
        }

        /// <summary>
        /// 从指定的 Loop 阶段中移除一个 ILoopModule 实例
        /// </summary>
        public static void RemoveModule<TLoopModule>(TLoopModule loopModule) where TLoopModule : ILoopModule
        {
            var loopSystem = LoopSystemTool.RemovePlayerLoop(loopModule.LoopType, loopModule.FlagType);
            PlayerLoop.SetPlayerLoop(loopSystem);
        }

        public static void SetPlayerLoop()
        {
            LoopSystemTool.SetPlayerLoop();
        }
        #endregion

        #region IMonoObserver 注册与分发
        /// <summary>
        /// 注册一个监听器到 Update 阶段，每帧调用一次 <c>IMonoObserver.OnUpdate()</c>
        /// </summary>
        public static void AddUpdateListener<TMonoObserver>(TMonoObserver updateObserver) where TMonoObserver : IMonoObserver
        {
            _updateModules.Register(updateObserver);
        }

        /// <summary>
        /// 从 Update 阶段中移除一个已注册的监听器
        /// </summary>
        public static void RemoveUpdateListener<TMonoObserver>(TMonoObserver updateObserver) where TMonoObserver : IMonoObserver
        {
            _updateModules.Unregister(updateObserver);
        }

        /// <summary>
        /// 注册一个监听器到 FixedUpdate 阶段，每物理帧调用一次 <c>IMonoObserver.OnFixedUpdate()</c>
        /// </summary>
        public static void AddFixedUpdateObserver<TMonoObserver>(TMonoObserver fixedUpdateObserver) where TMonoObserver : IMonoObserver
        {
            _fixedUpdateModules.Register(fixedUpdateObserver);
        }

        /// <summary>
        /// 从 FixedUpdate 阶段中移除一个已注册的监听器
        /// </summary>
        public static void RemoveFixedUpdateObserver<TMonoObserver>(TMonoObserver fixedUpdateObserver) where TMonoObserver : IMonoObserver
        {
            _fixedUpdateModules.Unregister(fixedUpdateObserver);
        }

        /// <summary>
        /// 注册一个监听器到 LateUpdate 阶段，每帧调用一次 <c>IMonoObserver.OnLateUpdate()</c>
        /// </summary>
        public static void AddLateUpdateListener<TMonoObserver>(TMonoObserver lateUpdateObserver) where TMonoObserver : IMonoObserver
        {
            _lateUpdateModules.Register(lateUpdateObserver);
        }

        /// <summary>
        /// 从 LateUpdate 阶段中移除一个已注册的监听器
        /// </summary>
        public static void RemoveLateUpdateListener<TMonoObserver>(TMonoObserver lateUpdateObserver) where TMonoObserver : IMonoObserver
        {
            _lateUpdateModules.Unregister(lateUpdateObserver);
        }
        #endregion


        #region 命名式协程控制器（适用于工具型逻辑）
        /// <summary>
        /// 启动一个协程，按指定名字创建并缓存运行容器（GameObject + MonoBehaviour）
        /// </summary>
        /// <param name="name">协程标识名，重复调用同名将复用旧容器</param>
        /// <param name="enumerator">协程枚举器</param>
        /// <returns>返回 Unity 的 Coroutine 句柄</returns>
        public static Coroutine StartCoroutine(string name, IEnumerator enumerator)
        {
            if (!_coroutineObjects.TryGetValue(name, out var coroutineRunner))
            {
                GameObject gameObject = new GameObject(name);
                coroutineRunner = gameObject.AddComponent<CoroutineRunner>();
                _coroutineObjects.Add(name, coroutineRunner);
            }

            return coroutineRunner.StartCoroutine(enumerator);
        }

        /// <summary>
        /// 停止一个正在运行的命名协程，并销毁其运行容器
        /// </summary>
        /// <param name="name">协程标识名</param>
        public static void StopCoroutine(string name)
        {
            if (_coroutineObjects.TryGetValue(name, out var coroutineRunner))
            {
                coroutineRunner.StopAllCoroutines();
                _coroutineObjects.Remove(name);
                GameObject.Destroy(coroutineRunner.gameObject);
            }
            else
            {
                Debug.LogWarning($"名字为{name}的协程不存在");
            }
        }
        #endregion
    }

}
