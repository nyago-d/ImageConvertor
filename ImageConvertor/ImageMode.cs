namespace ImageConvertor
{
    /// <summary>
    /// 画像のモード関連
    /// </summary>
    internal static class ImageMode
    {
        /// <summary>
        /// RGB→グレースケール変換
        /// <para>
        /// 普通のRGB平均。
        /// ガンマ補正とか輝度で変換とかしたければここでごにょごにょすると良い。
        /// </para>
        /// </summary>
        public static Gray ToGrayScale(this RGB rgb)
        {
            var w = rgb.Width;
            var h = rgb.Height;

            var gray = new Gray(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    gray.Value[x, y] = (byte)((rgb.Red[x, y] + rgb.Green[x, y] + rgb.Blue[x, y]) / 3);
                }
            }

            return gray;
        }

        /// <summary>
        /// HSV→グレースケール変換
        /// </summary>
        public static Gray ToGrayScale(this HSV hsv)
        {
            var w = hsv.Width;
            var h = hsv.Height;

            var gray = new Gray(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    gray.Value[x, y] = RoundingUtil.ToByte(hsv.Value[x, y] * byte.MaxValue);
                }
            }

            return gray;
        }

        /// <summary>
        /// RGB→HSV変換
        /// </summary>
        public static HSV ToHSV(this RGB rgb)
        {
            var w = rgb.Width;
            var h = rgb.Height;

            var hsv = new HSV(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var r = rgb.Red[x, y] / 255.0f;
                    var g = rgb.Green[x, y] / 255.0f;
                    var b = rgb.Blue[x, y] / 255.0f;

                    var ary = new float[] { r, g, b };
                    var min = ary.Min();
                    var max = ary.Max();

                    hsv.Hue[x, y] = GetHue(r, g, b, min, max);
                    hsv.Saturation[x, y] = GetSaturation(min, max);
                    hsv.Value[x, y] = max;
                }
            }

            return hsv;
        }

        /// <summary>
        /// HSV→RGB変換
        /// </summary>
        public static RGB ToRGB(this HSV hsv)
        {
            var w = hsv.Width;
            var h = hsv.Height;

            var rgb = new RGB(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var v = hsv.Value[x, y];
                    var s = hsv.Saturation[x, y];

                    var r = v;
                    var g = v;
                    var b = v;

                    if (s != 0)
                    {
                        var hh = hsv.Hue[x, y] * 6f;
                        var i = (int)hh;
                        var f = hh - i;

                        var p = v * (1f - s);
                        var q = v * (1 - f * s);
                        var t = v * (1 - (1 - f) * s);

                        switch (i)
                        {
                            default:
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
                            case 5:
                                r = v;
                                g = p;
                                b = q;
                                break;
                        }
                    }

                    rgb.Red[x, y] = RoundingUtil.ToByte(r * 255f);
                    rgb.Green[x, y] = RoundingUtil.ToByte(g * 255f);
                    rgb.Blue[x, y] = RoundingUtil.ToByte(b * 255f);
                }
            }

            return rgb;
        }

        /// <summary>
        /// 色相を取得
        /// </summary>
        private static float GetHue(float red, float green, float blue, float min, float max)
        {
            var hue = 0f;

            // RGBが同じ場合はなんでもいいけど0にしておく
            if (max == min)
            {
                return hue;
            }

            // Rが最大
            if (max == red)
            {
                hue = 60 * (green - blue) / (max - min);
            }
            // Gが最大
            else if (max == green)
            {
                hue = 60 * (blue - red) / (max - min) + 120;
            }
            // Bが最大
            else
            {
                hue = 60 * (red - green) / (max - min) + 240;
            }

            if (hue < 0)
            {
                hue += 360;
            }

            return hue / 360;
        }

        /// <summary>
        /// 彩度を取得
        /// </summary>
        private static float GetSaturation(float min, float max)
        {
            if (max == 0)
            {
                return 0;
            }
            else
            {
                return (max - min) / max;
            }
        }
    }
}
