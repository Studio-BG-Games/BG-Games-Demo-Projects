#if UNITY_IPHONE || UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif



public class PostProcessB : MonoBehaviour
{
    [PostProcessBuild]
    public static void AddToEntitlements(BuildTarget buildTarget, string buildPath)
    {

        if (buildTarget != BuildTarget.iOS) return;

        // get project info
        string pbxPath = PBXProject.GetPBXProjectPath(buildPath);
        var proj = new PBXProject();
        proj.ReadFromFile(pbxPath);
        var guid = proj.GetUnityMainTargetGuid();

        // get entitlements path
        string[] idArray = Application.identifier.Split('.');
        var entitlementsPath = $"Unity-iPhone/{idArray[idArray.Length - 1]}.entitlements";

        // create capabilities manager
        var capManager = new ProjectCapabilityManager(pbxPath, entitlementsPath, null, guid);

        // Add necessary capabilities
        capManager.AddPushNotifications(true);

        // Write to file
        capManager.WriteToFile();
    }


}

public static class SKAdNetworkUpdater
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        string targetGUID = pbxProject.GetUnityMainTargetGuid();
#else
        string targetGUID = pbxProject.TargetGuidByName("Unity-iPhone");
#endif

        string shellPath = "/bin/sh";
        int index = 10;
        string name = "Update SKAdNetwork ids";
        string shellScript = @"cd ""${ CONFIGURATION_BUILD_DIR}/${UNLOCALIZED_RESOURCES_FOLDER_PATH}/Frameworks/UnityFramework.framework/""
if [[-d ""Frameworks""]]; then
    rm-fr Frameworks
 fi";
        pbxProject.InsertShellScriptBuildPhase(index, targetGUID, name, shellPath, shellScript);
        pbxProject.WriteToFile(projectPath);
    }


}
#endif
#endif
