using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGamePopup : MonoBehaviour
{
    [Header("UI References")]
    // Tên biến nên đổi thành pauseGamePopup để tránh nhầm với EndGame
    [SerializeField] private GameObject pauseGamePopup;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;     // Nút Tiếp tục (Chức năng 1)
    [SerializeField] private Button mainMenuButton;   // Nút Quay lại Menu (Chức năng 2)
    [SerializeField] private Button quitGameButton;     // Nút Thoát game (Chức năng 3)

    [SerializeField] private InputAction PauseGameAction;

    private bool isPaused = false;

    private void Start()
    {
        // Ban đầu vào trận đấu thì ẩn bảng Pause đi
        if (pauseGamePopup != null)
            pauseGamePopup.SetActive(false);
        PauseGameAction.Enable();
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện Click cho các nút bấm bằng code
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(BackToMainMenu);
        if (quitGameButton != null) quitGameButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        // Hủy đăng ký để tránh rò rỉ bộ nhớ (Memory Leak)
        if (resumeButton != null) resumeButton.onClick.RemoveListener(ResumeGame);
        if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(BackToMainMenu);
        if (quitGameButton != null) quitGameButton.onClick.RemoveListener(QuitGame);
    }

    private void Update()
    {
        // Lắng nghe nút ESC từ bàn phím (Hoạt động cho cả Input System cũ và mới)
        if (PauseGameAction.triggered)
        {
            // Nếu game đã kết thúc (Time.timeScale == 0 do EndGamePopup gọi), ta không cho Pause nữa
            if (!WaitingSceneUI.IsBattleStarted || (Time.timeScale == 0 && !isPaused))
                return;

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pauseGamePopup != null)
            pauseGamePopup.SetActive(true);

        // Đóng băng toàn bộ thời gian vật lý, Coroutine, Update và Animator trong game
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseGamePopup != null)
            pauseGamePopup.SetActive(false);

        // Khôi phục lại thời gian chạy bình thường của game
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Phải reset lại thời gian trước khi chuyển Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Phải reset lại thời gian trước khi chuyển Scene
        SceneManager.LoadScene(0); // Về màn hình Menu chính (Build Index = 0)
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Lệnh thoát hoàn toàn ứng dụng (Chỉ hoạt động khi Build ra file .exe/.apk)
    }
}