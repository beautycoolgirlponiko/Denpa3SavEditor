using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Denpa3SavEditor
{
    internal class DenpaMen
    {

        //  データ始まるアドレス
        Int32 denpaAddress = 0x13df8;
        public Int32 GetDenpaAddress() { return denpaAddress; }

        // なまえ
        string name;
        string bindName;

        // 身長
        public static int kMaxDenpaMenBodyHeight = 29;

        int bodyHeight = 0;
        int bindBodyHeight = 0;

        // 頭の形
        int headIndex = 0;
        int bindHeadIndex = 0;

        // 体の色
        int colorIndex = 0;
        int bindcolorIndex = 0;

        // 前髪の色
        int hearColorIndex = 0;
        int bindHearColorIndex = 0;

        // 前髪の色
        int skinColorIndex = 0;
        int bindSkinColorIndex = 0;

        // 性格
        int personalityIndex = 0;
        int bindPersonalityIndex = 0;

        // 個体値？　０２ならアンテナ有りのAPになる
        int individualIndex = 0;
        int bindIndividualIndex = 0;

        // デバッグ用
        private const int kDebugCorsorIndex = 88; // デバッグで書き換えるreadとかでかいてるなんばんめ番号
        int debugIndex = 0;

        // 顔パーツ
        private FacePart eye;
        private FacePart eyebrow;
        private FacePart nose;
        private FacePart mouth;
        private FacePart cheek;    // ほっぺ
        private FacePart cover;    // 被り物系。メガネなど
        private FacePart forelock; // 前髪

        private FacePart body;  // 体
        private FacePart faceHead;  // 頭の形
        private FacePart faceOutline; // 輪郭、肌

        // アンテナ
        private Antena antena = new Antena();

        // アンテナの種族番号  解析.txtにあるとおりこれがないとレベルアップ時にアンテナが消えるため、必要
        private int antenaGenreIndex = 0;
        private int bindAntenaGenreIndex = 0;


        public void Initialize(Int32 address, Dictionary<int, BitmapImage> images, BitmapImage faceTexture, BitmapImage foreLockTexture, BitmapImage faceOutlineTexture, BitmapImage faceHeadTexture, BitmapImage faceWaveTexture, BitmapImage bodyTexture)
        {

            denpaAddress = address;

            antena.Init(images,1.7);

            eye = new FacePart();
            eye.Initialize(FacePartType.Eye,0, 0, 32, 32, faceTexture);
            eye.SetMirror(13);

            eyebrow = new FacePart();
            eyebrow.Initialize(FacePartType.Eyebrow, 384, 0, 32, 32, faceTexture);
            eyebrow.SetMirror(18);
            //eyebrow.SetMirror(25);

            mouth = new FacePart();
            mouth.Initialize(FacePartType.Mouth,128, 0, 64, 32, faceTexture);

            nose = new FacePart();
            nose.Initialize(FacePartType.Nose,320, 0, 32, 32, faceTexture);

            cheek = new FacePart();
            cheek.Initialize(FacePartType.Cheek,416, 128, 32, 32, faceTexture);
            cheek.SetMirror(18);

            cover = new FacePart();
            cover.Initialize(FacePartType.Cover,448, 0, 32, 32, faceTexture);
            cover.SetMirror(15);

            forelock = new FacePart();
            forelock.Initialize(FacePartType.Forelock,0, 0, 64, 64, foreLockTexture,1.3);


            faceOutline = new FacePart();
            faceOutline.ActiveCropHorizontal(); // 横に並んでるので
            faceOutline.Initialize(FacePartType.FaceOutline,0, 0, 64, 128, faceOutlineTexture,0.65);
            faceOutline.SetMirror(30);


            faceHead = new FacePart();
            faceHead.ActiveCropHorizontal(); // 横に並んでるので
            faceHead.Initialize(FacePartType.FaceHead, 0, 0, 255, 230, faceHeadTexture,0.7);


            faceHead.SetBodyColorTexture(faceWaveTexture);

            body = new FacePart();
            body.Initialize(FacePartType.Body, 0, 0, 168, 197, bodyTexture);
            body.SetBodyColorTexture(faceWaveTexture);

            antena.SetDrawOffset(100,30);
            faceHead.SetDrawOffset(0, 3);
            faceOutline.SetDrawOffset(0, 12);
            cheek.SetDrawOffset(0, 22);
            eye.SetDrawOffset(0, 10);
            eyebrow.SetDrawOffset(0, 5);
            nose.SetDrawOffset(0, 18);
            mouth.SetDrawOffset(0, 30);
            cover.SetDrawOffset(0, 10);
            forelock.SetDrawOffset(1, 11);
            body.SetDrawOffset(0, -120);

        }

        public void DataSet(MainWindow mainwindow, byte[] sav)

        {
            // 名前
            name = ReadString(denpaAddress, sav);

            // 性格
            personalityIndex =  ReadValue(79 + denpaAddress, 1, sav);

            // 個体値？
            individualIndex = ReadValue(91 + denpaAddress, 1, sav);

            // アンテナ
            antena.SetIndex(ReadValue(62 + denpaAddress, 1, sav));
            antenaGenreIndex = ReadValue(88 + denpaAddress, 1, sav);

            // 身長
            bodyHeight = ReadValue(72 + denpaAddress, 1, sav);

            body.setBodyHeight(bodyHeight);
            setBodyHeight(bodyHeight);

            // 色
            colorIndex = ReadValue(74 + denpaAddress, 1, sav);

            eye.SetBodyColorIndex(colorIndex);

            // 色を設定
            faceHead.SetBodyColorIndex(colorIndex);
            body.SetBodyColorIndex(colorIndex);

            // 肌色
            skinColorIndex = ReadValue(75 + denpaAddress, 1, sav);
            faceOutline.SetSkinColorIndex(skinColorIndex);

            // 前髪色
            hearColorIndex = ReadValue(76 + denpaAddress, 1, sav);
            forelock.SetHearColorIndex(hearColorIndex);


            // 開始パラメータの保存
            bindName = name;
            bindBodyHeight = bodyHeight;
            bindcolorIndex = colorIndex;
            bindHearColorIndex =hearColorIndex;
            bindSkinColorIndex =skinColorIndex;
            bindPersonalityIndex = personalityIndex;
            bindIndividualIndex = individualIndex;
            bindAntenaGenreIndex = antenaGenreIndex;

            debugIndex = ReadValue(kDebugCorsorIndex + denpaAddress, 1, sav);

            // ===　顔パーツ

            // 頭
            PartDataSet(faceHead, 63, denpaAddress, sav);


            // 輪郭、肌
            PartDataSet(faceOutline, 64, denpaAddress, sav);      

            // 前髪
            PartDataSet(forelock, 65, denpaAddress, sav);
  

            // 顔セット
            PartDataSet(eyebrow, 66, denpaAddress, sav);
            PartDataSet(eye, 67, denpaAddress, sav);
            PartDataSet(nose, 68, denpaAddress, sav);
            PartDataSet(mouth, 69, denpaAddress, sav);
            PartDataSet(cheek, 70, denpaAddress, sav);
            PartDataSet(cover, 71, denpaAddress, sav);

        }

        public void ChangeProcess(MainWindow mainwindow)
        {
            // テキストボックスだいにゅー
            mainwindow.NameTextBox.Text = name;
            mainwindow.AntenaComboBox.SelectedValue = antena.GetIndex();
            mainwindow.HearColorTextBox.Text = hearColorIndex.ToString();
            mainwindow.SkinColorTextBox.Text = skinColorIndex.ToString();
            mainwindow.BodyHeightSlider.Value = bodyHeight;
            mainwindow.PersonalityComboBox.SelectedValue = personalityIndex;
            mainwindow.DebugTextBox.Text = debugIndex.ToString();
            mainwindow.bodyHeight.Text = bodyHeight.ToString();
        }

        // パーツdataのセット
        void PartDataSet(FacePart f, int startIndexAddress, Int32 charactorAddress, byte[] sav)
        {

            int index = ReadValue(charactorAddress + startIndexAddress, 1, sav);

            // ここにかきまーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーす！
            // １２種以外はさよーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーならｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗ
            if (f.GetType() == FacePartType.FaceOutline && index > 12)
            {
                index = 0;
            }

            f.SetIndex(index);
        }

        // 各パーツをキャンバスに描画する
        public void draw(Canvas c)
        {
            c.Children.Clear();

            antena.Draw(c);

            faceHead.Draw(c);
            faceOutline.Draw(c);

            cheek.Draw(c);
            eye.Draw(c);
            eyebrow.Draw(c);
            nose.Draw(c);
            mouth.Draw(c);
            cover.Draw(c);

            forelock.Draw(c);

            body.Draw(c);

            //antena.Draw(c,new Point(100, 30));


        }

        public void ChangeAntena(int index)
        {
            antena.SetIndex(index);

            // アンテナ管理番号に対応したジャンル番号を取得
            antenaGenreIndex = AntenaVariation.antenaDatas[index].index;
        }

        // いんでくすチェンジ
        public void ChangeParts(FacePartType type, int index)
        {
            switch (type)
            {
                case FacePartType.Eye:
                    eye.SetIndex(index);
                    break;
                case FacePartType.Eyebrow:
                    eyebrow.SetIndex(index);
                    break;
                case FacePartType.Nose:
                    nose.SetIndex(index);
                    break;
                case FacePartType.Mouth:
                    mouth.SetIndex(index);
                    break;
                case FacePartType.Cheek:
                    cheek.SetIndex(index);
                    break;
                case FacePartType.Cover:
                    cover.SetIndex(index);
                    break;
                case FacePartType.Forelock:
                    forelock.SetIndex(index);
                break;
                case FacePartType.FaceOutline:
                    faceOutline.SetIndex(index);
                    break;
                case FacePartType.FaceHead:
                    faceHead.SetIndex(index);
                    break;
            }
        }

        // 顔パーツ全て初期化
        public void PartsReset()
        {
            cheek.ResetIndex();
            eye.ResetIndex();
            eyebrow.ResetIndex();
            nose.ResetIndex();
            mouth.ResetIndex();
            cover.ResetIndex();
            forelock.ResetIndex();
            faceOutline.ResetIndex();
            faceHead.ResetIndex();

         
        }

        // 名前とか初期化
        public void ParamsReset(MainWindow mainwindow)
        {
            name = bindName;
            headIndex = bindHeadIndex;
            colorIndex = bindcolorIndex;
            hearColorIndex = bindHearColorIndex;

            skinColorIndex = bindSkinColorIndex;

            personalityIndex = bindPersonalityIndex;

            individualIndex = bindIndividualIndex;

            bodyHeight = bindBodyHeight;

            antenaGenreIndex = bindAntenaGenreIndex;

            setColorIndex(colorIndex);
            setSkinColorIndex(skinColorIndex);
            setHearColorIndex(hearColorIndex);
            antena.ResetIndex();

            ChangeProcess(mainwindow);
        }

        // すべてのでーたをバイナリに書き込む
        public void WriteAllDatas(byte[] sav)
        {
            // 名前
            WriteName(denpaAddress, name, sav);

            // 個体値？
            WriteValue(91 + denpaAddress, individualIndex, 1, sav);

            // 性格
            WriteValue(79 + denpaAddress, personalityIndex, 1, sav);

            // アンテナ
            WriteValue(62 + denpaAddress, antena.GetIndex(), 1, sav);

            // のジャンル
            WriteValue(88 + denpaAddress, antenaGenreIndex, 1, sav);

            // もしアンテナの種類を変更してたら
            if (antenaGenreIndex != bindAntenaGenreIndex)
            {

                // もしそれがタイプ別、単体攻撃のだい４進化系のだとしたら、０１を保存
                // たぶんあってる　QR３人に共通してたし・・
                if (antena.GetIndex() >= 181 &&
                    antena.GetIndex() <= 188)
                {
                    WriteValue(89 + denpaAddress, 1, 1, sav);
                }
            }

            // 身長
            WriteValue(72 + denpaAddress, bodyHeight, 1, sav);

            // 色
            WriteValue(74 + denpaAddress, colorIndex, 1, sav);

            // 肌
            WriteValue(75 + denpaAddress, skinColorIndex, 1, sav);

            // 髪色
            WriteValue(76 + denpaAddress, hearColorIndex, 1, sav);

            // 顔セット
            WriteValue(63 + denpaAddress, faceHead.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 64, faceOutline.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 65, forelock.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 66, eyebrow.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 67, eye.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 68, nose.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 69, mouth.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 70, cheek.GetIndex(), 1, sav);
            WriteValue(denpaAddress + 71, cover.GetIndex(), 1, sav);

            /* デバッグ用！！！！ */
            /* Mainwindow.xamlのほうの３０５行目　デバッグTextBoxのvisibleもtrueでかけます */
            //if (denpaAddress != 0x13A50)
            //{
            //    WriteValue(kDebugCorsorIndex + denpaAddress, debugIndex, 1, sav);
            //    WriteValue(89 + denpaAddress, 2, 1, sav);
            //    //WriteValue(93 + denpaAddress, 01, 1, sav);
            //    WriteValue(92 + denpaAddress, 1, 1, sav);
            //}

        }

        public void setName(string n) { name = n; }
        public string getName() { return name; }
        public void setColorIndex(int i) { 

            colorIndex = i;

            // 色を設定
            faceHead.SetBodyColorIndex(colorIndex);
            body.SetBodyColorIndex(colorIndex);
        }

        public void setHearColorIndex(int i)
        {

            hearColorIndex = i;

            // 色を設定
            forelock.SetHearColorIndex(i);
        }

        public void setDebugIndex(int i)
        {
            debugIndex = i;
        }
        public int getDebugIndex() { return debugIndex; }
        // そのままfacePartにスロー
        // facePart側でも計算
        public void setSkinColorIndex(int i) { 

            // 範囲外ならまた０に戻ってループw
            skinColorIndex = i % BodyColorVariation.skinColorRgb.Count;

            // 色を設定
            faceOutline.SetSkinColorIndex(i);
        }

        public void setBodyHeight(int h)
        {

            bodyHeight = h;

            body.setBodyHeight(bodyHeight);

            // 座標かえるので全部のパーツを更新
            UpdatePosition();

        }
        public void setPersonality(int h)
        {
            personalityIndex = h;
        }

        public void setIndividual(int i)
        {

            individualIndex = i;
        }

        public void UpdatePosition()
        {

            // 計算

            // ０が高い判定なので反転
            double normalized =
                1.0 - (double)bodyHeight / DenpaMen.kMaxDenpaMenBodyHeight;

            double min = 65;
            double max = -27;

            // 身長分ずらす
            double offsetY = Remap(normalized, min, max);

            // 新しい座標設
            // 頭
            faceHead.setBasePosition(
                new Point(
                    faceHead.GetOffsetPoint().X,
                    faceHead.GetOffsetPoint().Y + offsetY
                )
            );
            faceHead.UpdatePosition();

            // 輪郭
            faceOutline.setBasePosition(
                new Point(
                    faceOutline.GetOffsetPoint().X,
                    faceOutline.GetOffsetPoint().Y + offsetY
                )
            );
            faceOutline.UpdatePosition();

            // 前髪
            forelock.setBasePosition(
                new Point(
                    forelock.GetOffsetPoint().X,
                    forelock.GetOffsetPoint().Y + offsetY
                )
            );
            forelock.UpdatePosition();

            // 眉毛
            eyebrow.setBasePosition(
                new Point(
                    eyebrow.GetOffsetPoint().X,
                    eyebrow.GetOffsetPoint().Y + offsetY
                )
            );
            eyebrow.UpdatePosition();

            // 目
            eye.setBasePosition(
                new Point(
                    eye.GetOffsetPoint().X,
                    eye.GetOffsetPoint().Y + offsetY
                )
            );
            eye.UpdatePosition();

            // 鼻
            nose.setBasePosition(
                new Point(
                    nose.GetOffsetPoint().X,
                    nose.GetOffsetPoint().Y + offsetY
                )
            );
            nose.UpdatePosition();

            // 口
            mouth.setBasePosition(
                new Point(
                    mouth.GetOffsetPoint().X,
                    mouth.GetOffsetPoint().Y + offsetY
                )
            );
            mouth.UpdatePosition();

            // ほっぺ
            cheek.setBasePosition(
                new Point(
                    cheek.GetOffsetPoint().X,
                    cheek.GetOffsetPoint().Y + offsetY
                )
            );
            cheek.UpdatePosition();

            // 被り物
            cover.setBasePosition(
                new Point(
                    cover.GetOffsetPoint().X,
                    cover.GetOffsetPoint().Y + offsetY
                )
            );
            cover.UpdatePosition();

            // 体
            body.setBasePosition(
                new Point(
                    body.GetOffsetPoint().X,
                    body.GetOffsetPoint().Y + offsetY
                )
            );
            body.UpdatePosition();

            //アンテナ
            antena.setBasePosition(
                new Point(
                    antena.GetOffsetPoint().X,
                    antena.GetOffsetPoint().Y + offsetY
                )
            );

        }

        // 読み取り
        private int ReadValue(int offset, int byteCount, byte[] sav)
        {
            int value = 0;

            for (int i = 0; i < byteCount; i++)
            {
                value |= sav[offset + i] << (8 * i);
            }

            return value;
        }

        // 00 00　まで読む
        private string ReadString(int address, byte[] sav)
        {
            List<byte> bytes = new List<byte>();

            while (true)
            {
                byte b1 = sav[address];
                byte b2 = sav[address + 1];

                // UTF-16LEの終端 00 00
                if (b1 == 0x00 && b2 == 0x00)
                {
                    break;
                }

                bytes.Add(b1);
                bytes.Add(b2);

                address += 2;
            }

            return Encoding.Unicode.GetString(bytes.ToArray());
        }

        ///　senkei
        double Remap(double value, double min, double max)
        {
            return min + value * (max - min);
        }
        /// <summary>
        /// 指定アドレスに指定バイト数だけ書き込む
        /// </summary>
        /// <param name="offset">書き込み先アドレス</param>
        /// <param name="value">書き込む値</param>
        /// <param name="byteCount">書き込むバイト数(1～4)</param>
        private void WriteValue(int offset, int value, int byteCount, byte[] saveData)
        {
            for (int i = 0; i < byteCount; i++)
            {
                saveData[offset + i] = (byte)((value >> (8 * i)) & 0xFF);
            }
        }
        void WriteName(int address, string name, byte[] sav)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(name);

            int maxBytes = 12; // 最大6文字

            Array.Clear(sav, address, maxBytes);

            Array.Copy(bytes, 0, sav, address, Math.Min(bytes.Length, maxBytes));
        }
    }
}
 
    
