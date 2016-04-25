using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace TesseractModLoader.Window
{
	public class Debug : MonoBehaviour
	{
		public Rect debugWindowRect = new Rect(20,20,400,600);
		public bool debugWindow = false;
		public Vector2 debugScrollBar = new Vector2 ();
		List<String> Logs = new List<String>();
		String currentLog = "";
		public String consoleEntry = "";
        public String consoleCode = @"
        using System;
        using UnityEngine;
            
        namespace Stuff
        {                
            public class IsCool
            {                
                public static void Main()
                {
                    INSERTCODEHERE
                }
            }
        }
        ";

        void Start() {
			Application.logMessageReceived += HandleLog;
		}

		void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (debugWindow) {
				debugWindowRect = GUI.Window (2, debugWindowRect, DebugWindow, "Debug");
			}
		}

		public void DebugWindow(int windowID) {
			debugScrollBar = GUILayout.BeginScrollView (debugScrollBar,false,true);

			foreach (String s in Logs) {
				GUILayout.Label (s);
			}

			GUILayout.EndScrollView ();
            
            /*
            consoleEntry = GUILayout.TextField(consoleEntry);

            if (GUILayout.Button("COMPILE"))
            {
                CompileLine();
            }*/

			GUI.DragWindow ();
		}

        public void CompileLine()
        {
            try {
                CSharpCodeProvider csc = new CSharpCodeProvider();
                CompilerParameters cscpar = new CompilerParameters(new[] { "System.Core.dll", "UnityEngine.dll" });
                cscpar.GenerateInMemory = true;
                cscpar.GenerateExecutable = false;

                CompilerResults cscres = csc.CompileAssemblyFromSource(cscpar, consoleCode.Replace("INSERTCODEHERE", consoleEntry));

                if (cscres.Errors.HasErrors)
                {
                    UnityEngine.Debug.Log("ERROR");
                }
                else
                {
                    Assembly cscass = cscres.CompiledAssembly;
                    Type cscprog = cscass.GetType("Stuff.IsCool");
                    MethodInfo cscmain = cscprog.GetMethod("Main");
                    cscmain.Invoke(null, null);
                }

                consoleEntry = "";
            } catch (TypeLoadException e)
            {
                UnityEngine.Debug.Log(e.InnerException);
                UnityEngine.Debug.Log("EDTSEOITJPOIGJPOI");
            }
        }

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.P) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.P)) {
				debugWindow = !debugWindow;
			}

            if (Input.GetKeyDown(KeyCode.Return))
            {
                
            }
		}

		public void HandleLog (String logString, String stackTrace, LogType logType) {
			Logs.Add (logString);
			debugScrollBar.y = Mathf.Infinity;
		}
	}
}