using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt
{
	public class Camera
	{
		Vector2 camInitPos = Vector2.Zero;
		Vector2 worldBound;
		Viewport view;
		float scale = 1f;
		int camMovSpeed = 10;
		public float Scale
		{
			get  { return scale; }
			set { scale = value; }
		}

		public Vector2 CamInitPos
		{
			get { return camInitPos; }
			set { camInitPos = value; }
		}

		public Viewport View
		{
			get { return view; }
			set { view = value; }
		}

		public Vector2 WorldBound
		{
			get { return worldBound * scale; }
			set { worldBound = value; }
		}
		public Matrix CurrentCamTranslation 
		{ get
			{
				return Matrix.CreateTranslation(new Vector3(camInitPos, 0)) * 
					   Matrix.CreateScale(new Vector3(scale, scale, 0));
			}
		}

		public Camera(Vector2 startPos, Vector2 bounds, Viewport view)
		{
			CamInitPos = startPos;
			WorldBound = bounds;
			View = view;
		}

		public void FollowCharacter(Vector2 characterPos, Viewport v)
		{
			CamInitPos = characterPos - new Vector2(v.Width / 2, v.Height / 2) / scale;
			CamInitPos = -Vector2.Clamp(CamInitPos, Vector2.Zero, WorldBound / scale 
						 - new Vector2(v.Width, v.Height) / scale);
		}
	}
}

