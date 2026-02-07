using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {
    
    public static FoodSpawner Instance;

    [Header("Settings")]
    public int FoodNumber = 6;
    public float FoodSpawnRange = 30f;

    [Header("Prefab")]
    [SerializeField]private GameObject foodPrefab;

    [Header("Gameplay Values")]
    [SerializeField]private List<GameObject> FoodObjects = new List<GameObject> ();
    [SerializeField]private Sprite[] FoodSprites;
    [SerializeField]private GameObject FoodHolder;
    
    public void Awake() {
        Instance = this;

        FoodSprites = Resources.LoadAll<Sprite>("Food");
    }

    public void StartFoodSpawner() {
        if (FoodHolder != null) Destroy(FoodHolder);
        
        FoodHolder = new GameObject();
        FoodHolder.name = "Food Holder";

        FoodObjects.Clear();

        CheckFood();
    }

    public void StopFoodSpawner() {
        if (FoodHolder != null) Destroy(FoodHolder);
        foreach (GameObject obj in FoodObjects) { Destroy(obj); }
        FoodObjects = new List<GameObject>();
    }

    private void CheckFood() {
        while (FoodObjects.Count < FoodNumber) {
            SpawnFood();
        }
    }

    public void SpawnFood() {
        Vector2 pos = Random.insideUnitCircle * FoodSpawnRange;

        GameObject newFood = Instantiate(foodPrefab, Snake.Instance.transform.position + (Vector3)pos, Quaternion.identity);
        FoodObjects.Add(newFood);
        newFood.transform.SetParent(FoodHolder.transform);
        newFood.GetComponent<SpriteRenderer>().sprite = FoodSprites[Random.Range(0, FoodSprites.Length - 1)];
    }

    public void DestroyFood(GameObject Food) {
        FoodObjects.Remove(Food);
        Destroy(Food);
        CheckFood();
    }
}
