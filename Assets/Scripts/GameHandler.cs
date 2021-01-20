using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    [SerializeField] private GameObject gameObjectPlayer;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    private void Awake()
    {
        Instance = this;
        new AmmoDisplayHandler();
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        AmmoDisplayHandler.Instance.OnAmmoChanged += GameHandler_OnAmmoChanged;
    }

    private void GameHandler_OnAmmoChanged(object sender, AmmoDisplayHandler.OnAmmoDiplayChangeArgs e)
    {
        Debug.Log("Game Manager: Reload Event");
    }

    private void SpawnPlayer(float3 position)
    {
        Entity newEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(newEntity, new Translation { Value = position });
    }
}
