using UnityEngine;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Сохраняем исходное состояние GUI.enabled
        bool wasEnabled = GUI.enabled;
        
        // Делаем поле не редактируемым
        GUI.enabled = false;
        
        // Рисуем стандартное поле (как обычно, но без возможности изменения)
        EditorGUI.PropertyField(position, property, label, true);
        
        // Восстанавливаем исходное состояние GUI.enabled
        GUI.enabled = wasEnabled;
    }
}