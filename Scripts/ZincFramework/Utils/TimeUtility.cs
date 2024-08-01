using System;
using System.Diagnostics;


namespace ZincFramework
{
    /// <summary>
    /// ����TimeUtility�඼���ܵ�Unity��Time.timeScaleӰ��
    /// </summary>
    public class TimeUtility
    {
        public static readonly DateTime UTCFirstYear = new DateTime(1970, 1, 1);

        /// <summary>
        /// �洢ʱ����֮ǰ���е�ת������,�õ�����һ��ʱ���
        /// </summary>
        /// <param name="day">�������ڶ�����</param>
        /// <param name="hour">�������ڶ���ʱ</param>
        /// <param name="minute">�������ڶ��ٷ���</param>
        /// <param name="second">�������ڶ�����</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day = 0, int hour = 0,int minute = 0,int second = 0)
        {
            DateTime achievementDate = DateTime.Now + new TimeSpan(day, hour, minute, second);

            TimeSpan timeSpan = achievementDate - UTCFirstYear;
            //�õ�ʱ����������
            int timeStamp = (int)(timeSpan.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// �洢ʱ����֮ǰ���е�ת������,�õ�����һ��ʱ���
        /// </summary>
        /// <param name="day">�������ڶ�����</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day) - UTCFirstYear;
            //�õ�ʱ����������

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// �洢ʱ����֮ǰ���е�ת������,�õ�����һ��ʱ���
        /// </summary>
        /// <param name="day">�������ڶ�����</param>
        /// <param name="hour">�������ڶ���ʱ</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day,int hour)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour) - UTCFirstYear;
            //�õ�ʱ����������

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// �洢ʱ����֮ǰ���е�ת������,�õ�����һ��ʱ���
        /// </summary>
        /// <param name="day">�������ڶ�����</param>
        /// <param name="hour">�������ڶ���ʱ</param>
        /// <param name="minute">�������ڶ��ٷ���</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day, int hour, int minute)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour).AddMinutes(minute) - UTCFirstYear;
            //�õ�ʱ����������

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }


        /// <summary>
        /// �洢ʱ����֮ǰ���е�ת������,�õ�����һ��ʱ���(ʹ�õ����й��Ŵ��ļ�ʱ��ʽ)
        /// </summary>
        /// <param name="day">�������ڶ�����</param>
        /// <param name="hour">�������ڶ���ʱ��</param>
        /// <param name="quarter">�������ڶ��ٿ�</param>
        /// <returns></returns>
        public static int GetTimeOffsetAnciently(int day, int hour, int quarter)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour * 2).AddMinutes(quarter * 15) - UTCFirstYear;
            //�õ�ʱ����������

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }


        /// <summary>
        /// ����Ƿ�ﵽʱ��,��Ҫ����һ��ʱ���,��Ҫ�洢ʱ��֮����ʹ��
        /// </summary>
        /// <param name="timeStamp">�����ʱ���, ��GetTimeOffset�еõ�</param>
        /// <returns></returns>
        public static bool IsReachedTime(int timeStamp)
        {
            //�õ�ʱ����������
            TimeSpan timeSpan = new TimeSpan(timeStamp * TimeSpan.TicksPerSecond);
            DateTime achievement = UTCFirstYear + timeSpan;

            return (achievement - DateTime.Now).TotalSeconds <= 0;
        }


        /// <summary>
        /// �õ�����ʱ������ж�����
        /// </summary>
        /// <param name="timeStamp">�����ʱ���</param>
        /// <returns></returns>
        public static int GetCountdown(int timeStamp)
        {
            //�õ�ʱ����������
            TimeSpan timeSpan = new TimeSpan(timeStamp * TimeSpan.TicksPerSecond);
            DateTime achievement = UTCFirstYear + timeSpan;

            int countdown = (int)(achievement - DateTime.Now).TotalSeconds;
            if(countdown < 0)
            {
                countdown = 0;
            }
            return countdown;
        }

        /// <summary>
        /// �õ���ǰʱ��ļ���
        /// </summary>
        /// <param name="dateTime">���������</param>
        /// <returns></returns>
        public static string GetSeaSon(DateTime dateTime) => dateTime.Month switch
        {
            >= 3 and <= 5 => "����",
            >= 6 and <= 8 => "�ļ�",
            >= 9 and <= 11 => "�＾",
            12 or 1 or 2 => "����",
            _ => throw new ArgumentException("��������ڴ���,�㲻��������,��Ҫ����һ��С��1���ߴ���12������"),
        };

        /// <summary>
        /// ��ȡ�������е�ʱ��
        /// </summary>
        /// <returns></returns>
        public static long GetFunctionTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();

            return stopwatch.ElapsedTicks;
        }
    }
}
