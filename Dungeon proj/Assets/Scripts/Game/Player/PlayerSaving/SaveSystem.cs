using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveSystem
{
    public void SavePlayer(CoinController playerCoins, StaminaController staminaController, 
                           HealthController healthController, PlayerMovement playerMovement, 
                           PlayerWeaponController playerWeaponController)
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
        Debug.Log("Save file path: " + path); // Log the path for debugging

/*        try
        {*/
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData
            {
                coins = playerCoins.coinAmt,
                max_health = healthController._maximumHealth,
                curr_health = healthController._currentHealth,
                max_stamina = staminaController._maximumStamina,
                curr_stamina = staminaController._currentStamina,
                speed = playerMovement.GetSpeed(),
                weapons = playerWeaponController.GetInventoryIndexes(),
                can_use_ability = playerMovement.CanUseAbility(),
            };

            Debug.Log("Saved coins are " + data.coins);
            Debug.Log("Saved max_health is " + data.max_health);
            Debug.Log("Saved curr_health is " + data.curr_health);
            Debug.Log("Saved max_stamina is " + data.max_stamina);
            Debug.Log("Saved curr_stamina are " + data.curr_stamina);
            Debug.Log("Saved speed is " + data.speed);

            formatter.Serialize(stream, data);
            stream.Close();
/*        }
        catch (Exception exception)
        {
            Debug.LogError("Error saving player data: " + exception.Message);
        }*/

    }

    public PlayerData LoadPlayer()
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

    public void ResetSave()
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
                    data.can_use_ability = false;

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
}
