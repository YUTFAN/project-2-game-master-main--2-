using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour, Interactable
{
    private STAGE_NORMAL Stage_n;
    private STAGE_MIRROR Stage_m;

    private List<List<string>> text;
    private DialogSystem dialog;

    private int count;
    private static bool reset;

    public static bool inGame;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private FinalDoctor finalDoctor;
    public enum STAGE_NORMAL
    {
        // text[0]
        FIRSTROOM,
        // text[1], text[2]
        CHAT1,
        // text[3]
        GAME1,
        // text[4]
        GAME2,
        // text[5], text[6]
        GAME3,
        // text[7], text[8]
        CHAT2,
        // text[17], text[18]
        Final
    }

    public enum STAGE_MIRROR
    {
        // text[9], text[10]
        CHAT1,
        // text[11], text[12]
        CHAT2,
        // text[13], text[14]
        CHAT3
    }

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        reset = false;
        inGame = false;
        Stage_m = STAGE_MIRROR.CHAT1;
        Stage_n = STAGE_NORMAL.FIRSTROOM;
        dialog = GameObject.Find("UI").GetComponent<DialogSystem>();
        text = DialogSystem.GetTextFromFile(textFile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (GameStatus.GameStage == GameStatus.STAGE.Final) final();
        else if (GameStatus.Mirror) interact_m();
        else interact_n();
    }

    private void final()
    {
        if (count < 2)
        {
            if (!reset)
            {
                dialog.SendMesasge(text[17], gameObject);
                reset = true;
                count++;
            }
            else dialog.SendMesasge(text[19]);
        }
        else
        {
            if (!reset)
            {
                reset = true;
                dialog.SendMesasge(text[18], gameObject);
            }
            else dialog.SendMesasge(text[19]);
        }
    }

    public static void FinalReset()
    {
        reset = false;
    }

    public void Pickup()
    {
        if (GameStatus.GameStage == GameStatus.STAGE.Final) finalDoctor.CallDoctorback();
    }

    private void interact_n()
    {
        if (inGame)
        {
            dialog.SendMesasge(text[15]);
        }
        else if (Doctor.Status != Doctor.STATUS.FIRSTROOMOVER)
        {
            dialog.SendMesasge(text[0]);
        }
        else if (GameStatus.GameStage == GameStatus.STAGE.FirstRoom)
        {
            dialog.SendMesasge(text[1]);
            Stage_n = STAGE_NORMAL.CHAT1;
            GameStatus.GameStage = GameStatus.STAGE.SecondRoom;
        }
        else if (Stage_n == STAGE_NORMAL.CHAT1 && Stage_m == STAGE_MIRROR.CHAT1)
        {
            dialog.SendMesasge(text[2]);
        }
        else if (Stage_n == STAGE_NORMAL.CHAT1 && Stage_m == STAGE_MIRROR.CHAT2)
        {
            dialog.SendMesasge(text[3]);
            Stage_n = STAGE_NORMAL.GAME1;
            inGame = true;
        }
        else if (Stage_n == STAGE_NORMAL.GAME1)
        {
            dialog.SendMesasge(text[4]);
            Stage_n = STAGE_NORMAL.GAME2;
            inGame = true;
        }
        else if (Stage_n == STAGE_NORMAL.GAME2)
        {
            dialog.SendMesasge(text[5]);
            Stage_n = STAGE_NORMAL.GAME3;
            inGame = true;
        }
        else if (Stage_n == STAGE_NORMAL.GAME3 && Stage_m != STAGE_MIRROR.CHAT3)
        {
            dialog.SendMesasge(text[6]);
        }
        else if (Stage_n == STAGE_NORMAL.GAME3 && Stage_m == STAGE_MIRROR.CHAT3)
        {
            dialog.SendMesasge(text[7]);
            Stage_n = STAGE_NORMAL.CHAT2;
            GameStatus.GameStage = GameStatus.STAGE.ThirdRoom;
            Portal.OpenPortalThirdRoom();
            GameObject.Find("Wall_ThirdRoom").SetActive(false);
        }
        if (Stage_n == STAGE_NORMAL.CHAT2)
        {
            dialog.SendMesasge(text[8]);
        }
    }

    private void interact_m()
    {
        if (Stage_m == STAGE_MIRROR.CHAT1)
        {
            dialog.SendMesasge(text[9], null, true, 0);
            Stage_m = STAGE_MIRROR.CHAT2;
        }
        else if (Stage_m == STAGE_MIRROR.CHAT2 && Stage_n < STAGE_NORMAL.GAME1)
        {
            dialog.SendMesasge(text[10], null, true, 0);
        }
        else if (Stage_m == STAGE_MIRROR.CHAT2 && Stage_n >= STAGE_NORMAL.GAME1 && Stage_n < STAGE_NORMAL.GAME3)
        {
            dialog.SendMesasge(text[11], null, true, 0);
        }
        else if (Stage_m == STAGE_MIRROR.CHAT2 && Stage_n == STAGE_NORMAL.GAME3)
        {
            dialog.SendMesasge(text[12], null, true, 0);
            InventorySystem.AddItem(GameObject.Find("FlyPoker").GetComponent<FlyPoker>(), GameObject.Find("FlyPoker").GetComponent<FlyPoker>().image);
            Stage_m = STAGE_MIRROR.CHAT3;
        }
        else if (Stage_m == STAGE_MIRROR.CHAT3 && Stage_n == STAGE_NORMAL.GAME3)
        {
            dialog.SendMesasge(text[13], null, true, 0);
        }
        else if (Stage_m == STAGE_MIRROR.CHAT3 && Stage_n == STAGE_NORMAL.CHAT2)
        {
            dialog.SendMesasge(text[14], null, true, 0);
        }
    }

    public void EndPokerGame()
    {
        dialog.SendMesasge(text[16]);
        inGame = false;
    }

}
