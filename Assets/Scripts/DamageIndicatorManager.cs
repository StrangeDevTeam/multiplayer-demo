using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    public static DamageIndicatorManager singleton;

    [SerializeField]
    private DamageIndicator _prefab;

    public List<DamageIndicator> prefabList = new List<DamageIndicator>();

    private void Start()
    {
        singleton = this;
    }

    public void SpawnDamageSplashNumber(Transform location, int damage)
    {
        DamageIndicator newGO = Instantiate(_prefab);
        newGO.transform.position = location.position;
        newGO.setText(damage.ToString());
        prefabList.Add(newGO);
    }
}
