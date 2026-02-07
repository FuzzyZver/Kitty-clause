using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class BackgroundLoopSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private GameObject _skyPrefab;
    private GameObject _backgroundPrefab;
    private GameObject _middlegroundPrefab;
    private GameObject _foregroundPrefab;
    private GameObject _groundPrefab;

    private float _skyWidth;
    private float _backgroundWidth;
    private float _middlegroundWidth;
    private float _foregroundWidth;
    private float _groundWidth;

    private readonly List<GameObject> _sky = new();
    private readonly List<GameObject> _background = new();
    private readonly List<GameObject> _middleground = new();
    private readonly List<GameObject> _foreground = new();
    private readonly List<GameObject> _ground = new();

    private Transform _camera;
    private float _lastCamX;

    public void Init()
    {
        _camera = Camera.main.transform;
        _lastCamX = _camera.position.x;

        _skyPrefab = GameConfig.LevelConfig.SkyPrefab;
        _backgroundPrefab = GameConfig.LevelConfig.BackgroundPrefab;
        _middlegroundPrefab = GameConfig.LevelConfig.MiddlegroundPrefab;
        _foregroundPrefab = GameConfig.LevelConfig.ForegroundPrefab;
        _groundPrefab = GameConfig.LevelConfig.GroundPrefab;

        _skyWidth = GetWidth(_skyPrefab);
        _backgroundWidth = GetWidth(_backgroundPrefab);
        _middlegroundWidth = GetWidth(_middlegroundPrefab);
        _foregroundWidth = GetWidth(_foregroundPrefab);
        _groundWidth = GetWidth(_groundPrefab);

        SpawnInitial(_sky, _skyPrefab, _skyWidth);
        SpawnInitial(_background, _backgroundPrefab, _backgroundWidth);
        SpawnInitial(_middleground, _middlegroundPrefab, _middlegroundWidth);
        SpawnInitial(_foreground, _foregroundPrefab, _foregroundWidth);
        SpawnInitial(_ground, _groundPrefab, _groundWidth);
    }

    public void Run()
    {
        float camX = _camera.position.x;
        float dx = camX - _lastCamX;

        if (Mathf.Abs(dx) < 0.01f) return;

        bool movingRight = dx > 0;

        UpdateLayer(_sky, _skyPrefab, _skyWidth, movingRight);
        UpdateLayer(_background, _backgroundPrefab, _backgroundWidth, movingRight);
        UpdateLayer(_middleground, _middlegroundPrefab, _middlegroundWidth, movingRight);
        UpdateLayer(_foreground, _foregroundPrefab, _foregroundWidth, movingRight);
        UpdateLayer(_ground, _groundPrefab, _groundWidth, movingRight);

        _lastCamX = camX;
    }

    // -------------------- HELPERS --------------------

    private float GetWidth(GameObject prefab)
    {
        return prefab.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void SpawnInitial(List<GameObject> list, GameObject prefab, float width)
    {
        float camX = _camera.position.x;
        Vector3 basePos = prefab.transform.position;

        GameObject left = Object.Instantiate(
            prefab,
            new Vector3(camX - width, basePos.y, basePos.z),
            Quaternion.identity
        );

        GameObject mid = Object.Instantiate(
            prefab,
            new Vector3(camX, basePos.y, basePos.z),
            Quaternion.identity
        );

        GameObject right = Object.Instantiate(
            prefab,
            new Vector3(camX + width, basePos.y, basePos.z),
            Quaternion.identity
        );

        list.Add(left);
        list.Add(mid);
        list.Add(right);
    }

    private void UpdateLayer(List<GameObject> list, GameObject prefab, float width, bool movingRight)
    {
        if (movingRight)
        {
            GameObject rightMost = list[^1];

            if (_camera.position.x > rightMost.transform.position.x - width)
            {
                float newX = rightMost.transform.position.x + width;
                Vector3 pos = new Vector3(newX, prefab.transform.position.y, prefab.transform.position.z);

                GameObject g = Object.Instantiate(prefab, pos, Quaternion.identity);
                list.Add(g);

                Object.Destroy(list[0]);
                list.RemoveAt(0);
            }
        }
        else
        {
            GameObject leftMost = list[0];

            if (_camera.position.x < leftMost.transform.position.x + width)
            {
                float newX = leftMost.transform.position.x - width;
                Vector3 pos = new Vector3(newX, prefab.transform.position.y, prefab.transform.position.z);

                GameObject g = Object.Instantiate(prefab, pos, Quaternion.identity);
                list.Insert(0, g);

                Object.Destroy(list[^1]);
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
