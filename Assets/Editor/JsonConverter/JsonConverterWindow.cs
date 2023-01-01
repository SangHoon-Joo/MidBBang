using UnityEngine;
using UnityEditor;

namespace Krafton.SP2.X1.Lib
{
	public class JsonConverterWindow : EditorWindow 
	{
		public static string InputFolderPathPrefsName = "JsonConverter.InputFolderPath";
		public static string OuputFolderPathPrefsName = "JsonConverter.OutputFolderPath";
		public static string ConvertOnlyModifiedFilesPrefsName = "JsonConverter.ConvertOnlyModifiedFiles";
		
		private string _InputFolderPath;
		private string _OutputFolderPath;
		private bool _ConvertOnlyModifiedFiles;

		private JsonConverter _JsonConverter;

		[MenuItem ("Tools/Json Converter", false, 1)]
		public static void ShowWindow() 
		{
			EditorWindow.GetWindow(typeof(JsonConverterWindow), true, "Json Converter", true);
		}

		public void OnEnable()
		{
			if(_JsonConverter == null)
			{
				_JsonConverter = new JsonConverter();
			}

			_InputFolderPath = EditorPrefs.GetString(InputFolderPathPrefsName, Application.dataPath);
			if(string.IsNullOrEmpty(_InputFolderPath))
				_InputFolderPath = $"{Application.dataPath}".Replace("Assets", "Data");
			_OutputFolderPath = EditorPrefs.GetString(OuputFolderPathPrefsName, Application.dataPath);
			if(string.IsNullOrEmpty(_OutputFolderPath))
				_OutputFolderPath = $"{Application.dataPath}/AddressableResources/Data";
			_ConvertOnlyModifiedFiles = EditorPrefs.GetBool(ConvertOnlyModifiedFilesPrefsName, false);
		}
		
		public void OnDisable()
		{
			EditorPrefs.SetString(InputFolderPathPrefsName, _InputFolderPath);
			EditorPrefs.SetString(OuputFolderPathPrefsName, _OutputFolderPath);
			EditorPrefs.SetBool(ConvertOnlyModifiedFilesPrefsName, _ConvertOnlyModifiedFiles);
		}

		void OnGUI()
		{
			GUILayout.BeginHorizontal();

			GUIContent inputFolderContent = new GUIContent("Input Folder", "Select the folder where the Excel or CSV files to be processed are located.");
			EditorGUIUtility.labelWidth = 120.0f;
			EditorGUILayout.TextField(inputFolderContent, _InputFolderPath, GUILayout.MinWidth(120), GUILayout.MaxWidth(500));
			if(GUILayout.Button(new GUIContent("Select Folder"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
			{
				_InputFolderPath = EditorUtility.OpenFolderPanel("Select Folder with Excel or CSV Files", _InputFolderPath, Application.dataPath);
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			GUIContent outputFolderContent = new GUIContent("Output Folder", "Select the folder where the converted json files should be saved.");
			EditorGUILayout.TextField(outputFolderContent, _OutputFolderPath, GUILayout.MinWidth(120), GUILayout.MaxWidth(500));
			if(GUILayout.Button(new GUIContent("Select Folder"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
			{
				_OutputFolderPath = EditorUtility.OpenFolderPanel("Select Folder to save json files", _OutputFolderPath, Application.dataPath);
			}
			
			GUILayout.EndHorizontal();

			GUIContent modifiedToggleContent = new GUIContent("Convert Only Modified Files", "If checked, only Excel files which have been newly added or updated since the last conversion will be processed.");
			_ConvertOnlyModifiedFiles = EditorGUILayout.Toggle(modifiedToggleContent, _ConvertOnlyModifiedFiles);

			if(string.IsNullOrEmpty(_InputFolderPath) || string.IsNullOrEmpty(_OutputFolderPath))
			{
				GUI.enabled = false;
			}

			GUILayout.BeginArea(new Rect((Screen.width / 2) - (200 / 2), (Screen.height / 2) - (25 / 2), 200, 25));

			if(GUILayout.Button("Convert Files"))
			{
				_JsonConverter.ConvertFilesToJson(_InputFolderPath, _OutputFolderPath, _ConvertOnlyModifiedFiles);
			}

			GUILayout.EndArea();

			GUI.enabled = true;
		}
	}

	//[InitializeOnLoad]
	public class JsonAutoConverter 
	{	
		/// <summary>
		/// Class attribute [InitializeOnLoad] triggers calling the static constructor on every refresh.
		/// </summary>
		static JsonAutoConverter() 
		{
			string inputFolderPath = EditorPrefs.GetString(JsonConverterWindow.InputFolderPathPrefsName, Application.dataPath);
			string outputFolderPath = EditorPrefs.GetString(JsonConverterWindow.OuputFolderPathPrefsName, Application.dataPath);
			bool convertOnlyModifiedFiles = EditorPrefs.GetBool(JsonConverterWindow.ConvertOnlyModifiedFilesPrefsName, false);
			
			JsonConverter jsonConverter = new JsonConverter();
			jsonConverter.ConvertFilesToJson(inputFolderPath, outputFolderPath, convertOnlyModifiedFiles);
		}
	}
}