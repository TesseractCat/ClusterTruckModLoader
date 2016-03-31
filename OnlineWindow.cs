using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.IO;

namespace TesseractModLoader.Window
{
    public class Mod
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool Downloaded { get; set; } = false;
    }

    public class Online : MonoBehaviour
    {
        //TODO: Add some sort of way where the user can choose to replace file or not.
        public bool REPLACE = true;

        public Rect onlineWindowRect = new Rect(20, 20, 375, 600);
        public bool onlineWindow = false;
        public List<Mod> Mods;

        //TODO: Add ability (maybe config) for user to define their own mod list so its not hardcoded.
        public string modListUrl = @"https://raw.githubusercontent.com/TesseractCat/tesseract-loader/dev/ModList.xml";
        public Vector2 DataBaseModListScrollBar = new Vector2();

        public void Start()
        {
            //Fails to pass certificates when downloading so I had to do this.
            //TODO: Find and implement the correct way to do this
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error disabling certificates.");
            }
            string xml;
            using (WebClient wc = new WebClient())
            {
                xml = wc.DownloadString(modListUrl);
            }
            Mods = new List<Mod>();
            //UnityEngine.Debug.Log("XML Length: " + xml.Length.ToString());
            if (xml != null && xml.Length > 0)
            {
                const string ModStart = "<Mod>";
                const string ModEnd = "</Mod>";

                //Searchs through the xml document looking for a <Mod> </Mod> Pair
                int start = -1;
                int end = -1;
                do
                {
                    start = xml.IndexOf(ModStart, start + 1) + ModStart.Length;
                    end = xml.IndexOf(ModEnd, end + 1);
                    //If start was found and an end was found greater than start
                    if (start >= 0 && end > start)
                    {
                        //Section off single mod
                        string part = xml.Substring(start, end - start);

                        //Looks for name of mod
                        string ModName = GetValue(part, "Name");

                        //Looks for author of mod
                        string ModAuthor = GetValue(part, "Author");

                        //Looks for description of mod
                        string ModDescription = GetValue(part, "Description");

                        //Looks for Link of mod 
                        string ModLink = GetValue(part, "Link");

                        //Checks if all parts of mod were found. Empty parts are valid as long as the tags exist
                        if (ModName != null && ModAuthor != null && ModDescription != null && ModLink != null)
                        {
                            //Adds the mod to the mod list
                            Mods.Add(new Mod()
                            {
                                Name = ModName,
                                Author = ModAuthor,
                                Description = ModDescription,
                                Link = ModLink
                            });
                            UnityEngine.Debug.Log("Mod Loaded from database: " + ModName);
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Invalid Mod in database");
                        }
                    }
                    //When no more mods are to be found
                } while (start >= 0 && end >= start);
            }
            else
            {
                UnityEngine.Debug.Log("Error downloading database.");
            }
        }

        public void OnGUI()
        {
            GUI.skin = TesseractModLoader.Window.UI.UISkin;
            if (onlineWindow)
            {
                onlineWindowRect = GUI.Window(3, onlineWindowRect, OnlineWindow, "Online Mod Browser", GUI.skin.GetStyle("window"));
            }
        }

        public void OnlineWindow(int windowID)
        {
            DataBaseModListScrollBar = GUILayout.BeginScrollView(DataBaseModListScrollBar, false, true);
            if (Mods != null)
            {

                GUILayout.Label("Online mod database.");
                foreach (Mod m in Mods)
                {
                    GUILayout.Label("---");
                    GUILayout.Label("Name: " + m.Name);
                    GUILayout.Label("Author: " + m.Author);
                    GUILayout.Label("Description: " + m.Description);

                    //TODO: Make the layout better
                    //TODO: Show mods on modlist before the restart occurs
                    if (GUILayout.Button("Download (Requires Restart)") && !m.Downloaded)
                    {
                        m.Downloaded = true;
                        Uri uri = new Uri(m.Link);
                        string originalfilename = Path.GetFileNameWithoutExtension(uri.LocalPath);
                        string filename = originalfilename;
                        string extension = Path.GetExtension(uri.LocalPath);
                        bool foundSame = false;
                        if (REPLACE)
                        {
                            if (File.Exists(Application.dataPath + "/Managed/Mods/" + filename + "." + extension))
                            {
                                UnityEngine.Debug.Log("Removing duplicate mod.");
                                File.Delete(Application.dataPath + "/Managed/Mods/" + filename + "." + extension);
                            }
                        }
                        else
                        {
                            int change = 1;
                            while (File.Exists(Application.dataPath + "/Managed/Mods/" + filename + "." + extension))
                            {
                                foundSame = true;
                                filename += originalfilename + $"({change})";
                                change++;
                            }
                        }
                        using (WebClient wc = new WebClient())
                        {
                            UnityEngine.Debug.Log("Downloading mod " + m.Name);
                            wc.DownloadFile(uri, Application.dataPath + "/Managed/Mods/" + filename + extension);
                            UnityEngine.Debug.Log("Downloaded mod " + m.Name);
                        }
                    }
                }
            }
            else
            {
                GUILayout.Label("Error loading mod database.");
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U) || Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.U))
            {
                onlineWindow = !onlineWindow;
            }
        }

        /// <summary>
        /// Pulls tag value from mod data
        /// </summary>
        /// <param name="Whole">Entire Mod Data</param>
        /// <param name="Tag">Tag that vaule is being searched for</param>
        /// <returns>Value of tag</returns>
        private string GetValue(string Whole, string Tag)
        {
            string startTag = $"<{Tag}>";
            string endTag = $"</{Tag}>";

            int nameS = Whole.IndexOf(startTag) + startTag.Length;
            if (nameS >= 0)
            {
                int nameE = Whole.IndexOf(endTag, nameS);
                if (nameE >= 0)
                {
                    return Whole.Substring(nameS, nameE - nameS);
                }
            }
            return null;
        }
    }
}

