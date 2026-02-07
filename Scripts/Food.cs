using UnityEngine;

public class Food : MonoBehaviour {

    private static Transform SnakePos;
    private static FoodSpawner FoodSpawner;

    private float Distance = 0;

    void Start() {
        SnakePos = Snake.Instance.transform;
        FoodSpawner = FoodSpawner.Instance;
    }

    void Update() {
        Distance = Vector3.Distance(transform.position, SnakePos.position);
        if (Distance > 15f) {
            FoodSpawner.DestroyFood(this.gameObject);
        }
    }
}
