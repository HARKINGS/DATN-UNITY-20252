using UnityEngine;

public class ImageMerger : MonoBehaviour
{
    public Texture2D[] parts; // Assign 9 textures/sprites ở Inspector

    public Texture2D Merge9Parts()
    {
        if (parts.Length != 9 || parts[0] == null) return null;

        int size = parts[0].width; // Giả sử vuông và kích thước bằng nhau
        Texture2D result = new Texture2D(3 * size, 3 * size);

        // Ghép theo lưới 3x3
        for (int i = 0; i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            int x = col * size;
            int y = (2 - row) * size; // Đảo y vì Unity origin ở dưới trái

            Color[] pixels = parts[i].GetPixels();
            result.SetPixels(x, y, size, size, pixels);
        }
        result.Apply();
        return result;
    }
}