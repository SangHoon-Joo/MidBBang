using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
    /// <summary>
    /// Class that permits auto-loading a bootstrap scene when the editor switches play state. This class is
    /// initialized when Unity is opened and when scripts are recompiled. This is to be able to subscribe to
    /// EditorApplication's playModeStateChanged event, which is when we wish to open a new scene.
    /// </summary>
    /// <remarks>
    /// A critical edge case scenario regarding NetworkManager is accounted for here.
    /// A NetworkObject's GlobalObjectIdHash value is currently generated in OnValidate() which is invoked during a
    /// build and when the asset is loaded/viewed in the editor.
    /// If we were to manually open Bootstrap scene via EditorSceneManager.OpenScene(...) as the editor is exiting play
    /// mode, Bootstrap scene would be entering play mode within the editor prior to having loaded any assets, meaning
    /// NetworkManager itself has no entry within the AssetDatabase cache. As a result of this, any referenced Network
    /// Prefabs wouldn't have any entry either.
    /// To account for this necessary AssetDatabase step, whenever we're redirecting from a new scene, or a scene
    /// existing in our EditorBuildSettings, we forcefully stop the editor, open Bootstrap scene, and re-enter play
    /// mode. This provides the editor the chance to create AssetDatabase cache entries for the Network Prefabs assigned
    /// to the NetworkManager.
    /// If we are entering play mode directly from Bootstrap scene, no additional steps need to be taken and the scene
    /// is loaded normally.
    /// </remarks>
    [InitializeOnLoad]
    public class SceneBootstrapper
    {
        private const string _PreviousSceneKey = "PreviousScene";
        private const string _ShouldLoadBootstrapSceneKey = "LoadBootstrapScene";

        private const string _LoadBootstrapSceneOnPlay = "Tools/Load Bootstrap Scene On Play";
        private const string _DoNotLoadBootstrapSceneOnPlay = "Tools/Don't Load Bootstrap Scene On Play";

        private static bool _RestartingToSwitchScene;

        private static string BootstrapScene => EditorBuildSettings.scenes[0].path;

        // to track where to go back to
        static string PreviousScene
        {
            get => EditorPrefs.GetString(_PreviousSceneKey);
            set => EditorPrefs.SetString(_PreviousSceneKey, value);
        }

        static bool ShouldLoadBootstrapScene
        {
            get
            {
                if (!EditorPrefs.HasKey(_ShouldLoadBootstrapSceneKey))
                {
                    EditorPrefs.SetBool(_ShouldLoadBootstrapSceneKey, true);
                }

                return EditorPrefs.GetBool(_ShouldLoadBootstrapSceneKey, true);
            }
            set => EditorPrefs.SetBool(_ShouldLoadBootstrapSceneKey, value);
        }

        static SceneBootstrapper()
        {
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        [MenuItem(_LoadBootstrapSceneOnPlay, true)]
        static bool ShowLoadBootstrapSceneOnPlay()
        {
            return !ShouldLoadBootstrapScene;
        }

        [MenuItem(_LoadBootstrapSceneOnPlay)]
        static void EnableLoadBootstrapSceneOnPlay()
        {
            ShouldLoadBootstrapScene = true;
        }

        [MenuItem(_DoNotLoadBootstrapSceneOnPlay, true)]
        static bool ShowDoNotLoadBootstrapSceneOnPlay()
        {
            return ShouldLoadBootstrapScene;
        }

        [MenuItem(_DoNotLoadBootstrapSceneOnPlay)]
        static void DisableDoNotLoadBootstrapSceneOnPlay()
        {
            ShouldLoadBootstrapScene = false;
        }

        static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (!ShouldLoadBootstrapScene)
                return;
            
            if (_RestartingToSwitchScene)
            {
                if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
                {
                    // for some reason there's multiple start and stops events happening while restarting the editor playmode. We're making sure to
                    // set stoppingAndStarting only when we're done and we've entered playmode. This way we won't corrupt "activeScene" with the multiple
                    // start and stop and will be able to return to the scene we were editing at first
                    _RestartingToSwitchScene = false;
                }
                return;
            }

            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
            {
                // cache previous scene so we return to this scene after play session, if possible
                PreviousScene = EditorSceneManager.GetActiveScene().path;

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    // user either hit "Save" or "Don't Save"; open bootstrap scene

                    if (!string.IsNullOrEmpty(BootstrapScene) &&
                        System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == BootstrapScene))
                    {
                        var activeScene = EditorSceneManager.GetActiveScene();

                        _RestartingToSwitchScene = activeScene.path == string.Empty || !BootstrapScene.Contains(activeScene.path);

                        // we only manually inject Bootstrap scene if we are in a blank empty scene,
                        // or if the active scene is not already BootstrapScene
                        if (_RestartingToSwitchScene)
                        {
                            EditorApplication.isPlaying = false;

                            // scene is included in build settings; open it
                            EditorSceneManager.OpenScene(BootstrapScene);

                            EditorApplication.isPlaying = true;
                        }
                    }
                }
                else
                {
                    // user either hit "Cancel" or exited window; don't open bootstrap scene & return to editor
                    EditorApplication.isPlaying = false;
                }
            }
            else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                if (!string.IsNullOrEmpty(PreviousScene))
                {
                    EditorSceneManager.OpenScene(PreviousScene);
                }
            }
        }
    }
}
