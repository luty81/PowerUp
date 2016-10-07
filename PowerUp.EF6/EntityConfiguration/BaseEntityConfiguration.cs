using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public class BaseEntityConfiguration<T>: EntityTypeConfiguration<T> where T : class
	{
		protected void PrimaryKey<TProperty>(Expression<Func<T, TProperty>> property) where TProperty : struct
		{
			this.ConfigurePkFor(property);
		}

		protected void UniqueIndex(Expression<Func<T, string>> property, int maxLength = 100)
		{
			this.CreateUniqueIndexFor(property, maxLength);
		}

		protected void UniqueIndex<TColumn1, TColumn2>(
			Expression<Func<T, TColumn1>> property1,
			Expression<Func<T, TColumn2>> property2)
			where TColumn1 : struct
			where TColumn2 : struct
		{
			this.CreateCompositeUniqueIndex(property1, property2);
		}

		protected void ForeignKey<TForeign, TKey>(EntityRelationship<T, TForeign, TKey> FK)
			where TForeign : class
			where TKey : struct
		{
			FK.Configure(this);
		}

		protected void ForeignKey<TForeign>(
			Expression<Func<T, TForeign>> target,
			Expression<Func<TForeign, T>> returnToSource)
			where TForeign : class
		{
			this.ConfigureOneToOneRelationship(target, returnToSource);
		}

		protected void ForeignKey<TRightEntity, TLeftEntityKey, TRightEntityKey>(
			EntityRelationshipManyToMany<T, TRightEntity, TLeftEntityKey, TRightEntityKey> FK)
			where TRightEntity : class
			where TLeftEntityKey : struct
			where TRightEntityKey : struct
		{
			this
				.ConfigureManyToManyRelationship(
					FK.RightCollection, FK.LeftCollection, FK.LeftKey, FK.RightKey, FK.RelationshipName);
		}

		protected void RequiredColumns<TProperty>(params Expression<Func<T, TProperty>>[] properties)
			where TProperty : struct
		{
			properties.ToList().ForEach(p => Property(p).IsRequired());
		}

		protected void RequiredTextColumns(params Expression<Func<T, string>>[] properties)
		{
			properties.ToList().ForEach(p => Property(p).IsRequired());
		}

		protected void NotEmptyTextColumns(params Expression<Func<T, string>> [] properties)
		{
			Action<Expression<Func<T, string>>> ShouldNotBeEmpty =
				p => Property(p).IsRequired().HasColumnAnnotation("MinLengthAttribute", 1);

			properties.ToList().ForEach(ShouldNotBeEmpty);
		}
	}
}
