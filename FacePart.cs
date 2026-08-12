using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public enum FacePartType
{
    Eye,
    Eyebrow,
    Nose,
    Mouth,
    Cheek,
    Cover,
    Forelock,
    FaceOutline,
    FaceHead,
    Body
};

namespace Denpa3SavEditor
{
    internal class FacePart
    {
        // 座標
        Point position;

        // オフセットとか
        Point basePosition;

        // 元画像内のパーツの最初の切り出し位置
        public int StartOffsetX;
        public int StartOffsetY;

        // 描画時のオフセット
        public int drawOffsetX;
        public int drawOffsetY;

        FacePartType facePartType;

        // 切り出すサイズ
        public int Width;
        public int Height;

        // サイズ
        public double scale = 1.0;

        // 何番目
        public int Index = 0;
        public int bindIndex = 0;
        private bool isBind = true;

        // 使用する画像
        public BitmapSource Texture { get; set; }

        // 上から重ねる時の画像
        public BitmapSource maskTexture { get; set; }

        public Image baseCropImage = new Image();
        public Image baseCropImage2 = new Image();

        // 左右対称のパーツか
        public bool isMirror = false;

        // 幅
        public int mirrorDistance = 0;

        // 切り抜く方向が横になる
        private bool isCroppingHorizontal = false;

        // 体の色の管理番号　詳しくはbodyColor.cs
        int bodyColorIndex = 0;

        // 髪
        int hearColorIndex = 0;

        // 肌
        int skinColorIndex = 0;

        // 身長
        int bodyHeight= 0;

        // 初期化
        public void Initialize(FacePartType t, int offsetX, int offsetY, int width, int height, BitmapSource texture, double drawScale = 1.0)
        {
            facePartType = t;

            StartOffsetX = offsetX;
            StartOffsetY = offsetY;
            Width = width;
            Height = height;

            Texture = texture;

            scale = drawScale;

            position.X = 100;
            position.Y = 100;
        }
        public void SetIndex(int index)
        {
            Index = index;

            // 初回限定
            if (isBind)
            {
                bindIndex = index;
                isBind = false;
            }

            // 替えたのでイメージを更新
            UpdateImage();
        }
        public FacePartType GetType() { return facePartType; }

        // 色とそのマスク画像セット
        public void SetBodyColorIndex(int i) { 

            bodyColorIndex = i;

            // 替えたのでイメージを更新
            UpdateImage();

        }

        public void SetHearColorIndex(int i)
        {

            hearColorIndex = i;

            // 替えたのでイメージを更新
            UpdateImage();

        }

        public void SetSkinColorIndex(int i)
        {
            // 範囲外ならまた０に戻ってループw
            skinColorIndex = i % BodyColorVariation.skinColorRgb.Count;

            // 替えたのでイメージを更新
            UpdateImage();

        }

        public void SetBodyColorTexture(BitmapImage img){ maskTexture = img;}
        
        public void setBodyHeight(int i)
        {

            bodyHeight = i;

            // 替えたのでイメージを更新
            UpdateImage();
        }

        public void setBasePosition(Point pos) { basePosition = pos; }

        // offsetも含めた固定のを返す
        public Point GetOffsetPoint()
        {
            return new Point(
                (double)drawOffsetX,
                (double)drawOffsetY);
        }

        public void UpdatePosition()
        {

            // 実際に画面へ表示するサイズ
            double drawWidth = Width * scale;
            double drawHeight = Height * scale;;

            // 体は下部固定
            if (facePartType == FacePartType.Body)
            {

                Canvas.SetLeft(
                    baseCropImage,
                    position.X - (Width * scale) / 2 + basePosition.X
                );

                // 足元の位置は固定
                Canvas.SetBottom(
                    baseCropImage,
                    position.Y + drawOffsetY
                );

                return;
            }

            if (isMirror)
            {
                // ミラー間の距離もscale
                double scaledMirrorDistance = mirrorDistance * scale;

                // 左側
                Canvas.SetLeft(
                    baseCropImage2,
                    position.X - scaledMirrorDistance - drawWidth / 2
                );

                Canvas.SetTop(
                    baseCropImage2,
                    position.Y - drawHeight / 2 + basePosition.Y
                );

                // 右側
                Canvas.SetLeft(
                    baseCropImage,
                    position.X + scaledMirrorDistance - drawWidth / 2 + basePosition.X
                );

                Canvas.SetTop(
                    baseCropImage,
                    position.Y - drawHeight / 2 + basePosition.Y
                );

            }
            else
            {
                // 普通のパーツ
                Canvas.SetLeft(
                    baseCropImage,
                    position.X - drawWidth / 2 + basePosition.X
                );

                Canvas.SetTop(
                    baseCropImage,
                    position.Y - drawHeight / 2 + basePosition.Y
                );

            }
        }

        public int GetIndex()
        {
            return Index;
        }

        public void SetDrawOffset(int x, int y)
        {
            drawOffsetX = x;
            drawOffsetY = y;
        }

        // 初期のに
        public void ResetIndex()
        {
            Index = bindIndex;

            // 替えたのでイメージを更新
            UpdateImage();
        }

        // 左右対称設定。初期化時に行うように
        public void SetMirror(int mirrorOffset)
        {
            isMirror = true;
            this.mirrorDistance = mirrorOffset;
        }

        public void ActiveCropHorizontal() { isCroppingHorizontal = true; }

        public Image GetCropImage()
        {
            return baseCropImage;
        }
        public Point GetPoint() { return position; }


        // 描画させたいときに　Canvas.Children.Addする　死ねゴミエンジン


        // 先頭からn番目の顔パーツを描画する
        public void Draw(Canvas canvas)
        {

            // 今回のパーツを追加
            canvas.Children.Add(baseCropImage);

            if (isMirror)
            {
                canvas.Children.Add(baseCropImage2);
            }

            UpdatePosition();
        }

        // 画像の更新処理
        public void UpdateImage()
        {
            int OffsetXIndex;
            int OffsetYIndex;

            // n番目から切り出す位置を計算する
            if (isCroppingHorizontal)
            {
                Debug.WriteLine(Index);
                OffsetXIndex = Index % 16;
                OffsetYIndex = Index / 16;
            }
            else
            {
                OffsetXIndex = Index / 16;
                OffsetYIndex = Index % 16;
            }

            Int32Rect rect = new Int32Rect(
                StartOffsetX + OffsetXIndex * Width,
                StartOffsetY + OffsetYIndex * Height,
                Width,
                Height
            );

            CroppedBitmap crop = new CroppedBitmap(
                Texture,
                rect
            );

            //　りなかくなら
            if (facePartType == FacePartType.FaceOutline)
            {

                baseCropImage.Source = getColored(crop, BodyColorVariation.skinColorRgb[skinColorIndex]);

            }

            //　前髪なら
            else if (facePartType == FacePartType.Forelock)
            {
                // 髪の色取得
                if (hearColorIndex < HearColorVariation.HearColorRgb.Count)
                {
                    baseCropImage.Source = getColored(crop, HearColorVariation.HearColorRgb[hearColorIndex]);
                }
            }

            //　あたまなら
            else if (facePartType == FacePartType.FaceHead ||
                     facePartType == FacePartType.Body)
            {

                // 頭の種類からバリエーションを取得
                ColorVariations variation =
                    BodyColorVariation.bodyColorVariations[bodyColorIndex];

                // 柄の下地
                Rgb rgb = BodyColorVariation.ColorRgb[variation.color];

                baseCropImage.Source = getColored(crop, rgb);

                if (!variation.isSingleColor)
                {
                    // 柄の下地
                    Rgb rgb2 = BodyColorVariation.ColorRgb[variation.color2];

                    baseCropImage.Source = getColored(crop,maskTexture, rgb, rgb2);
                }

            }
            else
            {
                baseCropImage.Source = crop;
            }

            // もし体であれば身長に合わせて伸び縮みさせる
            if (facePartType == FacePartType.Body)
            {

                // ０が高い判定なので１から引いて反転させる
                double normalized =
                    1.0 - (double)bodyHeight / DenpaMen.kMaxDenpaMenBodyHeight;

                double min = 60;
                double max = 150;

                // 線形補完
                baseCropImage.Width = Width;
                baseCropImage.Height = Remap(normalized, min, max);
            }
            else
            {
                baseCropImage.Width = Width * scale;
                baseCropImage.Height = Height * scale;
            }


            // ミラーの場合ミラー用に
            if (isMirror)
            {
                // 頭なら
                if (facePartType == FacePartType.FaceHead ||
                     facePartType == FacePartType.Body)
                {
                    // 頭の種類からバリエーションを取得
                    ColorVariations variation =
                        BodyColorVariation.bodyColorVariations[bodyColorIndex];

                    // 柄の下地
                    Rgb rgb = BodyColorVariation.ColorRgb[variation.color];

                    baseCropImage2.Source = getColored(crop, rgb);

                    if (!variation.isSingleColor)
                    {
                        // 柄の下地
                        Rgb rgb2 = BodyColorVariation.ColorRgb[variation.color2];

                        baseCropImage2.Source = getColored(crop, maskTexture, rgb, rgb2);
                    }
                }
                //　輪郭なら
                else if (facePartType == FacePartType.FaceOutline)
                {
 
                    baseCropImage2.Source = getColored(crop, BodyColorVariation.skinColorRgb[skinColorIndex]);
                }
                else
                {
                    baseCropImage2.Source = crop;
                }


   
                    baseCropImage2.Width = Width * scale;
                    baseCropImage2.Height = Height * scale;
                


                    // 左右反転
                    baseCropImage2.LayoutTransform = new ScaleTransform(-1, 1);
            }

        }

        ///　senkei
        double Remap(double value, double min, double max)
        {
            return min + value * (max - min);
        }
        // 着色
        private BitmapSource getColored(CroppedBitmap crop, Rgb rgb)
        {
            BitmapSource colorCrop = new FormatConvertedBitmap(
                crop,
                PixelFormats.Bgra32,
                null,
                0
            );

            int stride = Width * 4;
            byte[] pixels = new byte[Height * stride];

            colorCrop.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte b = pixels[i + 0];
                byte g = pixels[i + 1];
                byte r = pixels[i + 2];

                // 白い部分だけ色を変更
                if (r > 150 && g > 150 && b > 150)
                {
                    pixels[i + 0] = rgb.b;
                    pixels[i + 1] = rgb.g;
                    pixels[i + 2] = rgb.r;
                }

            }

            return BitmapSource.Create(
                Width,
                Height,
                crop.DpiX,
                crop.DpiY,
                PixelFormats.Bgra32,
                null,
                pixels,
                stride
            );
        }

        // 下地＋上から画像を重ねて返すばーじょｎ
        private BitmapSource getColored(
        BitmapSource crop,
        BitmapSource mask,
        Rgb rgb,
        Rgb maskRgb)
        {
            int stride = crop.PixelWidth * 4;

            byte[] pixels = new byte[crop.PixelHeight * stride];
            crop.CopyPixels(pixels, stride, 0);

            int maskStride = mask.PixelWidth * 4;

            byte[] maskPixels = new byte[mask.PixelHeight * maskStride];
            mask.CopyPixels(maskPixels, maskStride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // 下地をrgbで着色
                pixels[i] = rgb.b;
                pixels[i + 1] = rgb.g;
                pixels[i + 2] = rgb.r;

                // マスク画像の色
                byte mr = maskPixels[i + 2];
                byte mg = maskPixels[i + 1];
                byte mb = maskPixels[i];

                // 白い縞々部分だけmaskRgbにする
                if (mr > 200 && mg > 200 && mb > 200)
                {
                    pixels[i] = maskRgb.b;
                    pixels[i + 1] = maskRgb.g;
                    pixels[i + 2] = maskRgb.r;
                }

                // pixels[i + 3] はアルファなのでそのまま
            }

            return BitmapSource.Create(
                crop.PixelWidth,
                crop.PixelHeight,
                crop.DpiX,
                crop.DpiY,
                PixelFormats.Bgra32,
                null,
                pixels,
                stride
            );
        }

    }
}
