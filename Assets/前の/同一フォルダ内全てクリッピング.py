from pathlib import Path
from PIL import Image

# test.py と同じフォルダ
INPUT_DIR = Path(__file__).parent
OUTPUT_DIR = INPUT_DIR / "cropped"

CROP_WIDTH = 64
CROP_HEIGHT = 48

OUTPUT_DIR.mkdir(exist_ok=True)

extensions = {".png", ".jpg", ".jpeg", ".bmp", ".webp", ".tga"}

for path in INPUT_DIR.iterdir():

    if path.suffix.lower() not in extensions:
        continue

    try:
        with Image.open(path) as img:

            width, height = img.size

            center_x = width // 2
            center_y = height // 2

            left = center_x - CROP_WIDTH // 2
            top = center_y - CROP_HEIGHT // 2
            right = left + CROP_WIDTH
            bottom = top + CROP_HEIGHT

            cropped = img.crop((left, top, right, bottom))

            output_path = OUTPUT_DIR / path.name
            cropped.save(output_path)

            print(f"完了: {path.name}")

    except Exception as e:
        print(f"エラー: {path.name} -> {e}")

print("全部完了！")