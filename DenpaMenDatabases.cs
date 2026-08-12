using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace Denpa3SavEditor
{

    /*
     * 
     *   読み取り専用　
     *   
     *   　　　電波人間の柄と色などおおまとまえた
     *   　　　あとアンテナ
     *   
     *   
     *   データ空間とクラス
     * 
     */


    // 色の三原色
    public struct Rgb
    {
        public byte r, g, b;
        public Rgb(byte R, byte G, byte B)
        {
            r = R;
            g = G;
            b = B;
        }

        public Rgb GetRgb() { return new Rgb(r, g, b); }
    }

    // 色
    public enum Color
    {

        BodyColor_None = 33,// 未設定

        BodyColor_Black = 0,
        BodyColor_Red,
        BodyColor_Cyan,
        BodyColor_Green,
        BodyColor_Orange,
        BodyColor_Yellow,
        BodyColor_Blue,
        BodyColor_White,

        BodyColor_Purple = 29,
        BodyColor_Sliver,
        BodyColor_Gold,
        BodyColor_Pink,
    }



    public struct ColorVariations

    {
        public Color color;
        public Color color2;

        //単色でON
        public bool isSingleColor = false;

        // 単色の場合
        public ColorVariations(Color c1)
        {
            color = c1;
            isSingleColor = true;
        }

        // 柄付き
        public ColorVariations(Color c1, Color c2)
        {
            color = c1;
            color2 = c2;
        }

    }
    public class BodyColorVariation
    {
        // 番号ー＞色の辞書
        public static readonly Dictionary<Color, Rgb> ColorRgb = new()
        {
            { Color.BodyColor_Black,  new Rgb(1, 1, 1) },
            { Color.BodyColor_Red,    new Rgb(250, 10, 10) },
            { Color.BodyColor_Cyan,   new Rgb(10, 200, 250) },
            { Color.BodyColor_Green,  new Rgb(10, 200, 10) },
            { Color.BodyColor_Orange, new Rgb(255, 150, 20) },
            { Color.BodyColor_Yellow, new Rgb(255, 230, 20) },
            { Color.BodyColor_Blue,   new Rgb(30, 100, 255) },
            { Color.BodyColor_White,  new Rgb(230, 235, 235) },

            { Color.BodyColor_Purple, new Rgb(180, 0, 180) },
            { Color.BodyColor_Sliver, new Rgb(180, 180, 180) },
            { Color.BodyColor_Gold,   new Rgb(255, 200, 50) },
            { Color.BodyColor_Pink,   new Rgb(255, 100, 180) },

        };

        // 番号ー＞色の辞書   肌色
        public static readonly Dictionary<int, Rgb> skinColorRgb = new()
        {
                { 0,new Rgb(247,218,198) },
                { 1,new Rgb(255,215,128) },
                { 2,new Rgb(255,190,170) },
                { 3,new Rgb(229,139,80) },
                { 4,new Rgb(210,105,71) },
                { 5,new Rgb(210,105,71) },
                { 6,new Rgb(140,61,41) },
                { 7,new Rgb(10,10,10) },
                { 8,new Rgb(255,255,255) }

        };

        // 柄リスト
        public static readonly List<ColorVariations> bodyColorVariations = new()
        {
            // 0～7：単色
            new(Color.BodyColor_Black),     // 0  黒
            new(Color.BodyColor_Red),       // 1  赤
            new(Color.BodyColor_Cyan),      // 2  水色
            new(Color.BodyColor_Green),     // 3  緑
            new(Color.BodyColor_Orange),    // 4  橙
            new(Color.BodyColor_Yellow),    // 5  黄色
            new(Color.BodyColor_Blue),      // 6  青
            new(Color.BodyColor_White),     // 7  白

            // 8～28：柄付き
            new(Color.BodyColor_Red,    Color.BodyColor_Cyan),    // 8  rc
            new(Color.BodyColor_Red,    Color.BodyColor_Green),   // 9  rg
            new(Color.BodyColor_Red,    Color.BodyColor_Orange),  // 10 rd
            new(Color.BodyColor_Yellow, Color.BodyColor_Red),     // 11 yr
            new(Color.BodyColor_Blue,   Color.BodyColor_Red),     // 12 br
            new(Color.BodyColor_Red,    Color.BodyColor_White),   // 13 rw
            new(Color.BodyColor_Green,  Color.BodyColor_Cyan),    // 14 gc
            new(Color.BodyColor_Orange, Color.BodyColor_Cyan),    // 15 dc
            new(Color.BodyColor_Yellow, Color.BodyColor_Cyan),    // 16 yc
            new(Color.BodyColor_Blue,   Color.BodyColor_Cyan),    // 17 bc
            new(Color.BodyColor_White,  Color.BodyColor_Cyan),    // 18 wc
            new(Color.BodyColor_Green,  Color.BodyColor_Orange),  // 19 gd
            new(Color.BodyColor_Yellow, Color.BodyColor_Green),   // 20 yg
            new(Color.BodyColor_Blue,   Color.BodyColor_Green),   // 21 bg
            new(Color.BodyColor_White,  Color.BodyColor_Green),   // 22 wg
            new(Color.BodyColor_Yellow, Color.BodyColor_Orange),  // 23 yd
            new(Color.BodyColor_Orange, Color.BodyColor_Blue),    // 24 db
            new(Color.BodyColor_White,  Color.BodyColor_Orange),  // 25 wd
            new(Color.BodyColor_Blue,   Color.BodyColor_Yellow),  // 26 by
            new(Color.BodyColor_White,  Color.BodyColor_Yellow),  // 27 wy
            new(Color.BodyColor_Blue,   Color.BodyColor_White),   // 28 bw

            // 29～32：特殊色
            new(Color.BodyColor_Purple),   // 29 紫
            new(Color.BodyColor_Sliver),   // 30 銀
            new(Color.BodyColor_Gold),     // 31 金
            new(Color.BodyColor_Pink),     // 32 ピンク
        };

    }

    public class HearColorVariation
    {
        // 番号ー＞色の辞書   髪色
        public static readonly Dictionary<int, Rgb> HearColorRgb = new() {
            { 0,new Rgb(40,40,40) },
            { 1,new Rgb(255,255,255) },
            { 2,new Rgb(205,144,42) },
            { 3,new Rgb(128,69,7) },
            { 4,new Rgb(148,42,0) },
            { 5,new Rgb(255,221,32) },
            { 6,new Rgb(0,89,255) },
            { 7,new Rgb(9,166,0) },
            { 8,new Rgb(161,40,255) },
            { 9,new Rgb(255,110,128) },
            { 10,new Rgb(248,87,0) },
            { 11,new Rgb(248,0,16) },
            { 12,new Rgb(187,187,187) },
            { 13,new Rgb(187,187,187) },
            { 14,new Rgb(187,187,187) },
            { 15,new Rgb(236,187,187) },

        };
    }

    public class PersonalityVariation
    {
        // 番号ー＞色の辞書
        public static readonly Dictionary<int, string> list = new()
        {
            {0, "♀おっとり系"},
            {1, "♂知的系"},
            {2, "♂熱血系"},
            {3, "♂おっとり系"},
            {4, "♀リア充系"},
            {5, "♂ダンディー系"},
            {6, "♂めんどくさがり系"},
            {100, "特殊-主人公"},
            {101, "特殊-あかり"},
            {102, "特殊-テル"},
            {103, "特殊-ルナ"},
            {104, "特殊-みちる"},
            {105, "特殊-ひのわ"},
            {106, "特殊-なし"},

        };
    }


    public class AntenaVariation
    {

        public struct AntenaInfo
        {
            public int index { get; set; }
            public string Name { get; set; }
            public string Genre { get; set; }



            public AntenaInfo(int i,string name, string genre)
            {
                Name = name;
                Genre = genre;
                index = i;
            }

        }

        /*

        public static readonly Dictionary<int, AntenaInfo> antenaDatas = new()
        {

            { 0, new AntenaInfo( 0,"アンテナ無し", "") },

            // こっから特殊アンテナ
            { 2, new AntenaInfo(1,"お宝2倍", "特殊") },
            { 3, new AntenaInfo(1,"お宝3倍", "特殊") },
            { 4, new AntenaInfo(1,"お宝5倍", "特殊") },

            { 5, new AntenaInfo(2,"レアお宝2倍", "特殊") },
            { 6, new AntenaInfo(2,"レアお宝3倍", "特殊") },
            { 7, new AntenaInfo(2,"レアお宝5倍", "特殊") },

            { 8, new AntenaInfo("ゴールド2倍", "特殊") },
            { 9, new AntenaInfo("ゴールド3倍", "特殊") },
            { 10, new AntenaInfo("ゴールド5倍", "特殊") },

            { 11, new AntenaInfo("ちょっとステルス", "特殊") },
            { 12, new AntenaInfo("そこそこステルス", "特殊") },
            { 13, new AntenaInfo("しばらくステルス", "特殊") },

            { 221, new AntenaInfo("つかまえる", "特殊") },
            { 222, new AntenaInfo("つかまえる2ばい", "特殊") },
            { 223, new AntenaInfo("つかまえる3ばい", "特殊") },


            // こっから補助アンテナ
            { 14, new AntenaInfo("ちょっと回復", "補助") },
            { 15, new AntenaInfo("そこそこ回復", "補助") },
            { 16, new AntenaInfo("全部かいふく", "補助") },

            { 17, new AntenaInfo("みんなちょっと回復", "補助") },
            { 18, new AntenaInfo("みんな回復", "補助") },
            { 19, new AntenaInfo("みんな全部回復", "補助") },

            { 20, new AntenaInfo("ちょっと復活", "補助") },
            { 21, new AntenaInfo("復活", "補助") },
            { 22, new AntenaInfo("かんぜん復活", "補助") },

            { 23, new AntenaInfo("みんなちょっと復活", "補助") },
            { 24, new AntenaInfo("みんな復活", "補助") },
            { 25, new AntenaInfo("みんなかんぜん復活", "補助") },

            { 26, new AntenaInfo("げどく", "補助") },
            { 27, new AntenaInfo("みんなげどく", "補助") },

            { 28, new AntenaInfo("やけどなおす", "補助") },
            { 29, new AntenaInfo("みんなやけどなおす", "補助") },

            { 30, new AntenaInfo("かわかす", "補助") },
            { 31, new AntenaInfo("みんなかわかす", "補助") },

            { 32, new AntenaInfo("かぜなおす", "補助") },
            { 33, new AntenaInfo("みんなかぜなおす", "補助") },

            { 34, new AntenaInfo("どろおとす", "補助") },
            { 35, new AntenaInfo("みんなどろおとす", "補助") },

            { 36, new AntenaInfo("おはらい", "補助") },
            { 37, new AntenaInfo("みんなおはらい", "補助") },

            { 38, new AntenaInfo("かんでんなおす", "補助") },
            { 39, new AntenaInfo("みんなかんでんなおす", "補助") },

            { 40, new AntenaInfo("あたためる", "補助") },
            { 41, new AntenaInfo("あたためる", "補助") },

            { 42, new AntenaInfo("しびれとる", "補助") },
            { 43, new AntenaInfo("みんなしびれとる", "補助") },

            { 44, new AntenaInfo("めざめる", "補助") },
            { 45, new AntenaInfo("みんなめざめる", "補助") },

            { 46, new AntenaInfo("めぐすり", "補助") },
            { 47, new AntenaInfo("みんなめぐすり", "補助") },


            // こっから補強アンテナ
            { 48, new AntenaInfo("むてき", "補強") },
            { 49, new AntenaInfo("みんなむてき", "補強") },

            { 50, new AntenaInfo("みんなこうふん", "補強") },

            { 51, new AntenaInfo("たくわえる", "補強") },
            { 52, new AntenaInfo("みんなたくわえる", "補強") },

            { 53, new AntenaInfo("わざはねかえす", "補強") },
            { 54, new AntenaInfo("みんなわざはねかえす", "補強") },

            { 55, new AntenaInfo("すこしつよくなれ", "補強") },
            { 56, new AntenaInfo("つよくなれ", "補強") },
            { 57, new AntenaInfo("すごくつよくなれ", "補強") },

            { 58, new AntenaInfo("みんなつよめになれ", "補強") },
            { 59, new AntenaInfo("みんなつよくなれ", "補強") },
            { 60, new AntenaInfo("みんなムキムキ", "補強") },

            { 61, new AntenaInfo("すこしかたくなれ", "補強") },
            { 62, new AntenaInfo("かたくなれ", "補強") },
            { 63, new AntenaInfo("すごくかたくなれ", "補強") },

            { 64, new AntenaInfo("みんなかためになれ", "補強") },
            { 65, new AntenaInfo("みんなかたくなれ", "補強") },
            { 66, new AntenaInfo("みんなカチカチ", "補強") },

            { 67, new AntenaInfo("すこしはやくなれ", "補強") },
            { 68, new AntenaInfo("はやくなれ", "補強") },
            { 69, new AntenaInfo("すごくはやくなれ", "補強") },

            { 70, new AntenaInfo("みんなはやめになれ", "補強") },
            { 71, new AntenaInfo("みんなはやくなれ", "補強") },
            { 72, new AntenaInfo("みんなかぜになれ", "補強") },

            { 73, new AntenaInfo("すこしかわしやすい", "補強") },
            { 74, new AntenaInfo("かわしやすい", "補強") },
            { 75, new AntenaInfo("すごくかわしやすい", "補強") },

            { 76, new AntenaInfo("みんなかわしやすい", "補強") },
            { 77, new AntenaInfo("みんなでスルー", "補強") },


            // こっから状態異常アンテナ
            { 78, new AntenaInfo("どくどくシグナル", "状態異常") },
            { 79, new AntenaInfo("どくどくウェーブ", "状態異常") },

            { 80, new AntenaInfo("もうどく", "状態異常") },
            { 82, new AntenaInfo("みんな猛毒", "状態異常") },

            { 83, new AntenaInfo("ねむらせる", "状態異常") },
            { 84, new AntenaInfo("みんなねむらせる", "状態異常") },

            { 85, new AntenaInfo("しびれさせる", "状態異常") },
            { 86, new AntenaInfo("みんなしびれさせる", "状態異常") },

            { 87, new AntenaInfo("めかくし", "状態異常") },
            { 88, new AntenaInfo("みんなめかくし", "状態異常") },

            { 89, new AntenaInfo("ブレスふうじ", "状態異常") },
            { 90, new AntenaInfo("みんなブレスふうじ", "状態異常") },

            { 91, new AntenaInfo("すこしよわくなれ", "状態異常") },
            { 92, new AntenaInfo("よわくなれ", "状態異常") },
            { 93, new AntenaInfo("すごくよわくなれ", "状態異常") },

            { 94, new AntenaInfo("みんなよわめになれ", "状態異常") },
            { 95, new AntenaInfo("よわめになれ", "状態異常") },
            { 96, new AntenaInfo("げきよわ", "状態異常") },

            { 97, new AntenaInfo("すこしやわくなれ", "状態異常") },
            { 98, new AntenaInfo("やわくなれ", "状態異常") },
            { 99, new AntenaInfo("すごくやわくなれ", "状態異常") },

            { 100, new AntenaInfo("みんなやわらかめ", "状態異常") },
            { 101, new AntenaInfo("やわらかめ", "状態異常") },
            { 102, new AntenaInfo("やわやわ", "状態異常") },

            { 103, new AntenaInfo("すこしおそくなれ", "状態異常") },
            { 104, new AntenaInfo("おそくなれ", "状態異常") },
            { 105, new AntenaInfo("すごくおそくなれ", "状態異常") },

            { 106, new AntenaInfo("みんなおそめになれ", "状態異常") },
            { 107, new AntenaInfo("みんなおそくなれ", "状態異常") },
            { 108, new AntenaInfo("みんなのろのろ", "状態異常") },


            // こっから技アンテナ
            { 109, new AntenaInfo("ひのたま", "技") },
            { 110, new AntenaInfo("かえんほうしゃ", "技") },
            { 111, new AntenaInfo("もえさかるごうか", "技") },

            { 112, new AntenaInfo("ばくはつ", "技") },
            { 113, new AntenaInfo("だいばくはつ", "技") },
            { 114, new AntenaInfo("ビッグバン", "技") },

            { 115, new AntenaInfo("やまかじ", "技") },
            { 116, new AntenaInfo("煮えたぎるマグマ", "技") },
            { 117, new AntenaInfo("じごくのごうか", "技") },

            { 118, new AntenaInfo("とがったこおり", "技") },
            { 119, new AntenaInfo("こおりのやいば", "技") },
            { 120, new AntenaInfo("つららミサイル", "技") },

            { 121, new AntenaInfo("ロックアイス", "技") },
            { 122, new AntenaInfo("まんねんごおり", "技") },
            { 123, new AntenaInfo("ダイヤモンドダスト", "技") },

            { 124, new AntenaInfo("あられ", "技") },
            { 125, new AntenaInfo("ふぶき", "技") },
            { 126, new AntenaInfo("もうふぶき", "技") },

            { 127, new AntenaInfo("つむじかぜ", "技") },
            { 128, new AntenaInfo("すなあらし", "技") },
            { 129, new AntenaInfo("サイクロン", "技") },

            { 130, new AntenaInfo("ビル風", "技") },
            { 131, new AntenaInfo("ぼうふう", "技") },
            { 132, new AntenaInfo("たつまき", "技") },

            { 133, new AntenaInfo("かまいたち", "技") },
            { 134, new AntenaInfo("ジェット気流", "技") },
            { 135, new AntenaInfo("ダウンバースト", "技") },

            { 136, new AntenaInfo("らくせき", "技") },
            { 137, new AntenaInfo("どしゃくずれ", "技") },
            { 138, new AntenaInfo("いわなだれ", "技") },

            { 139, new AntenaInfo("いしつぶて", "技") },
            { 140, new AntenaInfo("がんせきおとし", "技") },
            { 141, new AntenaInfo("いんせきらっか", "技") },

            { 142, new AntenaInfo("まぐにチューど3", "技") },
            { 143, new AntenaInfo("まぐにチューど6", "技") },
            { 144, new AntenaInfo("まぐにチューど12", "技") },

            { 145, new AntenaInfo("せいでんき", "技") },
            { 146, new AntenaInfo("でんきしょっく", "技") },
            { 147, new AntenaInfo("スパーク", "技") },

            { 148, new AntenaInfo("いなづま", "技") },
            { 149, new AntenaInfo("かみなり", "技") },
            { 150, new AntenaInfo("せいてんのへきれき", "技") },

            { 151, new AntenaInfo("ひゃくボルト", "技") },
            { 152, new AntenaInfo("いちまんボルト", "技") },
            { 153, new AntenaInfo("いちおくボルト", "技") },

            { 154, new AntenaInfo("みずでっぽう", "技") },
            { 155, new AntenaInfo("てっぽうみず", "技") },
            { 156, new AntenaInfo("だくりゅう", "技") },

            { 157, new AntenaInfo("バケツの水", "技") },
            { 158, new AntenaInfo("たきつぼ", "技") },
            { 159, new AntenaInfo("ジェットスプラッシュ", "技") },

            { 160, new AntenaInfo("たかなみ", "技") },
            { 161, new AntenaInfo("つなみ", "技") },
            { 162, new AntenaInfo("おおつなみ", "技") },

            { 163, new AntenaInfo("よわいこうせん", "技") },
            { 164, new AntenaInfo("まばゆいこうせん", "技") },
            { 165, new AntenaInfo("レーザービーム", "技") },

            { 166, new AntenaInfo("スポットライト", "技") },
            { 167, new AntenaInfo("はじけるせんこう", "技") },
            { 168, new AntenaInfo("フレア", "技") },

            { 169, new AntenaInfo("ふゆのひざし", "技") },
            { 170, new AntenaInfo("まなつのたいよう", "技") },
            { 171, new AntenaInfo("せきどうちょっか", "技") },

            { 172, new AntenaInfo("おどかす", "技") },
            { 173, new AntenaInfo("やみうち", "技") },
            { 174, new AntenaInfo("やみのつかい", "技") },

            { 175, new AntenaInfo("ダークホール", "技") },
            { 176, new AntenaInfo("くろいうず", "技") },
            { 177, new AntenaInfo("ブラックホール", "技") },

            { 178, new AntenaInfo("くろいきり", "技") },
            { 179, new AntenaInfo("たちこめるやみ", "技") },
            { 180, new AntenaInfo("ならくのそこ", "技") },


            // こっから2種属性アンテナ
            { 181, new AntenaInfo("やきつくすほのお", "2種属性") },
            { 182, new AntenaInfo("ひょうちゅうおとし", "2種属性") },
            { 183, new AntenaInfo("かみかぜ", "2種属性") },
            { 184, new AntenaInfo("まばゆいこうせん", "2種属性") },
            { 185, new AntenaInfo("でんきだま", "2種属性") },
            { 186, new AntenaInfo("げきりゅう", "2種属性") },
            { 187, new AntenaInfo("はかいこうせん", "2種属性") },
            { 188, new AntenaInfo("やみのしはいしゃ", "2種属性") },

            { 189, new AntenaInfo("もえさかるこおり", "2種属性") },
            { 190, new AntenaInfo("フレイムアイス", "2種属性") },

            { 191, new AntenaInfo("かさいせんぷう", "2種属性") },
            { 192, new AntenaInfo("ファイアストーム", "2種属性") },

            { 193, new AntenaInfo("だいふんか", "2種属性") },
            { 194, new AntenaInfo("メテオファイア", "2種属性") },

            { 195, new AntenaInfo("らくらいかさい", "2種属性") },
            { 196, new AntenaInfo("ファイアサンダー", "2種属性") },

            { 197, new AntenaInfo("ほのおのうみ", "2種属性") },
            { 198, new AntenaInfo("ハイドロファイア", "2種属性") },

            { 199, new AntenaInfo("こおりあらし", "2種属性") },
            { 200, new AntenaInfo("ジェットブリザード", "2種属性") },

            { 201, new AntenaInfo("こおりいんせき", "2種属性") },
            { 202, new AntenaInfo("アイスメテオ", "2種属性") },

            { 203, new AntenaInfo("かみなりふぶき", "2種属性") },
            { 204, new AntenaInfo("サンダーブリザード", "2種属性") },

            { 205, new AntenaInfo("ひさめ", "2種属性") },
            { 206, new AntenaInfo("アイススコール", "2種属性") },

            { 207, new AntenaInfo("つちけむり", "2種属性") },
            { 208, new AntenaInfo("マッドハリケーン", "2種属性") },

            { 209, new AntenaInfo("でんきあらし", "2種属性") },
            { 210, new AntenaInfo("ハリケーンサンダー", "2種属性") },

            { 211, new AntenaInfo("ぼうふうう", "2種属性") },
            { 212, new AntenaInfo("ハイドロストーム", "2種属性") },

            { 213, new AntenaInfo("かみなりがんせき", "2種属性") },
            { 214, new AntenaInfo("グラウンドサンダー", "2種属性") },

            { 215, new AntenaInfo("どせきりゅう", "2種属性") },
            { 216, new AntenaInfo("マッドウェーブ", "2種属性") },

            { 217, new AntenaInfo("らいう", "2種属性") },
            { 218, new AntenaInfo("サンダースコール", "2種属性") },

            { 219, new AntenaInfo("ならくのたいよう", "2種属性") },
            { 220, new AntenaInfo("ダークシャイニング", "2種属性") },


            // こっからバグアンテナ
            { 1, new AntenaInfo("アンテナの根っこ", "?????") },
            { 224, new AntenaInfo("_はつかっても_", "?????") },
            { 225, new AntenaInfo("AP＿が足りなくて__", "?????") },
            { 226, new AntenaInfo("HP自動回復", "?????") },
            { 227, new AntenaInfo("AP自動回復", "?????") },
            { 228, new AntenaInfo("HP自動回復", "?????") },
            { 229, new AntenaInfo("AP自動回復", "?????") },
            { 230, new AntenaInfo("AP自動回復", "?????") },
            { 231, new AntenaInfo("AP自動回復", "?????") },
            { 232, new AntenaInfo("加速", "?????") },
            { 233, new AntenaInfo("どくこうげき", "?????") },
            { 234, new AntenaInfo("もうどくこうげき", "?????") },
            { 235, new AntenaInfo("まひこうげき", "?????") },
            { 236, new AntenaInfo("ねむりこうげき", "?????") },
        };

        */
        
        public static readonly Dictionary<int, AntenaInfo> antenaDatas = new()
        {

       // アンテナ無し
        { 0, new AntenaInfo(0, "アンテナ無し", "") },


        // 特殊アンテナ
        { 2, new AntenaInfo(1, "お宝2倍", "特殊") },
        { 3, new AntenaInfo(1, "お宝3倍", "特殊") },
        { 4, new AntenaInfo(1, "お宝5倍", "特殊") },

        { 5, new AntenaInfo(2, "レアお宝2倍", "特殊") },
        { 6, new AntenaInfo(2, "レアお宝3倍", "特殊") },
        { 7, new AntenaInfo(2, "レアお宝5倍", "特殊") },

        { 8, new AntenaInfo(3, "ゴールド2倍", "特殊") },
        { 9, new AntenaInfo(3, "ゴールド3倍", "特殊") },
        { 10, new AntenaInfo(3, "ゴールド5倍", "特殊") },

        { 11, new AntenaInfo(4, "ちょっとステルス", "特殊") },
        { 12, new AntenaInfo(4, "そこそこステルス", "特殊") },
        { 13, new AntenaInfo(4, "しばらくステルス", "特殊") },


        // 補助アンテナ
        { 14, new AntenaInfo(5, "ちょっと回復", "補助") },
        { 15, new AntenaInfo(5, "そこそこ回復", "補助") },
        { 16, new AntenaInfo(5, "全部かいふく", "補助") },

        { 17, new AntenaInfo(6, "みんなちょっと回復", "補助") },
        { 18, new AntenaInfo(6, "みんな回復", "補助") },
        { 19, new AntenaInfo(6, "みんな全部回復", "補助") },

        { 20, new AntenaInfo(7, "ちょっと復活", "補助") },
        { 21, new AntenaInfo(7, "復活", "補助") },
        { 22, new AntenaInfo(7, "かんぜん復活", "補助") },

        { 23, new AntenaInfo(8, "みんなちょっと復活", "補助") },
        { 24, new AntenaInfo(8, "みんな復活", "補助") },
        { 25, new AntenaInfo(8, "みんなかんぜん復活", "補助") },

        { 26, new AntenaInfo(9, "げどく", "補助") },
        { 27, new AntenaInfo(9, "みんなげどく", "補助") },

        { 28, new AntenaInfo(10, "やけどなおす", "補助") },
        { 29, new AntenaInfo(10, "みんなやけどなおす", "補助") },

        { 30, new AntenaInfo(11, "かわかす", "補助") },
        { 31, new AntenaInfo(11, "みんなかわかす", "補助") },

        { 32, new AntenaInfo(12, "かぜなおす", "補助") },
        { 33, new AntenaInfo(12, "みんなかぜなおす", "補助") },

        { 34, new AntenaInfo(13, "どろおとす", "補助") },
        { 35, new AntenaInfo(13, "みんなどろおとす", "補助") },

        { 36, new AntenaInfo(14, "おはらい", "補助") },
        { 37, new AntenaInfo(14, "みんなおはらい", "補助") },

        { 38, new AntenaInfo(15, "かんでんなおす", "補助") },
        { 39, new AntenaInfo(15, "みんなかんでんなおす", "補助") },

        { 40, new AntenaInfo(16, "あたためる", "補助") },
        { 41, new AntenaInfo(16, "あたためる", "補助") },

        { 42, new AntenaInfo(17, "しびれとる", "補助") },
        { 43, new AntenaInfo(17, "みんなしびれとる", "補助") },

        { 44, new AntenaInfo(18, "めざめる", "補助") },
        { 45, new AntenaInfo(18, "みんなめざめる", "補助") },

        { 46, new AntenaInfo(19, "めぐすり", "補助") },
        { 47, new AntenaInfo(19, "みんなめぐすり", "補助") },


        // 補強アンテナ
        { 48, new AntenaInfo(20, "むてき", "補強") },
        { 49, new AntenaInfo(20, "みんなむてき", "補強") },

        { 50, new AntenaInfo(21, "みんなこうふん", "補強") },

        { 51, new AntenaInfo(22, "たくわえる", "補強") },
        { 52, new AntenaInfo(22, "みんなたくわえる", "補強") },

        { 53, new AntenaInfo(23, "わざはねかえす", "補強") },
        { 54, new AntenaInfo(23, "みんなわざはねかえす", "補強") },

        { 55, new AntenaInfo(24, "すこしつよくなれ", "補強") },
        { 56, new AntenaInfo(24, "つよくなれ", "補強") },
        { 57, new AntenaInfo(24, "すごくつよくなれ", "補強") },

        { 58, new AntenaInfo(25, "みんなつよめになれ", "補強") },
        { 59, new AntenaInfo(25, "みんなつよくなれ", "補強") },
        { 60, new AntenaInfo(25, "みんなムキムキ", "補強") },

        { 61, new AntenaInfo(26, "すこしかたくなれ", "補強") },
        { 62, new AntenaInfo(26, "かたくなれ", "補強") },
        { 63, new AntenaInfo(26, "すごくかたくなれ", "補強") },

        { 64, new AntenaInfo(27, "みんなかためになれ", "補強") },
        { 65, new AntenaInfo(27, "みんなかたくなれ", "補強") },
        { 66, new AntenaInfo(27, "みんなカチカチ", "補強") },

        { 67, new AntenaInfo(28, "すこしはやくなれ", "補強") },
        { 68, new AntenaInfo(28, "はやくなれ", "補強") },
        { 69, new AntenaInfo(28, "すごくはやくなれ", "補強") },

        { 70, new AntenaInfo(29, "みんなはやめになれ", "補強") },
        { 71, new AntenaInfo(29, "みんなはやくなれ", "補強") },
        { 72, new AntenaInfo(29, "みんなかぜになれ", "補強") },

        { 73, new AntenaInfo(30, "すこしかわしやすい", "補強") },
        { 74, new AntenaInfo(30, "かわしやすい", "補強") },
        { 75, new AntenaInfo(30, "すごくかわしやすい", "補強") },

        { 76, new AntenaInfo(31, "みんなかわしやすい", "補強") },
        { 77, new AntenaInfo(31, "みんなでスルー", "補強") },


        // 状態異常アンテナ
        { 78, new AntenaInfo(32, "どくどくシグナル", "状態異常") },
        { 79, new AntenaInfo(32, "どくどくウェーブ", "状態異常") },

        { 80, new AntenaInfo(33, "もうどく", "状態異常") },
        { 82, new AntenaInfo(33, "みんな猛毒", "状態異常") },

        { 83, new AntenaInfo(34, "ねむらせる", "状態異常") },
        { 84, new AntenaInfo(34, "みんなねむらせる", "状態異常") },

        { 85, new AntenaInfo(35, "しびれさせる", "状態異常") },
        { 86, new AntenaInfo(35, "みんなしびれさせる", "状態異常") },

        { 87, new AntenaInfo(36, "めかくし", "状態異常") },
        { 88, new AntenaInfo(36, "みんなめかくし", "状態異常") },

        { 89, new AntenaInfo(37, "ブレスふうじ", "状態異常") },
        { 90, new AntenaInfo(37, "みんなブレスふうじ", "状態異常") },

        { 91, new AntenaInfo(38, "すこしよわくなれ", "状態異常") },
        { 92, new AntenaInfo(38, "よわくなれ", "状態異常") },
        { 93, new AntenaInfo(38, "すごくよわくなれ", "状態異常") },

        { 94, new AntenaInfo(39, "みんなよわめになれ", "状態異常") },
        { 95, new AntenaInfo(39, "よわめになれ", "状態異常") },
        { 96, new AntenaInfo(39, "げきよわ", "状態異常") },

        { 97, new AntenaInfo(40, "すこしやわくなれ", "状態異常") },
        { 98, new AntenaInfo(40, "やわくなれ", "状態異常") },
        { 99, new AntenaInfo(40, "すごくやわくなれ", "状態異常") },

        { 100, new AntenaInfo(41, "みんなやわらかめ", "状態異常") },
        { 101, new AntenaInfo(41, "やわらかめ", "状態異常") },
        { 102, new AntenaInfo(41, "やわやわ", "状態異常") },

        { 103, new AntenaInfo(42, "すこしおそくなれ", "状態異常") },
        { 104, new AntenaInfo(42, "おそくなれ", "状態異常") },
        { 105, new AntenaInfo(42, "すごくおそくなれ", "状態異常") },

        { 106, new AntenaInfo(43, "みんなおそめになれ", "状態異常") },
        { 107, new AntenaInfo(43, "みんなおそくなれ", "状態異常") },
        { 108, new AntenaInfo(43, "みんなのろのろ", "状態異常") },

        // 技アンテナ
            { 109, new AntenaInfo(44, "ひのたま", "技") },
            { 110, new AntenaInfo(44, "かえんほうしゃ", "技") },
            { 111, new AntenaInfo(44, "もえさかるごうか", "技") },

            { 112, new AntenaInfo(45, "ばくはつ", "技") },
            { 113, new AntenaInfo(45, "だいばくはつ", "技") },
            { 114, new AntenaInfo(45, "ビッグバン", "技") },

            { 115, new AntenaInfo(46, "やまかじ", "技") },
            { 116, new AntenaInfo(46, "煮えたぎるマグマ", "技") },
            { 117, new AntenaInfo(46, "じごくのごうか", "技") },

            { 118, new AntenaInfo(47, "とがったこおり", "技") },
            { 119, new AntenaInfo(47, "こおりのやいば", "技") },
            { 120, new AntenaInfo(47, "つららミサイル", "技") },

            { 121, new AntenaInfo(48, "ロックアイス", "技") },
            { 122, new AntenaInfo(48, "まんねんごおり", "技") },
            { 123, new AntenaInfo(48, "ダイヤモンドダスト", "技") },

            { 124, new AntenaInfo(49, "あられ", "技") },
            { 125, new AntenaInfo(49, "ふぶき", "技") },
            { 126, new AntenaInfo(49, "もうふぶき", "技") },

            { 127, new AntenaInfo(50, "つむじかぜ", "技") },
            { 128, new AntenaInfo(50, "すなあらし", "技") },
            { 129, new AntenaInfo(50, "サイクロン", "技") },

            { 130, new AntenaInfo(51, "ビル風", "技") },
            { 131, new AntenaInfo(51, "ぼうふう", "技") },
            { 132, new AntenaInfo(51, "たつまき", "技") },

            { 133, new AntenaInfo(52, "かまいたち", "技") },
            { 134, new AntenaInfo(52, "ジェット気流", "技") },
            { 135, new AntenaInfo(52, "ダウンバースト", "技") },

            { 136, new AntenaInfo(53, "らくせき", "技") },
            { 137, new AntenaInfo(53, "どしゃくずれ", "技") },
            { 138, new AntenaInfo(53, "いわなだれ", "技") },

            { 139, new AntenaInfo(54, "いしつぶて", "技") },
            { 140, new AntenaInfo(54, "がんせきおとし", "技") },
            { 141, new AntenaInfo(54, "いんせきらっか", "技") },

            { 142, new AntenaInfo(55, "まぐにチューど3", "技") },
            { 143, new AntenaInfo(55, "まぐにチューど6", "技") },
            { 144, new AntenaInfo(55, "まぐにチューど12", "技") },

            { 145, new AntenaInfo(56, "せいでんき", "技") },
            { 146, new AntenaInfo(56, "でんきしょっく", "技") },
            { 147, new AntenaInfo(56, "スパーク", "技") },

            { 148, new AntenaInfo(57, "いなづま", "技") },
            { 149, new AntenaInfo(57, "かみなり", "技") },
            { 150, new AntenaInfo(57, "せいてんのへきれき", "技") },

            { 151, new AntenaInfo(58, "ひゃくボルト", "技") },
            { 152, new AntenaInfo(58, "いちまんボルト", "技") },
            { 153, new AntenaInfo(58, "いちおくボルト", "技") },

            { 154, new AntenaInfo(59, "みずでっぽう", "技") },
            { 155, new AntenaInfo(59, "てっぽうみず", "技") },
            { 156, new AntenaInfo(59, "だくりゅう", "技") },

            { 157, new AntenaInfo(60, "バケツの水", "技") },
            { 158, new AntenaInfo(60, "たきつぼ", "技") },
            { 159, new AntenaInfo(60, "ジェットスプラッシュ", "技") },

            { 160, new AntenaInfo(61, "たかなみ", "技") },
            { 161, new AntenaInfo(61, "つなみ", "技") },
            { 162, new AntenaInfo(61, "おおつなみ", "技") },

            { 163, new AntenaInfo(62, "よわいこうせん", "技") },
            { 164, new AntenaInfo(62, "まばゆいこうせん", "技") },
            { 165, new AntenaInfo(62, "レーザービーム", "技") },

            { 166, new AntenaInfo(63, "スポットライト", "技") },
            { 167, new AntenaInfo(63, "はじけるせんこう", "技") },
            { 168, new AntenaInfo(63, "フレア", "技") },

            { 169, new AntenaInfo(64, "ふゆのひざし", "技") },
            { 170, new AntenaInfo(64, "まなつのたいよう", "技") },
            { 171, new AntenaInfo(64, "せきどうちょっか", "技") },

            { 172, new AntenaInfo(65, "おどかす", "技") },
            { 173, new AntenaInfo(65, "やみうち", "技") },
            { 174, new AntenaInfo(65, "やみのつかい", "技") },

            { 175, new AntenaInfo(66, "ダークホール", "技") },
            { 176, new AntenaInfo(66, "くろいうず", "技") },
            { 177, new AntenaInfo(66, "ブラックホール", "技") },

            { 178, new AntenaInfo(67, "くろいきり", "技") },
            { 179, new AntenaInfo(67, "たちこめるやみ", "技") },
            { 180, new AntenaInfo(67, "ならくのそこ", "技") },

            // こっから2種属性アンテナ,アンテナの内部番号２が分からない・・
            { 181, new AntenaInfo(44,"やきつくすほのお", "第４進化") },
            { 182, new AntenaInfo(47,"ひょうちゅうおとし", "第４進化") },
            { 183, new AntenaInfo(50,"かみかぜ", "第４進化") },
            { 184, new AntenaInfo(62,"まばゆいこうせん", "第４進化") },
            { 185, new AntenaInfo(56,"でんきだま", "第４進化") },
            { 186, new AntenaInfo(59,"げきりゅう", "第４進化") },
            { 187, new AntenaInfo(62,"はかいこうせん", "第４進化") },
            { 188, new AntenaInfo(65,"やみのしはいしゃ", "第４進化") },

            { 189, new AntenaInfo(69, "もえさかるこおり", "2種属性") },
            { 190, new AntenaInfo(69, "フレイムアイス", "2種属性") },

            { 191, new AntenaInfo(70, "かさいせんぷう", "2種属性") },
            { 192, new AntenaInfo(70, "ファイアストーム", "2種属性") },

            { 193, new AntenaInfo(71, "だいふんか", "2種属性") },
            { 194, new AntenaInfo(71, "メテオファイア", "2種属性") },

            { 195, new AntenaInfo(72, "らくらいかさい", "2種属性") },
            { 196, new AntenaInfo(72, "ファイアサンダー", "2種属性") },

            { 197, new AntenaInfo(73, "ほのおのうみ", "2種属性") },
            { 198, new AntenaInfo(73, "ハイドロファイア", "2種属性") },

            { 199, new AntenaInfo(75, "こおりあらし", "2種属性") },
            { 200, new AntenaInfo(75, "ジェットブリザード", "2種属性") },

            { 201, new AntenaInfo(76, "こおりいんせき", "2種属性") },
            { 202, new AntenaInfo(76, "アイスメテオ", "2種属性") },

            { 203, new AntenaInfo(77, "かみなりふぶき", "2種属性") },
            { 204, new AntenaInfo(77, "サンダーブリザード", "2種属性") },

            { 205, new AntenaInfo(78, "ひさめ", "2種属性") },
            { 206, new AntenaInfo(78, "アイススコール", "2種属性") },

            { 207, new AntenaInfo(81, "つちけむり", "2種属性") },
            { 208, new AntenaInfo(81, "マッドハリケーン", "2種属性") },

            { 209, new AntenaInfo(82, "でんきあらし", "2種属性") },
            { 210, new AntenaInfo(82, "ハリケーンサンダー", "2種属性") },

            { 211, new AntenaInfo(83, "ぼうふうう", "2種属性") },
            { 212, new AntenaInfo(83, "ハイドロストーム", "2種属性") },

            { 213, new AntenaInfo(87, "かみなりがんせき", "2種属性") },
            { 214, new AntenaInfo(87, "グラウンドサンダー", "2種属性") },

            { 215, new AntenaInfo(88, "どせきりゅう", "2種属性") },
            { 216, new AntenaInfo(88, "マッドウェーブ", "2種属性") },

            { 217, new AntenaInfo(93, "らいう", "2種属性") },
            { 218, new AntenaInfo(93, "サンダースコール", "2種属性") },

            { 219, new AntenaInfo(99, "ならくのたいよう", "2種属性") },
            { 220, new AntenaInfo(99, "ダークシャイニング", "2種属性") },


        // 特殊
        { 221, new AntenaInfo(68, "つかまえる", "特殊") },
        { 222, new AntenaInfo(68, "つかまえる2ばい", "特殊") },
        { 223, new AntenaInfo(68, "つかまえる3ばい", "特殊") },

            // こっからバグアンテナ
            { 1,   new AntenaInfo(0,"アンテナの根っこ", "?????") },
            { 224, new AntenaInfo(0,"_はつかっても_", "?????") },
            { 225, new AntenaInfo(0,"AP＿が足りなくて__", "?????") },
            { 226, new AntenaInfo(0,"HP自動回復", "?????") },
            { 227, new AntenaInfo(0,"AP自動回復", "?????") },
            { 228, new AntenaInfo(0,"HP自動回復", "?????") },
            { 229, new AntenaInfo(0,"AP自動回復", "?????") },
            { 230, new AntenaInfo(0,"AP自動回復", "?????") },
            { 231, new AntenaInfo(0,"AP自動回復", "?????") },
            { 232, new AntenaInfo(0,"加速", "?????") },
            { 233, new AntenaInfo(0,"どくこうげき", "?????") },
            { 234, new AntenaInfo(0,"もうどくこうげき", "?????") },
            { 235, new AntenaInfo(0,"まひこうげき", "?????") },
            { 236, new AntenaInfo(0,"ねむりこうげき", "?????") },
        };
    }
}
