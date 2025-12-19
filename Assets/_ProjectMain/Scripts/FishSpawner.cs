using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    [SerializeField] Fish fishPrefab;
    [SerializeField] GameObject fishParentObject;
    [SerializeField] Fish.FishType[] fishTypes;

    void Awake()
    {
        for (int i = 0; i < fishTypes.Length; i++)
        {
            int num = 0;
            while (num < fishTypes[i].fishCount)
            {
                Fish fish = Instantiate(fishPrefab, fishParentObject.transform);
                fish.Type = fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }
    }
    public void RespawnFish(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Fish.FishType selectedType = GetRandomFishType();
            Fish fish = Instantiate(fishPrefab, fishParentObject.transform);
            fish.Type = selectedType;
            fish.ResetFish();
        }
    }

    private Fish.FishType GetRandomFishType()
    {
        float totalWeight = 0f;
        for (int i = 0; i < fishTypes.Length; i++)
        {
            totalWeight += fishTypes[i].fishCount;
        }
        float rand = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < fishTypes.Length; i++)
        {
            cumulative += fishTypes[i].fishCount;
            if (rand <= cumulative)
            {
                return fishTypes[i];
            }
        }
        return fishTypes[0]; // Fallback
    }
}
