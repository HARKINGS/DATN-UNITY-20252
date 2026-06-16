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
    [SerializeField] private Button restartButton;    // Kéo thả component Button vào đây
    [SerializeField] private Button mainMenuButton;   // Kéo thả component Button vào đây

    [Header("Component")]
    [SerializeField] private CharacterHealth health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        health = GetComponent<CharacterHealth>();
    }

    private void Start()
    {
        // Giữ lại logic ẩn popup khi bắt đầu game
        if (endGamePopup != null)
            endGamePopup.SetActive(false);
    }

    private void OnEnable()
    {
        if (health != null) health.OnDeath += ShowEndGamePopup;

        // Đăng ký sự kiện Click chuột bằng code khi popup active
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable()
    {
        if (health != null) health.OnDeath -= ShowEndGamePopup;

        // Hủy đăng ký để tránh rò rỉ bộ nhớ (Memory Leak)
        if (restartButton != null) restartButton.onClick.RemoveListener(RestartGame);
        if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void ShowEndGamePopup()
    {
        if (endGamePopup == null) return;
        endGamePopup.SetActive(true);
        if(gameObject.CompareTag("Player"))
            endGameTitle.text = "You Died!";
        else endGameTitle.text = "You Win!";
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
}
