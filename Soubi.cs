using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;

namespace Denpa3SavEditor
{

    enum SoubiType
    {
        Neck,
        Arm,
        Foot,
        Back,
        Clothes
    }

    internal class Soubi
    {

        // ハートのチョーカーからの数
        int kStartSoubiIndex = 801;

        public struct SoubiInfo
        {
            public string Type { get; }
            public string Name { get; }

            public SoubiInfo(string type, string name)
            {
                this.Type = type;
                this.Name = name;
            }
        }

        // 番号順に並んでる用
        public List<SoubiInfo> soubiList = new();

        // 各ジャンルの０～からの位置(それぞれ決まっている)
        public List<int> soubiStartIndexList = new()
        {
            0,
            170,
            230,
            90,
            280
        };

        // combo用
        public Dictionary<SoubiType, List<SoubiInfo>> soubiGenreList = new();

        public List<SoubiInfo> getSoubiList(SoubiType type) { return soubiGenreList[type]; }

        // 装備タイプ
        string[] types =
            {
                "Neck",
                "Arm",
                "Foot",
                "Back",
                "Clothes"
            };

        public void Init()
        {
            string jsonText = File.ReadAllText("Assets/soubiData.json");

            JsonDocument json = JsonDocument.Parse(jsonText);

            // タイプ分
            foreach (string type in types)
            {
                JsonElement array = json.RootElement.GetProperty(type);

                SoubiType soubiType = Enum.Parse<SoubiType>(type);

                // その種類のListを作る
                soubiGenreList[soubiType] = new List<SoubiInfo>();

                // 装備無
                soubiGenreList[soubiType].Add(
                    new SoubiInfo(type, "---")
                );

                foreach (JsonElement item in array.EnumerateArray())
                {
                    string name = item.GetString();


                    soubiGenreList[soubiType].Add(
                        new SoubiInfo(type, name)
                    );

                    soubiList.Add(new SoubiInfo(type, name));
                }

                
            }
        }

        // ８０１～からを　０～で返す
        public int GetSoubiGenreIndexFromId(SoubiType type, int id) {

            if (id >= kStartSoubiIndex)
            {
                // 801～を　0～に
                int index = id - kStartSoubiIndex;

                // 各要素の開始地点から引く  未選択の乱のぶん＋１
                return index - soubiStartIndexList[(int)type] + 1;

            }
            else
            {
                return 0;
            }
        }

        // の逆
        public int GetSoubiIdFromGenreIndex(SoubiType type, int index)
        {

            // 未選択以上なら
            if (index > 0)
            {
                return kStartSoubiIndex
                     + soubiStartIndexList[(int)type]
                     + index
                     - 1;
            }

            return 0;
        }
    }
}
