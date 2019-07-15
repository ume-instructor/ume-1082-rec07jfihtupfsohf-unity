using UnityEngine;
using System.Collections;

public enum Directions
{
	Up,
	Right,
	Down,
	Left,
}


[AddComponentMenu("UME/Move/Wander")]
[RequireComponent(typeof(Rigidbody2D))]
public class Wander : MonoBehaviour
{
	[SerializeField] 
	[Range(1.0f, 25.0f)] public float range = 2f;
	[SerializeField] 
	[Range(1.0f, 10.0f)] public float speed = 1f;
	[SerializeField] 
	[Range(0.25f, 5.0f)] public float delay = .5f;


	private Vector2 direction;
	private Vector3 startingPoint;
	private bool active = false;
	private Rigidbody2D m_Rigidbody2D;
	// draw a gizmo while editing
    void OnDrawGizmosSelected()
    {
		if (this.enabled){
			Vector3 pos = this.transform.position;
			if (active)
				pos = startingPoint;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, range);
		}
    }

	private void Start()
	{
		active = true;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
        range = range >= 0.1f ? range : 0.1f;
        startingPoint = this.transform.position;

		StartCoroutine(ChangeDirection());
	}

	private void FixedUpdate()

	{		
		float push = 1;
		float distance = Vector2.Distance(startingPoint, this.transform.position);
		if(distance > range){
				direction = (startingPoint - this.transform.position).normalized;
				push=Mathf.Max(1,distance-range)*2;
			}
		Vector3 move = Vector3.MoveTowards (m_Rigidbody2D.position, direction, speed*push);
		if (m_Rigidbody2D != null){
			m_Rigidbody2D.AddForce (direction*speed*push);
		}else{
			this.transform.position = move; 
		}
		Vector2 vel = m_Rigidbody2D.velocity;
		vel.x = Mathf.Min(vel.x,speed);
		vel.y = Mathf.Min(vel.y,speed);
		m_Rigidbody2D.velocity = vel;
	}


	private IEnumerator ChangeDirection()
	{
		while(true)
		{
			direction = Random.insideUnitCircle; 
			yield return new WaitForSeconds(delay);
		}
	}



}