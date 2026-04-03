using UnityEngine;
using UnityEngine.UI;

public class CombineToSingleImage : MonoBehaviour
{
    public Sprite[] slices; // Kéo 9 sprite đã cắt vào đây (theo thứ tự từ trái sang phải, trên xuống dưới)
    public int cols = 3;
    public int rows = 3;

    void Start()
    {
        CombineAndAssign();
    }

    void CombineAndAssign()
    {
        if (slices.Length != cols * rows)
            return;

        // Lấy kích thước thực của từng slice (sau Trim)
        int sliceWidth = (int)slices[0].rect.width;
        int sliceHeight = (int)slices[0].rect.height;

        int fullWidth = sliceWidth * cols;
        int fullHeight = sliceHeight * rows;

        Texture2D combined = new Texture2D(fullWidth, fullHeight, TextureFormat.RGBA32, false);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int index = row * cols + col;
                Sprite s = slices[index];
                Texture2D tex = s.texture;
                Rect r = s.rect;
                Color[] pixels = tex.GetPixels((int)r.x, (int)r.y, sliceWidth, sliceHeight);
                combined.SetPixels(
                    col * sliceWidth,
                    (rows - 1 - row) * sliceHeight,
                    sliceWidth,
                    sliceHeight,
                    pixels
                );
            }
        }
        combined.Apply();

        // Tạo sprite mới từ texture ghép
        Sprite finalSprite = Sprite.Create(
            combined,
            new Rect(0, 0, fullWidth, fullHeight),
            new Vector2(0.5f, 0.5f)
        );
        GetComponent<Image>().sprite = finalSprite;
        // Đảm bảo Image có kích thước phù hợp
        GetComponent<Image>().SetNativeSize();
    }
}
