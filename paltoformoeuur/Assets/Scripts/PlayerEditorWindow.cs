using System;
using UnityEditor;
using UnityEngine;

public class PlayerEditorWindow : EditorWindow
{
    public BodyData bodyData;
    public HandData handData;
    public HeadData headData;

    [MenuItem("Tools/PlayerData")]
    public static void ShowWindow()
    {
        GetWindow<PlayerEditorWindow>("PlayerData");
    }

    private void OnGUI()
    {
        GUILayout.Label("Refs");

        bodyData = (BodyData)EditorGUILayout.ObjectField("body", bodyData, typeof(BodyData), false);
        handData = (HandData)EditorGUILayout.ObjectField("hand", handData, typeof(HandData), false);
        headData = (HeadData)EditorGUILayout.ObjectField("head", headData, typeof(HeadData), false);

        EditorGUILayout.Space(10);
        
        EditorGUILayout.BeginVertical();
        GUILayout.Label("body settings");
        bodyData.speed = EditorGUILayout.FloatField("speed", bodyData.speed);
        bodyData.sprintSpeedMultiplier = EditorGUILayout.FloatField("sprint Speed Multiplier", bodyData.sprintSpeedMultiplier);
        bodyData.jumpHeight = EditorGUILayout.FloatField("jump Height", bodyData.jumpHeight);
        bodyData.launchForce = EditorGUILayout.FloatField("launch Force", bodyData.launchForce);
        bodyData.coyoteTime = EditorGUILayout.FloatField("coyote Time", bodyData.coyoteTime);
        bodyData.bufferingTime = EditorGUILayout.FloatField("buffering Jump", bodyData.bufferingTime);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.BeginVertical();
        GUILayout.Label("hand settings");
        handData.speed = EditorGUILayout.FloatField("speed", handData.speed);
        handData.sprintSpeedMultiplier = EditorGUILayout.FloatField("sprint Speed Multiplier", handData.sprintSpeedMultiplier);
        handData.dashSpeed = EditorGUILayout.FloatField("dash Speed", handData.dashSpeed);
        handData.dashDuration = EditorGUILayout.FloatField("dash Duration",  handData.dashDuration);
        handData.dashCooldown = EditorGUILayout.FloatField("dash Cooldown",  handData.dashCooldown);
        handData.recallSpeed = EditorGUILayout.IntField("recall Speed", handData.recallSpeed);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.BeginVertical();
        GUILayout.Label("head settings");
        headData.recallSpeed = EditorGUILayout.IntField("recall Speed", headData.recallSpeed);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(40);

        
        //tqt c'est utile
        GUILayout.Label("qui a pété ?");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Button("Nash");
        GUILayout.Button("Simon");
        GUILayout.Button("Louis");
        GUILayout.Button("Louisa");
        GUILayout.Button("Hippolyte");
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Button("Luc");
        GUILayout.Button("Eva");
        GUILayout.Button("Elliot");
        GUILayout.Button("Rayane");
        GUILayout.Button("Salomé");
        EditorGUILayout.EndHorizontal();

    }
    

    private void OnEnable()
    {
        string[] body = AssetDatabase.FindAssets("t:BodyData");
        string[] hand = AssetDatabase.FindAssets("t:HandData");
        string[] head = AssetDatabase.FindAssets("t:HeadData");

        if (body.Length > 0)
        {
            bodyData = AssetDatabase.LoadAssetAtPath<BodyData>(AssetDatabase.GUIDToAssetPath(body[0]));
        }

        if (hand.Length > 0)
        {
            handData = AssetDatabase.LoadAssetAtPath<HandData>(AssetDatabase.GUIDToAssetPath(hand[0]));
        }
        if (head.Length > 0)
        {
            headData = AssetDatabase.LoadAssetAtPath<HeadData>(AssetDatabase.GUIDToAssetPath(head[0]));
        }
    }
}
