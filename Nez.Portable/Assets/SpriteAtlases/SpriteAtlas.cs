using System;
using Nez.Textures;

namespace Nez.Sprites
{
	public class SpriteAtlas : IDisposable
	{
		public string[] Names;
		public Sprite[] Sprites;

		public string[] AnimationNames;
		public SpriteAnimation[] SpriteAnimations;

		// public Sprite GetSprite(string name)
		// {
		// 	var index = Array.IndexOf(Names, name);
		// 	return Sprites[index];
		// }
		public Sprite GetSprite(string name)
		{
			
			for (var i = 0; i < Names.Length; i++)
			{
				// Split the name string by semicolon
				var parts = Names[i]?.Split(';');

				// Check if the split resulted in parts and the first part matches the target name
				if (parts?.Length > 0 && parts[0] == name)
				{
					// Check if the index is within the bounds of the Sprites array
					if (i < Sprites.Length)
					{
						return Sprites[i];
					}
					else
					{
						// Handle the case where the index is out of bounds
						return null;
					}
				}
			}

			// Handle the case where the name is not found
			return null;
		}




		public SpriteAnimation GetAnimation(string name)
		{
			var index = Array.IndexOf(AnimationNames, name);
			return SpriteAnimations[index];
		}

		void IDisposable.Dispose()
		{
			// all our Sprites use the same Texture so we only need to dispose one of them
			if (Sprites != null)
			{
				Sprites[0].Texture2D.Dispose();
				Sprites = null;
			}
		}
	}
}
