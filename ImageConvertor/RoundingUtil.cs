namespace ImageConvertor
{
    /// <summary>
    /// 丸め関連の処理
    /// </summary>
    internal class RoundingUtil
    {
        /// <summary>
        /// 0～255に丸める
        /// </summary>
        public static byte ToByte(int num)
        {
            return (byte)Limit(num, byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// 0～255に丸める
        /// </summary>
        public static byte ToByte(float num)
        {
            return ToByte((int)num);
        }

        /// <summary>
        /// 指定範囲内に値を丸める
        /// </summary>
        public static T Limit<T>(T num, T min, T max) where T : IComparable
        {
            if (num.CompareTo(max) > 0)
            {
                return max;
            }
            else if (num.CompareTo(min) < 0)
            {
                return min;
            }
            else
            {
                return num;
            }
        }
    }
}
