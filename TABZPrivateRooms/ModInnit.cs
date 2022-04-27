using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using BepInEx;
using BepInEx.Configuration;

using HarmonyLib;
namespace TABZPrivateRooms
{

    [BepInPlugin("Locochoco.plugins.TABZPrivateRooms", "Private Rooms", "1.0.0.0")]
    [BepInProcess("GAME.exe")] //TABZ executable
    public class ModInnit : BaseUnityPlugin
    {
        private ConfigEntry<string> configPrivateRoomName;
        public static string PrivateRoomName;
        public void Awake()
        {
            try
            {
                configPrivateRoomName = Config.Bind("General",      // The section under which the option is shown
                                         "PrivateRoomName",  // The key of the configuration option in the configuration file
                                         "", // The default value
                                         "The private Room Name, share this information so your friends can join with you");
                Config.SettingChanged += (s,e) => LoadPrivateRoom();

                LoadPrivateRoom();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        private static readonly string OriginalVersionNumber = AccessTools.StaticFieldRefAccess<string>(typeof(MainMenuHandler), "mVersionString");
       
        private void LoadPrivateRoom() 
        {
            if (configPrivateRoomName.Value != null)
                PrivateRoomName = configPrivateRoomName.Value;

            Debug.Log(string.Format("Changing to private room {0}", PrivateRoomName));
            AccessTools.StaticFieldRefAccess<string>(typeof(MainMenuHandler), "mVersionString") = OriginalVersionNumber + PrivateRoomName;

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                MusicHandler.StopMusic();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene(0);
            }
        }
    }
}
