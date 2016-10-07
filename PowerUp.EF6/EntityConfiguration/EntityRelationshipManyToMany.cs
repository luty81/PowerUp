using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6.EntityConfiguration
{
	public class EntityRelationshipManyToMany<TLeft, TRight, TLeftKey, TRightKey>: IEntityRelationship<TLeft>
		where TLeft : class
		where TRight : class
		where TLeftKey : struct
		where TRightKey : struct
	{
		public Expression<Func<TRight, ICollection<TLeft>>> LeftCollection { get; private set; }
		public Expression<Func<TLeft, ICollection<TRight>>> RightCollection { get; private set; }
		public Expression<Func<TLeft, TLeftKey>> LeftKey { get; private set; }
		public Expression<Func<TRight, TRightKey>> RightKey { get; private set; }
		public string RelationshipName { get; private set; }

		public EntityRelationshipManyToMany(
			Expression<Func<TLeft, ICollection<TRight>>> rightCollection,
			Expression<Func<TRight, ICollection<TLeft>>> leftCollection,
			Expression<Func<TLeft, TLeftKey>> leftKey,
			Expression<Func<TRight, TRightKey>> rightKey,
			string relationshipName = null)
		{
			RightCollection = rightCollection;
			LeftCollection = leftCollection;
			LeftKey = leftKey;
			RightKey = rightKey;
			RelationshipName = relationshipName;
		}

		public void Configure(EntityTypeConfiguration<TLeft> entityConfiguration)
		{
			throw new NotImplementedException();
		}
	}
}
