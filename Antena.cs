using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Denpa3SavEditor
{
    internal class Antena
    {

        // 管理番号
        private int antenaIndex = 0;
        private int bindAntenaIndex = 0;

        private bool isBinded = false;

        const int kImageWidth = 72;
        const int kImageHeight = 50;

        double scale;

        string antenaName;
        string antenaGenre;

        Point position;
        Point basePosition;
        Point offsetPosition;

        public void SetDrawOffset(int x,int y) {


            offsetPosition.X = (double)x;
            offsetPosition.Y = (double)y;
        }
        public void setBasePosition(Point pos) { basePosition = pos; }

        public Point GetOffsetPoint() { return offsetPosition; }

        public Image image = new Image();
        Dictionary<int, BitmapImage> images;

        public void Init(Dictionary<int, BitmapImage> imgs,double drawSize)
        {
            images = imgs;
            scale = drawSize;

        }

        public void SetIndex(int i)
        {
            antenaIndex = i;

            if (!isBinded) {
                bindAntenaIndex = antenaIndex;
                isBinded = true;
            }

            image.Width = (double)kImageWidth * scale;
            image.Height = (double)kImageHeight * scale;

            if (antenaIndex < 224)
            {
                image.Source = images[antenaIndex];
            }
            else
            {
                image.Source = images[0];
            }

        }

        public int GetIndex () { return antenaIndex; }

        public void ResetIndex()
        {
            antenaIndex = bindAntenaIndex;

            // 替えたのでイメージを更新
            image.Width = (double)kImageWidth * scale;
            image.Height = (double)kImageHeight * scale;

            if (antenaIndex < 224)
            {
                image.Source = images[antenaIndex];
            }
            else
            {
                image.Source = images[0];
            }

        }

        public void AntenaDataSet(string n, string g)
        {
            antenaName = n;
            antenaGenre = g;
        }

        public void Draw(Canvas canvas)
        {

            image.Width = (double)kImageWidth * scale;
            image.Height = (double)kImageHeight * scale;

            if (antenaIndex < 224)
            {
                image.Source = images[antenaIndex];
            }
            else
            {
                image.Source = images[0];
            }

            // 左側
            Canvas.SetLeft(
                image,
                position.X - image.Width / 2 + basePosition.X
            );

            Canvas.SetTop(
                image,
                position.Y - image.Height / 2 + basePosition.Y
            );


            canvas.Children.Add(image);
        }
    }
}
