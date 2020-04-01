using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public AIEnemy Enemy;
    public Wave[] EnemyWaves;

    [System.Serializable]
    public class Wave
    {
        public int EnemyNum;
        public float TimeBetweenSpawn;
        public MapInfo MapInfomation;
    }

    int m_CurWaveEnemyNum;
    int m_CurWaveEnemykilled;

    MapGenerator m_MapGenerator;

    public event System.Action<int> WaveChange;

    Player m_Player;

    bool bFinalWave;

    private void Awake()
    {
        m_MapGenerator = FindObjectOfType<MapGenerator>() as MapGenerator;
        m_Player = FindObjectOfType<Player>();
        bFinalWave = false;
        AIEnemy.OnKilledStatic += EnemyKilled;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        for(int i = 0; i < EnemyWaves.Length; i++)
        {
            if (i == EnemyWaves.Length - 1)
                bFinalWave = true;

            int seed = UnityEngine.Random.Range(1, 1000);
            MapInfo mapinfo = new MapInfo(new Vector2(19, 25), 0.1f + 0.04f *(float)(i+1));
            m_MapGenerator.GenerateMap(seed, mapinfo);
            Vector3 playerpos = m_Player.transform.position;
            m_Player.transform.position = new Vector3(0f, playerpos.y, 0f);

            WaveChange(i + 1);

            yield return new WaitForSeconds(3f);

            Wave wave = EnemyWaves[i];
            m_CurWaveEnemyNum = wave.EnemyNum;
            m_CurWaveEnemykilled = 0;
            yield return StartCoroutine(WaveAttacking(wave));
        }
    }

    IEnumerator WaveAttacking(Wave wave)
    {
        if (bFinalWave)
            wave.EnemyNum = 9999;
        //spawn enemy
        for (int i = 0; i < wave.EnemyNum; i++)
        {
            yield return new WaitForSeconds(wave.TimeBetweenSpawn / 2f);

            Transform tiletransform = m_MapGenerator.GetRandomAvailableTile();
            yield return StartCoroutine(TileFlash(tiletransform));

            AIEnemy newenemy = Instantiate(Enemy, tiletransform.position, Quaternion.identity);

            yield return new WaitForSeconds(wave.TimeBetweenSpawn / 2f);
        }

        //Wait until all enemy killed
        while(true)
        {
            if (CurWaveFinished())
            {
                if(AudioManager.Instance != null)
                    AudioManager.Instance.Play2DSound("LevelCompleted");
                break;
            }
            yield return null;
        }

    }

    IEnumerator TileFlash(Transform tile)
    {
        int Times = 2;
        float flashtime = 0.5f;
        float curtime;

        Color flashcolor = Color.red;
        Material tileMaterial = tile.GetComponent<Renderer>().material;
        Color origincolor = tileMaterial.color;

        for(int i = 0; i < Times; i++)
        {
            curtime = 0;
            while (curtime < flashtime / 2f)
            {
                tileMaterial.color = Color.Lerp(tileMaterial.color, flashcolor, 0.1f);
                curtime += Time.deltaTime;
                yield return null;
            }
            while (curtime < flashtime)
            {
                tileMaterial.color = Color.Lerp(tileMaterial.color, origincolor, 0.1f);
                curtime += Time.deltaTime;
                yield return null;
            }
        }
        tileMaterial.color = origincolor;
    }

    void EnemyKilled()
    {
        m_CurWaveEnemykilled++;
    }

    bool CurWaveFinished()
    {
        if (m_CurWaveEnemyNum == m_CurWaveEnemykilled)
            return true;
        else
            return false;
    }

    private void OnDestroy()
    {
        AIEnemy.OnKilledStatic -= EnemyKilled;
    }
}
