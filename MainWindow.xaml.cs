using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Xml.Linq;
using static Denpa3SavEditor.AntenaVariation;

//using System.Windows.Shapes;
using static Denpa3SavEditor.DenpaMen;

namespace Denpa3SavEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, BitmapImage> antenaTextureImages= new();
        BitmapImage faceTextureImage;
        BitmapImage foreLockTextureImage;
        BitmapImage faceOutlineTextureImage;
        BitmapImage faceHeadTextureImage;
        BitmapImage waveTextureImage;

        BitmapImage bodyTextureImage;
        BitmapImage bodyColorIconTextureImage;

        List<Image> bodyColorIcons = new List<Image>();

        const int kDenpaMenDataMaxBytes = 0x9c; // 電波人間のデータの最大バイト数 (10進数で156byte)

        // パーティーの電波人間のリスト
        List<DenpaMen> partyDenpamens = new();

        const Int32 kPartyStartAddress = 0x13A50; // 電波人間のパーティーデータが始まるアドレス
        const int kPartyDenpamenMaxCount = 8; // パーティーの最大電波人間の数
        int currentPartyCount = 0; // 今読み込んでるパーティーの人数数え

        // ボックスの電波人間のリスト
        List<DenpaMen> boxDenpamens = new(); // 余裕を持ってもっとく

        const Int32 kBoxStartAddress = 0x14410; // 電波人間のボックスデータが始まるアドレス

        // 初期化できたか
        private static bool readSaveDated = false;


        enum previewMode
        {
            PreviewParty,
            PreviewBox
        }
        previewMode currentViewMode = previewMode.PreviewParty;

        // 辞書?
        Dictionary<previewMode, List<DenpaMen>> view;

        private void InitializeView()
        {
                    view = new Dictionary<previewMode, List<DenpaMen>>
            {
                { previewMode.PreviewParty, partyDenpamens },
                { previewMode.PreviewBox, boxDenpamens }
            };
        }

        string[] viewText = { "パーティー", "ボックス内" };

        // 現在選択してる電波人間
        int currentDenpamenIndex = 0;

        // 自動でタブ内のボタンの初期化などする
        struct selectTabParts
        {
            private int kPartsMaxCount;
            
            public void Init(FacePartType type,int offX,int offY, int w,int h,BitmapImage faceTextureImage, WrapPanel panel, MainWindow mainWindow, int maxCount, bool clipHorizontal = false)
            {

                kPartsMaxCount = maxCount;

                for (int i = 0; i < kPartsMaxCount; i++)
                {
                    FacePart eye = new FacePart();

                    eye.Initialize(type,offX, offY, w, h, faceTextureImage, 0.8);

                    if (clipHorizontal) {
                        eye.ActiveCropHorizontal();
                    }


                    eye.SetIndex(i);
                    
                    // ボタン追加
                    panel.Children.Add(CreateImageButton(eye,type,mainWindow, i));

                }
            }
           
            // 決まったレイアウトのボタンを返す関数
            private Button CreateImageButton(FacePart part, FacePartType typ, MainWindow mainWindow,int indx)
            {
                // imageつきボタン作成
                Button button = new Button();

                button.Width = 38;
                button.Height = 38;

                button.Margin = new Thickness(3);
                button.Background = Brushes.White;

                //Image image = new Image();
                //image.Source = part.GetCropImage().Source;

                Image image = part.GetCropImage();

                image.Width = 30;
                image.Height = 30;
                image.Stretch = Stretch.Uniform;

                button.Content = image;

                button.Tag = (index: indx, type: typ);

                // 押されたときの登録
                button.Click += (sender, e) =>
                {
                    Button clickedButton = (Button)sender;

                    var data = ((int index, FacePartType type))((Button)sender).Tag;

                    if (!readSaveDated) return;

                    // ★現在選択中のDenpaMenを取得
                    DenpaMen denpamen =
                        mainWindow.view[mainWindow.currentViewMode][mainWindow.currentDenpamenIndex];

                    // 電波人間のフォルムチェンジ！！！！！！！！！！！！！
                    denpamen.ChangeParts(data.type, data.index);

                };

                return button;
            }
        }

        

        // セレクトタブの
        selectTabParts eyePart;
        selectTabParts eyebrowPart;
        selectTabParts nosePart;
        selectTabParts mouthPart;
        selectTabParts cheekPart;
        selectTabParts coverPart;
        selectTabParts forelockPart;

        selectTabParts faceHeadPart;
        selectTabParts faceOutlinePart;

        public MainWindow()
        {
            InitializeComponent();

            // 顔用テキスチャ読込み
            faceTextureImage = new BitmapImage();

            faceTextureImage.BeginInit();
            faceTextureImage.UriSource = new Uri("pack://application:,,,/Assets/faceTexture.png", UriKind.Absolute);
            faceTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            faceTextureImage.EndInit();

            // 前髪用テキスチャ読込み
            foreLockTextureImage = new BitmapImage();

            foreLockTextureImage.BeginInit();
            foreLockTextureImage.UriSource = new Uri("pack://application:,,,/Assets/forelockTexture.png", UriKind.Absolute);
            foreLockTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            foreLockTextureImage.EndInit();

            // 頭の形テキスチャ読込み
            faceOutlineTextureImage = new BitmapImage();

            faceOutlineTextureImage.BeginInit();
            faceOutlineTextureImage.UriSource = new Uri("pack://application:,,,/Assets/outLineTexture.png", UriKind.Absolute);
            faceOutlineTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            faceOutlineTextureImage.EndInit();

            // 体テキスチャ読込み
            bodyTextureImage = new BitmapImage();

            bodyTextureImage.BeginInit();
            bodyTextureImage.UriSource = new Uri("pack://application:,,,/Assets/bodyTexture.png", UriKind.Absolute);
            bodyTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            bodyTextureImage.EndInit();

            // 頭のアウトラインテキスチャ読込み
            faceHeadTextureImage = new BitmapImage();

            faceHeadTextureImage.BeginInit();
            faceHeadTextureImage.UriSource = new Uri("pack://application:,,,/Assets/headTexture.png", UriKind.Absolute);
            faceHeadTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            faceHeadTextureImage.EndInit();

            // 頭の柄テキスチャ
            waveTextureImage = new BitmapImage();

            waveTextureImage.BeginInit();
            waveTextureImage.UriSource = new Uri("pack://application:,,,/Assets/wave.png", UriKind.Absolute);
            waveTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            waveTextureImage.EndInit();

            // 選択画面に表示するカラーアイコン
            int colorNum = 33;
            int kIconSize = 32;

            bodyColorIconTextureImage = new BitmapImage();

            bodyColorIconTextureImage.BeginInit();
            bodyColorIconTextureImage.UriSource = new Uri("pack://application:,,,/Assets/bodyColorIcon.png", UriKind.Absolute);
            bodyColorIconTextureImage.CacheOption = BitmapCacheOption.OnLoad;
            bodyColorIconTextureImage.EndInit();

            for (int i = 0; i < colorNum; i++)
            {
                Image img = new Image();

                // 切り取って
                Int32Rect rect = new Int32Rect(
                    i * kIconSize,
                    0,
                    kIconSize,kIconSize
                );

                CroppedBitmap crop = new CroppedBitmap(
                    bodyColorIconTextureImage,
                    rect
                );

                img.Source = crop;
                img.Width = kIconSize;
                img.Height = kIconSize;

                // ボタン追加
                BodyColorPickPanel.Children.Add(CreateBodyIconButton(i,img));

            }


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            string resourcePrefix = "Denpa3SavEditor.Assets.antena.";

            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (!resourceName.StartsWith(resourcePrefix) ||
                    !resourceName.EndsWith(".png"))
                {
                    continue;
                }

                string fileName = resourceName.Substring(resourcePrefix.Length);

                string[] numbers =
                    Path.GetFileNameWithoutExtension(fileName).Split(',');

                using Stream? stream =
                    assembly.GetManifestResourceStream(resourceName);

                if (stream == null)
                {
                    continue;
                }

                BitmapImage image = new BitmapImage();

                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();

                foreach (string number in numbers)
                {
                    if (int.TryParse(number, out int index))
                    {
                        antenaTextureImages[index] = image;
                    }
                }
            }
            // コンボボックスに登録
            var view = new ListCollectionView(AntenaVariation.antenaDatas.ToList());

            view.GroupDescriptions.Add(
                new PropertyGroupDescription("Value.Genre")
            );

            AntenaComboBox.ItemsSource = view;

            AntenaComboBox.SelectedIndex = 0;

            // コンボボックスに登録
            PersonalityComboBox.ItemsSource = PersonalityVariation.list;
            PersonalityComboBox.DisplayMemberPath = "Value";
            PersonalityComboBox.SelectedValuePath = "Key";

            PersonalityComboBox.SelectedIndex = 0;

            // 辞書？の初期化
            InitializeView();

            eyePart.Init( FacePartType.Eye, 0, 0, 32, 32, faceTextureImage, FacePartsEyePanel, this, 64);
            eyebrowPart.Init( FacePartType.Eyebrow, 384, 0, 32, 32, faceTextureImage, FacePartsEyebrowPanel, this, 20);
            mouthPart.Init( FacePartType.Mouth, 128, 0, 64, 32, faceTextureImage, FacePartsMouthPanel, this, 48);
            nosePart.Init( FacePartType.Nose, 320, 0, 32, 32, faceTextureImage, FacePartsNosePanel, this, 32);
            cheekPart.Init( FacePartType.Cheek, 416, 128, 32, 32, faceTextureImage, FacePartsCheekPanel, this, 12);
            coverPart.Init( FacePartType.Cover, 448, 0, 32, 32, faceTextureImage, FacePartsCoverPanel, this, 32);
            forelockPart.Init( FacePartType.Forelock, 0, 0, 64, 64, foreLockTextureImage, FacePartsForelockPanel, this, 32);

            faceHeadPart.Init( FacePartType.FaceHead, 0, 0, 255, 230, faceHeadTextureImage, FacePartsFaceHeadPanel, this, 39, true);

            faceOutlinePart.Init( FacePartType.FaceOutline, 0, 0, 64, 128, faceOutlineTextureImage, FacePartsFaceOutlinePanel, this, 12, true);

            BodyHeightSlider.Minimum = 0;
            BodyHeightSlider.Maximum = DenpaMen.kMaxDenpaMenBodyHeight;
            BodyHeightSlider.IsDirectionReversed = true;

        }

        // お金系のdataの読込
        public void ReadSaveData()
        {
            // === お金セット
            //int money = ReadValue(0x656, 3) / 4;
            // MoneyTextBox.Text = money.ToString();

            //  飼いビット (0100で１ｇでした
            long moneyLow = (saveData[0x656] >> 2) & 0x3F;
            long moneyMid = saveData[0x657];
            long moneyHigh = saveData[0x658];
            long moneyTop = saveData[0x659] & 0x1F;

            long money =
                moneyLow
                + moneyMid * 64
                + moneyHigh * 16384
                + moneyTop * 4194304;

            MoneyTextBox.Text = money.ToString();

            // === ジュエル

            // ジュエルの下位3bitを取り出す
            // 上位５ビット目からがジュエルと判明
            //0000  0000
            //000 0  0000
            // ↑ ここ

            int jewelLow = (saveData[0x659] >> 5) & 0x07;

            int jewelHigh =
                saveData[0x65A] |
                (saveData[0x65B] << 8);

            int jewel = jewelHigh * 8 + jewelLow;

            JewelTextBox.Text = jewel.ToString();
        }
        private void WriteJewel(int jewel)
        {
            // 0x659 の下位3bitに入る部分（0～7）
            int jewelLow = jewel & 0x07;

            // 8以上の部分
            int jewelHigh = jewel >> 3;

            // 0x659 の上位3bitだけを書き換える
            // 下位5bit（お金側）はそのまま残す
            saveData[0x659] =
                (byte)((saveData[0x659] & 0x1F) | (jewelLow << 5));

            // 上位部分を 0x65A～0x65B に書き込む
            saveData[0x65A] = (byte)(jewelHigh & 0xFF);
            saveData[0x65B] = (byte)((jewelHigh >> 8) & 0xFF);
        }
        private void WriteMoney(int money)
        {
            // 各バイトに分解
            long moneyLow = money % 64;
            long moneyMid = (money / 64) % 256;
            long moneyHigh = (money / 16384) % 256;
            long moneyTop = (money / 4194304) % 32;

            // 0x656
            // bit0～1は別データなので保持
            saveData[0x656] = (byte)(
                (saveData[0x656] & 0x03)
                | (moneyLow << 2)
            );

            // 0x657
            saveData[0x657] = (byte)moneyMid;

            // 0x658
            saveData[0x658] = (byte)moneyHigh;

            // 0x659
            // bit5～7はジュエルなので保持
            saveData[0x659] = (byte)(
                (saveData[0x659] & 0xE0)
                | moneyTop
            );
        }
        // 読み込むセーブデータ
        byte[] saveData;

        // 読み込んだセーブデータのファイルパス
        private string filePath;

        /// <summary>
		/// ファイルを開くダイアログボックスを表示
		/// </summary>
		private void button1_Click(object sender, RoutedEventArgs e)
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "バイナリファイル (*.bin)|*.bin|全てのファイル (*.*)|*.*";


            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {

                filePath = dialog.FileName;

                // ファイルのバイナリ読み込んで保持
                saveData = File.ReadAllBytes(dialog.FileName);

                ReadSaveData();

                // 一旦削除
               // DrawCanvas.Children.Clear();


                partyDenpamens.Clear();

                // ぱーてぃーよみこみ
                for (int i = 0; i < kPartyDenpamenMaxCount; i++)
                {
                    int address = kPartyStartAddress + i * kDenpaMenDataMaxBytes;

                    Debug.WriteLine(
                       $"DenpaMen[{i}] StartAddress = 0x{address:X}"
                    );

                    // もし00 00なら空データなので退散
                    if (ReadValue(address, 2) == 0)
                    {
                        break;
                    }

                    var denpaMen = new DenpaMen();
                    denpaMen.Initialize(
                    address,
                    antenaTextureImages,
                    faceTextureImage,
                    foreLockTextureImage,
                    faceOutlineTextureImage,
                    faceHeadTextureImage,
                    waveTextureImage,
                    bodyTextureImage
                    );

                    partyDenpamens.Add(denpaMen);

                }

                // ぱーてぃーのdataよみこみ
                foreach (var denpaMen in partyDenpamens)
                {
                    denpaMen.DataSet(this, saveData);
                }


                // 2人ぶんで抜けるためのフラグ
                bool isTwoConsecutiveEmpty = false;

                boxDenpamens.Clear();

                // ボックスよみこみ
                for (int i = 0; i < 200; i++) // 適当w
                {
                    int address = kBoxStartAddress + i * kDenpaMenDataMaxBytes;

                    // もし00 00なら空データなので退散
                    if (ReadValue(address, 2) == 0)
                    {

                        // 2回連続なら流石に空なので抜ける
                        if (isTwoConsecutiveEmpty)
                        {
                            break;
                        }
                        else
                        {
                            isTwoConsecutiveEmpty = true;
                            continue;
                        }

                    }
                    else
                    {
                        isTwoConsecutiveEmpty = false;
                    }

                    var denpaMen = new DenpaMen();
                    denpaMen.Initialize(
                        address,
                        antenaTextureImages,
                        faceTextureImage,
                        foreLockTextureImage,
                        faceOutlineTextureImage,
                        faceHeadTextureImage,
                        waveTextureImage,
                        bodyTextureImage
                    );

                    boxDenpamens.Add(denpaMen);
                }

                // ボックスのdataよみこみ
                foreach (var denpaMen in boxDenpamens)
                {
                    denpaMen.DataSet(this, saveData);
                }

                // textBox更新
                view[currentViewMode][currentDenpamenIndex].ChangeProcess(this);

                view[currentViewMode][currentDenpamenIndex].UpdatePosition();

                // 現在の奴だけ見る
                view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);

                // 右側の文字更新
                CurrentPreviewModeView.Text = viewText[(int)currentViewMode];


                readSaveDated = true;

            }
        }

        /// <summary>
        /// 名前を付けて保存ダイアログボックスを表示
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (saveData == null || string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("先にファイルを開くにょ");
                return;
            }

            if (!int.TryParse(MoneyTextBox.Text, out int money))
            {
                MessageBox.Show("（お金が何も書いて）ないです。");
                return;
            }

            if (!int.TryParse(JewelTextBox.Text, out int jewel))
            {
                MessageBox.Show("（ジュエルが何も書いて）ない！");
                return;
            }

            //WriteValue(0x656, money * 4, 3);
            //  WriteValue(0x659, jewel * 32, 2);
            WriteMoney(money);
            WriteJewel(jewel);

            // 電波人間書き込み
            foreach (var denpaMen in partyDenpamens)
            {
                denpaMen.WriteAllDatas(saveData);
            }

            foreach (var denpaMen in boxDenpamens)
            {
                denpaMen.WriteAllDatas(saveData);
            }

            File.WriteAllBytes(filePath, saveData);

            MessageBox.Show("保存したにょ");
        }

        // <
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDenpamenIndex > 0)
            {
                currentDenpamenIndex--;

                //DrawCanvas.Children.Clear();

                // 現在の電波情報を画面のに適用する
                view[currentViewMode][currentDenpamenIndex].ChangeProcess(this);

                // 現在の奴だけ見る
                view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);

            }
        }

        // ＞
        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            int maxIndex = view[currentViewMode].Count - 1; 
            
            if (currentDenpamenIndex < maxIndex)
            {
                currentDenpamenIndex++;

               // DrawCanvas.Children.Clear();

                // 現在の電波情報を画面のに適用する
                view[currentViewMode][currentDenpamenIndex].ChangeProcess(this);

                // 現在の奴だけ見る
                view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);
            }
        }

        // モード変更
        private void ChangeViewMode_Click(object sender, RoutedEventArgs e)
        {

            if (!readSaveDated) return;

            // 三こう演算子????!!ww
            currentViewMode = currentViewMode == previewMode.PreviewParty ? previewMode.PreviewBox : previewMode.PreviewParty;

            // 右側の文字更新
            CurrentPreviewModeView.Text = viewText[(int)currentViewMode];


           // DrawCanvas.Children.Clear();

            currentDenpamenIndex = 0;

            // 現在の電波情報を画面のに適用する
            view[currentViewMode][currentDenpamenIndex].ChangeProcess(this);

            // 現在の奴だけ見る
            view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);
        }

        // 顔パーツ初期化
        private void FaceReset_Click(object sender, RoutedEventArgs e)
        {

            if (!readSaveDated) return;

            MessageBoxResult result = MessageBox.Show(
                "顔パーツを初期状態に戻していい？",
                "ちゅうい",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                view[currentViewMode][currentDenpamenIndex].PartsReset();
                
            }
        }

        // パラメーター初期化
        private void ParametorReset_Click(object sender, RoutedEventArgs e)
        {
            if (!readSaveDated) return;

            view[currentViewMode][currentDenpamenIndex].ParamsReset(this);
        }

        private void BodyHeightSliderValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (readSaveDated)
            {
                double value = e.NewValue;

                view[currentViewMode][currentDenpamenIndex]
                    .setBodyHeight((int)value);
          
                view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);
            }
        }

        // TextBoxリアルタイム変更
        private void ParametorTextBox_Changed(object sender, RoutedEventArgs e)
        {
            if (!readSaveDated) return;

            TextBox textBox = (TextBox)sender;

            // それぞれのTextBoxにあった処理
            switch (textBox.Name)
            {
                case "NameTextBox":
                        view[currentViewMode][currentDenpamenIndex].setName(textBox.Text);
                        NameTextBox.Text = textBox.Text;
                    break;

                case "ColorTextBox":
                    if (int.TryParse(textBox.Text, out int colorIndex))
                    {
                        view[currentViewMode][currentDenpamenIndex].setColorIndex(colorIndex);
                    }
                    break;

                case "HearColorTextBox":
                    if (int.TryParse(textBox.Text, out int hearColorIndex))
                    {
                        view[currentViewMode][currentDenpamenIndex].setHearColorIndex(hearColorIndex);

                        HearColorTextBox.Text = hearColorIndex.ToString();
                    }
                    break;

                case "SkinColorTextBox":
                    if (int.TryParse(textBox.Text, out int skinColorIndex))
                    {

                        view[currentViewMode][currentDenpamenIndex].setSkinColorIndex(skinColorIndex);

                        SkinColorTextBox.Text = skinColorIndex.ToString();
                    }
                    break;

            }

            
            
           
           
        }
        private void DebugTextBox_Changed(object sender, RoutedEventArgs e)
        {
            if (!readSaveDated) return;


            TextBox textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int Index))
            {
                view[currentViewMode][currentDenpamenIndex].setDebugIndex(Index);
            }
        }

        // コンボボックス切り替え
        private void AntenaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!readSaveDated) return;

            if (AntenaComboBox.SelectedItem is KeyValuePair<int, AntenaInfo> selected)
            {
                int antenaIndex = selected.Key;
                AntenaInfo info = selected.Value;

                view[currentViewMode][currentDenpamenIndex].ChangeAntena(antenaIndex);

                // 個体値も決める（アンテナ無から生やした場合APがなくなるので）
                if (antenaIndex == 0)
                {
                    view[currentViewMode][currentDenpamenIndex].setIndividual(0);
                }
                else
                {
                    // アンテナあり
                    view[currentViewMode][currentDenpamenIndex].setIndividual(2);// 一旦０４とするｗアンテナありにすると４に変更されるので注意？
                }
            }
        }

        private void PersonalityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!readSaveDated) return;

            if (PersonalityComboBox.SelectedItem is KeyValuePair<int, string> selected)
            {
                int index = selected.Key;
                string info = selected.Value;

                view[currentViewMode][currentDenpamenIndex].setPersonality(index);
            }
        }

        // 体のアイコンの奴の
        private Button CreateBodyIconButton(int i, Image img) {

            Button button = new Button();

            button.Width = img.Width;
            button.Height = img.Height;

            button.Margin = new Thickness(3);

            button.Content = img;

            button.Tag = i;

            // 透明にする
            button.Background = Brushes.Transparent;

            // スタイル
            button.Style = (Style)FindResource("IconButtonStyle");

            // 押されたときの登録
            button.Click += (sender, e) =>
            {
                Button clickedButton = (Button)sender;

                int index = (int)clickedButton.Tag;

                if (!readSaveDated) return;

                // 色変更
                view[currentViewMode][currentDenpamenIndex].setColorIndex(index);
            };

            return button;
        }

        // へんしゅう画面出る
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!readSaveDated) return;

            Window window = new Window();

            window.Title = "値を編集";
            window.Width = 400;
            window.Height = 350;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = this;

            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(15);

            TextBox textBox = new TextBox();


            StringBuilder sb = new StringBuilder();

            //　現在の電波の開始アドレス
            Int32 address = view[currentViewMode][currentDenpamenIndex].GetDenpaAddress();

            int start = address;
            int length = kDenpaMenDataMaxBytes;

            for (int i = 0; i < length; i++)
            {
                sb.Append($"{saveData[start + i]:X2} ");

                if ((i + 1) % 16 == 0)
                    sb.AppendLine();
            }

            textBox.Text = sb.ToString();
            textBox.AcceptsReturn = true;

            textBox.Margin = new Thickness(0, 0, 0, 10);

            Button okButton = new Button();
            okButton.Content = "OK";
            okButton.Height = 30;

            okButton.Click += (s, args) =>
            {
                // 入力された値
                string value = textBox.Text;
                

                // 切り抜き？
                string[] values = textBox.Text
                 .Split(new[] { ' ', '\r', '\n', '\t' },
                        StringSplitOptions.RemoveEmptyEntries);

                // 書き込み
                for (int i = 0; i < values.Length; i++)
                {
                    saveData[start + i] = Convert.ToByte(values[i], 16);
                }

                // 再読み込み処理
                view[currentViewMode][currentDenpamenIndex].DataSet(this,saveData);
                view[currentViewMode][currentDenpamenIndex].ChangeProcess(this);
                view[currentViewMode][currentDenpamenIndex].draw(DrawCanvas);

              window.DialogResult = true;
                window.Close();
            };

            panel.Children.Add(textBox);
            panel.Children.Add(okButton);

            window.Content = panel;

            window.ShowDialog();
        }


        /// <summary>
        /// 指定アドレスに指定バイト数だけ書き込む
        /// </summary>
        /// <param name="offset">書き込み先アドレス</param>
        /// <param name="value">書き込む値</param>
        /// <param name="byteCount">書き込むバイト数(1～4)</param>
        private void WriteValue(int offset, int value, int byteCount)
        {
            for (int i = 0; i < byteCount; i++)
            {
                saveData[offset + i] = (byte)((value >> (8 * i)) & 0xFF);
            }
        }

        // 読み取り
        private int ReadValue(int offset, int byteCount)
        {
            int value = 0;

            for (int i = 0; i < byteCount; i++)
            {
                value |= saveData[offset + i] << (8 * i);
            }

            return value;
        }


        // メモ：上の通り0x656からが所持金データで、

        // ジュエルアドレス0x659が20 →1J、所持金が普通
        // ジュエルアドレス0x659が21 →1J、所持金が爆増した

        // ことからたぶん0x659の下一桁が所持金の最上位ビットなのだろうとおもう、しらんけど

        // 所持金とジュエルのバイトの位置が同じで、めんどいので、0x656から3バイトを所持金の上限とするｗ（ジュエルに影響しないため

        private const int MaxValue = 16777215;
        private void JewelTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(JewelTextBox.Text, out int value))
                {
                    if (value > MaxValue)
                    {
                        value = MaxValue;
                    }
                    else if (value < 0)
                    {
                        value = 0;
                    }

                    JewelTextBox.Text = value.ToString();
                }
                else
                {
                    JewelTextBox.Text = "0";
                }
            }
        }
    }
}