using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotName : MonoBehaviour
{
    public string Name;

    private void OnValidate()
    {
        if (transform.parent == null)
            return;

        int index = transform.GetSiblingIndex();
        gameObject.name = $"{Name}_Slot_{index + 1}";
    }
}
