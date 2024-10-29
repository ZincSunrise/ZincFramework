using System;


namespace ZincFramework
{
    namespace Security
    {
        namespace CheckCode
        {
            public class Crc32 : Crc
            {
                private uint[] _normalTable;
                private uint[] _reversedTable;

                private uint crcInit = 0;

                public override int CheckCodeLength => 4;

                public void Initialize(uint ploy, uint crcInit, bool isReverse = false)
                {
                    this.crcInit = crcInit;
                    CreateNormalCrc32Table(ploy);
                    if (isReverse)
                    {
                        CreateReversedCrc32Table(ploy);
                    }
                }

                public void CreateNormalCrc32Table(uint ploy)
                {
                    uint data;
                    _normalTable = new uint[256];
                    int i, j;
                    for (i = 0; i < 256; i++)
                    {
                        data = (uint)(i << 24);
                        for (j = 0; j < 8; j++)
                        {
                            if ((data & 0x80000000) == 0x80000000)
                                data = Convert.ToUInt32((data << 1) ^ ploy);
                            else
                                data <<= 1;
                        }
                        _normalTable[i] = data;
                    }
                }

                public void CreateReversedCrc32Table(uint ploy)
                {
                    uint data;
                    _reversedTable = new uint[256];
                    for (int i = 0; i < 256; i++)
                    {
                        data = (uint)i;
                        for (int j = 0; j < 8; j++)
                        {
                            if ((data & 1) == 1)
                            {
                                data = Convert.ToUInt32((data >> 1) ^ ploy);
                            }
                            else
                            {
                                data >>= 1;
                            }
                        }
                        _reversedTable[i] = data;
                    }
                }

                public uint CalcNormalCrc32(byte[] bytes)
                {
                    uint init = crcInit;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        init = Convert.ToUInt32((uint)(init << 8) ^ _normalTable[((init >> 24) & 0xff) ^ bytes[i]]);
                    }
                    return init;
                }

                public uint CalcReversedCrc32(byte[] bytes)
                {
                    uint init = crcInit;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        init = Convert.ToUInt32((uint)(init >> 8) ^ _reversedTable[(init & 0xff) ^ bytes[i]]);
                    }
                    return init;
                }

                public override byte[] GenerateCode(byte[] bytes)
                {
                    uint init = crcInit;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        init = (init << 8) ^ _normalTable[((init >> 24) & 0xff) ^ bytes[i]];
                    }

                    byte[] crcBytes = BitConverter.GetBytes(init);
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
