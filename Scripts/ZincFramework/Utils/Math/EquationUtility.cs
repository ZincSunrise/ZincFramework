using UnityEngine;

namespace ZincFramework
{
    namespace Math
    {
        public static class EquationUtility
        {
            /// <summary>
            /// ����ax^2 + bx + c = rightValue�ķ�ʽ��һԪ���η���
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <param name="rightValue"></param>
            /// <returns></returns>
            public static float[] SoluteUnivariateQuadraticEquation(float a, float b, float c, float rightValue = 0) => (b * b - 4 * a * (c - rightValue)) switch
            {
                > 0 when (b * b - 4 * a * (c - rightValue)) is float delta => new float[2] { (-b + Mathf.Sqrt(delta)) / (2 * a), (-b - Mathf.Sqrt(delta)) / (2 * a) },
                0 => new float[1] { -b / (2 * a) },

                < 0 => throw new System.ArgumentException("��������ô��������Сѧ���������᣿�����ȥ����"),

                _ => throw new System.ArgumentException("����ô����һ��������ѧ����������ˣ�")
            };

            /// <summary>
            /// ������ʽ����һԪ���η���
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <param name="rightValue"></param>
            /// <returns></returns>
            public static System.Numerics.Complex[] SoluteUnivariateQuadraticEquationComplex(float a, float b, float c, float rightValue = 0) => (b * b - 4 * a * (c - rightValue)) switch
            {
                > 0 when (b * b - 4 * a * (c - rightValue)) is float delta => new System.Numerics.Complex[2] { (-b + Mathf.Sqrt(delta)) / (2 * a), (-b - Mathf.Sqrt(delta)) / (2 * a) },
                0 => new System.Numerics.Complex[1] { -b / (2 * a) },
                < 0 when -(b * b - 4 * a * (c - rightValue)) is float delta => new System.Numerics.Complex[2] { new System.Numerics.Complex(-b / (2 * a), Mathf.Sqrt(delta) / (2 * a)), new System.Numerics.Complex(-b / (2 * a), -Mathf.Sqrt(delta) / (2 * a)) },
                _ => throw new System.ArgumentException("����ô����һ��������ѧ����������ˣ�"),
            };


            public static float[] SoluteSystemLinearEquations()
            {
                return null;
            }
        }
    }
}
