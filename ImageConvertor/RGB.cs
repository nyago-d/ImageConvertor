namespace ImageConvertor
{
    /// <summary>
    /// RGB画像
    /// <para>Alpha考え出すと面倒なのでとりあえずRGB。</para>
    /// </summary>
    internal class RGB
    {
        /// <summary>
        /// 赤
        /// </summary>
        public byte[,] Red { get; }

        /// <summary>
        /// 緑
        /// </summary>
        public byte[,] Green { get; }

        /// <summary>
        /// 青
        /// </summary>
        public byte[,] Blue { get; }

        /// <summary>
        /// 画像の幅
        /// </summary>
        public int Width => this.Red.GetLength(0);

        /// <summary>
        /// 画像の高さ
        /// </summary>
        public int Height => this.Red.GetLength(1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RGB(int width,  int height)
        {
            this.Red = new byte[width, height];
            this.Green = new byte[width, height];
            this.Blue = new byte[width, height];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RGB(byte[,] red, byte[,] green, byte[,] blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
    }
}
