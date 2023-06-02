namespace ImageConvertor
{
    /// <summary>
    /// ファイルの読み書き関連の処理
    /// </summary>
    internal static class IOUtil
    {
        /// <summary>
        /// グレースケールで画像を読み込み
        /// </summary>
        public static Gray LoadImageGray(string filename)
        {
            return LoadImageRGB(filename).ToGrayScale();
        }

        /// <summary>
        /// RGBチャネルで画像を読み込み
        /// </summary>
        public static RGB LoadImageRGB(string filename)
        {
            using var img = Image.Load<Rgba32>(filename);
            var w = img.Width;
            var h = img.Height;

            var rgb = new RGB(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    rgb.Red[x, y] = img[x, y].R;
                    rgb.Green[x, y] = img[x, y].G;
                    rgb.Blue[x, y] = img[x, y].B;
                }
            }

            return rgb;
        }

        /// <summary>
        /// Imageとして保存
        /// </summary>
        public static Image<Rgba32> SaveAsImage(this Gray src)
        {
            var w = src.Width;
            var h = src.Height;

            var img = new Image<Rgba32>(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    img[x, y] = new Rgba32(src.Value[x, y], src.Value[x, y], src.Value[x, y]);
                }
            }

            return img;
        }

        /// <summary>
        /// Imageとして保存
        /// </summary>
        public static Image<Rgba32> SaveAsImage(this RGB src)
        {
            var w = src.Width;
            var h = src.Height;

            var img = new Image<Rgba32>(w, h);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    img[x, y] = new Rgba32(src.Red[x, y], src.Green[x, y], src.Blue[x, y]);
                }
            }

            return img;
        }

        /// <summary>
        /// Imageとして保存
        /// </summary>
        public static Image<Rgba32> SaveAsImage(this HSV src)
        {
            return src.ToRGB().SaveAsImage();
        }

        // System.Drawing.Commonの時の実装
        // WindowsのみのサポートになったらしいのでImageSharpでの実装に変更
        // ImageSharp側に大体の機能があるじゃないかと言われたら元も子もない
        // https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only

        ///// <summary>
        ///// RGBチャネルで画像を読み込み
        ///// </summary>
        //public static RGB LoadImageRGB(string filename)
        //{
        //    var img = new Bitmap(filename);
        //    var w = img.Width;
        //    var h = img.Height;

        //    var rgb = new RGB(w, h);

        //    for (var x = 0; x < w; x++)
        //    {
        //        for (var y = 0; y < h; y++)
        //        {
        //            rgb.Red[x, y] = img.GetPixel(x, y).R;
        //            rgb.Green[x, y] = img.GetPixel(x, y).G;
        //            rgb.Blue[x, y] = img.GetPixel(x, y).B;
        //        }
        //    }

        //    return rgb;
        //}

        ///// <summary>
        ///// Imageとして保存
        ///// </summary>
        //public static Image SaveAsImage(this Gray src)
        //{
        //    var w = src.Width;
        //    var h = src.Height;

        //    var img = new Bitmap(w, h);

        //    for (var x = 0; x < w; x++)
        //    {
        //        for (var y = 0; y < h; y++)
        //        {
        //            img.SetPixel(x, y, Color.FromArgb(src.Value[x, y], src.Value[x, y], src.Value[x, y]));
        //        }
        //    }

        //    return img;
        //}

        ///// <summary>
        ///// Imageとして保存
        ///// </summary>
        //public static Image SaveAsImage(this RGB src)
        //{
        //    var w = src.Width;
        //    var h = src.Height;

        //    var img = new Bitmap(w, h);

        //    for (var x = 0; x < w; x++)
        //    {
        //        for (var y = 0; y < h; y++)
        //        {
        //            img.SetPixel(x, y, Color.FromArgb(src.Red[x, y], src.Green[x, y], src.Blue[x, y]));
        //        }
        //    }

        //    return img;
        //}
    }
}
