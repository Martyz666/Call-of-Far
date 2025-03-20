using UnityEngine;

public class scr_Equipp : MonoBehaviour
{

    public GameObject Slot1;
    public GameObject Slot2;

    #region - Update -

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Equip1();
        }

        if (Input.GetKeyDown("2"))
        {
            Equip2();
        }

    }

    #endregion

    #region - Equip -

    void Equip1()
    {
        Slot1.SetActive(true);
        Slot2.SetActive(false);

    }

    void Equip2()
    {
        Slot1.SetActive(false);
        Slot2.SetActive(true);

    }

    #endregion

}