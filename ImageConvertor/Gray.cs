namespace ImageConvertor
{
    /// <summary>
    /// グレースケール画像
    /// </summary>
    internal class Gray
    {
        /// <summary>
        /// 明度
        /// </summary>
        public byte[,] Value { get; }

        /// <summary>
        /// 画像の幅
        /// </summary>
        public int Width => this.Value.GetLength(0);

        /// <summary>
        /// 画像の高さ
        /// </summary>
        public int Height => this.Value.GetLength(1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Gray(int width, int height)
        {
            this.Value = new byte[width, height];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Gray(byte[,] value)
        {
            this.Value = value;
        }
    }
}
