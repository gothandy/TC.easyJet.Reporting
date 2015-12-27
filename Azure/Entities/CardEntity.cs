using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace Vincente.Azure.Entities
{
    public class CardEntity : TableEntity
    {

        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
    }
}
