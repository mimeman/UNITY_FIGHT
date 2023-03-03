using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    public float timeStamp;
    public float cooldown = 0.5f; //���� ��Ÿ��
    public float maxTime = 0.8f; //�޺� ��������� �ִ� �ð�
    public int maxCombo = 3; //�޺� �ִ� ���� Ƚ��
    int combo = 0; //���� �޺�
    float lastTime; //������ ���� �ð�


    void Start()
    {
        StartCoroutine(Melee());
    }
    




    IEnumerator Melee()
    {
        //��� �ݺ��ǹǷ� �� ���� ȣ��
        while (true)
        {
            //���� ���θ� Ȯ���ϰ� �޺� ����
            if (Input.GetButtonDown("Fire1"))
            {
                combo++;
                Debug.Log("Attack" + combo);
                lastTime = Time.time;

                //���� �߿���  maxTime �� �����ϰų� �޺��� ���� �����ϸ� �޺��� �����ϴ� �޺� ����
                while ((Time.time - lastTime) < maxTime && combo < maxCombo)
                {
                    //��Ÿ���� ���µǸ� ����
                    if (Input.GetButtonDown("Fire1") && (Time.time - lastTime) > cooldown)
                    {
                        combo++;
                        Debug.Log("Attack " + combo);
                        lastTime = Time.time;
                    }
                    yield return null;
                }
                //�޺� �ʱ�ȭ�ϰ� ���� �ð� �� �ٽ� �����Ͽ� �޺� �ٽ� ���� ���ɤ�\.
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
    public float cooldown = 0.5f; //���� ��Ÿ��
    private float maxTime = 0.8f; //�޺� ��������� �ִ� �ð�
    private float timeStamp; //�ð� �����
    private int maxCombo = 3; //�޺� �ִ� ���� Ƚ��
    private int combo = 0; //���� �޺�
    private float lastTime; //������ ���� �ð�

    */

}
