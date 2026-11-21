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
            mTrans.position += mTrans.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mTrans.position += mTrans.forward * speed * -1 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mTrans.position += mTrans.right * Time.deltaTime * speed * -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            mTrans.position += mTrans.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mTrans.Rotate(Vector3.up, -90 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mTrans.Rotate(Vector3.up, 90 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            mCam.Rotate(Vector3.left, 90 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            mCam.Rotate(Vector3.left, -90 * Time.deltaTime);
        }
    }
}
