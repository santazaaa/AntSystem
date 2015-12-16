using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 0.1F;
	public float sensitivityY = 0.1F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	public float maxrotationX; 
	public float maxrotationY;

	public float minrotationX; 
	public float minrotationY;

	float z = 351.94f;
	float rotationY = -28.020f;

	// Use this for initialization
	void Start () {
		maxrotationX = transform.localEulerAngles.y  + 3.2f;
		minrotationX = maxrotationX - 6.4f;

		maxrotationY = rotationY + 3.2f;
		minrotationY = rotationY - 3.2f;


	}
	
	// Update is called once per frame
	void Update () {

		if (axes == RotationAxes.MouseXAndY)
		{
			if (transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX < maxrotationX &&
				transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX > minrotationX)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;

				if (rotationY + Input.GetAxis ("Mouse Y") * sensitivityY < maxrotationY &&
				   rotationY + Input.GetAxis ("Mouse Y") * sensitivityY > minrotationY) {
					rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				}
				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, z);
			}
		}
		else if (axes == RotationAxes.MouseX)
		{
			if (transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX < maxrotationX &&
				transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX > minrotationX)
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, z);
		}
		else
		{
			if (rotationY + Input.GetAxis ("Mouse Y") * sensitivityY < maxrotationY &&
			    rotationY + Input.GetAxis ("Mouse Y") * sensitivityY > minrotationY) {
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			

				transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, z);
			}
		}
	}
}
