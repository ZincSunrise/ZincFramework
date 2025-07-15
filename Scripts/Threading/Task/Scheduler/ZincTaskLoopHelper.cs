using System;
using System.Threading;
using UnityEditor;
using UnityEngine;
using ZincFramework.Loop;
using ZincFramework.Loop.Internal;
using ZincFramework.Threading.Tasks.Internal;

namespace ZincFramework.Threading.Tasks
{
    public static class ZincTaskLoopHelper
    {
        #region 插入的特殊类型
        internal struct ZincYieldIntialize { }

        internal struct ZincYieldUpdate { }

        internal struct ZincYieldFixedUpdate { }

        internal struct ZincYieldPreLateUpdate { }

        internal struct ZincYieldLastPostLateUpdate { }

        private static Type SelectYieldType(E_LoopType? loopType) => loopType switch
        {
            not null when loopType == E_LoopType.Initialization => typeof(ZincYieldIntialize),
            not null when loopType == E_LoopType.Update => typeof(ZincYieldUpdate),
            not null when loopType == E_LoopType.FixedUpdate => typeof(ZincYieldFixedUpdate),
            not null when loopType == E_LoopType.PreLateUpdate => typeof(ZincYieldPreLateUpdate),
            not null when loopType == E_LoopType.EndOfFrame => typeof(ZincYieldLastPostLateUpdate),
            _ => throw new NotSupportedException("其他类型还不支持")
        };
        #endregion
        public static int MainThreadId { get; private set; }


        public static SynchronizationContext UnitySynchronizationContext => _unitySynchronizationContext;

        private static SynchronizationContext _unitySynchronizationContext;

        private static YieldContinuationModule[] _continuationModules;

        private static TimeRunnerModule[] _timeRunnerModules;
#if UNITY_EDITOR
        public static bool IsEditorExiting { get; private set; }
#endif

        [RuntimeInitializeOnLoadMethod(loadType: RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            MainThreadId = Thread.CurrentThread.ManagedThreadId;
            _unitySynchronizationContext = SynchronizationContext.Current;

            _continuationModules = new YieldContinuationModule[5];
            _timeRunnerModules = new TimeRunnerModule[5];

            ILoopModule[] carrier = new ILoopModule[2];

            for (int i = 0; i < _continuationModules.Length; i++)
            {
                InsertLoop((E_LoopType)i, carrier);
            }

            LoopSystemTool.SetPlayerLoop();
        }


        private static void InsertLoop(E_LoopType loopType, ILoopModule[] carrier)
        {
            int index = (int)loopType;
            YieldContinuationModule continuationModule = new YieldContinuationModule(loopType, SelectYieldType(loopType));
            TimeRunnerModule timeRunnerModule = new TimeRunnerModule(loopType, SelectYieldType(loopType));
            _continuationModules[index] = continuationModule;
            _timeRunnerModules[index] = timeRunnerModule;

            carrier[0] = timeRunnerModule;
            carrier[1] = continuationModule;

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.ExitingEditMode)
                {
                    IsEditorExiting = true;
                    // run rest action before clear.
                    if (continuationModule != null)
                    {
                        continuationModule.Tick();
                        continuationModule.Clear();
                    }
                    if (timeRunnerModule != null)
                    {
                        timeRunnerModule.Tick();
                        timeRunnerModule.Clear();
                    }

                    IsEditorExiting = false;
                }
            };
#endif
            LoopSystemTool.AddSameLoopTypePlayerLoops(carrier, loopType, loopType == E_LoopType.EndOfFrame);
        }


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void InitOnEditor()
        {
            Initialize();

            EditorApplication.update += ForceEditorPlayerLoopUpdate;
        }

        private static void ForceEditorPlayerLoopUpdate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }


            if (_continuationModules != null)
            {
                foreach (var item in _continuationModules)
                {
                    item?.Tick();
                }
            }

            if (_timeRunnerModules != null)
            {
                foreach (var item in _timeRunnerModules)
                {
                    item?.Tick();
                }
            }
        }
#endif


        /// <summary>
        /// 添加Yield的延续方法的方法
        /// </summary>
        /// <param name="loopType"></param>
        public static void AddContinuation(E_LoopType loopType, Action continuation)
        {
            _continuationModules[(int)loopType].Register(continuation);
        }

        /// <summary>
        /// 添加LoopItem的方法
        /// </summary>
        /// <param name="loopType"></param>
        /// <param name="loopItem"></param>
        public static void AddLoopItem(E_LoopType loopType, ILoopItem loopItem)
        {
            _timeRunnerModules[(int)loopType].Register(loopItem);
        }
    }
}
