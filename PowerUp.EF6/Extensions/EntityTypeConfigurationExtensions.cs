using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerUp.EF6
{
    public static class EntityTypeConfigurationExtensions
    {
        public static EntityTypeConfiguration<T> ConfigurePkFor<T, TKey>(this EntityTypeConfiguration<T> self, Expression<Func<T, TKey>> keyFieldSelector)
            where T : class where TKey : struct
        {
            self.HasKey(keyFieldSelector)
                .Property(keyFieldSelector)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkRequiredFor<TEntity, TForeignEntity, TKey>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            Expression<Func<TForeignEntity, TKey>> keySelector,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
            where TKey : struct
        {
            self.HasRequired(field)
                .WithMany()
                .Map(fkConfig => fkConfig.MapKey(keySelector.GetExpressionTargetMemberName()))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkRequiredFor<TEntity, TForeignEntity>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            string mapKeyName,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
        {
            self.HasRequired(field)
                .WithMany()
                .Map(fkConfig => fkConfig.MapKey(mapKeyName))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkRequiredWithNavigationFor<TEntity, TForeignEntity, TKey>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            Expression<Func<TForeignEntity, ICollection<TEntity>>> dependencyNavigation,
            Expression<Func<TForeignEntity, TKey>> keySelector,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
        {
            self.HasRequired(field)
                .WithMany(dependencyNavigation)
                .Map(fkConfig => fkConfig.MapKey(keySelector.GetExpressionTargetMemberName()))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkRequiredWithNavigationFor<TEntity, TForeignEntity>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            Expression<Func<TForeignEntity, ICollection<TEntity>>> dependencyNavigation,
            string mapKeyName,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
        {
            self.HasRequired(field)
                .WithMany(dependencyNavigation)
                .Map(fkConfig => fkConfig.MapKey(mapKeyName))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkOptionalFor<TEntity, TForeignEntity, TKey>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            Expression<Func<TForeignEntity, TKey>> keySelector,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
            where TKey : struct
        {

            self.HasOptional(field)
                .WithMany()
                .Map(fkConfig => fkConfig.MapKey(keySelector.GetExpressionTargetMemberName()))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkOptionalFor<TEntity, TForeignEntity>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> field,
            string mapKeyName,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
        {
            self.HasOptional(field)
                .WithMany()
                .Map(fkConfig => fkConfig.MapKey(mapKeyName))
                .WillCascadeOnDelete(cascadeOnDelete);

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureFkOptionalWithNavigationFor<TEntity, TForeignEntity, TKey>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> foreignPropertySelector,
            Expression<Func<TForeignEntity, ICollection<TEntity>>> dependencyNavigation,
            Expression<Func<TForeignEntity, TKey>> keySelector,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TForeignEntity : class
            where TKey : struct
        {
            self.HasOptional(foreignPropertySelector)
                .WithMany(dependencyNavigation)
                .Map(fkConfig => fkConfig.MapKey(keySelector.GetExpressionTargetMemberName()));

            return self;
        }

        public static EntityTypeConfiguration<TEntity> CreateUniqueIndexFor<TEntity>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, string>> field,
            int maxLength = 100)
            where TEntity : class
        {
            var order = self.GetType().GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy).Count(x => x.GetCustomAttribute<IndexAttribute>() != null);
            var indexName = String.Format("IX_Unique_{0}_{1}", typeof(TEntity).Name, ((MemberExpression)field.Body).Member.Name);
            var indexAnnotation = new IndexAnnotation(new IndexAttribute(indexName) { IsUnique = true, Order = order });
            self.Property(field).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation).HasMaxLength(maxLength);
            return self;
        }

        public static EntityTypeConfiguration<TEntity> CreateCompositeUniqueIndex<TEntity, TColumn1, TColumn2>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TColumn1>> column1Selector,
            Expression<Func<TEntity, TColumn2>> column2Selector)
            where TEntity : class
            where TColumn1 : struct
            where TColumn2 : struct
        {
            Func<Expression, string> nameFrom = c => ((MemberExpression)((LambdaExpression)c).Body).Member.Name;
            var indexName = String.Format("IX_Unique_{0}_{1}_{2}", typeof(TEntity).Name, nameFrom(column1Selector), nameFrom(column2Selector));


            Func<int, IndexAnnotation> indexAnnotation =
                order => new IndexAnnotation(new IndexAttribute(indexName) { IsUnique = true, Order = order });

            self.Property(column1Selector).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation(1));
            self.Property(column2Selector).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation(2));

            return self;
        }

        public static EntityTypeConfiguration<TEntity> ConfigureOneToOneRelationship<TEntity, TForeignEntity>(this EntityTypeConfiguration<TEntity> self,
            Expression<Func<TEntity, TForeignEntity>> foreignEntitySelector,
            Expression<Func<TForeignEntity, TEntity>> entityPropertySelector)
            where TEntity : class
            where TForeignEntity : class
        {
            self.HasOptional(foreignEntitySelector).WithRequired(entityPropertySelector);
            return self;
        }

        public static EntityTypeConfiguration<TLeftEntity> ConfigureManyToManyRelationship<TLeftEntity, TRightEntity, TLeftEntityKey, TRightEntityKey>(this EntityTypeConfiguration<TLeftEntity> self,
            Expression<Func<TLeftEntity, ICollection<TRightEntity>>> rightCollectionSelector,
            Expression<Func<TRightEntity, ICollection<TLeftEntity>>> leftCollectionSelector,
            Expression<Func<TLeftEntity, TLeftEntityKey>> leftKeySelector,
            Expression<Func<TRightEntity, TRightEntityKey>> rightKeySelector,
            string relationshipTableName = null)
            where TLeftEntity : class
            where TRightEntity : class
        {
            relationshipTableName = relationshipTableName ?? string.Format("{0}{1}", typeof(TLeftEntity).Name, typeof(TRightEntity).Name);
            self
                .HasMany(rightCollectionSelector)
                .WithMany(leftCollectionSelector)
                .Map(relationshipConfig =>
                    {
                        relationshipConfig.MapLeftKey(leftKeySelector.GetExpressionTargetMemberName());
                        relationshipConfig.MapRightKey(rightKeySelector.GetExpressionTargetMemberName());
                        relationshipConfig.ToTable(relationshipTableName);
                    });

            return self;
        }

        public static void AreRequired<TEntity, TProperty>(this EntityTypeConfiguration<TEntity> self,
            params Expression<Func<TEntity, TProperty>>[] properties)
            where TEntity : class
            where TProperty : struct
        {
            foreach(var property in properties)
            {
                self.Property(property).IsRequired();
            }
        }

        public static void AreRequiredString<TEntity>(this EntityTypeConfiguration<TEntity> self, 
            params Expression<Func<TEntity, string>>[] properties)
            where TEntity : class
        {

            
            foreach(var property in properties)
            {
                self.Property(property).IsRequired();
            }
        }
    }
}