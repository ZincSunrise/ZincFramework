using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace Security
    {
        namespace CheckCode
        {
            public static class CrcFactory
            {
                private static readonly Dictionary<string, Crc> _crcDic = new Dictionary<string, Crc>();

                /// <summary>
                /// 16位一般选择CCITT，网络通信则使用XMODEM， 32位一般使用标准，对错误检测能力要求高则使用Castagnoli
                /// </summary>
                /// <param name="crcType"></param>
                /// <param name="isReverse"></param>
                /// <returns></returns>
                public static Crc Create(E_Crc_Type crcType, bool isReverse = false)
                {
                    string name = crcType.ToString();
                    if (!_crcDic.TryGetValue(name, out Crc crc))
                    {
                        switch (crcType)
                        {
                            case E_Crc_Type.CCITT_16:
                                crc = new Crc16();
                                (crc as Crc16).Initialize(0x1021, 0xFFFF, isReverse);
                                break;
                            case E_Crc_Type.MODBUS_16:
                                crc = new Crc16();
                                (crc as Crc16).Initialize(0x8005, 0xFFFF, isReverse);
                                break;
                            case E_Crc_Type.XMODEM_16:
                                crc = new Crc16();
                                (crc as Crc16).Initialize(0x1021, 0x0000, isReverse);
                                break;
                            case E_Crc_Type.Standard_32:
                                crc = new Crc32();
                                (crc as Crc32).Initialize(0x04C11DB7, 0xFFFFFFFF, isReverse);
                                break;
                            case E_Crc_Type.Castagnoli_32:
                                crc = new Crc32();
                                (crc as Crc32).Initialize(0x1EDC6F41, 0xFFFFFFFF, isReverse);
                                break;
                            case E_Crc_Type.Koopman_32:
                                crc = new Crc32();
                                (crc as Crc32).Initialize(0x741B8CD7, 0xFFFFFFFF, isReverse);
                                break;
                            default:
                                return null;
                        }
                        _crcDic.Add(name, crc);
                    }
                    return crc;
                }
            }
        }
    }
}

