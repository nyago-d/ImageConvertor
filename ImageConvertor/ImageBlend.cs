namespace ImageConvertor
{
    /// <summary>
    /// 画像のブレンド（描画）モード関連
    /// </summary>
    internal static class ImageBlend
    {
        /// <summary>
        /// ブレンドする
        /// </summary>
        public static Gray Blend(this Gray src, Gray value, Func<byte[,], byte[,], byte[,]> func)
        {
            return new Gray(func(src.Value, value.Value));
        }

        /// <summary>
        /// ブレンドする
        /// </summary>
        public static RGB Blend(this RGB src, RGB value, Func<byte[,], byte[,], byte[,]> func)
        {
            return new RGB(func(src.Red, value.Red), func(src.Green, value.Green), func(src.Blue, value.Blue));
        }

        /// <summary>
        /// 加算-覆い焼き（リニア）
        /// </summary>
        public static byte[,] Add(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Add);
        }

        /// <summary>
        /// 加算
        /// </summary>
        public static byte Add(byte src, byte value)
        {
            return RoundingUtil.ToByte(src + value);
        }

        /// <summary>
        /// 減算
        /// </summary>
        public static byte[,] Subtract(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Subtract);
        }

        /// <summary>
        /// 減算
        /// </summary>
        public static byte Subtract(byte src, byte value)
        {
            return RoundingUtil.ToByte(src - value);
        }

        /// <summary>
        /// 乗算
        /// </summary>
        public static byte[,] Multiply(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Multiply);
        }

        /// <summary>
        /// 乗算
        /// </summary>
        public static byte Multiply(byte src, byte value)
        {
            return RoundingUtil.ToByte((float)src * value / byte.MaxValue);
        }

        /// <summary>
        /// 除算
        /// </summary>
        public static byte[,] Division(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Division);
        }

        /// <summary>
        /// 除算
        /// </summary>
        public static byte Division(byte src, byte value)
        {
            return RoundingUtil.ToByte((float)src / (float)value / byte.MaxValue);
        }

        /// <summary>
        /// スクリーン
        /// </summary>
        public static byte[,] Screen(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Screen);
        }

        /// <summary>
        /// スクリーン
        /// </summary>
        public static byte Screen(byte src, byte value)
        {
            return ImageAjdustment.Inverse(Multiply(ImageAjdustment.Inverse(src), ImageAjdustment.Inverse(value)));
        }

        /// <summary>
        /// オーバーレイ
        /// </summary>
        public static byte[,] Overlay(byte[,] src, byte[,] value)
        {
            return Calculate(src, value, Overlay);
        }

        /// <summary>
        /// オーバーレイ
        /// </summary>
        public static byte Overlay(byte src, byte value)
        {
            return src < 0.5 ? RoundingUtil.ToByte(Multiply(src, value) * 2) : ImageAjdustment.Inverse(RoundingUtil.ToByte(Multiply(ImageAjdustment.Inverse(src), ImageAjdustment.Inverse(value)) * 2));
        }

        /// <summary>
        /// 1px分の計算をする
        /// <para>同一サイズの2枚のチャネルの計算、必ず先にスケーリングすること。</para>
        /// </summary>
        private static byte[,] Calculate(byte[,] src, byte[,] value, Func<byte, byte, byte> func)
        {
            var dest = new byte[src.GetLength(0), src.GetLength(1)];

            for (var x = 0; x < dest.GetLength(0); x++)
            {
                for (var y = 0; y < dest.GetLength(1); y++)
                {
                    dest[x, y] = func(src[x, y], value[x, y]);
                }
            }

            return dest;
        }
    }
}
