﻿using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Data;

using NUnit.Framework;

namespace Tests.xUpdate
{
	using Model;

	public partial class MergeTests
	{
		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleManaged, ProviderName.OracleNative,
			ProviderName.Sybase, ProviderName.SapHana, ProviderName.Firebird, ProviderName.Firebird)]
		public void SameSourceDelete(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource1(db))
					.OnTargetKey()
					.DeleteWhenMatched()
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(2, rows, context);

				Assert.AreEqual(2, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void SameSourceDeleteWithPredicate(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource1(db))
					.OnTargetKey()
					.DeleteWhenMatchedAnd((t, s) => s.Id == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void DeletePartialSourceProjection_KnownFieldInCondition(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource1(db).Select(s => new TestMapping1() {  Id = s.Id }))
					.OnTargetKey()
					.DeleteWhenMatchedAnd((t, s) => s.Id == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void DeleteWithPredicatePartialSourceProjection_UnknownFieldInCondition(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var exception = Assert.Catch(
					() => table
					.Merge()
					.Using(GetSource1(db).Select(x => new TestMapping1() { Id = x.Id, Field1 = x.Field1 }))
					.OnTargetKey()
					.DeleteWhenMatchedAnd((t, s) => s.Field2 == 4)
					.Merge());

				Assert.IsInstanceOf<LinqToDBException>(exception);
				Assert.AreEqual("Column Field2 doesn't exist in source", exception.Message);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.SqlServer2008, ProviderName.SqlServer2012, ProviderName.SqlServer2014,
			TestProvName.SqlAzure, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void SameSourceDeleteWithPredicateDelete(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource1(db))
					.OnTargetKey()
					.DeleteWhenMatchedAnd((t, s) => s.Id == 4)
					.DeleteWhenMatched()
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(2, rows, context);

				Assert.AreEqual(2, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleManaged, ProviderName.OracleNative,
			ProviderName.Sybase, ProviderName.SapHana, ProviderName.Firebird)]
		public void OtherSourceDelete(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db))
					.On((t, s) => s.OtherId == t.Id && t.Id == 3)
					.DeleteWhenMatched()
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[3], result[2], null, null);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleManaged, ProviderName.OracleNative,
			ProviderName.Sybase, ProviderName.SapHana, ProviderName.Firebird)]
		public void OtherSourceDeletePartialSourceProjection_UnknownFieldInMatch(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var exception = Assert.Catch(
					() => table
					.Merge()
					.Using(GetSource1(db).Select(x => new TestMapping1() { Id = x.Id, Field1 = x.Field1 }))
					.On((t, s) => s.Field2 == 3)
					.DeleteWhenMatched()
					.Merge());

				Assert.IsInstanceOf<LinqToDBException>(exception);
				Assert.AreEqual("Column Field2 doesn't exist in source", exception.Message);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void OtherSourceDeleteWithPredicate(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db))
					.On((t, s) => s.OtherId == t.Id)
					.DeleteWhenMatchedAnd((t, s) => t.Id == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void AnonymousSourceDeleteWithPredicate(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db).Select(x => new
					{
						Key = x.OtherId,
						Field01 = x.OtherField1,
						Field02 = x.OtherField2,
						Field03 = x.OtherField3,
						Field04 = x.OtherField4,
						Field05 = x.OtherField5,
					}))
					.On((t, s) => s.Key == t.Id)
					.DeleteWhenMatchedAnd((t, s) => s.Key == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		// Oracle: implicit Delete to UpdateWithDelete conversion failed here
		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleManaged, ProviderName.OracleNative,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void AnonymousListSourceDeleteWithPredicate(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db).ToList().Select(x => new
					{
						Key = x.OtherId,
						Field01 = x.OtherField1,
						Field02 = x.OtherField2,
						Field03 = x.OtherField3,
						Field04 = x.OtherField4,
						Field05 = x.OtherField5,
					}))
					.On((t, s) => s.Key == t.Id)
					.DeleteWhenMatchedAnd((t, s) => s.Key == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleManaged, ProviderName.OracleNative,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void DeleteReservedAndCaseNames(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db).Select(x => new
					{
						select = x.OtherId,
						Field = x.OtherField1,
						field = x.OtherField2,
						insert = x.OtherField3,
						order = x.OtherField4,
						by = x.OtherField5
					}))
					.On((t, s) => s.select == t.Id)
					.DeleteWhenMatchedAnd((t, s) => s.select == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged,
			ProviderName.Sybase, ProviderName.Informix, ProviderName.SapHana, ProviderName.Firebird)]
		public void DeleteReservedAndCaseNamesFromList(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var rows = table
					.Merge()
					.Using(GetSource2(db).ToList().Select(x => new
					{
						update = x.OtherId,
						Update = x.OtherField1,
						UPDATE = x.OtherField2,
						uPDATE = x.OtherField3,
						UpDaTe = x.OtherField4,
						upDATE = x.OtherField5
					}))
					.On((t, s) => s.update == t.Id)
					.DeleteWhenMatchedAnd((t, s) => s.update == 4)
					.Merge();

				var result = table.OrderBy(x => x.Id).ToList();

				AssertRowCount(1, rows, context);

				Assert.AreEqual(3, result.Count);

				AssertRow(InitialTargetData[0], result[0], null, null);
				AssertRow(InitialTargetData[1], result[1], null, null);
				AssertRow(InitialTargetData[2], result[2], null, 203);
			}
		}

		[Test, MergeDataContextSource(ProviderName.Oracle, ProviderName.OracleNative, ProviderName.OracleManaged, ProviderName.Sybase)]
		public void DeleteFromPartialSourceProjection_MissingKeyField(string context)
		{
			using (var db = new TestDataConnection(context))
			{
				PrepareData(db);

				var table = GetTarget(db);

				var exception = Assert.Catch(
					() => table
						.Merge()
						.Using(table.Select(x => new TestMapping1() { Field1 = x.Field1 }))
						.OnTargetKey()
						.DeleteWhenMatched()
						.Merge());

				Assert.IsInstanceOf<LinqToDBException>(exception);
				Assert.AreEqual("Column Id doesn't exist in source", exception.Message);
			}
		}
	}
}
