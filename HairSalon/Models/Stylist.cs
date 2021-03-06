using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
  public class Stylist
  {
    private string _name;
    private int _id;

    public Stylist(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if (!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        return this.GetId().Equals(newStylist.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }
    public void UpdateStylistName(string newStylistName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE stylists SET name = @newStylistName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newStylistName";
      name.Value = _name;
      cmd.Parameters.Add(name);

      _name = newStylistName;
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Client> GetClients()
    {
    List<Client> allStylistsClients = new List<Client> {};
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @stylist_id;";

    MySqlParameter stylistId = new MySqlParameter();
    stylistId.ParameterName = "@stylist_id";
    stylistId.Value = this._id;
    cmd.Parameters.Add(stylistId);


    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    while(rdr.Read())
    {
      int clientId = rdr.GetInt32(0);
      string clientName = rdr.GetString(1);
      int clientStylistsId = rdr.GetInt32(2);

      Client newClient = new Client(clientName, clientStylistsId, clientId);
      allStylistsClients.Add(newClient);
    }
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return allStylistsClients;
  }

  public void Save()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO stylists (name) VALUES (@name);";

    MySqlParameter name = new MySqlParameter();
    name.ParameterName = "@name";
    name.Value = this._name;
    cmd.Parameters.Add(name);

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }

  }
  public static List<Stylist> GetAll()
  {
    List<Stylist> allStylists = new List<Stylist> {};
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM stylists;";
    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    while(rdr.Read())
    {
      int stylistId = rdr.GetInt32(0);
      string stylistName = rdr.GetString(1);
      Stylist newStylist = new Stylist(stylistName, stylistId);
      allStylists.Add(newStylist);
    }
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return allStylists;
  }
  public static Stylist Find(int id)
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM stylists WHERE id = (@searchId);";

    MySqlParameter searchId = new MySqlParameter();
    searchId.ParameterName = "@searchId";
    searchId.Value = id;
    cmd.Parameters.Add(searchId);

    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    int stylistId = 0;
    string stylistName = "";

    while(rdr.Read())
    {
      stylistId = rdr.GetInt32(0);
      stylistName = rdr.GetString(1);
    }
    Stylist newStylist = new Stylist(stylistName, stylistId);
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return newStylist;
  }
  public void DeleteStylist()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"DELETE FROM stylists WHERE id = @thisId;";

    MySqlParameter thisId = new MySqlParameter();
    thisId.ParameterName = "@thisId";
    thisId.Value = _id;
    cmd.Parameters.Add(thisId);

    cmd.ExecuteNonQuery();

    conn.Close();
    if (conn != null)
    {
        conn.Dispose();
    }
  }
  public static void DeleteAll()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"DELETE FROM stylists;";
    cmd.ExecuteNonQuery();
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
  }
}
}
