using System;

namespace ZincFramework
{
    namespace Security
    {
        namespace CheckCode
        {

            public class Crc16 : Crc
            {
                private ushort[] _normalTable;
                private ushort[] _reversedTable;

                private ushort _crcInit = 0;

                public override int CheckCodeLength => 2;

                public void Initialize(ushort ploy, ushort crcInit, bool isReverse = false)
                {
                    this._crcInit = crcInit;
                    CreateNormalCrc16Table(ploy);
                    if (isReverse)
                    {
                        CreateReversedCrc16Table(ploy);
                    }
                }


                private void CreateNormalCrc16Table(ushort ploy)
                {
                    ushort data;
                    _normalTable = new ushort[256];
                    int i, j;
                    for (i = 0; i < 256; i++)
                    {
                        data = (ushort)(i << 8);
                        for (j = 0; j < 8; j++)
                        {
                            if ((data & 0x8000) == 0x8000)
                            {
                                data = Convert.ToUInt16((ushort)(data << 1) ^ ploy);
                            }

                            else
                            {
                                data <<= 1;
                            }
                        }
                        _normalTable[i] = data;
                    }
                }

                private void CreateReversedCrc16Table(ushort ploy)
                {
                    ushort data;
                    _reversedTable = new ushort[256];
                    int i, j;
                    for (i = 0; i < 256; i++)
                    {
                        data = (ushort)i;
                        for (j = 0; j < 8; j++)
                        {
                            if ((data & 1) == 1)
                                data = Convert.ToUInt16((ushort)(data >> 1) ^ ploy);
                            else
                                data >>= 1;
                        }
                        _reversedTable[i] = data;
                    }
                }

                public ushort GetCrcCode(byte[] bytes)
                {
                    ushort init = _crcInit;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        init = Convert.ToUInt16((ushort)(init << 8) ^ _normalTable[((init >> 8) & 0xff) ^ bytes[i]]);
                    }
                    return init;
                }

                public ushort GetReversedCrcCode(byte[] bytes)
                {
                    ushort init = _crcInit;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        init = Convert.ToUInt16((ushort)(init >> 8) ^ _reversedTable[(init & 0xff) ^ bytes[i]]);
                    }
                    return init;
                }


                public override byte[] GenerateCode(byte[] bytes)
                {
                    byte[] crcBytes = BitConverter.GetBytes(GetCrcCode(bytes));
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(crcBytes);
                    }
                    return crcBytes;
                }
            }
        }
    }
}

