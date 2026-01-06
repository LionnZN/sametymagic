using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Skor, combo, tile list yönetimi.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text scoreText;
    public Text comboText;
    public Text missText;

    [Header("Gameplay")]
    public float hitWindowSeconds = 0.25f; // hangi zaman aralığında "hit" kabul edilsin (saniye)

    int score = 0;
    int combo = 0;
    int misses = 0;

    List<Tile> activeTiles = new List<Tile>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterTile(Tile t)
    {
        activeTiles.Add(t);
    }

    public void UnregisterTile(Tile t)
    {
        activeTiles.Remove(t);
    }

    public void TileMissed(Tile t)
    {
        UnregisterTile(t);
        misses++;
        combo = 0;
        UpdateUI();
        // efektler/ses buraya
    }

    public void TileHit(Tile t)
    {
        UnregisterTile(t);
        score += 100;
        combo++;
        UpdateUI();
        // efektler/ses buraya
    }

    // InputManager çağırır: bir sütuna dokunuldu/kliklendi.
    public void TryHitColumn(int column)
    {
        Tile best = null;
        float bestRemaining = float.MaxValue;

        // En uygun tile: aynı column, remainingTime küçük (yani neredeyse hit-zone'da)
        foreach (var tile in activeTiles)
        {
            if (tile.column != column) continue;
            float rem = tile.RemainingTime;
            if (Mathf.Abs(rem) <= hitWindowSeconds && rem < bestRemaining)
            {
                bestRemaining = rem;
                best = tile;
            }
        }

        if (best != null)
        {
            best.OnHit();
        }
        else
        {
            // Boş tap -> miss penalti
            misses++;
            combo = 0;
            UpdateUI();
            // yanlış dokunma efekti
        }
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (comboText) comboText.text = "Combo: " + combo;
        if (missText) missText.text = "Miss: " + misses;
    }
}
