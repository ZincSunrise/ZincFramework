using UnityEngine;


namespace ZincFramework
{
    public static class MathUtility
    {
        private readonly static int[] _primes = new int[] {
                                3, 7, 11, 17, 23, 29, 37, 47, 59, 71,
                                89, 107, 131, 163, 197, 239, 293, 353, 431, 521,
                                631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371,
                                4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
                                25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363,
                                156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403,
                                968897, 1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559,
                                5999471, 7199369
        };

        #region 数字计算相关
        public static int GetGcd(int a, int b)
        {
            return b == 0 ? a : GetGcd(b, a % b);
        }

        public static int GetLcm(int a, int b)
        {
            return a * b / GetGcd(a, b);
        }

        public static long GetGcd(long a, long b)
        {
            return b == 0 ? a : GetGcd(b, a % b);
        }

        public static long GetLcm(long a, long b)
        {
            return a * b / GetGcd(a, b);
        }

        public static bool IsPrimeNumber(int number)
        {
            if (number <= 3)
            {
                return number > 1;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            for (int i = 0; i < _primes.Length; i++)
            {
                if (number == _primes[i])
                {
                    return true;
                }
            }

            float sqrtNumber = Mathf.Sqrt(number);
            for (int i = 3; i <= sqrtNumber; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsPrimeNumber(long number)
        {
            if (number <= 3)
            {
                return number > 1;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            for (int i = 0; i < _primes.Length; i++)
            {
                if (number == _primes[i])
                {
                    return true;
                }
            }

            float sqrtNumber = Mathf.Sqrt(number);
            for (long i = 3; i <= sqrtNumber; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static long FastPower(long number, long power, long mod)
        {
            long result = 1;
            while(power > 0)
            {
                if ((power & 1) == 1)
                {
                    power -= 1;
                    result = result * number % mod;
                }
                number = number * number % mod;
                power >>= 1;
            }
            return result;
        }

        public static long ExtentGcd(long number, long mod, ref long x, ref long y)
        {
            if(mod == 0)
            {
                x = 1;
                y = 0;
                return number;
            }

            long result = ExtentGcd(mod, number % mod, ref y, ref x);
            y -= number / mod * x;
            return result;
        }

        public static long GetInverse(long a, long mod)
        {
            long x = 0; 
            long y = 0;
            long result = ExtentGcd(a, mod, ref x, ref y);

            return result == 1 ? (x % mod + mod) % mod : -1;
        }
        #endregion

        #region 空间计算相关
        public static float RadianToDegree(float radian)
        {
            return radian * Mathf.Rad2Deg;
        }

        public static float DegreeToRadian(float degree)
        {
            return degree * Mathf.Deg2Rad;
        }

        public static float DistanceXZ(Vector3 a, Vector3 b)
        {
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
        }

        public static float DistanceXY(Vector3 a, Vector3 b)
        {
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        }


        /// <summary>
        /// 检测是否在摄像机范围内
        /// </summary>
        /// <param name="postion">物体位置</param>
        /// <param name="camera">摄像机</param>
        /// <returns></returns>
        public static bool CheckInScreen(Vector3 postion, Camera camera)
        {
            UnityEngine.Vector3 screenPoint = camera.WorldToViewportPoint(postion);
            return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
        }

        /// <summary>
        /// 检测是否在主摄像机范围内
        /// </summary>
        /// <param name="postion">物体位置</param>
        /// <returns></returns>
        public static bool CheckInMainCameraScreen(Vector3 postion)
        {
            return CheckInScreen(postion, Camera.main);
        }

        /// <summary>
        /// 检测XZ平面扇形范围内中是否存在对应物体
        /// </summary>
        /// <param name="position">物体位置</param>
        /// <param name="forward">物体面朝向</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static bool CheckInSectorRange(Vector3 position, Vector3 forward, Vector3 targetPosition, float radius, float angle)
        {
            return DistanceXZ(position, targetPosition) <= radius && Vector3.Angle(forward, targetPosition - position) <= angle / 2;
        }

        /// <summary>
        /// 将XY平面的向量转换为XZ平面
        /// </summary>
        /// <returns></returns>
        public static Vector3 XYToXZVector(Vector2 XYVector)
        {
            return new Vector3(XYVector.x, 0 ,XYVector.y);
        }

        public static void Rotate(this Transform transform, float rotateSpeed, Vector3 axis)
        {
            transform.rotation *= Quaternion.AngleAxis(rotateSpeed, axis);
        }

        #endregion
    }
}

