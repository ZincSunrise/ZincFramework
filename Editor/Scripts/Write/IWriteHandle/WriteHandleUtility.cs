using System.Collections.Generic;
using ZincFramework.ScriptWriter.Exceptions;





namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public static class WriteHandleUtility
            {
                private readonly static Dictionary<string, int> _modifliers = new()
                {
                    {"public", 3},
                    {"protected", 2},
                    {"private", 1},
                };

                private static HashSet<string> _modifers;

                /// <summary>
                /// 左边必须比右边大
                /// </summary>
                /// <param name="modiflilerA"></param>
                /// <param name="modiflilerB"></param>
                /// <returns></returns>
                public static void Assert(string modiflilerA, string modiflilerB)
                {
                    if (_modifliers[modiflilerA] > _modifliers[modiflilerB])
                    {
                        throw new InvaildModifierException("你传入了一个过小的访问修饰符");
                    }
                }



                public static bool CheckInvalid(params string[] modifliers)
                {
                    _modifers = new HashSet<string>(modifliers);

                    if (_modifers.Contains("static") && (_modifers.Contains("abstract") || _modifers.Contains("override")))
                    {
                        return false;
                    }

                    if (_modifers.Contains("virtual") && _modifers.Contains("abstract") ||
                        _modifers.Contains("virtual") && _modifers.Contains("override") ||
                        _modifers.Contains("abstract") && _modifers.Contains("override"))
                    {
                        return false;
                    }

                    if (_modifers.Contains("sealed") && !_modifers.Contains("override"))
                    {
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}
