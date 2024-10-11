using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(Deck))]
public class DeckEditor : PropertyDrawer
{
    private ReorderableList reorderableList;

    private void Initialize(SerializedProperty property)
    {
        // Find the cardList property
        SerializedProperty cardListProp = property.FindPropertyRelative("cardList");

        // Create the reorderable list
        reorderableList = new ReorderableList(property.serializedObject, cardListProp, true, true, true, true);

        // Set up the display of each element
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = cardListProp.GetArrayElementAtIndex(index);
            if (element != null)
            {
                SerializedProperty cardNameProp = element.FindPropertyRelative("cardName");
                string cardLabel = cardNameProp != null ? cardNameProp.stringValue : "Unnamed Card";
                EditorGUI.PropertyField(rect, element, new GUIContent(cardLabel), true);
            }
        };

        // Set up the header
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, property.displayName);
        };
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Initialize the reorderable list if not done already
        if (reorderableList == null)
        {
            Initialize(property);
        }

        // Draw the reorderable list
        reorderableList.DoList(position);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (reorderableList == null)
        {
            Initialize(property);
        }

        return reorderableList.GetHeight();
    }
}
