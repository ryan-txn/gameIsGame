using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public void SavePlayer(CoinController playerCoins, StaminaController staminaController, HealthController healthController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.sigma";
        Debug.Log("Save file path: " + path); // Log the path for debugging

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData
        {
            coins = playerCoins.coinAmt,
            max_health = healthController._maximumHealth,
            curr_health = healthController._currentHealth,
            max_stamina = staminaController._maximumStamina,
            curr_stamina = staminaController._currentStamina,

        };

        Debug.Log("Saved coins are " + data.coins);
        Debug.Log("Saved max_health is " + data.max_health);
        Debug.Log("Saved curr_health is " + data.curr_health);
        Debug.Log("Saved max_stamina is " + data.max_stamina);
        Debug.Log("Saved curr_stamina are " + data.curr_stamina);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
