using System.Linq;

namespace LinqToDB.DataProvider.DB2iSeries {
	using SqlProvider;

	public class DB2iSeriesSqlProviderFlags {
		public DB2iSeriesSqlProviderFlags(
			DB2iSeriesVersion db2iVersion,
			//bool supportsOffsetClause,
			//bool supportsTruncateTable,
			bool supportsNamedParameters,
			bool mapGuidAsString)
		//bool supportsMergeStatement,
		//bool supportsNCharTypes)
		{
			version = db2iVersion;
			MapGuidAsString = mapGuidAsString;
			//SupportsOffsetClause = supportsOffsetClause;
			//SupportsTruncateTable = supportsTruncateTable;
			SupportsNamedParameters = supportsNamedParameters;
			//SupportsMergeStatement = supportsMergeStatement;
			//SupportsNCharTypes = supportsNCharTypes;

		}

		public DB2iSeriesSqlProviderFlags(DB2iSeriesVersion db2iVersion, SqlProviderFlags sqlProviderFlags)
			: this(
					db2iVersion,
					sqlProviderFlags.CustomFlags.Contains(Constants.ProviderFlags.SupportsNamedParameters),
					sqlProviderFlags.CustomFlags.Contains(Constants.ProviderFlags.MapGuidAsString)
				 ) {
		}

		public DB2iSeriesSqlProviderFlags(DB2iSeriesProviderOptions options)
			: this(
					options.DB2iSeriesVersion,
					options.ProviderType.IsIBM(),
					options.MapGuidAsString
					//options.SupportsOffsetClause,
					//options.SupportsTruncateTable,
					//supportsNamedParameters: options.ProviderType.IsIBM(),
					//supportsMergeStatement: options.SupportsMergeStatement,
					//supportsNCharTypes: options.SupportsNCharTypes
					) {

		}

		public DB2iSeriesSqlProviderFlags(
			DB2iSeriesVersion version,
			DB2iSeriesProviderType providerType,
			bool mapGuidAsString)
			: this(
					version,
				 //supportsOffsetClause: version >= DB2iSeriesVersion.V7_3,
				 //supportsTruncateTable: version >= DB2iSeriesVersion.V7_2,
				 providerType.IsIBM(),
				 //supportsMergeStatement: version >= DB2iSeriesVersion.V7_1,
				 //supportsNCharTypes: version >= DB2iSeriesVersion.V7_1
				 mapGuidAsString
				 ) {

		}

		DB2iSeriesVersion version;

		public bool MapGuidAsString { get; }
		public bool SupportsDecFloatTypes => version.SupportsDecFloatTypes();
		public bool SupportsMergeStatement => version.SupportsMergeStatement();
		public bool SupportsNamedParameters { get; }
		public bool SupportsNCharTypes => version.SupportsNCharTypes();
		public bool SupportsOffsetClause => version.SupportsOffsetClause();
		public bool SupportsTruncateTable => version.SupportsTruncateTable();

		public void SetCustomFlags(SqlProviderFlags sqlProviderFlags) {
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.MapGuidAsString, MapGuidAsString);
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.SupportsOffsetClause, SupportsOffsetClause);
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.SupportsTruncateTable, SupportsTruncateTable);
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.SupportsNamedParameters, SupportsNamedParameters);
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.SupportsMergeStatement, SupportsMergeStatement);
			sqlProviderFlags.SetFlag(Constants.ProviderFlags.SupportsNCharTypes, SupportsNCharTypes);
		}
	}
}
