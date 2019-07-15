using System;
using UnityEngine;

namespace UME
{
	[AddComponentMenu("UME/Move/Hunt")]
    public class Hunt : MonoBehaviour
    {
		[SerializeField] public LayerMask GroundLayer;
		[SerializeField] [Range(1,5)] public float speed = 1f; 
		[SerializeField] [Range(1,100)] private float range = 100f;
		public GameObject targetObject;
		private Animator m_Anim;            // Reference to the enemy's animator component.
		private Rigidbody2D m_Rigidbody2D;
		private Transform m_target;
		private bool m_hitTarget = false;
		private bool m_Grounded = true; 
		//private bool jumping = false;
		private Collider2D m_GroundCheck;    
		//private bool active = false;

    void OnDrawGizmosSelected()
    {
		if (this.enabled){
        	Gizmos.color = Color.red;
        	Gizmos.DrawWireSphere(this.transform.position, range);
		}
    }


		private void Start(){
			//active=true;
			Transform feet = transform.Find("Feet");
			if (feet == null){
				feet = this.transform;
			}
			m_GroundCheck = feet.GetComponent<Collider2D>();
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			if (targetObject == null) {
				targetObject=GameObject.FindWithTag("Player");
			}
			if (targetObject != null) {
				m_target = targetObject.transform;
			} 
			GroundCheck();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
			}
		}
		private void OnCollisionEnter2D(Collision2D other){
			if (other.gameObject == m_target.gameObject)
				m_hitTarget = true;
			
		}
		private void OnCollisionExit2D(Collision2D other){
			if (other.gameObject == m_target.gameObject)
				m_hitTarget = false;
			
		}
		// Update is called once per frame
        private void FixedUpdate()
		{	
			GroundCheck();
			float anim_speed = 0.0f;
			if (m_target != null && !m_hitTarget) {
				// check for arrival
				float distance = Vector3.Distance (this.transform.position, m_target.position);
				if( distance <= range) {
					HuntTarget();
				anim_speed = (speed+1.0f)*.08f;
				}
			}
			if (m_Anim != null){			
				try{
					m_Anim.SetBool("Ground", m_Grounded);
					m_Anim.SetFloat("Speed", anim_speed);
				}
				catch{
				}
			}

		}
		private void HuntTarget(){

			Vector2 m_target2d = new Vector2(m_target.position.x,m_target.position.y);
			Vector2 m_position2d = new Vector2(this.transform.position.x, this.transform.position.y);
			Vector2 force;
			// update position
			if (m_Rigidbody2D != null){
				float hunt_force = speed*m_Rigidbody2D.mass*20*speed;
				float hunt_jump = m_Rigidbody2D.mass*1500;

				force = (m_target2d - m_Rigidbody2D.position).normalized;
				force.x*=hunt_force;
				
				if(m_Grounded){
					force.y = Mathf.Max(0.0f,force.y);
					force.y*=hunt_jump;
				}else{
					force.y = 0.0f;
				}

				m_Rigidbody2D.AddForce(force);
			}else{
				force = Vector2.MoveTowards (m_position2d, m_target2d, speed);
				this.transform.position = force;
			}
		}

		private void GroundCheck(){
			if (m_GroundCheck != null) {
				m_Grounded=false;
				Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheck.bounds.max, m_GroundCheck.bounds.min, GroundLayer);
				if(colliders.Length>0)
					m_Grounded = true;
			}
		}
	}
}