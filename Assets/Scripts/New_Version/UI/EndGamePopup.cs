using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject endGamePopup;
    [SerializeField] private TMP_Text endGameTitle;

    [Header("Buttons")]
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        if (endGamePopup != null)
            endGamePopup.SetActive(false);
    }

    private void OnEnable()
    {
        // ĐĂNG KÝ LẮNG NGHE SỰ KIỆN TỪ VŨ TRỤ EVENT
        CombatEvents.OnGameEnded += HandleGameEnd;

        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(BackToMainMenu);
        if (quitGameButton != null) mainMenuButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        // HỦY ĐĂNG KÝ
        CombatEvents.OnGameEnded -= HandleGameEnd;

        if (restartButton != null) restartButton.onClick.RemoveListener(RestartGame);
        if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(BackToMainMenu);
        if (quitGameButton != null) mainMenuButton.onClick.RemoveListener(QuitGame);
    }

    // Hàm nhận dữ liệu trực tiếp từ Sự kiện truyền về
    private void HandleGameEnd(bool isPlayerWin)
    {
        if (endGamePopup == null) return;
        endGamePopup.SetActive(true);

        // Đọc thẳng kết quả từ biến truyền vào, không cần so sánh tag hay check máu
        endGameTitle.text = isPlayerWin ? "You Win!" : "You Died!";
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
}