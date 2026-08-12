import os

# この.pyと同じフォルダ
folder = os.path.dirname(os.path.abspath(__file__))

for filename in os.listdir(folder):
    if not filename.lower().endswith(".png"):
        continue

    if "_nobg" not in filename:
        continue

    old_path = os.path.join(folder, filename)
    new_filename = filename.replace("_nobg", "")
    new_path = os.path.join(folder, new_filename)

    # 同名ファイルが既にある場合はスキップ
    if os.path.exists(new_path):
        print(f"スキップ: {filename} → {new_filename}（既に存在）")
        continue

    os.rename(old_path, new_path)
    print(f"変更: {filename} → {new_filename}")

print("完了")