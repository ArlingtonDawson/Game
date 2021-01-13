using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class StartupBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectPlayer;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    // Start is called before the first frame update
    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, new BlobAssetStore());
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPlayer, settings);

        SpawnPlayer(new float3(0,0,0));
    }

    private void SpawnPlayer(float3 position)
    {
        Entity newEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(newEntity, new Translation { Value = position });
    }
}
