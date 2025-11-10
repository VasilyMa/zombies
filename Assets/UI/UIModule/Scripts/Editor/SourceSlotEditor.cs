#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(SourceSlot), true)]
public class SourceSlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Отрисовываем дефолтные поля
        DrawDefaultInspector();

        var slot = (SourceSlot)target;
        var go = slot.gameObject;

        // --- Кнопка ---
        var button = go.GetComponent<Button>();
        if (slot.IsButton)
        {
            if (button == null)
                go.AddComponent<Button>();
        }
        else
        {
            if (button != null)
                DestroyImmediate(button, false);
        }

        // --- Анимация ---
        var anim = go.GetComponent<Animation>();
        if (slot.IsAnimation)
        {
            if (anim == null)
                go.AddComponent<Animation>();
        }
        else
        {
            if (anim != null)
                DestroyImmediate(anim, false);
        }
    }
}
#endif