#nullable enable
using LinqToDB.Configuration;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace LinqToDB.DataProvider.DB2iSeries {
	public static class DB2iSeriesTools {
		#region DataProvider instances

		//private static readonly ConcurrentDictionary<string, DB2iSeriesDataProvider> dataProviders = new ConcurrentDictionary<string, DB2iSeriesDataProvider>();

		#endregion

		#region AutoDetection

		public static bool AutoDetectProvider { get; set; } = true;

		public static void RegisterProviderDetector() => DataConnection.AddProviderDetector(ProviderDetector);

		private static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString) {
			if(css.IsGlobal)
				return null;

			var providerName = css.ProviderName ?? string.Empty;
			switch(providerName) {
			}

			if(providerName.StartsWith(DB2iSeriesProviderName.DB2))
				return new DB2iSeriesDataProvider(new DB2iSeriesProviderOptions(providerName));

			if(css.Name.StartsWith(DB2iSeriesProviderName.DB2))
				return new DB2iSeriesDataProvider(css.Name);

			if(AutoDetectProvider) {
				try {
					var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;
					var csb = new DbConnectionStringBuilder() {
						ConnectionString = connectionString.ToUpper()
					};
					var providerOptions = new DB2iSeriesProviderOptions(null, csb.GetProviderType());
					providerOptions.NamingConvention = csb.GetNamingConvention();
					using(var conn = new DB2iSeriesDataProvider(providerOptions).CreateConnection(connectionString)) {
						conn.Open();
						providerOptions.DB2iSeriesVersion = conn.GetVersion().GetDB2iSeriesVersion();
					}
					return new DB2iSeriesDataProvider(providerOptions);
				} catch(Exception e) {
					throw ExceptionHelper.ConnectionStringParsingFailure(e);
				}

			}

			return null;
		}

		#endregion

		#region CreateDataConnection

		public static DataConnection CreateDataConnection(
			string connectionString,
			DB2iSeriesVersion version,
			DB2iSeriesProviderType providerType,
			bool mapGuidAsString) => new DataConnection(new DB2iSeriesDataProvider(new DB2iSeriesProviderOptions(null, providerType, version, mapGuidAsString)), connectionString);

		public static DataConnection CreateDataConnection(
			IDbConnection connection,
			DB2iSeriesVersion version,
			DB2iSeriesProviderType providerType,
			bool mapGuidAsString) => new DataConnection(new DB2iSeriesDataProvider(new DB2iSeriesProviderOptions(null, providerType, version, mapGuidAsString)), connection);

		public static DataConnection CreateDataConnection(
			IDbTransaction transaction,
			DB2iSeriesVersion version,
			DB2iSeriesProviderType providerType,
			bool mapGuidAsString) => new DataConnection(new DB2iSeriesDataProvider(new DB2iSeriesProviderOptions(null, providerType, version, mapGuidAsString)), transaction);

		#endregion

		#region BulkCopy

		public static BulkCopyType DefaultBulkCopyType = BulkCopyType.MultipleRows;

		[Obsolete("Please use the BulkCopy extension methods within DataConnectionExtensions")]
		public static BulkCopyRowsCopied MultipleRowsCopy<T>(DataConnection dataConnection,
			IEnumerable<T> source,
			int maxBatchSize = 1000,
			Action<BulkCopyRowsCopied> rowsCopiedCallback = null) where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
				BulkCopyType = BulkCopyType.MultipleRows,
				MaxBatchSize = maxBatchSize,
				RowsCopiedCallback = rowsCopiedCallback
			}, source);

		[Obsolete("Please use the BulkCopy extension methods within DataConnectionExtensions")]
		public static BulkCopyRowsCopied ProviderSpecificBulkCopy<T>(DataConnection dataConnection,
			IEnumerable<T> source,
			int bulkCopyTimeout = 0,
			bool keepIdentity = false,
			int notifyAfter = 0,
			Action<BulkCopyRowsCopied> rowsCopiedCallback = null) where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
				BulkCopyType = BulkCopyType.ProviderSpecific,
				BulkCopyTimeout = bulkCopyTimeout,
				KeepIdentity = keepIdentity,
				NotifyAfter = notifyAfter,
				RowsCopiedCallback = rowsCopiedCallback
			}, source);

		#endregion
	}
}
