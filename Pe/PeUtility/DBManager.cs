﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/27
 * 時刻: 23:43
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PeUtility
{
	public enum FalseCondition
	{
		Command,
		Expression,
	}
	
	public class CommandExpression
	{
		public CommandExpression()
		{
			Condition = false;
			TrueCommand = string.Empty;
			FalseCondition = FalseCondition.Command;
			FalseCommand = string.Empty;
			FalseExpression = null;
		}
		
		
		public CommandExpression(bool condition, string trueCommand): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Command;
		}
		
		public CommandExpression(bool condition, string trueCommand, string falseCommand): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Command;
			FalseCommand = falseCommand;
		}
		
		public CommandExpression(bool condition, string trueCommand, CommandExpression commandExpression): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Expression;
			FalseExpression = commandExpression;
		}
		/// <summary>
		/// 条件
		/// </summary>
		public bool Condition { get; private set; }
		/// <summary>
		/// 条件が真の場合のコマンド
		/// </summary>
		public string TrueCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合にコマンドと式のどちらを使用するか
		/// </summary>
		public FalseCondition FalseCondition { get; private set; }
		/// <summary>
		/// 条件が偽の場合のコマンド
		/// </summary>
		public string FalseCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合の式
		/// </summary>
		public CommandExpression FalseExpression { get; private set; }
		
		public string ToCode()
		{
			if(Condition) {
				return TrueCommand;
			}
			
			if(FalseCondition == FalseCondition.Command) {
				// 文字列
				return FalseCommand;
			} else {
				Debug.Assert(FalseCondition == FalseCondition.Expression);
				// 式
				return FalseExpression.ToCode();
			}
		}
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class  | AttributeTargets.Property,
		AllowMultiple = true,
		Inherited = true
	)]
	public sealed class TargetNameAttribute: Attribute
	{
		public TargetNameAttribute(string name)
		{
			TargetName = name;
		}
		
		public string TargetName { get; private set; }
	}
	
	public sealed class EntitySet
	{
		public EntitySet(string tableName, IDictionary<string, string> columnPropName)
		{
			TableName = tableName;
			ColumnPropertyMap = columnPropName;
		}
		
		public string TableName { get ; private set; }
		public IDictionary<string, string> ColumnPropertyMap { get; private set; }
	}
	
	public abstract class DbData
	{ }
	
	/// <summary>
	/// テーブル行に対応
	/// TargetNameAttribute
	/// </summary>
	public abstract class Entity: DbData
	{
	}
	
	/// <summary>
	/// データ取得単位に対応
	/// </summary>
	public abstract class Dto: DbData
	{
		public Dto()
		{ }
	}

	/// <summary>
	/// DB接続・操作の一元化
	/// 
	/// シングルスレッドで使用しないとトランザクションあたりが爆発する。しゃあない。
	/// </summary>
	public abstract class DBManager
	{
		public DBManager(DbConnection connection, bool isOpened)
		{
			Parameter = new Dictionary<string, object>();
			Expression = new Dictionary<string, CommandExpression>();
			
			Connection = connection;
			
			ConditionPattern = @"\[\[\w+\]\]";
			
			if(!isOpened) {
				Connection.Open();
			}
		}
		public DbConnection Connection { get; private set; }
		public DbTransaction BeginTransaction()
		{
			var tran = Connection.BeginTransaction();
			
			return tran;
		}
		
		public string ConditionPattern { get; set; }
		
		public Dictionary<string, object> Parameter { get; private set; }
		public Dictionary<string, CommandExpression> Expression { get; private set; }
		
		public virtual CommandExpression CreateExpresstion()
		{
			return new CommandExpression();
		}
		
		public void Clear()
		{
			Parameter.Clear();
			Expression.Clear();
		}
		
		public DbCommand CreateCommand()
		{
			return Connection.CreateCommand();
		}
		
		protected virtual object To(object value, Type toType)
		{
			return value;
		}
		public T To<T>(object value)
		{
			return (T)(To(value, typeof(T)));
		}
		
		/// <summary>
		/// DBに合わせてデータ調整
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual object DbValueFromValue(object value, Type type)
		{
			return value;
		}
		
		/// <summary>
		/// DBに合わせて型調整
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected virtual DbType DbTypeFromType(Type type)
		{
			var map = new Dictionary<Type, DbType>() {
				{ typeof(byte), DbType.Byte },
				{ typeof(sbyte), DbType.SByte },
				{ typeof(short), DbType.Int16 },
				{ typeof(ushort), DbType.UInt16 },
				{ typeof(int), DbType.Int32 },
				{ typeof(uint), DbType.UInt32 },
				{ typeof(long), DbType.Int64 },
				{ typeof(ulong), DbType.UInt64 },
				{ typeof(float), DbType.Single },
				{ typeof(double), DbType.Double },
				{ typeof(decimal), DbType.Decimal },
				{ typeof(bool), DbType.Boolean },
				{ typeof(string), DbType.String },
				{ typeof(char), DbType.StringFixedLength },
				{ typeof(Guid), DbType.Guid },
				{ typeof(DateTime), DbType.DateTime },
				{ typeof(DateTimeOffset), DbType.DateTimeOffset },
				{ typeof(byte[]), DbType.Binary },
				{ typeof(byte?), DbType.Byte },
				{ typeof(sbyte?), DbType.SByte },
				{ typeof(short?), DbType.Int16 },
				{ typeof(ushort?), DbType.UInt16 },
				{ typeof(int?), DbType.Int32 },
				{ typeof(uint?), DbType.UInt32 },
				{ typeof(long?), DbType.Int64 },
				{ typeof(ulong?), DbType.UInt64 },
				{ typeof(float?), DbType.Single },
				{ typeof(double?), DbType.Double },
				{ typeof(decimal?), DbType.Decimal },
				{ typeof(bool?), DbType.Boolean },
				{ typeof(char?), DbType.StringFixedLength },
				{ typeof(Guid?), DbType.Guid },
				{ typeof(DateTime?), DbType.DateTime },
			};
			
			if(map.ContainsKey(type)) {
				return map[type];
			}
			
			throw new ArgumentException(type.ToString());
		}
		
		protected virtual DbParameter MakeParameter(DbCommand command, string name, object value)
		{
			var param = command.CreateParameter();
			
			param.ParameterName = name;
			if(value != null) {
				var type = value.GetType();
				param.Value = DbValueFromValue(value, type);
				param.DbType = DbTypeFromType(type);
			}
			return param;
		}
		
		protected DbParameter[] MakeParameterList(DbCommand command)
		{
			var list = new List<DbParameter>(Parameter.Count);
			foreach(var pair in Parameter) {
				var param = MakeParameter(command, pair.Key, pair.Value);
				list.Add(param);
			}
			
			return list.ToArray();
		}
		
		protected bool SetParameter(DbCommand command)
		{
			if(Parameter.Count > 0) {
				var paramList = MakeParameterList(command);
				command.Parameters.Clear();
				command.Parameters.AddRange(paramList);
				command.Prepare();
				
				return true;
			}
			
			return false;
		}
		
		protected virtual string ExpressionReplace(string code)
		{
			if(Expression.Count == 0) {
				return code;
			}
			
			var pattern = ConditionPattern;
			var replacedCode = Regex.Replace(code, pattern, (Match m) => Expression[m.Groups[1].Value].ToCode());
			
			return replacedCode;
		}
		
		private T Executer<T>(Func<DbCommand,T> func, string code)
		{
			using(var command = CreateCommand()) {
				command.CommandText = ExpressionReplace(code);
				SetParameter(command);
				return func(command);
			}
		}
		
		public DbDataReader ExecuteReader(string code)
		{
			return Executer(command => command.ExecuteReader(), code);
		}
		
		public int ExecuteCommand(string code)
		{
			return Executer(command => command.ExecuteNonQuery(), code);
		}
		
		private IDictionary<string, string> GetTargetNamePropertyMap<T>()
			where T: DbData
		{
			var targetPropMap = new Dictionary<string, string>();
			foreach(var member in typeof(T).GetMembers()) {
				var tartgetNameAttribute = member.GetCustomAttribute(typeof(TargetNameAttribute)) as TargetNameAttribute;
				if(tartgetNameAttribute != null) {
					targetPropMap[tartgetNameAttribute.TargetName] = member.Name;
				}
			}
			
			return targetPropMap;
		}
		
		private IDictionary<string, PropertyInfo> GetPropertyMap<T>(ICollection<string> propertNameList)
		{
			var propMap = new Dictionary<string, PropertyInfo>(propertNameList.Count());
			foreach(var propertyName in propertNameList) {
				var prop = typeof(T).GetProperty(propertyName);
				propMap[propertyName] = prop;
			}
			return propMap;
		}
		
		private IEnumerable<T> GetDtoListImpl<T>(string code)
			where T: Dto, new()
		{
			var columnPropName = GetTargetNamePropertyMap<T>();
			var propMap = GetPropertyMap<T>(columnPropName.Values);
			using(var reader = ExecuteReader(code)) {
				while(reader.Read()) {
					var dto = new T();
					foreach(var columnPair in columnPropName) {
						var rawValue = reader[columnPair.Key];
						var property = propMap[columnPair.Value];
						var convedValue = To(rawValue, property.PropertyType);
						property.SetValue(dto, convedValue);
					}
					yield return dto;
				}
			}
		}
		
		
		public T GetDtoSingle<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code).Single();
		}
		
		public List<T> GetDtoList<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code).ToList();
		}
		
		private EntitySet GetEntitySet<T>()
			where T: Entity
		{
			var tableAttribute = (TargetNameAttribute)typeof(T).GetCustomAttribute(typeof(TargetNameAttribute));
			var tableName = tableAttribute.TargetName;
			var columnPropName = GetTargetNamePropertyMap<T>();
			
			return new EntitySet(tableName, columnPropName);
		}
		
		protected virtual string CreateInsertCommandCode(EntitySet entitySet)
		{
			var code = string.Format(
				"insert into {0} ({1}) values({2})",
				entitySet.TableName,
				string.Join(", ", entitySet.ColumnPropertyMap.Keys),
				string.Join(", ", entitySet.ColumnPropertyMap.Values.Select(s => ":" + s))
			);
			
			return code;
		}
		
		protected virtual string CreateUpdateCommandCode(EntitySet entitySet)
		{
			return null;
		}
		
		private void ExecuteEntityCommand<T>(IList<T> entityList, Func<EntitySet, string> func)
			where T: Entity
		{
			Parameter.Clear();
			
			var entitySet = GetEntitySet<T>();
			var code = func(entitySet);
			var propMap = GetPropertyMap<T>(entitySet.ColumnPropertyMap.Values);
			foreach(var entity in entityList) {
				foreach(var pair in propMap) {
					Parameter[pair.Key] = pair.Value.GetValue(entity);
				}
				ExecuteCommand(code);
			}
		}
		
		public void ExecuteInsert<T>(IList<T> entityList)
			where T: Entity
		{
			ExecuteEntityCommand(entityList, CreateInsertCommandCode);
		}
		
		public void ExecuteUpdate<T>(IList<T> entityList)
			where T: Entity
		{
			ExecuteEntityCommand(entityList, CreateUpdateCommandCode);
		}
		
		public void Close()
		{
			Connection.Close();
		}
	}
}
