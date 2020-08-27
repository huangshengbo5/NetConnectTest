using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using NetServer.script.logic;

class DbManager
    {

        public static MySqlConnection MySql;
        public static JavaScriptSerializer Js = new JavaScriptSerializer();

        //连接到myspl数据库
        public static bool Connect(string db, string ip, int port, string user, string pw)
        {
            MySql = new MySqlConnection();
            string a = string.Format("Database={0};Data Source ={1};port={2};User Id ={3};Password={4}", db, ip, port,
                user, pw);
            MySql.ConnectionString = a;
            try
            {
                MySql.Open();
                Console.WriteLine("[数据库] connect Successful");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        //字符串是否安全
        private static bool isSafeString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;| , | \/ | \) | \( | \[ | \] | \{ | \} | % | @ | \* | ! | \` ]");
        }

        public static bool IsAccountExist(string id)
        {
            if (!isSafeString(id))
            {
                return false;
            }

            string s = string.Format("select * from account where id = '{0}';",id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(s,MySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return !hasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库]IsSafeString err, " + e.Message);
                throw;
            }
        }

        public static bool Register(string id, string pw)
        {
            if (!isSafeString(id))
            {
                Console.WriteLine("[数据库] Register fail , id not safe");
                return false;
            }

            if (!isSafeString(pw))
            {
                Console.WriteLine("[数据库] Register fail , pw not safe");
                return false;
            }

            if (!IsAccountExist(id))
            {
                Console.WriteLine("[数据库] Register fail , is Exist");
                return false;
            }

            string sql = string.Format("insert into account set id = '{0}',pw='{1}';", id, pw);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,MySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库] Register fail" + e.Message);
                return false;
            }
        }

        //创建角色
        public static bool CreatePlayer(string id)
        {
            if (!isSafeString(id))
            {
                Console.WriteLine("[数据库] CreatePlayer Fail , id not safe ");
                return false;
            }

            //序列化
            PlayerData playerData = new PlayerData();
            string data = Js.Serialize(playerData);
            //写入数据库
            string sql = string.Format("insert into player set id='{0}',data='{1}';", id, data);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,MySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库] CreatePlayer err" + e.Message);
                return false;
            }
        }

        //检测用户密码
        public static bool CheckPassword(string id, string pw)
        {
            if (!isSafeString(id))
            {
                Console.WriteLine("[数据库] CheckPassword fail , id not safe");
                return false;
            }

            if (!isSafeString(pw))
            {
                Console.WriteLine("[数据库] CheckPassword fail , pw not safe");
                return false;
            }
            //查询
            string sql = string.Format("select * from account where id  ='{0}' and pw ={1};", id, pw);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,MySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return  !hasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库] CheckPassword err," + e.Message );
                throw;
            }
        }

        //获取玩家数据
        public static PlayerData GetPlayerData(String id)
        {
            if (!isSafeString(id))
            {
                Console.WriteLine("[数据库] GetPlayerData fail , id not safe");
                return null;
            }

            string sql = string.Format("select * from player where id = '{0}';", id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,MySql);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    return null;
                }

                dataReader.Read();
                string data = dataReader.GetString("data");
                PlayerData playerData = Js.Deserialize<PlayerData>(data);
                dataReader.Close();
                return playerData;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库] GetPlayerData Fail err" + e.Message);
                return  null;
            }
        }

        public static bool UpdatePlayerData(string id, PlayerData playerData)
        {
            string data = Js.Serialize(playerData);
            string sql = string.Format("update player set data ='{0}' where id = '{1}';", data, id);
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,MySql);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[数据库] UpdatePlayerData err" + e.Message);
                return false;
            }
        }
    }
