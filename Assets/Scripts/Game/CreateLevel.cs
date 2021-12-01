using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField] private Platform prefabPlatform;
    [SerializeField] private Player prefabPlayer;
    [SerializeField] private List<Enemy> prefabsEnemy;
    [SerializeField] private Monet prefabMonets;

    private readonly int maxPlatforms = 12;
    private readonly int intervalPlatforms = 2;
    private float spawnPosY = 0;
    private readonly float rangeSpawnPosXEnemy = 2;
    private readonly float intervalUnit = 0.5f;

    private Player player;
    private Queue<Platform> ArrayPlatforms;

    private void Awake()
    {
        ArrayPlatforms = new Queue<Platform>();
    }

    void Start()
    {
        SpawnPlatforms();
        CreatePlayer();
        Player.ActionEvent += NewPlatformCreate;
    }

    private void OnDisable()
    {
        Player.ActionEvent -= NewPlatformCreate;
    }

    private void NewPlatformCreate() //Уничтожение нижней и появление новой платформ
    {
        Destroy(ArrayPlatforms.Dequeue());
        CreateEnemy();
        CreateMonet();
        ArrayPlatforms.Enqueue(Instantiate(prefabPlatform, GenerateSpawnPositionPlatforms(), Quaternion.identity));
    }


    private void SpawnPlatforms() // Генерация платформ
    {
        for (int i = 0; i < maxPlatforms; i++)
        {
            ArrayPlatforms.Enqueue(Instantiate(prefabPlatform, GenerateSpawnPositionPlatforms(), Quaternion.identity));
            CreateEnemy();
            CreateMonet();
        }
    }

    private Vector3 GenerateSpawnPositionPlatforms() // Позиция для новых платформ
    {
        Vector3 randomPos = new Vector3(0, spawnPosY, 0);
        spawnPosY += intervalPlatforms;
        return randomPos;
    }

    private Player CreatePlayer() // Генерация плеера
    {
        player = Instantiate(prefabPlayer, new Vector3(0, intervalUnit, 0), Quaternion.identity);
        Camera.player = player;

        return player;
    }

    private Vector3 GenerateSpawnPositionEnemy()
    {
        float PosXEnemy = Random.Range(-rangeSpawnPosXEnemy, rangeSpawnPosXEnemy);
        float PosYEnemy = spawnPosY + intervalUnit;

        return new Vector3(PosXEnemy, PosYEnemy, 0);

    }

    private Enemy CreateEnemy()
    {
        int randomIndex = Random.Range(0, 3);
        int randomEnemy = Random.Range(0, prefabsEnemy.Count);

       if (randomIndex != 1)
        {
            return Instantiate(prefabsEnemy[randomEnemy], GenerateSpawnPositionEnemy(), Quaternion.identity);

        }
       else
        return null;
    }

    private Monet CreateMonet()
    {
        int randomIndex = Random.Range(0, 4);

        if (randomIndex == 1)
        {
            return Instantiate(prefabMonets, GenerateSpawnPositionEnemy(), Quaternion.identity);

        }
        else
            return null;
    }
}
