using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SleutelCollectible))]
public class SleutelCollectibleEditor : Editor
{
    private Texture2D iconTexture;

    bool showAdvanced = false;
    bool showMocement = false;
    bool clicked;
    SerializedProperty speedProp;
    SerializedProperty xaxis;
    SerializedProperty yaxis;
    SerializedProperty zaxis;
    SerializedProperty amplitude;
    SerializedProperty frequency;


    private void OnEnable()
    {
        iconTexture = EditorGUIUtility.IconContent("console.warnicon").image as Texture2D;
        speedProp = serializedObject.FindProperty("speed");
        xaxis = serializedObject.FindProperty("xaxis");
        yaxis = serializedObject.FindProperty("yaxis");
        zaxis = serializedObject.FindProperty("zaxis");
        amplitude = serializedObject.FindProperty("amplitude");
        frequency = serializedObject.FindProperty("frequency");
    }

    public override void OnInspectorGUI()
    {
        SleutelCollectible script = (SleutelCollectible)target;
        EditorGUILayout.HelpBox("Dit is een sleutelscript. Je moet het verwijzen aan een slotscript om het ook iets te laten doen", MessageType.Info);

        EditorGUI.BeginChangeCheck();

        Collider newCollider = (Collider)EditorGUILayout.ObjectField("Trigger Collider",script.col,typeof(Collider),true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(script, "col");
            script.col = newCollider;
            EditorUtility.SetDirty(script);
        }

        if (script.col == null)
        {
            EditorGUILayout.HelpBox("Er is geen collider component gevonden op dit object. Voeg een collider toe om dit script te laten werken", MessageType.Error);
        }
        else if(script.col.isTrigger == false)
        {
            EditorGUILayout.HelpBox("De collider van dit object moet worden ingesteld als een trigger. Vink 'isTrigger' aan in de collider instellingen", MessageType.Error);
        }


        // Toggle knop
        if (GUILayout.Button("Collectible Draaien"))
        {
            showAdvanced = !showAdvanced;
        }

        // Alleen tonen wanneer knop actief is
        if (showAdvanced)
        {
            EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(xaxis);
            EditorGUILayout.PropertyField(yaxis);
            EditorGUILayout.PropertyField(zaxis);
            speedProp.floatValue = EditorGUILayout.Slider("Draaisnelheid", speedProp.floatValue, 1f, 100f);
        }

        if (GUILayout.Button("Collectible Bewegen"))
        {
            showMocement = !showMocement;
        }
        if (showMocement)
        {
            amplitude.floatValue = EditorGUILayout.Slider("Amplitude", amplitude.floatValue, 0.1f, 2f);
            frequency.floatValue = EditorGUILayout.Slider("Frequentie", frequency.floatValue, 0.1f, 5f);
        }
        serializedObject.ApplyModifiedProperties();
    }
}