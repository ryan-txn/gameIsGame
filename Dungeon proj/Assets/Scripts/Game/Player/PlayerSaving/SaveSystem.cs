using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class PlayerDataWrapper
{
    public int coins;
    public float max_health;
    public float curr_health;
    public float max_stamina;
    public float curr_stamina;
    public float speed;
    public string weaponsJson;
    public bool ability;
}

public class SaveSystem
{
    private const string PLAYERPREFS_KEY = "DungeonLuncheonSave";

    public void SavePlayer(CoinController playerCoins, StaminaController staminaController, 
                           HealthController healthController, PlayerMovement playerMovement, 
                           PlayerWeaponController playerWeaponController, PlayerAbility playerAbility)
    {
        PlayerData data = new PlayerData
        {
            coins = playerCoins.coinAmt,
            max_health = healthController._maximumHealth,
            curr_health = healthController._currentHealth,
            max_stamina = staminaController._maximumStamina,
            curr_stamina = staminaController._currentStamina,
            speed = playerMovement.GetSpeed(),
            weapons = playerWeaponController.GetInventoryIndexes(),
            ability = playerAbility.CanUseAbility(),
        };

        Debug.Log("Saved coins are " + data.coins);
        Debug.Log("Saved max_health is " + data.max_health);
        Debug.Log("Saved curr_health is " + data.curr_health);
        Debug.Log("Saved max_stamina is " + data.max_stamina);
        Debug.Log("Saved curr_stamina are " + data.curr_stamina);
        Debug.Log("Saved speed is " + data.speed);

#if UNITY_WEBGL && !UNITY_EDITOR
        SaveToPlayerPrefs(data);
#else
        SaveToFile(data);
#endif
    }

    private void SaveToFile(PlayerData data)
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
        Debug.Log("Save file path: " + path);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            Debug.Log("Saved to file successfully");
        }
        catch (Exception exception)
        {
            Debug.LogError("Error saving player data to file: " + exception.Message);
        }
    }

    private void SaveToPlayerPrefs(PlayerData data)
    {
        try
        {
            PlayerPrefs.SetInt(PLAYERPREFS_KEY + "_coins", data.coins);
            PlayerPrefs.SetFloat(PLAYERPREFS_KEY + "_max_health", data.max_health);
            PlayerPrefs.SetFloat(PLAYERPREFS_KEY + "_curr_health", data.curr_health);
            PlayerPrefs.SetFloat(PLAYERPREFS_KEY + "_max_stamina", data.max_stamina);
            PlayerPrefs.SetFloat(PLAYERPREFS_KEY + "_curr_stamina", data.curr_stamina);
            PlayerPrefs.SetFloat(PLAYERPREFS_KEY + "_speed", data.speed);
            PlayerPrefs.SetInt(PLAYERPREFS_KEY + "_ability", data.ability ? 1 : 0);

            if (data.weapons != null)
            {
                string weaponsJson = JsonUtility.ToJson(new IntArrayWrapper { array = data.weapons });
                PlayerPrefs.SetString(PLAYERPREFS_KEY + "_weapons", weaponsJson);
            }
            else
            {
                PlayerPrefs.SetString(PLAYERPREFS_KEY + "_weapons", "");
            }

            PlayerPrefs.Save();
            Debug.Log("Saved to PlayerPrefs successfully");
        }
        catch (Exception exception)
        {
            Debug.LogError("Error saving player data to PlayerPrefs: " + exception.Message);
        }
    }

    public PlayerData LoadPlayer()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return LoadFromPlayerPrefs();
#else
        return LoadFromFile();
#endif
    }

    private PlayerData LoadFromFile()
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
        Debug.Log("Loading savefile from path: " + path);
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                if (data != null) {
                    return data;
                }
                else
                {
                    Debug.LogError("Deserialised data is null, resetting save");
                    ResetSave();
                    return LoadPlayer();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading player data: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    private PlayerData LoadFromPlayerPrefs()
    {
        try
        {
            if (!PlayerPrefs.HasKey(PLAYERPREFS_KEY + "_coins"))
            {
                Debug.LogError("No save data found in PlayerPrefs");
                return null;
            }

            int[] weapons = null;
            string weaponsJson = PlayerPrefs.GetString(PLAYERPREFS_KEY + "_weapons", "");
            if (!string.IsNullOrEmpty(weaponsJson))
            {
                try
                {
                    IntArrayWrapper wrapper = JsonUtility.FromJson<IntArrayWrapper>(weaponsJson);
                    weapons = wrapper.array;
                }
                catch
                {
                    weapons = null;
                }
            }

            PlayerData data = new PlayerData
            {
                coins = PlayerPrefs.GetInt(PLAYERPREFS_KEY + "_coins", 0),
                max_health = PlayerPrefs.GetFloat(PLAYERPREFS_KEY + "_max_health", 100),
                curr_health = PlayerPrefs.GetFloat(PLAYERPREFS_KEY + "_curr_health", 100),
                max_stamina = PlayerPrefs.GetFloat(PLAYERPREFS_KEY + "_max_stamina", 200),
                curr_stamina = PlayerPrefs.GetFloat(PLAYERPREFS_KEY + "_curr_stamina", 200),
                speed = PlayerPrefs.GetFloat(PLAYERPREFS_KEY + "_speed", 6),
                weapons = weapons,
                ability = PlayerPrefs.GetInt(PLAYERPREFS_KEY + "_ability", 0) == 1
            };

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading player data from PlayerPrefs: " + e.Message);
            return null;
        }
    }

    public void ResetSave()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ResetSavePlayerPrefs();
#else
        ResetSaveFile();
#endif
    }

    private void ResetSaveFile()
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
        Debug.Log("Resetting save file at: " + path);

        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                if (data != null)
                {
                    data.coins = 0;
                    data.max_health = 100;
                    data.curr_health = 100;
                    data.max_stamina = 200;
                    data.curr_stamina = 200;
                    data.speed = 6;
                    data.weapons = null;
                    data.ability = false;

                    stream = new FileStream(path, FileMode.Create);
                    formatter.Serialize(stream, data);
                    stream.Close();

                    Debug.Log("Save file reset successfully.");
                }
                else
                {
                    Debug.LogError("Save file data is null");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error resetting save file: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }

    private void ResetSavePlayerPrefs()
    {
        try
        {
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_coins");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_max_health");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_curr_health");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_max_stamina");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_curr_stamina");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_speed");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_weapons");
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY + "_ability");
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs save reset successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error resetting PlayerPrefs save: " + e.Message);
        }
    }
}

[System.Serializable]
public class IntArrayWrapper
{
    public int[] array;
}
