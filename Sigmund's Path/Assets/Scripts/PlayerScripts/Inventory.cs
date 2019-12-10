using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        [HideInInspector]
        public bool isFull;
        public GameObject slotGO;
    };
    //public bool[] isFull;
    //public GameObject[] slots;

    public Slot[] slotssss;
}
