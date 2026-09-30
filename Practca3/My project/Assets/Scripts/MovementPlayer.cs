using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    Transform mTrans;
    Transform mCam;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mTrans = GetComponent<Transform>();
        mCam = mTrans.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mCam.position += Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mCam.position += Vector3.forward * speed * -1 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mCam.position += Vector3.right * Time.deltaTime * speed * -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            mCam.position += Vector3.left * speed * Time.deltaTime * -1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mCam.Rotate(Vector3.up, -90 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mCam.Rotate(Vector3.up, 90 * Time.deltaTime);
        }
    }
}
