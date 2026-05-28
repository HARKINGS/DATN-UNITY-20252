using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject TutorialPanelPrefab;

    private void Awake()
    {
        //TutorialPanelPrefab = Resources.Load<GameObject>("TutorialPanel");
        TutorialPanelPrefab.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }   
    
    public void ExitGame()
    {
        Application.Quit();
    } 
    
    public void OptionsOn()
    {
        TutorialPanelPrefab.SetActive(true);
    }    

    public void OptionsOff()
    {
        TutorialPanelPrefab.SetActive(false);
    }

    public void TutorialGame()
    {

    }    
}
