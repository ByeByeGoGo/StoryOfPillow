using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State
{
    Seed,       //water*1
    Baby,       //water*1, fertilizer*1
    Stronger,   //water*2, fertilizer*2
    Champion,    //love value: 80, 20
    Ultimate
}

enum direction
{
    increase,
    stop,
    decrease
}

public class GameManager : MonoBehaviour {

    public GameObject UI;
    public int DeltaLove = 1;

    public GameObject Pillow;

    private int LoveValue = 0;
    /*
        hug: +20
        over-water: -10
        over-fertilize: -10
        bully(?): -20
    */
    private State PillowState;
    private int WateringNum;
    private int FertilizingNum;

    private int MoodCounter;
    public int MoodCountMax = 100;

    private int WanderCounter;
    private int WanderCountMax;
    private direction XDirection;
    private direction YDirection;
    private float Step;
    private bool ZIncrease;
    private float RotateStep;
    private float RotateBoundary;

	// Use this for initialization
	void Start () {
        LoveValue = 0;
        PillowState = State.Seed;
        WateringNum = 0;
        FertilizingNum = 0;
        MoodCounter = -1;
        WanderCounter = 0;
        WanderCountMax = 50;
        XDirection = direction.stop;
        YDirection = direction.stop;
        ZIncrease = true;
        Step = 0.01f;
        RotateStep = 2;
        RotateBoundary = 45;

        UI.GetComponent<UIControl>().SetStateText("Seed");
    }
	
	// Update is called once per frame
	void Update () {
        //Water simulate
        if (Input.GetKeyDown(KeyCode.J))
        {
            WateringEvent();
        }
        //Fertilize simulate
        if (Input.GetKeyDown(KeyCode.K))
        {
            FertilizingEvent();
        }
        //Hug simulate
        if (Input.GetKeyDown(KeyCode.H))
        {
            HugEvent();
        }
        //End game
        if (Input.GetKeyDown(KeyCode.Q) && PillowState == State.Champion)
        {
            Ending();
        }

        //Handle Pillow Material
        if (MoodCounter == MoodCountMax && (PillowState == State.Stronger || PillowState == State.Champion))
        {
            MoodCounter = -1;
            int form = 2;
            if (PillowState == State.Stronger)
            {
                form = 0;
            }
            else if (PillowState == State.Champion)
            {
                form = 1;
            }
            Pillow.GetComponent<PillowControl>().ChangeMaterial(form, 0);
        }
        else if (MoodCounter >= 0)
        {
            MoodCounter++;
        }

        LoveValue = CheckValue(LoveValue, 0, 100);

        UI.GetComponent<UIControl>().SetLoveBar((float)LoveValue / 100);

        if (PillowState == State.Stronger || PillowState == State.Champion || PillowState == State.Ultimate)
        {
            Wandering(true);
        }
	}

    //Handling Watering
    public void WateringEvent()
    {
        Wandering(false);
        WateringNum++;
        switch (PillowState)
        {
            case State.Seed:
                CheckGrowth();
                break;
            case State.Baby:
                if (WateringNum == 1)
                {
                    CheckGrowth();
                }
                else if (WateringNum == 2)
                {
                    ChangeLoveValue(false, 10);
                    WateringNum = 1;
                }
                break;
            case State.Stronger:
                if (WateringNum == 2)
                {
                    CheckGrowth();
                }
                else if (WateringNum == 3)
                {
                    ChangeLoveValue(false, 10);
                    WateringNum = 2;
                }
                break;
            case State.Champion:
                if (WateringNum == 5)
                {
                    ChangeLoveValue(false, 10);
                    WateringNum = 4;
                }
                break;
        }
    }

    //Handling Fertilizing
    public void FertilizingEvent()
    {
        Wandering(false);
        FertilizingNum++;
        switch (PillowState)
        {
            case State.Seed:
                ChangeLoveValue(false, 10);
                break;
            case State.Baby:
                if (FertilizingNum == 1)
                {
                    CheckGrowth();
                }
                else if (FertilizingNum == 2)
                {
                    ChangeLoveValue(false, 10);
                    FertilizingNum = 1;
                }
                break;
            case State.Stronger:
                if (FertilizingNum == 2)
                {
                    CheckGrowth();
                }
                else if (FertilizingNum == 3)
                {
                    ChangeLoveValue(false, 10);
                    FertilizingNum = 2;
                }
                break;
            case State.Champion:
                if (FertilizingNum == 5)
                {
                    ChangeLoveValue(false, 10);
                    FertilizingNum = 4;
                }
                break;
        }
    }

    //Handling hugging
    public void HugEvent()
    {
        Wandering(false);
        ChangeLoveValue(true, 20);
    }

    //Check if pillow can grow into the next state
    void CheckGrowth()
    {
        switch (PillowState)
        {
            case State.Seed:
                if (WateringNum == 1)
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Baby;
                    Pillow.GetComponent<PillowControl>().BecomeBaby();
                    UI.GetComponent<UIControl>().SetStateText("Baby");
                }
                break;
            case State.Baby:
                if (WateringNum == 1 && FertilizingNum == 1)
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Stronger;
                    Pillow.GetComponent<PillowControl>().BecomeStronger();
                    UI.GetComponent<UIControl>().SetStateText("Stronger");
                }
                break;
            case State.Stronger:
                if (WateringNum == 2 && FertilizingNum == 2)
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Champion;
                    Pillow.GetComponent<PillowControl>().ChangeMaterial(1, 0);
                    UI.GetComponent<UIControl>().SetStateText("Champion");
                }
                break;
            case State.Champion:
                if (LoveValue >= 80)
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Champion;
                    Pillow.GetComponent<PillowControl>().ChangeMaterial(2, 2);
                    UI.GetComponent<UIControl>().SetStateText("Teacher");
                }
                else if (LoveValue >= 20)
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Champion;
                    Pillow.GetComponent<PillowControl>().ChangeMaterial(2, 1);
                    UI.GetComponent<UIControl>().SetStateText("Normal");
                }
                else
                {
                    WateringNum = 0;
                    FertilizingNum = 0;
                    PillowState = State.Champion;
                    Pillow.GetComponent<PillowControl>().ChangeMaterial(2, 0);
                    UI.GetComponent<UIControl>().SetStateText("Fail");
                    Pillow.transform.localScale = new Vector3(Pillow.transform.localScale.x / 2, Pillow.transform.localScale.y, Pillow.transform.localScale.z);
                }
                PillowState = State.Ultimate;
                break;
        }
    }

    void Ending()
    {
        CheckGrowth();
    }

    void ChangeLoveValue(bool isIncrease, int value)
    {
        if (!isIncrease)
        {
            value *= -1;
        }
        LoveValue += value;
        CheckValue(LoveValue, 0, 100);

        int form = 2;
        int type = 5;
        if (PillowState == State.Stronger)
        {
            form = 0;
        }
        else if (PillowState == State.Champion)
        {
            form = 1;
        }
        else
        {
            return;
        }
        if (isIncrease)
        {
            type = 2;
        }
        else
        {
            type = 3;
        }

        Pillow.GetComponent<PillowControl>().ChangeMaterial(form, type);
        MoodCounter = 0;
    }

    void Wandering(bool Go)
    {
        if (!Go)
        {
            XDirection = direction.stop;
            YDirection = direction.stop;
            WanderCounter = 0;
            return;
        }

        int rand;
        if (WanderCounter == WanderCountMax)
        {
            WanderCounter = 0;
            rand = Random.Range(0, 100);
            //random to decide to change direction or not
            if (rand <= 15)
            {
                rand = Random.Range(-50, 50);
                if (rand < 0)
                {
                    switch (XDirection)
                    {
                        case direction.increase:
                            XDirection = direction.decrease;
                            break;
                        case direction.stop:
                            XDirection = direction.increase;
                            break;
                        case direction.decrease:
                            XDirection = direction.stop;
                            break;
                    }
                }
                else
                {
                    switch (XDirection)
                    {
                        case direction.increase:
                            XDirection = direction.stop;
                            break;
                        case direction.stop:
                            XDirection = direction.decrease;
                            break;
                        case direction.decrease:
                            XDirection = direction.increase;
                            break;
                    }
                }

                rand = Random.Range(-50, 50);
                if (rand < 0)
                {
                    switch (YDirection)
                    {
                        case direction.increase:
                            YDirection = direction.decrease;
                            break;
                        case direction.stop:
                            YDirection = direction.increase;
                            break;
                        case direction.decrease:
                            YDirection = direction.stop;
                            break;
                    }
                }
                else
                {
                    switch (YDirection)
                    {
                        case direction.increase:
                            YDirection = direction.stop;
                            break;
                        case direction.stop:
                            YDirection = direction.decrease;
                            break;
                        case direction.decrease:
                            YDirection = direction.increase;
                            break;
                    }
                }
            }
            else if (rand > 80)
            {
                XDirection = direction.stop;
                YDirection = direction.stop;
            }
        }
        else
        {
            WanderCounter++;
        }

        //Go
        float xStep = 0;
        float yStep = 0;
        switch (XDirection)
        {
            case direction.increase:
                xStep = Step;
                break;
            case direction.stop:
                xStep = 0;
                break;
            case direction.decrease:
                xStep = -Step;
                break;
        }
        switch (YDirection)
        {
            case direction.increase:
                yStep = Step;
                break;
            case direction.stop:
                yStep = 0;
                break;
            case direction.decrease:
                yStep = -Step;
                break;
        }

        Pillow.transform.position += new Vector3(xStep, 0, yStep);

        //Rotate pillow while walking
        if (XDirection != direction.stop || YDirection != direction.stop)
        {
            if (PillowState == State.Stronger)
            {
                Pillow.GetComponent<PillowControl>().ChangeMaterial(0, 1);
            }
            else if (PillowState == State.Champion)
            {
                Pillow.GetComponent<PillowControl>().ChangeMaterial(1, 1);
            }

            if (ZIncrease)
            {
                if (Pillow.transform.rotation.eulerAngles.z < (360f - RotateBoundary) && Pillow.transform.rotation.eulerAngles.z > RotateBoundary)
                {
                    ZIncrease = false;
                    Pillow.transform.Rotate(0, 0, -RotateStep);
                }
                else
                {
                    Pillow.transform.Rotate(0, 0, RotateStep);
                }
            }
            else
            {
                if (Pillow.transform.rotation.eulerAngles.z < (360f - RotateBoundary) && Pillow.transform.rotation.eulerAngles.z > RotateBoundary)
                {
                    ZIncrease = true;
                    Pillow.transform.Rotate(0, 0, RotateStep);
                }
                else
                {
                    Pillow.transform.Rotate(0, 0, -RotateStep);
                }
            }
        }
        else if (MoodCounter == -1) //Not in angry or happy now
        {
            int form = 2;
            if (PillowState == State.Stronger)
            {
                form = 0;
            }
            else if (PillowState == State.Champion)
            {
                form = 1;
            }
            else
            {
                return;
            }
            Pillow.GetComponent<PillowControl>().ChangeMaterial(form, 0);
        }
    }

    int CheckValue(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }

        return value;
    }
}
