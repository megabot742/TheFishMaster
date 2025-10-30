using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    [SerializeField] Fish fishPrefab;
    [SerializeField] GameObject fishParentObject;
    [SerializeField] Fish.FishType[] fishTypes;

    void Awake()
    {
        for(int i = 0; i < fishTypes.Length; i++)
        {
            int num = 0;
            while(num < fishTypes[i].fishCount)
            {
                Fish fish = Instantiate(fishPrefab,fishParentObject.transform);
                fish.Type = fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }   
    }
}
