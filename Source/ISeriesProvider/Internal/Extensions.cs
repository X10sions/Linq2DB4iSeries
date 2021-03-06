﻿using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using LinqToDB.SqlProvider;

namespace LinqToDB.DataProvider.DB2iSeries
{
	internal static class Extensions
	{
		public static string ToSqlString(this DbDataType dbDataType)
		{
			return DB2iSeriesSqlBuilder.GetDbType(dbDataType.DbType, dbDataType.Length, dbDataType.Precision, dbDataType.Scale);
		}

		public static SqlDataType GetTypeOrUnderlyingTypeDataType(this MappingSchema mappingSchema, Type type)
		{
			var sqlDataType = mappingSchema.GetDataType(type);
			if (sqlDataType.Type.DataType == DataType.Undefined)
				sqlDataType = mappingSchema.GetUnderlyingDataType(type, out var _);

			return sqlDataType.Type.DataType == DataType.Undefined ? SqlDataType.Undefined : sqlDataType;
		}

		public static bool IsGuidMappedAsString(this MappingSchema mappingSchema)
		{
			return mappingSchema is DB2iSeriesMappingSchemaBase iseriesMappingSchema
				&& iseriesMappingSchema.GuidMappedAsString;
		}

		public static DbDataType GetDbDataType(this MappingSchema mappingSchema, Type systemType, DataType dataType, int? length, int? precision, int? scale, bool mapGuidAsString, bool forceDefaultAttributes = false)
		{
			return DB2iSeriesDbTypes.GetDbDataType(systemType, dataType, length, precision, scale, mappingSchema.IsGuidMappedAsString(), forceDefaultAttributes);
		}

		public static DbDataType GetDbTypeForCast(this MappingSchema mappingSchema, SqlDataType type)
		{
			return DB2iSeriesDbTypes.GetDbTypeForCast(type, mappingSchema);
		}

		public static IDbConnection GetProviderConnection(this DataConnection dataConnection)
		{
			if (!(dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider))
				throw ExceptionHelper.InvalidProvider(dataConnection.DataProvider);

			var connection = iSeriesDataProvider.TryGetProviderConnection(dataConnection.Connection, dataConnection.MappingSchema);

			if (connection == null)
				throw ExceptionHelper.InvalidDbConnectionType(dataConnection.Connection);

			return connection;
		}

		public static BulkCopyType GetEffectiveType(this BulkCopyType bulkCopyType)
			=> bulkCopyType == BulkCopyType.Default ? DB2iSeriesTools.DefaultBulkCopyType : bulkCopyType;

		public static string GetQuotedLibList(this DataConnection dataConnection)
			=> "'" + string.Join("','", dataConnection.GetLibList()) + "'";

		public static IEnumerable<string> GetLibList(this DataConnection dataConnection)
		{
			IEnumerable<string> libraries = new string[] { };
			var connection = GetProviderConnection(dataConnection);

			if (dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider)
			{
				var libraryListKey = iSeriesDataProvider.ProviderType switch
				{
#if NETFRAMEWORK
					DB2iSeriesProviderType.AccessClient => "Library List",
#endif
					DB2iSeriesProviderType.Odbc => "DBQ",
					DB2iSeriesProviderType.OleDb => "Library List",
					DB2iSeriesProviderType.DB2 => "LibraryList",
					_ => throw ExceptionHelper.InvalidAdoProvider(iSeriesDataProvider.ProviderType)
				};

				var csb = new DbConnectionStringBuilder() { ConnectionString = dataConnection.ConnectionString };

				if (csb.TryGetValue(libraryListKey, out var libraryList))
				{
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

		public static string GetDelimiter(this DataConnection dataConnection)
			=> Constants.SQL.Delimiter(dataConnection.GetNamingConvetion());

		public static DB2iSeriesNamingConvention GetNamingConvetion(this DataConnection dataConnection)
		{
			if (dataConnection.DataProvider is DB2iSeriesDataProvider iSeriesDataProvider
				&& iSeriesDataProvider.ProviderType != DB2iSeriesProviderType.DB2)
			{
				var namingConventionKey = iSeriesDataProvider.ProviderType switch
				{
#if NETFRAMEWORK
					DB2iSeriesProviderType.AccessClient => "Naming",
#endif
					DB2iSeriesProviderType.Odbc => "NAM",
					DB2iSeriesProviderType.OleDb => "Naming Convention",
					_ => throw ExceptionHelper.InvalidAdoProvider(iSeriesDataProvider.ProviderType)
				};

				var csb = new DbConnectionStringBuilder() { ConnectionString = dataConnection.ConnectionString };

				if (csb.TryGetValue(namingConventionKey, out var namingConvention))
				{
					if (!(namingConvention is string namingConventionString))
						namingConventionString = ((int)namingConvention).ToString();

					return namingConventionString == "1" ? DB2iSeriesNamingConvention.System : DB2iSeriesNamingConvention.Sql;
				}
			}

			return DB2iSeriesNamingConvention.Sql;
		}

		public static void SetFlag(this SqlProviderFlags sqlProviderFlags, string flag, bool isSet)
		{
			if (isSet && !sqlProviderFlags.CustomFlags.Contains(flag))
				sqlProviderFlags.CustomFlags.Add(flag);

			if (!isSet && sqlProviderFlags.CustomFlags.Contains(flag))
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
			||	providerType == DB2iSeriesProviderType.OleDb;
	}
}
