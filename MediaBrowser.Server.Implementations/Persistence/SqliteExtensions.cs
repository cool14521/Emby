﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;

namespace MediaBrowser.Server.Implementations.Persistence
{
    /// <summary>
    /// Class SQLiteExtensions
    /// </summary>
    public static class SqliteExtensions
    {
        /// <summary>
        /// Connects to db.
        /// </summary>
        public static async Task<IDbConnection> ConnectToDb(string dbPath, bool isReadOnly, bool enablePooling, int? cacheSize, ILogger logger)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException("dbPath");
            }

            logger.Info("Sqlite {0} opening {1}", SQLiteConnection.SQLiteVersion, dbPath);

            var connectionstr = new SQLiteConnectionStringBuilder
            {
                PageSize = 4096,
                CacheSize = cacheSize ?? 2000,
                SyncMode = SynchronizationModes.Normal,
                DataSource = dbPath,
                JournalMode = SQLiteJournalModeEnum.Wal,
                Pooling = enablePooling,
                ReadOnly = isReadOnly
            };

            var connection = new SQLiteConnection(connectionstr.ConnectionString);

            await connection.OpenAsync().ConfigureAwait(false);

            return connection;
        }
    }
}