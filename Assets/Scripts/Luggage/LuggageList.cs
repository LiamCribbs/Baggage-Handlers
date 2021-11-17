using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu()]
public class LuggageList : ScriptableObject
{
    [Header("Object  |  Spawn Weight  |  Calculated Spawn Chance")]
    [Space(10)]
    public List<LuggagePrefab> luggage;

    int spawnWeightSum;

    void OnEnable()
    {
        // Get weight sum
        spawnWeightSum = 0;
        for (int i = 0; i < luggage.Count; i++)
        {
            spawnWeightSum += luggage[i].spawnWeight;
        }

        // Sort by spawnWeight
        luggage.Sort(Compare);
    }

    static int Compare(LuggagePrefab x, LuggagePrefab y)
    {
        return x.spawnWeight - y.spawnWeight;
    }

    /// <summary>
    /// Returns a random luggage PREFAB
    /// </summary>
    public GameObject GetRandom()
    {
        int spawnValue = Random.Range(0, spawnWeightSum);
        for (int i = 0; i < luggage.Count; i++)
        {
            if (spawnValue < luggage[i].spawnWeight)
            {
                return luggage[i].luggage;
            }

            spawnValue -= luggage[i].spawnWeight;
        }

        throw new System.Exception("This should never happen, we got a fuckup");
    }

    void OnValidate()
    {
        int length = luggage.Count;

        int spawnWeightSum = 0;
        for (int i = 0; i < length; i++)
        {
            spawnWeightSum += luggage[i].spawnWeight;
        }

        for (int i = 0; i < length; i++)
        {
            luggage[i].SPAWN_CHANCE = Mathf.RoundToInt((float)luggage[i].spawnWeight / spawnWeightSum * 100f).ToString() + '%';
        }
    }
}

[System.Serializable]
public class LuggagePrefab
{
    public GameObject luggage;
    public int spawnWeight = 1;

#if UNITY_EDITOR
    public string SPAWN_CHANCE;
#endif
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LuggagePrefab))]
public class LuggagePrefabDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, position.width * 0.5f, position.height);
        var unitRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.25f, position.height);
        var chanceRect = new Rect(position.x + position.width * 0.75f, position.y, position.width * 0.25f, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("luggage"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("spawnWeight"), GUIContent.none);
        GUI.enabled = false;
        EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("SPAWN_CHANCE"), GUIContent.none);
        GUI.enabled = true;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif