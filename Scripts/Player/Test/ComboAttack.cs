using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    public float timeStamp;
    public float cooldown = 0.5f; //공격 쿨타임
    public float maxTime = 0.8f; //콤보 종료까지의 최대 시간
    public int maxCombo = 3; //콤보 최대 공격 횟수
    int combo = 0; //현재 콤보
    float lastTime; //마지막 공격 시간


    void Start()
    {
        StartCoroutine(Melee());
    }
    




    IEnumerator Melee()
    {
        //계속 반복되므로 한 번만 호출
        while (true)
        {
            //공격 여부를 확인하고 콤보 시작
            if (Input.GetButtonDown("Fire1"))
            {
                combo++;
                Debug.Log("Attack" + combo);
                lastTime = Time.time;

                //공격 중에서  maxTime 에 도달하거나 콤보의 끝에 도달하면 콤보를 종료하는 콤보 루프
                while ((Time.time - lastTime) < maxTime && combo < maxCombo)
                {
                    //쿨타임이 리셋되면 공격
                    if (Input.GetButtonDown("Fire1") && (Time.time - lastTime) > cooldown)
                    {
                        combo++;
                        Debug.Log("Attack " + combo);
                        lastTime = Time.time;
                    }
                    yield return null;
                }
                //콤보 초기화하고 남은 시간 후 다시 공격하여 콤보 다시 시작 가능ㅇ\.
                combo = 0;
                yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
            }
            yield return null;
        }
    }

    void Meleed()
    {
        if (timeStamp < Time.time && Input.GetButtonDown("Melee") && combo < maxCombo)
        {
            combo++;
            Debug.Log("HIT " + combo);
            timeStamp = Time.time + cooldown;
        }
        if ((Time.time - timeStamp) > cooldown)
            combo = 0;
    }

    
    /*
     * 
    public float cooldown = 0.5f; //공격 쿨타임
    private float maxTime = 0.8f; //콤보 종료까지의 최대 시간
    private float timeStamp; //시간 찍어줌
    private int maxCombo = 3; //콤보 최대 공격 횟수
    private int combo = 0; //현재 콤보
    private float lastTime; //마지막 공격 시간

    */

}
