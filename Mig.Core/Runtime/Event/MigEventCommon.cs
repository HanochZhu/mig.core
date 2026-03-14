using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MigEventCommon
{
    public static readonly string UpdateMetallic = "UpdateMaterial";

    public static readonly string ChangePivotToGroupGizmoMode = "ChangePivotToGroupGizmoMode";
    public static readonly string UpdateGizmo = "UpdateGizmo";
    public static string OnDeleteModelUIClick = "OnDeleteModelUIClick";
    public static string AnnotateModel = "AnnotateModel";
    public static string OnPaintModel = "OnPaintModel";

    public static string SetCurrentGizmoState = "SetCurrentGizmoState";
    public static string QueryCurrentGizmoState = "QueryCurrentGizmoState";
    public static string OnGizmoStateChanged = "OnGizmoStateChanged";
    public static string ResponseCurrentGizmoState = "ResponseCurrentGizmoState";

    // interact
    public static string OnClickModel = "OnClickModel";
    public static string OnSelectedChanged = "OnSelectedChanged";
    public static string OnChangeSelectModelTexture = "OnChangeSelectModelTexture";

    public static string OnModelPropertiesChange = "OnModelPropertiesChange";

    public static string OnGizmosDragBegin = "OnGizmosDragBegin";
    public static string OnGizmosDragUpdate = "OnGizmosDragUpdate";
    public static string OnGizmosDragEnd = "OnGizmosDragEnd";

    public static string OnPrompt = "OnPrompt";
    public static string OnLoadingModelBegin = "OnLoadingModelBegin";
    public static string OnLoadingModelEnd = "OnLoadingModelEnd";

    public static string OnSaveModelBegin = "OnSaveModelBegin";
    public static string OnSaveModelEnd = "OnSaveModelEnd";

}
