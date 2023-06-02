using ImageConvertor;

IOUtil.LoadImageRGB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "before.jpg"))
      .Ajdust(ImageAjdustment.Value, 0.1f)
      .Filter(ImageFilter.GaussianFilter)
      .Scale(100, 200, ImageScale.Bilinear)
      .SaveAsImage()
      .Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "after.jpg"));
