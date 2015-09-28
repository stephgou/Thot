/// <summary>
/// Refactor friendly INotifyPropertyChanged using Lambda Expressions and Expression Trees to determine the property name
/// Whenever the name of the property is re-factored using a re-factor-tool, the delegate is re-factored too and the correct property name is used 
/// in the PropertyChanged event. 
/// <credits>This class was developed by Orktane http://www.orktane.com/Blog/Default.aspx and is included in nRoute Framework http://nroute.codeplex.com 
/// from an orginal idea from http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree </credits>
/// <summary>

using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;

namespace Doc.Search.App.Extensions
{
    public static class PropertyChangedExtension
    {
        private const string SELECTOR_MUSTBEPROP = "PropertySelector must select a property type.";

        public static void Notify<T>(this Action<string> notifier, Expression<Func<T>> propertySelector)
        {
            if (notifier != null)
                notifier(GetPropertyName(propertySelector));
        }

        public static void Notify<T>(this PropertyChangedEventHandler handler, Expression<Func<T>> propertySelector)
        {
            if (handler != null)
            {
                var memberExpression = GetMemberExpression(propertySelector);
                if (memberExpression != null)
                {
                    var _sender = ((ConstantExpression)memberExpression.Expression).Value;
                    handler(_sender, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
            //else we don't raise error for handler == null, because it is null when no has attached to the event..
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertySelector)
        {
            var _memberExpression = propertySelector.Body as MemberExpression;
            if (_memberExpression == null)
            {
                var _unaryExpression = propertySelector.Body as UnaryExpression;
                if (_unaryExpression != null) _memberExpression = _unaryExpression.Operand as MemberExpression;
            }
            if (_memberExpression != null)
            {
                // MemberType not yet in System.Linq.Expressions 4.0.10.0...
                //Guard.ArgumentValue((_memberExpression.Member.MemberType != MemberTypes.Property),
                //    "propertySelector", SELECTOR_MUSTBEPROP);
               return _memberExpression.Member.Name;
            }
            return null;
        }

        public static MemberExpression GetMemberExpression<T>(Expression<Func<T>> propertySelector)
        {
            var _memberExpression = propertySelector.Body as MemberExpression;
            if (_memberExpression != null)
            {
                // MemberType not yet in System.Linq.Expressions 4.0.10.0...
                //Guard.ArgumentValue((_memberExpression.Member.MemberType != MemberTypes.Property),
                //    "propertySelector", SELECTOR_MUSTBEPROP);
                return _memberExpression;
            }

            // all else
            return null;
        }
    }
}