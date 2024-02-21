using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Items/weapon")]
public class Weapon : Item
{
    public GameObject prefab;
    public int magazineSize;
    public int magazineCount;
    public float range;
}
