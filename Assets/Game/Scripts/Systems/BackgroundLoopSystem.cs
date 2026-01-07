using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class BackgroundLoopSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private GameObject _skyPrefab;
    private float _skyWidth;
    private GameObject _backgroundPrefab;
    private float _backgroundWidth;
    private GameObject _middlegroundPrefab;
    private float _middlegroundWidth;
    private GameObject _foregroundPrefab;
    private float _foregroundWidth;
    private GameObject _groundPrefab;
    private float _groundWidth;

    private List<GameObject> _firstGrounds = new List<GameObject>();
    private List<GameObject> _foreGrounds = new List<GameObject>();
    private List<GameObject> _middleGrounds = new List<GameObject>();
    private List<GameObject> _backGrounds = new List<GameObject>();
    private List<GameObject> _skyGrounds = new List<GameObject>();

    private Transform _camera;
    private float _lastDirection;

    public void Init()
    {
        _skyPrefab = GameConfig.LevelConfig.SkyPrefab;
        _foregroundPrefab = GameConfig.LevelConfig.ForegroundPrefab;
        _middlegroundPrefab = GameConfig.LevelConfig.MiddlegroundPrefab;
        _backgroundPrefab = GameConfig.LevelConfig.BackgroundPrefab;
        _groundPrefab = GameConfig.LevelConfig.GroundPrefab;

        _skyWidth = _skyPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _backgroundWidth = _backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _foregroundWidth = _foregroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _middlegroundWidth = _middlegroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _groundWidth = _groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        _camera = Camera.main.transform;
        _lastDirection = _camera.position.x;
        SpawnInitial(_skyGrounds, _skyPrefab, _skyWidth);
        SpawnInitial(_foreGrounds, _foregroundPrefab, _foregroundWidth);
        SpawnInitial(_middleGrounds, _middlegroundPrefab, _middlegroundWidth);
        SpawnInitial(_backGrounds, _backgroundPrefab, _backgroundWidth);
        SpawnInitial(_firstGrounds, _groundPrefab, _groundWidth);
    }

    public void Run()
    {
        float dx = _camera.position.x - _lastDirection;

        if (Mathf.Abs(dx) < 0.01f) return;

        if (dx > 0)
        {
            UpdateLayer(_skyGrounds, _skyPrefab, _skyWidth, true, 0, 30);
            UpdateLayer(_foreGrounds, _foregroundPrefab, _foregroundWidth, true, -0.9f, 12);
            UpdateLayer(_middleGrounds, _middlegroundPrefab, _middlegroundWidth, true, -0.7f, 18);
            UpdateLayer(_backGrounds, _backgroundPrefab, _backgroundWidth, true, 0, 20);
            UpdateLayer(_firstGrounds, _groundPrefab, _groundWidth, true, -6, 7);
        }
        else
        {
            UpdateLayer(_skyGrounds, _skyPrefab, _skyWidth, false, 0, 30);
            UpdateLayer(_foreGrounds, _foregroundPrefab, _foregroundWidth, false, -0.9f, 12);
            UpdateLayer(_middleGrounds, _middlegroundPrefab, _middlegroundWidth, false, -0.7f, 18);
            UpdateLayer(_backGrounds, _backgroundPrefab, _backgroundWidth, false, 0, 20);
            UpdateLayer(_firstGrounds, _groundPrefab, _groundWidth, false, -6, 7);
        }

        _lastDirection = _camera.position.x;
    }

    private void SpawnInitial(List<GameObject> list, GameObject prefab, float width)
    {
        float camX = _camera.position.x;

        GameObject left = Object.Instantiate(prefab);
        GameObject mid = Object.Instantiate(prefab);
        GameObject right = Object.Instantiate(prefab);

        list.Add(left);
        list.Add(mid);
        list.Add(right);
    }

    private void UpdateLayer(List<GameObject> list, GameObject prefab, float width, bool movingRight, float hight, float far)
    {
        if (movingRight)
        {
            GameObject rightMost = list[^1];

            if (_camera.position.x > rightMost.transform.position.x - width)
            {
                float newX = rightMost.transform.position.x + width;
                var g = Object.Instantiate(prefab, new Vector3(newX, hight, far), Quaternion.identity);
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
                var g = Object.Instantiate(prefab, new Vector3(newX, hight, far), Quaternion.identity);
                list.Insert(0, g);

                Object.Destroy(list[^1]);
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
