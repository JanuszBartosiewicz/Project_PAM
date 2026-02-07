using UnityEngine;

public class CameraScript : MonoBehaviour {

    public static CameraScript Instance;

    private AudioSource m_AudioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip Gameover;
    [SerializeField] private AudioClip[] Food;

    public void Awake() {
        if (Instance == null) { Instance = this; }
    }

    public void Start() {
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update() {
        transform.position = Snake.Instance.transform.position + new Vector3(0, -4, -1);
    }

    public void FoodSound() {
        m_AudioSource?.PlayOneShot(Food[Random.Range(0, Food.Length - 1)]);
    }

    public void GameOverSound() {
        m_AudioSource.PlayOneShot(Gameover);
    }

}
