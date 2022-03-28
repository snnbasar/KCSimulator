using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class Drawer_Pull_Z : MonoBehaviour
	{

		public Animator pull;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
			//Player = GameObject.FindGameObjectWithTag("Player").transform;

			gameObject.layer = 6;
			gameObject.tag = "Drawer_Pull_Z";
		}

		/*void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 10)
					{
						print("object name");
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
			pull.Play("openpull");
			SoundManager.instance.PlaySound(Soundlar.DrawerOpen);
			yield return new WaitForSeconds(.5f);
			open = true;
		}

		IEnumerator closing()
		{
			pull.Play("closepush");
			SoundManager.instance.PlaySound(Soundlar.DrawerClose);
			yield return new WaitForSeconds(.5f);
			open = false;
		}


	}
}