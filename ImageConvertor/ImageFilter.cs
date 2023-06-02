namespace ImageConvertor
{
    /// <summary>
    /// 画像のフィルタ関連
    /// </summary>
    internal static class ImageFilter
    {
        /// <summary>
        /// ガウシアンフィルタ
        /// </summary>
        public static readonly float[,] GaussianFilter = new float[,]
        {
            { (float)1/16, (float)2/16, (float)1/16 },
            { (float)2/16, (float)4/16, (float)2/16 },
            { (float)1/16, (float)2/16, (float)1/16 }
        };

        /// <summary>
        /// ガウシアンフィルタ
        /// </summary>
        public static readonly float[,] GaussianFilter2 = new float[,]
        {
            { (float)1/256, (float)4/256, (float)6/256, (float)4/256,(float)1/256 },
            { (float)4/256, (float)16/256, (float)24/256, (float)16/256,(float)4/256 },
            { (float)6/256, (float)24/256, (float)36/256, (float)25/256,(float)6/256 },
            { (float)4/256, (float)16/256, (float)24/256, (float)16/256,(float)4/256 },
            { (float)1/256, (float)4/256, (float)6/256, (float)4/256,(float)1/256 }
        };

        /// <summary>
        /// フラットフィルタ
        /// </summary>
        public static readonly float[,] FlatFilter = new float[,]
        {
            { (float)1/9, (float)1/9, (float)1/9 },
            { (float)1/9, (float)1/9, (float)1/9 },
            { (float)1/9, (float)1/9, (float)1/9 }
        };

        /// <summary>
        /// アウトラインフィルタ
        /// </summary>
        public static readonly float[,] OutlineFilter = new float[,]
        {
            { 0, -1, 0 },
            { -1, 0, 1 },
            { 0, 1, 0 }
        };

        /// <summary>
        /// ラプラシアンフィルタ
        /// </summary>
        public static readonly float[,] LaplacianFilter = new float[,]
        {
            { 0, 1, 0 },
            { 1, -4, 1 },
            { 0, 1, 0 }
        };

        /// <summary>
        /// アンシャープフィルタ
        /// </summary>
        public static readonly float[,] UnSharpFilter = new float[,]
        {
            { (float)-1/9, (float)-1/9, (float)-1/9 },
            { (float)-1/9, (float)17/9, (float)-1/9 },
            { (float)-1/9, (float)-1/9, (float)-1/9 }
        };

        /// <summary>
        /// ソーベルフィルタ
        /// </summary>
        public static readonly float[,] SobelFilter = new float[,]
        {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 }
        };

        /// <summary>
        /// フィルタする
        /// </summary>
        public static Gray Filter(this Gray src, float[,] filter)
        {
            return new Gray(Filter(src.Value, filter));
        }

        /// <summary>
        /// フィルタする
        /// </summary>
        public static RGB Filter(this RGB src, float[,] filter)
        {
            return new RGB(Filter(src.Red, filter), Filter(src.Green, filter), Filter(src.Blue, filter));
        }

        /// <summary>
        /// 1px分の計算をする
        /// </summary>
        private static byte[,] Filter(byte[,] src, float[,] filter)
        {
            var w = src.GetLength(0);
            var h = src.GetLength(1);
            var filterSize = filter.GetLength(0);

            var output = new byte[w, h];
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var sum = 0.0f;

                    // 全範囲フィルタかける
                    for (var xx = -filterSize / 2; xx <= filterSize / 2; xx++)
                    {
                        for (var yy = -filterSize / 2; yy <= filterSize / 2; yy++)
                        {
                            // はみ出ていなければ加算
                            if (0 <= x + xx && x + xx < w && 0 <= y + yy && y + yy < h)
                            {
                                sum += src[x + xx, y + yy] * filter[xx + filterSize / 2, yy + filterSize / 2];
                            }
                        }
                    }

                    output[x, y] = RoundingUtil.ToByte(sum);
                }
            }

            return output;
        }
    }
}
