using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BeautySalonApp.Security
{
    public static class PasswordCache
    {
        private static readonly Dictionary<string, string> _passwordCache = new Dictionary<string, string>();
        private static readonly object _lockObject = new object();
        private static readonly string cacheFile = "password_cache.dat";

        static PasswordCache()
        {
            LoadCacheFromFile();
        }

        public static void CachePassword(string username, string password)
        {
            lock (_lockObject)
            {
                _passwordCache[username] = password;
                SaveCacheToFile();
                System.Diagnostics.Debug.WriteLine($"Кэширован пароль для: {username}");
            }
        }

        public static string GetCachedPassword(string username)
        {
            lock (_lockObject)
            {
                if (_passwordCache.ContainsKey(username))
                {
                    System.Diagnostics.Debug.WriteLine($"Пароль найден в кэше для: {username}");
                    return _passwordCache[username];
                }
                System.Diagnostics.Debug.WriteLine($"Пароль НЕ найден в кэше для: {username}");
                return null;
            }
        }

        public static void RemoveCachedPassword(string username)
        {
            lock (_lockObject)
            {
                if (_passwordCache.ContainsKey(username))
                {
                    _passwordCache.Remove(username);
                    SaveCacheToFile();
                    System.Diagnostics.Debug.WriteLine($"Пароль удален из кэша для: {username}");
                }
            }
        }

        public static void ClearCache()
        {
            lock (_lockObject)
            {
                _passwordCache.Clear();
                SaveCacheToFile();
                System.Diagnostics.Debug.WriteLine("Кэш полностью очищен");
            }
        }

        public static bool IsPasswordCached(string username)
        {
            lock (_lockObject)
            {
                return _passwordCache.ContainsKey(username);
            }
        }

        public static int GetCachedUsersCount()
        {
            lock (_lockObject)
            {
                return _passwordCache.Count;
            }
        }

        public static List<string> GetCachedUsernames()
        {
            lock (_lockObject)
            {
                return new List<string>(_passwordCache.Keys);
            }
        }

        private static void SaveCacheToFile()
        {
            try
            {
                using (FileStream fs = new FileStream(cacheFile, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, _passwordCache);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения кэша: {ex.Message}");
            }
        }

        private static void LoadCacheFromFile()
        {
            try
            {
                if (File.Exists(cacheFile))
                {
                    using (FileStream fs = new FileStream(cacheFile, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        var loadedCache = (Dictionary<string, string>)formatter.Deserialize(fs);
                        _passwordCache.Clear();
                        foreach (var item in loadedCache)
                        {
                            _passwordCache[item.Key] = item.Value;
                        }
                        System.Diagnostics.Debug.WriteLine($"Загружен кэш из файла: {_passwordCache.Count} пользователей");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки кэша: {ex.Message}");
            }
        }
    }
}