using TMPro;
using UnityEngine;

public class UiSettings : MonoBehaviour
{
    public TextMeshProUGUI cointext;
    public GameObject firstkeyslot;
    public KeySlot[] keyslot;
    public GameObject keyslotobject;
    public GameObject keyslotparent;
    public int currentkeyslot = 0;


    public SlotScript[] slotScripts;
    public int keyAmount;

    void Start()
    {
        AddCoin(0);
        SetupSlots();
    }

    private void SetupSlots()
    {
        slotScripts = FindObjectsByType<SlotScript>(FindObjectsSortMode.None);
        for (int i = 0; i < slotScripts.Length; i++)
        {
            keyAmount = keyAmount + slotScripts[i].sleutel_collectibles.Count;
        }
        keyslot = new KeySlot[keyAmount];
        float dist = 15;
        for (int i = 0; i < keyAmount; i++)
        {
            GameObject slotobject = Instantiate(keyslotobject, Vector2.zero, Quaternion.identity, keyslotparent.transform);
            RectTransform recttransform = slotobject.GetComponent<RectTransform>();
            recttransform.anchoredPosition = new Vector2(dist, 0);
            slotobject.transform.localEulerAngles = Vector3.zero;
            dist = dist + 25;
            keyslot[i] = slotobject.GetComponent<KeySlot>();
        }
    }


    public void AddKey()
    {
        Debug.Log("vertel de stomme keyslot dat hij iets moet gaan doen");
        keyslot[currentkeyslot].FilledIn();
        currentkeyslot++;
    }

    public void AddCoin(int count)
    {
        cointext.text = "Coins: " + count;
    }
}
