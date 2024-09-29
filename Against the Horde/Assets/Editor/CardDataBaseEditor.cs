#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CustomEditor(typeof(CardDatabase))]
public class CardDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardDatabase cardDatabase = (CardDatabase)target;

        if (GUILayout.Button("Populate Database"))
        {
            PopulateDatabase(cardDatabase);
        }
    }

    private void PopulateDatabase(CardDatabase cardDatabase)
    {
        string[] guids = AssetDatabase.FindAssets("t:CardObjects");
        List<CardObjects> allCards = new List<CardObjects>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CardObjects cardObj = AssetDatabase.LoadAssetAtPath<CardObjects>(assetPath);
            if (cardObj != null)
            {
                allCards.Add(cardObj);
            }
        }

        cardDatabase.allCards = allCards;
        EditorUtility.SetDirty(cardDatabase);
        Debug.Log($"CardDatabase populated with {allCards.Count} cards.");
    }
}
#endif
