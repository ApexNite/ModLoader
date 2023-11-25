using NeoModLoader.constants;
using NeoModLoader.services;
using UnityEngine;

namespace NeoModLoader.api;
/// <summary>
/// If you want to create a simple mod, you can inherit this class.
/// <para>Then NML will find this class in your compiled mod, then load it into ModLoader provided by WorldBox</para>
/// <para>You can get your mod's information by <see cref="GetDeclaration"/></para>
/// <para>You can get your mod's gameObject loaded to ModLoader by <see cref="GetGameObject"/></para>
/// <para>You must override <see cref="OnModLoad"/> to load your mod or add loading code to Awake and others will called automatically</para>
/// <remarks>
/// The common order of calling is:
/// OnModLoad -> Awake -> OnEnable -> Start -> Update
/// </remarks>
/// </summary>
public abstract class BasicMod<T> : MonoBehaviour, IMod, ILocalizable, IConfigurable where T : BasicMod<T>
{
    private ModDeclare _declare = null!;
    private ModConfig _config = null!;
    /// <summary>
    /// Instance of your mod.
    /// </summary>
    public static T Instance { get; private set; }
    private bool _isLoaded = false;
    /// <summary>
    /// Get the gameObject the mod attached to.
    /// </summary>
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    /// <summary>
    /// Get the url set in mod.json or url of WorldBoxOpenMods' organization.
    /// </summary>
    public virtual string GetUrl()
    {
        return string.IsNullOrEmpty(_declare.RepoUrl) ? CoreConstants.OrgURL : _declare.RepoUrl;
    }

    /// <summary>
    /// Do not call this method manually, it is useless.
    /// <remarks>Unless you know what to do, like try loading again after an exception on <see cref="OnModLoad"/></remarks>
    /// </summary>
    public void OnLoad(ModDeclare pModDecl, GameObject pGameObject)
    {
        if (_isLoaded) return;
        _declare = pModDecl;
        Instance = (T)this;
        _config ??= LoadConfig();
        LogInfo("OnLoad");
        OnModLoad();
        LogInfo("Loaded");
        _isLoaded = true;
    }

    private ModConfig LoadConfig()
    {
        ModConfig persistent_config =
            new ModConfig(Path.Combine(Paths.ModsConfigPath, $"{_declare.UID}.config"), true);
        
        string default_config_path = Path.Combine(_declare.FolderPath, Paths.ModDefaultConfigFileName);
        if(!File.Exists(default_config_path)) return persistent_config;
        
        ModConfig default_config = new ModConfig(Path.Combine(_declare.FolderPath, Paths.ModDefaultConfigFileName), false);
        persistent_config.MergeWith(default_config);

        return persistent_config;
    }
    /// <summary>
    /// You should override this method to load your mod.
    /// </summary>
    protected abstract void OnModLoad();
    /// <summary>
    /// Log a message with mod name.
    /// </summary>
    public static void LogInfo(string message)
    {
        LogService.LogInfo($"[{Instance._declare.Name}]: {message}");
    }
    /// <summary>
    /// Log a warning message with mod name.
    /// </summary>
    public static void LogWarning(string message)
    {
        LogService.LogWarning($"[{Instance._declare.Name}]: {message}");
    }
    /// <summary>
    /// Log an error message with mod name.
    /// </summary>
    public static void LogError(string message)
    {
        LogService.LogError($"[{Instance._declare.Name}]: {message}");
    }
    /// <summary>
    /// Get the gameObject the mod attached to.
    /// </summary>
    public ModDeclare GetDeclaration()
    {
        return _declare;
    }
    /// <summary>
    /// If you need to add locale files for your mod, create locale files written by JSON under `Locales` directory in your mod 
    /// </summary>
    /// <returns>The path to the directory of your locale files</returns>
    public string GetLocaleFilesDirectory(ModDeclare pModDeclare)
    {
        return Path.Combine(pModDeclare.FolderPath, "Locales");
    }
    /// <summary>
    /// Get the config of your mod.
    /// </summary>
    /// <returns>Config instance reference</returns>
    public ModConfig GetConfig()
    {
        return _config;
    }
}