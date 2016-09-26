using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    public class PowerUpEntityConfiguration<T> where T : class
    {
        public PowerUpEntityConfiguration<T> WithPrimaryKeyOn<TKey>(Expression<Func<T, TKey>> property)
            where TKey : struct
        {
            _sourceConfiguration.ConfigurePkFor(property);
            return this;
        }

        public PowerUpEntityConfiguration<T> RequiredRelatedTo<TForeign, TKey>(
            Expression<Func<T, TForeign>> targetEntity,
            Expression<Func<TForeign, TKey>> targetKey,
            Expression<Func<TForeign, ICollection<T>>> fromSourceToTargetNavigation = null)
                where TForeign : class
                where TKey : struct
        {
            if(fromSourceToTargetNavigation == null)
            {
                _sourceConfiguration.ConfigureFkRequiredFor(targetEntity, targetKey);
            }
            else
            {
                _sourceConfiguration.ConfigureFkRequiredWithNavigationFor(targetEntity, fromSourceToTargetNavigation, targetKey);
            }

            return this;
        }

        public PowerUpEntityConfiguration<T> OptionallyRelatedTo<TForeign, TKey>(
            Expression<Func<T, TForeign>> targetEntity,
            Expression<Func<TForeign, TKey>> targetKey,
            Expression<Func<TForeign, ICollection<T>>> fromSourceToTargetNavigation = null)
                where TForeign : class
                where TKey : struct
        {
            if(fromSourceToTargetNavigation == null)
            {
                _sourceConfiguration.ConfigureFkOptionalFor(targetEntity, targetKey);
            }
            else
            {
                _sourceConfiguration.ConfigureFkOptionalWithNavigationFor(targetEntity, fromSourceToTargetNavigation, targetKey);
            }

            return this;
        }

        public PowerUpEntityConfiguration<T> WithUniqueIndexOn(Expression<Func<T, string>> property, int maxLength = 100)
        {
            _sourceConfiguration.CreateUniqueIndexFor(property, maxLength);
            return this;
        }

        public PowerUpEntityConfiguration<T> WithCompositeUniqueIndexOn<TColumn1, TColumn2>(
            Expression<Func<T, TColumn1>> property1,
            Expression<Func<T, TColumn2>> property2)
            where TColumn1 : struct
            where TColumn2 : struct
        {
            _sourceConfiguration.CreateCompositeUniqueIndex(property1, property2);
            return this;
        }

        public PowerUpEntityConfiguration<T> AddOneToOneRelationship<TForeign>(
            Expression<Func<T, TForeign>> target,
            Expression<Func<TForeign, T>> returnToSource)
            where TForeign : class
        {
            _sourceConfiguration.ConfigureOneToOneRelationship(target, returnToSource);
            return this;
        }

        public PowerUpEntityConfiguration<T> AddManyToManyRelationship<TRightEntity, TLeftEntityKey, TRightEntityKey>(
            Expression<Func<T, ICollection<TRightEntity>>> rightCollection,
            Expression<Func<TRightEntity, ICollection<T>>> leftCollection,
            Expression<Func<T, TLeftEntityKey>> leftKey,
            Expression<Func<TRightEntity, TRightEntityKey>> rightKey,
            string relationshipName = null)
                where TRightEntity : class
        {
            _sourceConfiguration.ConfigureManyToManyRelationship(rightCollection, leftCollection, leftKey, rightKey, relationshipName);
            return this;
        }

        public PowerUpEntityConfiguration<T> WithRequiredProperties<TProperty>(params Expression<Func<T, TProperty>>[] properties)
            where TProperty : struct
        {
            _sourceConfiguration.AreRequired(properties);
            return this;
        }

        public PowerUpEntityConfiguration<T> WithRequiredStrings<TProperty>(params Expression<Func<T, String>>[] properties)
        {
            _sourceConfiguration.AreRequiredString(properties);
            return this;
        }

        public PowerUpEntityConfiguration(EntityTypeConfiguration<T> sourceConfiguration)
        {
            _sourceConfiguration = sourceConfiguration;
        }

        private readonly EntityTypeConfiguration<T> _sourceConfiguration;
    }
}
