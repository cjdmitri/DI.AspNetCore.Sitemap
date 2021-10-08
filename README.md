<h1>DI.AspNetCore.Sitemap</h1>
<p>Библиотека предназначена для работы (CRUD) с <b>картой сайта Вашего web-проекта</b>.</p>
<h3>Версия 1.0.0</h3>
<h4>Возможности данной версии:</h4>
<ol>
<li><b>bool CreateSitemap(string pathFile)</b> - Создает файл карты сайта. Если файл существует, то возвращает false</li>
<li><b>bool AddItem(string pathFile, string url, DateTime dateModified, ChangeFrequency change, double priority)</b> - Добавить элемент в карту сайта</li>
<li><b>bool AddItems(string pathFile, List<SitemapItem> items)</b> - Пакетное добавление элементов в карту сайта</li>
<li><b>bool UpdateItem(string pathFile, string url, DateTime dateModified, ChangeFrequency change, double priority)</b> - Обновление элемента карты сайта. Поиск элемента осуществляется по Url. Если нет совпадений, то создаётся новый элемент</li>
<li><b>bool RemoveItem(string pathFile, string url)</b> - Удаление элемента карты сайта. Поиск элемента осуществляется по Url</li>
</ol>
