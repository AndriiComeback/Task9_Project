using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
    private Rigidbody _rigidbody;   //кэшированный компонент Rigidbody (чтобы не создавать каждый раз, когда обращаемся)

    [SerializeField]
    private float jumpForce = 80f;  //сила прыжка

    [SerializeField]
    private float maxSlope = 30f;   //Максимальный уклон, по которому может идти персонаж

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

    //Вызывается каждый шаг просчета физики, вне зависимости от FPS (вне зависимости от скорости рендеринга)
    void FixedUpdate() {
        if (onGround)   //если стоим на земле
        {
            Move();

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

    private void Move() {
        
    }

    private void OnDestroy() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}