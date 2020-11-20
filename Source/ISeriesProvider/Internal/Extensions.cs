using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace LinqToDB.DataProvider.DB2iSeries {
	public static class Extensions {
		public static string ToSqlString(this DbDataType dbDataType) => DB2iSeriesSqlBuilder.GetDbType(dbDataType.DbType, dbDataType.Length, dbDataType.Precision, dbDataType.Scale);

		public static SqlDataType GetTypeOrUnderlyingTypeDataType(this MappingSchema mappingSchema, Type type) {
			var sqlDataType = mappingSchema.GetDataType(type);
			if(sqlDataType.Type.DataType == DataType.Undefined)
				sqlDataType = mappingSchema.GetUnderlyingDataType(type, out var _);

			return sqlDataType.Type.DataType == DataType.Undefined ? SqlDataType.Undefined : sqlDataType;
		}

		public static bool IsGuidMappedAsString(this MappingSchema mappingSchema) => mappingSchema is DB2iSeriesMappingSchemaBase iseriesMappingSchema
				&& iseriesMappingSchema.GuidMappedAsString;

		public static DB2iSeriesVersion GetDB2iSeriesVersion(this Version version) => version switch {
			//var x when x >= new Version(7, 4) => DB2iSeriesVersion.V7_4,
			var x when x >= new Version(7, 3) => DB2iSeriesVersion.V7_3,
			var x when x >= new Version(7, 2) => DB2iSeriesVersion.V7_2,
			var x when x >= new Version(7, 1) => DB2iSeriesVersion.V7_1,
			//var x when x >= new Version(6, 1) => DB2iSeriesVersion.V6_1,
			var x when x >= new Version(5, 4) => DB2iSeriesVersion.V5_4,
			_ => DB2iSeriesVersion.V7_1
		};

		//public static Version AsVersion(string serverVersion) {
		//	var serverVersionParts = serverVersion.Split('.');
		//	var major = int.Parse(serverVersionParts[0]);
		//	var minor = int.Parse(serverVersionParts[1]);
		//	var build = int.Parse(serverVersionParts[2]);
		//	return new Version(major, minor, build);
		//}

		public static Version GetVersion(this IDbConnection connection) {
			if(connection is DbConnection) {
				var dbConnection = connection as DbConnection;
			var doOpenclose = dbConnection.State != ConnectionState.Open;
			if(doOpenclose)
				dbConnection.Open();
			//var version = AsVersion(dbConnection.ServerVersion);
			var version = new Version(dbConnection.ServerVersion.Split(' ')[0]);
			if(doOpenclose)
				dbConnection.Close();
			return version;
			}
			return new Version();
		}

		public static Version GetVersion(this IDataProvider dataProvider, string connectionString) {
			using(var conn = (DbConnection)dataProvider.CreateConnection(connectionString)) {
				conn.Open();
				return conn.GetVersion();
			}
		}

		public static Version GetVersion(this DataConnection dataConnection) => ((DbConnection)dataConnection.Connection).GetVersion();


		public static DbDataType GetDbDataType(this MappingSchema mappingSchema, Type systemType, DataType dataType, int? length, int? precision, int? scale, bool forceDefaultAttributes, DB2iSeriesVersion db2iVersion) => DB2iSeriesDbTypes.GetDbDataType(systemType, dataType, length, precision, scale, mappingSchema.IsGuidMappedAsString(), forceDefaultAttributes, db2iVersion);

		public static DbDataType GetDbTypeForCast(this MappingSchema mappingSchema, SqlDataType type, DB2iSeriesVersion db2iVersion) => DB2iSeriesDbTypes.GetDbTypeForCast(type, mappingSchema, db2iVersion);

		public static string GetDelimiter(this DataConnection dataConnection)
			=> dataConnection.GetNamingConvention().GetDelimiter();

		public static DB2iSeriesNamingConvention GetNamingConvention(this DataConnection dataConnection) {
				return new DbConnectionStringBuilder() { ConnectionString = dataConnection.ConnectionString }.GetNamingConvention();
		}

		public static DB2iSeriesNamingConvention GetNamingConvention(this DbConnectionStringBuilder csb) {
			foreach(var key in new[] {
				"NAM",
//#if NETFRAMEWORK
				"Naming",
//#endif
				"Naming Convention" }) {
				var value = csb[key].ToString().ToLower();
				switch( value ){
					case "1":
					case "system":
						return DB2iSeriesNamingConvention.System;
				}
			}
			return DB2iSeriesNamingConvention.Sql;
		}

		public static IDbConnection GetProviderConnection(this DataConnection dataConnection) {
			if(!(dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider))
				throw ExceptionHelper.InvalidProvider(dataConnection.DataProvider);

			var connection = iSeriesDataProvider.TryGetProviderConnection(dataConnection.Connection, dataConnection.MappingSchema);

			if(connection == null)
				throw ExceptionHelper.InvalidDbConnectionType(dataConnection.Connection);

			return connection;
		}

		public static BulkCopyType GetEffectiveType(this BulkCopyType bulkCopyType)
			=> bulkCopyType == BulkCopyType.Default ? DB2iSeriesTools.DefaultBulkCopyType : bulkCopyType;

		public static string GetQuotedLibList(this DataConnection dataConnection)
			=> "'" + string.Join("','", dataConnection.GetLibList()) + "'";

		public static IEnumerable<string> GetLibList(this DataConnection dataConnection) {
			IEnumerable<string> libraries = new string[] { };
			var connection = GetProviderConnection(dataConnection);

			if(dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider) {
				var libraryListKey = iSeriesDataProvider.ProviderOptions.ProviderType switch {
#if NETFRAMEWORK
					DB2iSeriesProviderType.AccessClient => "Library List",
#endif
					DB2iSeriesProviderType.Odbc => "DBQ",
					DB2iSeriesProviderType.OleDb => "Library List",
					DB2iSeriesProviderType.DB2 => "LibraryList",
					_ => throw ExceptionHelper.InvalidAdoProvider(iSeriesDataProvider.ProviderOptions.ProviderType)
				};

				var csb = new DbConnectionStringBuilder() { ConnectionString = dataConnection.ConnectionString };

				if(csb.TryGetValue(libraryListKey, out var libraryList)) {
					return libraryList
						.ToString()
						.Split(',', ' ')
						.Select(x => x.Trim())
						.Where(x => x != string.Empty)
						.ToList();
				}
			}

			return Enumerable.Empty<string>();
		}

		public static void SetFlag(this SqlProviderFlags sqlProviderFlags, string flag, bool isSet) {
			if(isSet && !sqlProviderFlags.CustomFlags.Contains(flag))
				sqlProviderFlags.CustomFlags.Add(flag);

			if(!isSet && sqlProviderFlags.CustomFlags.Contains(flag))
				sqlProviderFlags.CustomFlags.Remove(flag);
		}

		public static bool IsIBM(this DB2iSeriesProviderType providerType)
			=> providerType == DB2iSeriesProviderType.DB2
#if NETFRAMEWORK
			|| providerType == DB2iSeriesProviderType.AccessClient
#endif
			;

		public static bool IsDB2(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.DB2;

		public static bool IsAccessClient(this DB2iSeriesProviderType providerType)
			 =>
#if NETFRAMEWORK
			providerType == DB2iSeriesProviderType.AccessClient;
#else
			false;
#endif

		public static bool IsOdbc(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.Odbc;

		public static bool IsOleDb(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.OleDb;

		public static bool IsOdbcOrOleDb(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.Odbc
			|| providerType == DB2iSeriesProviderType.OleDb;
	}
}
