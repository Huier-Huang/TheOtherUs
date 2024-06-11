using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Reactor.Utilities.Attributes;
using UnityEngine;
using Version = SemanticVersioning.Version;

namespace TheOtherUs.Modules.Compatibility;

public sealed class SubmergedCompatibility : ICompatibility
{
    public const ShipStatus.MapType SUBMERGED_MAP_TYPE = (ShipStatus.MapType)6;

    private Type SubmarineStatusType;
    private MethodInfo CalculateLightRadiusMethod;

    private MethodInfo RpcRequestChangeFloorMethod;
    private Type FloorHandlerType;
    private MethodInfo GetFloorHandlerMethod;

    private Type VentPatchDataType;
    private PropertyInfo InTransitionField;

    private Type CustomTaskTypesType;
    private FieldInfo RetrieveOxigenMaskField;
    public TaskTypes RetrieveOxygenMask;
    private Type SubmarineOxygenSystemType;
    private MethodInfo SubmarineOxygenSystemInstanceField;
    private MethodInfo RepairDamageMethod;

    public Version Version { get; private set; }
    public BasePlugin Plugin { get; private set; }
    public Assembly Assembly { get; private set; }
    public Type[] Types { get; private set; }
    public Dictionary<string, Type> InjectedTypes { get; private set; }

    public MonoBehaviour SubmarineStatus { get; private set; }

    public bool IsSubmerged { get; private set; }


    public void SetupMap(ShipStatus map)
    {
        if (map == null)
        {
            IsSubmerged = false;
            SubmarineStatus = null;
            return;
        }

        IsSubmerged = map.Type == SUBMERGED_MAP_TYPE;
        if (!IsSubmerged) return;

        SubmarineStatus =
            map.GetComponent(Il2CppType.From(SubmarineStatusType))?.TryCast(SubmarineStatusType) as MonoBehaviour;
    }


    public void OnLoad(PluginInfo pluginInfo, Assembly assembly, BasePlugin plugin)
    {
        Plugin = plugin;
        Version = pluginInfo.Metadata.Version.BaseVersion();
        Assembly = assembly;
        Initialize();
    }


    public void Initialize()
    {
        Types = AccessTools.GetTypesFromAssembly(Assembly);
        InjectedTypes = (Dictionary<string, Type>)AccessTools
            .PropertyGetter(Types.FirstOrDefault(t => t.Name == "ComponentExtensions"), "RegisteredTypes")
            .Invoke(null, Array.Empty<object>());

        SubmarineStatusType = Types.First(t => t.Name == "SubmarineStatus");
        CalculateLightRadiusMethod = AccessTools.Method(SubmarineStatusType, "CalculateLightRadius");

        FloorHandlerType = Types.First(t => t.Name == "FloorHandler");
        GetFloorHandlerMethod = AccessTools.Method(FloorHandlerType, "GetFloorHandler", [typeof(PlayerControl)]);
        RpcRequestChangeFloorMethod = AccessTools.Method(FloorHandlerType, "RpcRequestChangeFloor");

        VentPatchDataType = Types.First(t => t.Name == "VentPatchData");

        InTransitionField = AccessTools.Property(VentPatchDataType, "InTransition");

        CustomTaskTypesType = Types.First(t => t.Name == "CustomTaskTypes");
        RetrieveOxigenMaskField = AccessTools.Field(CustomTaskTypesType, "RetrieveOxygenMask");
        var RetrieveOxigenMaskTaskTypeField = AccessTools.Field(CustomTaskTypesType, "taskType");
        var OxygenMaskCustomTaskType = RetrieveOxigenMaskField.GetValue(null);
        RetrieveOxygenMask = (TaskTypes)RetrieveOxigenMaskTaskTypeField.GetValue(OxygenMaskCustomTaskType)!;

        SubmarineOxygenSystemType =
            Types.First(t => t.Name == "SubmarineOxygenSystem" && t.Namespace == "Submerged.Systems.Oxygen");
        SubmarineOxygenSystemInstanceField = AccessTools.PropertyGetter(SubmarineOxygenSystemType, "Instance");
        RepairDamageMethod = AccessTools.Method(SubmarineOxygenSystemType, "RepairDamage");
    }

    public MonoBehaviour AddSubmergedComponent(GameObject obj, string typeName)
    {
        var validType = InjectedTypes.TryGetValue(typeName, out var type);
        return validType
            ? obj.AddComponent(Il2CppType.From(type)).TryCast<MonoBehaviour>()
            : obj.AddComponent<MissingSubmergedBehaviour>();
    }

    public float GetSubmergedNeutralLightRadius(bool isImpostor)
    {
 
        return (float)CalculateLightRadiusMethod.Invoke(SubmarineStatus, [null, true, isImpostor])!;
    }

    public void ChangeFloor(bool toUpper)
    {
        var _floorHandler = ((Component)GetFloorHandlerMethod.Invoke(null, [
            CachedPlayer.LocalPlayer.Control
        ])).TryCast(FloorHandlerType) as MonoBehaviour;
        RpcRequestChangeFloorMethod.Invoke(_floorHandler, [toUpper]);
    }

    public bool getInTransition()
    {
        return (bool)InTransitionField.GetValue(null)!;
    }

    public void RepairOxygen()
    {
        try
        {
            ShipStatus.Instance.RpcRepairSystem((SystemTypes)130, 64);
            RepairDamageMethod.Invoke(SubmarineOxygenSystemInstanceField.Invoke(null, Array.Empty<object>()),
                [CachedPlayer.LocalPlayer.Control, 64]);
        }
        catch (NullReferenceException)
        {
            Message("null reference in engineer oxygen fix");
        }
    }

    public static class Classes
    {
        public const string ElevatorMover = "ElevatorMover";
    }

    public string GUID { get; set; } = "Submerged";
}

[RegisterInIl2Cpp]
public class MissingSubmergedBehaviour(IntPtr ptr) : MonoBehaviour(ptr);