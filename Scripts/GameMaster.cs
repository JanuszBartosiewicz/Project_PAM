using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance;

    public bool IsPaused = false;

    [Header("UI References")]
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject StartScreen;
    [SerializeField] private GameObject GameoverScreen;
    [SerializeField] private TMP_Text GameOverPoints;

    [SerializeField] private ParticleSystem MenuParticle;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        InputSystem.actions.FindAction("Pause").performed += _ => SwitchPause();

        GameScreen.SetActive(false);
        PauseScreen.SetActive(false);
        StartScreen.SetActive(true);
        GameoverScreen.SetActive(false);

        IsPaused = true;
    }

    public void StartGame() {
        //Debug.Log("Gra Rozpoczêta");

        Snake.Instance.ResetSnake();
        FoodSpawner.Instance.StartFoodSpawner();
        BackgroundScript.Instance.ResetBackground();
        PointSystem.Instance.StartPointSystem();

        GameScreen.SetActive(true);
        PauseScreen.SetActive(false);
        StartScreen.SetActive(false);

        MenuParticle.gameObject.SetActive(false);

        UnPauseGame();
    }

    public void ExitGame() {
        //Debug.Log("Gra Zakoñczona");

        FoodSpawner.Instance.StopFoodSpawner();

        GameScreen.SetActive(false);
        PauseScreen.SetActive(false);
        StartScreen.SetActive(true);
        GameoverScreen.SetActive(false);

        MenuParticle.gameObject.SetActive(true);
    }

    public void Gameover(){
        IsPaused = true;

        GameOverPoints.text = "Punkty " + PointSystem.Instance.GetPoints();

        CameraScript.Instance.GameOverSound();
        PointSystem.Instance.AddPlayerRecord();

        GameScreen.SetActive(false);
        PauseScreen.SetActive(false);
        GameoverScreen.SetActive(true);
    }


    //Game Pause
    public void SwitchPause() {
        if (IsPaused) {
            UnPauseGame();
        } else {
            PauseGame();
        }
    }
    public void PauseGame() {
        //Debug.Log("Paused");
        IsPaused = true;

        PauseScreen.SetActive(true);
        GameScreen.SetActive(false);
    }
    public void UnPauseGame() {
        //Debug.Log("UnPaused");
        IsPaused = false;

        PauseScreen.SetActive(false);
        GameScreen.SetActive(true);
    }
}
