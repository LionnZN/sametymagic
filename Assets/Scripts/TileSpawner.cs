using System.Collections.Generic;
using UnityEngine;

// Beatmap JSON wrapper + spawn logic.
// Beatmap JSON format (Assets/beatmap.json):
// { "beats": [ {"time":0.5, "column":0}, {"time":1.0, "column":2}, ... ] }
public class TileSpawner : MonoBehaviour
{
    [Header("References")]
    public TextAsset beatmapJson;        // beatmap TextAsset (see example)
    public GameObject tilePrefab;        // prefab with Tile.cs
    public Transform[] spawnPoints;      // top spawn transforms (one per column)
    public Transform hitZone;            // hit zone transform (target position)
    public AudioSource music;            // müzik oynatıcı

    [Header("Spawn settings")]
    public float travelTime = 2.0f;      // spawn -> hit zone süresi (saniye)

    List<Beat> beats = new List<Beat>();
    int nextBeatIndex = 0;

    void Start()
    {
        if (beatmapJson != null)
        {
            var wrapper = JsonUtility.FromJson<BeatmapWrapper>(beatmapJson.text);
            if (wrapper != null && wrapper.beats != null)
                beats = new List<Beat>(wrapper.beats);
        }
        // optional: sort beats by time to be safe
        beats.Sort((a, b) => a.time.CompareTo(b.time));
    }

    void Update()
    {
        if (music == null || beats == null || nextBeatIndex >= beats.Count) return;

        float audioTime = music.time;

        // spawn when audioTime >= beat.time - travelTime
        while (nextBeatIndex < beats.Count && audioTime >= beats[nextBeatIndex].time - travelTime)
        {
            Spawn(beats[nextBeatIndex]);
            nextBeatIndex++;
        }
    }

    void Spawn(Beat b)
    {
        if (b.column < 0 || b.column >= spawnPoints.Length)
        {
            Debug.LogWarning("Beat column out of range: " + b.column);
            return;
        }

        Vector3 spawnPos = spawnPoints[b.column].position;
        GameObject go = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
        Tile tile = go.GetComponent<Tile>();
        tile.column = b.column;
        tile.startPos = spawnPos;
        tile.targetPos = hitZone.position;
        tile.travelTime = travelTime;
        tile.gm = GameManager.Instance;

        GameManager.Instance.RegisterTile(tile);
    }

    [System.Serializable]
    public class Beat
    {
        public float time;
        public int column;
    }

    [System.Serializable]
    class BeatmapWrapper
    {
        public Beat[] beats;
    }
}
