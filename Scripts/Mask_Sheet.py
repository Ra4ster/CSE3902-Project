import argparse
from pathlib import Path

import numpy as np
import skimage.io

BG_COLOR = np.array([0, 128, 0], dtype=np.uint8)
TRANSPARENT_COLOR = np.array([116, 116, 116], dtype=np.uint8)


def main() -> None:
    parser = argparse.ArgumentParser(description="Create a white-on-black bitmask from a PNG spritesheet.")
    parser.add_argument("-p", "--path", required=True, help="Path of spritesheet to bitmask")
    args = parser.parse_args()

    path = Path(args.path)
    if path.suffix.lower() != ".png":
        print("[Error] Not a valid image file (requires png).")
        return

    img: np.ndarray = skimage.io.imread(path)
    rgb = img[:, :, :3] if img.ndim == 3 and img.shape[2] >= 3 else img

    mask = np.all(rgb == BG_COLOR, axis=-1) | np.all(rgb == TRANSPARENT_COLOR, axis=-1)
    output = np.full(rgb.shape, 255, dtype=np.uint8)
    output[mask] = 0

    skimage.io.imsave(path.suffix.lower() + "_masked" + path.suffix.upper(), output)


if __name__ == "__main__":
    main()