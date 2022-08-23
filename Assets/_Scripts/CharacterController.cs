using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
    private Rigidbody _rigidbody;   //������������ ��������� Rigidbody (����� �� ��������� ������ ���, ����� ����������)

    [SerializeField]
    private float jumpForce = 80f;  //���� ������

    [SerializeField]
    private float maxSlope = 30f;   //������������ �����, �� �������� ����� ���� ��������

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private GameObject rocketPrefab;

	[SerializeField]
	private List<Transform> gunPoints;

	[SerializeField]
	private Transform rocketPoint;

	[SerializeField]
    private float shootPower = 1000f;

	private bool onGround = false;  //����� �� �������� �� ���������� ����������� (��� �����/������)

    //������������� �������
    void Awake() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();  //������� � ���������� (��������) ��������� Rigidbody
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start() {
        //����� ����� �������������� � ������� ��������� � ������ ����, ����� ����, ��� ��� ��� ��������������������� � �� ������ Awake () 
        //��������, ����� ����� ���������, � ������� ��� ��� �� �������� �����������. 
    }

    //��������� ��������� ���������� �������������� � �����-�� ������ �����������
    private void OnCollisionExit(Collision collision) {
        onGround = false;
    }

    //��������� ��������� �������� �������������� � �����-�� ������ �����������
    private void OnCollisionStay(Collision collision) {
        onGround = CheckIsOnGround(collision);
    }

    //���������� ������ ����. ������� ����� �������� � ����������� �� ��������� ���������� � �������� ���������.
    void Update() {
        Shoot();
    }

    private void Shoot() {
		if (Input.GetButtonDown("Fire1")) {
			foreach (Transform gunPoint in gunPoints) {
				GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
				newBullet.GetComponent<Rigidbody>().AddForce(gunPoint.forward * shootPower);
				Destroy(newBullet, 5);
			}
		}
		if (Input.GetButtonDown("Fire2")) {
			GameObject newRocket = Instantiate(rocketPrefab, rocketPoint.position, rocketPoint.rotation) as GameObject;
			newRocket.GetComponentInChildren<Rigidbody>().velocity = rocketPoint.forward * 8;
			Destroy(newRocket, 5);
		}
	}

    //���������� ������ ��� �������� ������, ��� ����������� �� FPS (��� ����������� �� �������� ����������)
    void FixedUpdate() {
        if (onGround)   //���� ����� �� �����
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))    //���� ����� ����� "������"
            {
                _rigidbody.AddForce(Vector3.up * jumpForce);    //������������ � Rigidbody ����, ������������ �����, � ������� ��������, ������ jumpForce.
            }
        }
    }

    // ���������, �������� �� ����������� ���������� ��� ����, ����� �������� �� ��� �����.
    //������ Collision ��� ��������.
    //return true, ���� ����������� ����������, false - ���� ���.
    private bool CheckIsOnGround(Collision collision) {
        for (int i = 0; i < collision.contacts.Length; i++) //��������� ��� ����� ���������������
        {
            if (collision.contacts[i].point.y < transform.position.y)   //���� ����� ��������������� ��������� ���� ������ ������ ���������
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)   //���� ����� ����������� �� ��������� ���������� ��������
                {
                    return true;    //������� ����� ��������������� � ���������� ������������ - ������� �� �������, ���������� �������� true.
                }
            }
        }
        return false;   //���������� ����������� �� �������, ���������� �������� false.
    }

    private void Move() {
        
    }

    private void OnDestroy() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}