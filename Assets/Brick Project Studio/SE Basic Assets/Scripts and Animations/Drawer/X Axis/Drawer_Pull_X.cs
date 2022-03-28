using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{

	public class Drawer_Pull_X : MonoBehaviour
	{

		public Animator pull_01;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
			//Player = GameObject.FindGameObjectWithTag("Player").transform;
			gameObject.layer = 6;
			gameObject.tag = "Drawer_Pull_X";
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
			pull_01.Play("openpull_01");
			SoundManager.instance.PlaySound(Soundlar.DrawerOpen);
			yield return new WaitForSeconds(.5f);
			open = true;
		}

		IEnumerator closing()
		{
			pull_01.Play("closepush_01");
			SoundManager.instance.PlaySound(Soundlar.DrawerClose);
			yield return new WaitForSeconds(.5f);
			open = false;
		}


	}
}