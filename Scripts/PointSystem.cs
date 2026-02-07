using TMPro;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PointSystem : MonoBehaviour {
    public static PointSystem Instance;

    [SerializeField] private TMP_Text PointText;

    private int Points = 0;

    public List<PointRecord> PlayerPointRecords = new List<PointRecord> ();

    public GameObject ListHolder;
    public GameObject ListObjectPrefab;
    public List<GameObject> ListObjects;
    
    public void AddPoints(int points) {
        Points += points;
        if (PointText != null)
            PointText.text = Points.ToString();
    }

    public int GetPoints() {
        return Points;
    }

    public void Awake() {
        Instance = this;
    }

    public void Start() {
        PlayerPointRecords = PointRecordSaveSystem.Load();
    }

    public void StartPointSystem() {
        Points = 0;
    }

    public void SpawnList() {
        ListObjects = new List<GameObject>();
        int i = 1;
        foreach (PointRecord p in GetRecordList()) {
            GameObject obj = Instantiate(ListObjectPrefab);
            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = i + ". " + p.Name + "   " + p.Value;
            obj.transform.SetParent(ListHolder.transform);
            ListObjects.Add(obj);
            i++;    
        }
    }

    public void DestroyList() {
        foreach (GameObject obj in ListObjects) {
            Destroy(obj);
        }
    }

    public List<PointRecord> GetRecordList() {
        List<PointRecord> List = new List<PointRecord> {
            new PointRecord("Mistrz", 250),
            new PointRecord("W¹¿", 150),
            new PointRecord("Mistrz", 75),
            new PointRecord("Nowicjusz", 25),
            new PointRecord("Ktoœ", 10)
        };

        if (PlayerPointRecords != null) 
            List.AddRange(PlayerPointRecords);
        List.Sort((a, b) => b.Value.CompareTo(a.Value));

        return List;
    }

    public void AddPlayerRecord() {
        PlayerPointRecords.Add(new PointRecord("Gracz", Points));
        PointRecordSaveSystem.Save(PlayerPointRecords);
    }

}

[Serializable]
public class PointRecord {

    public string Name;
    public int Value;

    public PointRecord(string name, int value) {
        Name = name;
        Value = value;
    }

}

[Serializable]
public class PointRecordList {
    public List<PointRecord> records = new List<PointRecord>();
}

public static class PointRecordSaveSystem {
    private static string fileName = "points.json";

    private static string FullPath => Path.Combine(Application.persistentDataPath, fileName);

    public static void Save(List<PointRecord> records) {
        PointRecordList wrapper = new PointRecordList();
        wrapper.records = records;

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(FullPath, json);

        Debug.Log("Zapisano wyniki do: " + FullPath);
    }

    public static List<PointRecord> Load() {
        if (!File.Exists(FullPath)) {
            Debug.Log("Brak pliku zapisu, zwracam pust¹ listê.");
            return new List<PointRecord>();
        }

        string json = File.ReadAllText(FullPath);
        PointRecordList wrapper = JsonUtility.FromJson<PointRecordList>(json);

        return wrapper.records;
    }
}