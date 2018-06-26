using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (TownBuilder))]
public class TownBuilderEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI ();
        TownBuilder builder = target as TownBuilder;

        if (GUILayout.Button ("Generate"))
        {
            builder.Generate ();
        }

        if (GUILayout.Button ("Generate Random"))
        {
            builder.GenerateRandom ();
        }
    }
}