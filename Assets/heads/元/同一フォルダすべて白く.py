from pathlib import Path
from PIL import Image

# test.py と同じフォルダ
INPUT_DIR = Path(__file__).parent
OUTPUT_DIR = INPUT_DIR / "white"

OUTPUT_DIR.mkdir(exist_ok=True)

extensions = {".png", ".jpg", ".jpeg", ".bmp", ".webp", ".tga"}

for path in INPUT_DIR.iterdir():

    if path.suffix.lower() not in extensions:
        continue

    try:
        img = Image.open(path).convert("RGBA")

        # 白い画像を作る
        white = Image.new("RGBA", img.size, (255, 255, 255, 0))

        # 元画像のアルファ値だけ取得
        alpha = img.getchannel("A")

        # RGBを白、アルファは元画像のまま
        white.putalpha(alpha)

        output_path = OUTPUT_DIR / path.name
        white.save(output_path)

        print(f"完了: {path.name}")

    except Exception as e:
        print(f"エラー: {path.name} -> {e}")

print("全部完了！")