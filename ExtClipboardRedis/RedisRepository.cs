using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtClipboardRedis
{
    public class RedisRepository
    {
        private IDatabase db;
        private readonly ConnectionMultiplexer connection;

        public RedisRepository(string hostPort = "localhost:6379")
        {
            this.connection = ConnectionMultiplexer.Connect(hostPort);
            this.db = this.connection.GetDatabase();
        }

        /// <summary>
        /// Fetch a key's value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Set a key to a value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetString(string key, string value)
        {
            return db.StringSet(new RedisKey(key), new RedisValue(value));
        }

        /// <summary>
        /// Add a value to the left side of a list
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long LeftPush(string listName, string value)
        {
            return db.ListLeftPush(listName, value);
        }

        /// <summary>
        /// Add an array of values to the left side of a list
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long LeftPush(string listName, string[] values)
        {
            RedisValue[] redisValues = new RedisValue[values.Length];
            for(int i=0; i<values.Length; i++)
            {
                redisValues[i] = new RedisValue(values[i]);
            }
            return db.ListLeftPush(listName, redisValues);
        }

        /// <summary>
        /// Add a value to the right side of a list
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long RightPush(string listName, string value)
        {
            return db.ListRightPush(listName, value);
        }

        /// <summary>
        /// Add an array of values to the right side of a list
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RightPush(string listName, string[] values)
        {
            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = new RedisValue(values[i]);
            }
            return db.ListRightPush(listName, redisValues);
        }

        public RedisValue[] GetAllList(string key)
        {
            return db.ListRange(key, 0, -1);
        }
    }
}
