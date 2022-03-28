using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor : MonoBehaviour
	{

		public Animator openandclose;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
			//Player = GameObject.FindGameObjectWithTag("Player").transform;
			gameObject.layer = 6;
			gameObject.tag = "opencloseDoor";
		}

		/*void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 15)
					{
						if (open == false)
						{
							if (Input.GetMouseButtonDown(0))
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (Input.GetMouseButtonDown(0))
								{
									StartCoroutine(closing());
								}
							}

						}

					}
				}

			}

		}*/

		public void DoAction()
        {
			if (open == false)
			{
				StopAllCoroutines();
				StartCoroutine(opening());
			}
			if (open == true)
			{
				StopAllCoroutines();
				StartCoroutine(closing());
			}
		}


		IEnumerator opening()
		{
			openandclose.Play("Opening");
			SoundManager.instance.PlaySound(Soundlar.CabinetOpen);
			yield return new WaitForSeconds(.5f);
			open = true;
		}

		IEnumerator closing()
		{
			openandclose.Play("Closing");
			SoundManager.instance.PlaySound(Soundlar.CabinetClose);
			yield return new WaitForSeconds(.5f);
			open = false;
		}


	}
}