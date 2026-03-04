using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SlotScript))]
public class SlotScriptEditor : Editor
{
    // Static omdat DrawGizmo static is
    private static Texture2D warningIcon;

    private void OnEnable()
    {
        if (warningIcon == null)
            warningIcon = EditorGUIUtility.IconContent("console.warnicon").image as Texture2D;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SlotScript slot = (SlotScript)target;

        if (slot.sleutel_collectibles == null || slot.sleutel_collectibles.Count == 0 || slot.sleutel_collectibles[0] == null)
        {
            EditorGUILayout.HelpBox(
                "kijk uit, geen sleutel verbonden aan dit slotscript. Er is niks dat de deuranimatie kan activeren",
                MessageType.Warning
            );
        }

        if (slot.animator == null)
        {
            EditorGUILayout.HelpBox(
                "kijk uit, geen animator component gevonden op dit object, er is geen animatie om af te spelen wanneer de sleutels zijn verzameld",
                MessageType.Warning
            );
        }

        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }


    // 🔥 Dit tekent de gizmo in Scene View
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void DrawSlotGizmo(SlotScript slot, GizmoType gizmoType)
    {
        if (slot.sleutel_collectibles != null && slot.sleutel_collectibles.Count > 0 && slot.sleutel_collectibles[0] != null)
            return;

        if (warningIcon == null)
            warningIcon = EditorGUIUtility.IconContent("console.warnicon").image as Texture2D;

        if (warningIcon == null)
            return;

        Vector3 worldPos = slot.transform.position + Vector3.up * slot.emptyWarningHeight;
        Vector2 guiPoint = HandleUtility.WorldToGUIPoint(worldPos);

        Handles.BeginGUI();

        Color old = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, slot.emptyWarningAlpha);

        float size = slot.emptyWarningIconSize;

        GUI.DrawTexture(
            new Rect(guiPoint.x - size / 2f,
                     guiPoint.y - size / 2f,
                     size,
                     size),
            warningIcon
        );

        GUI.color = old;
        Handles.EndGUI();
    }
}