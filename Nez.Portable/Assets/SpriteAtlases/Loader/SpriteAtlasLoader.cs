using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Nez.Sprites
{
	public class SpriteInfo
	{
		public string Name { get; set; }
		public Rectangle SourceRect { get; set; }
		public Vector2 Origin { get; set; }
	}

	public static class SpriteAtlasLoader
	{
		/// <summary>
		/// parses a .atlas file and loads up a SpriteAtlas with it's associated Texture
		/// </summary>
		public static SpriteAtlas ParseSpriteAtlas(string dataFile, bool premultiplyAlpha = false)
		{
			var spriteAtlas = ParseSpriteAtlasData(dataFile);
			using (var stream = TitleContainer.OpenStream(dataFile.Replace(".txt", ".png")))
				return spriteAtlas.AsSpriteAtlas(premultiplyAlpha
					? TextureUtils.TextureFromStreamPreMultiplied(stream)
					: Texture2D.FromStream(Core.GraphicsDevice, stream));
		}

		/// <summary>
		/// parses a .atlas file into a temporary SpriteAtlasData class. If leaveOriginsRelative is true, origins will be left as 0 - 1 range instead
		/// of multiplying them by the width/height.
		/// </summary>
		private static string[] ReadDataFile(string dataFile) 
		{
			return File.ReadAllLines(dataFile);
		}
		internal static SpriteAtlasData ParseSpriteAtlasData(string dataFile, bool leaveOriginsRelative = false)
		{
		    var spriteAtlas = new SpriteAtlasData();
		    var semicolonSplitter = new char[] { ';' };

		    try
		    {
		        var dataFileLines = File.ReadAllLines(dataFile);
		        var currentIndex = 0;
		        var i = 0;
		        // Parse sprites
		        while (currentIndex < dataFileLines.Length && !string.IsNullOrWhiteSpace(dataFileLines[currentIndex]))
		        {
			        i += 1;
		            var line = dataFileLines[currentIndex];
		            spriteAtlas.Names.Add(line);

		            // sprite properties
		            line = dataFileLines[currentIndex++];
		            var lineParts = line.Split(semicolonSplitter, StringSplitOptions.RemoveEmptyEntries);

		            if (lineParts.Length != 10)
		            {
		                throw new FormatException("Invalid format for sprite properties.");
		            }

		            var isRotated = int.Parse(lineParts[1]) == 1;
		            var sourceRectangle = new Rectangle(
		                int.Parse(lineParts[2]),
		                int.Parse(lineParts[3]),
		                int.Parse(lineParts[4]),
		                int.Parse(lineParts[5]));
		            var size = new Vector2(
		                int.Parse(lineParts[6]),
		                int.Parse(lineParts[7]));
		            var pivotPoint = new Vector2(
		                float.Parse(lineParts[8], CultureInfo.InvariantCulture),
		                float.Parse(lineParts[9], CultureInfo.InvariantCulture));

		            spriteAtlas.SourceRects.Add(sourceRectangle);

		            // Compute origin based on pivotPoint and size
		            //var origin = pivotPoint * size / new Vector2(sourceRectangle.Width, sourceRectangle.Height);

		            // if (leaveOriginsRelative)
		            //     spriteAtlas.Origins.Add(origin);
		            // else
		            //     spriteAtlas.Origins.Add(origin * new Vector2(sourceRectangle.Width, sourceRectangle.Height));
		            //System.Console.WriteLine(pivotPoint);
		            //spriteAtlas.Origins.Add(new Vector2(25f,25f));
		            
		            var origin = new Vector2(pivotPoint.X, pivotPoint.Y);

		            if (leaveOriginsRelative)
			            spriteAtlas.Origins.Add(origin);
		            else
			            spriteAtlas.Origins.Add(origin * new Vector2(sourceRectangle.Width, sourceRectangle.Height));
		        }

		        // Rest of the parsing logic for animations...
				System.Console.WriteLine(i);
		        return spriteAtlas;
		    }
		    catch (Exception ex)
		    {
		        // Handle the exception or log it
		        System.Console.WriteLine($"Error parsing sprite atlas data: {ex.Message}");
		        return null;
		    }
		}


			/*
			internal static SpriteAtlasData ParseSpriteAtlasData(string dataFile, bool leaveOriginsRelative = false)
			{
				var spriteAtlas = new SpriteAtlasData();

				var parsingSprites = true;
				var commaSplitter = new char[] { ',' };

				string line = null;
				using (var streamFile = File.OpenRead(dataFile))
				{
					using (var stream = new StreamReader(streamFile))
					{
						while ((line = stream.ReadLine()) != null)
						{
							// once we hit an empty line we are done parsing sprites so we move on to parsing animations
							if (parsingSprites && string.IsNullOrWhiteSpace(line))
							{
								parsingSprites = false;
								continue;
							}

							if (parsingSprites)
							{
								spriteAtlas.Names.Add(line);

								// source rect
								line = stream.ReadLine();
								var lineParts = line.Split(commaSplitter, StringSplitOptions.RemoveEmptyEntries);
								var rect = new Rectangle(int.Parse(lineParts[0]), int.Parse(lineParts[1]), int.Parse(lineParts[2]), int.Parse(lineParts[3]));
								spriteAtlas.SourceRects.Add(rect);

								// origin
								line = stream.ReadLine();
								lineParts = line.Split(commaSplitter, StringSplitOptions.RemoveEmptyEntries);
								var origin = new Vector2(float.Parse(lineParts[0], System.Globalization.CultureInfo.InvariantCulture), float.Parse(lineParts[1], System.Globalization.CultureInfo.InvariantCulture));

								if (leaveOriginsRelative)
									spriteAtlas.Origins.Add(origin);
								else
									spriteAtlas.Origins.Add(origin * new Vector2(rect.Width, rect.Height));
							}
							else
							{
								// catch the case of a newline at the end of the file
								if (string.IsNullOrWhiteSpace(line))
									break;

								spriteAtlas.AnimationNames.Add(line);

								// animation fps
								line = stream.ReadLine();
								spriteAtlas.AnimationFps.Add(int.Parse(line));

								// animation frames
								line = stream.ReadLine();
								var frames = new List<int>();
								spriteAtlas.AnimationFrames.Add(frames);
								var lineParts = line.Split(commaSplitter, StringSplitOptions.RemoveEmptyEntries);

								foreach (var part in lineParts)
									frames.Add(int.Parse(part));
							}
						}
					}
				}
				return spriteAtlas;
			} */
		}
	}

