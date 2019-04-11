using UnityEngine;
using UnityEditor;

public abstract class EditorWithSubEditors<TEditor, TTarget> : Editor
    where TEditor : Editor
    where TTarget : Object
{
    protected TEditor[] subEditors;

    protected void CheckAndCreateSubEditor(TTarget[] subEditorTargets)
    {

        if (subEditors != null && subEditors.Length == subEditorTargets.Length)
        {
            return;
        }

        CleanUpEditors();

        subEditors = new TEditor[subEditorTargets.Length];

        for (int n = 0; n < subEditors.Length; n++)
        {
            subEditors[n] = CreateEditor(subEditorTargets[n]) as TEditor;
            SubEditorSetup(subEditors[n]);


        }
    }

    protected void CleanUpEditors()
    {
        if (subEditors == null || subEditors.Length == 0)
        {
            return;

        }


        for (int n = 0; n < subEditors.Length; n++)
        {
            DestroyImmediate(subEditors[n]);
        }
        subEditors = null;
    }

    protected abstract void SubEditorSetup(TEditor editor);
    
}
