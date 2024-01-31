using System.Collections;
using UnityEngine;

public class propLıghtController : MonoBehaviour
{
	Rigidbody rb;
	private bool _isEntered;
	private bool _ray;
	private bool _isPropMoney;

	[SerializeField] private float _carSpeed;
	private Quaternion qua;
	private Vector3 pos;
	private BoxCollider _box;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void Start()
	{
		qua = transform.rotation;
		pos = transform.position;

		foreach(BoxCollider item in gameObject.GetComponents<BoxCollider>())
		{
			if(item.isTrigger == false)
			{
				_box = item;
				break;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			StartCoroutine(changePropLayer("Prop"));
			if (_box != null)
			{
				_box.isTrigger = false;
			}
			_isEntered = false;
			_isPropMoney = false;
		}


	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_isEntered)
		{
			StopCoroutine(checkPlayerIsEntered());
			StartCoroutine(ForceMethod(other, _carSpeed));
			makePropMoney();
			_isEntered = true;
			_isPropMoney = true;
		}

	}
	private void makePropMoney()
	{
		if (_isPropMoney) return;
		moneyManager.Instance.PropMoneyMaker();
	}
	private void isPlayerInPosition()
	{
		if (_box!= null)
		{
			_box.isTrigger = true;
		}
	}
	private IEnumerator checkPlayerIsEntered()
	{
		yield return new WaitForSeconds(6f);

		if (!_isEntered)
			yield break;

		if (_box != null)
		{
			_box.isTrigger = false;
			_ray = false;
		}

		yield break;
	}

	public void preparePropToBeReady()
	{
		bool _temp = GameManager.instance.getShiftMethod();
		_carSpeed = GameManager.instance.AbsoulteCarSpeed;
		if (_carSpeed > 30 && _temp && !_ray)
		{
			isPlayerInPosition();
			_ray = true;
			return;
		}
	}
	private IEnumerator ForceMethod(Collider c, float cs)
	{
		playDustAnim();
		pushObject(c, cs);
		StartCoroutine(changePropLayer("afterCollision"));
		yield return new WaitForSeconds(3f);
		resetPushedObject(pos, qua);
		yield break;

	}
	private IEnumerator changePropLayer(string text)
	{
		yield return new WaitForSeconds(1f);
		this.gameObject.layer = LayerMask.NameToLayer(text);
	}

	private void pushObject(Collider _c, float speed)
	{

		Vector3 collisionPoint = _c.ClosestPointOnBounds(transform.position);
		Vector3 forceDirection = (collisionPoint - transform.position).normalized;

		float forceMagnitude = 10f;
		rb.isKinematic = false;
		rb.AddForce(-forceDirection * forceMagnitude, ForceMode.Impulse);
		rb.AddTorque(-forceDirection * speed);
	}
	private void playDustAnim()
	{
		GameObject particle = ParticleManager.Instance.GetPooledParticle(transform.position, transform.rotation);

		if (particle != null)
		{
			particle.SetActive(true);
			StartCoroutine(DeactivateAndReturnToPool(particle));
		}
	}
	private IEnumerator DeactivateAndReturnToPool(GameObject particle)
	{
		yield return new WaitForSeconds(1f);

		particle.SetActive(false);
		ParticleManager.Instance.ReturnToPool(particle);
	}
	private bool IsCarInPosition()
	{
		float _d = 5f;
		float distance = Vector3.Distance(transform.position, GameManager.instance._car.transform.position);
		Debug.Log(distance);
		return distance < _d;
	}
	private IEnumerator DelayedTriggerActivation(float delay)
	{
		yield return new WaitForSeconds(delay);

		gameObject.layer = LayerMask.NameToLayer("Prop");
	}
	private void resetPushedObject(Vector3 _pos, Quaternion _q)
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		transform.position = _pos;
		transform.rotation = _q;

		rb.isKinematic = true;

		if (IsCarInPosition())
		{
			_box.isTrigger = true;
			_isEntered = true;
			return;
		}



	}
}





