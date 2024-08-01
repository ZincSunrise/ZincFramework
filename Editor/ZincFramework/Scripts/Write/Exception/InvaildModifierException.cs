using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZincFramework
{
    namespace Writer
    {
        namespace Exceptions
        {
            public class InvaildModifierException : Exception
            {
                public override string Message => _message ?? $"�������η�֮����ڳ�ͻ������м��{string.Join(' ', _modifiers)}";

                private readonly string _message;

                private readonly string[] _modifiers;

                public InvaildModifierException(string message, params string[] modifiers)
                {
                    _message = message;
                    _modifiers = modifiers;
                }


                public InvaildModifierException(params string[] modifiers)
                {
                    _modifiers = modifiers;
                }
            }
        }
    }
}