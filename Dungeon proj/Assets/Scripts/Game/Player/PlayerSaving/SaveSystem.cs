using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public void SavePlayer(CoinController playerCoins)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.sigma";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData
        {
            coins = playerCoins.coinAmt,
        };

        Debug.Log("Saved coins are " + data.coins);

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
