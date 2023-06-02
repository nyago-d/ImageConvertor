namespace ImageConvertor
{
    /// <summary>
    /// 画像の拡大縮小関連
    /// </summary>
    internal static class ImageScale
    {
        // バイキュービックの重み計算用パラメタ（-0.5～-1くらいで小さいほどシャープ化が強くなるとかなんとか）
        private const float Alpha = -0.5f;

        /// <summary>
        /// スケーリングする
        /// </summary>
        public static Gray Scale(this Gray original, int width, int height, Func<byte[,], int, int, byte[,]> func)
        {
            return new Gray(func(original.Value, width, height));
        }

        /// <summary>
        /// スケーリングする
        /// </summary>
        public static RGB Scale(this RGB original, int width, int height, Func<byte[,], int, int, byte[,]> func)
        {
            return new RGB(func(original.Red, width, height), func(original.Green, width, height), func(original.Blue, width, height));
        }

        /// <summary>
        /// 最近傍法によるスケーリング
        /// </summary>
        public static byte[,] NearestNeighbor(byte[,] original, int width, int height)
        {
            // 元の画像の幅、高さ
            var w = original.GetLength(0);
            var h = original.GetLength(1);

            // スケールを計算（元の画像に対する新しい画像のスケール）
            var scaleX = (float)width / w;
            var scaleY = (float)height / h;

            // 新しい画像における単位長さ
            var unitX = 1.0f / scaleX;
            var unitY = 1.0f / scaleY;

            var output = new byte[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var srcX = (int)Math.Round(unitX * (x + 0.5f), MidpointRounding.AwayFromZero);
                    var srcY = (int)Math.Round(unitY * (y + 0.5f), MidpointRounding.AwayFromZero);

                    output[x, y] = Get(original, srcX, srcY, w, h);
                }
            }

            return output;
        }

        /// <summary>
        /// バイリニアによるスケーリング
        /// </summary>
        public static byte[,] Bilinear(byte[,] original, int width, int height)
        {
            // 元の画像の幅、高さ
            var w = original.GetLength(0);
            var h = original.GetLength(1);

            // スケールを計算（新しい画像に対する元の画像のスケール）
            var unitX = (float)w / width;
            var unitY = (float)h / height;

            // 左上から他3点の座標を計算するための単位長さ（縮小の場合：元の画像/拡大の場合：新しい画像）
            var offsetX = unitX > 1.0f ? (int)unitX : 1;
            var offsetY = unitY > 1.0f ? (int)unitY : 1;

            var output = new byte[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    // 元の画像の座標を計算
                    var srcX = (unitX * (x + 0.5f));
                    var srcY = (unitY * (y + 0.5f));

                    // 左上から元の画像の座標までの正規化長さ
                    var ratioX = (srcX - (int)srcX) / offsetX;
                    var ratioY = (srcY - (int)srcY) / offsetY;

                    // 加重平均を計算
                    var sum = 0f;
                    for (var xx = 0; xx <= 1; xx++)
                    {
                        for (var yy = 0; yy <= 1; yy++)
                        {
                            var point = Get(original, (int)srcX + (int)(xx * offsetX), (int)srcY + (int)(yy * offsetY), w, h);
                            var weightX = GetBilinearWeight(ratioX, xx);
                            var weightY = GetBilinearWeight(ratioY, yy);
                            sum += point * weightX * weightY;
                        }
                    }

                    output[x, y] = RoundingUtil.ToByte(sum);
                }
            }

            return output;
        }

        /// <summary>
        /// バイキュービックによるスケーリング
        /// <para>
        /// 明らかにパフォーマンスが悪いけどここだけ書き方変えるのもどうかなと思ってそのまま。
        /// ・チャネルごとに毎回同じ計算をするのでまとめた方が良い
        /// ・単純に並列処理をした方が良い
        /// ・重み計算は先にするかキャッシュすると良い…気がする
        /// </para>
        /// </summary>
        public static byte[,] Bicubic(byte[,] original, int width, int height)
        {
            // 元の画像の幅、高さ
            var w = original.GetLength(0);
            var h = original.GetLength(1);

            // スケールを計算（新しい画像に対する元の画像のスケール）
            var unitX = (float)w / width;
            var unitY = (float)h / height;

            // 左上から他3点の座標を計算するための単位長さ（縮小の場合：元の画像/拡大の場合：新しい画像）
            var offsetX = unitX > 1.0f ? (int)unitX : 1;
            var offsetY = unitY > 1.0f ? (int)unitY : 1;

            var output = new byte[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    // 元の画像の座標を計算
                    var srcX = (unitX * (x + 0.5f));
                    var srcY = (unitY * (y + 0.5f));

                    // 左上から元の画像の座標までの正規化長さ
                    var ratioX = (srcX - (int)srcX) / offsetX;
                    var ratioY = (srcY - (int)srcY) / offsetY;

                    // 加重平均を計算
                    var sum = 0f;
                    for (var xx = -1; xx <= 2; xx++)
                    {
                        for (var yy = -1; yy <= 2; yy++)
                        {
                            var point = Get(original, (int)srcX + (int)(xx * offsetX), (int)srcY + (int)(yy * offsetY), w, h);
                            var weightX = GetBicubicWeight(ratioX, xx);
                            var weightY = GetBicubicWeight(ratioY, yy);
                            sum += point * weightX * weightY;
                        }
                    }

                    output[x, y] = RoundingUtil.ToByte(sum);
                }
            }

            return output;
        }

        /// <summary>
        /// バイリニアの重み計算
        /// </summary>
        private static float GetBilinearWeight(float ratio, int pos)
        {
            if (pos == 0)
            {
                return 1 - ratio;
            }
            else
            {
                return ratio;
            }
        }

        /// <summary>
        /// バイキュービックの重み計算
        /// </summary>
        private static float GetBicubicWeight(float ratio, int pos)
        {
            var res = 0f;
            var distance = Math.Abs(ratio - pos);
            if (distance <= 1.0f)
            {
                res = 1 - ((Alpha + 3.0f) * distance * distance) + ((Alpha + 2.0f) * distance * distance * distance);
            }
            else if (distance <= 2.0f)
            {
                res = (-4.0f * Alpha) + (8.0f * Alpha * distance) - (5.0f * Alpha * distance * distance) + (Alpha * distance * distance * distance);
            }

            return res;
        }

        /// <summary>
        /// 指定した座標の値を取得する
        /// <para>はみでる分は端。</para>
        /// </summary>
        private static byte Get(byte[,] src, int x, int y, int w, int h)
        {
            return src[RoundingUtil.Limit(x, 0, w - 1), RoundingUtil.Limit(y, 0, h - 1)];
        }
    }
}
