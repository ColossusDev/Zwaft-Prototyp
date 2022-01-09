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
    private int roadWidth = 10;

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

            //Manuelle Funktionen
            EditorGUILayout.LabelField("Manuell Waypoint Functions");
            if (GUILayout.Button("Create Waypoint"))
            {
                CreateWaypoint();
            }
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Remove Waypoint"))
                {
                    RemoveWaypoint();
                }
            }

            //Automatische Funktionen
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Automatic Waypoint System");
            if (GUILayout.Button("Calculate based on Vertex Path"))
            {
                CalculateFromVertexPath();
            }
            waypointSteps = EditorGUILayout.IntField("Waypoint Amount: ", waypointSteps);
            roadWidth = EditorGUILayout.IntField("Road Width: ", roadWidth);
            if (GUILayout.Button("Delete all Waypoints"))
            {
                ClearWaypoint();
            }
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Remove Waypoint"))
                {
                    RemoveWaypoint();
                }
            }

            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void CalculateFromVertexPath()
    {
        if (waypointRoot.GetComponent<PathCreator>() != null)
        {
            VertexPath vertexPath = waypointRoot.GetComponent<PathCreator>().editorData.GetVertexPath(waypointRoot.transform);
            if (vertexPath != null)
            {
                float stepSize = (float)(1f / waypointSteps);
                Debug.Log("stepSize: " + stepSize);
                float posOnPath = 0;

                for (float i = 0f; i < waypointSteps; i++)
                {
                    Vector3 position = vertexPath.GetPointAtTime(posOnPath, EndOfPathInstruction.Loop);
                    Vector3 rotation = vertexPath.GetDirection(posOnPath, EndOfPathInstruction.Loop);

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
                    waypoint.width = roadWidth;

                    posOnPath += stepSize;
                }


                ConnectLastAndFirstWaypoint();
                CalculateRoad();
            }
        }
    }

    void CalculateFromVertexPath2()
    {
        if (waypointRoot.GetComponent<PathCreator>() != null)
        {
            VertexPath vertexPath = waypointRoot.GetComponent<PathCreator>().editorData.GetVertexPath(waypointRoot.transform);
            if (vertexPath != null)
            {
                float stepSize = (float)(1f / waypointSteps);
                Debug.Log("stepSize: " + stepSize);
                float posOnPath = 0;

                //Vector Erkennung (mehr Waypoints in Kurven)
                Vector3 currentDirection = new Vector3(0,0,0);


                for (float i = 0f; i < waypointSteps; i++)
                {
                    Vector3 position = vertexPath.GetPointAtTime(posOnPath, EndOfPathInstruction.Loop);
                    Vector3 rotation = vertexPath.GetDirection(posOnPath, EndOfPathInstruction.Loop);

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
                    waypoint.width = roadWidth;

                    posOnPath += stepSize;
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

    void ClearWaypoint()
    {
        int temp = waypointRoot.childCount;
        for (int i = 0; i < temp; i++)
        {
            DestroyImmediate(waypointRoot.GetChild(0).GetComponent<Waypoint>().gameObject);
        }
    }

}
