using UnityEngine;
using System;

namespace UME{
    [AddComponentMenu("UME/Move/Oscillate")]
    public class Oscillate : MonoBehaviour {
		public Vector2 direction = Vector2.right;
		[Range(0.1f, 10.0f)] public float amplitude = 10;
		[Range(0.1f, 10.0f)] public float frequency = 1;
		[Range(0.0f, 10.0f)] public float phase=1;
		private Vector3 pos;
		private Rigidbody2D m_Rigidbody2D;
		private bool active;

		void OnDrawGizmosSelected()
		{
			if (this.enabled){
				Vector3 gpos = this.transform.position;
				if (active){ gpos=pos;}
				Gizmos.color = Color.green;
				Gizmos.DrawRay(gpos, direction*amplitude);
				Gizmos.DrawRay(gpos, direction*-amplitude);
			}
		}


		void Start () {
			active = true;
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			//register initial position to oscillate around
			pos = this.transform.position;
			if (m_Rigidbody2D != null) {
				pos = m_Rigidbody2D.position;
			}

	
		}

		void FixedUpdate () {
			float oscillation = amplitude * (float)Math.Sin ( (phase+(float)Time.fixedTime) * frequency);
			Vector3 oscillation_pos = pos+(oscillation * new Vector3(direction.x,direction.y,0.0f));
			Vector3 move;
			if (m_Rigidbody2D != null){
				move = Vector3.MoveTowards (m_Rigidbody2D.position, oscillation_pos, 1.0f);
				m_Rigidbody2D.MovePosition (move);
			}else{
				move = Vector3.MoveTowards (this.transform.position, oscillation_pos,frequency);
				this.transform.position = move; 
			}
		}

	}
}
