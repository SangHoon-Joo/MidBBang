using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;

namespace Krafton.SP2.X1.Lib
{
	public class AtlasSpriteLabelingTool
	{
		[MenuItem("Tools/AtlasSprite Labeling", false, 50)]
		public static void AtlasSpriteLabeling()
		{
			foreach(string assetPath in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(guid => AssetDatabase.GUIDToAssetPath(guid)))
			{
				SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
				SerializedProperty packedSprites = new SerializedObject(spriteAtlas).FindProperty("m_PackedSprites");
				Sprite[] sprites = Enumerable.Range(0, packedSprites.arraySize)
					.Select(index => packedSprites.GetArrayElementAtIndex(index).objectReferenceValue)
					.OfType<Sprite>()
					.ToArray();
				foreach(var sprite in sprites)
				{
					AssetDatabase.SetLabels(sprite, new string[] {spriteAtlas.name});
				}
			}
		}
	}
}
