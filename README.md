## 画像処理あれこれ
   
### これはなに？

- 画像処理の自前実装です
- 学習目的のため、パフォーマンスは考慮していません
- ライブラリとして欲しいのであれば、ImageSharpとかを使うのが良いと思います

### 使い方メモ

#### 読み込み
```csharp
var imageGray = IOUtil.LoadImageGray("画像のファイルパス");
var imageRGB = IOUtil.LoadImageRGB("画像のファイルパス");
```

#### 書き込み
```csharp
image.SaveAsImage().Save("保存先のファイルパス");
```

### モードの変換
```csharp
var imageGray = imageRGB.ToGrayScale();
var imageHSV = imageRGB.ToHSV();
```

### 回転
```csharp
// 時計回り90°回転
var rotateImage1 = image.Rotate(ImageRotate.TurnRight);
// 30°回転
var rotateImage2 = image.Rotate(30);
// 水平方向に反転
var rotateImage3 = image.Rotate(ImageRotate.ReverseHorizontal);
```

### 拡大縮小
```csharp
// ニアレストネイバー
var scaleImage1 = image.Scale(100, 200, ImageScale.NearestNeighbor);
// バイキュービック
var scaleImage2 = image.Scale(100, 200, ImageScale.Bicubic);
```

### 色調補正
```csharp
// 色相の補正
var ajustImage1 = image.Ajdust(ImageAjdustment.Hue, 0.5f);
// 明度の補正
var ajustImage2 = image.Ajdust(ImageAjdustment.Value, -0.5f);
// 反転
var ajustImage3 = image.Ajdust(ImageAjdustment.Inverse);
```

### 描画モード
```csharp
// 乗算
var blendImage1 = image1.Blend(image2, ImageBlend.Multiply);
// オーバーレイ
var blendImage2 = image1.Blend(image3, ImageBlend.Overlay);
```
※画像サイズは一致必須

### フィルタ
```csharp
// ガウシアンフィルタ
var filterImage1 = image.Filter(ImageFilter.GaussianFilter);
// アウトラインフィルタ
var filterImage2 = image.Filter(ImageFilter.OutlineFilter);
```

### メソッドチェーンできます
```csharp
IOUtil.LoadImageRGB("画像のファイルパス")
      .Ajdust(ImageAjdustment.Value, 0.1f)
      .Filter(ImageFilter.GaussianFilter)
      .Scale(100, 200, ImageScale.Bilinear)
      .SaveAsImage()
      .Save("保存先のファイルパス");
```