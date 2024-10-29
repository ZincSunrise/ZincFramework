using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public interface IHandleMessage
            {
                BaseMessage Message { get; set; }

                void HandleMessage();
            }
        }
    }
}
