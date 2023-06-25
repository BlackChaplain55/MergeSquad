using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelTokensHelper : MonoBehaviour
{
    [Range(0.1f, 50)] public float Quality;
    [field: SerializeField] public LevelToken[] Levels { get; private set; }
    public List<Vector3> MiddlePoints;
    [SerializeField] private GameController game;
    [SerializeField] private LineRenderer lineRenderer;

    private void Awake()
    {
        Validate();
        game.OnSceneChanged += sceneIndex =>
        {
            Validate();
            SetupLevels();
        };
        if ((GameStates)SceneManager.GetActiveScene().buildIndex != GameStates.Map) return;
        SetupLevels();
    }

    private void OnValidate()
    {
        if ((GameStates)SceneManager.GetActiveScene().buildIndex != GameStates.Map)
            return;
        Validate();

        Array.Sort(Levels);
    }

    [ContextMenu("Setup Levels")]
    public void SetupLevels()
    {
        if (Levels == null) return;
        int i = 0;

        foreach (var levelToken in Levels)
        {
            if (levelToken == null) continue;
            int index = levelToken.transform.GetSiblingIndex();
            levelToken.SetLevel(i++);
            levelToken.SetEnabled(levelToken.LevelData.Level <= game.GameProgress.UnlockedLevel);
            levelToken.ToBattleButton.onClick.AddListener(() => game.LoadLevel(levelToken.LevelData));
        }
    }

    [ContextMenu("DrawRoute")]
    public void DrawRoute()
    {
        bool firstDraw = true;
        DrawRoute(firstDraw);
    }

    [ExecuteInEditMode]
    public void DrawRoute(bool firstDraw)
    {
        lineRenderer.positionCount = 0;
        if (firstDraw)
            MiddlePoints.Clear();

        for (int i = 0; i < Levels.Length - 1; i++)
        {
            Vector3 current = Levels[i].transform.position;
            Vector3 next = Levels[i + 1].transform.position;
            float distance = Vector3.Distance(current, next);
            int steps = Convert.ToInt32(distance * Quality);
            float stepLength = distance / steps;
            int prevPointsCount = lineRenderer.positionCount;
            lineRenderer.positionCount += steps;

            int positive = (int)Mathf.Round((i % 2 - 0.5f) * 2);

            if (firstDraw)
            {
                Vector3 mid = GetRandomMidPoint(current, next, positive);
                MiddlePoints.Add(GetRandomMidPoint(current, mid, positive));
                MiddlePoints.Add(GetRandomMidPoint(mid, next, positive));
            }
                
            Vector3 midpoint = MiddlePoints[i*2];
            Vector3 midpoint2 = MiddlePoints[i*2 + 1];
            for (int j = 0; j < steps; j++)
                lineRenderer.SetPosition(prevPointsCount + j, CubicInterpolation(current, midpoint, midpoint2, next, stepLength * j));
        }
    }

    private void Validate()
    {
        Debug.Log(Levels == null ? "Levels exists and contains null? - " + Levels?.Contains(null) : "Levels not exists");
        if (Levels == null || Levels?.Length < 1 || Levels.Contains(null))
            Levels = FindObjectsOfType<LevelToken>();
        if (game == null)
            game = GetComponent<GameController>();
        if (lineRenderer == null)
            lineRenderer = GameObject.FindGameObjectWithTag("MapRoute")?.GetComponent<LineRenderer>();
    }

    private Vector3 QuadraticInterpolation(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    private Vector3 CubicInterpolation(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticInterpolation(a, b, c, t);
        Vector3 bc_cd = QuadraticInterpolation(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

    private Vector3 GetRandomMidPoint(Vector3 current, Vector3 next, int positive)
    {
        float distance = Vector3.Distance(current, next);
        Vector3 mid = current * 0.5f + next * 0.5f;
        Vector2 normDirection = (current - next).normalized;
        Vector2 perp2d = Vector2.Perpendicular(normDirection);

        perp2d *= UnityEngine.Random.Range(0.25f, distance * 0.5f) * positive;
        return mid + new Vector3(perp2d.x, perp2d.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var point in MiddlePoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(point, 0.15f);
        }
    }
}

[CustomEditor(typeof(LevelTokensHelper))]
public class SpawnerHelper : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        LevelTokensHelper levelHelper = (LevelTokensHelper)target;

        var midPoints = levelHelper.MiddlePoints;
        int count = midPoints.Count;
        for (int i = 0; i < count; i++)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 point = Handles.PositionHandle(midPoints[i], Quaternion.identity);
            var style = new GUIStyle();
            style.fontSize = 32;
            Handles.Label(midPoints[i], (i + 1).ToString(), style);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Route curve midpoint");

                midPoints[i] = point;

                bool firstDraw = false;
                levelHelper.DrawRoute(firstDraw);
            }
        }
    }
}