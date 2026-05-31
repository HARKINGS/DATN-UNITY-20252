using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject waitingScenePanel;
    [SerializeField] private TMP_Text countdown;

    private void Start()
    {
        Time.timeScale = 0f;
        // Khởi động chuỗi đếm ngược ngay khi màn chơi bắt đầu
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        // 1. Đảm bảo thời gian chạy bình thường để đếm ngược hoạt động
        waitingScenePanel.SetActive(true);

        // 2. Vòng lặp đếm ngược từ 3 về 1
        int count = 3; // Số giây đếm ngược
        while (count > 0)
        {
            if (countdown != null)
                countdown.text = count.ToString(); // Cập nhật UI đếm ngược
            yield return new WaitForSecondsRealtime(1f); // Đợi 1 giây
            count--;
        }

        // 3. Hiện chữ thông báo bắt đầu
        if (countdown != null)
            countdown.text = "Start!";
        yield return new WaitForSecondsRealtime(0.5f);

        // 4. Dọn dẹp: Tắt toàn bộ bảng chuẩn bị để người chơi bắt đầu combat
        waitingScenePanel.SetActive(false);

        // Đảm bảo game chạy bình thường, KHÔNG để timeScale = 0 ở đây bạn nhé!
        Time.timeScale = 1f;
    }
}
