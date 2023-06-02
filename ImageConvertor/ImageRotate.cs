namespace ImageConvertor
{
    /// <summary>
    /// 画像の回転関連
    /// </summary>
    internal static class ImageRotate
    {
        /// <summary>
        /// 回転する
        /// </summary>
        public static Gray Rotate(this Gray src, Func<byte[,], byte[,]> func)
        {
            return new Gray(func(src.Value));
        }

        /// <summary>
        /// 回転する
        /// </summary>
        public static RGB Rotate(this RGB src, Func<byte[,], byte[,]> func)
        {
            return new RGB(func(src.Red), func(src.Green), func(src.Blue));
        }

        /// <summary>
        /// 回転する
        /// </summary>
        public static Gray Rotate(this Gray src, int angle)
        {
            return new Gray(Roate(src.Value, angle));
        }

        /// <summary>
        /// 回転する
        /// </summary>
        public static RGB Rotate(this RGB src, int angle)
        {
            return new RGB(Roate(src.Red, angle), Roate(src.Green, angle), Roate(src.Blue, angle));
        }

        /// <summary>
        /// 任意の角度で回転
        /// <para>とりあえずはみ出た分は削っちゃう。含めたいならその分画像を拡大する必要がありそう。</para>
        /// </summary>
        private static byte[,] Roate(this byte[,] src, int angle)
        {
            var w = src.GetLength(0);
            var h = src.GetLength(1);

            var centerX = w / 2;
            var centerY = h / 2;

            var output = new byte[w, h];
            
            // 回転元の画像ベースで回転先を計算すると穴が空いてしまうため、
            // 回転後の画像ベースで回転元を計算する。ので、角度は正負反転する。
            var sin = Math.Sin(-angle * (Math.PI / 180));
            var cos = Math.Cos(-angle * (Math.PI / 180));

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var xx = (int)Math.Round((x - centerX) * cos - (y - centerY) * sin + centerX);
                    var yy = (int)Math.Round((x - centerX) * sin + (y - centerY) * cos + centerY);

                    if (0 <= xx && xx < w && 0 <= yy && yy < h)
                    {
                        output[x, y] = src[xx, yy];
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 転置
        /// </summary>
        public static byte[,] Transpose(byte[,] src)
        {
            return Calculate(src, true, (s, x, y, w, h) => s[y, x]);
        }

        /// <summary>
        /// 反時計回り90°回転
        /// </summary>
        public static byte[,] TurnLeft(byte[,] src)
        {
            return Calculate(src, true, (s, x, y, w, h) => s[h - 1 - y, x]);
        }

        /// <summary>
        /// 時計回り90°回転
        /// </summary>
        public static byte[,] TurnRight(byte[,] src)
        {
            return Calculate(src, true, (s, x, y, w, h) => s[y, w - 1 - x]);
        }

        /// <summary>
        /// 180°回転
        /// </summary>
        public static byte[,] TurnOver(byte[,] src)
        {
            return Calculate(src, false, (s, x, y, w, h) => s[w - 1 - x, h - 1 - y]);
        }

        /// <summary>
        /// 水平方向に反転
        /// </summary>
        public static byte[,] ReverseHorizontal(byte[,] src)
        {
            return Calculate(src, false, (s, x, y, w, h) => s[x, h - 1 - y]);
        }

        /// <summary>
        /// 垂直方向に反転
        /// </summary>
        public static byte[,] ReverseVertical(byte[,] src)
        {
            return Calculate(src, false, (s, x, y, w, h) => s[w - 1 - x, y]);
        }

        /// <summary>
        /// 計算する
        /// </summary>
        private static byte[,] Calculate(byte[,] src, bool reverseAspect, Func<byte[,], int, int, int, int, byte> func)
        {
            var w = reverseAspect ? src.GetLength(1) : src.GetLength(0);
            var h = reverseAspect ? src.GetLength(0) : src.GetLength(1);

            var output = new byte[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    output[x, y] = func(src, x, y, w, h);
                }
            }

            return output;
        }
    }
}
