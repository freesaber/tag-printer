
from PIL import Image

# 加载图片
input_image_path = "./bird.png"  # 替换为你的实际文件路径
output_ico_path_transparent = "./Bird_Logo.ico"  # 替换为你的保存路径

# 打开图片
image = Image.open(input_image_path).convert("RGBA")

# 保存为 ICO 文件
image.save(output_ico_path_transparent, format="ICO")