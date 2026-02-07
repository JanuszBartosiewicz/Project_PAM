using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {

    public static BackgroundScript Instance;    

    [SerializeField] private GameObject BackgroundObject;
    //[SerializeField] private List<GameObject> Objs;

    [Space]
    public float BackgroundSize = 13f;
    private GameObject BackgroundHolder;
    private Transform SnakeTransform;

    public void Awake() {
        Instance = this;
    }

    public void Start () {
        if (BackgroundObject == null) { Debug.LogWarning("BackgroundObject is missing", this); }
        SnakeTransform = Snake.Instance.transform;

        ResetBackground();
    }

    public void ResetBackground() {
        if (BackgroundHolder != null) Destroy(BackgroundHolder);
        BackgroundHolder = new GameObject();
        BackgroundHolder.name = "Background Holder";

        SpawnBackground(new Vector2(0, 0));
        SpawnBackground(new Vector2(1, 0));
        SpawnBackground(new Vector2(-1, 0));
        SpawnBackground(new Vector2(0, 1));
        SpawnBackground(new Vector2(0, -1));
        SpawnBackground(new Vector2(1, 1));
        SpawnBackground(new Vector2(-1, -1));
        SpawnBackground(new Vector2(-1, 1));
        SpawnBackground(new Vector2(1, -1));
    }

    public void StopBackground() {
        Destroy(BackgroundHolder);
    }

    public void SpawnBackground(Vector2 pos) {
        GameObject obj = Instantiate(BackgroundObject);
        obj.transform.position = new Vector3(pos.x * BackgroundSize, pos.y * BackgroundSize, 0);
        obj.transform.SetParent(BackgroundHolder.transform);
    }

    void Update() { 
        if (BackgroundHolder != null) {
            Vector3 pos = new Vector3(Mathf.RoundToInt(SnakeTransform.position.x / BackgroundSize), Mathf.RoundToInt(SnakeTransform.position.y / BackgroundSize), 0);
            BackgroundHolder.transform.position = pos * BackgroundSize;
        }
    }
}
