﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using ZeroWaste.SharePortal.Models;
using N2;
using N2.Collections;
using N2.Web.Mvc.Html;

namespace ZeroWaste.SharePortal.Extensions
{
    public static class HtmlHelper
    {
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            // If the enum is nullable, add an 'empty' item to the collection
            if (metadata.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString NewsArchiveActionLink<TModel>(this HtmlHelper<TModel> helper, DateTime date)
        {
            return NewsArchiveActionLink(helper, date, "MMMM yyyy", "yyyyMM", null);
        }

        public static MvcHtmlString NewsArchiveActionLink<TModel>(this HtmlHelper<TModel> helper, DateTime date, string dataFormat)
        {
            return NewsArchiveActionLink(helper, date, "MMMM yyyy", dataFormat, null);
        }

        public static MvcHtmlString NewsArchiveActionLink<TModel>(this HtmlHelper<TModel> helper, DateTime date, string displayFormat, string dataFormat, object htmlAttributes)
        {
            return NewsArchiveActionLink(helper, date.ToString(displayFormat), date, dataFormat, htmlAttributes);
        }

        public static MvcHtmlString NewsArchiveActionLink<TModel>(this HtmlHelper<TModel> helper, string linkText, DateTime date, string dataFormat, object htmlAttributes)
        {
            var newsPage = helper.CurrentItem().Parent as NewsContainer;

            //ContentItem item = N2.Find.Items.Where.Type.Eq(typeof(NewsContainer))
            //    .Filters(new PublishedFilter(),
            //        new AncestorFilter(newsPage),
            //        new AccessFilter())
            //    .Select().FirstOrDefault();

            string url = "#";
            if (newsPage != null)
            {
                if (dataFormat.Contains("M"))
                    url = string.Format("{0}/archive/{1:yyyy}" + "/{1:MM}", newsPage.Url, date);
                else
                    url = string.Format("{0}/archive/{1:yyyy}", newsPage.Url, date);
            }

            //string url = item == null ? "#" : string.Format("{0}?date={1:" + dataFormat + "}", item.Url, date);
            var link = new TagBuilder("a");
            link.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            link.Attributes.Add("href", url);
            link.SetInnerText(linkText);
            return MvcHtmlString.Create(link.ToString(TagRenderMode.Normal));
        }
    }
}