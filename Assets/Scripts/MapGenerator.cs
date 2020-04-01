using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public Vector2 MapSize { get; set; }
    public float ObstaclePercent { get; set; }

    public MapInfo(Vector2 mapsize, float obstaclepercent)
    {
        MapSize = mapsize;
        ObstaclePercent = obstaclepercent;
    }
}

public class MapGenerator : MonoBehaviour
{
    public Vector2 MapSize;
    public Transform TilePrefab;
    public Transform Obstacle;
    public Transform SurroundingWall;

    public Transform[] Weapon;
    public Transform HealthPack;

    [Range(0,1)]
    public float OutlinePercent;

    [Range(0, 0.25f)]
    public float ObstaclePercent = 0.1f;
    public int seed = 666;
    [Range(2f, 5f)]
    public float ObstacleHeight = 2f;
    [Range(0.8f, 5f)]
    public float ObstacleHeightVar = 1f;

    Transform[,] m_ArrayTile;
    List<Transform> m_AvailableTile;

    Transform m_HealthPack;
    Transform m_Weapon;


    void Start()
    {
        //GenerateMap();
    }

    public void GenerateMap(int newseed, MapInfo mapinfo)
    {
        if (m_HealthPack != null)
            Destroy(m_HealthPack.gameObject);
        if (m_Weapon != null)
            Destroy(m_Weapon.gameObject);

        MapSize = mapinfo.MapSize;
        ObstaclePercent = mapinfo.ObstaclePercent;
        seed = newseed;
        //generate Map
        int mapW = (int)MapSize.y;
        int mapH = (int)MapSize.x;

        m_ArrayTile = new Transform[mapH, mapW];

        m_AvailableTile = new List<Transform>();
        m_AvailableTile.Clear();


        string holdername = "Generated Map";
        if(transform.Find(holdername))
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        GameObject newMapHolder = new GameObject(holdername);
        newMapHolder.transform.parent = transform;

        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                Vector3 tilepos = new Vector3(-MapSize.y / 2f + 0.5f + y, 0f, -MapSize.x / 2f + 0.5f + x);
                Transform newTile = Instantiate(TilePrefab, tilepos, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - OutlinePercent);
                newTile.parent = newMapHolder.transform;
                m_ArrayTile[x, y] = newTile;
            }
        }

        //generate obstacle
        bool[,] mapArray = new bool[mapH, mapW];
        mapArray.Initialize();

        System.Random prng = new System.Random(seed);
        int numObstacle = (int)(MapSize.x * MapSize.y * ObstaclePercent);

        for(int i = 0; i < numObstacle; i++)
        {
            int rndx = prng.Next(0, mapH);
            int rndy = prng.Next(0, mapW);
            mapArray[rndx, rndy] = true;
        }

        int rseed = prng.Next();
        MyUtil.Shuffle2D(mapArray, rseed, mapH, mapW);

        //deal with central part
        int midx = (int)(MapSize.x / 2f);
        int midy = (int)(MapSize.y / 2f);
        mapArray[midx, midy] = false;
        if(mapH % 2 == 0)
            mapArray[midx - 1, midy] = false;
        if (mapW % 2 == 0)
            mapArray[midx, midy-1] = false;
        if (mapW % 2 == 0 && mapH % 2 == 0)
            mapArray[midx - 1, midy - 1] = false;



        Color c = new Color((float)prng.Next(30, 200) / 255f, (float)prng.Next(30, 200) / 255f, (float)prng.Next(30, 200) / 255f);
        //Generate obstacles
        for (int x = 0; x < mapH; x++)
        {
            for (int y = 0; y < mapW; y++)
            {
                if(mapArray[x, y])
                {
                    Cord newCord = new Cord(x, y);
                    Vector3 newPos = CordToPosition(newCord, mapW, mapH);
                    Transform newOb = Instantiate(Obstacle, newPos, Quaternion.identity);
                    float ratio = (float)prng.Next((int)(0.7f * 10), (int)(ObstacleHeightVar * 10f)) / 10f;
                    float height = ObstacleHeight * ratio;
                    newOb.localScale = new Vector3(1 - OutlinePercent, height, 1 - OutlinePercent);
                    newOb.position += Vector3.up * height / 2f;
                    newOb.parent = newMapHolder.transform;
                    Material ma = newOb.GetComponent<Renderer>().material;
                    float X2C = newOb.position.x - 0f;
                    float Z2C = newOb.position.z - 0f;
                    float Xratio = X2C / MapSize.x * 2f;
                    float Zratio = Z2C / MapSize.y * 2f;

                    ma.color = new Color(c.r + (1 - c.r) * Xratio, c.g + (1 - c.g) * Zratio, c.b + (1 - c.b) * Xratio);
                }
                else
                {
                    m_AvailableTile.Add(m_ArrayTile[x, y]);
                }
            }
        }

        //Generate Weapon
        int weaponNum = prng.Next(1, 5);
        for(int i = 0; i < weaponNum; i++)
        {
            Transform tile = GetRandomAvailableTile();
            int index = Random.Range(0, Weapon.Length);
            m_Weapon = Instantiate(Weapon[index], new Vector3(tile.position.x, 1, tile.position.z), Quaternion.Euler(new Vector3(0, 90, 90)));
            m_Weapon.GetComponent<Gun>().StartAnimation();
        }


        //Generate HealthPack
        int hpNum = prng.Next(1, 5);
        for (int i = 0; i < hpNum; i++)
        {
            Transform healthtile = GetRandomAvailableTile();
            m_HealthPack = Instantiate(HealthPack, new Vector3(healthtile.position.x, 1, healthtile.position.z), Quaternion.identity);
        }

        //Generate Surrounding Walls
        float W = (float)mapW;
        float H = (float)mapH;

        Vector3 Lwallpos = new Vector3(-W / 2f - 0.5f, 0.5f, 0f);
        Transform newLwall = Instantiate(SurroundingWall, Lwallpos, Quaternion.identity);
        newLwall.localScale = new Vector3(1f, 1f, H + 1);
        newLwall.parent = newMapHolder.transform;

        Vector3 Rwallpos = new Vector3(W / 2f + 0.5f, 0.5f, 0f);
        Transform newRwall = Instantiate(SurroundingWall, Rwallpos, Quaternion.identity);
        newRwall.localScale = new Vector3(1f, 1f, H + 1);
        newRwall.parent = newMapHolder.transform;

        Vector3 Dwallpos = new Vector3(0f, 0.5f, -H / 2f - 0.5f);
        Transform newDwall = Instantiate(SurroundingWall, Dwallpos, Quaternion.identity);
        newDwall.localScale = new Vector3(W, 1f, 1f);
        newDwall.parent = newMapHolder.transform;

        Vector3 Uwallpos = new Vector3(0f, 0.5f, H / 2f + 0.5f);
        Transform newUwall = Instantiate(SurroundingWall, Uwallpos, Quaternion.identity);
        newUwall.localScale = new Vector3(W, 1f, 1f);
        newUwall.parent = newMapHolder.transform;


    }

    Vector3 CordToPosition(Cord cord, int mapWidth, int mapHeight)
    {
        float posx = -mapWidth / 2f + 0.5f + cord.y;
        float posz = -mapHeight / 2f + 0.5f + cord.x;
        Vector3 position = new Vector3(posx, 0f, posz);
        return position;
    }

    public Transform GetRandomAvailableTile()
    {
        int length = m_AvailableTile.Count;
        int randomi = UnityEngine.Random.Range(0, length);
        return m_AvailableTile[randomi];
    }

    public class Cord
    {
        public int x;
        public int y;

        public Cord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
