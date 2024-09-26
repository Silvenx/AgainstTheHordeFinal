using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DeckObjects", order = 2)]
public class DeckObjects : ScriptableObject
{
    public List<CardObjects> cardList;
}
