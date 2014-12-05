using System.Collections.Generic;
using N2;
using N2.Details;
using N2.Integrity;

namespace ZeroWaste.SharePortal.Models
{
    [PartDefinition("Columns", IconUrl = "{IconsUrl}/text_columns.png")]
    [RestrictChildren(typeof(Column))]
    public class Columns : PartModelBase
    {
        public override string TemplateKey
        {
            get { return "Columns"; }
        }

        [EditableChildren("Columns", "Columns", 1)]
        public List<Column> ColumnsList { get; set; }

        [PartDefinition("Column")]
        [RestrictParents(typeof(Columns))]
        [AllowedZones("Columns")]
        public class Column : ContentItem
        {
            [EditableText(Title = "Column width (x / 12ths)", DefaultValue = 3, Required = true, Validate = true, ValidationExpression = "^[1-9][0-2]*", ValidationMessage = "Please specify a valid number of columns (1-12).")]
            public virtual int NumberOfColumns { get; set; }

            [EditableText(Title = "Column Offset (x / 12ths)", DefaultValue = 0, Required = false, Validate = false, ValidationExpression = "^[1-9][0-2]*", ValidationMessage = "Column offset.")]
            public virtual int Offset { get; set; }
        }
    }
}