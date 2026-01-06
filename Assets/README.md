# Magic-Tiles Unity Prototype

Basit Magic Tiles tarzı prototip. Unity 2022.3 LTS hedeflenmiştir.

Kurulum özeti:
1. Yeni Unity 2D proje oluşturun (Unity 2022.3 LTS önerilir).
2. Bu repo'daki `Assets/` içeriğini proje klasörünüze kopyalayın veya projeyi klonlayın.
3. Sahne kurulum adımları için Assets/README.md içindeki yönergeleri takip edin (spawn points, hit zone, prefab, UI bağlamaları).

Ayrıca assets içinde örnek beatmap ve temel scriptler vardır (Tile, TileSpawner, GameManager, InputManager).

Mobil için notlar:
- Audio latency ve input offset için calibrasyon ekranı eklemeniz önerilir.
- Performans: Instantiate/Destroy yerine object pooling ekleyin.
