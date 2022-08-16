using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
    private Rigidbody _rigidbody;   //������������ ��������� Rigidbody (����� �� ��������� ������ ���, ����� ����������)

    [SerializeField]
    private float movingForce = 20.0f;  //���� ��� ������������

    [SerializeField]
    private float jumpForce = 80f;  //���� ������

    [SerializeField]
    private float maxSlope = 30f;   //������������ �����, �� �������� ����� ���� ��������

    [SerializeField]
    private float damping = 0.3f;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private List<Transform> gunPoints;

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
        LookAtTarget(); //������������ ��������� � ������� 
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
	}

    //���������� ������ ��� �������� ������, ��� ����������� �� FPS (��� ����������� �� �������� ����������)
    void FixedUpdate() {
        if (onGround)   //���� ����� �� �����
        {
            ApplyMovingForce(); //������������ � ��������� �������������� ����, ��������������� ���� ����� (������� WSAD ��� ��������)

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

    // ������������ � ������������ ���� ����������� ��������� � ����������� �� �������� ���� ������
    private void ApplyMovingForce() {
        //��� �������� ���� �� ��� ��� ���� ��� ������������ ��������� ��� ���������. Transform ������������� ������������� �������, ��������������� ��� ������� ��� Z � ��� X (� ��� Y ����):
        Vector3 xAxisForce = transform.right * Input.GetAxis("Horizontal"); //���������� ���� �� ��� �
        Vector3 zAxisForce = transform.forward * Input.GetAxis("Vertical"); //���������� ���� �� ��� Z

        Vector3 resultXZForce = xAxisForce + zAxisForce;    //���������� �������

        if (resultXZForce.magnitude > 0) {
			//���� ������� ��� ���������������� �������, ������ ������ 1, 
			//��������� ������ ������ �������� 1,41... (���������� ������ �� ����).
			//�� ���� �������� ����� ������� ������ �� ���������, ��� ������ �� ����� �� ����.
			//����� ����� �� ����, ����������� �������������� ������ (��������� ��� ����� ������ 1):
			resultXZForce.Normalize();

			resultXZForce = resultXZForce * movingForce; //�������� �������������� ������ �� ���� �������� ��������� (������ ��������)

			_rigidbody.AddForce(resultXZForce); //������������ ���� � Rigidbody
		} else {
			Vector3 dampedVelocity = _rigidbody.velocity * damping;
			dampedVelocity.y = _rigidbody.velocity.y;
			_rigidbody.velocity = dampedVelocity;
		}
	}

    /*private void LookAtTarget() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
        {
			Debug.DrawLine(transform.position, hit.point);
			Vector3 position = ray.GetPoint(hit.distance);
            position.y = transform.position.y;
			transform.LookAt(position);
			//transform.LookAt(new Vector3(2.6f,0.3f,2.7f));
		}
    }*/
    private void LookAtTarget() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

		if (plane.Raycast(ray, out distance)) {
			Vector3 position = ray.GetPoint(distance);
			transform.LookAt(position);
		}
	}

}