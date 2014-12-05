using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2;
using N2.Details;
using N2.Integrity;

namespace ZeroWaste.SharePortal.Models.Parts
{
    [PartDefinition("Accordion Part", IconUrl = "{IconsUrl}/text_columns.png")]
    [RestrictChildren(typeof(AccordionItem))]

    public class AccordionPart : PartModelBase
    {

        public override string TemplateKey
        {
            get { return "AccordionItems"; }
        }

        [EditableChildren("AccordionItems", "AccordionItems", 102)]
        public List<AccordionItem> AccordionItems { get; set; }


        [PartDefinition("Accordion Item")]
        [RestrictParents(typeof(AccordionPart))]
        [AllowedZones("AccordionItems")]
        public class AccordionItem : ContentItem
        {
            [EditableText(Title = "Title", Required = true, ValidationMessage = "Title can not be empty.", SortOrder = 101)]
            public virtual string ItemTitle { get; set; }

            [EditableFreeTextArea(SortOrder = 201, ContainerName = Defaults.Containers.Content)]
            [DisplayableTokens]
            public virtual string Text { get; set; }
        }

    }
}