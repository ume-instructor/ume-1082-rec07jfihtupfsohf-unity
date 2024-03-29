using UnityEngine;
using System;
using System.Collections;

namespace UME{
    [AddComponentMenu("UME/Move/Patrol")]
    public class Patrol : MonoBehaviour {
		public Transform[] waypoints = new Transform[2];
		[Range(1,5)] public float speed = 1.0f;
		[Range(0.0f, 10.0f)] public float checkinDistance = 1.0f;
		[Range(0.1f, 5.0f)] public float delay = 2.0f;

		private int m_target_idx = 0;
		private Transform m_target;
		private float m_target_attention = 0;
		private bool m_hitTarget = false;
		private Rigidbody2D m_Rigidbody2D;
		private Animator m_Anim;

		private float m_speed = 0.1f;
		void OnDrawGizmosSelected()
		{
			if (this.enabled){
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(this.transform.position, checkinDistance);
			}
		}

		private void Start()
		{
			m_speed = speed*m_speed;
			// set initial waypoint traget
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			m_target_attention = delay;
			if (waypoints.Length > 0) {
				if (waypoints [m_target_idx] != null) {
					m_target = waypoints [m_target_idx];
				}
			}
		}

		private void OnCollisionStay2D(Collision2D other){
			m_target_attention -= Time.deltaTime;
			if (m_target_attention <= 0) {
				updateTarget ();
				m_target_attention = delay;
			}
		}

		private void OnCollisionEnter2D(Collision2D other){
			if (m_target != null && other.transform.position == m_target.position) {
				m_hitTarget = true;
			}
		}
		private void updateTarget(){
			if (waypoints.Length > 0) {
				//update target after delay time expires
				m_target_idx = (m_target_idx >= waypoints.Length - 1) ? 0 : m_target_idx + 1;
				m_target = waypoints [m_target_idx];
			}
			//reset delay time

		}

		// Update is called once per frame
		private void FixedUpdate()
		{
			m_speed = speed*.1f;
			if (m_target != null) {
				// check for arrival
				if ( Vector3.Distance (this.transform.position, m_target.position) <= checkinDistance || m_hitTarget) {
					//start counting delay time
					m_target_attention -= Time.deltaTime;
					if (m_Anim != null){
						m_Anim.SetFloat("Speed",0.0f);
					}
					if (m_Rigidbody2D != null) {
						m_Rigidbody2D.position = this.transform.position;
					}
					if (m_target_attention <= 0) {
						updateTarget ();
						m_target_attention = delay;
						m_hitTarget = false;
					}
				} else {
					Vector3 move;
					// update position
					if (m_Rigidbody2D != null){
						move = Vector3.MoveTowards (m_Rigidbody2D.position, m_target.position, m_speed);
						m_Rigidbody2D.MovePosition (move);
					}else{
						move = Vector3.MoveTowards (this.transform.position, m_target.position, m_speed);
						this.transform.position = move;
					}
					if (m_Anim != null){			
						try{
							m_Anim.SetBool("Ground", true);
							m_Anim.SetFloat("Speed", (speed+1.0f)*0.085f);
						}
						catch{
						}
					}



				}

			}
		}




	}
}
	

