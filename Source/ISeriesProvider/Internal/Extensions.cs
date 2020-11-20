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

		#region Version

		public static Version GetVersion(this IDbConnection connection) {
			if(connection is DbConnection) {
				var dbConnection = connection as DbConnection;
				var doOpenclose = dbConnection.State != ConnectionState.Open;
				if(doOpenclose)
					dbConnection.Open();
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

		#endregion

		public static DbDataType GetDbDataType(this MappingSchema mappingSchema, Type systemType, DataType dataType, int? length, int? precision, int? scale, bool forceDefaultAttributes, DB2iSeriesVersion db2iVersion) => DB2iSeriesDbTypes.GetDbDataType(systemType, dataType, length, precision, scale, mappingSchema.IsGuidMappedAsString(), forceDefaultAttributes, db2iVersion);

		public static DbDataType GetDbTypeForCast(this MappingSchema mappingSchema, SqlDataType type, DB2iSeriesVersion db2iVersion) => DB2iSeriesDbTypes.GetDbTypeForCast(type, mappingSchema, db2iVersion);

		//public static IDbConnection GetProviderConnection(this DataConnection dataConnection) {
		//	if(!(dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider))
		//		throw ExceptionHelper.InvalidProvider(dataConnection.DataProvider);

		//	var connection = iSeriesDataProvider.TryGetProviderConnection(dataConnection.Connection, dataConnection.MappingSchema);

		//	if(connection == null)
		//		throw ExceptionHelper.InvalidDbConnectionType(dataConnection.Connection);

		//	return connection;
		//}

		public static BulkCopyType GetEffectiveType(this BulkCopyType bulkCopyType)
			=> bulkCopyType == BulkCopyType.Default ? DB2iSeriesTools.DefaultBulkCopyType : bulkCopyType;

		public static string GetQuotedLibList(this DataConnection dataConnection)
			=> "'" + string.Join("','", dataConnection.GetLibList()) + "'";

		public static IEnumerable<string> GetLibraryList(this DbConnectionStringBuilder csb) {
			foreach(var key in new[] {
				"DBQ",
				"LibraryList",
				"Library List"
			}) {
				if(csb.TryGetValue(key, out var value)) {
					return value.ToString().Split(',', ' ').Select(x => x.Trim()).Where(x => x != string.Empty).ToList();
				}
			}
			return new string[] { };
		}

		public static IEnumerable<string> GetLibList(this DataConnection dataConnection) {
			var csb = new DbConnectionStringBuilder() { ConnectionString = dataConnection.ConnectionString };
			return csb.GetLibraryList();
		}

		public static void SetFlag(this SqlProviderFlags sqlProviderFlags, string flag, bool isSet) {
			if(isSet && !sqlProviderFlags.CustomFlags.Contains(flag))
				sqlProviderFlags.CustomFlags.Add(flag);

			if(!isSet && sqlProviderFlags.CustomFlags.Contains(flag))
				sqlProviderFlags.CustomFlags.Remove(flag);
		}

	}
}
