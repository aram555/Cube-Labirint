using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Skin ID and Cost")]
    [SerializeField] private int ID;
    [SerializeField] private int cost;
    private void OnMouseUpAsButton() {
        Shop.Instance.Buy(ID, cost);
    }
}
