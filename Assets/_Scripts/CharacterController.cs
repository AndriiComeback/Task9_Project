using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
    private Rigidbody _rigidbody;   //кэшированный компонент Rigidbody (чтобы не создавать каждый раз, когда обращаемся)

    [SerializeField]
    private float movingForce = 20.0f;  //сила для передвижения

    [SerializeField]
    private float jumpForce = 80f;  //сила прыжка

    [SerializeField]
    private float maxSlope = 30f;   //Максимальный уклон, по которому может идти персонаж

    [SerializeField]
    private float damping = 0.3f;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private List<Transform> gunPoints;

    [SerializeField]
    private float shootPower = 1000f;


	private bool onGround = false;  //Стоит ли персонаж на подходящей поверхности (или летит/падает)

    //Инициализация объекта
    void Awake() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();  //Находим и запоминаем (кэшируем) компонент Rigidbody
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start() {
        //Здесь будет взаимодействие с другими объектами в начале игры, после того, как они уже проинициализировались в их методе Awake () 
        //Например, можно найти инвентарь, и вычесть его вес из скорости перемещения. 
    }

    //Коллайдер персонажа прекращает взаимодействие с каким-то другим коллайдером
    private void OnCollisionExit(Collision collision) {
        onGround = false;
    }

    //Коллайдер персонажа начинает взаимодействие с каким-то другим коллайдером
    private void OnCollisionStay(Collision collision) {
        onGround = CheckIsOnGround(collision);
    }

    //Вызывается каждый кадр. Частота может меняться в зависимости от сложности рендеринга и мощности компьтера.
    void Update() {
        LookAtTarget(); //Поворачиваем персонажа к курсору 
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

    //Вызывается каждый шаг просчета физики, вне зависимости от FPS (вне зависимости от скорости рендеринга)
    void FixedUpdate() {
        if (onGround)   //если стоим на земле
        {
            ApplyMovingForce(); //прикладываем к персонажу горизонтальную силу, соответствующую осям ввода (кнопкам WSAD или стрелкам)

            if (Input.GetKeyDown(KeyCode.Space))    //Если игрок нажал "пробел"
            {
                _rigidbody.AddForce(Vector3.up * jumpForce);    //прикладываем к Rigidbody силу, направленную вверх, и имеющую величину, равную jumpForce.
            }
        }
    }

    // Проверяем, подходит ли поверхность коллайдера для того, чтобы персонаж на ней стоял.
    //Объект Collision для проверки.
    //return true, если поверхность подходящая, false - если нет.
    private bool CheckIsOnGround(Collision collision) {
        for (int i = 0; i < collision.contacts.Length; i++) //Проверяем все точки соприкосновения
        {
            if (collision.contacts[i].point.y < transform.position.y)   //если точка соприкосновения находится ниже центра нашего персонажа
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)   //Если уклон поверхности не превышает допустимое значение
                {
                    return true;    //найдена точка соприкосновения с подходящей поверхностью - выходим из функции, возвращаем значение true.
                }
            }
        }
        return false;   //Подходящая поверхность не найдена, возвращаем значение false.
    }

    // Рассчитываем и прикладываем силу перемещения персонажа в зависимости от значений осей инпута
    private void ApplyMovingForce() {
        //При рассчете силы по той или иной оси используются локальные оси персонажа. Transform автоматически предоставляет вектора, соответствующие его текущей оси Z и оси X (и оси Y тоже):
        Vector3 xAxisForce = transform.right * Input.GetAxis("Horizontal"); //определяем силу по оси Х
        Vector3 zAxisForce = transform.forward * Input.GetAxis("Vertical"); //определяем силу по оси Z

        Vector3 resultXZForce = xAxisForce + zAxisForce;    //Складываем вектора

        if (resultXZForce.magnitude > 0) {
			//Если сложить два перпендикулярных вектора, каждый длиной 1, 
			//получится вектор длиной примерно 1,41... (квадратный корень из двух).
			//То есть персонаж будет быстрее бегать по диагонали, чем строго по одной из осей.
			//Чтобы этого не было, нормализуем результирующий вектор (установим его длину равной 1):
			resultXZForce.Normalize();

			resultXZForce = resultXZForce * movingForce; //умножаем результирующий вектор на силу движения персонажа (задаем скорость)

			_rigidbody.AddForce(resultXZForce); //Прикладываем силу к Rigidbody
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