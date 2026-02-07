using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Snake : MonoBehaviour {

    public static Snake Instance;

    private Vector2 direction = Vector2.up;
    private InputAction movment;
    private Vector3 previousPosition;

    private List<Transform> bodyParts = new List<Transform>();

    [Header("Snake Settings")]
    [SerializeField] private float SnakeSpeed = 0.5f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private bool ShowMovementArrow = true;

    [Header("Prefabs")]
    [SerializeField] private Transform bodyPrefab;
    [SerializeField] private GameObject ArrowPrefab;
    
    //Helping Objects
    private GameObject MovmentArrow;
    private GameObject SnakeBodyHolder;
    private SpriteRenderer SnakeSprite;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        transform.position = Vector3.zero;
        movment = InputSystem.actions.FindAction("Move");

        SnakeSprite = GetComponent<SpriteRenderer>();
        SnakeSprite.enabled = false;

        if (ArrowPrefab != null && ShowMovementArrow)
            MovmentArrow = Instantiate(ArrowPrefab);
    }

    public void ResetSnake() {
        if (SnakeBodyHolder != null) Destroy(SnakeBodyHolder);
        SnakeBodyHolder = new GameObject();
        SnakeBodyHolder.name = "Snake Body Holder";

        SnakeSprite.enabled = true;

        bodyParts = new List<Transform>();
        Grow(3);
    }

    void Update() {
        Vector2 moveValue = movment.ReadValue<Vector2>();

        if (moveValue != Vector2.zero) {
            direction = Vector2.Lerp(direction, moveValue, Time.deltaTime * turnSpeed);
            direction.Normalize();
        }
    }

    void FixedUpdate() {
        if (!GameMaster.Instance.IsPaused)
            Move();
    }

    void Move() {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        previousPosition = transform.position;
        transform.position = Vector3.Lerp(
            transform.position,
            transform.position += (Vector3)direction * SnakeSpeed,
            Time.fixedDeltaTime
         );
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        if (MovmentArrow != null && ShowMovementArrow) {
            MovmentArrow.transform.position = transform.position + (Vector3)direction * 1.1f;
            MovmentArrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        foreach (Transform part in bodyParts) {
            Vector3 temp = part.position;
            part.position = Vector3.Lerp(
                part.transform.position,
                previousPosition + ((Vector3)direction * 0.35f),
                Time.fixedDeltaTime * SnakeSpeed
                );
            //part.position = previousPosition - ((Vector3)direction * 0.5f);
            previousPosition = temp;
        }
    }

    public void Grow() {
        Transform newPart = Instantiate(bodyPrefab);
        if (bodyParts.Count > 0) { 
            newPart.position = bodyParts[bodyParts.Count - 1].position;
        } else { 
            newPart.position = transform.position - (Vector3)direction;
        }
        newPart.SetParent(SnakeBodyHolder.transform);
        bodyParts.Add(newPart);
        newPart.name = "Snake Body:" + bodyParts.Count;
        if (bodyParts.Count <= 3) {
            Destroy(newPart.GetComponent<CircleCollider2D>());
        }
    }

    public void Grow(int size) {
        for (int i = 0; i < size; i++) { 
            Grow();
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Food")) {
            Grow();
            PointSystem.Instance.AddPoints(1);
            CameraScript.Instance.FoodSound();
            FoodSpawner.Instance.DestroyFood(other.gameObject);
        } else if (other.CompareTag("Wall") || other.CompareTag("Body")) {
            GAMEOVER();
        } 
    }

    public void GAMEOVER () {
        GameMaster.Instance.Gameover();
    }

}
