using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //0 = primary, 2 = secondary
    public GameObject[] weapons;
    private int primarySecondary;

    private void Start()
    {
        //InitVariables();
        primarySecondary = gameObject.GetComponent<PlayerMovement2>().primarySecondary ;
    }

    private void Update()
    {
        
    }

    public void AddItem(GameObject item, int index)
    {
        if (weapons[index] != null)
        {
            if ((index + 1) <= 1 && weapons[index + 1] == null)
            {
                weapons[index + 1] = item;
                primarySecondary = 1;
                gameObject.GetComponent<PlayerMovement2>().buttonPrimaryToSecondary();
            }
            else
            {
                RemoveItem(index);
                weapons[index] = item;
                gameObject.GetComponent<PlayerMovement2>().ChangeWeapon(GetItem(primarySecondary), item);
            }

        }

    }
    public void RemoveItem(int index)
    {
        weapons[index] = null;
    }

    
    public GameObject GetItem(int primarySecondary)
    {
        return weapons[primarySecondary];
    }

    private void InitVariables()
    {
        //weapons = new GameObject[2];
    }
}
