using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiveBall : Skill
{
   
    [SerializeField] GameObject[] balls;
    [SerializeField] float radius;
    [SerializeField] float speed;
    [SerializeField] float speedThrowingBalls;

    private float angle;
    private float currentAngle;
    private float[] timeExist;
    private float[] timeThrow;
    private Vector2[] playerPos;


    private void Awake()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            // Tăng góc theo thời gian
            currentAngle = angle + i * (360f / balls.Length);
            // Tính toán vị trí mới của đối tượng con trên quỹ đạo
            float x = base.caster.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
            float y = base.caster.position.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
            // Cập nhật vị trí của đối tượng con
            balls[i].transform.position = new Vector3(x, y, 0);
        }
        timeExist = new float[balls.Length];
        playerPos = new Vector2[balls.Length];
        timeThrow = new float[balls.Length];    
        SetTimeExist();
       

    }
    private void Update()
    {
        angle += speed * Time.deltaTime;
        for (int i = 0; i < balls.Length; ++i)
        {
            //Nếu thời gian tồn tại chưa đủ thì xoay ball
            if(timeExist[i] <= 4f)
            {
                // Tăng góc theo thời gian
                currentAngle = angle + i * (360f / balls.Length);
                // Tính toán vị trí mới của đối tượng con trên quỹ đạo
                float x = base.caster.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
                float y = base.caster.position.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
                // Cập nhật vị trí của đối tượng con
                balls[i].transform.position = new Vector3(x, y, 0);
                timeExist[i] += Time.deltaTime;
                if(caster.GetComponent<Enemy>().Player != null) playerPos[i] = -(balls[i].transform.position - caster.GetComponent<Enemy>().Player.position).normalized;
                timeThrow[i] = 0;
            }
            else
            {
                //Throw the ball
                if (i == balls.Length - 1 && IsActive == true)
                {
                    IsActive = false;
                    StartCoroutine(CooldownRoutine());
                }
                //neu thoi gian ton tai duoc 4s thi dung
                if (timeThrow[i] >= 2)
                {
                    balls[i].SetActive(false);
                } else
                {
                    //nem ve phia player
                    float xMovement = playerPos[i].x * speedThrowingBalls * Time.deltaTime;
                    float yMovement = playerPos[i].y * speedThrowingBalls * Time.deltaTime;
                    balls[i].transform.position += new Vector3(xMovement, yMovement, 0);
                    timeThrow[i] += Time.deltaTime;
                }
               
            }

        }
    }
    public override IEnumerator Activate()
    {
        gameObject.SetActive(true);
        IsActive = true;
        SetTimeExist();
        for (int i = 0; i < balls.Length; i++)
        {
            //balls[i].GetComponent<NuclearBall>().DmgDeal = -damage; 
            timeExist[i] = 0f;
            balls[i].SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }    
    }
    private void SetTimeExist()
    {
        
        for (int i = 0; i < balls.Length; i++)
        {
            timeExist[i] = float.MinValue;
        }
    }
   
}
