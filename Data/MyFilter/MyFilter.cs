using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

public class MyFilter
{
    private readonly Context _context;

    public MyFilter(Context context)
    {
        _context = context;
    }

    // Generic method for filter
    public IQueryable<T> PerformOperation<T>(T model) where T : class
    {
        var query = _context.Set<T>().AsQueryable();
        var properties = model.GetType().GetProperties();
        query = query.OrderByDescending(x => EF.Property<object>(x, "Id"));


        // If all properties are null, return all data
        if (properties.All(x => x.GetValue(model, null) == null))
        {
            return query;
        }

        foreach (var property in properties)
        {
            var value = property.GetValue(model, null);
            if (value != null)
            {
                
                 if (IsComplexType(property.PropertyType))
                {
                    // Handle nested properties
                    query = ApplyNestedPropertyFilter(query, value, property);
                }
                else
                {
                    // Handle regular properties
                    if (property.Name == "CreateDate" || property.Name == "UpdateDate")
                    {
                        // For CreateDate and UpdateDate, filter by days
                        query = query.Where(BuildDateFilterExpression<T>(property, value));
                    }
                    else
                    {
                        // For other properties, use the existing logic
                        query = query.Where(BuildPropertyExpression<T>(property, value));
                    }
                }
            }
        }

        return query;
    }

    private bool IsComplexType(Type type)
    {
        // Check if the type is a complex type or a reference type
        return type.IsClass && type != typeof(string) && !type.IsPrimitive;
    }

    private IQueryable<T> ApplyNestedPropertyFilter<T>(IQueryable<T> query, object nestedModel, PropertyInfo property) where T : class
    {
        var nestedProperties = property.PropertyType.GetProperties();

        foreach (var nestedProperty in nestedProperties)
        {
            var nestedValue = nestedProperty.GetValue(nestedModel, null);
            if (nestedValue != null)
            {
                query = query.Where(BuildNestedPropertyExpression<T>(property, nestedProperty, nestedValue));
            }
        }

        return query;
    }

   private Expression<Func<T, bool>> BuildPropertyExpression<T>(PropertyInfo property, object value)
{
    var parameter = Expression.Parameter(typeof(T));
    var propertyExpression = Expression.Property(parameter, property);

    // Check if the property is nullable
    if (property.PropertyType.IsGenericType &&
        property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
    {
        // If it's nullable, use the coalesce operator to provide a default value
        var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
        var valueExpression = Expression.Property(propertyExpression, "Value");

        // Create an expression for comparison
        var equalExpression = Expression.Equal(valueExpression, Expression.Constant(value));

        // Combine with a check for HasValue
        var finalExpression = Expression.AndAlso(hasValueExpression, equalExpression);

        return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
    }
    else
    {
        // For non-nullable types, use the standard Equal expression
        var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
        return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
    }
}

private Expression<Func<T, bool>> BuildNestedPropertyExpression<T>(PropertyInfo parentProperty, PropertyInfo nestedProperty, object value)
{
    var parameter = Expression.Parameter(typeof(T));
    var parentPropertyExpression = Expression.Property(parameter, parentProperty);
    var nestedPropertyExpression = Expression.Property(parentPropertyExpression, nestedProperty);

    // Check if the nested property is nullable
    if (nestedProperty.PropertyType.IsGenericType &&
        nestedProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
    {
        // If it's nullable, use the coalesce operator to provide a default value
        var hasValueExpression = Expression.Property(nestedPropertyExpression, "HasValue");
        var valueExpression = Expression.Property(nestedPropertyExpression, "Value");

        // Create an expression for comparison
        var equalExpression = Expression.Equal(valueExpression, Expression.Constant(value));

        // Combine with a check for HasValue
        var finalExpression = Expression.AndAlso(hasValueExpression, equalExpression);

        return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
    }
    else
    {
        // For non-nullable types, use the standard Equal expression
        var equalExpression = Expression.Equal(nestedPropertyExpression, Expression.Constant(value));
        return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
    }
}

   private Expression<Func<T, bool>> BuildDateFilterExpression<T>(PropertyInfo property, object value)
{
    var parameter = Expression.Parameter(typeof(T));
    var propertyExpression = Expression.Property(parameter, property);

    // If it's nullable, use the Value property to access the underlying non-nullable DateTime
    var valueExpression = property.PropertyType.IsGenericType &&
        property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
        ? Expression.Property(propertyExpression, "Value")
        : propertyExpression;

    // Extract only the date part for comparison
    var datePartExpression = Expression.Property(valueExpression, "Date");

    // Extract only the date part of the provided value
    var providedDateValue = ((DateTime)value).Date;
    var providedDateConstant = Expression.Constant(providedDateValue);

    // Create an expression for comparison
    var equalExpression = Expression.Equal(datePartExpression, providedDateConstant);

    return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
}

}