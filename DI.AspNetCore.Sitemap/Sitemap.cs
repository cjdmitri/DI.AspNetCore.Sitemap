using System;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;


/*
<urlset>	обязательный	 Инкапсулирует этот файл и указывает стандарт текущего протокола.
<url>	обязательный	 Родительский тег для каждой записи URL-адреса. Остальные теги являются дочерними для этого тега.
<loc>	обязательный	 URL-адрес страницы. Этот URL-адрес должен начинаться с префикса (например, HTTP) и заканчиваться косой чертой, если Ваш веб-сервер требует этого. Длина этого значения не должна превышать 2048 символов.
<lastmod>	необязательно	 Дата последнего изменения файла. Эта дата должна быть в формате W3C Datetime. Этот формат позволяет при необходимости опустить сегмент времени и использовать формат ГГГГ-ММ-ДД.
Обратите внимание, что этот тег не имеет отношения к заголовку "If-Modified-Since (304)", который может вернуть сервер, поэтому поисковые системы могут по-разному использовать информацию из этих двух источников.
<changefreq>	необязательно	 Вероятная частота изменения этой страницы. Это значение предоставляет общую информацию для поисковых систем и может не соответствовать точно частоте сканирования этой страницы. Допустимые значения:
always
hourly
daily
weekly
monthly
yearly
never
Значение"всегда" должно использоваться для описания документов, которые изменяются при каждом доступе к этим документам. Значение "никогда" должно использоваться для описания архивных URL-адресов.
Имейте в виду, что значение для этого тега рассматривается как подсказка, а не как команда. Несмотря на то, что сканеры поисковой системы учитывают эту информацию при принятии решений, они могут сканировать страницы с пометкой "ежечасно" менее часто, чем указано, а страницы с пометкой "ежегодно" – более часто, чем указано. Сканеры могут периодически сканировать страницы с пометкой "никогда", чтобы отслеживать неожиданные изменения на этих страницах.
<priority>	необязательно	 Приоритетность URL относительно других URL на Вашем сайте. Допустимый диапазон значений — от 0,0 до 1,0. Это значение не влияет на процедуру сравнения Ваших страниц со страницами на других сайтах — оно только позволяет указать поисковым системам, какие страницы, по Вашему мнению, более важны для сканеров.
Приоритет страницы по умолчанию — 0,5.
Следует учитывать, что приоритет, который Вы назначили странице, не влияет на положение Ваших URL на страницах результатов той или иной поисковой системы. Поисковые системы используют эту информацию при обработке URL, которые относятся к одному и тому же сайту, поэтому можно использовать этот тег для увеличения вероятности присутствия в поисковом индексе Ваших самых важных страниц.
Кроме того, следует учитывать, что назначение высокого приоритета всем URL на Вашем сайте не имеет смысла. Поскольку приоритетность – величина относительная, этот параметр используется для того, чтобы определить очередность обработки URL в пределах сайта.
 
https://www.sitemaps.org/ru/protocol.html

 */
namespace DI.AspNetCore.Sitemap
{
    /// <summary>
    /// Функции и методы для управления картой сайта CRUD
    /// </summary>
    public static class Sitemap
    {
        public enum ChangeFrequency
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }

        /// <summary>
        /// Создает файл карты сайта
        /// </summary>
        /// <param name="pathFile">Путь к файлу карты сайта</param>
        /// <returns>Успешное выполнение</returns>
        public static bool CreateSitemap(string pathFile)
        {
            try
            {
                //Проверяем наличие файла карты сайта,
                //если карта сайта найдена, то прекращаем работу
                if (File.Exists(pathFile))
                    return false;

                //Если файл не найден, то продолжаем работу
                XNamespace NS = "http://www.sitemaps.org/schemas/sitemap/0.9";
                XDocument xdoc = new XDocument();
                XElement root = new XElement(NS + "urlset");

                xdoc.Add(new XComment("Sitemap lib ASP .Net Core. DI.AspNetCore.Sitemap. Email:cjdmitri@gmail.com"));
                xdoc.Add(root);
                xdoc.Save(pathFile);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании карты сайта");
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// Добавляет элемент в карту сайта
        /// </summary>
        /// <param name="pathFile">Путь к файлу карты сайта</param>
        /// <param name="url">Ссылка, которую необходимо добавить</param>
        /// <param name="dateModified">Дата модификации</param>
        /// <param name="change">Когда роботу нужно делать переобход</param>
        /// <param name="priority">Приоритет. От 0.1 до 1.0</param>
        /// <returns>Успешное выполнение</returns>
        public static bool AddItem(string pathFile, string url, DateTime dateModified, ChangeFrequency change, double priority)
        {
            try
            {
                //Проверяем наличие файла карты сайта,
                //если карта сайта не найдена, то создаем файл карты сайта
                if (!File.Exists(pathFile))
                    CreateSitemap(pathFile);

                XDocument doc = XDocument.Load(pathFile);
                XNamespace NS = doc.Root.GetDefaultNamespace();

                //Карты сайта имеют ограниечение на 50000 записей
                //проверяе количество записей
                //если число записей достигло предела, возвращаем false
                if (doc.Root.Nodes().Count() > 49998)
                {
                    Console.WriteLine("Превышено количество записей для одной карты сайта");
                    return false;
                }

                XElement u = new XElement(NS + "url");
                XElement loc = new XElement(NS + "loc", url);

                u.Add(loc);
                u.Add(new XElement(NS + "lastmod", dateModified.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
                u.Add(new XElement(NS + "priority", priority.ToString("N1", CultureInfo.InvariantCulture)));
                u.Add(new XElement(NS + "changefreq", change.ToString().ToLower()));

                doc.Root.Add(u);
                doc.Save(pathFile);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении элемента в карту сайта");
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// Добавляет элемент в карту сайта
        /// </summary>
        /// <param name="pathFile">Путь к карте сайта</param>
        /// <param name="item">Запись карты сайта</param>
        /// <returns></returns>
        public static bool AddItem(string pathFile, SitemapItem item)
        {
            return AddItem(pathFile, item.Url, item.DateModified, item.Change, item.Priority);
        }

        /// <summary>
        /// Пакетное добавление элементов в карту сайта
        /// </summary>
        /// <param name="pathFile">Путь к карте сайта</param>
        /// <param name="items">Список элементов</param>
        /// <returns></returns>
        public static bool AddItems(string pathFile, List<SitemapItem> items)
        {
            try
            {
                //Проверяем наличие файла карты сайта,
                //если карта сайта не найдена, то создаем файл карты сайта
                if (!File.Exists(pathFile))
                    CreateSitemap(pathFile);

                XDocument doc = XDocument.Load(pathFile);
                XNamespace NS = doc.Root.GetDefaultNamespace();

                //Карты сайта имеют ограниечение на 50000 записей
                //проверяе количество записей
                //если число записей достигло предела, возвращаем false
                if (doc.Root.Nodes().Count() > 49998)
                {
                    Console.WriteLine("Превышено количество записей для одной карты сайта");
                    return false;
                }

                //Проход по всем записям
                //
                foreach(SitemapItem item in items)
                {
                    XElement u = new XElement(NS + "url");
                    XElement loc = new XElement(NS + "loc", item.Url);

                    u.Add(loc);
                    u.Add(new XElement(NS + "lastmod", item.DateModified.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
                    u.Add(new XElement(NS + "priority", item.Priority.ToString("N1", CultureInfo.InvariantCulture)));
                    u.Add(new XElement(NS + "changefreq", item.Change.ToString().ToLower()));

                    doc.Root.Add(u);
                }
                //Сохраняем карту сайта
                doc.Save(pathFile);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении элемента в карту сайта");
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// Обновление элемента карты сайта
        /// </summary>
        /// <param name="pathFile">Путь к файлу карты сайта</param>
        /// <param name="url">Ссылка, которую необходимо добавить</param>
        /// <param name="dateModified">Дата модификации</param>
        /// <param name="change">Когда роботу нужно делать переобход</param>
        /// <param name="priority">Приоритет. От 0.1 до 1.0</param>
        /// <returns>Успешное выполнение</returns>
        /// <returns></returns>
        public static bool UpdateItem(string pathFile, string url, DateTime dateModified, ChangeFrequency change, double priority)
        {
            try
            {
                XDocument doc = XDocument.Load(pathFile);
                XNamespace NS = doc.Root.GetDefaultNamespace();
                XElement root = doc.Element(NS + "urlset");
                bool modified = false;


                //Проход по всем url-элементам
                foreach (XElement el in root.Elements(NS + "url"))
                {
                    if (el.Element(NS + "loc") != null)
                    {
                        //Поиск совпадений дочернего элемента loc по url
                        //Если совпадение найдено, то вносим изменения
                        if (el.Element(NS + "loc").Value == url)
                        {
                            //Проверяем поле lastmod
                            if (el.Element(NS + "lastmod") != null)
                                el.Element(NS + "lastmod").Value = dateModified.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00";
                            else
                            {
                                XElement lastMod = new XElement(NS + "lastmod", dateModified.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00");
                                el.Add(lastMod);
                            }
                            //Проверяем поле priority
                            if (el.Element(NS + "priority") != null)
                                el.Element(NS + "priority").Value =
                                    priority.ToString("N1", CultureInfo.InvariantCulture);
                            else
                            {
                                XElement pr = new XElement(NS + "priority", priority.ToString("N1", CultureInfo.InvariantCulture));
                                el.Add(pr);
                            }
                            //Проверяем поле changefreq
                            if (el.Element(NS + "changefreq") != null)
                                el.Element(NS + "changefreq").Value = change.ToString().ToLower();
                            else
                            {
                                XElement ch = new XElement(NS + "changefreq", change.ToString().ToLower());
                                el.Add(ch);
                            }
                            //Сохраняем изменения в файл и указываем, что изменения внесены
                            doc.Save(pathFile);
                            modified = true;
                            return true;
                        }
                    }
                }
                //Если изменения не внесены, значит не было найдено совпадение по url
                //Значит добавляем новый элемент в карту сайта
                if (!modified)
                {
                    return AddItem(pathFile, url, dateModified, change, priority);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Удаление элемента карты сайта
        /// </summary>
        /// <param name="pathFile">Путь к файлу карты сайта</param>
        /// <param name="url">Ссылка, которую необходимо удалить</param>
        /// <returns></returns>
        public static bool RemoveItem(string pathFile, string url)
        {
            try
            {
                XDocument doc = XDocument.Load(pathFile);
                XNamespace NS = doc.Root.GetDefaultNamespace();
                XElement root = doc.Element(NS + "urlset");

                //Проход по всем url-элементам
                foreach (XElement el in root.Elements(NS + "url"))
                {
                    if (el.Element(NS + "loc") != null)
                    {
                        //Поиск совпадений дочернего элемента loc по url
                        //Если совпадение найдено, то вносим изменения
                        if (el.Element(NS + "loc").Value == url)
                        {
                            el.Remove();
                            //Сохраняем изменения в файл
                            doc.Save(pathFile);
                            return true;
                        }
                    }
                }
                Console.WriteLine("Элемент с данным url не найден");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }
        }


    }
}
