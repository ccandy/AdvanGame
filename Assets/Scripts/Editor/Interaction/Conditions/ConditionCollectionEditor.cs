using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConditionCollection))]
public class ConditionCollectionEditor : EditorWithSubEditors<ConditionEditor, Condition>
{
    public SerializedProperty collectionsProperty;

    private ConditionCollection conditionCollection;

    private SerializedProperty descriptionProperty;
    private SerializedProperty conditionProperty;
    private SerializedProperty reactionProperty;

    private const float conditionButtonWidth    = 30f;
    private const float collectionButtonWidth   = 125f;

    private const string condtionCollectionPropDescriptionName          = "description";
    private const string condtionCollectionPropRequiredCondtionName     = "requiredConditions";
    private const string condtionCollectionPropReactionCollectionName   = "reactionCollection";


    private void OnEnable()
    {
        conditionCollection = (ConditionCollection)target;
        if(target == null)
        {
            DestroyImmediate(this);
            return;
        }

        descriptionProperty     = serializedObject.FindProperty(condtionCollectionPropDescriptionName);
        conditionProperty       = serializedObject.FindProperty(condtionCollectionPropRequiredCondtionName);
        reactionProperty        = serializedObject.FindProperty(condtionCollectionPropReactionCollectionName);

        CheckAndCreateSubEditor(conditionCollection.requiredConditions);
    }

    private void OnDisable()
    {
        CleanUpEditors();
    }

    protected override void SubEditorSetup(ConditionEditor editor)
    {
        editor.editorType           = ConditionEditor.EditorType.ConditionCollection;
        editor.conditionsProperty   = conditionProperty;

    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CheckAndCreateSubEditor(conditionCollection.requiredConditions);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginHorizontal();
        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);
        if(GUILayout.Button("Remove Collection", GUILayout.Width(collectionButtonWidth)))
        {
            collectionsProperty.RemoveFromObjectArray(conditionCollection);
        }

        if (descriptionProperty.isExpanded)
        {
            ExpandedGUI();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();

    }

    private void ExpandedGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(descriptionProperty);
        EditorGUILayout.Space();

        float space = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition",     GUILayout.Width(space));
        EditorGUILayout.LabelField("Satisfied",     GUILayout.Width(space));
        EditorGUILayout.LabelField("Add/Remove",    GUILayout.Width(space));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for(int n = 0; n < subEditors.Length; n++)
        {
            subEditors[n].OnInspectorGUI();
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+", GUILayout.Width(conditionButtonWidth)))
        {
            Condition newCondition = ConditionEditor.CreateCondition();
            conditionProperty.AddToObjectArray(newCondition);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(reactionProperty);

    }

    public static ConditionCollection CreateCondtionCollection()
    {
        ConditionCollection newConditionCollection      = CreateInstance<ConditionCollection>();
        newConditionCollection.description              = "New Condtion Collection";
        newConditionCollection.requiredConditions       = new Condition[1];
        newConditionCollection.requiredConditions[0]    = ConditionEditor.CreateCondition();
        return newConditionCollection;
    }
}

