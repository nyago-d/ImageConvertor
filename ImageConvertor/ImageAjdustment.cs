namespace ImageConvertor
{
    /// <summary>
    /// 画像の色調補正
    /// </summary>
    internal static class ImageAjdustment
    {
        /// <summary>
        /// 補正する
        /// </summary>
        public static Gray Ajdust(this Gray src, Func<byte[,], byte[,]> func)
        {
            return new Gray(func(src.Value));
        }

        /// <summary>
        /// 補正する
        /// </summary>
        public static RGB Ajdust(this RGB src, Func<byte[,], byte[,]> func)
        {
            return new RGB(func(src.Red), func(src.Green), func(src.Blue));
        }

        /// <summary>
        /// 補正する
        /// </summary>
        public static RGB Ajdust(this RGB src, Func<HSV, float, HSV> func, float offsetRate)
        {
            return src.ToHSV().Ajdust(func, offsetRate).ToRGB();
        }

        /// <summary>
        /// 補正する
        /// </summary>
        public static HSV Ajdust(this HSV src, Func<HSV, float, HSV> func, float offsetRate)
        {
            return func(src, offsetRate);
        }

        /// <summary>
        /// 反転
        /// </summary>
        public static byte[,] Inverse(byte[,] src)
        {
            return Calculate(src, Inverse);
        }

        /// <summary>
        /// 反転
        /// </summary>
        public static byte Inverse(byte src)
        {
            return (byte)(byte.MaxValue - src);
        }

        /// <summary>
        /// 色相の調整
        /// </summary>
        public static HSV Hue(HSV hsv, float offsetRate)
        {
            return new HSV(Calculate(hsv.Hue, offsetRate, Add), hsv.Saturation, hsv.Value);
        }

        /// <summary>
        /// 彩度の調整
        /// </summary>
        public static HSV Saturation(HSV hsv, float offsetRate)
        {
            return new HSV(hsv.Hue, Calculate(hsv.Saturation, offsetRate, Add), hsv.Value);
        }

        /// <summary>
        /// 明度の調整
        /// </summary>
        public static HSV Value(HSV hsv, float offsetRate)
        {
            return new HSV(hsv.Hue, hsv.Saturation, Calculate(hsv.Value, offsetRate, Add));
        }

        /// <summary>
        /// 加算
        /// </summary>
        private static float Add(float src, float offsetRate)
        {
            return RoundingUtil.Limit((float)src + src * offsetRate, 0f, 1f);
        }

        /// <summary>
        /// 1px分の計算をする
        /// </summary>
        private static byte[,] Calculate(byte[,] src, Func<byte, byte> func)
        {
            var dest = new byte[src.GetLength(0), src.GetLength(1)];

            for (var x = 0; x < dest.GetLength(0); x++)
            {
                for (var y = 0; y < dest.GetLength(1); y++)
                {
                    dest[x, y] = func(src[x, y]);
                }
            }

            return dest;
        }

        /// <summary>
        /// 1px分の計算をする
        /// </summary>
        private static float[,] Calculate(float[,] src, float offsetRate, Func<float, float, float> func)
        {
            var dest = new float[src.GetLength(0), src.GetLength(1)];

            for (var x = 0; x < dest.GetLength(0); x++)
            {
                for (var y = 0; y < dest.GetLength(1); y++)
                {
                    dest[x, y] = func(src[x, y], offsetRate);
                }
            }

            return dest;
        }
    }
}
