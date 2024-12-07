using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    #region Singleton
    
    private static CollectablesManager instance;
    public static CollectablesManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion
    
    public List<Collectable> AvailableBuffs;
    public List<Collectable> AvailableDebuffs;

    [Range(0, 100)] public float buffSpawnChance;
    [Range(0, 100)] public float debuffSpawnChance;
}
