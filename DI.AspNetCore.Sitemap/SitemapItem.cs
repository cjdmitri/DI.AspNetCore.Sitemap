using System;
using System.Collections.Generic;
using System.Text;
using static DI.AspNetCore.Sitemap.Sitemap;

namespace DI.AspNetCore.Sitemap
{
    /// <summary>
    /// Модель единичной записи карты сайта
    /// </summary>
   public class SitemapItem
    {
        /// <summary>
        /// Ссылка
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Дата последней модификации
        /// </summary>
        public DateTime DateModified { get; set; }
        /// <summary>
        /// Частота обхода
        /// </summary>
        public ChangeFrequency Change { get; set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public double Priority { get; set; }
    }
}
