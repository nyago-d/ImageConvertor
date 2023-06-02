namespace ImageConvertor
{
    /// <summary>
    /// HSV画像
    /// </summary>
    internal class HSV
    {
        /// <summary>
        /// 色相
        /// </summary>
        public float[,] Hue { get; }

        /// <summary>
        /// 彩度
        /// </summary>
        public float[,] Saturation { get; }

        /// <summary>
        /// 明度
        /// </summary>
        public float[,] Value { get; }

        /// <summary>
        /// 画像の幅
        /// </summary>
        public int Width => this.Hue.GetLength(0);

        /// <summary>
        /// 画像の高さ
        /// </summary>
        public int Height => this.Hue.GetLength(1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HSV(int width, int height)
        {
            this.Hue = new float[width, height];
            this.Saturation = new float[width, height];
            this.Value = new float[width, height];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HSV(float[,] hue, float[,] saturation, float[,] value)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }
    }
}
