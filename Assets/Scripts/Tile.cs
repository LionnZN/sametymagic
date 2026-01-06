using UnityEngine;

// Basit tile - spawn noktasından hit-zone'a travelTime içinde LERP ile hareket eder.
// Hit veya sona ulaşma durumunu GameManager'a bildirir.
public class Tile : MonoBehaviour
{
    [HideInInspector] public int column;
    [HideInInspector] public float travelTime;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public GameManager gm;

    float elapsed = 0f;
    bool wasHit = false;

    void Start()
    {
        transform.position = startPos;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / travelTime);
        transform.position = Vector3.Lerp(startPos, targetPos, t);

        if (elapsed >= travelTime && !wasHit)
        {
            // Tile hit-zone'u geçti -> miss
            gm.TileMissed(this);
            Destroy(gameObject);
        }
    }

    // GameManager tarafından gerçek bir "hit" tespit edildiğinde çağrılır
    public void OnHit()
    {
        if (wasHit) return;
        wasHit = true;
        gm.TileHit(this);
        // burada vurulma efekti/ses tetiklenebilir
        Destroy(gameObject);
    }

    public float RemainingTime => Mathf.Max(0f, travelTime - elapsed);
}
