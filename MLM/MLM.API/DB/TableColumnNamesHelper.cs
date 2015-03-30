using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace MLM.API.DB
{
    public class TableColumnNamesHelper
    {
        public static string GetTableName(Type type)
        {
            return "tbl" + type.Name;
        }

        public static string GetColumnNamePrependedByEntityName(PropertyInfo propertyInfo)
        {
            return propertyInfo.ReflectedType.Name + propertyInfo.Name;
        }

        public static string GetColumnNameFromProperty<T>(Type t, Expression<Func<T>> memberExpression)
        {
            var property = GetMemberName(memberExpression);
            if (property != "Id")
                return property;
            else
            {
                PropertyInfo propertyInfo = t.GetProperty(property);
                return GetColumnNamePrependedByEntityName(propertyInfo);
            }
        }

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}
