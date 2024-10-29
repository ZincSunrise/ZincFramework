using UnityEngine;
using ZincFramework;


namespace ZincFramework
{
    public static class ColorTool
    {
        public static readonly float[,] sRGBToXYZ =
        {
        { 0.4124564f, 0.3575761f, 0.1804375f },
        { 0.2126729f, 0.7151522f, 0.0721750f },
        { 0.0193339f, 0.1191920f, 0.9503041f }
    };

        // XYZ到Lab的转换常量  
        public const float Xn = 0.95047f;
        public const float Yn = 1.0f;
        public const float Zn = 1.08883f;


        public static Color GetRandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }


        public static float CalculateDifference(Color color1, Color color2)
        {
            if (color1 == color2)
            {
                return 0;
            }

            RGBToLab(color1, out var L1, out var a1, out var b1);
            RGBToLab(color2, out var L2, out var a2, out var b2);

            //参考《现代颜色技术原理及应用》P88数据

            //------------------------------------------
            float mean_Cab = (GetChroma(a1, b1) + GetChroma(a2, b2)) / 2;    //两个样品彩度的算术平均值
            float mean_Cab_pow7 = Mathf.Pow(mean_Cab, 7);       //两彩度平均值的7次方
            float G = 0.5f * (1 - Mathf.Pow(mean_Cab_pow7 / (mean_Cab_pow7 + Mathf.Pow(25, 7)), 0.5f));  //G表示CIELab 颜色空间a轴的调整因子,是彩度的函数.

            //声明L' a' b' （1,2）
            float LL1 = L1;
            float aa1 = a1 * (1 + G);
            float bb1 = b1;

            float LL2 = L2;
            float aa2 = a2 * (1 + G);
            float bb2 = b2;

            float CC1 = GetChroma(aa1, bb1); //两样本的彩度值
            float CC2 = GetChroma(aa2, bb2);

            float hh1 = GetHueAngle(aa1, bb1);//两样本的色调角
            float hh2 = GetHueAngle(aa2, bb2);

            float delta_LL = LL1 - LL2;
            float delta_CC = CC1 - CC2;
            float delta_hh = hh1 - hh2;
            float delta_HH = 2 * Mathf.Sin(Mathf.PI * delta_hh / 360) * Mathf.Sqrt(CC1 * CC2);

            //-------第三步--------------
            //计算公式中的加权函数SL,SC,SH,T
            float mean_LL = (LL1 + LL2) / 2;
            float mean_CC = (CC1 + CC2) / 2;
            float mean_hh = (hh1 + hh2) / 2;

            float SL = 1 + 0.015f * Mathf.Pow(mean_LL - 50, 2) / Mathf.Pow(20 + Mathf.Pow(mean_LL - 50, 2), 0.5f);
            float SC = 1 + 0.045f * mean_CC;
            float T = 1 - 0.17f * Mathf.Cos((mean_hh - 30) * Mathf.PI / 180) + 0.24f * Mathf.Cos((2 * mean_hh) * Mathf.PI / 180)
                  + 0.32f * Mathf.Cos((3 * mean_hh + 6) * Mathf.PI / 180) - 0.2f * Mathf.Cos((4 * mean_hh - 63) * Mathf.PI / 180);
            float SH = 1 + 0.015f * mean_CC * T;

            //------第四步--------
            //计算公式中的RT
            float mean_CC_pow7 = Mathf.Pow(mean_CC, 7);
            float RC = 2 * Mathf.Pow(mean_CC_pow7 / (mean_CC_pow7 + Mathf.Pow(25, 7)), 0.5f);
            float delta_xita = 30 * -Mathf.Exp(-Mathf.Pow((mean_hh - 275) / 25, 2));        //△θ 以°为单位

            float RT = -Mathf.Sin((2 * delta_xita) * Mathf.PI / 180) * RC;
            //旋转函数RT  

            float L_item = delta_LL / SL;
            float C_item = delta_CC / SC;
            float H_item = delta_HH / SH;

            float E00 = Mathf.Pow(L_item * L_item + C_item * C_item + H_item * H_item + RT * C_item * H_item, 0.5f);

            return E00;
        }

        public static float CalculateDifference2(Color color1, Color color2)
        {
            if (color1 == color2)
            {
                return 0;
            }

            RGBToLab(color1, out var L1, out var a1, out var b1);
            RGBToLab(color2, out var L2, out var a2, out var b2);

            float C1 = GetChroma(a1, b1);
            float C2 = GetChroma(a2, b2);

            float CAverage = (C1 + C2) / 2;
            float CPow7 = Mathf.Pow(CAverage, 7);

            float CSqrtPow7 = Mathf.Sqrt(CPow7 / (CPow7 + Mathf.Pow(5, 7)));
            float G = 0.5f * (1 - CSqrtPow7);

            float aa1 = (1 + G) * a1;
            float aa2 = (1 + G) * a2;

            float cc1 = GetChroma(aa1, b1);
            float cc2 = GetChroma(aa2, b2);

            float CcAverage = (cc1 + cc2) / 2;

            float h1 = Mathf.Atan2(b1, aa1);
            float h2 = Mathf.Atan2(b1, aa1);

            float deltaHAverge = (h1 - h2) % (2 * Mathf.PI);
            float hAverage = (h1 + h2) / 2;

            float T = 1 - 0.17f * Mathf.Cos(hAverage - Mathf.PI / 6) + 0.24f * Mathf.Cos(2 * hAverage) + 0.32f * Mathf.Cos(3 * hAverage + MathUtility.DegreeToRadian(6)) - 0.2f * Mathf.Cos(4 * hAverage - MathUtility.DegreeToRadian(63));

            float deltaL = L2 - L1;
            float deltaC = cc2 - cc1;
            float deltaH = 2 * Mathf.Sqrt(cc1 * cc2) * Mathf.Sin(deltaHAverge / 2);

            float LAverage = (L2 + L1) / 2;

            float SL = (1 + 0.015f * Mathf.Pow(LAverage - 50, 2)) / Mathf.Sqrt(20 + Mathf.Pow(LAverage - 50, 2));
            float SC = 1 + 0.045f * CcAverage;
            float SH = 1 + 0.015f * CcAverage * T;

            float point = -Mathf.Pow((hAverage - 275) / 25, 2);
            float RT = -2 * Mathf.Sqrt(CSqrtPow7) * Mathf.Sin(Mathf.PI / 3 * Mathf.Exp(point));

            float divideC = deltaC / SC;
            float divedeH = deltaH / SH;

            float SSL = Mathf.Pow(deltaL / SL, 2);
            float SSC = Mathf.Pow(divideC, 2);
            float SSH = Mathf.Pow(divedeH, 2);
            float RHC = RT * divideC * divedeH;
            float E00 = Mathf.Sqrt(SSL + SSC + SSH + RHC);
            return E00;
        }

        public static float CalculateDifference(float L1, float a1, float b1, float L2, float a2, float b2)
        {
            //参考《现代颜色技术原理及应用》P88数据

            //------------------------------------------
            float mean_Cab = (GetChroma(a1, b1) + GetChroma(a2, b2)) / 2;    //两个样品彩度的算术平均值
            float mean_Cab_pow7 = Mathf.Pow(mean_Cab, 7);       //两彩度平均值的7次方
            float G = 0.5f * (1 - Mathf.Pow(mean_Cab_pow7 / (mean_Cab_pow7 + Mathf.Pow(25, 7)), 0.5f));  //G表示CIELab 颜色空间a轴的调整因子,是彩度的函数.

            //声明L' a' b' （1,2）
            float LL1 = L1;
            float aa1 = a1 * (1 + G);
            float bb1 = b1;

            float LL2 = L2;
            float aa2 = a2 * (1 + G);
            float bb2 = b2;

            float CC1 = GetChroma(aa1, bb1); //两样本的彩度值
            float CC2 = GetChroma(aa2, bb2);

            float hh1 = Mathf.Abs(aa1 - bb1) % (2 * Mathf.PI);
            float hh2 = Mathf.Abs(aa2 - bb2) % (2 * Mathf.PI);

            float delta_LL = LL1 - LL2;
            float delta_CC = CC1 - CC2;
            float delta_hh = hh1 - hh2;
            float delta_HH = 2 * Mathf.Sin(Mathf.PI * delta_hh / 360) * Mathf.Sqrt(CC1 * CC2);

            //-------第三步--------------
            //计算公式中的加权函数SL,SC,SH,T
            float mean_LL = (LL1 + LL2) / 2;
            float mean_CC = (CC1 + CC2) / 2;
            float mean_hh = (hh1 + hh2) / 2;

            float SL = 1 + 0.015f * Mathf.Pow(mean_LL - 50, 2) / Mathf.Pow(20 + Mathf.Pow(mean_LL - 50, 2), 0.5f);
            float SC = 1 + 0.045f * mean_CC;
            float T = 1 - 0.17f * Mathf.Cos((mean_hh - 30) * Mathf.PI / 180) + 0.24f * Mathf.Cos((2 * mean_hh) * Mathf.PI / 180)
                  + 0.32f * Mathf.Cos((3 * mean_hh + 6) * Mathf.PI / 180) - 0.2f * Mathf.Cos((4 * mean_hh - 63) * Mathf.PI / 180);
            float SH = 1 + 0.015f * mean_CC * T;

            //------第四步--------
            //计算公式中的RT
            float mean_CC_pow7 = Mathf.Pow(mean_CC, 7);
            float RC = 2 * Mathf.Pow(mean_CC_pow7 / (mean_CC_pow7 + Mathf.Pow(25, 7)), 0.5f);
            float delta_xita = 30 * -Mathf.Exp(-Mathf.Pow((mean_hh - 275) / 25, 2));        //△θ 以°为单位

            float RT = -Mathf.Sin(2 * delta_xita * Mathf.PI / 180) * RC;
            //旋转函数RT  

            float L_item = delta_LL / SL;
            float C_item = delta_CC / SC;
            float H_item = delta_HH / SH;

            float E00 = Mathf.Pow(L_item * L_item + C_item * C_item + H_item * H_item + RT * C_item * H_item, 0.5f);

            return E00;
        }


        public static Color HSVtoRGB(float h, float s, float v)
        {
            float r = 0, g = 0, b = 0;

            // 如果饱和度为0，则为灰色，所有RGB值都相同
            if (s == 0)
            {
                r = g = b = v; // 灰色
            }
            else
            {
                // 色调在六个区段中的位置
                float sector = h / 60.0f;
                int i = Mathf.FloorToInt(sector); // 取得整数部分（对应区段）
                float f = sector - i; // 小数部分

                // 辅助变量，用于计算RGB分量
                float p = v * (1 - s);
                float q = v * (1 - s * f);
                float t = v * (1 - s * (1 - f));

                // 根据i的值确定所在的色相区间，并计算RGB
                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;
                    default: // case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }

            // 返回颜色（RGB值在0到1之间）
            return new Color(r, g, b);
        }

        // RGB到Lab的转换函数  
        public static void RGBToLab(Color rgb, out float L, out float a, out float b)
        {
            // 应用逆伽马校正  
            float R = rgb.linear.r;
            float G = rgb.linear.g;
            float B = rgb.linear.b;

            // RGB到XYZ  
            float X = sRGBToXYZ[0, 0] * R + sRGBToXYZ[0, 1] * G + sRGBToXYZ[0, 2] * B;
            float Y = sRGBToXYZ[1, 0] * R + sRGBToXYZ[1, 1] * G + sRGBToXYZ[1, 2] * B;
            float Z = sRGBToXYZ[2, 0] * R + sRGBToXYZ[2, 1] * G + sRGBToXYZ[2, 2] * B;

            // 归一化XYZ  
            X /= Xn;
            Y /= Yn;
            Z /= Zn;

            // XYZ到Lab  
            float fx = XYZToLab(X);
            float fy = XYZToLab(Y);
            float fz = XYZToLab(Z);

            L = 116.0f * fy - 16.0f;
            L = L < 0 ? 0 : L;
            a = 500.0f * (fx - fy);
            b = 200.0f * (fy - fz);
        }

        // XYZ到Lab的转换函数  
        private static float XYZToLab(float t)
        {
            if (t > 0.008856f)
                return Mathf.Pow(t, 1.0f / 3.0f);
            else
                return 7.787f * t + 0.137931f;
        }

        private static float GetChroma(float a, float b)
        {
            return (float)System.Math.Sqrt(a * a + b * b);
        }

        public static float GetHueAngle(float a, float b)
        {
            float h = 0;
            float hab = 0;

            //此处改进由于纯黑色的a为0，所以而除数不能为0，，除0意味着除以无限接近0，所以结果为无限大(负数为无限小)，所以反正切的值为+90°或-90°(270°)，经测试无论正负对结果无影响。
            if (a == 0)
            {
                return 90;
            }

            h = (180 / Mathf.PI) * Mathf.Atan(b / a); //有正有负

            if (a > 0 && b > 0)
            {
                hab = h;
            }
            else if (a < 0 && b > 0)
            {
                hab = 180 + h;
            }
            else if (a < 0 && b < 0)
            {
                hab = 180 + h;
            }
            else
            {
                //a>0&&b<0
                hab = 360 + h;
            }

            return hab;
        }
    }
}

