using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PathCreation;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private int waypointSteps = 100;

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
            waypointSteps = EditorGUILayout.IntField("Waypoint Amount: ", waypointSteps);
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
        if (GUILayout.Button("Calculate based on Vertex Path"))
        {
            CalculateFromVertexPath();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }

        }
    }

    void CalculateFromVertexPath()
    {
        if (waypointRoot.GetComponent<PathCreator>() != null)
        {
            VertexPath vertexPath = waypointRoot.GetComponent<PathCreator>().editorData.GetVertexPath(waypointRoot.transform);
            if (vertexPath != null)
            {
                int temp = 1 / waypointSteps;
                for (float i = 0f; i < 1; i = i + temp)
                {
                    Vector3 position = vertexPath.GetPointAtTime(i, EndOfPathInstruction.Loop);
                    Vector3 rotation = vertexPath.GetDirection(i, EndOfPathInstruction.Loop);

                    GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
                    waypointObject.transform.SetParent(waypointRoot, false);

                    Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

                    if (waypointRoot.childCount > 1)
                    {
                        waypoint.SetPrevWaypoint(waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>());
                        waypoint.GetPrevWaypoint().SetNextWaypoint(waypoint);
                    }

                    waypoint.transform.position = position;
                    waypoint.transform.forward = rotation;
                }

                ConnectLastAndFirstWaypoint();
                CalculateRoad();
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
