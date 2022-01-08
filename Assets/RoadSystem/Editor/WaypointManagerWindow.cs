using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    public void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if(waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Empty Gameobjekt aus waypointRoot anlegen",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if(GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if (GUILayout.Button("Loop"))
        {
            ConnectLastAndFirstWaypoint();
        }
        if (GUILayout.Button("Calculate Road"))
        {
            CalculateRoad();
        }


        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWayPointBefore();
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }

        }
    }

    void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if(waypointRoot.childCount > 1)
        {
            waypoint.SetPrevWaypoint(waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>());
            waypoint.GetPrevWaypoint().SetNextWaypoint(waypoint);

            waypoint.transform.position = waypoint.GetPrevWaypoint().transform.position;
            waypoint.transform.forward = waypoint.GetPrevWaypoint().transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateWayPointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if (selectedWaypoint.GetPrevWaypoint() != null)
        {
            newWaypoint.SetPrevWaypoint(selectedWaypoint.GetPrevWaypoint());
            selectedWaypoint.GetPrevWaypoint().SetNextWaypoint(newWaypoint);
        }

        newWaypoint.SetNextWaypoint(selectedWaypoint);

        selectedWaypoint.SetPrevWaypoint(newWaypoint);

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.SetPrevWaypoint(selectedWaypoint);

        if (selectedWaypoint.GetNextWaypoint() != null)
        {
            selectedWaypoint.GetNextWaypoint().SetPrevWaypoint(newWaypoint);
            newWaypoint.SetNextWaypoint(selectedWaypoint.GetNextWaypoint());
        }

        selectedWaypoint.SetNextWaypoint(newWaypoint);

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void RemoveWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectedWaypoint.GetNextWaypoint() != null)
        {
            selectedWaypoint.GetNextWaypoint().SetPrevWaypoint(selectedWaypoint.GetPrevWaypoint());
        }
        if (selectedWaypoint.GetPrevWaypoint() != null)
        {
            selectedWaypoint.GetPrevWaypoint().SetNextWaypoint(selectedWaypoint.GetNextWaypoint());
            Selection.activeGameObject = selectedWaypoint.GetPrevWaypoint().gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }

    void ConnectLastAndFirstWaypoint()
    {
        Waypoint lastWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 1).gameObject.GetComponent<Waypoint>();
        Waypoint firstWaypoint = waypointRoot.GetChild(0).gameObject.GetComponent<Waypoint>();

        lastWaypoint.SetNextWaypoint(firstWaypoint);
        firstWaypoint.SetPrevWaypoint(lastWaypoint);
    }

    void CalculateRoad()
    {
        for (int i = 0; i < waypointRoot.childCount; i++)
        {
            waypointRoot.GetChild(i).GetComponent<Waypoint>().CalculateAllValues();
        }
    }

}
