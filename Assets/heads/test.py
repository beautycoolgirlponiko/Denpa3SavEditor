from pathlib import Path
from PIL import Image

# この.pyがあるフォルダ
INPUT_DIR = Path(__file__).parent

# 出力
OUTPUT_FILE = INPUT_DIR / "combined.png"

# 1行に並べる枚数
COLUMNS = 16

# 連番画像を取得
files = sorted(
    INPUT_DIR.glob("*.png"),
    key=lambda p: int(p.stem) if p.stem.isdigit() else 999999
)

# combined.png 自身を除外
files = [f for f in files if f.name != OUTPUT_FILE.name]

if not files:
    print("画像がありません")
    exit()

# 画像サイズを取得
with Image.open(files[0]) as img:
    width, height = img.size

# 行数
rows = (len(files) + COLUMNS - 1) // COLUMNS

# 透明背景で作成
output = Image.new(
    "RGBA",
    (width * COLUMNS, height * rows),
    (0, 0, 0, 0)
)

# 貼り付け
for index, file in enumerate(files):
    with Image.open(file) as img:
        img = img.convert("RGBA")

        x = (index % COLUMNS) * width
        y = (index // COLUMNS) * height

        output.paste(img, (x, y))

# 保存
output.save(OUTPUT_FILE)

print(f"完了: {OUTPUT_FILE}")
print(f"{len(files)}枚 → {COLUMNS}列 × {rows}行")