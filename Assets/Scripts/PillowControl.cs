using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowControl : MonoBehaviour {

    public int threshold;

    public GameObject Seed;
    public GameObject BabyForm;

    public Material StrongerIdle;
    public Material StrongerSmile;
    public Material StrongerHappy;
    public Material StrongerAngry;
    public Material StrongerSad;

    public Material ChampionIdle;
    public Material ChampionSmile;
    public Material ChampionHappy;
    public Material ChampionAngry;
    public Material ChampionSad;

    public Material Teacher;
    public Material Normal;
    public Material Fail;

    private Renderer rend;

    private GameManager gameManager;

    private int wateringCnt;
    private int fertilizeringCnt;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        wateringCnt = 0;
        fertilizeringCnt = 0;
    }

    // Use this for initialization
    void Start () {
        rend = gameObject.GetComponent<Renderer>();
        rend.material = StrongerIdle;
        rend.enabled = false;
        Seed.SetActive(true);
        Seed.transform.position = gameObject.transform.position;
        BabyForm.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BecomeBaby()
    {
        Seed.GetComponent<Renderer>().enabled = false;
        BabyForm.SetActive(true);
        BabyForm.transform.position = Seed.transform.position;
    }

    public void BecomeStronger()
    {
        BabyForm.SetActive(false);
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.transform.position = BabyForm.transform.position;
        ChangeMaterial(0, 0);
    }

    public void ChangeMaterial(int form, int type)
    {
        if (form == 0)
        {
            switch (type)
            {
                case 0:
                    rend.material = StrongerIdle;
                    break;
                case 1:
                    rend.material = StrongerSmile;
                    break;
                case 2:
                    rend.material = StrongerHappy;
                    break;
                case 3:
                    rend.material = StrongerAngry;
                    break;
                case 4:
                    rend.material = StrongerSad;
                    break;
            }
        }
        else if (form == 1)
        {
            switch (type)
            {
                case 0:
                    rend.material = ChampionIdle;
                    break;
                case 1:
                    rend.material = ChampionSmile;
                    break;
                case 2:
                    rend.material = ChampionHappy;
                    break;
                case 3:
                    rend.material = ChampionAngry;
                    break;
                case 4:
                    rend.material = ChampionSad;
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case 0:
                    rend.material = Fail;
                    break;
                case 1:
                    rend.material = Normal;
                    break;
                case 2:
                    rend.material = Teacher;
                    break;
                default:
                    rend.material = Fail;
                    break;
            }
        }
    }

    void OnParticleCollision(GameObject obj) {
        if (obj.tag == "Water") {
            wateringCnt += 1;
        } else if (obj.tag == "Fertilizer") {
            fertilizeringCnt += 1;
        }

        if (wateringCnt >= threshold) {
            wateringCnt = wateringCnt % threshold;
            gameManager.WateringEvent();
            Debug.Log("watering Hit!");
        }

        if (fertilizeringCnt > threshold) {
            fertilizeringCnt = fertilizeringCnt % threshold;
            gameManager.FertilizingEvent();
            Debug.Log("fertilizering Hit!");
        }
    }
}
