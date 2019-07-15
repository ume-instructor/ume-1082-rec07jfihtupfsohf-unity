using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
namespace UME
{
    [AddComponentMenu("UME/Control/KeyForce")]
	[RequireComponent(typeof(Rigidbody2D))]
	[Serializable]
	public class KeyForce : BaseKey{
		public Vector2 force = Vector2.zero;
		public float maxVelocity = 10f ;
		private Rigidbody2D m_rigidbody;
		private Animator m_Anim;          
		private Vector2 applyForce;
		//private Vector2 speed = Vector2.zero;
		public override void Initialize () {
			m_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
			m_Anim = GetComponent<Animator>();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", true);
				m_Anim.SetFloat ("Speed", 0.0f);
			}
		}

		// void Update () {
		// 	GetKey();
		// }
		void FixedUpdate() {
			GetKey();
			if (m_Anim) {
				m_Anim.SetFloat ("Speed",  (float)(Mathf.Abs(m_rigidbody.velocity.x)));
				m_Anim.SetFloat ("vSpeed",  (float)(Mathf.Abs(m_rigidbody.velocity.y)));
				}
		}
		public override void Activate(){
			if (m_rigidbody){
				m_rigidbody.AddForce (force);
				m_rigidbody.velocity = new Vector2(Mathf.Clamp(m_rigidbody.velocity.x,-maxVelocity,maxVelocity),Mathf.Clamp(m_rigidbody.velocity.y,-maxVelocity,maxVelocity));

			}
		}
	}
} 