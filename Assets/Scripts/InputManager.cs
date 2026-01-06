using UnityEngine;

// Basit input: mouse veya touch, en yakın spawnPoint.x'e göre sütunu bulur ve GameManager.TryHitColumn çağırır.
// Öneri: spawnPoints referansını TileSpawner'dan alın.
public class InputManager : MonoBehaviour
{
    public TileSpawner spawner; // bağla inspector'dan

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (spawner == null) Debug.LogWarning("InputManager: spawner referansı yok.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = mainCam.ScreenToWorldPoint(Input.mousePosition);
            HandleTap(wp);
        }

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                Vector3 wp = mainCam.ScreenToWorldPoint(t.position);
                HandleTap(wp);
            }
        }
    }

    void HandleTap(Vector3 worldPos)
    {
        if (spawner == null || spawner.spawnPoints == null || spawner.spawnPoints.Length == 0) return;

        // spawnPoints'in x'leri arasından en yakın column'u seç
        int bestIndex = 0;
        float bestDist = Mathf.Abs(worldPos.x - spawner.spawnPoints[0].position.x);

        for (int i = 1; i < spawner.spawnPoints.Length; i++)
        {
            float d = Mathf.Abs(worldPos.x - spawner.spawnPoints[i].position.x);
            if (d < bestDist)
            {
                bestDist = d;
                bestIndex = i;
            }
        }

        GameManager.Instance.TryHitColumn(bestIndex);
    }
}
